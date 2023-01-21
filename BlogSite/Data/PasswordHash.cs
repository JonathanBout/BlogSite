using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace BlogSite.Data
{
	public class PasswordHash
	{
		private const int SaltSize = 16;
		private const int HashSize = 128;
		private readonly string _passwordHash;
		
		private PasswordHash(string hash)
		{
			_passwordHash = hash;
		}

		public static PasswordHash Hash(string password)
		{
			byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA512);
			byte[] hash = pbkdf2.GetBytes(HashSize);
			byte[] hashBytes = new byte[SaltSize + HashSize];
			Array.Copy(salt, 0, hashBytes, 0, SaltSize);
			Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
			return new PasswordHash(Convert.ToBase64String(hashBytes));
		}

		public static bool Compare(string hash, string password)
		{
			return new PasswordHash(hash).Compare(password);
		}

		public bool Compare(string password)
		{
			byte[] hashBytes = Convert.FromBase64String(_passwordHash);
			if (hashBytes.Length != HashSize + SaltSize) { return false; }
			
			byte[] salt = new byte[SaltSize];
			Array.Copy(hashBytes, 0, salt, 0, SaltSize);

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA512);
			byte[] hash = pbkdf2.GetBytes(HashSize);
			
			for (int i = 0; i < HashSize; i++)
			{
				if (hashBytes[i + SaltSize] != hash[i]) 
				{
					return false;
				}
			}
			return true;
		}

		public static implicit operator string(PasswordHash passwordHash)
		{
			return passwordHash._passwordHash;
		}
	}
}