using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace SikuliStandard.sikuli_UTIL
{
    public class APILauncher
    {
        private Process APIProcess;
        private ProcessStartInfo APIProcessStartInfo;
        public string API_Output;

        private string APIJar;
        private string WorkingDir;
        private string APIPath;
        private string JarReleaseAddress;

        public APILauncher(out string LogMessage, bool Windowless = false)
        {
            APIJar = "sikulirestapi-1.0.jar";
            JarReleaseAddress = "http://sourceforge.net/projects/sikulirestapi/files/sikulirestapi-1.0.jar/download";
            WorkingDir = Directory.GetCurrentDirectory();
            APIPath = Path.Combine(WorkingDir, APIJar);
            if (Windowless)
            {
                APIProcessStartInfo = new ProcessStartInfo("java", "-jar \"" + APIPath + "\"");
            }
            else
            {
                APIProcessStartInfo = new ProcessStartInfo("javaw", "-jar \"" + APIPath + "\"");
            }
            APIProcess = new Process();
            APIProcess.StartInfo = APIProcessStartInfo;

            if (APIProcess.Start())
            {
                LogMessage = string.Format("API Path - {0} \n Proces Info - Id - {1}, Title - {2}, Start Time : {3}", APIPath, APIProcess.Id, APIProcess.MainWindowTitle, APIProcess.StartTime);
            }
            else
            {
                LogMessage = string.Format("API Path - {0} \n Error in starting Proces", APIPath);
            }
        }

        public void Start()
        {
            VerifyJarExists();
            Util.Log.WriteLine("Starting jetty server...");
            if (APIProcess.HasExited)
            {
                APIProcess.Start();
            }
        }

        public void Stop()
        {
            Util.Log.WriteLine("Stopping jetty server...");
            APIProcess.Kill();
            Util.Log.WriteLine("Jetty server stopped!");
            Util.Log.WriteLine("Client log for this test run can be located at: " + Util.Log.LogPath);
            Util.Log.WriteLine("Exiting...");
        }

        public void VerifyJarExists()
        {
            if (File.Exists(APIPath))
            {
                Util.Log.WriteLine("Jar already downloaded, launching jetty server...");
            }
            else
            {
                Util.Log.WriteLine("Jar not downloaded, downloading jetty server jar from SourceForge...");
                WebClient client = new WebClient();
                client.DownloadFile(JarReleaseAddress, APIPath);
                Util.Log.WriteLine("File downloaded!");
            }
        }
    }
}
