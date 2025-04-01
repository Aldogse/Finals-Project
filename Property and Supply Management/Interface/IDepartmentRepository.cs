using Contracts_and_Models.Models;

namespace Property_and_Supply_Management.Interface
{
	public interface IDepartmentRepository
	{
		Task<List<Department>> GetDepartmentsAsync();
		Task<Department> GetDepartmentByIdAsync(int department_id);
	}
}
