using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;
using Contracts_and_Models.Models;

namespace Contracts_and_Models.Request
{
	public  class AddMedicationRequest
	{
        public string medication_name { get; set; }
		public StatMedicationType MedicationType { get; set; }
		[ForeignKey("department")]
		public int department_id { get; set; }
		public int Quantity { get; set; }
		public DateTime ExpirationDate { get; set; }
	}
}
