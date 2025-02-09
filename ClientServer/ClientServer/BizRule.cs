// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BizRule
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public class BizRule
  {
    public static BizRuleType[] BizRuleTypes = new BizRuleType[6]
    {
      BizRuleType.FieldAccess,
      BizRuleType.FieldRules,
      BizRuleType.InputForms,
      BizRuleType.LoanAccess,
      BizRuleType.MilestoneRules,
      BizRuleType.PrintForms
    };
    public static BizRule.Condition[] Conditions = new BizRule.Condition[12]
    {
      BizRule.Condition.Null,
      BizRule.Condition.LoanPurpose,
      BizRule.Condition.LoanType,
      BizRule.Condition.LoanStatus,
      BizRule.Condition.CurrentLoanAssocMS,
      BizRule.Condition.RateLock,
      BizRule.Condition.PropertyState,
      BizRule.Condition.LoanDocType,
      BizRule.Condition.FinishedMilestone,
      BizRule.Condition.AdvancedCoding,
      BizRule.Condition.LoanProgram,
      BizRule.Condition.TPOActions
    };
    public static BizRule.Condition[] SpecializedConditions = new BizRule.Condition[11]
    {
      BizRule.Condition.LoanPurpose,
      BizRule.Condition.LoanType,
      BizRule.Condition.LoanStatus,
      BizRule.Condition.CurrentLoanAssocMS,
      BizRule.Condition.RateLock,
      BizRule.Condition.PropertyState,
      BizRule.Condition.LoanDocType,
      BizRule.Condition.FinishedMilestone,
      BizRule.Condition.AdvancedCoding,
      BizRule.Condition.LoanProgram,
      BizRule.Condition.TPOActions
    };
    public static BizRule.Condition[] GeneralConditions = new BizRule.Condition[1];
    public static BizRule.Condition[] EmptyConditions = new BizRule.Condition[0];
    public static string[] ConditionStrings = new string[14]
    {
      "Not Entered",
      "Loan Purpose",
      "Loan Type",
      "Loan Status",
      "Current Role",
      "Rate",
      "Property State",
      "Doc Type",
      "Finished Milestone",
      "Advanced Conditions",
      "Loan Program",
      "Property Type",
      "Occupancy",
      "TPO Actions"
    };
    public static string[] LoanPurposeStrings = new string[7]
    {
      "Not Entered",
      "Other",
      "Purchase",
      "Cash Out Refi",
      "No Cash Out Refi",
      "Construction",
      "Construction Perm"
    };
    public static string[] LoanTypeStrings = new string[7]
    {
      "Not Entered",
      "Other",
      "Conventional",
      "FHA",
      "VA",
      "USDA RHS",
      "HELOC"
    };
    public static string[] LoanStatusStrings = new string[3]
    {
      "Active Loan",
      "Originated Loan",
      "Adverse Loan"
    };
    public static string[] MoveRuleLoanStatusDbColNames = new string[9]
    {
      "lsActiveLoan",
      "lsLoanOriginated",
      "lsAppApprovedNotAccepted",
      "lsAppDenied",
      "lsAppWithdrawn",
      "lsFileClosed",
      "lsLoanPurchased",
      "lsPreapprovalReqDenied",
      "lsPreapprovalReqApprovedNotAccepted"
    };
    public static string[] LockDateStrings = new string[2]
    {
      "Unlocked",
      "Locked"
    };
    public static string[] LoanAccessRightStrings = new string[4]
    {
      "View Only",
      "Edit",
      "Custom",
      "Does Not Apply"
    };
    public static string[] LoanActionAccessRightString = new string[3]
    {
      "Hide",
      "Enable",
      "Disable"
    };
    public static string[] FieldAccessRightStrings = new string[4]
    {
      "Hide",
      "View Only",
      "Edit",
      "Does Not Apply"
    };
    public static string[] FieldRuleTypeStrings = new string[4]
    {
      "Range",
      "Dropdown List - Lock",
      "Dropdown List - Unlock",
      "Advanced Coding"
    };
    public static string[] RuleStatusStrings = new string[2]
    {
      "Inactive",
      "Active"
    };
    public static string[] ChannelStatusString = new string[5]
    {
      "No channel selected",
      "Banked - Retail",
      "Banked - Wholesale",
      "Brokered",
      "Correspondent"
    };
    public static string[] PropertyTypeStrings = new string[9]
    {
      "1 Unit",
      "2-4 Units",
      "Condominium",
      "PUD",
      "Cooperative",
      "Manufactured Housing Single Wide",
      "Manufactured Housing Multiwide",
      "Unknown",
      "Detached Condominium"
    };
    public static string[] PropertyOccupancyStrings = new string[3]
    {
      "Investor",
      "Second Home",
      "Primary Residence"
    };

    private BizRule()
    {
    }

    public static BizRuleType[] AllBizRuleTypes
    {
      get => (BizRuleType[]) Enum.GetValues(typeof (BizRuleType));
    }

    public static BizRule.Condition GetConditionEnum(string str)
    {
      for (int index = 0; index < BizRule.ConditionStrings.Length; ++index)
      {
        if (string.Compare(BizRule.ConditionStrings[index], str, true) == 0)
          return (BizRule.Condition) Enum.ToObject(typeof (BizRule.Condition), index);
      }
      return BizRule.Condition.Null;
    }

    public static string translateAccessRight(string val, bool toInternalValue)
    {
      if (toInternalValue)
      {
        switch (val)
        {
          case "View Only / Disabled":
            return "View Only";
          case "Edit / Enabled":
            return "Edit";
          default:
            return val;
        }
      }
      else
      {
        switch (val)
        {
          case "View Only":
            return "View Only / Disabled";
          case "Edit":
            return "Edit / Enabled";
          default:
            return val;
        }
      }
    }

    public static BizRule.LoanPurpose GetLoanPurposeEnum(string str)
    {
      switch (str)
      {
        case "Other":
          return BizRule.LoanPurpose.Other;
        case "Purchase":
          return BizRule.LoanPurpose.Purchase;
        case "Cash-Out Refinance":
          return BizRule.LoanPurpose.CashOutRefi;
        case "NoCash-Out Refinance":
          return BizRule.LoanPurpose.NoCashOutRefi;
        case "ConstructionOnly":
          return BizRule.LoanPurpose.Construction;
        case "ConstructionToPermanent":
          return BizRule.LoanPurpose.ConstructionPerm;
        default:
          return BizRule.LoanPurpose.Null;
      }
    }

    public static BizRule.LoanType GetLoanTypeEnum(string str)
    {
      switch (str)
      {
        case "Conventional":
          return BizRule.LoanType.Conv;
        case "FarmersHomeAdministration":
          return BizRule.LoanType.USDA_RHS;
        case "FHA":
          return BizRule.LoanType.FHA;
        case "Other":
          return BizRule.LoanType.Other;
        case "VA":
          return BizRule.LoanType.VA;
        case "HELOC":
          return BizRule.LoanType.HELOC;
        default:
          return BizRule.LoanType.Null;
      }
    }

    public static BizRule.RateLock GetRateLockEnum(string str)
    {
      return string.Compare(str, "Y", true) == 0 ? BizRule.RateLock.Locked : BizRule.RateLock.Unlocked;
    }

    public static BizRule.LoanStatus GetLoanStatusEnum(string str)
    {
      switch (str)
      {
        case "Application approved but not accepted":
          return BizRule.LoanStatus.AdverseLoan;
        case "Application denied":
          return BizRule.LoanStatus.AdverseLoan;
        case "Application withdrawn":
          return BizRule.LoanStatus.AdverseLoan;
        case "File Closed for incompleteness":
          return BizRule.LoanStatus.AdverseLoan;
        case "Loan Originated":
          return BizRule.LoanStatus.OriginatedLoan;
        case "Loan purchased by your institution":
          return BizRule.LoanStatus.OriginatedLoan;
        case "Preapproval request approved but not accepted":
          return BizRule.LoanStatus.AdverseLoan;
        case "Preapproval request denied by financial institution":
          return BizRule.LoanStatus.AdverseLoan;
        default:
          return BizRule.LoanStatus.ActiveLoan;
      }
    }

    public static BizRule.LoanStatus GetLoanStatusEnumForPlinth(string str)
    {
      switch (str)
      {
        case "1":
        case "6":
          return BizRule.LoanStatus.OriginatedLoan;
        case "2":
        case "3":
        case "4":
        case "5":
        case "7":
        case "8":
          return BizRule.LoanStatus.AdverseLoan;
        default:
          return BizRule.LoanStatus.ActiveLoan;
      }
    }

    public static BizRule.PropertyType GetPropertyTypeEnum(string str)
    {
      switch (str)
      {
        case "1 Unit":
          return BizRule.PropertyType.Unit_1;
        case "2-4 Units":
          return BizRule.PropertyType.Units_24;
        case "Condominium":
          return BizRule.PropertyType.Condominium;
        case "Cooperative":
          return BizRule.PropertyType.Cooperative;
        case "Detached Condominium":
          return BizRule.PropertyType.DetachedCondominium;
        case "Manufactured Housing Multiwide":
          return BizRule.PropertyType.ManufacturedHousingMultiwide;
        case "Manufactured Housing Single Wide":
          return BizRule.PropertyType.ManufacturedHousingSingleWide;
        case "PUD":
          return BizRule.PropertyType.PUD;
        default:
          return BizRule.PropertyType.Unknow;
      }
    }

    public static BizRule.PropertyOccupancy GetPropertyOccupancyTypeEnum(string str)
    {
      switch (str)
      {
        case "Investor":
          return BizRule.PropertyOccupancy.Investor;
        case "PrimaryResidence":
          return BizRule.PropertyOccupancy.PrimaryResidence;
        case "SecondHome":
          return BizRule.PropertyOccupancy.SecondHome;
        default:
          return BizRule.PropertyOccupancy.Unknown;
      }
    }

    public static string ConditionStateAsString(
      BizRule.Condition condition,
      int conditionState,
      string milestoneID)
    {
      switch (condition)
      {
        case BizRule.Condition.Null:
          return "";
        case BizRule.Condition.CurrentLoanAssocMS:
        case BizRule.Condition.FinishedMilestone:
        case BizRule.Condition.LoanProgram:
          return milestoneID;
        default:
          return string.Concat((object) conditionState);
      }
    }

    public static string GetChannelValue(string loanValue)
    {
      switch (loanValue.ToLower())
      {
        case "banked - retail":
          return "1";
        case "banked - wholesale":
          return "2";
        case "brokered":
          return "3";
        case "correspondent":
          return "4";
        default:
          return "0";
      }
    }

    public static string Type2Name(BizRuleType ruleType)
    {
      switch (ruleType)
      {
        case BizRuleType.None:
          return "Unknown";
        case BizRuleType.MilestoneRules:
          return "Milestone Completion";
        case BizRuleType.LoanAccess:
          return "Persona Access to Loans";
        case BizRuleType.FieldAccess:
          return "Persona Access to Fields";
        case BizRuleType.FieldRules:
          return "Field Data Entry";
        case BizRuleType.InputForms:
          return "Input Form List";
        case BizRuleType.Triggers:
          return "Field Triggers";
        case BizRuleType.PrintForms:
          return "Loan Form Printing";
        case BizRuleType.PrintSelection:
          return "Print Auto Selection";
        case BizRuleType.LoanFolderRules:
          return "Loan Folder Business Rule";
        case BizRuleType.EmailTriggers:
          return "Email Triggers";
        case BizRuleType.AutomatedConditions:
          return "Automated Conditions";
        case BizRuleType.MilestoneTemplateConditions:
          return "Milestone Templates";
        case BizRuleType.LoanActionAccess:
          return "Persona Access to Loan Actions";
        case BizRuleType.LoanActionCompletionRules:
          return "Loan Action Completion";
        case BizRuleType.AutoLockExclusionRules:
          return "Auto Lock Exclusion";
        case BizRuleType.DDMFeeScenarios:
          return "DDM Fee Rule Scenario";
        case BizRuleType.DDMDataPopTiming:
          return "DDM Data Population Timing";
        case BizRuleType.DDMFieldScenarios:
          return "DDM Field Rule Scenario";
        case BizRuleType.DDMDataTables:
          return "DDM Data Table";
        case BizRuleType.ServiceWorkflowRules:
          return "Service Workflow";
        case BizRuleType.AutomatedEnhancedConditions:
          return "Automated Enhanced Conditions";
        default:
          return "Unknown Business Rule Type";
      }
    }

    public enum Condition
    {
      [Description("No Condition")] Null,
      [Description("Loan Purpose")] LoanPurpose,
      [Description("Loan Type")] LoanType,
      [Description("Loan State")] LoanStatus,
      [Description("Current Role")] CurrentLoanAssocMS,
      [Description("Rate")] RateLock,
      [Description("Property State")] PropertyState,
      [Description("Doc Type")] LoanDocType,
      [Description("Finished Milestone")] FinishedMilestone,
      [Description("Advanced Conditions")] AdvancedCoding,
      [Description("Loan Program")] LoanProgram,
      [Description("Property Type")] PropertyType,
      [Description("Occupancy")] Occupancy,
      [Description("TPO Actions")] TPOActions,
      [Description("Alerts")] Alerts,
    }

    public enum LoanPurpose
    {
      Null,
      Other,
      Purchase,
      CashOutRefi,
      NoCashOutRefi,
      Construction,
      ConstructionPerm,
    }

    public enum LoanType
    {
      Null,
      Other,
      Conv,
      FHA,
      VA,
      USDA_RHS,
      HELOC,
    }

    public enum LoanStatus
    {
      ActiveLoan,
      OriginatedLoan,
      AdverseLoan,
    }

    public enum RateLock
    {
      Unlocked,
      Locked,
    }

    [Flags]
    public enum LoanAccessRight
    {
      ViewAllOnly = 1,
      DocTracking = 2,
      ConversationLog = 4,
      Task = 8,
      ProfitMgmt = 16, // 0x00000010
      ConditionTracking = 32, // 0x00000020
      DocTrackingViewOnly = 64, // 0x00000040
      ConditionTrackingViewOnly = 128, // 0x00000080
      ConversationLogViewOnly = 256, // 0x00000100
      TaskViewOnly = 512, // 0x00000200
      ProfitMgmtViewOnly = 1024, // 0x00000400
      LockRequest = 2048, // 0x00000800
      LockRequestViewOnly = 4096, // 0x00001000
      FormFields = 8192, // 0x00002000
      DoesNotApply = 16384, // 0x00004000
      DisclosureTracking = 32768, // 0x00008000
      DisclosureTrackingViewOnly = 65536, // 0x00010000
      DocTrackingRequestRetrieveService = 131072, // 0x00020000
      DocTrackingRetrieveServiceCurrent = 262144, // 0x00040000
      DocTrackingRetrieveServiceNotCurrent = 524288, // 0x00080000
      DocTrackingRetrieveServiceUnassigned = 1048576, // 0x00100000
      DocTrackingRequestRetrieveBorrower = 2097152, // 0x00200000
      DocTrackingRetrieveBorrowerCurrent = 4194304, // 0x00400000
      DocTrackingRetrieveBorrowerNotCurrent = 8388608, // 0x00800000
      DocTrackingRetrieveBorrowerUnassigned = 16777216, // 0x01000000
      DocTrackingUnassignedFiles = 33554432, // 0x02000000
      DocTrackingUnprotectedDocs = 67108864, // 0x04000000
      DocTrackingProtectedDocs = 134217728, // 0x08000000
      DocTrackingCreateDocs = 268435456, // 0x10000000
      DocTrackingOrderDisclosures = 536870912, // 0x20000000
      DocTrackingPartial = 1073741824, // 0x40000000
      EditAll = DocTrackingRetrieveBorrowerNotCurrent | DocTrackingRetrieveBorrowerCurrent | DocTrackingRequestRetrieveBorrower | DocTrackingRetrieveServiceUnassigned | DocTrackingRetrieveServiceNotCurrent | DocTrackingRetrieveServiceCurrent | DocTrackingRequestRetrieveService | DisclosureTrackingViewOnly | DisclosureTracking | DoesNotApply | FormFields | LockRequestViewOnly | LockRequest | ProfitMgmtViewOnly | TaskViewOnly | ConversationLogViewOnly | ConditionTrackingViewOnly | DocTrackingViewOnly | ConditionTracking | ProfitMgmt | Task | ConversationLog | DocTracking | ViewAllOnly, // 0x00FFFFFF
    }

    public enum LoanActionAccessRight
    {
      Hide,
      Enable,
      Disable,
    }

    public enum FieldAccessRight
    {
      Hide = 0,
      ViewOnly = 1,
      Edit = 2,
      DoesNotApply = 4,
    }

    public enum FieldRuleType
    {
      Range,
      ListLock,
      ListUnlock,
      Code,
    }

    public enum RuleStatus
    {
      Inactive,
      Active,
    }

    public enum LoanFolderMoveRuleOption
    {
      None,
      Milestone,
      LoanStatus,
    }

    public enum ActivationReturnCode
    {
      InconsistentFieldRules = -4, // 0xFFFFFFFC
      ExceptionThrown = -3, // 0xFFFFFFFD
      ConditionConflict = -2, // 0xFFFFFFFE
      RuleIDNotFound = -1, // 0xFFFFFFFF
      Succeed = 0,
      NoOp = 1,
    }

    public enum PropertyType
    {
      Unit_1,
      Units_24,
      Condominium,
      PUD,
      Cooperative,
      ManufacturedHousingSingleWide,
      ManufacturedHousingMultiwide,
      Unknow,
      DetachedCondominium,
    }

    public enum PropertyOccupancy
    {
      Unknown = -1, // 0xFFFFFFFF
      Investor = 0,
      SecondHome = 1,
      PrimaryResidence = 2,
    }
  }
}
