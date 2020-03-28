using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Tests
{
    public static class Config
    {
        public static string DriverPath = Path.Combine(Directory.GetCurrentDirectory() + "../../../../");
        public static string LogsPath = Path.Combine(Directory.GetCurrentDirectory() + "../../../../Logs/");
        public static string PicsPath = Path.Combine(Directory.GetCurrentDirectory() + "../../../../Pics/");
    }
}
