using System.Reflection;
using Contracts_and_Models.Models;
using Contracts_and_Models.Request;
using Contracts_and_Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Controllers
{
	[ApiController]
	[Route("Item/v1/")]
	public class ItemController : ControllerBase
	{
		private readonly IItemRepository _itemRepository;
		private readonly PAS_DBContext _pAS_DBContext;

		public ItemController(IItemRepository itemRepository, PAS_DBContext pAS_DBContext)
        {
			_itemRepository = itemRepository;
			_pAS_DBContext = pAS_DBContext;
		}

		//BASIC CRUD FUNCTIONALITY
		[HttpGet("get-all-items")]
		public async Task<IActionResult> GetItems()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var items = await _itemRepository.GetItemsAsync();
				if(items == null)
				{
					return Ok("No items found");
				}
				var response =  items.Select(item => new ItemDetailsResponse
				{
					id = item.id,
					asset_name = item.asset_name,
					Amount = item.Amount,
					Quantity = item.Quantity,
					purchase_date = item.purchase_date.ToShortDateString(),
					maintenance_date = item.maintenance_date.ToString(),
					AssignedTo = item.Department.department_name,
					User = item.User.ToString(),
                    Status = item.Status.ToString(),
				}).ToList();
				

                Console.WriteLine(response);
                
                return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500,ex.Message);
			}
		}
		[HttpDelete("delete-item/{id}")]
		public async Task<IActionResult> DeleteItem(int id, [FromBody] DisposalItemRequest disposalItemRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			using var transaction = await _pAS_DBContext.Database.BeginTransactionAsync();
			try
			{
				var item_to_disposed = await _itemRepository.GetItemByIdAsync(id);

				if(item_to_disposed == null)
				{
					return NotFound();
				}

				//Add the item on the disposed table
				var disposed_item = new DisposedItem()
				{
					Reason = disposalItemRequest.Reason,
					item_name = item_to_disposed.asset_name,
					DisposalDate = disposalItemRequest.DisposalDate,
					DisposalMethod = disposalItemRequest.DisposalMethod,
				};

				//add to disposal
				_pAS_DBContext.Entry(disposed_item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
			     _pAS_DBContext.DisposedItems.Add(disposed_item);
				var disposed_item_saved = await _pAS_DBContext.SaveChangesAsync();
				Console.WriteLine($"Item Added on Disposed: {disposed_item_saved}");

				//remove from item table
				_pAS_DBContext.Items.Remove(item_to_disposed);
				var item_deleted = await _pAS_DBContext.SaveChangesAsync();
				Console.WriteLine($"Changes on database: {item_deleted}");

				await transaction.CommitAsync();
				return Ok();

			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500,$"Server error: {ex.Message}");
			}
		}
		[HttpPost("add-item")]
		public async Task <IActionResult> AddItem([FromBody] AddItemRequest itemRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var new_item_request = new Item 
				{
					asset_name = itemRequest.asset_name,
					Amount = itemRequest.Amount,
					Quantity = itemRequest.Quantity,
					purchase_date = itemRequest.purchase_date,
					maintenance_date = itemRequest.maintenance_date,
					Status = Contracts_and_Models.Enums.Status.Active,
					AssignedTo = (int) Contracts_and_Models.Enums.ItemUser.No_User
				};

				_pAS_DBContext.Add(new_item_request);
				await _pAS_DBContext.SaveChangesAsync();
				return Ok(new_item_request);
			}
			catch (Exception ex)
			{
				return StatusCode(500,$"Server error : {ex.Message}");
			}
		}
		[HttpPut("update-item/{id}")]
		public async Task<IActionResult> UpdateItemAsync(int id, [FromBody] UpdateItemRequest updateItemRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var item_to_update = await _itemRepository.GetItemByIdAsync(id);
				if(item_to_update == null)
				{
					return NotFound();
				}
				item_to_update.asset_name = updateItemRequest.asset_name;
				item_to_update.purchase_date = updateItemRequest.purchase_date;
				item_to_update.AssignedTo = updateItemRequest.AssignedTo;
				item_to_update.maintenance_date = updateItemRequest.maintenance_date;
				item_to_update.LastMaintenanceDate = updateItemRequest.LastMaintenanceDate;
				item_to_update.Amount = updateItemRequest.Amount;
				item_to_update.Quantity = updateItemRequest.Quantity;
				item_to_update.User = updateItemRequest.User;

				 _pAS_DBContext.Items.Update(item_to_update);
				await _pAS_DBContext.SaveChangesAsync();
				return Ok();
				
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Server error: {ex.Message}");
			}
		}

		//this function will update the item status and put it on the maintenance table
		[HttpPut("maintenance-item/{id}")]
		public async Task <IActionResult> ItemStatusUpdateToMaintenance(int id, [FromBody]StatusUpdateRequest statusUpdateRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var transaction = await _pAS_DBContext.Database.BeginTransactionAsync();
			try
			{
				var item_to_update = await _itemRepository.GetItemByIdAsync(id);

				if(item_to_update == null)
				{
					return NotFound();
				}

				//first update the status from active to inmaintenance
				item_to_update.Status = Contracts_and_Models.Enums.Status.InMaintenance;
				_pAS_DBContext.Update(item_to_update);
				await _pAS_DBContext.SaveChangesAsync();

				//add the item to maintenance page or table with proper details why it needs maintenance
				var maintenance_information = new MaintenanceItem()
				{
					item_id = id,
					start_date = statusUpdateRequest.start_date,
					end_date = statusUpdateRequest.end_date,
					reason = statusUpdateRequest.reason,
					Status = Contracts_and_Models.Enums.MaintenanceStatus.InProgress
				};

				_pAS_DBContext.Entry(maintenance_information).State = Microsoft.EntityFrameworkCore.EntityState.Added;
				_pAS_DBContext.MaintenanceItems.Add(maintenance_information);
				await _pAS_DBContext.SaveChangesAsync();

				await transaction.CommitAsync();
				return Ok();

			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500,ex.Message);
			}
		}
	}
}

