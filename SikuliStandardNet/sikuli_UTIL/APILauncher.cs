using SikuliStandardNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SikuliStandard.sikuli_UTIL
{
    public class APILauncher
    {
        public Process apiProcess;
        public string API_Output;
        public EventHandler EvtLogMessage { get; set; }

        private string APIJar;
        private string WorkingDir;
        private string APIPath;
        private string JarReleaseAddress;
        public string Error;
        List<string> commandsList;
        public bool IsProcessStarted;
        public static string ProcessId;

        public APILauncher(out string LogMessage, bool WindowLess, bool UseCustomJava, string CustomJavaPath = "")
        {
            LogMessage = string.Empty;
            APIJar = "sikulirestapi-1.0.jar";
            JarReleaseAddress = "http://sourceforge.net/projects/sikulirestapi/files/sikulirestapi-1.0.jar/download";
            WorkingDir = Path.GetDirectoryName(typeof(APILauncher).Assembly.Location);
            APIPath = Path.Combine(WorkingDir, APIJar);
            commandsList = new List<string>(3);
            string customBinPath = Path.Combine(CustomJavaPath, @"bin");
            string customExePath = Path.Combine(customBinPath, @"java.exe");
            string customWExePath = Path.Combine(customBinPath, @"javaw.exe");
            if (WindowLess)
            {
                if (!UseCustomJava)
                {
                    commandsList.Add(JavaUtils.JavaExePath);
                }
                else
                {
                    commandsList.Add(customExePath);
                }
            }
            else
            {
                if (!UseCustomJava)
                {
                    commandsList.Add(JavaUtils.JavawExePath);
                }
                else
                {
                    commandsList.Add(customWExePath);
                }
            }
            commandsList.Add("-jar \"" + APIPath);
            if (!WindowLess)
            {
                if (!UseCustomJava)
                {
                    commandsList.Add(JavaUtils.JavaBinPath);
                }
                else
                {
                    commandsList.Add(customBinPath);
                }
            }
            IsProcessStarted = false;
        }

        private void TriggerLogEvent(SikuliErrorModel ex)
        {
            if (EvtLogMessage != null)
            {
                EvtLogMessage(ex, EventArgs.Empty);
            }
        }

        public void Start()
        {
            VerifyJarExists();
            ExecuteCommandSync(commandsList);
        }

        public async Task<bool> Stop()
        {
            if (apiProcess != null)
            {
                apiProcess.Close();
            }
            return true;
        }

        public void ForceClose()
        {
            apiProcess.Kill();
        }

        public void VerifyJarExists()
        {
            SikuliErrorModel ex = new SikuliErrorModel();
            if (File.Exists(APIPath))
            {
                ex.Message = "Jar Found on Path: " + APIPath;
            }
            else
            {
                ex.Message = "Jar Not Found on Path: " + APIPath;
                WebClient client = new WebClient();
                client.DownloadFile(JarReleaseAddress, APIPath);
            }
            TriggerLogEvent(ex);
        }

        public void ExecuteCommandSync(object command)
        {
            SikuliErrorModel ex = new SikuliErrorModel();
            try
            {
                try
                {
                    if (!string.IsNullOrEmpty(ProcessId) && Process.GetProcessById(int.Parse(ProcessId)) != null)
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                    // Process.GetProcessById sometimes throws exception if it cannot find a process instead of returning null
                }
                List<string> commnadConfigs = (List<string>)command;
                //ExInfo += "Executing Command: " + commnadConfigs[0] + " " + commnadConfigs[1] + Environment.NewLine;

                apiProcess = new Process();
                if (commnadConfigs.Count > 2)
                {
                    apiProcess.StartInfo.FileName = commnadConfigs[0];
                    apiProcess.StartInfo.Arguments = commnadConfigs[1];
                    apiProcess.StartInfo.WorkingDirectory = commnadConfigs[2];
                }
                else
                {
                    apiProcess.StartInfo = new ProcessStartInfo(commnadConfigs[0], commnadConfigs[1]);
                }
                //apiProcess.StartInfo.UseShellExecute = false;
                //apiProcess.StartInfo.RedirectStandardOutput = true;
                //apiProcess.StartInfo.RedirectStandardError = true;
                IsProcessStarted = apiProcess.Start();
                if (IsProcessStarted)
                {
                    ex.Message = "Jar started on Path: " + APIPath + " Process id is: " + apiProcess.Id.ToString();
                    TriggerLogEvent(ex);
                    ProcessId = apiProcess.Id.ToString();
                }
            }
            catch (Exception e)
            {
                Error = "Failed to execute the command, Error: " + e.Message;
                ex.Message = Error;
                ex.Exception = e;
                TriggerLogEvent(ex);
            }
        }
    }

    public class SikuliErrorModel
    {
        public Exception Exception;
        public string Message;
    }
}
