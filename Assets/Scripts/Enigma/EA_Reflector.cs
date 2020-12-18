using System.Collections.Generic;
using UnityEngine;

public class EA_Reflector : MonoBehaviour
{
    #region F/P
    [SerializeField] TextAsset code = null;
    Dictionary<char, char> encryptedData = new Dictionary<char, char>();
    public Dictionary<char, char> EncryptedData => encryptedData;
    public bool IsValid => code;
    #endregion

    #region UnityMethods
    private void Start()
    {
        InitReflector();
    }
    #endregion

    #region Methods
    void InitReflector()
    {
        if (!IsValid) return;
        char[] _code = code.ToString().ToCharArray();
        if (_code.Length != 26) return;
        for (int i = 0; i < 26; i++)
        {
            char letterRead = EA_Letters.intToLetters[i];
            char encodeRead = _code[i];

            encryptedData[letterRead] = encodeRead;
        }
    }
    #endregion
}
