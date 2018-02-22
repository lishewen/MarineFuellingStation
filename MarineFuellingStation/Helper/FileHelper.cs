using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using System.Drawing;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;

namespace MFS.Helper
{
    public static class FileHelper
    {
        public static async Task SaveAsAsync(this IFormFile file, string filePath)
        {
            //保存文件
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var inputStream = file.OpenReadStream();
                await inputStream.CopyToAsync(fileStream);
            }

            //打水印
            ApplyWatermark(filePath, DateTime.Now.ToString());
        }

        private static void ApplyWatermark(string filename, string watermarkText)
        {
            using (var bitmap = Image.FromFile(filename))
            {
                using (var tempBitmap = new Bitmap(bitmap.Width, bitmap.Height))
                {
                    using (Graphics grp = Graphics.FromImage(tempBitmap))
                    {
                        grp.DrawImage(bitmap, 0, 0);
                        bitmap.Dispose();
                        Brush brush = new SolidBrush(Color.FromArgb(120, 255, 0, 0));
                        Font font = new Font("Segoe UI", 30, FontStyle.Bold, GraphicsUnit.Pixel);
                        SizeF textSize = grp.MeasureString(watermarkText, font);
                        Point position = new Point((tempBitmap.Width - ((int)textSize.Width + 10)), (tempBitmap.Height - ((int)textSize.Height + 10)));
                        grp.DrawString(watermarkText, font, brush, position);
                        tempBitmap.Save(filename);
                    }
                }
            }
        }
        /// <summary>
        /// 从微信照片服务器取得图片并保存到本地
        /// </summary>
        /// <param name="id">远程图片id</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="corpId">企业微信CorpId</param>
        /// <param name="secret">企业微信Secret</param>
        /// <returns></returns>
        public static bool SaveFileByWeixin(string id, string filePath, string fileName, string corpId, string secret)
        {
            try
            {
                using (var fileStream = new FileStream(filePath + fileName, FileMode.Create))
                {
                    string AccessToken = AccessTokenContainer.TryGetToken(corpId, secret);
                    MediaApi.Get(AccessToken, id, fileStream);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 使用EPPlus导出Excel(xlsx)
        /// </summary>
        /// <param name="sourceTable">数据源</param>
        /// <param name="strFileName">xlsx文件名(含路径、不含后缀名)</param>
        public static string ExportExcelByEPPlus<T>(IEnumerable<T> list, string strFileName)
        {
            var sourceTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
            {
                sourceTable.Columns.Add(pd.Name, pd.PropertyType);
            }
            foreach (T item in list)
            {
                var Row = sourceTable.NewRow();

                foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
                {
                    Row[pd.Name] = pd.GetValue(item);
                }
                sourceTable.Rows.Add(Row);
            }

            FileInfo file = new FileInfo(strFileName);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(strFileName);
            }
            using (ExcelPackage pck = new ExcelPackage(file))
            {
                //Create the worksheet
                string sheetName = string.IsNullOrEmpty(sourceTable.TableName) ? "导出数据" : sourceTable.TableName;
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(sourceTable, true);

                //Format the row
                ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
                Color borderColor = Color.FromArgb(155, 155, 155);

                using (ExcelRange rng = ws.Cells[1, 1, sourceTable.Rows.Count + 1, sourceTable.Columns.Count])
                {
                    rng.Style.Font.Name = "宋体";
                    rng.Style.Font.Size = 10;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));

                    rng.Style.Border.Top.Style = borderStyle;
                    rng.Style.Border.Top.Color.SetColor(borderColor);

                    rng.Style.Border.Bottom.Style = borderStyle;
                    rng.Style.Border.Bottom.Color.SetColor(borderColor);

                    rng.Style.Border.Right.Style = borderStyle;
                    rng.Style.Border.Right.Color.SetColor(borderColor);
                }

                //Format the header row
                using (ExcelRange rng = ws.Cells[1, 1, 1, sourceTable.Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 241, 246));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.FromArgb(51, 51, 51));
                }

                //Write it back to the client
                //httpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //httpContext.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}.xlsx", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
                //httpContext.Response.ContentEncoding = Encoding.UTF8;

                //httpContext.Response.BinaryWrite(pck.GetAsByteArray());
                //httpContext.Response.End();
                pck.Save();
                return strFileName;
            }
        }
    }
}
