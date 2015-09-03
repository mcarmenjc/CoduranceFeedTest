using System;
using System.Collections.Generic;

namespace CoduranceTwitter.Entities
{
	public class User
	{
		public string Name { get; set; }
		public IList<Message> Messages { get; set; }
		public IList<string> Followings { get; set; }
	}
}

