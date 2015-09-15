using System;
using NUnit.Framework;
using CoduranceTwitter.Entities;
using System.Collections.Generic;

namespace CoduranceTwitterTests
{
	[TestFixture ()]
	public class UserTest
	{
		[Test()]
		public void UserNameShouldBeAddedToFollowingsUserList ()
		{
			User user = new User ()
			{ 
				Name = "mcarmen",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			User followingUser = new User ()
			{ 
				Name = "rob",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			user.Follow (followingUser);
			Assert.AreEqual (1, user.Followings.Count);
		}

		[Test()]
		public void NewMessageShouldBeAddedToMessageListWhenPosting ()
		{
			User user = new User ()
			{ 
				Name = "mcarmen",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			user.Post ("Hello");
			Assert.AreEqual (1, user.Messages.Count);
		}
	}
}

