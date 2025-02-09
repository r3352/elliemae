// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.LoanDetailWithLock
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  internal class LoanDetailWithLock
  {
    private string loanid = string.Empty;
    private string loanNo = string.Empty;
    private string lockid = string.Empty;

    public string id
    {
      get => this.loanid;
      set => this.loanid = value;
    }

    public string loanNumber
    {
      get => this.loanNo;
      set => this.loanNo = value;
    }

    public string lockId
    {
      get => this.lockid;
      set => this.lockid = value;
    }

    public LoanDetailWithLock(string loanId, string loanNo, string lockId)
    {
      this.loanid = loanId;
      this.loanNo = loanNo;
      this.lockid = lockId;
    }
  }
}
