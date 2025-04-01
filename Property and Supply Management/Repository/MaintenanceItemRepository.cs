using Contracts_and_Models.Models;
using Microsoft.EntityFrameworkCore;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Repository
{
	public class MaintenanceItemRepository : IMaintenanceItemRepository
	{
		private readonly PAS_DBContext _pAS_DBContext;

		public MaintenanceItemRepository(PAS_DBContext pAS_DBContext)
        {
			_pAS_DBContext = pAS_DBContext;
		}

		public async Task<List<MaintenanceItem>> GetItemCompletedStatusAsync()
		{
			return await _pAS_DBContext.MaintenanceItems.Include(i => i.Item).Where(status => status.Status == Contracts_and_Models.Enums.MaintenanceStatus.Completed)
				.ToListAsync();
		}

		public async Task<MaintenanceItem> GetItemIdAsync(int id)
		{
			return await _pAS_DBContext.MaintenanceItems.Where(i => i.item_id == id).FirstOrDefaultAsync();
		}

		public async Task<List<MaintenanceItem>> GetItemInProgressStatusAsync()
		{
			return await _pAS_DBContext.MaintenanceItems.Include(i => i.Item).Where(status => status.Status == Contracts_and_Models.Enums.MaintenanceStatus.InProgress)
				.ToListAsync();
		}

		public async Task<List<MaintenanceItem>> GetItemPendingPartsStatusAsync()
		{
			return await _pAS_DBContext.MaintenanceItems.Include(i => i.Item).Where(status => status.Status == Contracts_and_Models.Enums.MaintenanceStatus.Pending_Parts)
				.ToListAsync();
		}

		public Task<List<MaintenanceItem>> GetItemsAsync()
		{
			return _pAS_DBContext.MaintenanceItems.Include(i => i.Item).Include(i => i.Item).ToListAsync();
		}

		public async Task<List<MaintenanceItem>> Get_Non_notifiedItemAsync()
		{
			return await _pAS_DBContext.MaintenanceItems.Include(d => d.Item).Where(n => n.IsNotified == false).ToListAsync();
		}
	}
}
