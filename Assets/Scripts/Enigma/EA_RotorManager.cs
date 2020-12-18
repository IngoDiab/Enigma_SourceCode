using System;
using System.Collections.Generic;
public class EA_RotorManager : EA_Singleton<EA_RotorManager>, IHandler<int, EA_Rotor>
{
    #region Action
    public event Action OnUpdateRotors = null;
    #endregion

    #region F/P
    Dictionary<int, EA_Rotor> items = new Dictionary<int, EA_Rotor>();
    public Dictionary<int, EA_Rotor> Items => items;
    #endregion

    #region UnityMethods
    private void OnDestroy()
    {
        OnUpdateRotor = null;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Init the rotors (and show errors if there is any)
    /// </summary>
    void InitRotors()
    {
        foreach (KeyValuePair<int, EA_Rotor> _rotor in items)
        {
            _rotor.Value.InitRotor();
        }
        EA_ErrorManager.Instance.MergeErrorsRotor(EA_ErrorManager.Instance.ErrorsRotor);
        EA_ErrorManager.Instance.SetUIErrorPanelRotor();

        EA_ErrorManager.Instance.MergeErrorsNotch(EA_ErrorManager.Instance.ErrorsNotch);
        EA_ErrorManager.Instance.SetUIErrorPanelNotch();
    }

    /// <summary>
    /// Add a rotor
    /// </summary>
    /// <param name="_rotor">Rotor to add</param>
    public void Add(EA_Rotor _rotor)
    {
        items.Add(_rotor.ID, _rotor);
        _rotor.name += $" [MANAGED]";
        OnUpdateRotors += _rotor.OnUpdateRotor;
    }

    public void Disable(int _key)
    {
        
    }

    public void Enable(int _key)
    {
        
    }

    /// <summary>
    /// Check if a rotor exist
    /// </summary>
    /// <param name="_key">Rotor's key</param>
    /// <returns></returns>
    public bool Exists(int _key)
    {
        return items.ContainsKey(_key);
    }

    /// <summary>
    /// Get a rotor
    /// </summary>
    /// <param name="_key">Rotor's key</param>
    /// <returns></returns>
    public EA_Rotor Get(int _key)
    {
        if (!Exists(_key)) return null;
        return items[_key];
    }

    /// <summary>
    /// Remove a rotor
    /// </summary>
    /// <param name="_key">Rotor's key</param>
    public void Remove(int _key)
    {
        if (Exists(_key))
            items.Remove(_key);
    }

    /// <summary>
    /// Reset all rotors
    /// </summary>
    public void ResetAllRotors()
    {
        InitRotors();
    }

    /// <summary>
    /// Set the step in the rotor rotation
    /// </summary>
    /// <param name="_id">Rotor id</param>
    public void SetNextTarget(int _id)
    {
        EA_Rotor _rotor = Get(_id);
        if (_rotor) _rotor.SetNextTarget();
    }

    /// <summary>
    /// Check if the rotor is on its notch
    /// </summary>
    /// <param name="_id">Rotor id</param>
    /// <returns></returns>
    public bool CheckRotorAboutToNotch(int _id)
    {
        EA_Rotor _rotor = EA_RotorManager.Instance.Get(_id);
        if (!_rotor) return false;
        return _rotor.IsAboutToNotch;
    }
    #endregion
}
