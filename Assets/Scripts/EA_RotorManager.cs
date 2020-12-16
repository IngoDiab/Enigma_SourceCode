using System;
using System.Collections.Generic;
using UnityEngine;

public class EA_RotorManager : EA_Singleton<EA_RotorManager>, IHandler<int, EA_Rotor>
{
    public event Action OnTick = null;

    Dictionary<int, EA_Rotor> items = new Dictionary<int, EA_Rotor>();

    public Dictionary<int, EA_Rotor> Items => items;

    protected override void Awake()
    {
        base.Awake();
        OnTick += () => EA_SoundManager.Instance.PlaySound(AudioType.Tick);
    }

    private void OnDestroy()
    {
        OnTick = null;
    }

    private void Update()
    {
        //RotateRotor(1);
    }

    public void Add(EA_Rotor _rotor)
    {
        items.Add(_rotor.ID, _rotor);
        _rotor.name += $" [MANAGED]";
    }

    public void Disable(int _key)
    {
        
    }

    public void Enable(int _key)
    {
        
    }

    public bool Exists(int _key)
    {
        return items.ContainsKey(_key);
    }

    public EA_Rotor Get(int _key)
    {
        if (!Exists(_key)) return null;
        return items[_key];
    }

    public void Remove(int _key)
    {
        if (Exists(_key))
            items.Remove(_key);
    }

    public void ResetAllRotors()
    {
        foreach (KeyValuePair<int,EA_Rotor> _rotor in items)
        {
            _rotor.Value.ResetRotor();
            Debug.Log($"Rotor {_rotor.Value.ID} reset");
        }
        OnTick.Invoke();
    }

    public void RotateRotor(int _id)
    {
        EA_Rotor _rotor = Get(_id);
        if (_rotor) _rotor.RotateRotor();
    }

    public bool CheckRotorAboutToNotch(int _id)
    {
        EA_Rotor _rotor = EA_RotorManager.Instance.Get(_id);
        if (!_rotor) return false;
        return _rotor.IsAboutToNotch;
    }
}
