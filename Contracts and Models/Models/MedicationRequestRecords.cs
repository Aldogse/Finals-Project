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
    public  class MedicationRequestRecords
    {
        [Key]
        public int request_id { get; set; }
        [ForeignKey("Medication")]
        public int medication_id { get; set; }
        [ForeignKey("Department")]
        public string Description_of_request { get; set; }
        public int department_id { get; set; }
        public EmergencyMedication Medication { get; set; }
        public Department Department { get; set; }
		public StatMedicationType medicationType { get; set; }
        public DateTime request_date { get; set; }
        public int Quantity { get; set; }
        public bool isApproved { get; set; }
        public DateTime? approval_date { get; set; }
        public bool? rejected { get; set; }
    }
}
