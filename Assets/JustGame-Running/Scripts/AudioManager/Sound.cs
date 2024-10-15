using UnityEngine;
namespace JustGame.SubwaySurfer
{
    [System.Serializable]
    public class Sound
    {
        public AudioType audioType;

        public AudioClip clip;


        public float volume = 1;

        [Range(-3f, 3f)]
        public float pitch = 1;

        public bool loop = false;

        public bool playAtAwake = false;

        [HideInInspector]
        public AudioSource source;
    }
}
