using Contracts_and_Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;
using Responses.Items;

namespace Property_and_Supply_Management.Controllers
{
	[ApiController]
	[Route("MaintenanceItem/v1/")]
	public class MaintenanceItemController : ControllerBase
	{
		private readonly IMaintenanceItemRepository _maintenanceItemRepository;
		private readonly PAS_DBContext _pAS_DBContext;
		private readonly IItemRepository _itemRepository;

		public MaintenanceItemController(IMaintenanceItemRepository maintenanceItemRepository, PAS_DBContext pAS_DBContext, IItemRepository itemRepository)
		{
			_maintenanceItemRepository = maintenanceItemRepository;
			_pAS_DBContext = pAS_DBContext;
			_itemRepository = itemRepository;
		}

		[HttpGet("get-all-item")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var items = await _maintenanceItemRepository.GetItemsAsync();
				if (items.Count == 0)
				{
					return Ok("No item undermaintenance");
				}

				var response = items.Select(item => new ItemMaintenanceDetailsResponse
				{
					maintenance_Id = item.maintenance_Id,
					item_id = item.item_id,
					asset_name = item.Item.asset_name,
					start_date = item.start_date.ToString(),
					end_date = item.end_date.ToString(),
					Status = item.Status.ToString(),
					reason = item.reason,
					complete_date = item.complete_date.ToString(),
				}).ToList();

				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
		[HttpPut("status-update-complete/{item_id}")]
		public async Task<IActionResult> MaintenanceCompleted(int item_id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			using var transaction = await _pAS_DBContext.Database.BeginTransactionAsync();
			try
			{
				var item_to_complete = await _maintenanceItemRepository.GetItemByIdAsync(item_id);
				var item_to_update = await _itemRepository.GetItemByIdAsync(item_id);

				if (item_to_complete == null)
				{
					return NotFound();
				}
				item_to_complete.complete_date = DateTime.Now;
				item_to_complete.Status = Contracts_and_Models.Enums.MaintenanceStatus.Completed;
				await _pAS_DBContext.SaveChangesAsync();

				item_to_update.Status = Contracts_and_Models.Enums.Status.Active;
				await _pAS_DBContext.SaveChangesAsync();

				await transaction.CommitAsync();
				return Ok(item_to_complete);
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, ex.Message);
			}
		}
		[HttpGet("inProgress-items")]
		public async Task<IActionResult> GetInprogressItems()
		{
			try
			{
				var request = await _maintenanceItemRepository.GetItemInProgressStatusAsync();

				if(request == null || request.Count == 0)
				{
					return Ok("No item in progress status");
				}

				var response =  request.Select(item => new ItemMaintenanceDetailsResponse
				{
					item_id = item.item_id,
					asset_name = item.Item.asset_name,
					end_date = item.end_date.ToString(),
					start_date = item.start_date.ToString(),
					reason = item.reason,
					Status = item.Status.ToString(),
				}).ToList();

				return Ok(response);
				
			}catch (Exception ex)
			{
				return StatusCode(500,ex.Message);
			}
		}

		[HttpGet("pending-parts-item")]
		public async Task <IActionResult> GetPendingPartsItems()
		{
			try
			{
				var request = await _maintenanceItemRepository.GetItemPendingPartsStatusAsync();

				if(request == null || request.Count == 0)
				{
					return Ok("No pending parts item status");
				}

				var response = request.Select(item => new ItemMaintenanceDetailsResponse
				{
					item_id = item.item_id,
					asset_name = item.Item.asset_name,
					end_date = item.end_date.ToString(),
					start_date = item.start_date.ToString(),
					reason = item.reason,
					Status = item.Status.ToString(),
				}).ToList();

				return Ok(response);
			}catch (Exception ex)
			{
				return StatusCode(500,ex.Message);
			}
		}

		[HttpGet("get-details/{item_id}")]
		public async Task<IActionResult> get_details(int item_id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}
			try
			{
				var maintenance_details = await _maintenanceItemRepository.GetItemByIdAsync(item_id);

				if(maintenance_details == null)
				{
					return NotFound();
				}

				var response = new ItemMaintenanceDetailsResponse
				{
					reason = maintenance_details.reason,
					asset_name = maintenance_details.Item.asset_name,
					start_date = maintenance_details.start_date.ToString(),
					end_date = maintenance_details.end_date.ToString(),
					Status = maintenance_details.Status.ToString(),
					maintenance_Id = maintenance_details.maintenance_Id,
					item_id = maintenance_details.item_id,
					complete_date = maintenance_details.complete_date.ToString(),
			    };

				return Ok(response);
			}
			catch (Exception)
			{
				throw;
			}
		}
    }
}
