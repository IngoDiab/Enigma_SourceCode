using System;
using UnityEngine;

public class EA_Enigma : EA_Singleton<EA_Enigma>
{
    #region Action
    public event Action OnKeyDownSound = null;
    #endregion

    #region F/P
    [SerializeField] EA_Reflector reflector = null;
    int nbRotors = 0;
    public bool IsValid => reflector;
    #endregion

    #region UnityMethods
    protected override void Awake()
    {
        base.Awake();
        OnKeyDownSound += () => EA_SoundManager.Instance.PlaySound(AudioType.KeyDown);
    }

    private void Start()
    {
        //nbRotors = EA_RotorManager.Instance.Items.Count;
    }

    private void OnDestroy()
    {
        OnKeyDownSound = null;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Encrypt/Decrypt a char
    /// </summary>
    /// <param name="_char"></param>
    /// <returns></returns>
    public char Encrypt(char _char)
    {
        if (_char.Equals('\0') || !IsValid) return '\0';
        OnKeyDownSound?.Invoke();
        nbRotors = EA_RotorManager.Instance.Items.Count;

        RotateRotor(1);
        //Rotate the first Rotor before everything

        char _resultRotorAller = TransitionInputRotor(_char,EA_RotorManager.Instance.Get(1),true);
        //Transition between the inputs and the first rotor

        for (int i = 1; i < nbRotors; i++)
        {
            _resultRotorAller = TransitionRotor(_resultRotorAller, EA_RotorManager.Instance.Get(i + 1), EA_RotorManager.Instance.Get(i), true);
        }
        //Transition between every rotor
 
        char _resultReflector = TransitionRotorReflector(_resultRotorAller, EA_RotorManager.Instance.Get(nbRotors));
        //Transition between the last rotor and the reflector

        char _resultRotorBack = TransitionInputRotor(_resultReflector, EA_RotorManager.Instance.Get(nbRotors),false);
        //Transition between the reflector and the last rotor

        for (int i = nbRotors - 1; i > 0; i--)
        {
            _resultRotorBack = TransitionRotor(_resultRotorBack, EA_RotorManager.Instance.Get(i), EA_RotorManager.Instance.Get(i+1), false);
        }
        //Transition between every rotor (backward)

        char _finalResult = TransitionRotorOutput(_resultRotorBack, EA_RotorManager.Instance.Get(1));
        //Final transition between the first rotor and the output

        EA_LightsManager.Instance.Enable(_finalResult);

        return _finalResult;
    }

    /// <summary>
    /// Rotate a rotor (and the others if necessary)
    /// </summary>
    /// <param name="_id">Rotor to rotate</param>
    void RotateRotor(int _id)
    {
        /*bool _isAboutToNotch = EA_RotorManager.Instance.CheckRotorAboutToNotch(_id);
        if (_isAboutToNotch)
        {
            EA_RotorManager.Instance.RotateRotor(_id);
            RotateRotor(_id + 1);
        }
        else
            EA_RotorManager.Instance.RotateRotor(_id);*/
        bool _isAboutToNotch = EA_RotorManager.Instance.CheckRotorAboutToNotch(_id);
        if (_isAboutToNotch)
        {
            EA_RotorManager.Instance.SetNextTarget(_id);
            RotateRotor(_id + 1);
        }
        else
            EA_RotorManager.Instance.SetNextTarget(_id);
    }

    /// <summary>
    /// Transition between an the inputs and a rotor
    /// </summary>
    /// <param name="_char">Char given by the inputs</param>
    /// <param name="_rotor">The rotor</param>
    /// <param name="_way">True if it's in the inputs to reflector way, false if it's in the reflector to inputs way</param>
    /// <returns></returns>
    char TransitionInputRotor(char _char,EA_Rotor _rotor, bool _way)
    {
        if (_way)
        {
            int _nbLetter = EA_Letters.lettersToInt[_char];
            _nbLetter = _nbLetter >= 0 ? _nbLetter : 26 + _nbLetter;
            int _posInRotor = (_rotor.NumberFirstLetter + _nbLetter) %26;
            char _charRotorIn = EA_Letters.intToLetters[_posInRotor];
            char _charRotorOut = _rotor.EncodageAller[_charRotorIn];
            return _charRotorOut;
        }
        else
        {
            int _nbLetter = EA_Letters.lettersToInt[_char];
            _nbLetter = _nbLetter >= 0 ? _nbLetter : 26 + _nbLetter;
            int _posInRotor = (_rotor.NumberFirstLetter + _nbLetter) % 26;
            char _charRotorIn = EA_Letters.intToLetters[_posInRotor];
            char _charRotorOut = _rotor.EncodageRetour[_charRotorIn];
            return _charRotorOut;
        }
    }

    /// <summary>
    /// Transition beteween rotors
    /// </summary>
    /// <param name="_char">Char given by the _rotorFrom</param>
    /// <param name="_rotorTo">Rotor that get the char</param>
    /// <param name="_rotorFrom">Rotor that gave the char</param>
    /// <param name="_way">True if it's in the inputs to reflector way, false if it's in the reflector to inputs way</param>
    /// <returns></returns>
    char TransitionRotor(char _char, EA_Rotor _rotorTo, EA_Rotor _rotorFrom, bool _way)
    {
        if (_way)
        {
            int _nbLetter = (EA_Letters.lettersToInt[_char] - _rotorFrom.NumberFirstLetter) % 26;
            _nbLetter = _nbLetter >= 0 ? _nbLetter : 26 + _nbLetter;
            int _posInRotor = (_rotorTo.NumberFirstLetter + _nbLetter) % 26;
            char _charRotorIn = EA_Letters.intToLetters[_posInRotor];

            char _charRotorOut = _rotorTo.EncodageAller[_charRotorIn];

            return _charRotorOut;
        }
        else
        {
            int _nbLetter =(EA_Letters.lettersToInt[_char] - _rotorFrom.NumberFirstLetter) % 26;
            _nbLetter = _nbLetter >= 0 ? _nbLetter : 26 + _nbLetter;
            int _posInRotor = (_rotorTo.NumberFirstLetter + _nbLetter)%26;
            char _charRotorIn = EA_Letters.intToLetters[_posInRotor];

            char _charRotorOut = _rotorTo.EncodageRetour[_charRotorIn];

            return _charRotorOut;
        }
    }

    /// <summary>
    /// Transition between the rotor and the reflector
    /// </summary>
    /// <param name="_char">Char given by the rotor</param>
    /// <param name="_rotor">Rotor that gave the char</param>
    /// <returns></returns>
    char TransitionRotorReflector(char _char, EA_Rotor _rotor)
    {
        int _nbLetter = (EA_Letters.lettersToInt[_char] - _rotor.NumberFirstLetter) % 26;
        _nbLetter = _nbLetter >= 0 ? _nbLetter : 26 + _nbLetter;
        char _charReflectorIn = EA_Letters.intToLetters[_nbLetter];
        char _charReflectorOut = reflector.EncryptedData[_charReflectorIn];
        return _charReflectorOut;
    }

    /// <summary>
    /// Transition between the rotor and the output
    /// </summary>
    /// <param name="_char">Char given by the rotor</param>
    /// <param name="_rotor">Rotor that gave the char</param>
    /// <returns></returns>
    char TransitionRotorOutput(char _char, EA_Rotor _rotor)
    {
        int _nbLetter = (EA_Letters.lettersToInt[_char] - _rotor.NumberFirstLetter) % 26;
        _nbLetter = _nbLetter >= 0 ? _nbLetter : 26 + _nbLetter;
        char _charOutput = EA_Letters.intToLetters[_nbLetter];
        return _charOutput;
    }

    #endregion

}
