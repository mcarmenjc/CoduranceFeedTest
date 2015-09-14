﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CoduranceTwitter.Entities
{
	public class User
	{
		public string Name { get; set; }
		public IList<Message> Messages { get; set; }
		public IList<string> Followings { get; set; }

		public void Follow(User user)
		{
			if (!Followings.Any (x => x == user.Name)) 
			{
				Followings.Add (user.Name);
			}
		}
	}
}

