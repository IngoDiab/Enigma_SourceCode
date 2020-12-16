using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_Light : MonoBehaviour, IItem<char>
{
    [SerializeField] char id = 'A';
    [SerializeField] Light lightComponent = null;
    [SerializeField] bool isEnabled = false;
    public bool IsValid => lightComponent;
    public char ID => id;
    public bool IsEnabled => isEnabled;

    public void Start()
    {
        InitLights();
    }

    public void OnDestroy()
    {
        EA_LightsManager.Instance.Remove(id);
    }

    public void InitLights()
    {
        EA_LightsManager.Instance.Add(this);
    }

    public void Disable()
    {
        if (!IsValid || !IsEnabled) return;
        lightComponent.enabled = false;
        isEnabled = false;
    }

    public void Enable()
    {
        if (!IsValid || IsEnabled) return;
        lightComponent.enabled = true;
        isEnabled = true;
    }
}
