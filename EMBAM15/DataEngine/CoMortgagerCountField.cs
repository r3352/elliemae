// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CoMortgagerCountField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class CoMortgagerCountField : VirtualField
  {
    public CoMortgagerCountField()
      : base("COMORTGAGORCOUNT", "# of Co-Mortgagers", FieldFormat.INTEGER)
    {
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.CoMortgagerFields;

    protected override string Evaluate(LoanData loan)
    {
      int num = 0;
      BorrowerPair[] borrowerPairs = loan.GetBorrowerPairs();
      for (int index = 1; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Borrower.FirstName.Trim() != "")
          ++num;
        if (borrowerPairs[index].CoBorrower.FirstName.Trim() != "")
          ++num;
      }
      return num.ToString();
    }
  }
}
