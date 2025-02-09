// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.BidTapePricingFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine.Log;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class BidTapePricingFields
  {
    public static class PricingFields
    {
      public static string[] basePricingFields = new string[42]
      {
        "2101",
        "2102",
        "2103",
        "2104",
        "2105",
        "2106",
        "2107",
        "2108",
        "2109",
        "2110",
        "2111",
        "2112",
        "2113",
        "2114",
        "2115",
        "2116",
        "2117",
        "2118",
        "2119",
        "2120",
        "2121",
        "2122",
        "2123",
        "2124",
        "2125",
        "2126",
        "2128",
        "2129",
        "2130",
        "2131",
        "2132",
        "2133",
        "2134",
        "2135",
        "2136",
        "2137",
        "2138",
        "2139",
        "2140",
        "2141",
        "4201",
        "3907"
      };
      public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

      static PricingFields()
      {
        foreach (string basePricingField in BidTapePricingFields.PricingFields.basePricingFields)
        {
          FieldDefinition baseField = StandardFields.All[basePricingField];
          if (baseField != null)
          {
            BidTapePricingFields.TargetTradePricingField field = new BidTapePricingFields.TargetTradePricingField(baseField);
            if (!BidTapePricingFields.PricingFields.All.Contains(field.FieldID))
              BidTapePricingFields.PricingFields.All.Add((FieldDefinition) field);
          }
        }
      }

      internal static void AddField(VirtualField field)
      {
        if (BidTapePricingFields.PricingFields.All.Contains(field.FieldID))
          return;
        BidTapePricingFields.PricingFields.All.Add((FieldDefinition) field);
      }
    }

    public class TargetTradePricingField : BankerEditionVirtualField
    {
      public const string FieldPrefix = "TGTTRADE";
      private string fieldID = string.Empty;
      private FieldDefinition baseField;

      public string BaseFieldID => this.fieldID;

      public TargetTradePricingField(FieldDefinition baseField)
        : base(BidTapePricingFields.TargetTradePricingField.GenerateBidTapePricingFieldID(baseField.FieldID), "Bid Tape - " + baseField.Description, baseField.Format)
      {
        this.fieldID = baseField.FieldID;
        this.baseField = baseField;
      }

      public override VirtualFieldType VirtualFieldType => VirtualFieldType.BidTapePricingFields;

      private TargetTradeLog getTradeTargetLog(LoanData loan)
      {
        TargetTradeLog tradeTargetLog = (TargetTradeLog) null;
        foreach (TargetTradeLog targetTradeLog in loan.GetLogList().GetAllRecordsOfType(typeof (TargetTradeLog)))
        {
          if (tradeTargetLog == null || tradeTargetLog.Date < targetTradeLog.Date)
            tradeTargetLog = targetTradeLog;
        }
        return tradeTargetLog;
      }

      private TargetTradeLog getTradeTargetLog(LoanData loan, string tradeId)
      {
        foreach (TargetTradeLog tradeTargetLog in loan.GetLogList().GetAllRecordsOfType(typeof (TargetTradeLog)))
        {
          if (tradeTargetLog.CommitmentNum == tradeId)
            return tradeTargetLog;
        }
        return (TargetTradeLog) null;
      }

      protected override string Evaluate(LoanData loan)
      {
        TargetTradeLog tradeTargetLog = this.getTradeTargetLog(loan);
        return tradeTargetLog == null ? string.Empty : this.Evaluate(tradeTargetLog);
      }

      public string Evaluate(TargetTradeLog log)
      {
        if (this.BaseFieldID == "2101")
          return log.BasePrice.ToString();
        if (this.BaseFieldID == "4201")
          return log.SRPPaidOut.ToString();
        if (this.BaseFieldID == "CommitmentNum")
          return log.CommitmentNum;
        int result = -1;
        if (!int.TryParse(this.BaseFieldID, out result))
          return string.Empty;
        int num = result - 2100;
        bool flag = num % 2 == 0;
        int idx = num / 2;
        if (log[idx] == null)
          return (string) null;
        return flag ? log[idx].Description : log[idx].Rate.ToString();
      }

      public override void SetValue(LoanData loan, string value)
      {
        TargetTradeLog targetTradeLog = this.getTradeTargetLog(loan);
        if (targetTradeLog == null)
        {
          targetTradeLog = new TargetTradeLog(DateTime.Now, (string) null, Array.Empty<PriceAdjustmentLogRecord>());
          loan.GetLogList().AddRecord((LogRecordBase) targetTradeLog);
        }
        this.SetValue(targetTradeLog, value);
        targetTradeLog.MarkAsDirty(this.FieldID);
      }

      public void SetValue(TargetTradeLog log, string value)
      {
        Decimal result1 = 0M;
        if (this.BaseFieldID == "2101" && Decimal.TryParse(value, out result1))
          log.BasePrice = result1;
        else if (this.BaseFieldID == "4201" && Decimal.TryParse(value, out result1))
          log.SRPPaidOut = result1;
        else if (this.BaseFieldID == "CommitmentNum")
        {
          log.CommitmentNum = value;
        }
        else
        {
          int result2 = -1;
          if (!int.TryParse(this.BaseFieldID, out result2))
            return;
          int num = result2 - 2100;
          bool flag = num % 2 == 0;
          int idx = num / 2;
          if (log[idx] == null)
            log[idx] = new PriceAdjustmentLogRecord();
          if (flag)
          {
            log[idx].Description = value;
          }
          else
          {
            if (!Decimal.TryParse(value, out result1))
              return;
            log[idx].Rate = result1;
          }
        }
      }

      public override bool AllowEdit => true;

      public override FieldOptionCollection Options => this.baseField.Options;

      public override int ReportingDatabaseColumnSize => base.ReportingDatabaseColumnSize;

      public override ReportingDatabaseColumnType ReportingDatabaseColumnType
      {
        get => this.baseField.ReportingDatabaseColumnType;
      }

      public static string GenerateBidTapePricingFieldID(string baseFieldID)
      {
        return "TGTTRADE." + baseFieldID;
      }
    }
  }
}
