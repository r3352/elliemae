// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BidTapeManagement.BidTapeField
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.BidTapeManagement
{
  [Serializable]
  public class BidTapeField
  {
    public string BidTapeTemplateGuid { get; set; }

    public int SequenceNumber { get; set; }

    public string BidTapeFieldName { get; set; }

    public BidTapeField.TargetTypes TargetType { get; set; }

    public string TargetField { get; set; }

    public BidTapeField.LookupEnumerationIndicators LookupEnumerationIndicator { get; set; }

    public BidTapeField.ErrorIndicators ErrorIndicator { get; set; }

    public bool UserRequiredFieldIndicator { get; set; }

    public bool AppRequiredFieldIndicator { get; set; }

    public string TargetTypeStr
    {
      get => BidTapeField.TargetTypeToString(this.TargetType);
      set => this.TargetType = BidTapeField.TargetTypeFromString(value);
    }

    public string LookupEnumerationIndicatorStr
    {
      get => BidTapeField.LookupEnumerationIndicatorToString(this.LookupEnumerationIndicator);
      set
      {
        this.LookupEnumerationIndicator = BidTapeField.LookupEnumerationIndicatorFromString(value);
      }
    }

    public string ErrorIndicatorStr
    {
      get => BidTapeField.ErrorIndicatorToString(this.ErrorIndicator);
      set => this.ErrorIndicator = BidTapeField.ErrorIndicatorFromString(value);
    }

    public static BidTapeField.TargetTypes TargetTypeFromString(string val)
    {
      switch (val)
      {
        case "Loan Field":
          return BidTapeField.TargetTypes.LoanField;
        case "Price Field":
          return BidTapeField.TargetTypes.PriceField;
        case "Trade Field":
          return BidTapeField.TargetTypes.TradeField;
        default:
          return BidTapeField.TargetTypes.NoTarget;
      }
    }

    public static string TargetTypeToString(BidTapeField.TargetTypes val)
    {
      switch (val)
      {
        case BidTapeField.TargetTypes.LoanField:
          return "Loan Field";
        case BidTapeField.TargetTypes.PriceField:
          return "Price Field";
        case BidTapeField.TargetTypes.TradeField:
          return "Trade Field";
        default:
          return "No Target";
      }
    }

    public static BidTapeField.LookupEnumerationIndicators LookupEnumerationIndicatorFromString(
      string val)
    {
      BidTapeField.LookupEnumerationIndicators result;
      return val != null && Enum.TryParse<BidTapeField.LookupEnumerationIndicators>(val, out result) ? result : BidTapeField.LookupEnumerationIndicators.None;
    }

    public static string LookupEnumerationIndicatorToString(
      BidTapeField.LookupEnumerationIndicators val)
    {
      return val != BidTapeField.LookupEnumerationIndicators.None ? val.ToString() : (string) null;
    }

    public static BidTapeField.ErrorIndicators ErrorIndicatorFromString(string val)
    {
      BidTapeField.ErrorIndicators result;
      return val != null && Enum.TryParse<BidTapeField.ErrorIndicators>(val, out result) ? result : BidTapeField.ErrorIndicators.Ignore;
    }

    public static string ErrorIndicatorToString(BidTapeField.ErrorIndicators val) => val.ToString();

    public enum TargetTypes
    {
      NoTarget,
      LoanField,
      PriceField,
      TradeField,
    }

    public enum DataFormats
    {
      String,
      Numeric,
      Integer,
    }

    public enum LookupEnumerationIndicators
    {
      None,
      Lookup,
      Enumeration,
    }

    public enum ErrorIndicators
    {
      Ignore,
      Warning,
      Reject,
    }
  }
}
