using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Contracts_and_Models.Models
{
	public class MaintenanceItem
	{
		[Key]
		public int maintenance_Id { get; set; }
		[ForeignKey("Item")]
        public int item_id { get; set; }
		public Item Item { get; set; }
		public DateTime? start_date { get; set; }
		public DateTime? end_date { get; set; }
		public string reason { get; set; }
		public MaintenanceStatus Status { get; set; }
		public bool IsNotified { get; set; }
	}
}
