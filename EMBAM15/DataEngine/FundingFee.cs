// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FundingFee
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FundingFee
  {
    private string lineID = string.Empty;
    private string cdLineID = "";
    private bool balanceChecked;
    private string feeDescription = string.Empty;
    private string payee = string.Empty;
    private string paidBy = string.Empty;
    private string paidTo = string.Empty;
    private double amount;
    private double pocAmount;
    private string pocPaidBy = string.Empty;
    private double ptcAmount;
    private string ptcPaidBy = string.Empty;
    private object tag;
    private double pocBorrower2015;
    private double pocSeller2015;
    private double pocBroker2015;
    private double pocLender2015;
    private double pocOther2015;
    private double pacBroker2015;
    private double pacLender2015;
    private double pacOther2015;

    public FundingFee()
    {
    }

    public FundingFee(
      string lineID,
      bool balanceChecked,
      string feeDescription,
      string payee,
      string paidBy,
      string paidTo,
      double amount,
      double pocBorrower2015,
      double pocSeller2015,
      double pocBroker2015,
      double pocLender2015,
      double pocOther2015,
      double pacBroker2015,
      double pacLender2015,
      double pacOther2015)
      : this(lineID, balanceChecked, feeDescription, payee, paidBy, paidTo, amount, 0.0, "", 0.0, "")
    {
      this.pocBorrower2015 = pocBorrower2015;
      this.pocSeller2015 = pocSeller2015;
      this.pocBroker2015 = pocBroker2015;
      this.pocLender2015 = pocLender2015;
      this.pocOther2015 = pocOther2015;
      this.pacBroker2015 = pacBroker2015;
      this.pacLender2015 = pacLender2015;
      this.pacOther2015 = pacOther2015;
    }

    public FundingFee(
      string lineID,
      bool balanceChecked,
      string feeDescription,
      string payee,
      string paidBy,
      string paidTo,
      double amount,
      double pocAmount,
      string pocPaidBy,
      double ptcAmount,
      string ptcPaidBy)
    {
      this.lineID = lineID;
      this.balanceChecked = balanceChecked;
      this.feeDescription = feeDescription;
      this.payee = payee;
      this.paidBy = paidBy;
      this.paidTo = paidTo;
      this.amount = amount;
      this.pocAmount = pocAmount;
      this.pocPaidBy = pocPaidBy;
      this.ptcAmount = ptcAmount;
      this.ptcPaidBy = ptcPaidBy;
    }

    public string LineID
    {
      get => this.lineID;
      set => this.lineID = value;
    }

    public int LineNumber => FundingFee.getLineNumber(this.lineID);

    private static int getLineNumber(string lineID)
    {
      if (lineID == "")
        return -1;
      string str = lineID.EndsWith(".") ? lineID.Substring(0, lineID.Length - 1) : lineID;
      if (str == "")
        return -1;
      if (!char.IsNumber(str[str.Length - 1]))
        str = str.Substring(0, str.Length - 1);
      return Utils.ParseInt((object) str, -1);
    }

    public string CDLineID
    {
      get => !(this.cdLineID == "") ? this.cdLineID : FundingFee.GetCDLineID(this.LineNumber);
      set => this.cdLineID = value;
    }

    public static string GetCDLineID(int lineNumber)
    {
      switch (lineNumber)
      {
        case 1:
          return "K.03";
        case 2:
          return "K.06";
        case 3:
          return "K.07";
        case 4:
          return "K.15";
        case 5:
          return "L.05";
        case 6:
          return "L.06";
        case 7:
          return "L.07";
        case 8:
          return "L.08";
        case 9:
          return "L.09";
        case 10:
          return "L.10";
        case 11:
          return "L.11";
        case 12:
          return "M.06";
        case 13:
          return "M.07";
        case 14:
          return "M.08";
        case 15:
          return "M.16";
        case 16:
          return "N.02";
        case 17:
          return "N.08";
        case 18:
          return "N.11";
        case 19:
          return "N.12";
        case 20:
          return "N.13";
        case 101:
          return "K.01";
        case 102:
          return "K.02";
        case 104:
          return "K.04";
        case 105:
          return "K.05";
        case 106:
          return "K.08";
        case 107:
          return "K.09";
        case 108:
          return "K.10";
        case 109:
          return "K.11";
        case 110:
          return "K.12";
        case 111:
          return "K.13";
        case 112:
          return "K.14";
        case 201:
          return "L.01";
        case 202:
          return "L.02";
        case 203:
          return "L.03";
        case 204:
          return "L.04";
        case 210:
          return "L.12";
        case 211:
          return "L.13";
        case 212:
          return "L.14";
        case 213:
          return "L.15";
        case 214:
          return "L.16";
        case 215:
          return "L.17";
        case 401:
          return "M.01";
        case 402:
          return "M.02";
        case 403:
          return "M.03";
        case 404:
          return "M.04";
        case 405:
          return "M.05";
        case 406:
          return "M.09";
        case 407:
          return "M.10";
        case 408:
          return "M.11";
        case 409:
          return "M.12";
        case 410:
          return "M.13";
        case 411:
          return "M.14";
        case 412:
          return "M.15";
        case 501:
          return "N.01";
        case 503:
          return "N.03";
        case 504:
          return "N.04";
        case 505:
          return "N.05";
        case 506:
          return "N.06";
        case 507:
          return "N.07";
        case 508:
          return "N.09";
        case 509:
          return "N.10";
        case 510:
          return "N.14";
        case 511:
          return "N.15";
        case 512:
          return "N.16";
        case 513:
          return "N.17";
        case 514:
          return "N.18";
        case 515:
          return "N.19";
        default:
          return lineNumber >= 21 && lineNumber <= 45 ? "K." + (lineNumber - 20).ToString("00") : "";
      }
    }

    public bool BalanceChecked
    {
      get => this.balanceChecked;
      set => this.balanceChecked = value;
    }

    public string FeeDescription
    {
      get => this.feeDescription;
      set => this.feeDescription = value;
    }

    public string FeeDescription2015
    {
      get => FundingFee.GetFeeDescription2015(this.lineID, this.feeDescription);
    }

    public static string GetFeeDescription2015(string lineID, string originalDescription)
    {
      int lineNumber = FundingFee.getLineNumber(lineID);
      switch (lineID)
      {
        case "701.":
        case "702.":
          return "Real Estate Commission";
        case "801a.":
          return "Origination Fee";
        case "801e.":
          return "Broker Fee";
        case "901.":
          return "Prepaid Interest";
        case "902.":
          return "Mortgage Insurance Premium";
        case "903.":
          return "Homeowner's Insurance Premium";
        case "1002.":
          return "Homeowner's Insurance";
        case "1003.":
          return "Mortgage Insurance";
        case "1006.":
          return "Flood Insurance";
        case "1010.":
          return "Mortgage Insurance";
        case "1103.":
          return "Title - " + originalDescription + " (optional)";
        default:
          if ((lineID.StartsWith("110") || lineID.StartsWith("111")) && lineNumber >= 520)
            return "Title - " + originalDescription;
          if (lineNumber >= 21 && lineNumber <= 45 || lineNumber >= 520)
            return originalDescription;
          switch (FundingFee.GetCDLineID(lineNumber))
          {
            case "K.01":
            case "M.01":
              return "Sale Price of Property";
            case "K.02":
            case "M.02":
              return "Sale Price of Any Personal Property Included in Sale";
            case "K.03":
            case "N.02":
              return "Closing Costs Paid at Closing (J)";
            case "K.08":
            case "L.12":
            case "M.09":
            case "N.14":
              return "City/Town Taxes";
            case "K.09":
            case "L.13":
            case "M.10":
            case "N.15":
              return "County Taxes";
            case "K.10":
            case "L.14":
            case "M.11":
            case "N.16":
              return "Assessments";
            case "L.01":
              return "Deposit";
            case "L.02":
              return "Loan Amount";
            case "L.03":
            case "N.03":
              return "Existing Loan(s) Assumed or Taken Subject to";
            case "L.05":
            case "N.08":
              return "Seller Credit";
            case "N.01":
              return "Excess Deposit";
            case "N.04":
              return "Payoff of First Mortgage Loan";
            case "N.05":
              return "Payoff of Second Mortgage Loan";
            default:
              return originalDescription;
          }
      }
    }

    public string Payee
    {
      get => this.payee;
      set => this.payee = value;
    }

    public string PaidBy
    {
      get => this.paidBy;
      set => this.paidBy = value;
    }

    public string PaidTo
    {
      get => this.paidTo;
      set => this.paidTo = value;
    }

    public double Amount
    {
      get => this.amount;
      set => this.amount = value;
    }

    public double POCAmount
    {
      get => this.pocAmount;
      set => this.pocAmount = value;
    }

    public string POCPaidBy
    {
      get => this.pocPaidBy;
      set => this.pocPaidBy = value;
    }

    public double PTCAmount
    {
      get => this.ptcAmount;
      set => this.ptcAmount = value;
    }

    public string PTCPaidBy
    {
      get => this.ptcPaidBy;
      set => this.ptcPaidBy = value;
    }

    public object Tag
    {
      get => this.tag;
      set => this.tag = value;
    }

    public double POCBorrower2015
    {
      get => this.pocBorrower2015;
      set => this.pocBorrower2015 = value;
    }

    public double POCSeller2015
    {
      get => this.pocSeller2015;
      set => this.pocSeller2015 = value;
    }

    public double POCBroker2015
    {
      get => this.pocBroker2015;
      set => this.pocBroker2015 = value;
    }

    public double POCLender2015
    {
      get => this.pocLender2015;
      set => this.pocLender2015 = value;
    }

    public double POCOther2015
    {
      get => this.pocOther2015;
      set => this.pocOther2015 = value;
    }

    public double PACBroker2015
    {
      get => this.pacBroker2015;
      set => this.pacBroker2015 = value;
    }

    public double PACLender2015
    {
      get => this.pacLender2015;
      set => this.pacLender2015 = value;
    }

    public double PACOther2015
    {
      get => this.pacOther2015;
      set => this.pacOther2015 = value;
    }
  }
}
