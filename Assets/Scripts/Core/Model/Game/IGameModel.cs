namespace Assets.Scripts.Core.Model.Game
{
    public interface IGameModel
    {
        RV_GameStatus Status { get; }

        RV_GameInfo Info { get; }

        void Clear();
    }
}