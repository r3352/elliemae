// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PlanCodeInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PlanCodeInfo : IXmlSerializable
  {
    private string planCode;
    private string emPlanID;
    private DocumentOrderType orderType;
    private bool isCustom;

    public PlanCodeInfo(
      string planCode,
      string emPlanID,
      DocumentOrderType orderType,
      bool isCustom)
    {
      this.planCode = planCode;
      this.emPlanID = emPlanID;
      this.orderType = orderType;
      this.isCustom = isCustom;
    }

    public string PlanCode => this.planCode;

    public string PlanID => this.emPlanID;

    public bool IsCustom => this.isCustom;

    public DocumentOrderType OrderType => this.orderType;

    public override string ToString()
    {
      return this.planCode == "" ? this.emPlanID : this.planCode + "/" + this.emPlanID;
    }

    public override bool Equals(object obj)
    {
      return obj is PlanCodeInfo planCodeInfo && string.Compare(this.planCode, planCodeInfo.planCode, true) == 0 && string.Compare(this.emPlanID, planCodeInfo.emPlanID, true) == 0;
    }

    public override int GetHashCode()
    {
      return StringComparer.CurrentCultureIgnoreCase.GetHashCode(this.planCode + this.emPlanID);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("PlanCode", (object) this.planCode);
      info.AddValue("PlanID", (object) this.emPlanID);
      info.AddValue("OrderType", (object) this.orderType);
      info.AddValue("IsCustom", (object) this.isCustom);
    }

    public static PlanCodeInfo FromDataObject(IHtmlInput dataObj, DocumentOrderType orderType)
    {
      string field1;
      string field2;
      if (orderType == DocumentOrderType.Opening)
      {
        field1 = dataObj.GetField("Opening.PlanID");
        field2 = dataObj.GetField("DISCLOSUREPLANCODE");
      }
      else
      {
        field1 = dataObj.GetField("PlanCode.ID");
        field2 = dataObj.GetField("1881");
      }
      if (string.IsNullOrEmpty(field2) && string.IsNullOrEmpty(field1))
        return (PlanCodeInfo) null;
      return string.IsNullOrEmpty(field2) ? new PlanCodeInfo(field2, field1, orderType, false) : new PlanCodeInfo(field2, field1, orderType, CustomPlanCode.IsCustomPlanCode(field2));
    }
  }
}
