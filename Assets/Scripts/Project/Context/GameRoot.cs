using strange.extensions.context.impl;

namespace Assets.Scripts.Project.Context
{
	public class GameRoot : ContextView
	{
	
		void Awake()
		{
			//Instantiate the context, passing it this instance.
			context = new GameContext(this);
		}
	}
}
