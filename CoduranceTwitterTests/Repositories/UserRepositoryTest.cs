using System;
using NUnit.Framework;
using CoduranceTwitter.Data;
using CoduranceTwitter.Entities;

namespace CoduranceTwitterTests
{
	[TestFixture()]
	public class UserRepositoryTest
	{
		private UserRepository _userRepository;

		[SetUp()]
		public void Init()
		{
			_userRepository = new UserRepository ();
			_userRepository.GetUser ("andy");
		}

		[Test()]
		public void UserExistanceShouldBeCorrectlyChecked()
		{
			Assert.IsFalse (_userRepository.DoesUserExists ("mcarmen"));
		}

		[Test()]
		public void GetUserShouldCreateANewOneIfItDoesNotExist()
		{
			Assert.IsFalse (_userRepository.DoesUserExists ("mcarmen"));
			_userRepository.GetUser ("mcarmen");
			Assert.IsTrue (_userRepository.DoesUserExists ("mcarmen"));
		}

		[Test()]
		public void GetUserShouldReturnAnExistingUser()
		{
			User andy = _userRepository.GetUser ("andy");
			Assert.AreEqual ("andy", andy.Name);
		}
	}
}

