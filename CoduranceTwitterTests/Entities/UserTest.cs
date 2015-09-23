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
			User user = new User ("mcarmen");
			User followingUser = new User ("rob");
			user.Follow (followingUser);
			Assert.AreEqual (1, user.Followings.Count);
		}

		[Test()]
		public void NewMessageShouldBeAddedToMessageListWhenPosting ()
		{
			User user = new User ("mcarmen");
			user.Post ("Hello");
			Assert.AreEqual (1, user.Messages.Count);
		}
	}
}

