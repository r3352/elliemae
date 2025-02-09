// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentMasterInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentMasterInfo : MasterCommitmentBase
  {
    private CorrespondentMasterCalculation _calculation;

    public string TpoId { get; set; }

    public string CompanyName { get; set; }

    public string OrganizationId { get; set; }

    public MasterCommitmentType CommitmentType { get; set; }

    public override MasterCommitmentStatus Status { get; set; }

    public Decimal AvailableAmount { get; set; }

    public string RateSheet { get; set; }

    public DateTime MasterEffectiveDateTime { get; set; }

    public DateTime MasterExpirationDateTime { get; set; }

    public int ExternalOriginatorManagementID { get; set; }

    public string Notes { get; set; }

    public CorrespondentTradeInfo[] TradesInfo { get; set; }

    public CorrespondentMasterInfo()
      : base(true)
    {
    }

    public CorrespondentMasterCalculation Calculation
    {
      get
      {
        return this._calculation ?? (this._calculation = new CorrespondentMasterCalculation((ITradeInfoObject) this));
      }
    }
  }
}
