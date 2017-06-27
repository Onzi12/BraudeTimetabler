using System.Data;
using System.IO;
using Excel;

namespace Api
{
    public class ExcelFileReader
    {
        public static DataSet ReadFile(string path, string fileType)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader;

                if (fileType == ".xls")
                {
                    // Reading from a binary Excel file ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else /*if (fileType == ".xlsx")*/
                {
                    // Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }


                // DataSet - Create column names from first row
                excelReader.IsFirstRowAsColumnNames = true;
                DataSet result = excelReader.AsDataSet();

                // Free resources (IExcelDataReader is IDisposable)
                excelReader.Close();

                return result;
            }
        }
    }
}