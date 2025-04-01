using Contracts_and_Models.Models;
using Microsoft.EntityFrameworkCore;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Repository
{
	public class DepartmentRepository : IDepartmentRepository
	{
		private readonly PAS_DBContext _pAS_DBContext;

		public DepartmentRepository(PAS_DBContext pAS_DBContext)
        {
			_pAS_DBContext = pAS_DBContext;
		}
        public async Task<Department> GetDepartmentByIdAsync(int department_id)
		{
			return await _pAS_DBContext.Departments.Include(i => i.items_in_possesion).Where(d => d.Id == department_id).FirstOrDefaultAsync();
		}

		public async Task<List<Department>> GetDepartmentsAsync()
		{
			return await _pAS_DBContext.Departments.Include(i => i.items_in_possesion).ToListAsync();
		}
	}
}
