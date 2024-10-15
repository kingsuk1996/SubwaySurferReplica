using UnityEngine;

namespace JustGame.SubwaySurfer
{
    [CreateAssetMenu(fileName = "GameSettingSO", menuName = "ScriptableObjects/GameSettingScriptableObject", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [Space(10)]
        [Header("PlayerManager")]
        public float PlayerBlinkingTimer = 0.0765246f;
        public float PlayerBlinkingMiniDuration = 0.1f;
        public float PlayerBlinkingTotalTimer = 0.0f;
        public float PlayerBlinkingTotalDuration = 2.0f;

        [Space(10)]
        [Header("BlockProperties")]
        public float Zoffset = -70;
        public float InitialBlockSpeed = 15;
        public float MaxSpeed = 40;
        public float SpeedMultiplier = .1f;
        public float SpeedIncreaseDelay = 2f;

    }
}