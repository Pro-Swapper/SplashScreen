using System;
using System.IO;
namespace ProSwapperSplashScreen
{
    public class EpicGamesLauncher
    {
        private class InstallationList
        {
            public string InstallLocation { get; set; }
            public string AppName { get; set; }
        }
        private class Root
        {
            public InstallationList[] InstallationList { get; set; }
        }
        public static string GetGamePath()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Epic\UnrealEngineLauncher\LauncherInstalled.dat";
                foreach (InstallationList a in Program.js.Deserialize<Root>(File.ReadAllText(path)).InstallationList)
                {
                    if (a.AppName == "Fortnite")
                        return a.InstallLocation;
                }
            } catch{}
            return null;
        }
    }
}
