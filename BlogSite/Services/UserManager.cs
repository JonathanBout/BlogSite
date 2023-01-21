using BlogSite.Data;
using LiteDB;
using System.Diagnostics;

namespace BlogSite.Services
{
	public class UserManager
	{
		ILiteCollection<User> Users { get; }
		public UserManager(Database database)
		{
			Users = database.GetCollection<User>();
#if DEBUG
			Users.DeleteAll();
			var generatedUsers = new User[Random.Shared.Next(11, 100)];
			Console.WriteLine("Generating {0} users with a password...", generatedUsers.Length);
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < generatedUsers.Length; i++)
			{
				Console.WriteLine("\tGenerating user {0}...", i);
				generatedUsers[i] = new User(
					0,
					Random.Shared.Word(),
					Random.Shared.Word(),
					Random.Shared.Word(),
					Random.Shared.Word() + "@" + Random.Shared.Word() + ".com",
					PasswordHash.Hash("hello"));
			}
			Console.WriteLine("Done in {0:n2} milliseconds. ({1:n2} milliseconds per item)", sw.Elapsed.TotalMilliseconds, (double)sw.Elapsed.TotalMilliseconds/generatedUsers.Length);
			Users.InsertBulk(generatedUsers);
#endif
		}

		public User[] GetUsers(string? searchText = null)
		{
			var query = Users.Query();
			if (!string.IsNullOrEmpty(searchText))
				query = query.Where(x => x.ToString().ToLower().Contains(searchText.ToLower()));
			return query.ToArray();
		}

		public User GetUser(int id)
		{
			return Users.FindById(id);
		}
	}
}
