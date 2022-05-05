using System;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;

namespace ElasticSearchManager
{
	public class Utilities
	{
        public static string ConvertDbBitValueToYesNo(object value, string defaultValue)
        {
            return ConvertDbBitValueToBool(value, defaultValue == Constants.PROPER_YES) ? Constants.PROPER_YES : Constants.PROPER_NO;
        }

        public static bool ConvertDbBitValueToBool(object value, bool defaultValue)
        {
            return value == DBNull.Value ? defaultValue : Convert.ToBoolean(value.ToString());
        }

        public static bool ConvertDbIntValueToBool(object value, bool defaultValue)
        {
            return (value == DBNull.Value || string.IsNullOrWhiteSpace(value.ToString())) ? defaultValue : Convert.ToInt32(value.ToString()) != 0;
        }

		public static string Setting(string SettingName)
		{
            string sEntry = ConfigurationManager.AppSettings[SettingName];
			if (sEntry != null)
				return sEntry.Trim();
			else
				return "";			
		}

        public static string ConnectionString(string name)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch
            {
                return string.Empty;
            }            
        }


		public static bool IsNumeric(string Value)
		{
            int i;
            return int.TryParse(Value, out i);
		}

        public static bool IsDate(string Value)
        {
            DateTime dt;
            return DateTime.TryParse(Value, out dt);
        }

        public static bool IsBool(string Value)
        {
            bool b;
            return bool.TryParse(Value, out b);
        }


		public static DateTime CombineDateTime(DateTime DatePart, DateTime TimePart)
		{
			return  new DateTime(DatePart.Year,  DatePart.Month,  DatePart.Day,  
				TimePart.Hour,  TimePart.Minute,  TimePart.Second); 
		}

		public static string RemoveTrailingChar(StringBuilder sb, string TrailingChar)
		{
			string message = sb.ToString();
			if (message.EndsWith(TrailingChar))
				message = message.Substring(0, message.Length - 1);

			return message;
		}

        /// <summary>
        /// Converts a separated string into a List object
        /// </summary>
        /// <param name="separatedTextList"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static List<string> SeparatedTextToList(string separatedTextList, string Separator)
        {
            string[] textList = separatedTextList.Split(Separator.ToCharArray());
            List<string> separatedList = new List<string>();

            separatedList.AddRange(textList);
            return separatedList;
        }

        /// <summary>
        /// Converts a List of type string to a separated list
        /// </summary>
        /// <param name="ListOfEntries"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static string ListToSeparatedText(List<string> ListOfEntries, string Separator)
        {
            return string.Join(Separator, ListOfEntries.ToArray());
        }

        public static string ApplicationPath
        {
            get
            {
                string fullPath = System.Reflection.Assembly.GetEntryAssembly().Location;
                FileInfo fi = new FileInfo(fullPath);

                return fi.DirectoryName;
            }
        }

        public static string SizeSuffix(Int64 value, int decimalPlaces = 1) {
            string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            if (value < 0) { return "-" + SizeSuffix(-value); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000) {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
            // Unix timestamp is milliseconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static double NanosecondsToMilliseconds(double nanoseconds) {
            return nanoseconds * 0.000001;
        }

    }
		

