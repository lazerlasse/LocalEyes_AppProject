using LocalEyesWebAPI.Constants;
using Microsoft.AspNetCore.Mvc;

namespace LocalEyesWebAPI.Data
{
    public static class MediaLinkGenerator
    {
        public static IEnumerable<string> GenerateLink(int id)
        {
            List<string> links = new();
            var files = SearchForFiles(id);

            foreach (var file in files)
            {
                links.Add($"{ConstantStringDictionary.BaseMediaUploadsUrl}{id}/{Path.GetFileName(file)}");
            }

            return links;
        }

        private static IEnumerable<string> SearchForFiles(int id)
        {
            // Generate full file path for the file.
            var fullFilePath = Path.Combine(ConstantStringDictionary.BaseUploadSavePath, id.ToString());

            IEnumerable<string> files = Directory.EnumerateFiles(fullFilePath);

            return files;
        }
    }
}
