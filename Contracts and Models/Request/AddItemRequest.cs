using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;
using Contracts_and_Models.Models;

namespace Contracts_and_Models.Request
{
	public  class AddItemRequest
	{
		[Required(ErrorMessage = "Name is required")]
		public string asset_name { get; set; }
        [Required(ErrorMessage = "Item Type is required")]
        public int Amount { get; set; }
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Purchase date is required")]
		public DateTime purchase_date { get; set; }
		public DateTime? maintenance_date { get; set; }
		public int? AssignedTo { get; set; }
		public ItemUser? User { get; set; }
		public Status Status { get; set; }
	}
}
