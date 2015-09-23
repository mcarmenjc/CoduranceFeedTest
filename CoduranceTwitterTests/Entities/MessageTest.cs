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
			Message message = new Message ("Message",
				"mcarmen",
				DateTime.Now.AddSeconds (-30));
			message.Print ();
			string expected = string.Format("Message (30 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}

		[Test()]
		public void PrintedMessageShouldPrintSecondsPassed()
		{
			Message message = new Message ("Message",
				"mcarmen",
				DateTime.Now.AddSeconds (-30));
			message.Print ();
			string expected = string.Format("Message (30 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}

		[Test()]
		public void PrintedMessageShouldPrintMinutesPassed()
		{
			Message message = new Message ("Message",
				"mcarmen",
				DateTime.Now.AddMinutes (-5));
			message.Print ();
			string expected = string.Format("Message (5 minutes ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}

		[Test()]
		public void PrintedMessageShouldPrintHoursPassed()
		{
			Message message = new Message ("Message",
				"mcarmen",
				DateTime.Now.AddHours (-2));
			message.Print ();
			string expected = string.Format("Message (2 hours ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}

		[Test()]
		public void PrintedWallMessageShouldHaveCorrectFormat()
		{
			Message message = new Message ("Message",
				"mcarmen",
				DateTime.Now.AddSeconds (-30));
			message.PrintInWall ();
			string expected = string.Format("mcarmen - Message (30 seconds ago){0}", Environment.NewLine);
			Assert.AreEqual (expected, _standardOut.ToString ());
		}
	}
}

