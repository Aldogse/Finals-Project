using Contracts_and_Models.Models;

namespace Property_and_Supply_Management.Interface
{
	public interface IEmergencyMedicationRepository
	{
		Task<List<EmergencyMedication>>GetEmergencyMedicationsAsync();
		Task<EmergencyMedication> GetMedicationAsync(int id);
	}
}
