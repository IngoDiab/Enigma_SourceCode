using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class EA_UIManager : EA_Singleton<EA_UIManager>
{
    #region F/P

    #region GeneralUI
    [SerializeField] Button resetButton = null;
    [SerializeField] Button quitButton = null;
    #endregion

    #region InputText
    [SerializeField] TMP_InputField enterText = null;
    public TMP_InputField EnterText => enterText;
    #endregion

    #region OutputText
    [SerializeField] Transform resultPanel = null;
    [SerializeField] TMP_Text resultText = null;
    #endregion

    #region Configs
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

    #endregion

    #region UIProperties
    public bool IsValidUI => IsValidInputResult && IsValidQuit && IsValidReset && IsValidRotorErrorUI && IsValidNotchErrorUI;
    public bool IsValidReset => resetButton;
    public bool IsValidQuit => quitButton;
    public bool IsValidInputResult => enterText && resultPanel && resultText;
    public bool IsValidRotorErrorUI => panelErrorRotor && textRotorError;
    public bool IsValidNotchErrorUI => panelErrorNotch && textNotchError;
    #endregion

    #region UnityMethods
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
    #endregion

    #region Methods
    /// <summary>
    /// Reset the lights and rotors
    /// </summary>
    public void AllReset()
    {
        if (!IsValidReset) return;
        EA_RotorManager.Instance.ResetAllRotors();
        EA_LightsManager.Instance.LightReset();
        enterText.text = "";
        resultText.text = "";
    }

    /// <summary>
    /// Show/Hide a panel
    /// </summary>
    /// <param name="_panel">Panel to show/hide</param>
    /// <param name="_show">True if show, false if hide</param>
    public void SetActiveUI(Transform _panel, bool _show)
    {
        if (_show) ShowUI(_panel);
        else HideUI(_panel);
    }

    /// <summary>
    /// Show a panel
    /// </summary>
    /// <param name="_panel">Panel to show</param>
    public void ShowUI(Transform _panel)
    {
        if (!IsValidUI) return;
        _panel.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hide a panel
    /// </summary>
    /// <param name="_panel">Panel to hide</param>
    public void HideUI(Transform _panel)
    {
        if (!IsValidUI) return;
        _panel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Add a text to a TMP_Text
    /// </summary>
    /// <param name="_text">Text to add something</param>
    /// <param name="_addText">Text to add</param>
    public void AddText(TMP_Text _text, string _addText)
    {
        _text.text += $"{_addText}";
    }

    /// <summary>
    /// Reset a TMP_Text
    /// </summary>
    /// <param name="_text">Text to reset</param>
    public void ResetText(TMP_Text _text)
    {
        _text.text = $"";
    }

    /// <summary>
    /// Quit the application
    /// </summary>
    public void Quit()
    {
        if (!IsValidQuit) return;
        Application.Quit(0);
    }

    /// <summary>
    /// Get the last letter of the input field
    /// </summary>
    /// <returns>The last character of the input field or '\0' if there's an issue</returns>
    public char GetLastLetter()
    {
        if (!IsValidInputResult) return '\0';
        if (string.IsNullOrEmpty(enterText.text)) return '\0';
        char[] allChars = enterText.text.ToUpper().ToCharArray();
        char _lastChar = allChars[allChars.Length-1];
        if(Regex.Match(_lastChar.ToString(),@"[A-Z]|[a-z]").Success) return _lastChar;
        return '\0';
    }

    /// <summary>
    /// Add a letter to the result text
    /// </summary>
    /// <param name="_letter">Letter to add</param>
    public void AddLetter(char _letter)
    {
        if (_letter.Equals('\0')) return;
        resultText.text += $"{_letter}";
    }

    /// <summary>
    /// Manage the result (from input text to result text)
    /// </summary>
    public void Result()
    {
        char _lastLetter = GetLastLetter();
        char _lastLetterCrypted = EA_Enigma.Instance.Encrypt(_lastLetter);
        if (_lastLetter.Equals('\0')) return;
        AddLetter(_lastLetterCrypted);
    }
    #endregion
}
