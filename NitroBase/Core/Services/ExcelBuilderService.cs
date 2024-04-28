using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Abstractions;
using Core.Models;
using Core.Enums;
using Core.Extensions;

namespace Core.Services
{
    public class ExcelBuilderService : IExcelBuilderService, IDisposable
    {
        private ExcelPackage _excelPackage;
        const byte DefaultHeight = 20;
        const byte DefaultRowSpan = 1;

        public IReflectionService _reflectionService { get; set; }
        public IGuard _guard { get; set; }

        public ExcelBuilderService(IReflectionService reflectionService, IGuard guard)
        {
            _reflectionService = reflectionService;
            _guard = guard;
        }

        public void CreateExcelDocument(string excelFilePath = null)
        {
            if (!string.IsNullOrWhiteSpace(excelFilePath))
            {
                _guard.Assert(File.Exists(excelFilePath), ExceptionCodeEnum.InvalidInput, "excelFilePath نامعتبر است.");
                _excelPackage = new ExcelPackage(excelFilePath);
            }
            else
                _excelPackage = new ExcelPackage();
        }

        public async Task<byte[]> GetExcelDocument()
        {
            var document = await _excelPackage.GetAsByteArrayAsync();
            _excelPackage.Dispose();

            return document;
        }

        private Header BuildHeader<T>()
        {
            var header = new Header();
            var row = new HeaderRow();
            var dataModelType = typeof(T);
            var propertyNames = _reflectionService.GetMemberNames(dataModelType);

            for (byte j = 0; j < propertyNames.Count; j++)
            {
                var propertyName = propertyNames[j];
                var hasDescriptionAttr = _reflectionService.TryGetAttribute<DescriptionAttribute>(dataModelType, propertyName, out var descriptionAttr);
                row.Columns.Add(new HeaderColumn()
                {
                    Title = hasDescriptionAttr ? descriptionAttr.Description : propertyName,
                    RowSpan = DefaultRowSpan,
                });
            }

            header.Rows.Add(row);
            return header;
        }

        public void AddSheet<T>(IEnumerable<T> dataCollection, string sheetName, Header header = null, Footer footer = null,
                                string excelFilePath = null, bool rightToLeft = true, int maxNoOfRecords = 50000, bool autoFitColumns = true)
        {
            var isNewWorkSheet = false;
            _guard.Assert(_excelPackage is not null, ExceptionCodeEnum.InvalidInput, $"ابتدا باید متد {nameof(CreateExcelDocument)} فراخوانی شود.");
            var dataModelType = typeof(T);
            var propertyNames = _reflectionService.GetMemberNames(dataModelType);
            dataCollection = dataCollection.ToList();

            // Reference: https://support.microsoft.com/en-us/office/excel-specifications-and-limits-1672b34d-7043-467e-8e27-269d656771c3
            _guard.Assert(dataCollection.Count() < 1048576, ExceptionCodeEnum.InvalidOperation, "تعداد رکوردها نمی تواند بیش از 1.048.576 باشد.");
            _guard.Assert(propertyNames.Count < 16384, ExceptionCodeEnum.InvalidOperation, "تعداد ستون ها نمی تواند بیش از 16.384 باشد.");

            ExcelWorksheet workSheet = _excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name.Equals(sheetName, StringComparison.InvariantCultureIgnoreCase));
            if (workSheet is null)
            {
                workSheet = _excelPackage.Workbook.Worksheets.Add(sheetName);
                isNewWorkSheet = true;
            }

            var valuesFirstRowIndex = 0;
            var valuesRowIndex = 0;
            Dictionary<int, int> explicitWidths = new();
            header ??= BuildHeader<T>();
            footer ??= new Footer();

