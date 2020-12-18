using System.Collections.Generic;

public class EA_LightsManager : EA_Singleton<EA_LightsManager>,IHandler<char,EA_Light>
{
    #region F/P
    Dictionary<char, EA_Light> items = new Dictionary<char, EA_Light>();
    EA_Light currentLight = null;

    public Dictionary<char, EA_Light> Items => items;
    #endregion

    #region Methods
    /// <summary>
    /// Add a light
    /// </summary>
    /// <param name="_light"></param>
    public void Add(EA_Light _light)
    {
        items.Add(_light.ID, _light);
        _light.name += $" [MANAGED]";
    }

    /// <summary>
    /// Disable the light
    /// </summary>
    /// <param name="_key">Light's key</param>
    public void Disable(char _key)
    {
        EA_Light _light = Get(_key);
        if (!_light) return;
        _light.Disable();
    }

    /// <summary>
    /// Enable a light
    /// </summary>
    /// <param name="_key">Light's key</param>
    public void Enable(char _key)
    {
        EA_Light _light = Get(_key);
        if (!_light) return;
        if (currentLight) currentLight.Disable();
        _light.Enable();
        currentLight = _light;
    }

    /// <summary>
    /// Check if the light exist
    /// </summary>
    /// <param name="_key">Light's key</param>
    /// <returns></returns>
    public bool Exists(char _key)
    {
        return items.ContainsKey(_key);
    }

    /// <summary>
    /// Get the light
    /// </summary>
    /// <param name="_key">Light's key</param>
    /// <returns>The light wanted</returns>
    public EA_Light Get(char _key)
    {
        if (!Exists(_key)) return null;
        return items[_key];
    }

    /// <summary>
    /// Remove a light
    /// </summary>
    /// <param name="_key">Light's key</param>
    public void Remove(char _key)
    {
        if (Exists(_key))
            items.Remove(_key);
    }

    /// <summary>
    /// Reset the current light
    /// </summary>
    public void LightReset()
    {
        if (!currentLight) return;
        currentLight.Disable();
        currentLight = null;
    }
    #endregion
}
