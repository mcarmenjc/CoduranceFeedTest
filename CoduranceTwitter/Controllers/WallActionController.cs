using System;
using CoduranceTwitter.Interfaces;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using CoduranceTwitter.Entities;

namespace CoduranceTwitter
{
	public class WallActionController : FeedActionController, IFeedActionController
	{
		public WallActionController (IUserRepository userRepository) : base(userRepository)
		{
		}

		public override void DoAction(string command)
		{
			string userName = GetUserNameFromCommand (command);
			List<Message> messagesToShow = GetWallMessages (userName);
			foreach (Message message in messagesToShow) 
			{
				message.PrintInWall ();
			}
		}

		private string GetUserNameFromCommand(string command)
		{
			string userName = string.Empty;
			string pat = @"(.*)\s+wall";
			Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
			Match match = regex.Match(command);
			if (match.Success) 
			{
				userName = match.Groups [1].ToString ();
			}
			return userName;
		}

		private List<Message> GetWallMessages(string userName)
		{
			User user = UserRepository.GetUser (userName);
			List<Message> messagesToShow = new List<Message> ();
			messagesToShow.AddRange (user.Messages);
			foreach (string name in user.Followings) 
			{
				User followingUser = UserRepository.GetUser (name);
				messagesToShow.AddRange (followingUser.Messages);
			}
			return messagesToShow;
		}
	}
}

