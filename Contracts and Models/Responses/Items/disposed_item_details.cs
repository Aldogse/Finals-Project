using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Responses.Items
{
    public class disposed_item_details
    {
		public int disposal_id { get; set; }
		public string item_name { get; set; }
		public string? Reason { get; set; }
		public string DisposalMethod { get; set; }
		public string DisposalDate { get; set; }
	}
}
