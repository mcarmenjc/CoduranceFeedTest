using System;
using CoduranceTwitter.Interfaces;
using System.Text.RegularExpressions;
using CoduranceTwitter.Entities;

namespace CoduranceTwitter.Controllers
{
	public class FollowActionController : FeedActionController, IFeedActionController
	{
		public FollowActionController (IUserRepository userRepository) : base(userRepository)
		{
		}

		public override void DoAction(string command)
		{
			Tuple<string, string> commandParams = GetFollowParams(command);
			User firstUser = UserRepository.GetUser(commandParams.Item1);
			User secondUser = UserRepository.GetUser(commandParams.Item2);
			firstUser.Follow (secondUser);
		}

		private Tuple<string, string> GetFollowParams(string command)
		{
			Tuple<string, string> followParams = new Tuple<string, string> (string.Empty, string.Empty);
			string pat = @"(.*)\s+follows\s+(.*)";
			Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
			Match match = regex.Match(command);
			if (match.Success) 
			{
				followParams = new Tuple<string, string> (match.Groups [1].ToString (), match.Groups [2].ToString ());
			}
			return followParams;
		}
	}
}

