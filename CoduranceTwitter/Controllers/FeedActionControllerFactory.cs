using System;
using CoduranceTwitter.Interfaces;

namespace CoduranceTwitter.Controllers
{
	public class FeedActionControllerFactory
	{
		private ReadActionController _readActionController;
		private PostActionController _postActionController;
		private FollowActionController _followActionController;
		private WallActionController _wallActionController;

		public FeedActionControllerFactory(IUserRepository repository)
		{
			_postActionController = new PostActionController (repository);
			_followActionController = new FollowActionController (repository);
			_wallActionController = new WallActionController (repository);
			_readActionController = new ReadActionController (repository);
		}

		public FeedActionController GetFeedActionController(string command)
		{
			if (command.Contains (" -> ")) 
			{
				return _postActionController;
			}
			if (command.Contains (" follows ")) 
			{
				return _followActionController;
			}
			if (command.Contains (" wall")) 
			{
				return _wallActionController;
			}
			return _readActionController;
		}
	}
}

