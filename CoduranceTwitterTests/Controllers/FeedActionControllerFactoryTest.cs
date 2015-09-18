using System;
using NUnit.Framework;

using CoduranceTwitter.Controllers;
using CoduranceTwitter.Interfaces;
using CoduranceTwitter.Data;

namespace CoduranceTwitterTests
{
	[TestFixture ()]
	public class FeedActionControllerFactoryTest
	{
		private FeedActionControllerFactory _factory;

		[SetUp()]
		public void Init()
		{
			IUserRepository repository = new UserRepository ();	
			_factory = new FeedActionControllerFactory (repository);
		}

		[Test()]
		public void PostControllerShouldBeReturned ()
		{
			const string command = "user -> message";
			FeedActionController controller = _factory.GetFeedActionController (command);
			Assert.IsInstanceOf<PostActionController> (controller);
		}

		[Test()]
		public void FollowControllerShouldBeReturned ()
		{
			const string command = "user follows another";
			FeedActionController controller = _factory.GetFeedActionController (command);
			Assert.IsInstanceOf<FollowActionController> (controller);
		}

		[Test()]
		public void WallControllerShouldBeReturned ()
		{
			const string command = "user wall";
			FeedActionController controller = _factory.GetFeedActionController (command);
			Assert.IsInstanceOf<WallActionController> (controller);
		}

		[Test()]
		public void ReadControllerShouldBeReturned ()
		{
			const string command = "user";
			FeedActionController controller = _factory.GetFeedActionController (command);
			Assert.IsInstanceOf<ReadActionController> (controller);
		}
	}
}

