using System.Collections.Generic;

namespace Assets.Scripts.Core.Manager.Screen
{
    public interface IScreenModel
    {
        /// <summary>
        /// List of ignored panels on history
        /// </summary>
        List<string> IgnoreHistory { get; set; }

        /// <summary>
        /// Panels list on history
        /// </summary>
        List<IPanelVo> History { get; set; }

        /// <summary>
        /// Get all history data
        /// </summary>
        string GetHistoryData();
    }
}