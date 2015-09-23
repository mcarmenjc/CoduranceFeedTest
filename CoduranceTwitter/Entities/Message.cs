using System;

namespace CoduranceTwitter.Entities
{
	public class Message
	{
		public string Text { get; private set; }
		public DateTime Timestamp { get; private set; }
		public string Owner { get; private set; }

		public Message(string text, string owner)
		{
			Text = text;
			Owner = owner;
			Timestamp = DateTime.Now;
		}

		public Message(string text, string owner, DateTime timestamp)
		{
			Text = text;
			Owner = owner;
			Timestamp = timestamp;
		}

		public void Print()
		{
			Console.WriteLine (string.Format ("{0} ({1})", Text, GetPassedTimeString (Timestamp)));
		}

		public void PrintInWall()
		{
			Console.WriteLine (string.Format ("{0} - {1} ({2})", Owner, Text, GetPassedTimeString (Timestamp)));
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
	}
}

