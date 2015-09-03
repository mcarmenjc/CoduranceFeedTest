using System;
using CoduranceTwitter.Interfaces;
using CoduranceTwitter.Entities;
using System.Collections.Generic;
using CoduranceTwitter.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoduranceTwitter.Controllers
{
	public class CoduranceFeedController : ICoduranceFeedController
	{
		public enum Action { 
			Post, 
			Read, 
			Follow, 
			Wall
		};

		private IUserRepository _userRepository;

		public CoduranceFeedController (IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public IList<string> DoAction(string command)
		{
			IList<string> actionOutput = new List<string> ();
			Action action = GetAction(command);
			switch (action) 
			{
				case Action.Follow:
					DoFollow (command);
					break;
				case Action.Post:
					DoPostMessage (command);
					break;
				case Action.Wall:
					DoSeeWall (command);
					break;
				case Action.Read:
				default:
					actionOutput = DoReadTimeLine (command);
					break;
			}

			return actionOutput;
		}

		private void DoFollow(string command)
		{
			string pat = @"(.*)\s+follows\s+(.*)";
			Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
			Match match = regex.Match(command);
			if (match.Success) 
			{
				User firstUser = _userRepository.GetUser( match.Groups[1].ToString());
				User secondUser = _userRepository.GetUser( match.Groups[2].ToString());
				if (!firstUser.Followings.Any (x => x == secondUser.Name)) 
				{
					firstUser.Followings.Add (secondUser.Name);
				}
			}
		}

		private void DoPostMessage(string command)
		{
			string pat = @"(.*)\s+->\s+(.*)";
			Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
			Match match = regex.Match(command);
			if (match.Success) 
			{
				User user = _userRepository.GetUser( match.Groups[1].ToString());
				string message = match.Groups[2].ToString();
				user.Messages.Add (new Message (){ 
					Text = message,
					Timestamp = DateTime.Now
				});
			}
		}

		private void DoSeeWall(string command)
		{
		}

		private IList<string> DoReadTimeLine(string userName)
		{
			User user = _userRepository.GetUser (userName);
			IList<string> userTimeLine = GetTimeLineMessages (user);
			return userTimeLine;
		}

		private IList<string> GetTimeLineMessages(User user)
		{
			IList<string> userTimeLine = new List<string> ();
			foreach (Message message in user.Messages) 
			{
				userTimeLine.Add (GetMessage (message));
			}
			return userTimeLine;
		}

		private string GetMessage(Message message)
		{
			TimeSpan span = DateTime.Now.Subtract ( message.Timestamp );
			string passedTime = string.Empty;
			if (span.Hours > 0) {
				passedTime = string.Format ("{0} hours ago", span.Hours);
			} 
			else 
			{
				if (span.Minutes > 0) {
					passedTime = string.Format ("{0} minutes ago", span.Minutes);
				} 
				else 
				{
					passedTime = string.Format ("{0} seconds ago", span.Seconds);
				}
			}
			return string.Format ("{0} ({1})", message.Text, passedTime);
		}

		internal Action GetAction(string command)
		{
			if (command.Contains ("->")) 
			{
				return Action.Post;
			}
			if (command.Contains ("follows")) 
			{
				return Action.Follow;
			}
			if (command.Contains ("wall")) 
			{
				return Action.Wall;
			}
			return Action.Read;
		}
	}
}

