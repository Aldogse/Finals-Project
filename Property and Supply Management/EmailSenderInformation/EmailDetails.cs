namespace Property_and_Supply_Management.EmailSenderInformation
{
	public class EmailDetails
	{
		private readonly IConfiguration? _configuration;
		public EmailDetails(string email, string host, string password, int port)
		{
			Email = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
			Host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
			Password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
			Port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");
		}

		public string Email { get; set; }
		public string Host { get; set; }
		public string Password { get; set; }
		public int Port { get; set; }
	}
}