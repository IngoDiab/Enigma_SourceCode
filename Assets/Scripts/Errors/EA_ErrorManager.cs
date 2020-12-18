using System.Collections.Generic;

public class EA_ErrorManager : EA_Singleton<EA_ErrorManager>
{
    #region Rotor
    string msgRotor = "";
    List<string> errorsRotor = new List<string>();
    bool issueWithRotor = false;
    public string MsgRotor => msgRotor;
    public List<string> ErrorsRotor => errorsRotor;

    /// <summary>
    /// Save a message for the error
    /// </summary>
    /// <param name="_id">Rotor which have an issue</param>
    /// <returns></returns>
    public string ErrorRotor(int _id)
    {
        string _rotorError = "";
        _rotorError += $"There is an error on rotor {_id}\n";
        return _rotorError;
    }

    /// <summary>
    /// Merge all saved messages
    /// </summary>
    /// <param name="_errors">All errors</param>
    public void MergeErrorsRotor(List<string> _errors)
    {
        msgRotor = "";
        foreach (string _error in _errors)
        {
            msgRotor += _error;
        }
    }

    /// <summary>
    /// Set UIErrorRotor if there is at least one error
    /// </summary>
    public void SetUIErrorPanelRotor()
    {
        EA_UIManager.Instance.ResetText(EA_UIManager.Instance.TextRotorError);
        EA_UIManager.Instance.AddText(EA_UIManager.Instance.TextRotorError, msgRotor);
        issueWithRotor = !string.IsNullOrEmpty(msgRotor);
        EA_UIManager.Instance.SetActiveUI(EA_UIManager.Instance.PanelErrorRotor, issueWithRotor);
        EA_UIManager.Instance.EnterText.readOnly = issueWithRotor || issueWithNotch;
        errorsRotor.Clear();
    }
    #endregion

    #region Notch
    string msgNotch = "";
    List<string> errorsNotch = new List<string>();
    bool issueWithNotch = false;

    public string MsgNotch => msgNotch;
    public List<string> ErrorsNotch => errorsNotch;

    /// <summary>
    /// Save a message for the error
    /// </summary>
    /// <param name="_id">Notch which have an issue</param>
    /// <returns></returns>
    public string ErrorNotch(int _id)
    {
        string _notchError = "";
        _notchError += $"There is an error on notch {_id}\n";
        return _notchError;
    }

    /// <summary>
    /// Merge all saved messages
    /// </summary>
    /// <param name="_errors">All errors</param>
    public void MergeErrorsNotch(List<string> _errors)
    {
        msgNotch = "";
        foreach (string _error in _errors)
        {
            msgNotch += _error;
        }
    }

    /// <summary>
    /// Set UIErrorNotch if there is at least one error
    /// </summary>
    public void SetUIErrorPanelNotch()
    {
        EA_UIManager.Instance.ResetText(EA_UIManager.Instance.TextNotchError);
        EA_UIManager.Instance.AddText(EA_UIManager.Instance.TextNotchError, msgNotch);
        issueWithNotch = !string.IsNullOrEmpty(msgNotch);
        EA_UIManager.Instance.SetActiveUI(EA_UIManager.Instance.PanelErrorNotch, issueWithNotch);
        EA_UIManager.Instance.EnterText.readOnly = issueWithRotor || issueWithNotch;
        errorsNotch.Clear();
    }
    #endregion
}
