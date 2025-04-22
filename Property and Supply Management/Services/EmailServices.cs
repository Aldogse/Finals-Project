using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.EmailSenderInformation;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Services
{
	public class EmailServices
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;

		public EmailServices(IServiceScopeFactory serviceScopeFactory)
		{
			_serviceScopeFactory = serviceScopeFactory;
		}
		//THIS EMAIL FUNCTION BELOW IS FOR ITEM
		//Email notification for all items that is under maintenance
		public async Task MaintenanceNotification(int item_id)
		{
			using var scope = _serviceScopeFactory.CreateScope();
			
			  var db_context =  scope.ServiceProvider.GetRequiredService<PAS_DBContext>();
			  var email_details = scope.ServiceProvider.GetRequiredService<EmailDetails>();
			   var item = scope.ServiceProvider.GetRequiredService<IItemRepository>();

			try
			{
				var item_to_notify = await item.GetItemByIdAsync(item_id);

				var smtp = new SmtpClient()
				{
					Port = email_details.Port,
					Credentials = new NetworkCredential(email_details.Email, email_details.Password),
					EnableSsl = true,
					Host = email_details.Host,
					UseDefaultCredentials = false
				};

				var message = new MailMessage()
				{
					Body = $"This is to inform you that item {item_to_notify.asset_name} with item id : {item_to_notify.id} is due for maintenance.",
					IsBodyHtml = true,
					Subject = "Maintenance Notice",
					From = new MailAddress(email_details.Email),
				};

				message.To.Add(item_to_notify.Department.contact_person_email);
				await smtp.SendMailAsync(message);
			}
			catch(Exception ex)
			{
				throw;
			}							
		}


		//THIS EMAIL FUNCTION BELOW IS FOR MEDICATION
		//Notification for approve medication request
		public async Task ApproveMedicineNotification(int request_id)
		{
			var scope = _serviceScopeFactory.CreateScope();

			var medication_history_repository = scope.ServiceProvider.GetRequiredService<IRequestMedicationHistory>();
			var email_details = scope.ServiceProvider.GetRequiredService<EmailDetails>();
			try
			{
				var medication_to_approve = await medication_history_repository.GetRequestAsync(request_id);

				var smtp = new SmtpClient()
				{
					Port = email_details.Port,
					Host = email_details.Host,
					Credentials = new NetworkCredential(email_details.Email,email_details.Password),
					UseDefaultCredentials = false,
					EnableSsl = true,
				};

				var mail_message = new MailMessage()
				{
					Body = $"This is to inform you that request id: {medication_to_approve.request_id} for {medication_to_approve.Medication.MedicationName} with Quantity of {medication_to_approve.Quantity} has been approved",
					From = new MailAddress(email_details.Email),
					IsBodyHtml = true,
					Subject = "Medication Approval",
				};
				mail_message.To.Add(medication_to_approve.Department.contact_person_email);
				await smtp.SendMailAsync(mail_message);
			}
			catch (Exception)
			{
				throw;
			}


			
		}
	}
} 
