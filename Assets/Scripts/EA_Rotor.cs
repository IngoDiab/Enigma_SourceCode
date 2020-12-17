using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_Rotor : MonoBehaviour, IItem<int>
{
    [SerializeField] int id = 0;
    [SerializeField] float speedRota = 5;
    Dictionary<char, char> encodageAller = new Dictionary<char, char>();
    Dictionary<char, char> encodageRetour = new Dictionary<char, char>();
    [SerializeField] char[] encodeLetters = new char[26];
    [SerializeField, Range(0, 100)] float minDistance = 1;
    [SerializeField] char notchLetter = 'A';

    Vector3 targetRota = Vector3.zero;

    float valueToFixTheRotorDoor = 5.697001f;

    char firstLetter = '\0';
    int numberFirstLetter = 0;  //Place dans l'alphabet - 1

    public int ID => id;
    public bool IsTurned()
    {
        if((transform.eulerAngles.z - targetRota.z) < 0)
        Vector3.Distance(transform.eulerAngles, targetRota) < minDistance;
    }
    public bool IsAboutToNotch => firstLetter.Equals(notchLetter);
    public bool IsEnabled => true;
    public int NumberFirstLetter => numberFirstLetter;
    public Dictionary<char, char> EncodageAller => encodageAller;
    public Dictionary<char, char> EncodageRetour => encodageRetour;
    private void Start()
    {
        targetRota = transform.eulerAngles;
        InitRotor();
        InitRotorEncodageAller();
        InitRotorEncodageRetour();
        EA_RotorManager.Instance.Add(this);
    }

    private void OnDestroy()
    {
        EA_RotorManager.Instance.Remove(id);
    }

    private void Update()
    {
        RotateRotor();
    }

    public void InitRotor()
    {
        string _configRotor = EA_UIManager.Instance.RotorConfig[id-1].text;      //id-1 because Rotor1 has id 1 but in the RotorConfig, its letter is the 0
        string _configNotch = EA_UIManager.Instance.NotchConfig[id-1].text;

        bool isErrorRotor = _configRotor.Length != 1;
        bool isErrorNotch = _configNotch.Length != 1;

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

    void InitRotorEncodageAller()
    {
        for (int i = 0; i < 26; i++)
        {
            char letterRead = EA_Letters.intToLetters[i];
            char encodeRead = encodeLetters[i];

            encodageAller[letterRead] = encodeRead;
        }
    }

    void InitRotorEncodageRetour()
    {
        foreach (KeyValuePair<char,char> _associtaion in encodageAller)
        {
            char _key = encodageAller[_associtaion.Value];
            char _value = encodageAller[_associtaion.Key];
            encodageRetour[_key] = _value;
        }
    }

    /*public void RotateRotorManyTimes(int _nbtimes)
    {
        for (int i = 0; i < _nbtimes; i++)
        {
            RotateRotor();
        }
    }*/

    /*public void RotateRotor()
    {
        float angle = transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle - (360f/26f));
        numberFirstLetter++;
        numberFirstLetter = numberFirstLetter > 25 ? 0 : numberFirstLetter;
        firstLetter = EA_Letters.intToLetters[numberFirstLetter];
    }*/

    public void RotateRotor()
    {
        //int _targetInt = EA_Letters.lettersToInt[_target];
        if (IsTurned) return;
        transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles,targetRota,Time.deltaTime* speedRota);
    }

    public void SetNextTarget()
    {
        targetRota = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - (360f / 26f));
        numberFirstLetter++;
        numberFirstLetter = numberFirstLetter > 25 ? 0 : numberFirstLetter;
        firstLetter = EA_Letters.intToLetters[numberFirstLetter];
    }

    public void ResetRotor()
    {
        InitRotor();
        Debug.Log($"First letter : {firstLetter} number {numberFirstLetter}");
    }

    public void DebugLogEncodage(string _sens)
    {
        if (_sens.Equals("Aller"))
        {
            foreach (KeyValuePair<char, char> code in encodageAller)
            {
                string test = $"{code.Key} a pour valeur {code.Value} dans ce rotor à l'aller";
                Debug.Log(test);
            }
        }
        else if (_sens.Equals("Retour"))
        {
            foreach (KeyValuePair<char, char> code in encodageRetour)
            {
                string test = $"{code.Key} a pour valeur {code.Value} dans ce rotor au retour";
                Debug.Log(test);
            }
        }
    }
    public void Enable()
    {
    }
    public void Disable()
    {
        
    }
}