            if (isNewWorkSheet)
            {
                header.Rows.ForEach(row =>
                {
                    if (row.Height < 1)
                        row.Height = DefaultHeight;
                });

                for (var i = 0; i < header.Rows.Count; i++)
                {
                    var row = header.Rows[i];
                    for (var j = 0; j < row.Columns.Count; j++)
                    {
                        var col = row.Columns[j];
                        var fromRow = i;
                        var fromCol = j;

                        var matrix = (object[,])workSheet.Cells.Value;
                        //byte existingRowspan = 0;
                        byte existingColspan = 0;
                        if (matrix is not null)
                        {
                            //for (var x = 0; true; x++)
                            //{
                            //    if (matrix.HasValue(x, j))
                            //        existingRowspan++;
                            //    else
                            //        break;
                            //}
                            for (var y = 0; true; y++)
                            {
                                if (matrix.HasValue(i, y)) //existingRowspan
                                    existingColspan++;
                                else
                                    break;
                            }
                        }
                        //if (existingRowspan > 0)
                        //    fromRow = existingRowspan;
                        if (existingColspan > 0)
                            fromCol = existingColspan;

                        var toRow = fromRow + col.RowSpan;
                        var toCol = fromCol + col.ColSpan;
                        fromRow += 1;
                        fromCol += 1;

                        workSheet.Cells[fromRow, fromCol, toRow, toCol].Merge(col.Title, true, true);
                        workSheet.Cells[fromRow, fromCol].SetBackgroundColor(col.BackgroundColor.ToDrawing_Color());

                        if (col.Width > 0)
                            explicitWidths.Add(fromCol, col.Width);
                    }

                    workSheet.Cells[i + 1, 1].EntireRow.Height = row.Height;
                }
            }

            var headerRowsCount = ((object[,])workSheet.Cells.Value).GetLength(0);
            valuesFirstRowIndex = headerRowsCount + 1;
            valuesRowIndex = valuesFirstRowIndex;

            if (dataCollection.Count() > maxNoOfRecords)
                dataCollection = dataCollection.Take(maxNoOfRecords);

            foreach (var item in dataCollection)
            {

                for (var j = 0; j < propertyNames.Count; j++)
                {
                    var colIndex = j + 1;

                    var hasFormatAttr = _reflectionService.TryGetAttribute<DisplayFormatAttribute>(item.GetType(), propertyNames[j], out var displayFormat);

                    var value = _reflectionService.GetMemberValue(item, propertyNames[j]);

                    SetValue(workSheet, valuesRowIndex, colIndex, value, hasFormatAttr ? displayFormat.Format : null);
                }

                valuesRowIndex++;
            }

            foreach (var footerRow in footer.Rows)
            {
                foreach (var footerColumn in footerRow.Columns)
                {
                    SetValue(workSheet, valuesRowIndex, footerColumn.ColumnIndex, footerColumn.Value);

                    if (footerColumn.BackgroundColor != null)
                        workSheet.Cells[valuesRowIndex, footerColumn.ColumnIndex].SetBackgroundColor(footerColumn.BackgroundColor.ToDrawing_Color());

                    workSheet.Cells[valuesRowIndex, footerColumn.ColumnIndex].SetBorderAround();
                }

                valuesRowIndex++;
            }

            var allCells = workSheet.Cells[workSheet.Dimension.Address];
            allCells.SetAllBorderLines();
            allCells.SetFont();
            allCells.SetHorizontalAlignment();
            allCells.SetVerticalAlignment();

            if (autoFitColumns)
                allCells.AutoFitColumns();

            foreach (var explicitWidth in explicitWidths)
                workSheet.Columns[explicitWidth.Key].Width = explicitWidth.Value;

            workSheet.View.RightToLeft = rightToLeft;
        }

        public async Task<byte[]> ExportToExcel<T>(
            IEnumerable<T> dataCollection,
            string sheetName,
            Header header,
            Footer footer = null,
            string excelFilePath = null,
            bool rightToLeft = true,
            int maxNoOfRecords = 50000,
            bool autoFitColumns = true)
        {
            CreateExcelDocument(excelFilePath);
            AddSheet(dataCollection, sheetName, header, footer, excelFilePath, rightToLeft, maxNoOfRecords, autoFitColumns);
            var result = await GetExcelDocument();
            Dispose();

            return result;
        }

        private void SetValue(ExcelWorksheet workSheet, int rowId, int colId, object value, string format = null)
        {
            if (value is byte || value is int || value is long || value is decimal || value is float)
            {
                var decimalValue = Convert.ToDecimal(value);

                if (format == null)
                {
                    workSheet.Cells[rowId, colId].SetNumberValue(decimalValue, decimalValue < 0);
                }
                else
                {
                    workSheet.Cells[rowId, colId].Value = value;
                    workSheet.Cells[rowId, colId].Style.Numberformat.Format = format;
                }
            }
            else
            {
                workSheet.Cells[rowId, colId].Value = value;
            }
        }

        public void Dispose()
        {
            if (_excelPackage is not null)
                _excelPackage.Dispose();
        }
    }

}
