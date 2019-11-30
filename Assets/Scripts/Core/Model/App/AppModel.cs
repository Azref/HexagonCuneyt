using UnityEngine;

namespace Assets.Scripts.Core.Model.App
{
    public class AppModel : IAppModel
    {
        private RV_AppStatus _status;

        [PostConstruct]
        public void OnPostConstruct()
        {
            _status = Resources.Load<RV_AppStatus>("RuntimeVariables/AppStatus");

        }

        public RV_AppStatus Status
        {
            get
            {
                if (_status == null)
                    OnPostConstruct();

                return _status;
            }
        }

        public void Clear()
        {
        }
    }
}