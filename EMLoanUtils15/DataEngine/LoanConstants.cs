// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanConstants
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LoanConstants
  {
    public static readonly string[] ConditionCategories = new string[6]
    {
      "Assets",
      "Credit",
      "Income",
      "Legal",
      "Misc",
      "Property"
    };
    public static readonly string[] PriorToValues = new string[5]
    {
      "PTA",
      "PTD",
      "PTF",
      "AC",
      "PTP"
    };
    public static readonly string[] PriorToUIValues = new string[5]
    {
      "Approval",
      "Docs",
      "Funding",
      "Closing",
      "Purchase"
    };
    public static readonly string[] PostClosingConditionSources = new string[13]
    {
      "Escrow",
      "Investor",
      "Recorder's Office",
      "Borrowers",
      "FHA",
      "VA",
      "MI Company",
      "Other",
      "Manual",
      "Condition Set",
      "Automated Conditions",
      "Freddie Mac",
      "Fannie Mae"
    };

    public static string PriorToUIConversion(string priorTo)
    {
      switch (priorTo)
      {
        case "PTA":
          return "Approval";
        case "PTD":
          return "Docs";
        case "PTF":
          return "Funding";
        case "AC":
          return "Closing";
        case "PTP":
          return "Purchase";
        default:
          return string.Empty;
      }
    }

    public static string PriorToValueConversion(string priorToUI)
    {
      switch (priorToUI)
      {
        case "Approval":
          return "PTA";
        case "Docs":
          return "PTD";
        case "Funding":
          return "PTF";
        case "Closing":
          return "AC";
        case "Purchase":
          return "PTP";
        default:
          return string.Empty;
      }
    }
  }
}
