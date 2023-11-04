using LibraryWebDb.Models;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryWebDb.Helpers
{
    public static class FileUploadHelper
    {
        static public async Task<string> UploadAsync(IFormFile formFile)
        {
            if (formFile != null)
            {
                var filename = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
                using var fs = new FileStream(@$"wwwroot/uploads/{filename}", FileMode.Create);
                await formFile.CopyToAsync(fs);
                return $@"/uploads/{filename}";
            }

            throw new Exception("File weren't uploaded");
        }
    }
}
