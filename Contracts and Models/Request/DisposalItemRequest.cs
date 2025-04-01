using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;
using Contracts_and_Models.Models;

namespace Contracts_and_Models.Request
{
	public  class DisposalItemRequest
	{
		public string? Reason { get; set; }

		[Required(ErrorMessage = "Disposal Method is required")]
		public DisposalMethod DisposalMethod { get; set; }
		public DateTime DisposalDate { get; set; }
	}
}
