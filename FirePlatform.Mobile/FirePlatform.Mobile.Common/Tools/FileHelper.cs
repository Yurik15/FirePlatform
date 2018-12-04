using System;
using System.Net;

namespace FirePlatform.Mobile.Common.Tools
{
    public static class FileHelper
    {
        public static string DownloadFileString(string url)
        {
            string responseContent = string.Empty;
            using (var webClient = new WebClient())
            {
                responseContent = webClient.DownloadString(url);
            }
            return responseContent;
        }
        public static byte[] DownloadFileBytes(string url)
        {
            byte[] responseContent = null;
            using (var webClient = new WebClient())
            {
                responseContent = webClient.DownloadData(url);
            }
            return responseContent;
        }
    }
}
