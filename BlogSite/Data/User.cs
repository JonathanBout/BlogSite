using LiteDB;

namespace BlogSite.Data
{
	public class User : IId
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }

		[BsonCtor]
		public User(int id, string firstName, string lastName, string middleName, string email, string passwordHash)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			MiddleName = middleName;
			Email = email;
			PasswordHash = passwordHash;
		}

		public override string ToString()
		{
			string res = FirstName;
			if (!string.IsNullOrEmpty(MiddleName))
			{
				res += " " + MiddleName;
			}
			return res + " " + LastName;
		}
	}
}
