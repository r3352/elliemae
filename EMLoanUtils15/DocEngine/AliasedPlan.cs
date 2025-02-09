// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.AliasedPlan
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class AliasedPlan : Plan
  {
    private StandardPlan basePlan;
    private CustomPlan customPlan;

    public AliasedPlan(StandardPlan basePlan, CustomPlanCode customCode)
      : base(customCode.PlanCodeID, customCode.Description)
    {
      this.basePlan = basePlan;
      this.customPlan = new CustomPlan(customCode);
    }

    public override PlanType PlanType => PlanType.Alias;

    public override bool Active => this.customPlan.Active;

    public override FieldDefinition[] GetFieldDefinitions() => this.basePlan.GetFieldDefinitions();

    public override bool ContainsField(string encFieldId)
    {
      return this.basePlan.ContainsField(encFieldId);
    }

    public override string GetField(string encFieldId)
    {
      if (encFieldId.ToLower() == "plancode.plantype")
        return "Alias";
      return encFieldId.ToLower() == "plancode.invcd" ? (!(this.basePlan.GetField(encFieldId) == "0000") ? this.basePlan.GetField(encFieldId) : "") : (CustomPlan.IsCustomPlanField(encFieldId) ? this.customPlan.GetField(encFieldId) : this.basePlan.GetField(encFieldId));
    }

    public override bool HideInvestorName => this.customPlan.HideInvestorName;

    public override void Apply(IHtmlInput dataObj, DocumentOrderType orderType, bool clearMetadata)
    {
      base.Apply(dataObj, orderType, clearMetadata);
      if (orderType != DocumentOrderType.Opening)
      {
        if (this.customPlan.HideInvestorName)
          dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.ProgSpnsrNm", orderType), "");
        else
          dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.ProgSpnsrNm", orderType), this.customPlan.InvestorName);
      }
      else
      {
        if (this.customPlan.HideInvestorName)
          return;
        dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.ProgSpnsrNm", orderType), this.customPlan.InvestorName);
      }
    }

    public override void ApplyPlanMetadata(IHtmlInput dataObj, DocumentOrderType orderType)
    {
      base.ApplyPlanMetadata(dataObj, orderType);
      if (orderType != DocumentOrderType.Opening)
      {
        if (this.customPlan.HideInvestorName)
          dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.ProgSpnsrNm", orderType), "");
        else
          dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.ProgSpnsrNm", orderType), this.customPlan.InvestorName);
      }
      else
      {
        if (this.customPlan.HideInvestorName)
          return;
        dataObj.SetField(Plan.MapFieldIDForOrderType("PlanCode.ProgSpnsrNm", orderType), this.customPlan.InvestorName);
      }
    }
  }
}
