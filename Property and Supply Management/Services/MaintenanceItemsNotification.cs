using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Services
{
	public class MaintenanceItemsNotification : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public MaintenanceItemsNotification(IServiceScopeFactory serviceScopeFactory)
        {
			_serviceScopeFactory = serviceScopeFactory;
		}
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _serviceScopeFactory.CreateScope();
				{
					var database = scope.ServiceProvider.GetRequiredService<PAS_DBContext>();
					var maintenanceRepository = scope.ServiceProvider.GetRequiredService<IMaintenanceItemRepository>();
					var email_service = scope.ServiceProvider.GetRequiredService<EmailServices>();

					await PendingItemForMaintenanceNotification(database,maintenanceRepository,email_service);
					await Task.Delay(TimeSpan.FromHours(12),stoppingToken);
				}
			}
		}

		private async Task PendingItemForMaintenanceNotification(PAS_DBContext pAS_DBContext, IMaintenanceItemRepository maintenanceItemRepository,IEmailServiceRepository emailServices)
		{
			var transaction = pAS_DBContext.Database.BeginTransaction();
			try
			{
				var un_notified_items = await maintenanceItemRepository.Get_Non_notifiedItemAsync();
				foreach (var item in un_notified_items)
				{
					await emailServices.MaintenanceNotification(item.item_id);
					item.IsNotified = true;
					pAS_DBContext.MaintenanceItems.Update(item);
				}
				await pAS_DBContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				throw new Exception(ex.ToString());
			}
		}
	}
}
