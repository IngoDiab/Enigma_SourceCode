using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_LightsManager : EA_Singleton<EA_LightsManager>,IHandler<char,EA_Light>
{
    Dictionary<char, EA_Light> items = new Dictionary<char, EA_Light>();
    EA_Light currentLight = null;

    public Dictionary<char, EA_Light> Items => items;

    public void Add(EA_Light _light)
    {
        items.Add(_light.ID, _light);
        _light.name += $" [MANAGED]";
    }

    public void Disable(char _key)
    {
        EA_Light _light = Get(_key);
        if (!_light) return;
        _light.Disable();
    }

    public void Enable(char _key)
    {
        EA_Light _light = Get(_key);
        if (!_light) return;
        if (currentLight) currentLight.Disable();
        _light.Enable();
        currentLight = _light;
    }

    public bool Exists(char _key)
    {
        return items.ContainsKey(_key);
    }

    public EA_Light Get(char _key)
    {
        if (!Exists(_key)) return null;
        return items[_key];
    }

    public void Remove(char _key)
    {
        if (Exists(_key))
            items.Remove(_key);
    }

    public void LightReset()
    {
        if (!currentLight) return;
        currentLight.Disable();
        currentLight = null;
    }
}
