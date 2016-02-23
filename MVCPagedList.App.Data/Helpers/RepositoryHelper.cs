using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MVCPagedList.App.Data.Helpers
{
    public static class RepositoryHelper
    {
        public static void LogToDisk(string msg)
        {
            try
            {
                StreamWriter sw = new StreamWriter(string.Format("{0}\\log.txt", Assembly.GetExecutingAssembly().CodeBase), true, Encoding.Default);
                sw.WriteLine(string.Format("New entry at {0} :", DateTime.Now.ToString()));
                sw.WriteLine(msg);
                sw.WriteLine("\n--------------------------------------------------------------------------------------\n");
                sw.WriteLine();
                sw.Close();
            }
            catch { }
        }
    }
}
