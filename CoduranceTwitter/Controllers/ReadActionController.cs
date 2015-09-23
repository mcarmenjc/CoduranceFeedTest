using System;
using CoduranceTwitter.Interfaces;
using CoduranceTwitter.Entities;
using System.Collections.Generic;
using System.Linq;

namespace CoduranceTwitter.Controllers
{
	public class ReadActionController : FeedActionController, IFeedActionController
	{
		public ReadActionController (IUserRepository userRepository) : base(userRepository)
		{
		}

		public override void DoAction(string command)
		{
			User user = UserRepository.GetUser (command);
			foreach (Message message in user.Messages.OrderByDescending(x => x.Timestamp)) 
			{
				message.Print ();
			}
		}
	}
}

