using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Core.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Core.Extensions
{
    public static class ExcelBuilderServiceExtensions
    {

        private const string NumberFormat = "#,##0";
        private const string NegativeNumberFormat = "#,##0 ;(#,##0)";
        private const string ColoredNegativeNumberFormat = "#,##0 ;[Red](#,##0)";

        public static bool HasValue(this object[,] matrix, int rowId, int colId)
        {
            try
            {
                return matrix.GetValue(rowId, colId) != null;
            }
            catch
            {
                return false;
            }
        }

        public static System.Drawing.Color? ToDrawing_Color(this Color color)
        {
            if (color is null) return null;

            return System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue);
        }

        public static void Merge(this ExcelRange cells, object value, bool merge = false, bool setBorder = false, System.Drawing.Color? bgColor = null)
        {
            if (merge)
                cells.Merge = true;

            if (bgColor.HasValue)
            {
                cells.Style.Fill.BackgroundColor.SetColor(bgColor.Value);
            }

            if (setBorder)
            {
                cells.SetAllBorderLines();
            }

            cells.Value = value;
        }

        public static void SetNumberValue(this ExcelRange cells, decimal value, bool formatNegativeNumber = false)
        {
            //cells.Value = (long)value;
            cells.Value = value;
            cells.Style.Numberformat.Format = formatNegativeNumber ? ColoredNegativeNumberFormat : NumberFormat;
        }

        public static void SetAllBorderLines(this ExcelRange cells)
        {
            cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        public static void SetBorderAround(this ExcelRange cells)
        {
            cells.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }

        public static void SetHorizontalAlignment(this ExcelRange cells, ExcelHorizontalAlignment alignment = ExcelHorizontalAlignment.Center)
        {
            cells.Style.HorizontalAlignment = alignment;
        }

        public static void SetVerticalAlignment(this ExcelRange cells, ExcelVerticalAlignment alignment = ExcelVerticalAlignment.Center)
        {
            cells.Style.VerticalAlignment = alignment;
        }

        public static void SetFont(this ExcelRange cells, string fontName = "Tahoma")
        {
            cells.Style.Font.Name = fontName;
        }

        public static void SetBackgroundColor(this ExcelRange cells, System.Drawing.Color? color = null)
        {
            var colFromHex = color ?? System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cells.Style.Fill.BackgroundColor.SetColor(colFromHex);
        }

        public static void SetMainBackgroundColor(this ExcelRange cells, System.Drawing.Color? color = null)
        {
            var colFromHex = color ?? System.Drawing.ColorTranslator.FromHtml("#ececec");
            cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cells.Style.Fill.BackgroundColor.SetColor(colFromHex);
        }

        public static void SetTrueColumnWidth(this ExcelColumn column, double width)
        {
            var z = width >= 1 + 2 / 3
                ? Math.Round((Math.Round(7 * (width - 1 / 256), 0) - 5) / 7, 2)
                : Math.Round((Math.Round(12 * (width - 1 / 256), 0) - Math.Round(5 * width, 0)) / 12, 2);

            var errorAmt = width - z;

            var adj = width >= 1 + 2 / 3
                ? Math.Round(7 * errorAmt - 7 / 256, 0) / 7
                : Math.Round(12 * errorAmt - 12 / 256, 0) / 12 + 2 / 12;

            if (z > 0)
            {
                column.Width = width + adj;
                return;
            }

            column.Width = 0d;
        }

        //public static string ToStringMessage(this Exception error)
        //{
        //    var builder = new StringBuilder();
        //    var realError = error;
        //    builder.AppendLine(error.Message);
        //    while (realError.InnerException != null)
        //    {
        //        builder.AppendLine(realError.InnerException.Message);
        //        realError = realError.InnerException;
        //    }
        //    return builder.ToString();
        //}


    }
}
