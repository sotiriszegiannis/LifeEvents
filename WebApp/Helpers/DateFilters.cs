using AutoMapper;
using Domain;
using Repository;
using Helper;

namespace WebApp
{
    /// <summary>
    /// Contains date filters for the UI,set at the user local time
    /// </summary>
    /// <remarks>
    /// we need to convert it to the users local time since there could be a case where utc now might be in a previous date(e.g.22:00)
    //which means the local time could be after midnight. This means that the user time zone is in a different day than the system.
    //so in this case the start of day would be retrieving the previous day, which is wrong
    /// </remarks>
    public class DateFilters
    {
        string UserTimeZone { get; set; }
        public DateFilters(UsersRepository usersRepository)
        {
            UserTimeZone = usersRepository.Get().Result.TimeZone;
        }
        public Dates Get(DateTime utdDay)
        {
            return new Dates(UserTimeZone,utdDay);
        }
        public bool IsToday(DateTime dateInUserTimeZone)
        {
            return dateInUserTimeZone.Day == DateTime.UtcNow.ToIanaTimeZone(UserTimeZone).Day;
        }
        public class Dates
        {
            string UserTimeZone { get; set; }
            private DateTime UtcDay { get; set; }
            public Dates(string userTimeZone,DateTime utcDay)
            {
                UserTimeZone=userTimeZone;
                UtcDay = utcDay;
            }
            public DateTime StartOfDay
            {
                get
                {
                    return UtcDay.ToIanaTimeZone(UserTimeZone).GetStartOfDayDate();
                }
            }
            public DateTime EndOfDay
            {
                get
                {                    
                    return UtcDay.ToIanaTimeZone(UserTimeZone).GetEndOfDayDate();
                }
            }            
            public DateTime StartOfWeek
            {
                get
                {
                    return UtcDay.ToIanaTimeZone(UserTimeZone).GetFirstDayOfWeek();
                }
            }
            public DateTime EndOfWeek
            {
                get
                {
                    return UtcDay.ToIanaTimeZone(UserTimeZone).GetLastDayOfWeek();
                }
            }
            public DateTime StartOfMonth
            {
                get
                {
                    return UtcDay.ToIanaTimeZone(UserTimeZone).GetFirstDayOfMonth();
                }
            }
            public DateTime EndOfMonth
            {
                get
                {
                    return UtcDay.ToIanaTimeZone(UserTimeZone).GetLastDayOfMonth();
                }
            }
        }
    }
}
