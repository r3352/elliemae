// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.InterimServicing.EscrowInterestLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.InterimServicing
{
  public class EscrowInterestLog : ServicingTransactionBase
  {
    private string comments = string.Empty;

    public EscrowInterestLog() => this.TransactionType = ServicingTransactionTypes.EscrowInterest;

    public EscrowInterestLog(XmlElement e)
      : base(e)
    {
      this.comments = new AttributeReader(e).GetString(nameof (Comments));
    }

    public override void Add(XmlElement newlog, bool use5DecimalsForIndexRates)
    {
      base.Add(newlog, use5DecimalsForIndexRates);
      newlog.SetAttribute("Type", ServicingTransactionTypes.EscrowInterest.ToString());
      newlog.SetAttribute("Comments", this.comments);
    }

    public DateTime IncurredDate
    {
      get => this.TransactionDate;
      set => this.TransactionDate = value;
    }

    public double InterestAmount
    {
      get => this.TransactionAmount;
      set => this.TransactionAmount = value;
    }

    public string Comments
    {
      get => this.comments;
      set => this.comments = value;
    }
  }
}
