using LocalEyesWebAPI.Constants;
using System.Text.RegularExpressions;

namespace LocalEyesWebAPI.Data
{
    public class UploadFileHandler
    {
        private readonly ILogger<UploadFileHandler> _logger;

        public UploadFileHandler(ILogger<UploadFileHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SaveUploadAsync(List<IFormFile> files, int id)
        {
            // Generate full file path for the file.
            var fullSavePath = Path.Combine(ConstantStringDictionary.BaseUploadSavePath, id.ToString());

            if (!Directory.Exists(fullSavePath))
            {
                Directory.CreateDirectory(fullSavePath);
            }

            foreach (var file in files)
            {
                var fileName = file.FileName.ToUpper().Replace(" ", "-").Replace("Æ", "AE").Replace("Ø", "OE").Replace("Å", "AA");

                var fullFilePath = Path.Combine(fullSavePath, fileName);

                if (!File.Exists(fullFilePath))
                {
                    // Save file to disk.
                    try
                    {
                        using var fileStream = new FileStream(fullFilePath, FileMode.Create);
                        await file.CopyToAsync(fileStream);
                        _logger.LogInformation("Filen blev gemt med succes. " + Path.GetFileName(fullFilePath));
                    }
                    catch (Exception ex)
                    {
                        // Send error to logger..
                        _logger.LogError("Filen kunne ikke uploades: " + ex.Message);

                        // Upload failed, return false.
                        return false;
                    }
                }
                else
                {
                    // Log information.
                    _logger.LogInformation("Filen blev ikke uploaded, da den allerede er uploadet! " + file.FileName);
                }
            }

            return true;
        }
    }
}