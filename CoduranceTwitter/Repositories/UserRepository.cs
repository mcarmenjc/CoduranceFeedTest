﻿using System;
using System.Linq;
using CoduranceTwitter.Entities;
using System.Collections.Generic;
using CoduranceTwitter.Interfaces;

namespace CoduranceTwitter.Data
{
	public class UserRepository : IUserRepository
	{
		private IList<User> _users;

		public UserRepository ()
		{
			_users = new List<User> ();
		}

		private void AddUser(string name)
		{
			User newUser = new User (name);
			_users.Add (newUser);
		}

		public User GetUser(string name)
		{
			if (!DoesUserExists(name))
			{
				AddUser (name);
			}
			return _users.FirstOrDefault (x => x.Name == name);
		}

		public bool DoesUserExists (string name)
		{
			return _users.Any (x => x.Name == name);
		}
	}
}

