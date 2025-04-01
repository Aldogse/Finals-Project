using System.Net;
using System.Net.Mail;
using Property_and_Supply_Management.Database;
using Property_and_Supply_Management.EmailSenderInformation;
using Property_and_Supply_Management.Interface;

namespace Property_and_Supply_Management.Services
{
	public class EmailServices : IEmailServiceRepository
	{
		private readonly IItemRepository _itemRepository;
		private readonly EmailDetails _emailDetails;

		public EmailServices(IItemRepository itemRepository,EmailDetails emailDetails)
        {
			_itemRepository = itemRepository;
			_emailDetails = emailDetails;
		}
        public async Task MaintenanceNotification(int id)
		{
			try
			{
				var department = await _itemRepository.GetItemByIdAsync(id);
				var smtp = new SmtpClient()
				{
					UseDefaultCredentials = false,
					Credentials = new NetworkCredential(_emailDetails.Email,_emailDetails.Password),
					Port = _emailDetails.Port,
					Host = _emailDetails.Host,
					EnableSsl = true,					
				};

				var message = new MailMessage()
				{
					Body = $"This Notification is to inform that {department.asset_name} that in your possession is scheduled for maintenance",
					IsBodyHtml = true,
					Subject = "Maintenance Notification",
					From = new MailAddress(_emailDetails.Email),						
				};

				message.To.Add(department.Department.contact_person_email);
				await smtp.SendMailAsync(message);

			}
			catch (Exception ex)
			{
				throw new Exception (ex.ToString());
			}
		}
	}
}
