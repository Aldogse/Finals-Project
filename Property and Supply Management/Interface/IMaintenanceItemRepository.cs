using Contracts_and_Models.Models;

namespace Property_and_Supply_Management.Interface
{
	public interface IMaintenanceItemRepository
	{
		Task<List<MaintenanceItem>> GetItemsAsync();
		Task<MaintenanceItem> GetItemByIdAsync(int id);
		Task<List<MaintenanceItem>> Get_Non_notifiedItemAsync();
		Task<List<MaintenanceItem>> GetItemInProgressStatusAsync();
		Task<List<MaintenanceItem>> GetItemCompletedStatusAsync();
		Task<List<MaintenanceItem>> GetItemPendingPartsStatusAsync();
	}
}
