using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_Rotor : MonoBehaviour, IItem<int>
{
    [SerializeField] int id = 0;
    Dictionary<char, char> encodageAller = new Dictionary<char, char>();
    Dictionary<char, char> encodageRetour = new Dictionary<char, char>();
    [SerializeField] char[] encodeLetters = new char[26];
    [SerializeField] char firstLetterRotorBegin = 'A';
    [SerializeField] char notchLetter = 'A';

    float valueToFixTheRotorDoor = 5.697001f;

    char firstLetter = '\0';
    int numberFirstLetter = 0;  //Place dans l'alphabet - 1

    public int ID => id;
    public bool IsAboutToNotch => firstLetter.Equals(notchLetter);
    public bool IsEnabled => true;
    public int NumberFirstLetter => numberFirstLetter;
    public Dictionary<char, char> EncodageAller => encodageAller;
    public Dictionary<char, char> EncodageRetour => encodageRetour;
    private void Start()
    {
        InitRotor();
        InitRotorEncodageAller();
        InitRotorEncodageRetour();
        EA_RotorManager.Instance.Add(this);
    }

    private void OnDestroy()
    {
        EA_RotorManager.Instance.Remove(id);
    }

    void InitRotor()
    {
        firstLetter = firstLetterRotorBegin;
        numberFirstLetter = EA_Letters.lettersToInt[firstLetterRotorBegin];
        transform.eulerAngles = new Vector3(0, 0, valueToFixTheRotorDoor - (360f / 26f) * numberFirstLetter + 1);
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

    public void RotateRotorManyTimes(int _nbtimes)
    {
        for (int i = 0; i < _nbtimes; i++)
        {
            RotateRotor();
        }
    }

    public void RotateRotor()
    {
        float angle = transform.eulerAngles.z;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle - (360f/26f));
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
