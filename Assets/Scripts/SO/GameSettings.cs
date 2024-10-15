using UnityEngine;


[CreateAssetMenu(fileName = "GameSettingSO", menuName = "ScriptableObjects/GameSettingScriptableObject", order = 1)]
public class GameSettings : ScriptableObject
{
    [Header("BlockSpawner")]
    public int MinSpawnCount = 4;
    public int MaxSpawnCount = 6;

    [Space(10)]
    [Header("ObjectPool")]
    public int MaxObect = 20;

    [Space(10)]
    [Header("CoinPool")]
    public int MaxCoin = 20;

    [Space(10)]
    [Header("BlockMovement")]
    public float DestroyZone = 75f;
    public float MaxSpeed = 40;
    public float SpeedMultiplier = .5f;
    public float SpeedIncreaseDelay = 2f;

    [Space(10)]
    [Header("DogController")]
    public float DogSpeed = 1;

    [Space(10)]
    [Header("PlayerManager")]
    public float playerBlinkingTimer = 0.0f;
    public float playerBlinkingMiniDuration = 0.1f;
    public float playerBlinkingTotalTimer = 0.0f;
    public float playerBlinkingTotalDuration = 2.0f;

}
