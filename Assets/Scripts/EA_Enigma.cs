using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_Enigma : EA_Singleton<EA_Enigma>
{
    public event Action OnKeyDownSound = null;

    [SerializeField] EA_Reflector reflector = null;
    public bool IsValid => reflector;

    protected override void Awake()
    {
        base.Awake();
        OnKeyDownSound += () => EA_SoundManager.Instance.PlaySound(AudioType.KeyDown);
    }

    private void OnDestroy()
    {
        OnKeyDownSound = null;
    }

    public char Decode(char _char)
    {
        if (_char.Equals('\0')) return '\0';
        OnKeyDownSound?.Invoke();

        RotateRotor(1);

        char _resultRotor1 = TransitionInputRotor(_char,EA_RotorManager.Instance.Get(1),true);
        char _resultRotor2 = TransitionRotor(_resultRotor1, EA_RotorManager.Instance.Get(2), EA_RotorManager.Instance.Get(1),true);
        char _resultRotor3 = TransitionRotor(_resultRotor2, EA_RotorManager.Instance.Get(3), EA_RotorManager.Instance.Get(2),true);

        char _resultReflector = TransitionRotorReflector(_resultRotor3, EA_RotorManager.Instance.Get(3));

        char _resultRotor3Back = TransitionInputRotor(_resultReflector, EA_RotorManager.Instance.Get(3),false);
        char _resultRotor2Back = TransitionRotor(_resultRotor3Back, EA_RotorManager.Instance.Get(2), EA_RotorManager.Instance.Get(3), false);
        char _resultRotor1Back = TransitionRotor(_resultRotor2Back, EA_RotorManager.Instance.Get(1), EA_RotorManager.Instance.Get(2), false);

        char _result = TransitionRotorOutput(_resultRotor1Back, EA_RotorManager.Instance.Get(1));

        EA_LightsManager.Instance.Enable(_result);

        return _result;
    }

    void RotateRotor(int _id)
    {
        bool _isAboutToNotch = EA_RotorManager.Instance.CheckRotorAboutToNotch(_id);
        if (_isAboutToNotch)
        {
            EA_RotorManager.Instance.RotateRotor(_id);
            RotateRotor(_id + 1);
        }
        else
            EA_RotorManager.Instance.RotateRotor(_id);
    }

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

    char TransitionRotorReflector(char _char, EA_Rotor _rotor)
    {
        int _nbLetter = (EA_Letters.lettersToInt[_char] - _rotor.NumberFirstLetter) % 26;
        _nbLetter = _nbLetter >= 0 ? _nbLetter : 26 + _nbLetter;
        char _charReflectorIn = EA_Letters.intToLetters[_nbLetter];
        char _charReflectorOut = reflector.Encodage[_charReflectorIn];
        return _charReflectorOut;
    }

    char TransitionRotorOutput(char _char, EA_Rotor _rotor)
    {
        int _nbLetter = (EA_Letters.lettersToInt[_char] - _rotor.NumberFirstLetter) % 26;
        _nbLetter = _nbLetter >= 0 ? _nbLetter : 26 + _nbLetter;
        char _charOutput = EA_Letters.intToLetters[_nbLetter];
        return _charOutput;
    }

}
