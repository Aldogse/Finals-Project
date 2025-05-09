using Contracts_and_Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Property_and_Supply_Management.Interface;
using Responses.Items;

namespace Property_and_Supply_Management.Controllers
{
	[ApiController]
	[Route("ItemRequest/v1/")]
	public class ItemRequestController : ControllerBase
	{
		private readonly IItemRequestRepository _itemRequestRepository;

		public ItemRequestController(IItemRequestRepository itemRequestRepository)
		{
			_itemRequestRepository = itemRequestRepository;
		}

		//CRUD FUNCTIONS
		[HttpGet("get-all-request")]
		public async Task<IActionResult> GetAllRequest() 
		{
			try
			{
				var request = await _itemRequestRepository.GetAllItemRequest();

				if(request == null || request.Count == 0)
				{
					return NotFound();
				}

				var response =  request.Select(item => new ItemRequestDetailsResponse
				{
					assetName = item.Item.asset_name,
					department_name = item.Department.department_name,
					quantity = item.quantity,
					request_id = item.request_id,
					user = item.itemUser.ToString(),
				}).ToList();

				return Ok(response);
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet("get-request/{request_id}")]
		public async Task <IActionResult> GetRequest(int request_id)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest();
			}

			try
			{
				var request = await _itemRequestRepository.GetRequestByIdAsync(request_id);

				if(request == null)
				{
					return NotFound();
				}

				var response = new ItemRequestDetailsResponse
				{
					assetName= request.Item.asset_name,
					department_name= request.Department.department_name,
					quantity= request.quantity,
					request_id= request.request_id,
					user= request.itemUser.ToString(),
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
