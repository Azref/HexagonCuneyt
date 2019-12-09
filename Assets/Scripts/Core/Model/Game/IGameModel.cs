namespace Assets.Scripts.Core.Model.Game
{
    public interface IGameModel
    {
        RV_GameStatus Status { get; }

        RV_GameInfo Info { get; }

        RV_Int Score { get; set; }

        void Clear();
    }
}