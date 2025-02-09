// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FedTresholdAdjustment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FedTresholdAdjustment
  {
    public FedTresholdAdjustment(
      int id,
      int ruleIndex,
      int adjustmentYear,
      string lowerRange,
      string upperRange,
      string ruleValue,
      string ruleType,
      DateTime lastModifiedDTTM)
    {
      this.ID = id;
      this.RuleIndex = ruleIndex;
      this.AdjustmentYear = adjustmentYear;
      this.LowerRange = lowerRange;
      this.UpperRange = upperRange;
      this.RuleValue = ruleValue;
      this.RuleType = ruleType;
      this.LastModifiedDateTime = lastModifiedDTTM;
    }

    public FedTresholdAdjustment()
    {
    }

    public int ID { get; set; }

    public int RuleIndex { get; set; }

    public int AdjustmentYear { get; set; }

    public string LowerRange { get; set; }

    public string UpperRange { get; set; }

    public string RuleValue { get; set; }

    public string RuleType { get; set; }

    public DateTime LastModifiedDateTime { get; set; }
  }
}
