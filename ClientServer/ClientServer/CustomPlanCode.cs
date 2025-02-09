// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.CustomPlanCode
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class CustomPlanCode : IXmlSerializable
  {
    private string planCode;
    private string description;
    private DocumentOrderType orderType = DocumentOrderType.Closing;
    private bool isEMAlias;
    private string planCodeID;
    private string investor;
    private bool importInvestorToLoan;
    private bool isActive;

    public CustomPlanCode()
    {
    }

    public CustomPlanCode(
      string planCode,
      string description,
      DocumentOrderType orderType,
      bool isEMAlias,
      string planCodeID,
      string investor,
      bool importInvestorToLoan,
      bool isActive)
    {
      this.planCode = planCode;
      this.description = description;
      this.orderType = orderType;
      this.isEMAlias = isEMAlias;
      this.planCodeID = planCodeID;
      this.investor = investor;
      this.importInvestorToLoan = importInvestorToLoan;
      this.isActive = isActive;
    }

    public CustomPlanCode(XmlSerializationInfo info)
    {
      this.planCode = info.GetString(nameof (PlanCode));
      this.description = info.GetString(nameof (Description));
      this.orderType = (DocumentOrderType) info.GetValue(nameof (OrderType), typeof (DocumentOrderType));
      this.isEMAlias = info.GetBoolean(nameof (IsEMAlias));
      this.planCodeID = info.GetString(nameof (PlanCodeID));
      this.investor = info.GetString(nameof (Investor));
      this.importInvestorToLoan = info.GetBoolean(nameof (ImportInvestorToLoan));
      this.isActive = info.GetBoolean(nameof (IsActive));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("PlanCode", (object) this.planCode);
      info.AddValue("Description", (object) this.description);
      info.AddValue("OrderType", (object) this.orderType);
      info.AddValue("IsEMAlias", (object) this.isEMAlias);
      info.AddValue("PlanCodeID", (object) this.planCodeID);
      info.AddValue("Investor", (object) this.investor);
      info.AddValue("ImportInvestorToLoan", (object) this.importInvestorToLoan);
      info.AddValue("IsActive", (object) this.isActive);
    }

    public string PlanCode
    {
      get => this.planCode;
      set => this.planCode = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public DocumentOrderType OrderType
    {
      get => this.orderType;
      set => this.orderType = value;
    }

    public bool IsEMAlias
    {
      get => this.isEMAlias;
      set => this.isEMAlias = value;
    }

    public string PlanCodeID
    {
      get => this.planCodeID;
      set => this.planCodeID = value;
    }

    public string Investor
    {
      get => this.investor;
      set => this.investor = value;
    }

    public bool ImportInvestorToLoan
    {
      get => this.importInvestorToLoan;
      set => this.importInvestorToLoan = value;
    }

    public bool IsActive
    {
      get => this.isActive;
      set => this.isActive = value;
    }

    public static bool IsCustomPlanCode(string planCode)
    {
      return planCode.StartsWith("C.", StringComparison.CurrentCultureIgnoreCase);
    }
  }
}
