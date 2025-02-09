// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.CustomPlan
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class CustomPlan : Plan
  {
    private CustomPlanCode customCode;
    public static readonly string[] CustomPlanFields = new string[4]
    {
      "1881",
      "PlanCode.Desc",
      "PlanCode.LoanProgTyp",
      "PlanCode.ProgSpnsrNm"
    };

    public CustomPlan(CustomPlanCode customCode)
      : base(customCode.PlanCodeID, customCode.Description)
    {
      this.customCode = customCode;
    }

    public override PlanType PlanType => PlanType.Custom;

    public override FieldDefinition[] GetFieldDefinitions()
    {
      List<FieldDefinition> fieldDefinitionList = new List<FieldDefinition>();
      foreach (string codeMetadataField in Plan.ClosingPlanCodeMetadataFields)
        fieldDefinitionList.Add((FieldDefinition) StandardFields.GetField(codeMetadataField));
      return fieldDefinitionList.ToArray();
    }

    public override DocumentOrderType OrderType => this.customCode.OrderType;

    public override bool Active => this.customCode.IsActive;

    public override bool HideInvestorName => !this.customCode.ImportInvestorToLoan;

    public override string GetField(string encFieldId)
    {
      switch (encFieldId.ToLower())
      {
        case "1881":
          return this.customCode.PlanCode;
        case "plancode.loanprogtyp":
          return CustomPlan.GetLoanProgTypForDocumentOrderType(this.customCode.OrderType);
        case "plancode.progspnsrnm":
          return this.customCode.Investor;
        case "plancode.desc":
          return this.customCode.Description;
        case "plancode.plantype":
          return "Custom";
        default:
          return "";
      }
    }

    public override bool ContainsField(string encFieldId) => this.IsPlanMetadataField(encFieldId);

    public static bool IsCustomPlanField(string fieldId)
    {
      return Array.Find<string>(CustomPlan.CustomPlanFields, (Predicate<string>) new StringPredicate(fieldId, true)) != null;
    }

    internal static string GetLoanProgTypForDocumentOrderType(DocumentOrderType orderType)
    {
      switch (orderType)
      {
        case DocumentOrderType.Opening:
          return "Opening";
        case DocumentOrderType.Closing:
          return "Closing";
        case DocumentOrderType.Both:
          return "Both";
        default:
          return "";
      }
    }
  }
}
