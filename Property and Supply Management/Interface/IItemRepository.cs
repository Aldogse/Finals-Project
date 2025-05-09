using Contracts_and_Models.Models;
using Contracts_and_Models.Responses;
using Responses.Items;

namespace Property_and_Supply_Management.Interface
{
    public interface IItemRepository
	{
		Task<List<Item>> GetItemsAsync();
		Task<Item> GetItemByIdAsync(int id);
		Task<List<Item>> GetItemsByUser(int id);
		Task<List<Item>> ItemDueForMaintenance();
		Task<paginated_response<ItemDetailsResponse>> paginated_Response(int current_page,int page_size);
		
	}
}
