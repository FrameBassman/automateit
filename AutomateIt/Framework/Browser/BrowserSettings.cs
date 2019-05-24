namespace AutomateIt.Framework.Browser
{
    public class BrowserSettings
    {
        public const string DOWNLOAD_DIRECTORY_NAME = "Downloads";
        public string DownloadDirectory { get; }

        public BrowserSettings()
        {
            DownloadDirectory = "c:\\" + DOWNLOAD_DIRECTORY_NAME;
        }
    }
}
