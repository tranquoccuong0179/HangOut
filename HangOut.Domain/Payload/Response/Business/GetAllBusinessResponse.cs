using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangOut.Domain.Payload.Response.Business
{
    public class GetAllBusinessResponse
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string MainImage { get; set; }
        public DayOfWeek? StartDay { get; set; }
        public DayOfWeek? EndDay { get; set; }

        public string OpeningHours { get; set; }

        public string Addresss { get; set; }

        public string Province { get; set; }

        public string Latidue { get; set; }

        public string Longtidue { get; set; }

        public string CategoryName { get; set; }

        public List<EventsOfBusinessResponse> EventsOfBusiness { get; set; } = new List<EventsOfBusinessResponse>();
    }

    public class EventsOfBusinessResponse
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }

        public string MainImage { get; set; }
    }
}
