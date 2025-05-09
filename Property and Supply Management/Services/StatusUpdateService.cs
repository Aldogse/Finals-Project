using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;
using Contracts_and_Models.Models;

namespace Property_and_Supply_Management.Services
{
	public class StatusUpdateService : BackgroundService
	{
		private readonly ILogger<StatusUpdateService> _logger;
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public StatusUpdateService(ILogger<StatusUpdateService>logger,IServiceScopeFactory serviceScopeFactory)
        {
			_logger = logger;
			_serviceScopeFactory = serviceScopeFactory;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using var scope = _serviceScopeFactory.CreateScope();
					var database = scope.ServiceProvider.GetRequiredService<PAS_DBContext>();
					var item_repository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
					var maintenance_item_repository = scope.ServiceProvider.GetRequiredService<IMaintenanceItemRepository>();

					await status_update(database,item_repository,maintenance_item_repository);
					_logger.LogInformation($"Status service completed at {DateTime.Today}");
					await Task.Delay(TimeSpan.FromHours(12),stoppingToken);
				}
				catch (Exception ex)
				{
					throw new Exception();
				}
			}
			
		}

		private async Task status_update(PAS_DBContext pAS_DBContext,IItemRepository itemRepository,IMaintenanceItemRepository maintenanceRepository)
		{
			
			var transaction = await pAS_DBContext.Database.BeginTransactionAsync();
			try
			{
				var items_to_update = await itemRepository.ItemDueForMaintenance();
				if (items_to_update == null)
				{
					items_to_update = new List<Item>();
				}

				foreach (var item in items_to_update)
				{					
					//check if the item already exist on the maintenance database
					if (await maintenanceRepository.GetItemByIdAsync(item.id) == null)
					{
						item.Status = Contracts_and_Models.Enums.Status.InMaintenance;
						var new_item_under_maintenance = new MaintenanceItem
						{
							item_id = item.id,
							Status = Contracts_and_Models.Enums.MaintenanceStatus.InProgress,
							reason = "First Compliance maintenance check",
							IsNotified = false,
							start_date = DateTime.Now,
							end_date = DateTime.Now.AddDays(7)
						};
						pAS_DBContext.MaintenanceItems.Add(new_item_under_maintenance);
						pAS_DBContext.Update(item);
						await transaction.CommitAsync();
						await pAS_DBContext.SaveChangesAsync();
					}
					else
					{
						continue;
					}
				}

			} catch (Exception) 
			{
				await transaction.RollbackAsync();
				throw new ArgumentException(); 
			}
		}
	}
}
