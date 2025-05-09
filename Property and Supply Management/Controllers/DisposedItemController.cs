using Microsoft.AspNetCore.Mvc;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;
using Responses.Items;

namespace Property_and_Supply_Management.Controllers
{
	[ApiController]
	[Route("DisposedItems/v1/")]
	public class DisposedItemController : ControllerBase
	{
		private readonly PAS_DBContext _pAS_DBContext;
		private readonly IDisposedItemRepository _disposedItemRepository;

		public DisposedItemController(PAS_DBContext pAS_DBContext, IDisposedItemRepository disposedItemRepository)
		{
			_pAS_DBContext = pAS_DBContext;
			_disposedItemRepository = disposedItemRepository;
		}

		[HttpGet("get-all-disposed-items")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var DisposedItems = await _disposedItemRepository.GetDisposedItemsAsync();

				if(DisposedItems == null)
				{
					return Ok("No Disposed Item listed");
				}

				var response = DisposedItems.Select(item => new disposed_item_details
				{
					disposal_id = item.disposal_id,
					item_name = item.item_name,
					Reason = item.Reason,
					DisposalDate = item.DisposalDate.ToShortDateString(),
					DisposalMethod = item.DisposalMethod.ToString(),
				}).ToList();

				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500,$"Server error: {ex.Message}");
			}
		}
		[HttpDelete("delete-records/{disposal_id}")]
		public async Task <IActionResult> DeleteItemRecords(int disposal_id)
		{
			if(disposal_id <= 0)
			{
				return BadRequest("Invalid Disposal ID");
			}
			try
			{
				var item_to_delete = await _disposedItemRepository.GetDisposedItemByIdAsync(disposal_id);

				if(item_to_delete == null)
				{
					return NotFound();
				}

				_pAS_DBContext.Remove(item_to_delete);
				await _pAS_DBContext.SaveChangesAsync();
				return Ok("Item Deleted from hospital records");
			}
			catch (Exception ex)
			{
				return StatusCode(500,ex.Message);
			}
		}
    }
}
