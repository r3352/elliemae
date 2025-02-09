// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.FieldSearchUtil
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  public static class FieldSearchUtil
  {
    public static List<SearchableRuleType> AllSearchableRuleTypes
    {
      get
      {
        FieldSearchRuleType[] values = (FieldSearchRuleType[]) Enum.GetValues(typeof (FieldSearchRuleType));
        List<SearchableRuleType> searchableRuleTypes = new List<SearchableRuleType>();
        foreach (FieldSearchRuleType fieldSearchRuleType in values)
          searchableRuleTypes.Add(new SearchableRuleType(FieldSearchUtil.Type2Name(fieldSearchRuleType), fieldSearchRuleType));
        return searchableRuleTypes;
      }
    }

    public static string Type2Name(FieldSearchRuleType ruleType)
    {
      switch (ruleType)
      {
        case FieldSearchRuleType.None:
          return "Unknown";
        case FieldSearchRuleType.MilestoneRules:
          return "Milestone Completion";
        case FieldSearchRuleType.LoanAccess:
          return "Persona Access to Loans";
        case FieldSearchRuleType.FieldAccess:
          return "Persona Access to Fields";
        case FieldSearchRuleType.FieldRules:
          return "Field Data Entry";
        case FieldSearchRuleType.InputForms:
          return "Input Form List";
        case FieldSearchRuleType.Triggers:
          return "Field Triggers";
        case FieldSearchRuleType.PrintForms:
          return "Loan Form Printing";
        case FieldSearchRuleType.PrintSelection:
          return "Print Auto Selection";
        case FieldSearchRuleType.AutomatedConditions:
          return "Automated Conditions";
        case FieldSearchRuleType.PiggyBackingFields:
          return "Piggyback Loan Synchronization";
        case FieldSearchRuleType.LoanCustomFields:
          return "Loan Custom Fields";
        case FieldSearchRuleType.BorrowerCustomFields:
          return "Borrower Custom Fields";
        case FieldSearchRuleType.BusinessCustomFields:
          return "Business Custom Fields";
        case FieldSearchRuleType.Alerts:
          return "Alerts";
        case FieldSearchRuleType.LockRequestAdditionalFields:
          return "Lock Request Additional Fields";
        case FieldSearchRuleType.HtmlEmailTemplate:
          return "HTML Email Templates";
        case FieldSearchRuleType.CompanyStatusOnline:
          return "Company Status Online";
        case FieldSearchRuleType.TPOCustomFields:
          return "TPO Custom Fields";
        case FieldSearchRuleType.LoanActionAccess:
          return "Persona Access to Loan Actions";
        case FieldSearchRuleType.LoanActionCompletionRules:
          return "Loan Action Completion";
        case FieldSearchRuleType.DDMFeeScenarios:
          return "DDM Fee Rule Scenario";
        case FieldSearchRuleType.DDMFieldScenarios:
          return "DDM Field Rule Scenario";
        case FieldSearchRuleType.DDMDataTables:
          return "DDM Data Table";
        default:
          return "Custom";
      }
    }

    public static FieldSearchRuleType Name2Type(string name)
    {
      if (string.Compare(name, "Milestone Completion", true) == 0)
        return FieldSearchRuleType.MilestoneRules;
      if (string.Compare(name, "Persona Access to Loans", true) == 0)
        return FieldSearchRuleType.LoanAccess;
      if (string.Compare(name, "Persona Access to Fields", true) == 0)
        return FieldSearchRuleType.FieldAccess;
      if (string.Compare(name, "Field Data Entry", true) == 0)
        return FieldSearchRuleType.FieldRules;
      if (string.Compare(name, "Input Form List", true) == 0)
        return FieldSearchRuleType.InputForms;
      if (string.Compare(name, "Field Triggers", true) == 0)
        return FieldSearchRuleType.Triggers;
      if (string.Compare(name, "Loan Form Printing", true) == 0)
        return FieldSearchRuleType.PrintForms;
      if (string.Compare(name, "Print Auto Selection", true) == 0)
        return FieldSearchRuleType.PrintSelection;
      if (string.Compare(name, "Automated Conditions", true) == 0)
        return FieldSearchRuleType.AutomatedConditions;
      if (string.Compare(name, "Piggyback Loan Synchronization", true) == 0)
        return FieldSearchRuleType.PiggyBackingFields;
      if (string.Compare(name, "Loan Custom Fields", true) == 0)
        return FieldSearchRuleType.LoanCustomFields;
      if (string.Compare(name, "Borrower Custom Fields", true) == 0)
        return FieldSearchRuleType.BorrowerCustomFields;
      if (string.Compare(name, "Business Custom Fields", true) == 0)
        return FieldSearchRuleType.BusinessCustomFields;
      if (string.Compare(name, "Alerts", true) == 0)
        return FieldSearchRuleType.Alerts;
      if (string.Compare(name, "Lock Request Additional Fields") == 0)
        return FieldSearchRuleType.LockRequestAdditionalFields;
      if (string.Compare(name, "HTML Email Templates") == 0)
        return FieldSearchRuleType.HtmlEmailTemplate;
      if (string.Compare(name, "Company Status Online") == 0)
        return FieldSearchRuleType.CompanyStatusOnline;
      if (string.Compare(name, "TPO Custom Fields") == 0)
        return FieldSearchRuleType.TPOCustomFields;
      if (string.Compare(name, "Persona Access to Loan Actions", true) == 0)
        return FieldSearchRuleType.LoanActionAccess;
      if (string.Compare(name, "Loan Action Completion") == 0)
        return FieldSearchRuleType.LoanActionCompletionRules;
      if (string.Compare(name, "DDM Fee Rule Scenario") == 0)
        return FieldSearchRuleType.DDMFeeScenarios;
      if (string.Compare(name, "DDM Field Rule Scenario") == 0)
        return FieldSearchRuleType.DDMFieldScenarios;
      return string.Compare(name, "DDM Data Table") == 0 ? FieldSearchRuleType.DDMDataTables : FieldSearchRuleType.None;
    }

    public static string GetValidRuleTypes()
    {
      return FieldSearchUtil.Type2Name(FieldSearchRuleType.FieldRules) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.LoanAccess) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.FieldAccess) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.MilestoneRules) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.InputForms) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.Triggers) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.PrintForms) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.PrintSelection) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.AutomatedConditions) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.PiggyBackingFields) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.LoanCustomFields) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.BorrowerCustomFields) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.BusinessCustomFields) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.Alerts) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.LockRequestAdditionalFields) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.HtmlEmailTemplate) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.CompanyStatusOnline) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.TPOCustomFields) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.LoanActionAccess) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.LoanActionCompletionRules) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.DDMFeeScenarios) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.DDMFieldScenarios) + "," + FieldSearchUtil.Type2Name(FieldSearchRuleType.DDMDataTables);
    }
  }
}
