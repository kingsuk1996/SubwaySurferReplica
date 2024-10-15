using System;
using UnityEngine;
using UnityEngine.Audio;

namespace RedApple.SubwaySurfer
{
	public class AudioManager : MonoBehaviour
	{
		public static AudioManager Instance;
		public Sound[] sounds;
		void Awake()
		{
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
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
		public void Play(string sound)
		{
			Sound s = Array.Find(sounds, item => item.name == sound);
			s.source.Play();
		}
		public void Stop(string sound)
		{
			Sound s = Array.Find(sounds, item => item.name == sound);
			s.source.Stop();
		}
		public void Pause(string sound)
		{
			Sound s = Array.Find(sounds, item => item.name == sound);
			s.source.Pause();
		}
		public void PlayOneShot(string sound)
		{
			Sound s = Array.Find(sounds, item => item.name == sound);
			//Debug.LogError("===="+s.clip.name+" >>> ");
			s.source.PlayOneShot(s.clip);
		}
	}
}