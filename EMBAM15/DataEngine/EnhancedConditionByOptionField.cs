// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EnhancedConditionByOptionField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class EnhancedConditionByOptionField : VirtualField
  {
    private const string FieldPrefix = "ENHOP";
    private EnhancedConditionByOptionProperty property;
    private bool allowInReportingDb = true;

    public EnhancedConditionByOptionProperty Property => this.property;

    public EnhancedConditionByOptionField(
      EnhancedConditionByOptionProperty property,
      string description,
      FieldFormat format)
      : this(property, description, format, true)
    {
    }

    public EnhancedConditionByOptionField(
      EnhancedConditionByOptionProperty property,
      string description,
      FieldFormat format,
      bool allowInReportingDb)
      : base("ENHOP." + property.ToString(), description, format, FieldInstanceSpecifierType.EnhancedConditionByOption)
    {
      this.property = property;
      this.allowInReportingDb = allowInReportingDb;
    }

    public EnhancedConditionByOptionField(
      EnhancedConditionByOptionField parent,
      string documentTitle)
      : base((VirtualField) parent, (object) documentTitle)
    {
      this.property = parent.property;
      this.allowInReportingDb = parent.AllowInReportingDatabase;
    }

    public override VirtualFieldType VirtualFieldType
    {
      get => VirtualFieldType.EnhancedConditionFieldByOption;
    }

    public override bool AllowInReportingDatabase => this.allowInReportingDb;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new EnhancedConditionByOptionField(this, string.Concat(instanceSpecifier));
    }

    protected override string Evaluate(LoanData loan)
    {
      string str1 = string.Empty;
      string[] strArray = this.InstanceSpecifier.ToString().Split('.');
      string a = "";
      string conditionType = "";
      string fieldName = "";
      string str2 = "";
      if (strArray.Length != 0)
        a = strArray[0];
      if (strArray.Length > 1)
        conditionType = strArray[1];
      if (strArray.Length > 2)
        str2 = strArray[2];
      if (strArray.Length > 3)
        fieldName = strArray[3];
      EnhancedConditionLog[] enhancedConditions = loan.GetLogList().GetAllEnhancedConditions(false);
      if (((IEnumerable<EnhancedConditionLog>) enhancedConditions).Count<EnhancedConditionLog>() < 0)
        return "";
      bool flag1 = false;
      bool flag2 = true;
      List<EnhancedConditionLog> list = new List<EnhancedConditionLog>((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l =>
      {
        bool? internalPrint = l.InternalPrint;
        bool flag3 = true;
        if (internalPrint.GetValueOrDefault() == flag3 & internalPrint.HasValue)
          return true;
        bool? externalPrint = l.ExternalPrint;
        bool flag4 = true;
        return externalPrint.GetValueOrDefault() == flag4 & externalPrint.HasValue;
      })).ToList<EnhancedConditionLog>();
      if (string.Equals(a, "INTERNAL", StringComparison.InvariantCultureIgnoreCase) || string.Equals(a, "EXTERNAL", StringComparison.InvariantCultureIgnoreCase))
      {
        flag2 = false;
        if (string.Equals(a, "INTERNAL", StringComparison.InvariantCultureIgnoreCase))
        {
          flag1 = true;
          list = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l =>
          {
            bool? internalPrint = l.InternalPrint;
            bool flag5 = true;
            return internalPrint.GetValueOrDefault() == flag5 & internalPrint.HasValue;
          })).ToList<EnhancedConditionLog>();
        }
        else if (string.Equals(a, "EXTERNAL", StringComparison.InvariantCultureIgnoreCase))
          list = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l =>
          {
            bool? externalPrint = l.ExternalPrint;
            bool flag6 = true;
            return externalPrint.GetValueOrDefault() == flag6 & externalPrint.HasValue;
          })).ToList<EnhancedConditionLog>();
      }
      if (!string.IsNullOrEmpty(conditionType) && !string.Equals("ALL", conditionType, StringComparison.InvariantCultureIgnoreCase))
        list = list.Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l => string.Equals(l.EnhancedConditionType, conditionType, StringComparison.InvariantCultureIgnoreCase))).ToList<EnhancedConditionLog>();
      switch (str2)
      {
        case "OP":
          list = list.Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l => l.StatusOpen)).ToList<EnhancedConditionLog>();
          break;
        case "CL":
          list = list.Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l => !l.StatusOpen)).ToList<EnhancedConditionLog>();
          break;
      }
      switch (this.property)
      {
        case EnhancedConditionByOptionProperty.COP:
          if (!string.IsNullOrEmpty(fieldName))
            list = list.Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l => string.Equals(l.Category, fieldName, StringComparison.InvariantCultureIgnoreCase))).ToList<EnhancedConditionLog>();
          if (list != null)
          {
            foreach (EnhancedConditionLog enhancedConditionLog in list)
            {
              if (str1 != string.Empty)
                str1 += "\r\n";
              str1 = str1 + enhancedConditionLog.Title + "  ";
              str1 += flag1 || flag2 && enhancedConditionLog.ExternalDescription == string.Empty ? enhancedConditionLog.InternalDescription : enhancedConditionLog.ExternalDescription;
            }
          }
          return str1;
        case EnhancedConditionByOptionProperty.PTO:
          if (!string.IsNullOrEmpty(fieldName))
            list = list.Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l => string.Equals(l.PriorTo, fieldName, StringComparison.InvariantCultureIgnoreCase))).ToList<EnhancedConditionLog>();
          if (list != null)
          {
            foreach (EnhancedConditionLog enhancedConditionLog in list)
            {
              if (str1 != string.Empty)
                str1 += "\r\n";
              str1 = str1 + enhancedConditionLog.Title + "  ";
              str1 += flag1 || flag2 && enhancedConditionLog.ExternalDescription == string.Empty ? enhancedConditionLog.InternalDescription : enhancedConditionLog.ExternalDescription;
            }
          }
          return str1;
        case EnhancedConditionByOptionProperty.SOP:
          if (!string.IsNullOrEmpty(fieldName))
            list = list.Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l => string.Equals(l.Source, fieldName, StringComparison.InvariantCultureIgnoreCase))).ToList<EnhancedConditionLog>();
          if (list != null)
          {
            foreach (EnhancedConditionLog enhancedConditionLog in list)
            {
              if (str1 != string.Empty)
                str1 += "\r\n";
              str1 = str1 + enhancedConditionLog.Title + "  ";
              str1 += flag1 || flag2 && enhancedConditionLog.ExternalDescription == string.Empty ? enhancedConditionLog.InternalDescription : enhancedConditionLog.ExternalDescription;
            }
          }
          return str1;
        case EnhancedConditionByOptionProperty.ROP:
          if (!string.IsNullOrEmpty(fieldName))
            list = list.Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l => string.Equals(l.Recipient, fieldName, StringComparison.InvariantCultureIgnoreCase))).ToList<EnhancedConditionLog>();
          if (list != null)
          {
            foreach (EnhancedConditionLog enhancedConditionLog in list)
            {
              if (str1 != string.Empty)
                str1 += "\r\n";
              str1 = str1 + enhancedConditionLog.Title + "  ";
              str1 += flag1 || flag2 && enhancedConditionLog.ExternalDescription == string.Empty ? enhancedConditionLog.InternalDescription : enhancedConditionLog.ExternalDescription;
            }
          }
          return str1;
        case EnhancedConditionByOptionProperty.TOP:
          if (!string.IsNullOrEmpty(fieldName))
            list = list.Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (l => string.Equals(l.Status, fieldName, StringComparison.InvariantCultureIgnoreCase))).ToList<EnhancedConditionLog>();
          if (list != null)
          {
            foreach (EnhancedConditionLog enhancedConditionLog in list)
            {
              if (str1 != string.Empty)
                str1 += "\r\n";
              str1 = str1 + enhancedConditionLog.Title + "  ";
              str1 += flag1 || flag2 && enhancedConditionLog.ExternalDescription == string.Empty ? enhancedConditionLog.InternalDescription : enhancedConditionLog.ExternalDescription;
            }
          }
          return str1;
        case EnhancedConditionByOptionProperty.TOW:
          if (!string.IsNullOrEmpty(fieldName) && list != null)
          {
            foreach (EnhancedConditionLog enhancedConditionLog in list)
            {
              IList<StatusTrackingDefinition> trackingDefinitions = enhancedConditionLog.Definitions.TrackingDefinitions;
              int num = ((IEnumerable<RoleInfo>) loan.Settings.AllRoles).Where<RoleInfo>((Func<RoleInfo, bool>) (r => r.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase))).Select<RoleInfo, int>((Func<RoleInfo, int>) (r => r.ID)).SingleOrDefault<int>();
              if (num == -1)
                return "";
              if (trackingDefinitions != null)
              {
                foreach (StatusTrackingDefinition trackingDefinition in (IEnumerable<StatusTrackingDefinition>) trackingDefinitions)
                {
                  if (trackingDefinition.AllowedRoles != null && ((IEnumerable<int>) trackingDefinition.AllowedRoles).Contains<int>(num))
                  {
                    if (str1 != string.Empty)
                      str1 += "\r\n";
                    str1 = str1 + enhancedConditionLog.Title + "  ";
                    str1 += flag1 || flag2 && enhancedConditionLog.ExternalDescription == string.Empty ? enhancedConditionLog.InternalDescription : enhancedConditionLog.ExternalDescription;
                    break;
                  }
                }
              }
            }
          }
          return str1;
        default:
          return str1;
      }
    }
  }
}
