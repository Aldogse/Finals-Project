using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Request.Items
{
    public class StatusUpdateRequest
    {
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
        public string reason { get; set; }
    }
}
