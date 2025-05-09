using Contracts_and_Models.Models;
using Contracts_and_Models.Responses;

namespace Property_and_Supply_Management.Interface
{
	public interface IItemRequestRepository
	{
		Task<List<ItemRequestRecords>> GetAllItemRequest();
		Task<ItemRequestRecords> GetRequestByIdAsync(int request_id);
	}
}
