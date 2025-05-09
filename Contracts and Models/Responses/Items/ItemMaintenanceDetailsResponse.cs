using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Responses.Items
{
    public class ItemMaintenanceDetailsResponse
    {
        public int maintenance_Id { get; set; }
        public int item_id { get; set; }
        public string asset_name { get; set; }
        public string start_date { get; set; }
        public string? end_date { get; set; }
        public string reason { get; set; }
        public string Status { get; set; }
        public string complete_date { get; set; }
    }
}
