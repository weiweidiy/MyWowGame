using System;
using System.Globalization;
using System.Text;

namespace Framework.Helper
{
    public static class TimeHelper
    {
        private static DateTime m_UTCDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 获取当前的时间的Unity时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetUnixTimeStamp()
        {
            return (long)(DateTime.Now - m_UTCDateTime).TotalSeconds;
        }

        /// <summary>
        /// 获得当前DateTime的Unity时间戳(从1970年1月1日（UTC/GMT的午夜）开始所经过的秒数，不考虑闰秒。)
        /// </summary>
        /// <param name="utcTime"></param>
        /// <returns></returns>
        public static long GetUnixTimeStamp(DateTime utcTime)
        {
            return (long)(utcTime - m_UTCDateTime).TotalSeconds;
        }

        /// <summary>
        /// 获取当前DateTime与上一个时间戳的差值
        /// </summary>
        /// <param name="nextDateTime"></param>
        /// <param name="lastUnixTimeStamp"></param>
        /// <returns></returns>
        public static int GetSecondBetweenUnixTimeStamp(DateTime nextDateTime, long lastUnixTimeStamp)
        {
            return (int)(GetUnixTimeStamp(nextDateTime) - lastUnixTimeStamp);
        }

        /// <summary>
        /// 获取两个DateTime之间的时间戳的差值
        /// </summary>
        /// <param name="nextDateTime"></param>
        /// <param name="lastDateTime"></param>
        /// <returns></returns>
        public static int GetSecondBetweenUnixTimeStamp(DateTime nextDateTime, DateTime lastDateTime)
        {
            return (int)(GetUnixTimeStamp(nextDateTime) - GetUnixTimeStamp(lastDateTime));
        }

        /// <summary>
        /// 将当前时间戳转换成DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(long unixTimeStamp)
        {
            return m_UTCDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        /// <summary>
        /// 获取当前DateTime.Today的String值
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeTodayString()
        {
            return DateTime.Today.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取当前DateTime.Now的String值
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeNowString()
        {
            return DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 获取昨天DateTime.Today的String值
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeYesterdayString()
        {
            return DateTime.Today.AddDays(-1).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// 将Sting类型的DateTime转换成DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetStringDateTime(string dateTime)
        {
            return DateTime.Parse(dateTime);
        }

        /*
         * 将时间秒转换成如下sting格式
         * Hour && Minute && Second
         * 23:59:59
         * 59:59
         * 00:59
         */
        private static StringBuilder m_StringBuilder = new StringBuilder(12);

        public static string FormatSecond(int second)
        {
            m_StringBuilder.Clear();
            var ts = new TimeSpan(0, 0, 0, Convert.ToInt32(second));
            if (second >= 3600)
            {
                m_StringBuilder.Append(ts.Hours.ToString("00"));
                m_StringBuilder.Append(":");
            }

            m_StringBuilder.Append(ts.Minutes.ToString("00"));
            m_StringBuilder.Append(":");
            m_StringBuilder.Append(ts.Seconds.ToString("00"));
            return m_StringBuilder.ToString();
        }
    }
}