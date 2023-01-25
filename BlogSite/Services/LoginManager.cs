using BlogSite.Data;
using LiteDB;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace BlogSite.Services
{
	public class LoginManager
	{
		private ILiteCollection<User> Users { get; set; }
		public LoginManager(Database database)
		{
			Users = database.GetCollection<User>();
		}

		/// <summary>
		/// Logs the user in. with the specified username and password.<br/>
		/// <see langword="true"/>	| <see cref="User"/>| User exists, password correct.<br/>
		/// <see langword="true"/>	| <see langword="null"/> | User exists, password incorrect.<br/>
		/// <see langword="false"/>| <see langword="null"/> | User does not exist.<br/>
		/// <see langword="false"/>| <see cref="User"/>| Should not be possible.<br/>
		/// </summary>
		/// <param name="username">The entered username.</param>
		/// <param name="password">The entered password.</param>
		/// <param name="user">The resulting user, <see cref="null"/> if authentication failed.</param>
		/// <returns><see cref="true"/> if the user exists.</returns>
		public bool TryLogin(string username, string password, out User? user)
		{
			user = null;
			if (Users.FindOne(x => x.Username == username) is User foundUser)
			{
				if (PasswordHash.Compare(foundUser.PasswordHash, password))
				{
					user = foundUser;
					return true;
				}
				return true;
			}
			return false;
		}

		public bool TryLogin(string userinfoString, out User? user)
		{
			user = null;
			string[] splits = userinfoString.Split('-');
			byte[] idBytes = Encoding.UTF8.GetBytes(splits[0]);
			if (splits.Length != 2 || idBytes.Length != 4) return false;
			int userId = BitConverter.ToInt32(idBytes);
			var foundUser = Users.FindById(userId);
			if (foundUser is null) return false;
			string hash = splits[1];
			if (PasswordHash.Compare(hash, foundUser.PasswordHash))
			{
				user = foundUser;
			}
			return true;
		}

		public User? TryCreateUser(string username, string password, string firstName, string middleName, string lastName, string email)
		{
			if (Users.Exists(x => x.Username == username || x.Email == email))
			{
				return null;
			}
			int id = Users.Insert(new User(0, firstName, lastName, middleName, email, PasswordHash.Hash(password), username));
			return Users.FindById(id);
		}
	}
}
