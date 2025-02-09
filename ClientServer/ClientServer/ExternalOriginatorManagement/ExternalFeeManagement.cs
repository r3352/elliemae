// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.ExternalFeeManagement
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class ExternalFeeManagement
  {
    public int FeeManagementID { get; set; }

    public int ExternalOrgID { get; set; }

    public string FeeName { get; set; }

    public string Description { get; set; }

    public string Code { get; set; }

    public ExternalOriginatorEntityType Channel { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Condition { get; set; }

    public string AdvancedCode { get; set; }

    public string AdvancedCodeXml { get; set; }

    public double FeePercent { get; set; }

    public double FeeAmount { get; set; }

    public int FeeBasedOn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime DateUpdated { get; set; }

    public ExternalOriginatorStatus Status { get; set; }

    public ExternalFeeManagement(
      int externalOrgID,
      string feeName,
      string code,
      ExternalOriginatorEntityType channel,
      DateTime startDate,
      DateTime endDate,
      int condition,
      string advancedCode,
      string advancedCodeXml,
      double feePercent,
      double feeAmount,
      int feeBasedOn,
      ExternalOriginatorStatus status)
    {
      this.ExternalOrgID = externalOrgID;
      this.FeeName = feeName;
      this.Code = code;
      this.Channel = channel;
      this.StartDate = startDate;
      this.EndDate = endDate;
      this.Condition = condition;
      this.AdvancedCode = advancedCode;
      this.AdvancedCodeXml = advancedCodeXml;
      this.FeePercent = feePercent;
      this.FeeAmount = feeAmount;
      this.FeeBasedOn = feeBasedOn;
      this.Status = status;
    }

    public ExternalFeeManagement()
    {
    }
  }
}
