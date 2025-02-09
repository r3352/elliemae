// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EnhancedConditionSingleAttributeField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class EnhancedConditionSingleAttributeField : VirtualField
  {
    private const string FieldPrefix = "Enhanced";
    private EnhancedConditionSingleProperty property;
    private bool allowInReportingDb = true;

    public EnhancedConditionSingleProperty Property => this.property;

    public EnhancedConditionSingleAttributeField(
      EnhancedConditionSingleProperty property,
      string description,
      FieldFormat format)
      : this(property, description, format, true)
    {
    }

    public EnhancedConditionSingleAttributeField(
      EnhancedConditionSingleProperty property,
      string description,
      FieldFormat format,
      bool allowInReportingDb)
      : base("Enhanced." + property.ToString(), description, format, FieldInstanceSpecifierType.EnhancedConditionSingleAttribute)
    {
      this.property = property;
      this.allowInReportingDb = allowInReportingDb;
    }

    public EnhancedConditionSingleAttributeField(
      EnhancedConditionSingleAttributeField parent,
      string documentTitle)
      : base((VirtualField) parent, (object) documentTitle)
    {
      this.property = parent.property;
      this.allowInReportingDb = parent.AllowInReportingDatabase;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.EnhancedConditionField;

    public override bool AllowInReportingDatabase => this.allowInReportingDb;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new EnhancedConditionSingleAttributeField(this, string.Concat(instanceSpecifier));
    }

    protected override string Evaluate(LoanData loan)
    {
      string[] strArray = this.InstanceSpecifier.ToString().Split('.');
      string conditionType = "";
      string conditionName = "";
      string trackingValue = "";
      if (strArray.Length > 1)
      {
        conditionType = strArray[0];
        conditionName = strArray[1];
        if (strArray.Length > 2)
          trackingValue = strArray[2];
      }
      EnhancedConditionLog enhancedConditionLog = this.getEnhancedConditionLog(loan, conditionName, conditionType);
      if (enhancedConditionLog == null)
        return "";
      switch (this.property)
      {
        case EnhancedConditionSingleProperty.NAME:
          return enhancedConditionLog.Title.Trim();
        case EnhancedConditionSingleProperty.IDES:
          return enhancedConditionLog.InternalDescription;
        case EnhancedConditionSingleProperty.EDES:
          return enhancedConditionLog.ExternalDescription;
        case EnhancedConditionSingleProperty.PAIR:
          string str1 = "";
          string pairId = enhancedConditionLog.PairId;
          BorrowerPair borrowerPair = !(pairId == BorrowerPair.All.Id) ? loan.GetBorrowerPair(pairId) : BorrowerPair.All;
          if (borrowerPair != null)
            str1 = borrowerPair.ToString();
          return str1;
        case EnhancedConditionSingleProperty.TYPE:
          return enhancedConditionLog.EnhancedConditionType.ToString();
        case EnhancedConditionSingleProperty.SRC:
          return enhancedConditionLog.Source;
        case EnhancedConditionSingleProperty.RDTL:
          return enhancedConditionLog.Recipient;
        case EnhancedConditionSingleProperty.PTO:
          return enhancedConditionLog.PriorTo;
        case EnhancedConditionSingleProperty.CAT:
          return enhancedConditionLog.Category;
        case EnhancedConditionSingleProperty.SCON:
          SourceOfCondition? sourceOfCondition = enhancedConditionLog.SourceOfCondition;
          if (!sourceOfCondition.HasValue)
            return string.Empty;
          sourceOfCondition = enhancedConditionLog.SourceOfCondition;
          return sourceOfCondition.ToString();
        case EnhancedConditionSingleProperty.SDTE:
          DateTime? startDate = enhancedConditionLog.StartDate;
          if (!startDate.HasValue)
            return string.Empty;
          startDate = enhancedConditionLog.StartDate;
          return this.FormatDateTime(startDate.Value);
        case EnhancedConditionSingleProperty.EDTE:
          DateTime? endDate = enhancedConditionLog.EndDate;
          if (!endDate.HasValue)
            return string.Empty;
          endDate = enhancedConditionLog.EndDate;
          return this.FormatDateTime(endDate.Value);
        case EnhancedConditionSingleProperty.IID:
          return enhancedConditionLog.InternalId;
        case EnhancedConditionSingleProperty.EID:
          return enhancedConditionLog.ExternalId;
        case EnhancedConditionSingleProperty.PINT:
          bool? internalPrint = enhancedConditionLog.InternalPrint;
          if (!internalPrint.HasValue)
            return string.Empty;
          internalPrint = enhancedConditionLog.InternalPrint;
          return this.FormatBool(internalPrint.Value);
        case EnhancedConditionSingleProperty.PEXT:
          bool? externalPrint = enhancedConditionLog.ExternalPrint;
          if (!externalPrint.HasValue)
            return string.Empty;
          externalPrint = enhancedConditionLog.ExternalPrint;
          return this.FormatBool(externalPrint.Value);
        case EnhancedConditionSingleProperty.TOW:
          string str2 = "";
          IList<StatusTrackingDefinition> trackingDefinitions = enhancedConditionLog.Definitions.TrackingDefinitions;
          int num = ((IEnumerable<RoleInfo>) loan.Settings.AllRoles).Where<RoleInfo>((Func<RoleInfo, bool>) (r => r.Name.Equals(trackingValue, StringComparison.OrdinalIgnoreCase))).Select<RoleInfo, int>((Func<RoleInfo, int>) (r => r.ID)).SingleOrDefault<int>();
          if (num == -1)
            return "";
          if (trackingDefinitions != null)
          {
            foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) trackingDefinitions)
            {
              foreach (int allowedRole in trackingDefinition.AllowedRoles)
              {
                if (allowedRole == num)
                  str2 = str2 + trackingDefinition.Name + "  ";
              }
            }
          }
          return str2;
        case EnhancedConditionSingleProperty.ICOM:
          string str3 = "";
          foreach (CommentEntry comment in (CollectionBase) enhancedConditionLog.Comments)
          {
            if (comment.IsInternal)
              str3 = str3 + comment.Comments + "\n";
          }
          return str3;
        case EnhancedConditionSingleProperty.ECOM:
          CommentEntryCollection comments = enhancedConditionLog.Comments;
          string str4 = "";
          foreach (CommentEntry commentEntry in (CollectionBase) comments)
          {
            if (!commentEntry.IsInternal)
              str4 = str4 + commentEntry.Comments + "\n";
          }
          return str4;
        case EnhancedConditionSingleProperty.ADBY:
          string str5 = "";
          List<StatusTrackingEntry> statusTrackingEntries1 = enhancedConditionLog.Trackings.GetStatusTrackingEntries();
          if (statusTrackingEntries1.Count > 0)
          {
            foreach (StatusTrackingEntry statusTrackingEntry in statusTrackingEntries1)
            {
              if (string.Equals(statusTrackingEntry.Status, trackingValue, StringComparison.InvariantCultureIgnoreCase))
              {
                str5 = statusTrackingEntry.UserId;
                break;
              }
            }
          }
          return str5;
        case EnhancedConditionSingleProperty.ADTE:
          string str6 = "";
          List<StatusTrackingEntry> statusTrackingEntries2 = enhancedConditionLog.Trackings.GetStatusTrackingEntries();
          if (statusTrackingEntries2.Count > 0)
          {
            foreach (StatusTrackingEntry statusTrackingEntry in statusTrackingEntries2)
            {
              if (string.Equals(statusTrackingEntry.Status, trackingValue, StringComparison.InvariantCultureIgnoreCase))
              {
                str6 = this.FormatDateTime(statusTrackingEntry.Date);
                break;
              }
            }
          }
          return str6;
        case EnhancedConditionSingleProperty.OWN:
          int? ownerId = enhancedConditionLog.Owner;
          return ((IEnumerable<RoleInfo>) loan.Settings.AllRoles).Where<RoleInfo>((Func<RoleInfo, bool>) (r => r.ID.ToString().Equals(ownerId.ToString(), StringComparison.OrdinalIgnoreCase))).Select<RoleInfo, string>((Func<RoleInfo, string>) (r => r.Name)).SingleOrDefault<string>() ?? "";
        default:
          return "";
      }
    }

    private EnhancedConditionLog getEnhancedConditionLog(
      LoanData loan,
      string conditionName,
      string conditionType)
    {
      EnhancedConditionLog[] enhancedConditions = loan.GetLogList().GetAllEnhancedConditions(false);
      EnhancedConditionLog enhancedConditionLog1 = (EnhancedConditionLog) null;
      if (enhancedConditions != null)
      {
        foreach (EnhancedConditionLog enhancedConditionLog2 in enhancedConditions)
        {
          if (string.Equals(enhancedConditionLog2.Title.Trim(), conditionName, StringComparison.InvariantCultureIgnoreCase) && string.Equals(enhancedConditionLog2.EnhancedConditionType.ToString(), conditionType, StringComparison.InvariantCultureIgnoreCase))
          {
            enhancedConditionLog1 = enhancedConditionLog2;
            break;
          }
        }
      }
      return enhancedConditionLog1;
    }
  }
}
