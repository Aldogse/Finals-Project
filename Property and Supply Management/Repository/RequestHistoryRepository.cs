using Contracts_and_Models.Models;
using Contracts_and_Models.Responses;
using Microsoft.EntityFrameworkCore;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Repository
{
	public class RequestHistoryRepository : IRequestMedicationHistory
	{
		private readonly PAS_DBContext _pas_db_Context;
        public RequestHistoryRepository(PAS_DBContext pAS_DBContext)
        {
            _pas_db_Context = pAS_DBContext;
        }

		public async Task<List<MedicationRequestRecords>> GetAllApproveRequestAsync()
		{
			return await _pas_db_Context.MedicationRequestHistory.Where(status => status.isApproved == true).Include(d => d.Department)
				 .Include(m => m.Medication).ToListAsync();
		}

		public async Task<List<MedicationRequestRecords>> GetAllPendingRequestAsync()
		{
			return await _pas_db_Context.MedicationRequestHistory.Where(status => status.isApproved != true && (status.rejected == null || status.rejected == false)).Include(d => d.Department)
				 .Include(m => m.Medication).ToListAsync();
		}

		public async Task<List<MedicationRequestRecords>> GetAllRejectedRequestAsync()
		{
			return await _pas_db_Context.MedicationRequestHistory.Where(s => s.rejected == true)
				.Include(d => d.Department)
				.Include(m => m.Medication)
				.ToListAsync();
		}

		public async Task<MedicationRequestRecords> GetRequestAsync(int request_id)
		{
			var medicine = await _pas_db_Context.MedicationRequestHistory.Where(i => i.request_id == request_id)
				.Include(d => d.Department)
				.Include(m => m.Medication)
				.FirstOrDefaultAsync();

			return medicine;
		}

		public async Task<List<MedicationRequestRecords>> GetRequestHistory()
		{
			return await _pas_db_Context.MedicationRequestHistory.Include(d => d.Department).Include(m => m.Medication).ToListAsync();
		}
	}
}
