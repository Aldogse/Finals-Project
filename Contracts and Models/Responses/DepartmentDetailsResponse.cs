using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Models;

namespace Contracts_and_Models.Responses
{
	public  class DepartmentDetailsResponse
	{
        public int department_id { get; set; }
        public string department_name { get; set; }
		public string? contact_person_email { get; set; }
		public List<ItemDetailsResponse> items_in_possesion { get; set; }
	}
}
