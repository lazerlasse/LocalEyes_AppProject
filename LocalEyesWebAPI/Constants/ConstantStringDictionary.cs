namespace LocalEyesWebAPI.Constants
{
    public static class ConstantStringDictionary
    {
        public const string BaseHostUrl = "https://app.localeyes.dk";
        public static readonly string BaseUploadSavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        public static readonly string BaseMediaUploadsUrl = "https://app.localeyes.dk/uploads/";
    }
}
