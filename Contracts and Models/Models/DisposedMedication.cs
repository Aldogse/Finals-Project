using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Contracts_and_Models.Models
{
	public class DisposedMedication
	{
		[Key]
		public int disposal_id { get; set; }
		public string MedicationName { get; set; }
        public int Quantity { get; set; }
        public StatMedicationType MedicationType { get; set; }
		public MedicineDisposalType medicineDisposalType { get; set; }

		public bool NotifiedByEmail {get; set; }
	}
}
