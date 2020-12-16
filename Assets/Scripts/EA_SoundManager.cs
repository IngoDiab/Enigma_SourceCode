using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EA_SoundManager : EA_Singleton<EA_SoundManager>
{
    [SerializeField] List<EA_Sound> allSounds = new List<EA_Sound>();

    public void PlaySound(AudioType _type)
    {
        List<EA_Sound> _sounds = allSounds.Where((s) => s.Type == _type).ToList();
        _sounds.ForEach((s) => s.Play());
    }
}

[Serializable]
public class EA_Sound
{
    [SerializeField] string name = "";
    [SerializeField] AudioType type = AudioType.None;
    [SerializeField] AudioSource source = null;
    [SerializeField] float volume = 0;
    [SerializeField] AudioClip sound = null;

    public AudioType Type => type;
    public bool IsValid => sound;

    public void Play()
    {
        if (!IsValid) return;
        if (source)
        {
            source.clip = sound;
            source.volume = volume;
            source.Play();
        }
        else
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
    }
}

public enum AudioType
{
    None,
    Tick,
    KeyDown,
    Rain
}
