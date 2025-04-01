using Contracts_and_Models.Models;
using Microsoft.EntityFrameworkCore;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Repository
{
	public class DisposedItemRepository : IDisposedItemRepository
	{
		private readonly PAS_DBContext _pAS_DBContext;

		public DisposedItemRepository(PAS_DBContext pAS_DBContext)
        {
			_pAS_DBContext = pAS_DBContext;
		}

		public async Task<DisposedItem> GetDisposedItemByIdAsync(int id)
		{
			var item =  await _pAS_DBContext.DisposedItems.Where(item => item.disposal_id == id).FirstOrDefaultAsync();
			return item;
		}

		public async Task<List<DisposedItem>> GetDisposedItemsAsync()
		{
			var response = await _pAS_DBContext.DisposedItems.ToListAsync();
			return response;
		}
	}
}
