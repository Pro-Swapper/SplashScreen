using System.Net;
namespace ProSwapperSplashScreen
{
    class api
    {
        public class imgapi
        {
            public string[] url { get; set; }
        }
        private static readonly string[] hosturls = { "https://pro-swapper.github.io/api/splashscreen.json", "https://raw.githubusercontent.com/Pro-Swapper/api/main/splashscreen.json" };
        public static string[] GetImgurUrls()
        {
            int url = 0;
            string data = "";
            WebClient web = new WebClient();
            redo:  try
            {
                data = web.DownloadString(hosturls[url]);
            }
            catch
            {
                url++;
                goto redo;
            }
            return Program.js.Deserialize<imgapi>(data).url;
        }
    }
}
