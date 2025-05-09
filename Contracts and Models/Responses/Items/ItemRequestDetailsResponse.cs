using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Responses.Items
{
    public class ItemRequestDetailsResponse
    {
        public int request_id { get; set; }
        public string assetName { get; set; }
        public int quantity { get; set; }
        public string department_name { get; set; }
        public string user { get; set; }

    }
}
