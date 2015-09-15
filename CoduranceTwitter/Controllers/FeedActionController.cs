using System;
using CoduranceTwitter.Interfaces;

namespace CoduranceTwitter.Controllers
{
	public class FeedActionController : IFeedActionController
	{
		private IUserRepository _userRepository;

		public FeedActionController (IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public IUserRepository UserRepository 
		{
			get { return _userRepository; }
		}

		public virtual void DoAction (string command)
		{
		}
	}
}

