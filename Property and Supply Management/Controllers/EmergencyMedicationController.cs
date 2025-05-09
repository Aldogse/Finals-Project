using Contracts_and_Models.Models;
using Contracts_and_Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;
using Request.Medicine;
using Responses.Medicine;

namespace Property_and_Supply_Management.Controllers
{
    [ApiController]
    [Route("Medications/v1/")]
	public class EmergencyMedicationController : ControllerBase
	{
		private readonly IEmergencyMedicationRepository _emergencyMedicationRepository;
		private readonly PAS_DBContext _pAS_DBContext;
		//private readonly IEmailServiceRepository _emailServiceRepository;
		private readonly IRequestMedicationHistory _requestMedicationHistory;
		public EmergencyMedicationController(IRequestMedicationHistory requestMedicationHistory,
									   IEmergencyMedicationRepository emergencyMedicationRepository,
									   PAS_DBContext pAS_DBContext
/*									   IEmailServiceRepository emailServiceRepository*/)
        {
			_emergencyMedicationRepository = emergencyMedicationRepository;
			_pAS_DBContext = pAS_DBContext;
			//_emailServiceRepository = emailServiceRepository;
			_requestMedicationHistory = requestMedicationHistory;			
		}

		[HttpPost("medication-request")]
		public async Task<IActionResult> medication_request([FromBody] medication_request meds_request)
		{
			var transaction = await _pAS_DBContext.Database.BeginTransactionAsync();
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var medicine_to_update = await _emergencyMedicationRepository.GetMedicationAsync(meds_request.medication_id);

				if(medicine_to_update.Quantity < meds_request.quantity)
				{
					return BadRequest("Quantity is below on what you requested");
				}

				//this is to store the value in request history table
				var request_records = new MedicationRequestRecords()
				{
					department_id = meds_request.department_id,
					request_date = DateTime.Now,
					Quantity = meds_request.quantity,
					medicationType = meds_request.medication_type,
					medication_id = meds_request.medication_id,
					Description_of_request = meds_request.description_of_request
				};

				//add the request on the database for history tracking purposes
				request_records.rejected = false;
				_pAS_DBContext.MedicationRequestHistory.Add(request_records);

				//save the changes for the database
				await _pAS_DBContext.SaveChangesAsync();

				await transaction.CommitAsync();
				return Ok(request_records);
				
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500, ex.Message);
			}
		}
		[HttpPut("medication-request-approval/{request_id}")]
		public async Task <IActionResult> medication_approval(int request_id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var transaction = await _pAS_DBContext.Database.BeginTransactionAsync();
			try
			{
				var request = await _requestMedicationHistory.GetRequestAsync(request_id);
				var medication = await _emergencyMedicationRepository.GetMedicationAsync(request.medication_id);

				if(request == null)
				{
					return NotFound("Request not found");
				}

				//update the quantity once approved
				medication.Quantity = medication.Quantity - request.Quantity;

				request.isApproved = true;
				request.rejected = false;
				request.approval_date = DateTime.Now;

				_pAS_DBContext.EmergencyMedications.Update(medication);
				_pAS_DBContext.MedicationRequestHistory.Update(request);
				await _pAS_DBContext.SaveChangesAsync();
				//await _emailServiceRepository.MedicineApprovalNotification(request_id);

				transaction.Commit();
				return Ok(request);
			}
			catch (Exception ex)
			{
               return StatusCode(500, $"Internal Server Error: {ex.Message}");
			}
		}


		//CRUD OPERATIONS
		[HttpGet("get-all-medications")]
		public async Task <IActionResult> GetAll()
		{
			try
			{
				var medicines = await _emergencyMedicationRepository.GetEmergencyMedicationsAsync();

				if(medicines == null)
				{
					return Ok("No Medication stored");
				}

				var response = medicines.Select(medicine => new MedicineDetailsResponse
				{
					drug_id = medicine.drug_id,
					MedicationName = medicine.MedicationName,
					MedicationType = medicine.MedicationType.ToString(),
					department_name = medicine.department.department_name,
					Quantity = medicine.Quantity,
					ExpirationDate = medicine.ExpirationDate.ToShortDateString(),					
				}).ToList();

				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500,ex.Message);
			}
		}

		[HttpPost("add-medication")]
		public async Task <IActionResult> AddMedication([FromBody] AddMedicationRequest addMedicationRequest)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{

				if(addMedicationRequest.ExpirationDate <= DateTime.Today)
				{
					return BadRequest();
				}

				if(addMedicationRequest.Quantity <= 0)
				{
					return BadRequest();
				}

				var new_medication = new EmergencyMedication()
				{
					MedicationName = addMedicationRequest.medication_name,
					MedicationType = addMedicationRequest.MedicationType,
					department_id = addMedicationRequest.department_id,
					Quantity = addMedicationRequest.Quantity,
					ExpirationDate = addMedicationRequest.ExpirationDate
				};
				_pAS_DBContext.Add(new_medication);
				await _pAS_DBContext.SaveChangesAsync();
				return Ok();			
			}
			catch (Exception ex)
			{
				return StatusCode(500,ex.Message);
			}
		}

		[HttpPut("remove-medication/{medicine_id}")]
		public async Task <IActionResult> RemoveMedication(int medicine_id, [FromBody]MedicineDisposalTypeRequest disposedMedication)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var transaction = await _pAS_DBContext.Database.BeginTransactionAsync();

			try
			{
				var medicine_to_delete = await _emergencyMedicationRepository.GetMedicationAsync(medicine_id);
				
				if(medicine_to_delete == null)
				{
					return NotFound();
				}

				var add_to_disposed = new DisposedMedication()
				{
					MedicationName = medicine_to_delete.MedicationName,
					MedicationType = medicine_to_delete.MedicationType,
					Quantity = medicine_to_delete.Quantity,
					medicineDisposalType = disposedMedication.medicineDisposalTypeRequest
				};

				_pAS_DBContext.Entry(add_to_disposed).State = Microsoft.EntityFrameworkCore.EntityState.Added;
				_pAS_DBContext.DisposedMedications.Add(add_to_disposed);

				medicine_to_delete.isRemoved = true;

				 _pAS_DBContext.Update(medicine_to_delete);
				await _pAS_DBContext.SaveChangesAsync();

				await transaction.CommitAsync();
				return Ok();

			}
			catch (Exception)
			{
				await transaction.RollbackAsync();
				throw new Exception();
				
			}
		}
		[HttpGet("get-medication-requests")]
		public async Task <IActionResult> get_request_history()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			try
			{
				var request_history = await _requestMedicationHistory.GetRequestHistory();

				var response = request_history.Select(request => new RequestHistoryResponse
				{
					request_id = request.request_id,
					medication_name = request.Medication.MedicationName,
					requestor_department = request.Department.department_name,
					quantity = request.Quantity,
					request_date = request.request_date.ToShortDateString(),
					type_of_medication = request.medicationType.ToString(),	
					description = request.Description_of_request,
					status = request.isApproved,
					approval_date = request.approval_date.ToString()
				}).ToList();

				return Ok(response);
			}  
			catch (Exception ex)
			{
               return StatusCode(500,$"Error: {ex.Message}");
			}
		}

		[HttpGet("get-medication/{medication_id}")]
		public async Task <IActionResult> get_medication(int medication_id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var medication = await _emergencyMedicationRepository.GetMedicationAsync(medication_id);

				if(medication == null)
				{
					return NotFound("Medicine not found");
				}

				var response = new MedicineDetailsResponse
				{
					drug_id = medication.drug_id,
					department_name = medication.department.department_name,
					ExpirationDate = medication.ExpirationDate.ToShortDateString(),
					MedicationName = medication.MedicationName,
					MedicationType = medication.MedicationType.ToString(),
					Quantity = medication.Quantity,
				};

				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500,$"Internal Server error: {ex.Message}");
			}
		}
    }
}
