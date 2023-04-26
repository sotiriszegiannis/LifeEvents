using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class MiniDate
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public MiniDate() { }
        public MiniDate(DateTime dateTime)
        {
            this.Year = dateTime.Year;
            this.Month = dateTime.Month;
            this.Day = dateTime.Day;
            this.Hour = dateTime.Hour;
            this.Minute = dateTime.Minute;
        }
        public DateTime Parse(MiniDate miniDate)
        {
            return new DateTime(miniDate.Year, miniDate.Month, miniDate.Day, miniDate.Hour, miniDate.Minute, 0);
        }
    }
}
