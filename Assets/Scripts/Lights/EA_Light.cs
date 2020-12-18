using UnityEngine;

public class EA_Light : MonoBehaviour, IItem<char>
{
    #region F/P
    [SerializeField] char id = 'A';
    [SerializeField] Light lightComponent = null;
    [SerializeField] bool isEnabled = false;
    public bool IsValid => lightComponent;
    public char ID => id;
    public bool IsEnabled => isEnabled;
    #endregion

    #region UnityMethods
    public void Start()
    {
        InitLight();
    }

    public void OnDestroy()
    {
        EA_LightsManager.Instance.Remove(id);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Init the light
    /// </summary>
    public void InitLight()
    {
        EA_LightsManager.Instance.Add(this);
    }
    /// <summary>
    /// Disable the light
    /// </summary>
    public void Disable()
    {
        if (!IsValid || !IsEnabled) return;
        lightComponent.enabled = false;
        isEnabled = false;
    }
    /// <summary>
    /// Enable the light
    /// </summary>
    public void Enable()
    {
        if (!IsValid || IsEnabled) return;
        lightComponent.enabled = true;
        isEnabled = true;
    }
    #endregion
}
