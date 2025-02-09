// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ILoanConsole
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public interface ILoanConsole
  {
    bool OpenLoan(string guid, LoanInfo.LockReason lockReason, bool displayEditor);

    bool OpenLoan(
      string guid,
      LoanInfo.LockReason lockReason,
      bool displayEditor,
      bool interactive);

    bool OpenLoan(string guid, bool displayEditor);

    bool OpenLoan(string guid, bool displayEditor, bool interactive);

    bool OpenLoan(string guid);

    bool OpenLoan(LoanDataMgr loan, bool displayEditor);

    bool OpenNewLoan(
      string loanFolder,
      string loanName,
      LoanTemplateSelection template,
      bool displayEditor);

    bool StartNewLoan(bool displayEditor);

    bool StartNewLoan(string loanFolder, bool displayEditor);

    void DisplayEditor();

    bool HasOpenLoan { get; }

    bool CloseLoan(bool allowCancel);

    bool CloseLoanWithoutPrompts(bool saveChanges);

    bool SaveLoan(bool enableRateLockValidation = false, bool enableBackupLoanFile = false);

    bool SaveLoan(bool mergeOnly, bool enableRateLockValidation = false, bool enableBackupLoanFile = false);

    void PerformAutoSave();

    bool HasAccessToLoanTab { get; }
  }
}
