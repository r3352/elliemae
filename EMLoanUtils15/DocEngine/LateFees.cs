// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.LateFees
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class LateFees
  {
    private Decimal lateChargePercent;
    private int lateFeeGracePeriodDays;
    private Decimal lateFeeMin;
    private Decimal lateFeeMax;
    private string basis = string.Empty;

    internal LateFees(DocEngineResponse response)
    {
      if (response.ResponseXml.SelectSingleNode("LateFees/LateChargePercent/@value") != null)
        this.lateChargePercent = Utils.ParseDecimal((object) response.ResponseXml.SelectSingleNode("LateFees/LateChargePercent/@value").Value, 0M);
      if (response.ResponseXml.SelectSingleNode("LateFees/LateFeeGracePeriodDays/@value") != null)
        this.lateFeeGracePeriodDays = Utils.ParseInt((object) response.ResponseXml.SelectSingleNode("LateFees/LateFeeGracePeriodDays/@value").Value, 0);
      if (response.ResponseXml.SelectSingleNode("LateFees/LateFeeMinAmount/@value") != null)
        this.lateFeeMin = Utils.ParseDecimal((object) response.ResponseXml.SelectSingleNode("LateFees/LateFeeMinAmount/@value").Value, 0M);
      if (response.ResponseXml.SelectSingleNode("LateFees/LateFeeMaxAmount/@value") != null)
        this.lateFeeMax = Utils.ParseDecimal((object) response.ResponseXml.SelectSingleNode("LateFees/LateFeeMaxAmount/@value").Value, 0M);
      if (response.ResponseXml.SelectSingleNode("LateFees/Basis/@value") == null)
        return;
      this.basis = response.ResponseXml.SelectSingleNode("LateFees/Basis/@value").Value;
    }

    public Decimal LateChargePercent => this.lateChargePercent;

    public int LateFeeGracePeriodDays => this.lateFeeGracePeriodDays;

    public Decimal LateFeeMin => this.lateFeeMin;

    public Decimal LateFeeMax => this.lateFeeMax;

    public string Basis => this.basis;
  }
}
