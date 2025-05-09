 using Contracts_and_Models.Models;
using Microsoft.EntityFrameworkCore;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Repository
{
	public class EmergencyMedicationRepository : IEmergencyMedicationRepository
	{
		private readonly PAS_DBContext _pAS_DBContext;

		public EmergencyMedicationRepository(PAS_DBContext pAS_DBContext)
        {
            _pAS_DBContext = pAS_DBContext;
        }
        public async Task<List<EmergencyMedication>> GetEmergencyMedicationsAsync()
		{
			return await _pAS_DBContext.EmergencyMedications.Where(r => r.isRemoved == false || r.isRemoved == null)
				.Include(d => d.department)
				.ToListAsync();
		}

		public async Task<EmergencyMedication> GetMedicationAsync(int id)
		{
			var Medication = await _pAS_DBContext.EmergencyMedications.Include(d => d.department)
				                                                      .Where(i => i.drug_id == id).FirstOrDefaultAsync();
			return Medication;
		}

	}
}
