using System.Data;
using System.IO;
using OfficeOpenXml;

namespace EmployeeManagementSystem.Helpers
{
    public class ExcelHelper
    {
        public static byte[] GenerateExcelFile(DataTable dataTable, string sheetName)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(sheetName);
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);
                return package.GetAsByteArray();
            }
        }
    }
}
