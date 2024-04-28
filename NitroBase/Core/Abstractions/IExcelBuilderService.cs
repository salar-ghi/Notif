using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Abstractions
{
    public interface IExcelBuilderService
    {
        void CreateExcelDocument(string excelFilePath = null);

        void AddSheet<T>(IEnumerable<T> dataCollection, string sheetName, Header header = null, Footer footer = null,
                         string excelFilePath = null, bool rightToLeft = true, int maxNoOfRecords = 50000, bool autoFitColumns = true);

        Task<byte[]> GetExcelDocument();

        void Dispose();

        Task<byte[]> ExportToExcel<T>(IEnumerable<T> dataCollection, string sheetName, Header header = null,
                                      Footer footer = null, string excelFilePath = null, bool rightToLeft = true,
                                      int maxNoOfRecords = 50000, bool autoFitColumns = true);
    }
}
