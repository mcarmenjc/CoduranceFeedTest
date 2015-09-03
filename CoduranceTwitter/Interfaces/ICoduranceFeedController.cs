using System;
using System.Collections.Generic;

namespace CoduranceTwitter.Interfaces
{
	public interface ICoduranceFeedController
	{
		IList<string> DoAction(string command);
	}
}

