using DevCommon;
using System;
using UnityEngine;

namespace JustGame.SubwaySurfer
{
    public class AudioManager : Singleton<AudioManager>
    {
        public Sound[] sounds;

        protected override void Awake()
        {
            base.Awake();
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playAtAwake;
            }
        }

        public void Play(AudioType sound)
        {
            Sound s = Array.Find(sounds, item => item.audioType == sound);
            s.source.Play();
        }

        public void PlayOneShot(AudioType sound)
        {
            Sound s = Array.Find(sounds, item => item.audioType == sound);
            s.source.PlayOneShot(s.clip);
        }

        public void Pause(AudioType sound)
        {
            Sound s = Array.Find(sounds, item => item.audioType == sound);
            s.source.Pause();
        }

        public void Stop(AudioType sound)
        {
            Sound s = Array.Find(sounds, item => item.audioType == sound);
            s.source.Stop();
        }
    }
}