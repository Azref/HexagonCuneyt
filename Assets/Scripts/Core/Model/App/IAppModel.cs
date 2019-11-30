namespace Assets.Scripts.Core.Model.App
{
    /// <summary>
    /// app related datas
    /// </summary>
    public interface IAppModel
    {
        /// <summary>
        /// App status object
        /// </summary>
        RV_AppStatus Status { get; }

        /// <summary>
        /// Clear function
        /// </summary>
        void Clear();
    }
}