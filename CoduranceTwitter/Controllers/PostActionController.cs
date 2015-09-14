using System;
using CoduranceTwitter.Interfaces;

namespace CoduranceTwitter
{
	public class PostActionController : FeedActionController, IFeedActionController
	{
		public PostActionController (IUserRepository userRepository) : base(userRepository)
		{
		}

		public override void DoAction(string command)
		{
		}
	}
}

