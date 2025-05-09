using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Request.Items
{
    public class borrow_item_request
    {
        public int item_id { get; set; }
        public string description_of_request { get; set; }
        public int quantity { get; set; }
        public ItemUser user { get; set; }
        public int department_id { get; set; }

    }
}
