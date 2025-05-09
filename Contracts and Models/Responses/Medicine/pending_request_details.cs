using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responses.Medicine
{
    public class pending_request_details
    {
        public int request_id { get; set; }
        public string medication_name { get; set; }
        public string requestor_department { get; set; }
        public string type_of_medication { get; set; }
        public string request_date { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
    }
}
