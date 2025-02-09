// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.LoanDataSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public abstract class LoanDataSource : RemotableDataSource
  {
    private LoanData loan;
    private UserInfo currentUser;

    protected LoanDataSource(LoanData loan, UserInfo currentUser, bool readOnly)
      : base(readOnly)
    {
      this.loan = loan;
      this.currentUser = currentUser;
    }

    public LoanData Loan => this.loan;

    public UserInfo CurrentUser => this.currentUser;

    public override void Dispose()
    {
      base.Dispose();
      this.loan = (LoanData) null;
      this.currentUser = (UserInfo) null;
    }

    public override object Clone() => (object) this.MemberwiseClone(false);
  }
}
