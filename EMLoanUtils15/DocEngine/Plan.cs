// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.Plan
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public abstract class Plan : IHtmlInput
  {
    private const string className = "Plan�";
    private static readonly string sw = Tracing.SwDataEngine;
    protected static string[] OpeningPlanCodeMetadataFields = new string[7]
    {
      "Opening.PlanID",
      "DISCLOSUREPLANCODE",
      "Opening.LoanProgTyp",
      "Opening.ProgCd",
      "VEND.X263",
      "Opening.PlanDesc",
      "Opening.InvCd"
    };
    protected static string[] ClosingPlanCodeMetadataFields = new string[7]
    {
      "PlanCode.ID",
      "1881",
      "PlanCode.LoanProgTyp",
      "PlanCode.ProgCd",
      "PlanCode.ProgSpnsrNm",
      "PlanCode.Desc",
      "PlanCode.InvCd"
    };
    private string planID;
    private string description;

    internal Plan(string planID, string description)
    {
      this.planID = planID;
      this.description = description;
    }

    public string PlanID => this.planID;

    public string Description => this.description;

    public abstract PlanType PlanType { get; }

    public string InvestorName => this.GetField("PlanCode.ProgSpnsrNm");

    public string InvestorCode => this.GetField("PlanCode.InvCd");

    public string InvestorPlanCode => this.GetField("1881");

    public virtual bool HideInvestorName => false;

    public virtual bool Active => true;

    public virtual DocumentOrderType OrderType
    {
      get
      {
        switch (this.GetField("PlanCode.LoanProgTyp"))
        {
          case "Opening":
            return DocumentOrderType.Opening;
          case "Closing":
            return DocumentOrderType.Closing;
          default:
            return DocumentOrderType.Both;
        }
      }
    }

    public PlanCodeInfo ToPlanCodeInfo()
    {
      return new PlanCodeInfo(this.InvestorPlanCode, this.PlanID, this.OrderType, this.PlanType != PlanType.Standard);
    }

    public abstract FieldDefinition[] GetFieldDefinitions();

    public abstract bool ContainsField(string encFieldId);

    public abstract string GetField(string encFieldId);

    public virtual void SetField(string id, string val) => throw new NotSupportedException();

    internal static string MapFieldIDForOrderType(string fieldId, DocumentOrderType orderType)
    {
      if (orderType == DocumentOrderType.Opening)
      {
        for (int index = 0; index < Plan.ClosingPlanCodeMetadataFields.Length; ++index)
        {
          if (string.Compare(fieldId, Plan.ClosingPlanCodeMetadataFields[index], true) == 0)
            return Plan.OpeningPlanCodeMetadataFields[index];
        }
      }
      return fieldId;
    }

    public virtual bool IsDirty(string id) => false;

    public virtual void ClearDirtyTable()
    {
    }

    public virtual void CleanField(string id)
    {
    }

    public virtual Investor GetInvestor()
    {
      return new Investor(this.GetField("PlanCode.InvCd"), this.GetField("PlanCode.ProgSpnsrNm"));
    }

    public void Apply(LoanProgram template)
    {
      this.Apply((IHtmlInput) template, DocumentOrderType.Closing, true);
    }

    public void Apply(IHtmlInput dataObj, DocumentOrderType orderType)
    {
      this.Apply(dataObj, orderType, true);
    }

    public virtual void Apply(IHtmlInput dataObj, DocumentOrderType orderType, bool clearMetadata)
    {
      Investor investor = this.GetInvestor();
      if (clearMetadata)
        Plan.ClearPlanMetadata(dataObj, orderType, !investor.IsGeneric && !this.HideInvestorName);
      foreach (FieldDefinition fieldDefinition in this.GetFieldDefinitions())
      {
        if (!investor.IsGeneric && !this.HideInvestorName || !Investor.IsInvestorField(fieldDefinition.FieldID))
        {
          if (this.ContainsField(fieldDefinition.FieldID))
          {
            try
            {
              string id = Plan.MapFieldIDForOrderType(fieldDefinition.FieldID, orderType);
              dataObj.SetField(id, this.GetField(fieldDefinition.FieldID));
            }
            catch (Exception ex)
            {
              Tracing.Log(Plan.sw, nameof (Plan), TraceLevel.Warning, "Failed to set value for plan code field '" + fieldDefinition.FieldID + "': " + ex.Message);
            }
          }
        }
      }
      if (!(dataObj is LoanData))
        return;
      EncompassDocs.SetDocEngine(dataObj, "New_Encompass_Docs_Solution");
    }

    public virtual FieldConflict[] CompareTo(IHtmlInput dataObj)
    {
      this.GetInvestor();
      List<FieldConflict> fieldConflictList = new List<FieldConflict>();
      foreach (FieldDefinition fieldDefinition in this.GetFieldDefinitions())
      {
        if (!this.IsPlanMetadataField(fieldDefinition.FieldID))
        {
          if (this.ContainsField(fieldDefinition.FieldID))
          {
            try
            {
              string str = dataObj.GetField(fieldDefinition.FieldID);
              string field = this.GetField(fieldDefinition.FieldID);
              if (fieldDefinition.FieldID == "423" && str != "Biweekly")
                str = "";
              if (!fieldDefinition.CompareValues(field, str))
                fieldConflictList.Add(new FieldConflict(fieldDefinition.FieldID, (object) field, (object) str));
            }
            catch (Exception ex)
            {
              Tracing.Log(Plan.sw, nameof (Plan), TraceLevel.Warning, "Failed to compare value for plan code field '" + fieldDefinition.FieldID + "': " + ex.Message);
            }
          }
        }
      }
      return fieldConflictList.ToArray();
    }

    public bool IsPlanMetadataField(string fieldId)
    {
      return Array.Find<string>(Plan.OpeningPlanCodeMetadataFields, (Predicate<string>) new StringPredicate(fieldId, true)) != null || Array.Find<string>(Plan.ClosingPlanCodeMetadataFields, (Predicate<string>) new StringPredicate(fieldId, true)) != null;
    }

    public virtual void ApplyPlanMetadata(IHtmlInput dataObj, DocumentOrderType orderType)
    {
      Investor investor = this.GetInvestor();
      foreach (string codeMetadataField in Plan.ClosingPlanCodeMetadataFields)
      {
        if (!investor.IsGeneric && !this.HideInvestorName || !Investor.IsInvestorField(codeMetadataField))
        {
          if (this.ContainsField(codeMetadataField))
            dataObj.SetField(Plan.MapFieldIDForOrderType(codeMetadataField, orderType), this.GetField(codeMetadataField));
          else
            dataObj.SetField(Plan.MapFieldIDForOrderType(codeMetadataField, orderType), "");
        }
      }
    }

    public override bool Equals(object obj)
    {
      return obj is Plan plan && string.Compare(plan.PlanID, this.PlanID, true) == 0;
    }

    public override int GetHashCode()
    {
      return StringComparer.CurrentCultureIgnoreCase.GetHashCode(this.PlanID);
    }

    public static void ClearPlanMetadata(
      IHtmlInput dataObj,
      DocumentOrderType orderType,
      bool clearInvestorFields = true)
    {
      foreach (string codeMetadataField in Plan.ClosingPlanCodeMetadataFields)
      {
        if (clearInvestorFields || !Investor.IsInvestorField(codeMetadataField))
          dataObj.SetField(Plan.MapFieldIDForOrderType(codeMetadataField, orderType), "");
      }
    }

    public static Plan Synchronize(SessionObjects sessionObjects, LoanProgram loanProgram)
    {
      string field1 = loanProgram.GetField("PlanCode.ID");
      string field2 = loanProgram.GetField("1881");
      if (string.IsNullOrEmpty(field2) && string.IsNullOrEmpty(field1))
        return (Plan) null;
      if (loanProgram.DocumentOrderType == DocumentOrderType.None)
        return (Plan) null;
      PlanCodeInfo planInfo = new PlanCodeInfo(field2, field1, loanProgram.DocumentOrderType, CustomPlanCode.IsCustomPlanCode(field2));
      Plan plan = Plans.GetPlan(sessionObjects, planInfo);
      if (plan == null)
        throw new InvalidPlanCodeException(field2 == "" ? field1 : field2);
      plan.Apply((IHtmlInput) loanProgram, loanProgram.DocumentOrderType);
      return plan;
    }

    private static DocumentOrderType parseOrderType(string orderType)
    {
      switch (orderType)
      {
        case "Opening":
          return DocumentOrderType.Opening;
        case "Closing":
          return DocumentOrderType.Closing;
        case "Both":
          return DocumentOrderType.Both;
        default:
          return DocumentOrderType.None;
      }
    }

    FieldFormat IHtmlInput.GetFormat(string id)
    {
      return ((IHtmlInput) this).GetFieldDefinition(id).Format;
    }

    FieldDefinition IHtmlInput.GetFieldDefinition(string id)
    {
      return (FieldDefinition) StandardFields.GetField(id);
    }

    string IHtmlInput.GetSimpleField(string id) => this.GetField(id);

    string IHtmlInput.GetOrgField(string id) => this.GetField(id);

    bool IHtmlInput.IsLocked(string id) => false;

    void IHtmlInput.RemoveLock(string id)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    void IHtmlInput.AddLock(string id)
    {
      throw new Exception("The method or operation is not implemented.");
    }

    void IHtmlInput.SetCurrentField(string id, string val) => this.SetField(id, val);

    public void SetField(string id, string val, bool isUserModified)
    {
      throw new NotImplementedException();
    }
  }
}
