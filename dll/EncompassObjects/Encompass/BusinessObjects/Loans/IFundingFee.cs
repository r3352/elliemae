// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.IFundingFee
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans
{
  [Guid("944C38F8-D511-4143-8E1A-A1FAAE3E2E71")]
  public interface IFundingFee
  {
    string LineID { get; }

    string CDLineID { get; }

    bool BalanceChecked { get; }

    string FeeDescription { get; }

    string FeeDescription2015 { get; }

    string Payee { get; }

    string PaidBy { get; }

    string PaidTo { get; }

    double Amount { get; }

    double POCAmount { get; }

    string POCPaidBy { get; }

    double PTCAmount { get; }

    string PTCPaidBy { get; }

    double POCBorrower2015 { get; }

    double POCSeller2015 { get; }

    double POCBroker2015 { get; }

    double POCLender2015 { get; }

    double POCOther2015 { get; }

    double PACBroker2015 { get; }

    double PACLender2015 { get; }

    double PACOther2015 { get; }
  }
}
