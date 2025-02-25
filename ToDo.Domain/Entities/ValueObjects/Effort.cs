
using Microsoft.EntityFrameworkCore;

namespace ToDo.Domain.Entities.ValueObjects
{
    [Owned]
    public class Effort : ValueObject
    {
        public Effort() { }

        public Effort(int weeks, int days, int hours, int minutes) 
        { 
            Weeks = weeks;
            Days = days;
            Hours = hours;
            Minutes = minutes;
        }

        public int Weeks { get; private set; }

        public int Days { get; private set; }

        public int Hours { get; private set; }

        public int Minutes { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Weeks;
            yield return Days;
            yield return Hours;
            yield return Minutes;
        }

        public override string ToString() 
        {
            return $"{Weeks}w {Days}d {Hours}h {Minutes}m";
        }
    }
}
