using APICoursePlatform.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    internal class FileService
    {

        public static async Task<GeneralResponse<string>> UploadFileAsync(IFormFile imageFile)
        {
            //File Size
            if (imageFile == null || imageFile.Length == 0)
            {
                return GeneralResponse<string>.FailResponse("File can't be null", null);
            }

            if (imageFile.Length > 5 * 1024 * 1024) // 5MB limit
            {
                return GeneralResponse<string>.FailResponse("Maximum size can be 5 MB", null);
            }

            //Extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf" };
            var extension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return GeneralResponse<string>.FailResponse($"Extension is not valid ({extension})", null);
            }

            //Name changing
            try
            {
                string fileName = Guid.NewGuid().ToString() + extension;

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return GeneralResponse<string>.SuccessResponse("File uploaded successfully", "/uploads/" + fileName);
            }
            catch (Exception ex)
            {

                return GeneralResponse<string>.FailResponse($"Error uploading file: {ex.Message}", ex.Message);
            }
        }
        public static string GetFilePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            return Path.Combine(uploadsFolder, fileName);
        }


    }
}
