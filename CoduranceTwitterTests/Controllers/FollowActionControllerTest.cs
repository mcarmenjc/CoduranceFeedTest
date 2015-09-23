using System;
using NUnit.Framework;
using CoduranceTwitter;
using CoduranceTwitter.Interfaces;
using Moq;
using System.Linq;
using CoduranceTwitter.Entities;
using System.Collections.Generic;
using CoduranceTwitter.Controllers;

namespace CoduranceTwitterTests
{
	[TestFixture ()]
	public class FollowActionControllerTest
	{
		private FollowActionController _followController;
		private Mock<IUserRepository> _userRepositoryMock;

		[SetUp()]
		public void Init()
		{
			User firstUser = new User ("mcarmen");
			User secondUser = new User ("rob");
			User thirdUser = new User ("arthur");
			firstUser.Follow (secondUser);
			_userRepositoryMock = new Mock<IUserRepository> ();
			_userRepositoryMock.Setup (x => x.GetUser (firstUser.Name)).Returns (firstUser);
			_userRepositoryMock.Setup (x => x.GetUser (secondUser.Name)).Returns (secondUser);
			_userRepositoryMock.Setup (x => x.GetUser (thirdUser.Name)).Returns (thirdUser);
			_followController = new FollowActionController (_userRepositoryMock.Object);
		}

		[Test()]
		public void FollowingUserListShouldBeUpdatedWhenFollowCommandIsExecuted ()
		{
			const string firstUser = "mcarmen";
			const string secondUser = "arthur";
			string command = string.Format("{0} follows {1}", firstUser, secondUser);
			_followController.DoAction (command);
			Assert.IsTrue (_followController.UserRepository.GetUser(firstUser).Followings.Any(x => x == secondUser));
		}

		[Test()]
		public void FollowingUserListShouldNotBeUpdatedWhenUserIsAlreadyFollowed ()
		{
			const string firstUser = "mcarmen";
			const string secondUser = "rob";
			string command = string.Format("{0} follows {1}", firstUser, secondUser);
			_followController.DoAction (command);
			Assert.AreEqual (1, _followController.UserRepository.GetUser(firstUser).Followings.Count);
		}
	}
}

