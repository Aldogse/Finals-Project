using Contracts_and_Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;
using Request.Department;
using Responses.Departments;
using Responses.Items;

namespace Property_and_Supply_Management.Controllers
{
    [ApiController]
	[Route("Department/v1/")]
	public class DepartmentController : ControllerBase
	{
		private readonly IDepartmentRepository _departmentRepository;
		private readonly PAS_DBContext _pAS_DBContext;

		public DepartmentController(IDepartmentRepository departmentRepository,PAS_DBContext pAS_DBContext)
        {
			_departmentRepository = departmentRepository;
			_pAS_DBContext = pAS_DBContext;
		}

		//CRUD FUNCTIONS
		[AllowAnonymous]
		[HttpGet("get-all-departments")]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var departments = await _departmentRepository.GetDepartmentsAsync();

				if (departments == null || departments.Count == 0)
				{
					return NotFound();
				}

				var response = departments.Select(department => new DepartmentDetailsResponse
				{
					department_id = department.Id,
					department_name = department.department_name,
					contact_person_email = department.contact_person_email,
					items_in_possesion = department.items_in_possesion.Select(item => new ItemDetailsResponse
					{
						asset_name = item.asset_name,
						Quantity = item.Quantity,
						Amount = item.Amount,
						purchase_date = item.purchase_date.ToShortDateString(),
						maintenance_date = item.maintenance_date.ToString(),
						AssignedTo = item.AssignedTo.ToString(),
						User = item.User.ToString(),
						Status = item.Status.ToString(),
					}).ToList(),
				}).ToList();

				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("get-department/{id}")]
		public async Task <IActionResult> GetDepartment( int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var department = await _departmentRepository.GetDepartmentByIdAsync(id);

				var response = new DepartmentDetailsResponse
				{
					department_id = department.Id,
					department_name = department.department_name,
					contact_person_email = department.contact_person_email,
					items_in_possesion = department.items_in_possesion.Select(item => new ItemDetailsResponse
					{
						id = item.id,
						asset_name = item.asset_name,
						AssignedTo = item.Department.department_name,
						User = item.User.ToString(),
						Status= item.Status.ToString(),
						Quantity = item.Quantity,
						Amount = item.Amount,
						purchase_date = item.purchase_date.ToShortDateString(),
						maintenance_date = item.maintenance_date.ToString(),
					}).ToList(),
				};
				return Ok(response);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		//Add email information
		[HttpPut("update-email-information/{department_id}")]
		public async Task <IActionResult> update_email_information(int department_id,[FromBody]add_email_request department_email)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var department = await _departmentRepository.GetDepartmentByIdAsync(department_id);

				if(department == null)
				{
					return NotFound();
				}

				department.contact_person_email = department_email.new_email;
				_pAS_DBContext.Update(department);
				await _pAS_DBContext.SaveChangesAsync();

				return Ok("Email has been added");
			}
			catch (Exception ex)
			{
				return StatusCode(500,$"Internal server error: {ex.Message}");				
			}
		}

	}
}
