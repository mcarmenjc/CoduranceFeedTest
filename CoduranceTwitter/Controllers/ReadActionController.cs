using System;
using CoduranceTwitter.Interfaces;

namespace CoduranceTwitter
{
	public class ReadActionController : FeedActionController, IFeedActionController
	{
		public ReadActionController (IUserRepository userRepository) : base(userRepository)
		{
		}

		public override void DoAction(string command)
		{
		}
	}
}

