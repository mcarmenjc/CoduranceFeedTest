using System;

namespace CoduranceTwitter.Entities
{
	public class Message
	{
		public string Text { get; set; }
		public DateTime Timestamp { get; set; }
		public string Owner { get; set; }

		public void Print()
		{
			Console.WriteLine (string.Format ("{0} ({1})", Text, GetPassedTimeString (Timestamp)));
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

