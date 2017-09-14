using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        }
    }
}
