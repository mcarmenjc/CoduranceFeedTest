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
					actionOutput = DoSeeWall (command);
					break;
				case Action.Read:
				default:
					actionOutput = DoReadTimeLine (command);
					break;
			}

			return actionOutput;
		}

		private IList<string> DoSeeWall(string command)
		{
			IList<string> messages = new List<string> ();
			string pat = @"(.*)\s+wall";
			Regex regex = new Regex(pat, RegexOptions.IgnoreCase);
			Match match = regex.Match(command);
			if (match.Success) 
			{
				User user = _userRepository.GetUser( match.Groups[1].ToString());
				List<Message> messagesToShow = GetWallMessages (user);
				messages = GetAllDisplayedWallMessages (messagesToShow);
			}
			return messages;
		}

		private IList<string> GetAllDisplayedWallMessages(List<Message> messagesToShow)
		{
			IList<string> messages = new List<string> ();
			foreach (Message message in messagesToShow.OrderByDescending(x => x.Timestamp)) 
			{
				messages.Add (GetDisplayedWallMessage(message));
			}
			return messages;
		}

		private string GetDisplayedWallMessage(Message message)
		{
			return string.Format("{0} - {1} ({2})", message.Owner, message.Text, GetPassedTimeString(message.Timestamp));
		}

		private List<Message> GetWallMessages (User user)
		{
			List<Message> messagesToShow = new List<Message> ();
			messagesToShow.AddRange (user.Messages);
			foreach (string userName in user.Followings) 
			{
				User followingUser = _userRepository.GetUser (userName);
				messagesToShow.AddRange (followingUser.Messages);
			}
			return messagesToShow;
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
					Timestamp = DateTime.Now,
					Owner = user.Name
				});
			}
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
			foreach (Message message in user.Messages.OrderByDescending(x => x.Timestamp)) 
			{
				userTimeLine.Add (GetMessage (message));
			}
			return userTimeLine;
		}

		private string GetMessage(Message message)
		{			
			return string.Format ("{0} ({1})", message.Text, GetPassedTimeString(message.Timestamp));
		}

		private string GetPassedTimeString(DateTime messageTimeStamp)
		{
			TimeSpan span = DateTime.Now.Subtract ( messageTimeStamp );
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

			return passedTime;
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

