using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;

public class EA_Rotor : MonoBehaviour, IItem<int>
{
    #region Action
    public event Action OnTick = null;
    #endregion

    #region F/P
    [SerializeField] int id = 0;
    [SerializeField] float speedRota = 5;
    Dictionary<char, char> encodageAller = new Dictionary<char, char>();
    Dictionary<char, char> encodageRetour = new Dictionary<char, char>();
    [SerializeField] TextAsset code = null;
    [SerializeField, Range(0, 100)] float minDistance = 1;
    [SerializeField] char notchLetter = 'A';

    float angle = 0;

    float valueToFixTheRotorDoor = 6.972001f;

    char firstLetter = '\0';
    int numberFirstLetter = 0;  //Place dans l'alphabet - 1

    public bool IsValid => code;
    public int ID => id;
    public bool IsAboutToNotch => firstLetter.Equals(notchLetter);
    public bool IsEnabled => true;
    public int NumberFirstLetter => numberFirstLetter;
    public Dictionary<char, char> EncodageAller => encodageAller;
    public Dictionary<char, char> EncodageRetour => encodageRetour;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        OnTick += () => EA_SoundManager.Instance.PlaySound(AudioType.Tick);
    }

    private void Start()
    {
        InitRotor();
        InitEncryptRotorForward();
        InitEncryptRotorBackward();
        EA_RotorManager.Instance.Add(this);
    }

    private void OnDestroy()
    {
        EA_RotorManager.Instance.Remove(id);
        OnTick = null;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Init the rotor (and save errors if there is any)
    /// </summary>
    public void InitRotor()
    {
        string _configRotor = EA_UIManager.Instance.RotorConfig[id-1].text;      //id-1 because Rotor1 has id 1 but in the UI RotorConfig, its letter is the 0
        string _configNotch = EA_UIManager.Instance.NotchConfig[id-1].text;

        bool isErrorRotor = _configRotor.Length != 1 || !Regex.Match(_configRotor, @"[A-Z]|[a-z]").Success;
        bool isErrorNotch = _configNotch.Length != 1 || !Regex.Match(_configNotch, @"[A-Z]|[a-z]").Success;

        if (isErrorRotor)
        {
            EA_ErrorManager.Instance.ErrorsRotor.Add(EA_ErrorManager.Instance.ErrorRotor(id));
        }

        if (isErrorNotch)
        {
            EA_ErrorManager.Instance.ErrorsNotch.Add(EA_ErrorManager.Instance.ErrorNotch(id));
        }

        if (isErrorNotch || isErrorRotor) return;       //Permet d'afficher les problèmes de configuration rotor ET de configuration de notch pour un même rotor

        firstLetter = char.ToUpper(_configRotor.ToCharArray()[0]);
        numberFirstLetter = EA_Letters.lettersToInt[firstLetter];
        transform.eulerAngles = new Vector3(0, 0, valueToFixTheRotorDoor - (360f / 26f) * numberFirstLetter + 1);

        notchLetter = char.ToUpper(_configNotch.ToCharArray()[0]);
    }

    /// <summary>
    /// Init all associations thanks to encodeLetters (in the input-reflector way)
    /// </summary>
    void InitEncryptRotorForward()
    {
        if (!IsValid) return;
        char[] _code = code.ToString().ToCharArray();
        if (_code.Length != 26) return;
        for (int i = 0; i < 26; i++)
        {
            char letterRead = EA_Letters.intToLetters[i];
            char encodeRead = _code[i];

            encodageAller[letterRead] = encodeRead;
        }
    }

    /// <summary>
    /// Init all associations thanks to encodageAller (in the reflector-input way)
    /// </summary>
    void InitEncryptRotorBackward()
    {
        foreach (KeyValuePair<char,char> _associtaion in encodageAller)
        {
            char _key = encodageAller[_associtaion.Value];
            char _value = encodageAller[_associtaion.Key];
            encodageRetour[_key] = _value;
        }
    }

    public void UpdateRotor()
    {
        RotateRotor();
    }

    /// <summary>
    /// Rotate the rotor
    /// </summary>
    public void RotateRotor()
    {
        float targetAngle = -(numberFirstLetter) / 26f * 360 + valueToFixTheRotorDoor;
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * speedRota);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
    }

    /// <summary>
    /// Set the target to rotate to
    /// </summary>
    public void SetNextTarget()
    {
        numberFirstLetter++;
        numberFirstLetter = numberFirstLetter > 25 ? 0 : numberFirstLetter;
        firstLetter = EA_Letters.intToLetters[numberFirstLetter];
        OnTick?.Invoke();
    }

    /// <summary>
    /// Reset the rotor
    /// </summary>
    public void ResetRotor()
    {
        InitRotor();
        Debug.Log($"First letter : {firstLetter} number {numberFirstLetter}");
    }
    public void Enable()
    {
    }
    public void Disable()
    {
        
    }
#endregion
}
