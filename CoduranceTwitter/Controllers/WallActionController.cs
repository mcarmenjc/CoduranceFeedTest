using System;
using CoduranceTwitter.Interfaces;

namespace CoduranceTwitter
{
	public class WallActionController : FeedActionController, IFeedActionController
	{
		public WallActionController (IUserRepository userRepository) : base(userRepository)
		{
		}

		public override void DoAction(string command)
		{
		}
	}
}

