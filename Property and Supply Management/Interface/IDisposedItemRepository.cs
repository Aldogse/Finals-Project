using Contracts_and_Models.Models;

namespace Property_and_Supply_Management.Interface
{
	public interface IDisposedItemRepository
	{
		Task<List<DisposedItem>> GetDisposedItemsAsync();
		Task<DisposedItem> GetDisposedItemByIdAsync(int id);
	}
}
