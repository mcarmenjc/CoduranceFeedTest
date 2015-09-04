using NUnit.Framework;
using System;
using CoduranceTwitter.Controllers;
using CoduranceTwitter.Data;
using System.Linq;
using System.Collections.Generic;
using CoduranceTwitter.Interfaces;
using CoduranceTwitter.Entities;
using Moq;

namespace CoduranceTwitterTests
{
	[TestFixture ()]
	public class CoduranceFeedControllerTest
	{
		[Test ()]
		public void FollowCommandIsCorrectlyDetected ()
		{
			const string command = "somebody follows anotherbody";
			CoduranceFeedController feedController = new CoduranceFeedController (new UserRepository ());
			CoduranceFeedController.Action action = feedController.GetAction (command);
			Assert.AreEqual (CoduranceFeedController.Action.Follow, action);
		}

		[Test ()]
		public void PostCommandIsCorrectlyDetected ()
		{
			const string command = "somebody -> message";
			CoduranceFeedController feedController = new CoduranceFeedController (new UserRepository ());
			CoduranceFeedController.Action action = feedController.GetAction (command);
			Assert.AreEqual (CoduranceFeedController.Action.Post, action);
		}

		[Test ()]
		public void WallCommandIsCorrectlyDetected ()
		{
			const string command = "somebody wall";
			CoduranceFeedController feedController = new CoduranceFeedController (new UserRepository ());
			CoduranceFeedController.Action action = feedController.GetAction (command);
			Assert.AreEqual (CoduranceFeedController.Action.Wall, action);
		}

		[Test ()]
		public void ReadCommandIsCorrectlyDetected ()
		{
			const string command = "somebody";
			CoduranceFeedController feedController = new CoduranceFeedController (new UserRepository ());
			CoduranceFeedController.Action action = feedController.GetAction (command);
			Assert.AreEqual (CoduranceFeedController.Action.Read, action);
		}

		[Test()]
		public void NewUserShouldBeAddedToUsersList ()
		{
			const string command = "mcarmen";
			IUserRepository userRepository = new UserRepository ();
			CoduranceFeedController feedController = new CoduranceFeedController (userRepository);
			Assert.IsFalse (userRepository.DoesUserExists(command));
			feedController.DoAction (command);
			Assert.IsTrue (userRepository.DoesUserExists(command));
		}

		[Test()]
		public void EmptyTimeLineShouldBeReturnedWhenNewUser ()
		{
			const string command = "mcarmen";
			IUserRepository userRepository = new UserRepository ();
			CoduranceFeedController feedController = new CoduranceFeedController (userRepository);
			IList<string> timeLine = feedController.DoAction (command);
			Assert.AreEqual (0, timeLine.Count);
		}

		[Test()]
		public void EmptyTimeLineShouldBeReturnedWhenUserHasNotPublishedAnything ()
		{
			const string command = "mcarmen";
			User user = new User () {
				Name = command,
				Messages = new List<Message>()
			};
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (It.IsAny<string>())).Returns (user);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			IList<string> timeLine = feedController.DoAction (command);
			Assert.AreEqual (0, timeLine.Count);
		}

