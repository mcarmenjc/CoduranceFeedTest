using System;
using CoduranceTwitter.Entities;

namespace CoduranceTwitter.Interfaces
{
	public interface IUserRepository
	{
		void AddUser(string name);
		User GetUser(string name);
		bool DoesUserExists (string name);
	}
}

