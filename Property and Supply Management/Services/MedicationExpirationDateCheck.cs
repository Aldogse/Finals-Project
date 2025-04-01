using Contracts_and_Models.Models;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.EmailSenderInformation;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Services
{
    public class MedicationExpirationDateCheck : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly EmailDetails _emailDetails;

        public MedicationExpirationDateCheck(IServiceScopeFactory serviceScopeFactory, EmailDetails emailDetails)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _emailDetails = emailDetails;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<PAS_DBContext>();
                    var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

                    await ExpirationCheck(dbContext, itemRepository);
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
            }
        }

        private async Task ExpirationCheck(PAS_DBContext pAS_DBContext, IItemRepository itemRepository)
        {
			var transaction = await pAS_DBContext.Database.BeginTransactionAsync();
			try
            {
                var items = await itemRepository.GetItemsAsync();               

                foreach (var item in items)
                {
                    if (item.maintenance_date <= DateTime.Today)
                    {
                        var maintenance_item = new MaintenanceItem()
                        {
                            item_id = item.id,
                            start_date = DateTime.Now,
                            end_date = null,
                            reason = "Quarter Maintenance Check",
                            Status = Contracts_and_Models.Enums.MaintenanceStatus.InProgress
                        };

                        pAS_DBContext.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        pAS_DBContext.MaintenanceItems.Add(maintenance_item);

                        //update the status of the item in items table and get the department email
                        item.Status = Contracts_and_Models.Enums.Status.InMaintenance;
                        var department_email = item.Department.contact_person_email;

                    }
                }
                await transaction.CommitAsync();
                await pAS_DBContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error:{ex.Message}");
            }
        }
    }
}
