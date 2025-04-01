using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts_and_Models.Responses
{
	public class UserItemInPossesionRecordsResponse
	{
        public string User { get; set; }
		public ICollection<ItemDetailsResponse> items { get; set; }
    }
}
