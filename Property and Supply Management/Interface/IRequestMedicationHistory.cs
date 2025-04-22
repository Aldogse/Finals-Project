using Contracts_and_Models.Models;
using Contracts_and_Models.Responses;

namespace Property_and_Supply_Management.Interface
{
	public interface IRequestMedicationHistory
	{
		Task<List<MedicationRequestRecords>> GetRequestHistory();
		Task<MedicationRequestRecords> GetRequestAsync(int request_id);
		Task<List<MedicationRequestRecords>> GetAllApproveRequestAsync();
		Task<List<MedicationRequestRecords>> GetAllPendingRequestAsync();
		Task<List<MedicationRequestRecords>>GetAllRejectedRequestAsync();
	}
}
