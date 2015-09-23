using System;
using System.Collections.Generic;
using System.Linq;

namespace CoduranceTwitter.Entities
{
	public class User
	{
		public string Name { get; private set; }
		public IList<Message> Messages { get; private set;}
		public IList<string> Followings { get; private set;}

		public User(string name)
		{
			Name = name;
			Messages = new List<Message> ();
			Followings = new List<string> ();
		}

		public void Follow(User user)
		{
			if (!Followings.Any (x => x == user.Name)) 
			{
				Followings.Add (user.Name);
			}
		}

		public void Post(string message)
		{
			Message postedMessage = new Message (message, Name);
			Messages.Add (postedMessage);
		}

		public void Post(string message, DateTime timestamp)
		{
			Message postedMessage = new Message (message, Name, timestamp);
			Messages.Add (postedMessage);
		}
	}
}

