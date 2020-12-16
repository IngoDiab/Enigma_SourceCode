using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_Reflector : MonoBehaviour
{
    Dictionary<char, char> encodage = new Dictionary<char, char>()
    {
        {'A','Y'},
        {'Y','A'},

        {'B','R'},
        {'R','B'},

        {'C','U'},
        {'U','C'},

        {'D','H'},
        {'H','D'},

        {'E','Q'},
        {'Q','E'},

        {'F','S'},
        {'S','F'},

        {'G','L'},
        {'L','G'},

        {'I','P'},
        {'P','I'},

        {'J','X'},
        {'X','J'},

        {'K','N'},
        {'N','K'},

        {'M','O'},
        {'O','M'},

        {'T','Z'},
        {'Z','T'},

        {'V','W'},
        {'W','V'},
    };

    public Dictionary<char, char> Encodage => encodage;

    void DebugLogEncodage()
    {
        foreach (KeyValuePair<char, char> code in encodage)
        {
            string test = $"{code.Key} a pour valeur {code.Value} dans le reflector";
            Debug.Log(test);
        }
    }
}
