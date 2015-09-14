using System;
using NUnit.Framework;
using System.IO;
using CoduranceTwitter.Entities;

namespace CoduranceTwitterTests
{
	[TestFixture()]
	public class MessageTest
	{
		private StringWriter _standardOut;

		[SetUp()]
		public void Init()
		{
			_standardOut = new StringWriter ();
			Console.SetOut (_standardOut);
		}

		[Test()]
		public void PrintedMessageShouldHaveCorrectFormat()
		{
			Message message = new Message () 
			{
				Text = "Message",
				Timestamp = DateTime.Now.AddSeconds (-30)
			};
			message.Print ();
			string expected = string.Format("Message (30 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}

		public void PrintedMessageShouldPrintSecondsPassed()
		{
			Message message = new Message () 
			{
				Text = "Message",
				Timestamp = DateTime.Now.AddSeconds (-30)
			};
			message.Print ();
			string expected = string.Format("Message (30 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}

		public void PrintedMessageShouldPrintMinutesPassed()
		{
			Message message = new Message () 
			{
				Text = "Message",
				Timestamp = DateTime.Now.AddMinutes (-5)
			};
			message.Print ();
			string expected = string.Format("Message (5 minutes ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}

		public void PrintedMessageShouldPrintHoursPassed()
		{
			Message message = new Message () 
			{
				Text = "Message",
				Timestamp = DateTime.Now.AddHours (-2)
			};
			message.Print ();
			string expected = string.Format("Message (2 hours ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}
	}
}

