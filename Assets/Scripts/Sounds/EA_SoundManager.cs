using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EA_SoundManager : EA_Singleton<EA_SoundManager>
{
    #region F/P
    [SerializeField] List<EA_Sound> allSounds = new List<EA_Sound>();
    #endregion

    #region UnityMethods
    private void Start()
    {
        PlaySound(AudioType.Rain);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Play all sounds from a type
    /// </summary>
    /// <param name="_type"></param>
    public void PlaySound(AudioType _type)
    {
        List<EA_Sound> _sounds = allSounds.Where((s) => s.Type == _type).ToList();
        _sounds.ForEach((s) => s.Play());
    }
    #endregion
}
