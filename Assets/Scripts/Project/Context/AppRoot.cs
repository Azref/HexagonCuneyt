using strange.extensions.context.impl;

namespace Assets.Scripts.Project.Context
{
	public class AppRoot : ContextView
	{
	
		void Awake()
		{
			//Instantiate the context, passing it this instance.
			context = new AppContext(this);
		}
	}
}
