// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanIdentity
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  public class LoanIdentity : ILoanIdentity
  {
    private LoanIdentity id;

    internal LoanIdentity(LoanIdentity id) => this.id = id;

    public string Guid => this.id.Guid;

    public string LoanFolder => this.id.LoanFolder;

    public string LoanName => this.id.LoanName;

    public override string ToString() => this.id.ToString();

    internal static LoanIdentityList ToList(LoanIdentity[] ids)
    {
      LoanIdentityList list = new LoanIdentityList();
      for (int index = 0; index < ids.Length; ++index)
        list.Add(new LoanIdentity(ids[index]));
      return list;
    }
  }
}
