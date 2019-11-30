using Assets.Scripts.Core.Manager.Pool;
using strange.extensions.mediation.impl;

namespace Assets.Scripts.Core.View
{
    /// <summary>
    /// If pool access needed in any ...View script. This view should be derived from this script.
    /// </summary>
    public class PoolView : EventView
    {
        /// <summary>
        /// Pool model
        /// </summary>
        [Inject] public IObjectPoolModel pool { get; set; }
    }
}

