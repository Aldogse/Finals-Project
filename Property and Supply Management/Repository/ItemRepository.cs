using Contracts_and_Models.Models;
using Contracts_and_Models.Responses;
using Microsoft.EntityFrameworkCore;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Repository
{
	public class ItemRepository : IItemRepository
	{
		private readonly PAS_DBContext _pAS_DBContext;

		public ItemRepository(PAS_DBContext pAS_DBContext)
        {
			_pAS_DBContext = pAS_DBContext;
		}

		public async Task<Item> GetItemByIdAsync(int id)
		{
			var item = await _pAS_DBContext.Items.Where(item => item.id == id).Include(d => d.Department).FirstOrDefaultAsync();
			return item;
		}

		public async Task<List<Item>> GetItemsAsync()
		{
			var items = await _pAS_DBContext.Items.Include(d => d.Department).ToListAsync();
			return items;
		}

		public Task<List<Item>> GetItemsByUser(int id)
		{
			throw new NotImplementedException();
		}

		public async Task<paginated_response<ItemDetailsResponse>> paginated_Response(int current_page, int page_size)
		{
			var items = _pAS_DBContext.Items.AsQueryable();

			var total_count = await items.CountAsync();

			var paginated_query = items.OrderBy(i => i.id).Skip((current_page - 1) * page_size).Take(page_size);

			var item_query = await paginated_query.Select(item => new ItemDetailsResponse
			{
				id = item.id,
				asset_name = item.asset_name,
				Amount = item.Amount,
				User = item.User.ToString(),
				maintenance_date = item.maintenance_date.ToString(),
				AssignedTo = item.AssignedTo.ToString(),
				Quantity = item.Quantity,
				Status = item.Status.ToString(),
				purchase_date = item.purchase_date.ToShortDateString(),
			}).ToListAsync();

			return new paginated_response<ItemDetailsResponse>(current_page,page_size,total_count,item_query);
		}
	}
}
