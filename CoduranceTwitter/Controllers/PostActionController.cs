using System;
using CoduranceTwitter.Interfaces;
using System.Text.RegularExpressions;
using CoduranceTwitter.Entities;

namespace CoduranceTwitter.Controllers
{
	public class PostActionController : FeedActionController, IFeedActionController
	{
		public PostActionController (IUserRepository userRepository) : base(userRepository)
		{
		}

		public override void DoAction(string command)
		{
			Tuple<string, string> userMessage = GetUserAndMessageFromCommand (command);
			User user = UserRepository.GetUser( userMessage.Item1);
			user.Post (userMessage.Item2);
		}

		private Tuple<string, string> GetUserAndMessageFromCommand(string command)
		{
			Tuple<string, string> userMessage = new Tuple<string, string> (string.Empty, string.Empty);
			string pat = @"(.*)\s+->\s+(.*)";
			Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
			Match match = regex.Match(command);
			if (match.Success) 
			{
				userMessage = new Tuple<string, string> (match.Groups [1].ToString (), match.Groups [2].ToString ());
			}
			return userMessage;
		}
	}
}

