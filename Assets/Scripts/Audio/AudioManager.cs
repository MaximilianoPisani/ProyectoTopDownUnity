using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public List<AudioClip> musicClips; 
    public List<AudioClip> sfxClips;

    private Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDict = new Dictionary<string, AudioClip>();
    private bool _allowRestart = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

   
            foreach (var clip in musicClips)
            {
                if (clip != null && !musicDict.ContainsKey(clip.name))
                    musicDict.Add(clip.name, clip);
            }

            foreach (var clip in sfxClips)
            {
                if (clip != null && !sfxDict.ContainsKey(clip.name))
                    sfxDict.Add(clip.name, clip);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("MusicBackground "); 
    }

    public void PlayMusic(string name, bool loop = true)
    {
        if (musicDict.TryGetValue(name, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            _allowRestart = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Música not found: " + name);
        }
    }
    public void StopMusic()
    {
        _allowRestart = false;
        musicSource.Stop();
    }

    private void Update()
    {
        if (_allowRestart && !musicSource.isPlaying && musicSource.clip != null)
        {
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        if (sfxDict.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SFX not found: " + name);
        }
    }
}