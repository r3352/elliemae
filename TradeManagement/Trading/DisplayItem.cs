// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.DisplayItem
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

#nullable disable
namespace EllieMae.EMLite.Trading
{
  internal class DisplayItem
  {
    public string TradeFieldDisplayName;
    public string LoanFieldDisplayName;
    public bool IsCommentItem;
    public bool IsChecked;

    public DisplayItem(string tradeFieldDisplayName, string loanFieldDisplayName)
      : this(tradeFieldDisplayName, loanFieldDisplayName, false, true)
    {
    }

    public DisplayItem(
      string tradeFieldDisplayName,
      string loanFieldDisplayName,
      bool isCommentItem)
      : this(tradeFieldDisplayName, loanFieldDisplayName, isCommentItem, false)
    {
    }

    public DisplayItem(
      string tradeFieldDisplayName,
      string loanFieldDisplayName,
      bool isCommentItem,
      bool isChecked)
    {
      this.TradeFieldDisplayName = tradeFieldDisplayName;
      this.LoanFieldDisplayName = loanFieldDisplayName;
      this.IsCommentItem = isCommentItem;
      if (this.IsCommentItem)
        this.IsChecked = false;
      else
        this.IsChecked = isChecked;
    }
  }
}