    public static class ExtensionMethods
    {
        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(s.Length - length, length);
            }
            else
            {
                return s;
            }
        }

        public static string Fill(this string original, params object[] values)
        {
            return string.Format(original, values);
        }

        public static string Serialize(this List<string> lst)
        {
            var xs = new XmlSerializer(lst.GetType());
            var xml = new Utf8StringWriter();
            xs.Serialize(xml, lst);

            return xml.ToString();

        }

        public static string Serialize(this List<int> lst)
        {
            var xs = new XmlSerializer(lst.GetType());
            var xml = new Utf8StringWriter();
            xs.Serialize(xml, lst);

            return xml.ToString();
        }

        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
    }

	public class AppSettings
	{
        private const string VIERO_DIRECT_CONNECTION_STRING = "VieroDirect";
		private const string OUTPUT_DEBUG_INFO_TO_FILE = "OutputDebug_File";
		private const string MAX_DEBUG_FILE_SIZE = "MaxDebugFileSize";
		private const string NUMBER_OF_DEBUG_FILES_TO_ROTATE = "NumberOfDebugFilesToRotate";
		private const string DISK_LOGGING_LEVEL = "DiskLoggingLevel";
		private const string DB_LOGGING_LEVEL = "DbLoggingLevel";

        private const string SMTP_HOST = "SmtpHost";
        private const string SMTP_USER_NAME = "SmtpUserName";
        private const string SMTP_PASSWORD = "SmtpPassword";
        private const string SMTP_AUTHENTICATION = "SmtpAuthentication";
        private const string SMTP_PORT = "SmtpPort";
        private const string SMTP_SSL = "SmtpSsl";
        private const string SMTP_TLS = "SmtpTls";

        private const string ADMIN_EMAIL_ADDRESS = "AdminEmailAddress";

        private const string EMAIL_FROM_ADDRESS = "EmailFromAddress";
        private const string EMAIL_FROM_NAME = "EmailFromName";

		private const string LOGGING_LITERAL_NONE = "NONE";
		private const string LOGGING_LITERAL_CYCLE_RESULTS = "CYCLE";
		private const string LOGGING_LITERAL_FULL_RESULTS = "FULL";
	    private const string SUPPORT_EMAIL = "SupportEmail";

        private const string EMERGENCY_EMAIL_TO_ADDRESS = "EmergencyEmailToAddress";
        private const string EMERGENCY_SMTP_HOST = "EmergencySmtpHost";
        private const string EMERGENCY_SMTP_USERNAME = "EmergencySmtpUserName";
        private const string EMERGENCY_SMTP_PASSWORD = "EmergencySmtpPassword";

        private const string VIERO_USER_ID = "VieroUserID";
        private const string VIERO_PASSWORD = "VieroPassword";

        private const string UNIFIED_VIERO_SERVER = "UnifiedVieroServer";
        private const string UNIFIED_VIERO_DB = "UnifiedVieroDB";

        private const string LONG_TERM_COMMAND_TIME_OUT = "LongTermCommandTimeOut";

        public static int LongTermCommandTimeOut = 0;

		public static int DebugLevel = 0;
		public static byte DiskLogLevel = Constants.LOGGING_FULL;
		public static byte DbLogLevel = Constants.LOGGING_NONE;

        public static bool OutputDebugInfoToFile = true;

        public static string OutputDebugFile = Constants.APP_NAME + ".log";
        public static long MaxDebugFileSize = Constants.DEFAULT_MAX_DEBUG_FILE_SIZE;
        public static int NumberOfDebugFilesToRotate = Constants.DEFAULT_NUMBER_OF_DEBUG_FILES_TO_ROTATE;

        public static string SmtpHost = string.Empty;
        public static string SmtpUserID = string.Empty;
        public static string SmtpPassword = string.Empty;
        public static string SmtpAuthentication = string.Empty;
        public static int SmtpPort = Constants.DEFAULT_SMTP_PORT;
        public static bool SmtpSsl = false;
        public static bool SmtpTls = false;
        public static string AdminEmailAddress = string.Empty;
        public static string EmailFromAddress = string.Empty;
        public static string EmailFromName = string.Empty;

	    public static string SupportEmail = string.Empty;
	    
        //record emergency email settings
        public static string EmergencyEmailToAddress = string.Empty;
        public static string EmergencySmtpHost = string.Empty;
        public static string EmergencySmtpPassword = string.Empty;
        public static string EmergencySmtpUserName = string.Empty;

        public static string VieroUserID = string.Empty;
        public static string VieroPassword = string.Empty;

        public static string UnifiedVieroServer = string.Empty;
        public static string UnifiedVieroDB = string.Empty;


        public static string VieroDirectConnectionString;

	    static AppSettings ()
		{
			string setting;

	        VieroDirectConnectionString = Utilities.ConnectionString(VIERO_DIRECT_CONNECTION_STRING);

			//determine disk logging behaviour
			setting = Utilities.Setting(DISK_LOGGING_LEVEL).ToUpper();
			if (setting == LOGGING_LITERAL_NONE)
				DiskLogLevel = Constants.LOGGING_NONE;
			else if (setting == LOGGING_LITERAL_CYCLE_RESULTS)
				DiskLogLevel = Constants.LOGGING_CYCLE;
			else if (setting == LOGGING_LITERAL_FULL_RESULTS)
				DiskLogLevel = Constants.LOGGING_FULL;
			else
				DiskLogLevel = Constants.LOGGING_FULL;

			//determine db logging behaviour
			setting = Utilities.Setting(DB_LOGGING_LEVEL).ToUpper();
			if (setting == LOGGING_LITERAL_NONE)
				DbLogLevel = Constants.LOGGING_NONE;
			else if (setting == LOGGING_LITERAL_CYCLE_RESULTS)
				DbLogLevel = Constants.LOGGING_CYCLE;
			else if (setting == LOGGING_LITERAL_FULL_RESULTS)
				DbLogLevel = Constants.LOGGING_FULL;
			else
				DbLogLevel = Constants.LOGGING_NONE;

						
			SmtpHost = Utilities.Setting(SMTP_HOST);
            SmtpUserID = Utilities.Setting(SMTP_USER_NAME);			
            SmtpPassword = Utilities.Setting(SMTP_PASSWORD);
            SmtpAuthentication = Utilities.Setting(SMTP_AUTHENTICATION);

            setting = Utilities.Setting(SMTP_PORT);
            if (setting.Length > 0 && Utilities.IsNumeric(setting))
                SmtpPort = int.Parse(setting);

            setting = Utilities.Setting(SMTP_SSL);
            if (setting.Length > 0 && Utilities.IsBool(setting) )
                SmtpSsl = Convert.ToBoolean(setting);


            setting = Utilities.Setting(SMTP_TLS);
            if (setting.Length > 0 && Utilities.IsBool(setting))
                SmtpTls = Convert.ToBoolean(setting);

            AdminEmailAddress = Utilities.Setting(ADMIN_EMAIL_ADDRESS);

            EmailFromAddress = Utilities.Setting(EMAIL_FROM_ADDRESS);
            EmailFromName = Utilities.Setting(EMAIL_FROM_NAME);

            EmergencyEmailToAddress = Utilities.Setting(EMERGENCY_EMAIL_TO_ADDRESS);
            EmergencySmtpHost = Utilities.Setting(EMERGENCY_SMTP_HOST);
            EmergencySmtpPassword = Utilities.Setting(EMERGENCY_SMTP_PASSWORD);
            EmergencySmtpUserName = Utilities.Setting(EMERGENCY_SMTP_USERNAME);

            VieroUserID = Utilities.Setting(VIERO_USER_ID);
            VieroPassword = Utilities.Setting(VIERO_PASSWORD);

            UnifiedVieroServer = Utilities.Setting(UNIFIED_VIERO_SERVER);
            UnifiedVieroDB = Utilities.Setting(UNIFIED_VIERO_DB);

            SupportEmail = Utilities.Setting(SUPPORT_EMAIL);

            //LongTermCommandTimeOut = Convert.ToInt32(Utilities.Setting(LONG_TERM_COMMAND_TIME_OUT));

		}


		/// <summary>
		/// Gets the numeric setting.
		/// </summary>
		/// <param name="SettingName">Name of the setting.</param>
		/// <param name="DefaultValue">Default value.</param>
		/// <returns>The value of the setting</returns>
		private static int GetNumericSetting(string SettingName, int DefaultValue)
		{
			string setting = Utilities.Setting(SettingName);
			if (Utilities.IsNumeric(setting))
				return Convert.ToInt32(setting);
			else
                return DefaultValue;
		}	
	}

}