		[Test()]
		public void TimeLineShouldBeReturnedWhenUserHasPublishedSomething ()
		{
			const string command = "mcarmen";
			User user = new User () {
				Name = command,
				Messages = new List<Message>()
			};
			user.Messages.Add (new Message (){ 
				Text = "First Message",
				Timestamp = DateTime.Now
			});
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (It.IsAny<string>())).Returns (user);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			IList<string> timeLine = feedController.DoAction (command);
			Assert.AreEqual (1, timeLine.Count);
		}

		[Test()]
		public void TimeLineMessageFormatShouldBeCorrect ()
		{
			const string command = "mcarmen";
			User user = new User () {
				Name = command,
				Messages = new List<Message>()
			};
			user.Messages.Add (new Message (){ 
				Text = "First Message",
				Timestamp = DateTime.Now
			});
			const string expectedMessage = "First Message (0 seconds ago)";
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (It.IsAny<string>())).Returns (user);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			IList<string> timeLine = feedController.DoAction (command);
			Assert.AreEqual (expectedMessage, timeLine.First());
		}

		[Test()]
		public void TimeLineMessagesShouldBeOrderByTimeStamp ()
		{
			const string command = "mcarmen";
			User user = new User () {
				Name = command,
				Messages = new List<Message>()
			};
			user.Messages.Add (new Message (){ 
				Text = "First Message",
				Timestamp = DateTime.Now
			});
			user.Messages.Add (new Message (){ 
				Text = "Second Message",
				Timestamp = DateTime.Now.AddMinutes(-1)
			});
			const string firstExpectedMessage = "First Message (0 seconds ago)";
			const string secondExpectedMessage = "Second Message (1 minutes ago)";
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (It.IsAny<string>())).Returns (user);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			IList<string> timeLine = feedController.DoAction (command);
			Assert.AreEqual (firstExpectedMessage, timeLine.First());
			Assert.AreEqual (secondExpectedMessage, timeLine.ElementAt(1));
		}

		[Test()]
		public void FollowCommandShouldCreateFirstUserIfItDoesNotExist ()
		{
			const string firstUser = "mcarmen";
			const string secondUser = "rob";
			string command = string.Format("{0} follows {1}", firstUser, secondUser);
			IUserRepository userRepository = new UserRepository ();
			CoduranceFeedController feedController = new CoduranceFeedController (userRepository);
			Assert.IsFalse (userRepository.DoesUserExists(firstUser));
			feedController.DoAction (command);
			Assert.IsTrue (userRepository.DoesUserExists(firstUser));
		}

		[Test()]
		public void FollowCommandShouldCreateSecondUserIfItDoesNotExist ()
		{
			const string firstUser = "mcarmen";
			const string secondUser = "rob";
			string command = string.Format("{0} follows {1}", firstUser, secondUser);
			IUserRepository userRepository = new UserRepository ();
			CoduranceFeedController feedController = new CoduranceFeedController (userRepository);
			Assert.IsFalse (userRepository.DoesUserExists(secondUser));
			feedController.DoAction (command);
			Assert.IsTrue (userRepository.DoesUserExists(secondUser));
		}

		[Test()]
		public void FollowingUserListShouldBeUpdatedWhenFollowCommandIsExecuted ()
		{
			const string firstUser = "mcarmen";
			const string secondUser = "rob";
			string command = string.Format("{0} follows {1}", firstUser, secondUser);
			IUserRepository userRepository = new UserRepository ();
			CoduranceFeedController feedController = new CoduranceFeedController (userRepository);
			feedController.DoAction (command);
			Assert.AreEqual (secondUser, userRepository.GetUser(firstUser).Followings.First());
		}

		[Test()]
		public void FollowingUserListShouldNotBeUpdatedWhenUserIsAlreadyFollowed ()
		{
			User firstUser = new User () {
				Name = "mcarmen",
				Followings = new List<string>()
			};
			User secondUser = new User () {
				Name = "rob"
			};
			firstUser.Followings.Add (secondUser.Name);
			string command = string.Format("{0} follows {1}", firstUser.Name, secondUser.Name);
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (firstUser.Name)).Returns (firstUser);
			userRepositoryMock.Setup (x => x.GetUser (secondUser.Name)).Returns (secondUser);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			feedController.DoAction (command);
			Assert.AreEqual (1, firstUser.Followings.Count);
		}

		[Test()]
		public void PostCommandShouldCreateUserIfItDoesNotExist ()
		{
			const string user = "mcarmen";
			string command = string.Format("{0} -> First message!", user);
			IUserRepository userRepository = new UserRepository ();
			CoduranceFeedController feedController = new CoduranceFeedController (userRepository);
			Assert.IsFalse (userRepository.DoesUserExists(user));
			feedController.DoAction (command);
			Assert.IsTrue (userRepository.DoesUserExists(user));
		}

		[Test()]
		public void PostShouldUpdateMessageUserList ()
		{
			User user = new User () {
				Name = "mcarmen",
				Messages = new List<Message>()
			};
			string command = string.Format("{0} -> First message!", user.Name);
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (It.IsAny<string> ())).Returns (user);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			feedController.DoAction (command);
			Assert.AreEqual (1, user.Messages.Count);
		}

		[Test()]
		public void WallCommandShouldCreateUserIfItDoesNotExist ()
		{
			const string user = "mcarmen";
			string command = string.Format("{0} wall", user);
			IUserRepository userRepository = new UserRepository ();
			CoduranceFeedController feedController = new CoduranceFeedController (userRepository);
			Assert.IsFalse (userRepository.DoesUserExists(user));
			feedController.DoAction (command);
			Assert.IsTrue (userRepository.DoesUserExists(user));
		}

		[Test()]
		public void WallCommandShouldReturnEmptyMessageListIfNewUserUser ()
		{
			const string user = "mcarmen";
			string command = string.Format("{0} wall", user);
			IUserRepository userRepository = new UserRepository ();
			CoduranceFeedController feedController = new CoduranceFeedController (userRepository);
			IList<string> wall = feedController.DoAction (command);
			Assert.AreEqual (0, wall.Count);
		}

		[Test()]
		public void WallCommandShouldReturnAllMessagesFromUser ()
		{
			User user = new User (){ 
				Name = "mcarmen",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			user.Messages.Add(new Message(){
				Text = "First message",
				Timestamp = DateTime.Now,
				Owner = user.Name
			});
			string command = string.Format("{0} wall", user.Name);
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (It.IsAny<string> ())).Returns (user);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			IList<string> wall = feedController.DoAction (command);
			Assert.AreEqual (user.Messages.Count, wall.Count);
		}

		[Test()]
		public void WallMessagesShouldHaveBeenFormattedCorrectly ()
		{
			User user = new User (){ 
				Name = "mcarmen",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			user.Messages.Add(new Message(){
				Text = "First message",
				Timestamp = DateTime.Now,
				Owner = user.Name
			});
			string command = string.Format("{0} wall", user.Name);
			string expectedMessage = string.Format ("{0} - {1} (0 seconds ago)", user.Name, user.Messages.First().Text);
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (It.IsAny<string> ())).Returns (user);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			IList<string> wall = feedController.DoAction (command);
			Assert.AreEqual (expectedMessage, wall.First());
		}

		[Test()]
		public void WallCommandShouldReturnFollowingPeopleMessages ()
		{
			User user = new User (){ 
				Name = "mcarmen",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			user.Messages.Add(new Message(){
				Text = "First message",
				Timestamp = DateTime.Now,
				Owner = user.Name
			});
			User followingUser = new User (){ 
				Name = "rob",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			followingUser.Messages.Add(new Message(){
				Text = "First message from following user",
				Timestamp = DateTime.Now,
				Owner = followingUser.Name
			});
			user.Followings.Add (followingUser.Name);
			string command = string.Format("{0} wall", user.Name);
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (user.Name)).Returns (user);
			userRepositoryMock.Setup (x => x.GetUser (followingUser.Name)).Returns (followingUser);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			IList<string> wall = feedController.DoAction (command);
			Assert.AreEqual (user.Messages.Count + followingUser.Messages.Count, wall.Count);
		}

		[Test()]
		public void WallMessagesShouldBeOrderedByTimeStamp ()
		{
			User user = new User (){ 
				Name = "mcarmen",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			user.Messages.Add(new Message(){
				Text = "First message",
				Timestamp = DateTime.Now,
				Owner = user.Name
			});
			User followingUser = new User (){ 
				Name = "rob",
				Followings = new List<string>(),
				Messages = new List<Message>()
			};
			followingUser.Messages.Add(new Message(){
				Text = "First message from following user",
				Timestamp = DateTime.Now.AddMinutes(-1),
				Owner = followingUser.Name
			});
			user.Followings.Add (followingUser.Name);
			string command = string.Format("{0} wall", user.Name);
			string firstMessageExpected = string.Format ("{0} - {1} (0 seconds ago)", user.Name, user.Messages.First().Text);
			string secondMessageExpected = string.Format ("{0} - {1} (1 minutes ago)", followingUser.Name, followingUser.Messages.First().Text);
			Mock<IUserRepository> userRepositoryMock = new Mock<IUserRepository> ();
			userRepositoryMock.Setup (x => x.GetUser (user.Name)).Returns (user);
			userRepositoryMock.Setup (x => x.GetUser (followingUser.Name)).Returns (followingUser);
			CoduranceFeedController feedController = new CoduranceFeedController (userRepositoryMock.Object);
			IList<string> wall = feedController.DoAction (command);
			Assert.AreEqual (firstMessageExpected, wall.First());
			Assert.AreEqual (secondMessageExpected, wall.ElementAt(1));
		}
	}
}

