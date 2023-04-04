using System;
using System.Collections.Generic;
using System.Text;

namespace SikuliStandardNet
{
    public static class JavaUtils
    {
        public static string JavaPath
        {
            get
            {
                string javaPath = Environment.GetEnvironmentVariable("JAVA_HOME");

                if (!string.IsNullOrEmpty(javaPath))
                {
                    return javaPath;
                }
                else
                {
                    //TODO: find from registry or somewhere else
                    return string.Empty;
                }
            }
        }

        public static string JavaBinPath
        {
            get
            {
                if (string.IsNullOrEmpty(JavaPath))
                {
                    return string.Empty;
                }
                if (!JavaPath.Contains("bin"))
                {
                    return System.IO.Path.Combine(JavaPath, @"bin");
                }
                return JavaPath;
            }
        }

        public static string JavaExePath
        {
            get
            {
                if (string.IsNullOrEmpty(JavaBinPath))
                {
                    return string.Empty;
                }
                return System.IO.Path.Combine(JavaBinPath, "java.exe");
            }
        }

        public static string JavawExePath
        {
            get
            {
                if (string.IsNullOrEmpty(JavaBinPath))
                {
                    return string.Empty;
                }
                return System.IO.Path.Combine(JavaBinPath, "javaw.exe");
            }
        }
    }
}
