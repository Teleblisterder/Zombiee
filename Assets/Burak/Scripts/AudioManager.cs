using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 0.5f;
    [Range(0.1f, 3f)] public float pitch = 1f;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Efekt Ayarlarý (SFX)")]
    public Sound[] sounds;
    private AudioSource sfxSource;

    [Header("Müzik Ayarlarý (BGM)")]
    public AudioClip backgroundMusic;
    [Range(0f, 1f)] public float musicVolume = 0.2f;
    private AudioSource musicSource;

    
    private Dictionary<string, float> lastPlayedTimes = new Dictionary<string, float>();

    void Awake()
    {
       
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = true;
        
        if (backgroundMusic != null)
        {
            musicSource.Play();
        }
    }

   
    public void Play(string name, float cooldown = 0f)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Ses Bulunamadý: " + name);
            return;
        }

        
        if (cooldown > 0)
        {
            if (lastPlayedTimes.ContainsKey(name))
            {
                if (Time.time < lastPlayedTimes[name] + cooldown) return;
            }
            lastPlayedTimes[name] = Time.time;
        }

       
        sfxSource.pitch = s.pitch + UnityEngine.Random.Range(-0.1f, 0.1f);
        sfxSource.PlayOneShot(s.clip, s.volume);
    }

   
    public void PlayRandom(string[] names, float cooldown = 0.5f)
    {
        string randomName = names[UnityEngine.Random.Range(0, names.Length)];
        Play(randomName, cooldown);
    }

   
    public void StopMusic() => musicSource.Stop();
    public void StartMusic() => musicSource.Play();
}