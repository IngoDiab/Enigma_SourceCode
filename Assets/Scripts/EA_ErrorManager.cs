using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EA_ErrorManager : EA_Singleton<EA_ErrorManager>
{
    #region Rotor
    string msgRotor = "";
    List<string> errorsRotor = new List<string>();
    bool issueWithRotor = false;
    public string MsgRotor => msgRotor;
    public List<string> ErrorsRotor => errorsRotor;

    public string ErrorRotor(int _id)
    {
        string _rotorError = "";
        _rotorError += $"There is an error on rotor {_id}\n";
        return _rotorError;
    }

    public void MergeErrorsRotor(List<string> _errors)
    {
        msgRotor = "";
        foreach (string _error in _errors)
        {
            msgRotor += _error;
        }
    }

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

    public string ErrorNotch(int _id)
    {
        string _notchError = "";
        _notchError += $"There is an error on notch {_id}\n";
        return _notchError;
    }

    public void MergeErrorsNotch(List<string> _errors)
    {
        msgNotch = "";
        foreach (string _error in _errors)
        {
            msgNotch += _error;
        }
    }

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
