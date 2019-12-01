namespace Assets.Scripts.Core.Model.Game
{
    public interface IGameModel
    {
        RV_GameStatus Status { get; }

        RV_Hexagon Hexagon { get; }

        RV_Grid Grid { get; }

        void Clear();
    }
}