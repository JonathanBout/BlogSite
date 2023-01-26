using System.ComponentModel.DataAnnotations;

namespace BlogSite.Data.FormData
{
	public class RegisterFormData
	{
		[Required]
		[MinLength(1), MaxLength(20)]
		[RegularExpression(@"^[a-zA-Z-_0-9]*$",
			ErrorMessage = "A username can only contain the up- and lowercase characters a-z, the '-', the '_' and numbers 0-9.")]
		public string Username { get; set; } = "";
		[Required]
		public string FirstName { get; set; } = "";
		public string MiddleName { get; set; } = "";
		[Required]
		public string LastName { get; set; } = "";

		[Required, EmailAddress]
		public string Email { get; set; } = "";

		[Required, MinLength(6, ErrorMessage = "Password length should be more than 6")]
		[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[#?!@$%^&*-]).*$",
			ErrorMessage = "Password should contain at least one uppercase, one lowercase and one special character.")]
		public string Password { get; set; } = "";
		[Required, Compare(nameof(Password), ErrorMessage = "Entered passwords are not the same.")]
		public string ConfirmPassword { get; set; } = "";
	}
}
