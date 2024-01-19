namespace ASP.NET_Task5.Helpers
{
    public class UploadFileHelper
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };
        public async static Task<string> UploadFile(IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(fileExtension) || !AllowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("The file type is not accepted.");
            }

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine("wwwroot", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{uniqueFileName}";
        }
    }
}
