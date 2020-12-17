using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class EA_UIManager : EA_Singleton<EA_UIManager>
{
    [SerializeField] Image resultPanel = null;
    [SerializeField] Button resetButton = null;
    [SerializeField] Button quitButton = null;

    #region Crypte/Decrypte
    [SerializeField] TMP_InputField enterText = null;
    [SerializeField] TMP_Text resultText = null;

    public TMP_InputField EnterText => enterText;
    #endregion

    #region Config
    [SerializeField] List<TMP_InputField> rotorConfig = new List<TMP_InputField>();
    [SerializeField] List<TMP_InputField> notchConfig = new List<TMP_InputField>();
    public List<TMP_InputField> RotorConfig => rotorConfig;
    public List<TMP_InputField> NotchConfig => notchConfig;
    #endregion

    #region Error Rotor
    [SerializeField] Transform panelErrorRotor = null;
    [SerializeField] TMP_Text textRotorError = null;

    public Transform PanelErrorRotor => panelErrorRotor;
    public TMP_Text TextRotorError => textRotorError;
    #endregion

    #region Error Notch
    [SerializeField] Transform panelErrorNotch = null;
    [SerializeField] TMP_Text textNotchError = null;

    public Transform PanelErrorNotch => panelErrorNotch;
    public TMP_Text TextNotchError => textNotchError;
    #endregion

    public bool IsValidUI => IsValidInputResult && IsValidQuit && IsValidReset && IsValidRotorErrorUI && IsValidNotchErrorUI;
    public bool IsValidReset => resetButton;
    public bool IsValidQuit => quitButton;
    public bool IsValidInputResult => enterText && resultPanel && resultText;
    public bool IsValidRotorErrorUI => panelErrorRotor && textRotorError;
    public bool IsValidNotchErrorUI => panelErrorNotch && textNotchError;

    protected override void Awake()
    {
        base.Awake();
        resetButton.onClick.AddListener(AllReset);
        quitButton.onClick.AddListener(Quit);
        enterText.onValueChanged.AddListener((_string) => Result());
    }

    private void OnDestroy()
    {
        resetButton.onClick.RemoveListener(AllReset);
        quitButton.onClick.RemoveListener(Quit);
        enterText.onValueChanged.RemoveListener((_string) => Result());
    }

    public void AllReset()
    {
        if (!IsValidReset) return;
        EA_RotorManager.Instance.ResetAllRotors();
        EA_LightsManager.Instance.LightReset();
        enterText.text = "";
        resultText.text = "";
    }

    public void SetActiveUI(Transform _panel, bool _show)
    {
        if (_show) ShowUI(_panel);
        else HideUI(_panel);
    }

    public void ShowUI(Transform _panel)
    {
        if (!IsValidUI) return;
        _panel.gameObject.SetActive(true);
    }

    public void HideUI(Transform _panel)
    {
        if (!IsValidUI) return;
        _panel.gameObject.SetActive(false);
    }

    public void AddText(TMP_Text _text, string _addText)
    {
        _text.text += $"{_addText}";
    }

    public void ResetText(TMP_Text _text)
    {
        _text.text = $"";
    }

    public void Quit()
    {
        if (!IsValidQuit) return;
        Application.Quit();
    }

    public char GetLastLetter()
    {
        if (!IsValidInputResult) return '\0';
        if (string.IsNullOrEmpty(enterText.text)) return '\0';
        char[] allChars = enterText.text.ToUpper().ToCharArray();
        char _lastChar = allChars[allChars.Length-1];
        if(Regex.Match(_lastChar.ToString(),@"[A-Z]|[a-z]").Success) return _lastChar;
        return '\0';
    }

    public void AddLetter(char _letter)
    {
        if (_letter.Equals('\0')) return;
        resultText.text += $"{_letter}";
    }

    public void Result()
    {
        char _lastLetter = GetLastLetter();
        char _lastLetterCrypted = EA_Enigma.Instance.Decode(_lastLetter);
        if (_lastLetter.Equals('\0')) return;
        AddLetter(_lastLetterCrypted);
    }
}
