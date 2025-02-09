// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.LoanIdentity
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  /// <summary>
  /// Represents the set of identifying information for a single Loan.
  /// </summary>
  public class LoanIdentity : ILoanIdentity
  {
    private EllieMae.EMLite.DataEngine.LoanIdentity id;

    internal LoanIdentity(EllieMae.EMLite.DataEngine.LoanIdentity id) => this.id = id;

    /// <summary>Gets the globally unqiue identifier for the loan.</summary>
    public string Guid => this.id.Guid;

    /// <summary>
    /// Gets the name of the loan folder in which the loan resides.
    /// </summary>
    public string LoanFolder => this.id.LoanFolder;

    /// <summary>Gets the name of the loan.</summary>
    public string LoanName => this.id.LoanName;

    /// <summary>
    /// Provides a string representation of the LoanIdentity object.
    /// </summary>
    /// <returns>The concatenation of the loan folder and name.</returns>
    public override string ToString() => this.id.ToString();

    internal static LoanIdentityList ToList(EllieMae.EMLite.DataEngine.LoanIdentity[] ids)
    {
      LoanIdentityList list = new LoanIdentityList();
      for (int index = 0; index < ids.Length; ++index)
        list.Add(new LoanIdentity(ids[index]));
      return list;
    }
  }
}
