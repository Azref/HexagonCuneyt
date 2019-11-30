namespace Assets.Scripts.Core.Manager.Screen
{
    /// <summary>
    /// Screen manager related events
    /// </summary>
    public enum ScreenEvent
    {
        ShowLoader,
        ShowError,
        HideLoader,
        OpenPanel,
        Back,
        FocusChanged,
        PauseChanged,
        AppQuit,
        Initialize,
        Block,
        UnBlock,
        ClearLayer
    }
}