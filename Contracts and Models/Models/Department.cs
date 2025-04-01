using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Responses;

namespace Contracts_and_Models.Models
{
	public class Department
	{
		[Key]
		public int Id { get; set; }
		public string department_name { get; set; }
		public string contact_person_email { get; set; }
		public List<Item> items_in_possesion { get; set; }
	}
}
