using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts_and_Models.Responses
{
    public  class paginated_response <T>
    {
		public paginated_response(int current_page, int page_size, int total_count, List<T> items)
		{
			this.current_page = current_page;
			this.page_size = page_size;
			this.total_count = total_count;
			this.items = items;
		}

		public int current_page { get; set; }
        public int page_size { get; set; }
        public int total_count { get; set; }
        public List<T> items { get; set; }
    }
}
