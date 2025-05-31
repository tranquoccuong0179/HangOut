using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace HangOut.Domain.Payload.Response.Business
{
    public class GetAllBusinessResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string MainImage {  get; set; }

        public string Description {  get; set; }

        public string Address {  get; set; }

        public string Province {  get; set; }

        public bool isActive {  get; set; }
    }
}
