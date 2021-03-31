using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QUDMMSAPI.Common;
// The following to two namespace contains
// the functions for manipulating the
// Excel file 
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace QUDMMSAPI.Common
{


    public class ExcelHelper
    {
        public static string DumpExcel(DataTable tbl)
        {
            //string[] tbl_j = tbl.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

            //JArray tbl_j = JArray.Parse(JsonConvert.SerializeObject(tbl));

            // Creating an instance
            // of ExcelPackage
            ExcelPackage excel = new ExcelPackage();

            // name of the sheet
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");

            // setting the properties
            // of the work sheet 
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            // Setting the properties
            // of the first row
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            // Header of the Excel sheet
            workSheet.Cells[1, 1].Value = "Assn Id";
            workSheet.Cells[1, 2].Value = "Course Code";
            workSheet.Cells[1, 3].Value = "Section Number";
            workSheet.Cells[1, 4].Value = "Course Title";
            workSheet.Cells[1, 5].Value = "Assn Year";
            workSheet.Cells[1, 6].Value = "Assn Term";
            workSheet.Cells[1, 7].Value = "Course Delivery";
            workSheet.Cells[1, 8].Value = "Instructor Id";
            workSheet.Cells[1, 9].Value = "Instructor Name";
            workSheet.Cells[1, 10].Value = "Notes";
            // Inserting the article data into excel
            // sheet by using the for each loop
            // As we have values to the first row 
            // we will start with second row
            int recordIndex = 2;

            //foreach (var record in tbl)
            //{
            //    workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
            //    workSheet.Cells[recordIndex, 2].Value = record.assn_id;
            //    workSheet.Cells[recordIndex, 3].Value = record.course_code;
            //    workSheet.Cells[recordIndex, 4].Value = record.section_number;

            //    recordIndex++;
            //}

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                workSheet.Cells[recordIndex, 1].Value = (recordIndex - 1).ToString();
                workSheet.Cells[recordIndex, 2].Value = Convert.ToString(tbl.Rows[i]["assn_id"]);
                workSheet.Cells[recordIndex, 3].Value = Convert.ToString(tbl.Rows[i]["course_code"]);
                workSheet.Cells[recordIndex, 4].Value = Convert.ToString(tbl.Rows[i]["section_number"]);
                workSheet.Cells[recordIndex, 5].Value = Convert.ToString(tbl.Rows[i]["course_title"]);
                workSheet.Cells[recordIndex, 6].Value = Convert.ToString(tbl.Rows[i]["assn_year"]);
                workSheet.Cells[recordIndex, 7].Value = Convert.ToString(tbl.Rows[i]["assn_term"]);
                workSheet.Cells[recordIndex, 8].Value = Convert.ToString(tbl.Rows[i]["course_delivery"]);
                workSheet.Cells[recordIndex, 9].Value = Convert.ToString(tbl.Rows[i]["instructor_1_id"]);
                workSheet.Cells[recordIndex, 10].Value = Convert.ToString(tbl.Rows[i]["instructor_1_name"]);
                workSheet.Cells[recordIndex, 11].Value = Convert.ToString(tbl.Rows[i]["assn_notes"]);
                recordIndex++;
            }

            // By default, the column width is not 
            // set to auto fit for the content
            // of the range, so we are using
            // AutoFit() method here. 
            workSheet.Column(1).AutoFit();
            workSheet.Column(2).AutoFit();
            workSheet.Column(3).AutoFit();
            workSheet.Column(4).AutoFit();
            workSheet.Column(5).AutoFit();
            workSheet.Column(6).AutoFit();
            workSheet.Column(7).AutoFit();
            workSheet.Column(8).AutoFit();
            workSheet.Column(9).AutoFit();
            workSheet.Column(10).AutoFit();
            workSheet.Column(11).AutoFit();
            // file name with .xlsx extension 
            //string p_strPath = "H:\\geeksforgeeks.xlsx";
            //string BasePath = Directory.GetCurrentDirectory();//需要读取当前路径时记得用函数而不是./



            string BasePath = Directory.GetCurrentDirectory();//需要读取当前路径时记得用函数而不是./
            string UploadPath = @"/Temp/"; // 文件存储路径
            string FileName = "OutputReport.xlsx";
            string Path = BasePath + UploadPath + FileName;
            if (File.Exists(Path))
                File.Delete(Path);
            // Create excel file on physical disk 
            FileStream objFileStrm = File.Create(Path);
            objFileStrm.Close();

            // Write content to excel file 
            File.WriteAllBytes(Path, excel.GetAsByteArray());
            //Close Excel package
            excel.Dispose();
            //Console.ReadKey();
            return (FileName);
        }
    }
}

