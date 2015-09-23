using System;
using System.Collections.Generic;
using System.Linq;

namespace CoduranceTwitter.Entities
{
	public class User
	{
		public string Name { get; private set; }
		public IList<Message> _messages;
		public IList<string> _followings;

		public User(string name)
		{
			Name = name;
			_messages = new List<Message> ();
			_followings = new List<string> ();
		}

		public void Follow(User user)
		{
			if (!_followings.Any (x => x == user.Name)) 
			{
				_followings.Add (user.Name);
			}
		}

		public IList<string> GetAllFollowers()
		{
			return _followings;
		}

		public void Post(string message)
		{
			Message postedMessage = new Message (message, Name);
			_messages.Add (postedMessage);
		}

		public void Post(string message, DateTime timestamp)
		{
			Message postedMessage = new Message (message, Name, timestamp);
			_messages.Add (postedMessage);
		}

		public IList<Message> GetAllPosts()
		{
			return _messages;
		}
	}
}

