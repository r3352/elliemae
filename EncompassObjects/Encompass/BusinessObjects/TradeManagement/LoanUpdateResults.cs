// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.LoanUpdateResults
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represents loan update results.</summary>
  public class LoanUpdateResults
  {
    /// <summary>
    /// Gets or sets a list of loan numbers which need to be assigned to the trade/pool.
    /// </summary>
    public List<string> AllLoans { get; private set; }

    /// <summary>
    /// Gets or sets a list of loan numbers which have been processed.
    /// </summary>
    public List<string> ProcessedLoans { get; set; }

    /// <summary>
    /// Gets or sets a list of loan numbers and process results if loan assignment failed.
    /// </summary>
    public Dictionary<string, string> LoansWithError { get; set; }

    /// <summary>Gets or sets trade error</summary>
    public string TradeError { get; set; }

    /// <summary>e
    /// Returns true if trade error found
    /// </summary>
    public bool IsTradeError => !string.IsNullOrEmpty(this.TradeError);

    /// <summary>Constructor of LoanUpdateResults.</summary>
    /// <param name="allLoans">a list of Loan Numbers.</param>
    /// <remarks>Create an instance of LoanUpdateResults.</remarks>
    public LoanUpdateResults(List<string> allLoans)
    {
      this.AllLoans = allLoans;
      this.ProcessedLoans = new List<string>();
      this.LoansWithError = new Dictionary<string, string>();
    }

    /// <summary>Get the result summary of loan assignment process.</summary>
    /// <returns>result summary of loan assignment process.</returns>
    /// <remarks>Get the result summary of loan assignment process.</remarks>
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

    /// <summary>Get the result details of loan assignment process.</summary>
    /// <returns>result details of loan assignment process.</returns>
    /// <remarks>Get the result details of loan assignment process, including result summary and detailed error message for each loan.
    /// </remarks>
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
