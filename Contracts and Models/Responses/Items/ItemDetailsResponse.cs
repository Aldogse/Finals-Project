using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Responses.Items
{
    public class ItemDetailsResponse
    {
        public int id { get; set; }
        public string asset_name { get; set; }
        public int Quantity { get; set; }
        public int Amount { get; set; }
        public string purchase_date { get; set; }
        public string? maintenance_date { get; set; }
        public string? AssignedTo { get; set; }
        public string? User { get; set; }
        public string Status { get; set; }
    }
}
