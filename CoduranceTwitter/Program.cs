using System;
using System.Collections.Generic;
using CoduranceTwitter.Interfaces;
using CoduranceTwitter.Data;

namespace CoduranceTwitter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			IUserRepository users = new UserRepository ();
			ICoduranceFeedController feedController = new CoduranceFeedController (users);

			Console.WriteLine ("         Welcome to Codurance Feed!!");
			Console.WriteLine ("----------------------------------------------");
			Console.WriteLine ("Please type one of the following commands:");
			Console.WriteLine (" - Post: <username> -> <message>");
			Console.WriteLine (" - Follow: <username> follows <other user>");
			Console.WriteLine (" - Read: <username>");
			Console.WriteLine (" - Wall: <username> wall");
			while (true) 
			{
				Console.Write ("> ");
				string command = Console.ReadLine ();
				IList<string> messages = feedController.DoAction (command);
				foreach (string message in messages) 
				{
					Console.WriteLine (message);
				}
			}
		}
	}
}
