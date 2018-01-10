using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;

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
    }
}
