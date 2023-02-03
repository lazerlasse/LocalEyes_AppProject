using LocalEyesWebAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace LocalEyesWebAPI.Data
{
    public static class FileHandlerService
    {
        public static void DeleteMediaFolderAndFiles(int id)
        {
            var pathToDelete = Path.Combine(ConstantStringDictionary.BaseUploadSavePath, id.ToString());

            if (Directory.Exists(pathToDelete))
            {
                var filesToDelete = Directory.EnumerateFiles(pathToDelete);

                foreach (var file in filesToDelete)
                {
                    File.Delete(file);
                }

                Directory.Delete(pathToDelete);
            }

            return;
        }
    }
}
