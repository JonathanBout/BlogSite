using System.ComponentModel.DataAnnotations;

namespace BlogSite.Data.FormData
{
	public class LoginFormData
	{
		[Required]
		public string Username { get; set; } = "";
		[Required]
		public string Password { get; set; } = "";
		public bool RememberMe { get; set; }
	}
}
