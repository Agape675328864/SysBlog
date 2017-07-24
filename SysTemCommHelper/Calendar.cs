using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jusfoun.YunCompany.Common
{
    public static class Calendar
    {

        /// <summary>
        /// 返回标准日期格式string
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }


        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        public static string GetDate(string datetimestr, string replacestr)
        {
            if (datetimestr == null)
            {
                return replacestr;
            }

            if (datetimestr.Equals(""))
            {
                return replacestr;
            }

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;

        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string(包含微妙)
        /// </summary>
        public static string GetDateTimeF()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }

        /// <summary>
        /// 返回相对于当前时间的相对天数
        /// </summary>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间 
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime, string formatStr)
        {
            if (fDateTime == "0000-0-0 0:00:00")
            {

                return fDateTime;
            }
            DateTime s = Convert.ToDateTime(fDateTime);
            return s.ToString(formatStr);
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd HH:mm:ss
        /// </sumary>
        public static string GetStandardDateTime(string fDateTime)
        {
            string retDateTime = GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
            if (retDateTime == "1900-01-01 00:00:00") retDateTime = "";
            return retDateTime;
        }
        /// <summary>
        /// 获取当前时间戳，得一10位数秒数
        /// </summary>
        /// <returns></returns>
        public static int GetTimeStamp()
        {
            return GetTimeStamp(DateTime.Now);
        }
        /// <summary>
        /// 获取给定时间格式字符串的时间戳，得一10位数秒数
        /// </summary>
        /// <param name="strDateTime"></param>
        /// <returns></returns>
        public static int GetTimeStamp(string strDateTime)
        {
            return GetTimeStamp(StrToDateTime(strDateTime));
        }
        /// <summary>
        /// 获取给定时间对像的时间戳得一10位数秒数
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetTimeStamp(DateTime dt)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan toNow = dt.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return Tool.ToInt(timeStamp);
        }
        /// <summary>
        /// 将UNIX时间戳转换成系统时间
        /// </summary>
        /// <param name="strtime"></param>
        /// <returns></returns>
        public static DateTime StrToTime(string strtime)
        {
            string timeStamp = strtime;
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;

        }
        public static DateTime StrToTime(int strtime)
        {
            return StrToTime(strtime.ToString());
        }
        public static DateTime StrToDateTime(string fDateTime)
        {
            string initDate = "1900-01-01 00:00:00";
            if (String.IsNullOrEmpty(fDateTime))
            {
                fDateTime = initDate;
            }
            DateTime s = Convert.ToDateTime(initDate);
            try
            {
                s = Convert.ToDateTime(fDateTime);
            }
            catch
            {
                s = Convert.ToDateTime(initDate);
            }
            return s;
        }

        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Sec"></param>
        /// <returns></returns>
        public static int StrDateDiffSeconds(string Time, int Sec)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddSeconds(Sec);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }

        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int StrDateDiffMinutes(string time, int minutes)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }

        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int StrDateDiffHours(string time, int hours)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }
        public static string TimeDiff(string strDateTime)
        {
            return TimeDiff(GetTimeStamp(), GetTimeStamp(strDateTime));
        }
        public static string TimeDiff(int startTimeStamp, int endTimeStamp)
        {
            int timeDiff = endTimeStamp - startTimeStamp;
            int days = (int)(timeDiff / 86400);
            int remain = timeDiff % 86400;
            int hours = (int)(remain / 3600);
            remain %= 3600;
            int mins = (int)(remain / 60);
            int secs = remain % 60;
            string ret = "";
            if (days != 0) ret += days + "天";
            if (hours != 0) ret += hours + "小时";
            if (mins != 0) ret += mins + "分";
            if (secs != 0) ret += secs + "秒";
            if (days >= 0 && hours >= 0 && mins >= 0 && secs >= 0) ret = "还剩" + ret;
            else ret = "已过" + ret;
            return ret;
        }
    }
}
