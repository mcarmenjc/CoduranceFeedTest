using System;
using NUnit.Framework;
using System.IO;
using CoduranceTwitter;
using Moq;
using CoduranceTwitter.Interfaces;
using CoduranceTwitter.Entities;
using System.Collections.Generic;
using CoduranceTwitter.Controllers;

namespace CoduranceTwitterTests
{
	[TestFixture ()]
	public class ReadActionControllerTest
	{
		private ReadActionController _readController;
		private Mock<IUserRepository> _userRepositoryMock;
		private StringWriter _standardOut;

		[SetUp()]
		public void Init()
		{
			_standardOut = new StringWriter ();
			Console.SetOut (_standardOut);
			User user = new User ("mcarmen");
			user.Post ("Message",
					DateTime.Now.AddSeconds(-30));
			User otherUser = new User ("rob");
			User anotherUser = new User ("arthur");
			anotherUser.Post ("1 Message",
					DateTime.Now.AddSeconds(-30));
			anotherUser.Post ("2 Message",
						DateTime.Now.AddSeconds(-20));
			_userRepositoryMock = new Mock<IUserRepository> ();
			_userRepositoryMock.Setup (x => x.GetUser (user.Name)).Returns (user);
			_userRepositoryMock.Setup (x => x.GetUser (otherUser.Name)).Returns (otherUser);
			_userRepositoryMock.Setup (x => x.GetUser (anotherUser.Name)).Returns (anotherUser);
			_readController = new ReadActionController (_userRepositoryMock.Object);
		}

		[Test()]
		public void UserMessagesShouldBePrinted()
		{
			const string command = "mcarmen";
			_readController.DoAction (command);
			string expected = string.Format("Message (30 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString());
		}

		[Test()]
		public void NothingShouldBePrintedIfUserHasNoMessages()
		{
			const string command = "rob";
			_readController.DoAction (command);
			Assert.IsNullOrEmpty (_standardOut.ToString());
		}

		[Test()]
		public void UserMessagesShouldBePrintedInTheCorrectOrder()
		{
			const string command = "arthur";
			_readController.DoAction (command);
			string expected = string.Format ("2 Message (20 seconds ago){0}1 Message (30 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}
	}
}

