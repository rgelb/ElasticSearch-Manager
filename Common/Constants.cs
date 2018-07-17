using System;

namespace ElasticSearchManager
{	
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class Constants
	{
		public static readonly DateTime SQL_MIN_DATE = new DateTime(1753, 1, 1);
		public static readonly DateTime SQL_MAX_DATE = new DateTime(9999, 1, 1);
		public const string DB_FRIENDLY_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public static readonly DateTime DEFAULT_NOTIFICATION_TIME = new DateTime(1900, 1, 1, 0, 30, 0);
        public static readonly DateTime MIN_DB_DATE = new DateTime(1900, 1, 1, 0, 0, 0);
        public static readonly DateTime MAX_DB_DATE = new DateTime(9999, 12, 31, 0, 0, 0);
        public const string PROPER_YES = "Yes";
        public const string PROPER_NO = "No";

        public const int DB_RESULT_SUCCESS = 1;
		public static readonly int NOT_FOUND = -1;
	    public static string APP_NAME = "Elastic Search Manager";

	    public const byte LOGGING_NONE = 0;
		public const byte LOGGING_CYCLE = 1;
		public const byte LOGGING_FULL = 2;

        public const string DEFAULT_DEBUG_LOG_FILE = "debug.log";
        public const long DEFAULT_MAX_DEBUG_FILE_SIZE = 20971520;		//20 MB
        public const int DEFAULT_NUMBER_OF_DEBUG_FILES_TO_ROTATE = 5;

        public const int DEFAULT_SMTP_PORT = 25;
        
        public const string MSG_ISSUES = "Following issues have been encountered:";
	    public const string MSG_STARTED = "Application Started";
        public const string MSG_ENDED = "Application Ended";
        public const string MSG_TIMERS_STARTED = "Timers Started";
	}
}
