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
		public string Username { get; set; }

		public User() : this(0, "", "", "", "", "", "") { }

		[BsonCtor]
		public User(int id, string firstName, string lastName, string middleName, string email, string passwordHash, string username)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
			MiddleName = middleName;
			Email = email;
			PasswordHash = passwordHash;
			Username = username;
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
