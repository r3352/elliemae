// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.LoanUpdateResults
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class LoanUpdateResults
  {
    public List<string> AllLoans { get; private set; }

    public List<string> ProcessedLoans { get; set; }

    public Dictionary<string, string> LoansWithError { get; set; }

    public string TradeError { get; set; }

    public bool IsTradeError => !string.IsNullOrEmpty(this.TradeError);

    public LoanUpdateResults(List<string> allLoans)
    {
      this.AllLoans = allLoans;
      this.ProcessedLoans = new List<string>();
      this.LoansWithError = new Dictionary<string, string>();
    }

    public string ToSummary()
    {
      if (this.IsTradeError)
        return "Trade Error: " + this.TradeError;
      int num1 = this.AllLoans != null ? this.AllLoans.Count<string>() : 0;
      int num2 = this.ProcessedLoans != null ? this.ProcessedLoans.Count<string>() : 0;
      int num3 = this.LoansWithError != null ? this.LoansWithError.Count<KeyValuePair<string, string>>() : 0;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Completed Result : Processed: " + (object) num2 + ", ");
      stringBuilder.Append("Successful: " + (object) (num2 - num3) + ", ");
      stringBuilder.Append("Errors: " + (object) num3 + ", ");
      stringBuilder.Append("Not Processed: " + (object) (num1 - num2));
      return stringBuilder.ToString();
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(this.ToSummary());
      if (this.LoansWithError != null && this.LoansWithError.Count<KeyValuePair<string, string>>() > 0)
      {
        stringBuilder.AppendLine("Detailed Errors: ");
        foreach (string key in this.LoansWithError.Keys)
          stringBuilder.AppendLine(this.LoansWithError[key]);
      }
      return stringBuilder.ToString();
    }
  }
}
