using System;
using UnityEngine;

[Serializable]
public class EA_Sound
{
    #region F/P
    [SerializeField] string name = "";
    [SerializeField] AudioType type = AudioType.None;
    [SerializeField] AudioSource source = null;
    [SerializeField,Range(0,1)] float volume = 0;
    [SerializeField] bool isLoop = false;
    [SerializeField] AudioClip sound = null;

    public AudioType Type => type;
    public bool IsValid => sound;
    #endregion

    #region Methods
    /// <summary>
    /// Play the sound
    /// </summary>
    public void Play()
    {
        if (!IsValid) return;
        if (source)
        {
            source.clip = sound;
            source.volume = volume;
            source.loop = isLoop;
            source.Play();
        }
        else
            AudioSource.PlayClipAtPoint(sound, Camera.main.transform.position);
    }
    #endregion
}
