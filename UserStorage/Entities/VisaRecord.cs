using System;

namespace UserStorage.Entities
{
    public struct VisaRecord
    {
        public string Country { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public VisaRecord(string country, DateTime start, DateTime end)
        {
            Country = country;
            StartDate = start;
            EndDate = end;
        }
    }
}
