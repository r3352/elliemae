// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.BatchPrintRecord
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  [Serializable]
  public class BatchPrintRecord
  {
    private string loanNumber = string.Empty;
    private string loanGUID = string.Empty;
    private string borrowerName = string.Empty;
    private string errorMessage = string.Empty;
    private string formName = string.Empty;
    private bool canPrint;

    public BatchPrintRecord() => this.canPrint = false;

    public BatchPrintRecord(
      string loanGUID,
      string loanNumber,
      string borrowerName,
      string formName,
      bool canPrint,
      string errorMessage)
    {
      this.loanGUID = loanGUID;
      this.LoanNumber = loanNumber;
      this.borrowerName = borrowerName;
      this.errorMessage = errorMessage;
      this.formName = formName;
      this.canPrint = canPrint;
    }

    public string LoanNumber
    {
      set => this.loanNumber = value;
      get => this.loanNumber;
    }

    public string LoanGUID
    {
      set => this.loanGUID = value;
      get => this.loanGUID;
    }

    public string BorrowerName
    {
      set => this.borrowerName = value;
      get => this.borrowerName;
    }

    public string ErrorMessage
    {
      set => this.errorMessage = value;
      get => this.errorMessage;
    }

    public string FormName
    {
      set => this.formName = value;
      get => this.formName;
    }

    public bool CanPrint
    {
      set => this.canPrint = value;
      get => this.canPrint;
    }
  }
}
