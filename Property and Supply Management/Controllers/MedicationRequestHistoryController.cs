using Contracts_and_Models.Models;
using Contracts_and_Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Controllers
{
	[ApiController]
	[Route("MedicationHistory/v1/")]
	public class MedicationRequestHistoryController : ControllerBase
	{
		private readonly IRequestMedicationHistory _requestMedicationHistory;
		private readonly PAS_DBContext _pAS_DBContext;

		public MedicationRequestHistoryController(IRequestMedicationHistory requestMedicationHistory, PAS_DBContext pAS_DBContext)
		{
			_requestMedicationHistory = requestMedicationHistory;
			_pAS_DBContext = pAS_DBContext;
		}

		[HttpGet("get-all-approved-request")]
		public async Task<IActionResult> get_approved_meds()
		{
			try
			{
				var medications = await _requestMedicationHistory.GetAllApproveRequestAsync();

				if (medications == null || medications.Count == 0)
				{
					return Ok("No Approve Medication on file");
				}

				var response = medications.Select(meds => new RequestHistoryResponse
				{
					request_id = meds.request_id,
					medication_name = meds.Medication.MedicationName,
					description = meds.Description_of_request,
					approval_date = meds.approval_date.ToString(),
					requestor_department = meds.Department.department_name,
					request_date = meds.request_date.ToString(),
					quantity = meds.Quantity,
					status = meds.isApproved,
					type_of_medication = meds.medicationType.ToString(),
				}).ToList();

				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpGet("get-all-pending-request")]
		public async Task<IActionResult> get_pending_requests()
		{
            await Console.Out.WriteLineAsync("I work properly");
            try
			{
				var medications = await _requestMedicationHistory.GetAllPendingRequestAsync();

				if (medications == null || medications.Count == 0)
				{
					return Ok(medications);
				}

				var response = medications.Select(meds => new RequestHistoryResponse
				{
					request_id = meds.request_id,
					medication_name = meds.Medication.MedicationName,
					description = meds.Description_of_request,
					approval_date = meds.approval_date.ToString(),
					requestor_department = meds.Department.department_name,
					request_date = meds.request_date.ToString(),
					quantity = meds.Quantity,
					status = meds.isApproved,
					type_of_medication = meds.medicationType.ToString(),
				}).ToList();
				

				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpPut("reject-medication-request/{request_id}")]
		public async Task<IActionResult> reject_request(int request_id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var request = await _requestMedicationHistory.GetRequestAsync(request_id);

				if (request == null)
				{
					return NotFound();
				}

				request.isApproved = false;
				request.rejected = true;

				_pAS_DBContext.Update(request);
				await _pAS_DBContext.SaveChangesAsync();
				return Ok(request);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal Server Error: {ex.Message}");
			}
		}
		[HttpGet("get-request-details/{request_id}")]
		public async Task <IActionResult> get_request(int request_id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var request = await _requestMedicationHistory.GetRequestAsync(request_id);

				if(request == null)
				{
					return StatusCode(404,"Request not found");
				}

				var response = new pending_request_details
				{
					request_id = request_id,
					medication_name = request.Medication.MedicationName,
					description = request.Description_of_request,
					requestor_department = request.Department.department_name,
					request_date = request.request_date.ToShortDateString(),
					quantity = request.Quantity,
					type_of_medication = request.medicationType.ToString(),
				};

				return Ok(response);
			}
			catch (Exception)
			{
				throw;
			}
		}

		[HttpGet("get-all-reject-request")]
		public async Task <IActionResult> getRejectRequest()
		{
			try
			{
				var rejected_request = await _requestMedicationHistory.GetAllRejectedRequestAsync();

				var response = rejected_request.Select(item => new RequestHistoryResponse
				{
					description = item.Description_of_request,
					approval_date = item.approval_date.ToString(),
					medication_name = item.Medication.MedicationName,
					quantity = item.Quantity,
					requestor_department = item.Department.department_name,
					request_date = item.request_date.ToShortDateString(),
					type_of_medication = item.medicationType.ToString(),
					
				}).ToList();

				return Ok(response);
			}
			catch (Exception)
			{
				throw;
			}
		}

	}
}
