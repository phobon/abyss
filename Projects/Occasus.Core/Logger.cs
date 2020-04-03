using System;
using System.IO;

namespace Occasus.Core
{
    public static class Logger
    {
        private const string FileExtension = ".txt";

        /// <summary>
        /// Gets the filename base.
        /// </summary>
        public static string FilenameBase
        {
            get
            {
                return DateTime.Now.ToString("ddMMyyyy") + "_Error";
            }
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="e">The exception.</param>
        public static void Log(Exception e)
        {
            var filename = FilenameBase + FileExtension;
            var count = 0;
            while (File.Exists(filename))
            {
                count++;
                filename = FilenameBase + count + FileExtension;
            }

            var writer = new StreamWriter(filename, false);
            writer.WriteLine("Exception Log:");
            writer.WriteLine(string.Empty);
            writer.WriteLine(e);
            writer.Close();
        }
    }
}
