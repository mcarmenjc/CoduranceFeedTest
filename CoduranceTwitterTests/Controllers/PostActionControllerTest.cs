using System;
using NUnit.Framework;
using CoduranceTwitter;
using Moq;
using CoduranceTwitter.Interfaces;
using CoduranceTwitter.Entities;
using System.Collections.Generic;
using CoduranceTwitter.Controllers;

namespace CoduranceTwitterTests
{
	[TestFixture()]
	public class PostActionControllerTest
	{
		private PostActionController _postController;
		private Mock<IUserRepository> _userRepositoryMock;
		[SetUp()]
		public void Init()
		{
			User user = new User ("mcarmen");
			_userRepositoryMock = new Mock<IUserRepository> ();
			_userRepositoryMock.Setup (x => x.GetUser (user.Name)).Returns (user);
			_postController = new PostActionController (_userRepositoryMock.Object);
		}

		[Test()]
		public void NewMessageShouldBePosted ()
		{
			const string userName = "mcarmen";
			string command = string.Format("{0} -> First message!", userName);
			_postController.DoAction (command);
			User user = _postController.UserRepository.GetUser (userName);
			Assert.AreEqual (1, user.GetAllPosts().Count);
		}
	}
}

