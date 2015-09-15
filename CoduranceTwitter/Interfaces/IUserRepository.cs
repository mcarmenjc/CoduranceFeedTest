using System;
using CoduranceTwitter.Entities;

namespace CoduranceTwitter.Interfaces
{
	public interface IUserRepository
	{
		User GetUser(string name);
		bool DoesUserExists (string name);
	}
}

