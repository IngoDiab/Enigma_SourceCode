using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class EA_UIManager : EA_Singleton<EA_UIManager>
{
    [SerializeField] Image resultPanel = null;
    [SerializeField] Button resetButton = null;
    [SerializeField] Button quitButton = null;

    [SerializeField] TMP_InputField enterText = null;
    [SerializeField] TMP_Text resultText = null;

    public bool IsValidUI => IsValidInputResult && IsValidQuit && IsValidReset;
    public bool IsValidReset => resetButton;
    public bool IsValidQuit => quitButton;
    public bool IsValidInputResult => enterText && resultPanel && resultText;

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

    /*public void SetActiveUI(bool _show)
    {
        if (_show) ShowUI();
        else HideUI();
    }

    public void ShowUI()
    {
        if (!IsValidUI) return;
        resetButton.gameObject.SetActive(true);
        enterText.gameObject.SetActive(true);
        resultPanel.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        if (!IsValidUI) return;
        resetButton.gameObject.SetActive(false);
        enterText.gameObject.SetActive(false);
        resultPanel.gameObject.SetActive(false);
    }*/

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
        //if(char.IsLetter(_lastChar) && !_lastChar.Equals('é') && !_lastChar.Equals('è')) return _lastChar;
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
