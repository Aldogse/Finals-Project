using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Contracts_and_Models.Request
{
    public  class medication_request
    {
        public int department_id { get; set; }
        public int medication_id { get; set; }
        public string description_of_request { get; set; }
        public StatMedicationType medication_type { get; set; }
        public DateTime request_date { get; set; }
        public int quantity { get; set; }
    }
}
