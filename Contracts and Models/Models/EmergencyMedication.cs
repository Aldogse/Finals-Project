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
	public class EmergencyMedication
	{
        [Key]
        public int drug_id { get; set; }
        public string MedicationName { get; set; }
        public StatMedicationType MedicationType { get; set; }
		[ForeignKey("department")]
		public int department_id { get; set; }
        public Department department { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
