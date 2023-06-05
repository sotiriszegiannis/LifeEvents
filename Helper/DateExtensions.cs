using Microsoft.JSInterop;
using System.Runtime.CompilerServices;

namespace Helper
{
    public static class DateExtensions
    {
        public static int GetTotalMinutes(this DateTime from,DateTime to)
        {
            return (int)Math.Ceiling(Math.Abs(to.Subtract(from).TotalMinutes));
        }
        /// <summary>
        /// Converts a UTC time to a local time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ianaTimeZone"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static DateTime ToIanaTimeZone(this DateTime date,string ianaTimeZone)
        {
            string windowsTimeZone;
            if(!TimeZoneInfo.TryConvertIanaIdToWindowsId(ianaTimeZone, out windowsTimeZone))
                throw new ArgumentOutOfRangeException("ianatimezone could not be related to windows timezone");
            var convertedDate = date.ToTimeZone(windowsTimeZone);            
            return DateTime.SpecifyKind(convertedDate, DateTimeKind.Unspecified);
        }
        /// <summary>
        /// Converts a local time to a UTC time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="ianaTimeZone"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static DateTime FromIanaTimeZone(this DateTime date, string ianaTimeZone)
        {
            string windowsTimeZone;
            if (!TimeZoneInfo.TryConvertIanaIdToWindowsId(ianaTimeZone, out windowsTimeZone))
                throw new ArgumentOutOfRangeException("ianatimezone could not be related to windows timezone");                        
            var convertedDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(date, DateTimeKind.Unspecified), TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZone));            
            return convertedDate;
        }
        public static bool IsTheSameDayWith(this DateTime inputDateTime, DateTime comparableDateTime)
        {
            return inputDateTime.Year == comparableDateTime.Year && inputDateTime.Month == comparableDateTime.Month &&
                   inputDateTime.Day == comparableDateTime.Day;
        }
        public static DateTime GetNow()
        {
            return DateTime.UtcNow;
        }
        public static string GetDateWithTime(this DateTime inputDatetime)
        {
            string timePart = inputDatetime.ToString("HH:mm");
            string datePart = inputDatetime.ToShortDateString();
            string finalPart = datePart + " " + timePart;
            return finalPart;
        }
        /// <summary>
        /// return a date of format e.g. May 29 2020 12:30 PM
        /// </summary>
        /// <param name="inputDateTime"></param>
        /// <param name="hideCurrentYear">if true it will not show the year part if date's year is the current one</param>
        /// <returns></returns>
        public static string GetDescriptiveDate(this DateTime inputDateTime, bool hideCurrentYear = false)
        {
            if (hideCurrentYear && DateTime.UtcNow.Year == inputDateTime.Year)
                return inputDateTime.ToString("MMMM dd h:mm tt");
            else
                return inputDateTime.ToString("MMMM dd yyyy h:mm tt");
        }
        /// <summary>
        /// Returns a date string that is how the date is being stored in the db
        /// </summary>
        /// <param name="inputDatetime"></param>
        /// <returns>e.g.:2019-08-02 02:21:33.517</returns>
        public static string GetSqlDate(this DateTime inputDatetime)
        {
            return inputDatetime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        public static string GetTimeAgo(this DateTime fromDate)
        {
            return _GetTimeAgo(fromDate);
        }
        public static string GetTimeAgo(this DateTime? fromDate)
        {
            if (fromDate.HasValue)
                return _GetTimeAgo(fromDate.Value);
            else
                return "";
        }
        public static string AddDatePrefix(this string str)
        {
            if (!string.IsNullOrEmpty(str))
                return DateTime.Now.ToString("ddMMyy_Hms") + str;
            else
                return DateTime.Now.ToString("ddMMyy_Hms");
        }
        public static string GetMinutesDurationDescription(this int _minutes)
        {
            if (_minutes > 0)
            {
                if (_minutes < 60)
                {
                    if (_minutes > 0)
                        return $"{_minutes}'";
                    else
                        return "";
                }
                else
                {
                    var hours = _minutes / 60;
                    var minutes = _minutes % 60;
                    if (minutes > 0)
                        return $"{hours}h {minutes}'";
                    else
                        return $"{hours}h";
                }
            }
            else
                return "";
        }
        public static string GetTimeDurationDescription(this TimeSpan timeSpan)
        {
            string timeDescription = "";
            if (timeSpan.TotalDays > 30)
                timeDescription = ((int)timeSpan.TotalDays / 30).ToString() + " months";
            else
                if (timeSpan.TotalDays > 7)
                timeDescription = ((int)timeSpan.TotalDays / 7).ToString() + " weeks";
            else
                if (timeSpan.TotalHours > 24)
                timeDescription = ((int)timeSpan.TotalDays).ToString() + " days";
            else
                if (timeSpan.TotalMinutes > 60)
                timeDescription = ((int)timeSpan.TotalHours).ToString() + " hours";
            else
            {
                if (timeSpan.TotalMinutes > 1)
                    timeDescription = ((int)timeSpan.TotalMinutes).ToString() + "'";
                else
                    timeDescription = ((int)timeSpan.TotalSeconds).ToString() + "\"";
            }
            return timeDescription;
        }
        public static DateTime GetStartOfDayDate(this DateTime input)
        {
            return input.Date;
        }
        public static DateTime Parse(this MiniDate input)
        {
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, 0);
        }
        public static DateTime GetEndOfDayDate(this DateTime input)
        {
            return input.GetStartOfDayDate().AddMinutes(1439);//23 hours and 59 minutes
        }
        public static DateTime GetUTCStartOfDayDateInTimeZone(this DateTime userTimeZone, string timeZoneId)
        {
            return userTimeZone.GetStartOfDayDate().FromTimeZone(HelperFunctions.GetTimeZoneInfo(timeZoneId));
        }
        public static DateTime GetUTCEndOfDayDateInTimeZone(this DateTime userTimeZone, string timeZoneId)
        {
            return userTimeZone.GetEndOfDayDate().FromTimeZone(HelperFunctions.GetTimeZoneInfo(timeZoneId));
        }
        public static DateTime FromTimeZone(this DateTime userTimeZone, string timeZoneId)
        {
            return userTimeZone.FromTimeZone(HelperFunctions.GetTimeZoneInfo(timeZoneId));
        }
        /// <summary>
        /// Returns the start of a day in a particular timezone
        /// </summary>
        /// <param name="userTimeZone"></param>
        /// <param name="timeZoneInfo"></param>
        /// <returns></returns>
        public static DateTime FromTimeZone(this DateTime userTimeZone, TimeZoneInfo timeZoneInfo)
        {
            return userTimeZone.AddMinutes(-timeZoneInfo.BaseUtcOffset.TotalMinutes);
        }
        public static DateTime ToTimeZone(this DateTime utcTime, string timezoneId)
        {
            if (!string.IsNullOrEmpty(timezoneId))
            {
                var timezones = TimeZoneInfo.GetSystemTimeZones();
                var targetTimeZone = timezones.FirstOrDefault(p => p.Id == timezoneId);
                return utcTime.ToTimeZone(targetTimeZone);
            }
            else
                return utcTime;
        }
        public static DateTime ToTimeZone(this DateTime utcTime, TimeZoneInfo timeZone)
        {
            if (utcTime.Kind != DateTimeKind.Utc)
                utcTime = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTime(utcTime, timeZone);
            //return utcTime.AddMinutes(timeZone.BaseUtcOffset.TotalMinutes);
        }
        public static DateTime SpecifyTimeZone(this DateTime dateTime, string timezoneId)
        {
            var timezones = TimeZoneInfo.GetSystemTimeZones();
            var timeZone = timezones.First(p => p.Id == timezoneId);
            var dateTimeUnspec = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timeZone);
        }
        /// <summary>
        /// Returns the number of day, of a date, inside the week. Monday is 0 and sunday 6.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetDayOfWeek(this DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case System.DayOfWeek.Monday:
                    return 0;
                case System.DayOfWeek.Tuesday:
                    return 1;
                case System.DayOfWeek.Wednesday:
                    return 2;
                case System.DayOfWeek.Thursday:
                    return 3;
                case System.DayOfWeek.Friday:
                    return 4;
                case System.DayOfWeek.Saturday:
                    return 5;
                case System.DayOfWeek.Sunday:
                    return 6;
                default:
                    throw new Exception("No match found for the given date");
            }
        }
        /// <summary>
        /// The input must be in the format hh:mm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TimeSpan? ParseTime(this string input)
        {
            var parts = input.Split(':');
            if (parts.Count() >= 2)
            {
                return new TimeSpan(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), parts.Count() > 2 ? Convert.ToInt32(parts[2]) : 0);
            }
            else
                return null;
        }        
        public static DateTime GetFirstDayOfWeek(this DateTime dateTime)
        {
            var dayOfWeek = (int)dateTime.DayOfWeek;
            if(dayOfWeek == 0)
            {
                dayOfWeek = 7;
            }
            var monday = dateTime.AddDays(-dayOfWeek + (int)DayOfWeek.Monday);
            return monday.GetStartOfDayDate();
        }
        public static DateTime GetLastDayOfWeek(this DateTime dateTime)
        {
            var sunday = dateTime.AddDays((7 - (int)dateTime.DayOfWeek)%7);
            return sunday.GetEndOfDayDate();
        }
        public static DateTime GetFirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return new DateTime(dateTime.Year, dateTime.Month, daysInMonth);
        }
        public static int ToMinutes(this int hours)
        {
            return hours * 60;
        }
        private static string _GetTimeAgo(DateTime utcFromDate)
        {
            string timeAgo = "";
            TimeSpan timeSpan = GetNow().Subtract(utcFromDate);
            if (timeSpan.TotalMinutes < 60)
                timeAgo = (int)timeSpan.TotalMinutes + " minutes ago";
            else
            if (Math.Floor(timeSpan.TotalHours) < 24)
                timeAgo = (int)timeSpan.TotalHours + " hours ago";
            else
                if (timeSpan.TotalDays < 7)
                timeAgo = Math.Floor(timeSpan.TotalDays) + " days ago";
            else
                if (timeSpan.TotalDays < 30)
                timeAgo = ((int)timeSpan.TotalDays / 7).ToString() + " weeks ago";
            return timeAgo;
        }
    }
}