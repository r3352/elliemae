// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LoanDetail
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class LoanDetail
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

    public LoanDetail(string loanId, string loanNo, string lockId)
    {
      this.loanid = loanId;
      this.loanNo = loanNo;
      this.lockid = lockId;
    }
  }
}
