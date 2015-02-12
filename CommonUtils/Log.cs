using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public interface IConsoleLog
    {
        void putConsoleMessage(String message, CommonUtils.Log.MessageLevel messageLevel);

        void onConsoleInitialized();

        void onConsoleRemoving();
    };

    public class Log
    {
        #region Public variables

        public static bool TimeStamp = true;
        public static bool appendToFile = false;

        public enum LogLevel { Silent, Verbose, Warning, Error, Critical }
        public enum MessageLevel { Verbose, Warning, Error, Critical }

        #endregion

        #region Internal variables

        static readonly object _locker = new object();
        static readonly object _locker2 = new object();

        private static List<IConsoleLog> console = null;
        private static int level = 0;
        private static String filename = "";

        #endregion

        public static void setLoggingLevel(LogLevel LoggingLevel)
        {
            switch (LoggingLevel)
            {
                case LogLevel.Silent:
                    level = 0;
                    break;
                case LogLevel.Verbose:
                    level = 4;
                    break;
                case LogLevel.Warning:
                    level = 3;
                    break;
                case LogLevel.Error:
                    level = 2;
                    break;
                case LogLevel.Critical:
                    level = 1;
                    break;
                default:
                    level = 0;
                    break;
            }
        }

        public static bool IsLoggerInitialized()
        {
            return filename.Length == 0;
        }

        public static void Init(String Filename, LogLevel LoggingLevel, String greetingMessage = "Logger started!")
        {
            switch (LoggingLevel)
            {
                case LogLevel.Silent:
                    level = 0;
                    break;
                case LogLevel.Verbose:
                    level = 4;
                    break;
                case LogLevel.Warning:
                    level = 3;
                    break;
                case LogLevel.Error:
                    level = 2;
                    break;
                case LogLevel.Critical:
                    level = 1;
                    break;
                default:
                    level = 0;
                    break;
            }

            if (console == null) console = new List<IConsoleLog>();

            filename = Filename;
            try
            {
                if (LoggingLevel != LogLevel.Silent)
                {
                    if (!appendToFile)
                    {
                        if (File.Exists(Filename))
                            File.Delete(Filename);

                        FileStream fs = File.Create(Filename);
                        StreamWriter tw = new StreamWriter(fs);
                        tw.WriteLine(String.Format("{0}{1}", getTimeStamp(), greetingMessage));
                        tw.Close();
                    }
                    else
                    {
                        StreamWriter sr = File.AppendText(filename);
                        sr.WriteLine(String.Format("\r\n\r\n--- NEW LOGGER ENTRY {0} ---\r\n{1}{2}", DateTime.Now.ToString(), getTimeStamp(), greetingMessage));
                        sr.Close();
                    }

                }
            }
            catch (Exception)
            {
                filename = "";
                throw;
            }
        }

        public static void Verbose(String str)
        {
            if (!IsLoggerInitialized()) return;

            if (level >= 4)
            {
                Write(String.Format("{0}VERBOSE: {1}", getTimeStamp(), str), MessageLevel.Verbose);
            }
        }

        public static void Warning(String str, Exception ex = null)
        {
            if (!IsLoggerInitialized()) return;

            if (level >= 3)
            {
                Write(String.Format("{0}WARNING: {1}\r\n{2}", getTimeStamp(), str, ex == null ? "" : ex.ToString()), MessageLevel.Warning);
            }
        }

        public static void Error(String str, Exception ex = null)
        {
            if (!IsLoggerInitialized()) return;

            if (level >= 2)
            {
                Write(String.Format("{0}ERROR: {1}\r\n{2}", getTimeStamp(), str, ex == null ? "" : ex.ToString()), MessageLevel.Error);
            }
        }

        public static void Critical(String str)
        {
            if (!IsLoggerInitialized()) return;

            if (level >= 1)
            {
                Write(String.Format("{0}CRITICAL: {1}", getTimeStamp(), str), MessageLevel.Critical);
            }
        }

        public static void RawMessage(String str)
        {
            if (!IsLoggerInitialized()) return;

            if (level >= 1)
            {
                Write(getTimeStamp() + str, MessageLevel.Verbose);
            }
        }

        private static void Write(String str, MessageLevel msgLevel)
        {
            lock (_locker)
            {
                StreamWriter sr = File.AppendText(filename);
                sr.WriteLine(str);
                sr.Close();


                if (console == null) console = new List<IConsoleLog>();
                foreach (IConsoleLog c in console)
                {
                    c.putConsoleMessage(String.Format("{0}\r\n", str), msgLevel);
                }
            }
        }

        private static String getTimeStamp()
        {
            lock (_locker2)
            {
                if (TimeStamp)
                    return String.Format("{0}   ", DateTime.Now.ToString());
                else
                    return "";
            }
        }

        public static void addConsole(IConsoleLog _console)
        {
            if (console == null) console = new List<IConsoleLog>();
            console.Add(_console);
            _console.onConsoleInitialized();
        }

        public static void removeConsoles()
        {
            foreach (IConsoleLog c in console)
            {
                c.onConsoleRemoving();
            }
            console.Clear();
        }



    }
}
