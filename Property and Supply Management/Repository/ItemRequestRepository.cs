using Contracts_and_Models.Models;
using Contracts_and_Models.Responses;
using Microsoft.EntityFrameworkCore;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Repository
{
	public class ItemRequestRepository : IItemRequestRepository
	{
		private readonly PAS_DBContext _pAS_DBContext;

		public ItemRequestRepository(PAS_DBContext pAS_DBContext)
        {
			_pAS_DBContext = pAS_DBContext;
		}
        public async Task<List<ItemRequestRecords>> GetAllItemRequest()
		{
			return await _pAS_DBContext.itemRequestRecords.Include(d => d.Department).Include(i => i.Item).ToListAsync(); 
		}

		public async Task<ItemRequestRecords> GetRequestByIdAsync(int request_id)
		{
			return await _pAS_DBContext.itemRequestRecords.Where(i => i.request_id == request_id)
				.Include(d => d.Department)
				.Include(i => i.Item)
				.FirstOrDefaultAsync();
		}
	}
}
