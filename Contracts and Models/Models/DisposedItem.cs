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
	public class DisposedItem
	{
		[Key]
		public int disposal_id {  get; set; }
		public string item_name { get; set; }
		public string? Reason { get; set; }
		public DisposalMethod DisposalMethod { get; set; }
		public DateTime DisposalDate { get; set; }

	}
}
