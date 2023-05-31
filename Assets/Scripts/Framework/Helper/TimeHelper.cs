using System;
using System.Text;

namespace Framework.Helper
{
    /*
     *
     * !!!! 任何时候使用到时间戳 Datetime 必须是UTC时间 才能参与运算
     * !!!! 如果计算公式不牵扯时间戳 可以使用当前时间
     * 
     */
    public static class TimeHelper
    {
        private static DateTime m_1970_01_01 = new(1970, 1, 1);

        //获取时间戳
        public static long GetUnixTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        //获取目标时间点的时间戳
        public static long GetUnixTimeStamp(DateTime pTargetDateTime) //这个时间必须是UTC时间 使用DateTime.UtcNow获取!
        {
            return (long)(pTargetDateTime - m_1970_01_01).TotalSeconds;
        }

        //获取差值
        public static int GetBetween(DateTime pLastDateTime, long pStartTimeStamp)
        {
            return (int)(GetUnixTimeStamp(pLastDateTime) - pStartTimeStamp);
        }

        public static int GetBetween(DateTime pLastDateTime, DateTime pStartDateTime)
        {
            return (int)(GetUnixTimeStamp(pLastDateTime) - GetUnixTimeStamp(pStartDateTime));
        }

        // 获取当前时间戳的UtcDateTime值
        public static DateTime GetUtcDateTime(long pTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(pTimeStamp).UtcDateTime;
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