using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Project_Api.Utilities
{
    public static class ImageHelper
    {
        public static string SaveImage(IFormFile image, string folderName)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public static void DeleteImage(string folderName, string imageName)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, imageName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public static string GetImageUrl(string folderName, string imageName)
        {
            return $"/{folderName}/{imageName}";
        }

        public static string GetImagePath(string folderName, string imageName)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName, imageName);
        }
    }
}
