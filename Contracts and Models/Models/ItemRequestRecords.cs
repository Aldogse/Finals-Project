using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Contracts_and_Models.Enums;

namespace Contracts_and_Models.Models
{
    public class ItemRequestRecords
    {
        [Key]
        public int request_id { get; set; }
        [ForeignKey("Item")]
        public int item_id { get; set; }
        public string description { get; set; }
        [JsonIgnore]
        public Item Item { get; set; }
        [ForeignKey("Department")]
        public int department_id { get; set; }
        [JsonIgnore]
        public Department Department { get; set; }
        public int quantity { get; set; }
        public ItemUser itemUser { get; set; }
        public DateTime? approvalDate { get; set; }

        public bool? isRejected { get; set; }


    }
}
