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
			User user = new User () {
				Name = "mcarmen",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			user.Messages.Add (new Message () 
				{
					Text = "Message",
					Timestamp = DateTime.Now.AddSeconds(-30),
					Owner = user.Name
				});
			User otherUser = new User () {
				Name = "rob",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			User anotherUser = new User () {
				Name = "arthur",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			anotherUser.Messages.Add (new Message () 
				{
					Text = "1 Message",
					Timestamp = DateTime.Now.AddSeconds(-30),
					Owner = user.Name
				});
			anotherUser.Messages.Add (new Message () 
				{
					Text = "2 Message",
					Timestamp = DateTime.Now.AddSeconds(-20),
					Owner = user.Name
				});
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

