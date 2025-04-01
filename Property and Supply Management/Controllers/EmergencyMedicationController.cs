using Contracts_and_Models.Models;
using Contracts_and_Models.Request;
using Contracts_and_Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Controllers
{
    [ApiController]
    [Route("Medications/v1/")]
	public class EmergencyMedicationController : ControllerBase
	{
		private readonly IEmergencyMedicationRepository _emergencyMedicationRepository;
		private readonly PAS_DBContext _pAS_DBContext;

		public EmergencyMedicationController(IEmergencyMedicationRepository emergencyMedicationRepository,PAS_DBContext pAS_DBContext)
        {
			_emergencyMedicationRepository = emergencyMedicationRepository;
			_pAS_DBContext = pAS_DBContext;
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
				return Ok("Medication added");			
			}
			catch (Exception ex)
			{
				return StatusCode(500,ex.Message);
			}
		}

		[HttpDelete("remove-medication/{medicine_id}")]
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

				 _pAS_DBContext.Remove(medicine_to_delete);
				await _pAS_DBContext.SaveChangesAsync();

				await transaction.CommitAsync();
				return Ok("Item removed");

			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				return StatusCode(500,ex.Message);
				
			}
		}
		
    }
}
