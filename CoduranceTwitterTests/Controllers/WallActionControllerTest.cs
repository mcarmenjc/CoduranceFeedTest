using System;
using NUnit.Framework;
using CoduranceTwitter;
using Moq;
using CoduranceTwitter.Interfaces;
using System.IO;
using CoduranceTwitter.Entities;
using System.Collections.Generic;
using CoduranceTwitter.Controllers;

namespace CoduranceTwitterTests
{
	[TestFixture()]
	public class WallActionControllerTest
	{
		private WallActionController _wallController;
		private Mock<IUserRepository> _userRepositoryMock;
		private StringWriter _standardOut;

		[SetUp()]
		public void Init()
		{
			_standardOut = new StringWriter ();
			Console.SetOut (_standardOut);
			User user = new User ("mcarmen");
			User anotherUser = new User ("arthur");
			anotherUser.Post("First message", DateTime.Now.AddSeconds(-30));
			User followingUser = new User ("rob");
			followingUser.Post("First message from following user",
				DateTime.Now.AddSeconds(-20));
			anotherUser.Follow(followingUser);
			_userRepositoryMock = new Mock<IUserRepository> ();
			_userRepositoryMock.Setup (x => x.GetUser (user.Name)).Returns (user);
			_userRepositoryMock.Setup (x => x.GetUser (anotherUser.Name)).Returns (anotherUser);
			_userRepositoryMock.Setup (x => x.GetUser (followingUser.Name)).Returns (followingUser);
			_wallController = new WallActionController (_userRepositoryMock.Object);
		}

		[Test()]
		public void WallActionShouldDisplayNothingIfNoMessagesPublished ()
		{
			string command = "mcarmen wall";
			_wallController.DoAction (command);
			Assert.IsNullOrEmpty (_standardOut.ToString());
		}

		[Test()]
		public void WallActionShouldDisplayAllUserMessages ()
		{
			string command = "rob wall";
			_wallController.DoAction (command);
			string expected = string.Format ("rob - First message from following user (20 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString());
		}

		[Test()]
		public void WallActionShouldShowUserAndFollowingsMessagesOrderedByTime ()
		{
			string command = "arthur wall";
			_wallController.DoAction (command);
			string expected = string.Format ("rob - First message from following user (20 seconds ago){0}arthur - First message (30 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString());
		}
	}
}

