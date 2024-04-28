using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebCore.MVC.ContentResult
{
    public class ExcelFileResult : FileContentResult
    {
        private const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelFileResult(string fileName, byte[] fileContents) : base(fileContents, ExcelContentType)
        {
            this.FileDownloadName = fileName;
        }
    }
}
