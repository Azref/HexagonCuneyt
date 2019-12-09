using Assets.Scripts.Core.Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Runtime Variables/Game Status", order = 1)]
public class RV_GameStatus : ScriptableObject
{
    public GameStatus value;

}