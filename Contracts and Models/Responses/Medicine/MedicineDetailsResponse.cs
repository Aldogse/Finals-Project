using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;
using Contracts_and_Models.Models;

namespace Responses.Medicine
{
    public class MedicineDetailsResponse
    {
        public int drug_id { get; set; }
        public string MedicationName { get; set; }
        public string MedicationType { get; set; }
        public string department_name { get; set; }
        public int Quantity { get; set; }
        public string ExpirationDate { get; set; }
    }
}
