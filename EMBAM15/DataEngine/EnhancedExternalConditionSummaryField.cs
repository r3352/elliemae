// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EnhancedExternalConditionSummaryField
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
  public class EnhancedExternalConditionSummaryField : VirtualField
  {
    private EnhancedExternalConditionProperty property;
    private bool allowInReportingDb;

    public EnhancedExternalConditionProperty Property => this.property;

    public override bool AllowInReportingDatabase => this.allowInReportingDb;

    public EnhancedExternalConditionSummaryField(
      EnhancedExternalConditionProperty property,
      string description,
      FieldFormat format,
      bool multiInstance)
      : this(property, description, format, true, multiInstance)
    {
    }

    public EnhancedExternalConditionSummaryField(
      EnhancedExternalConditionProperty property,
      string description,
      FieldFormat format,
      bool allowInReportingDb,
      bool multiInstance)
      : base(EnhancedExternalConditionSummaryField.generateFieldId(property), description, format, multiInstance ? FieldInstanceSpecifierType.EnhancedCondition : FieldInstanceSpecifierType.None)
    {
      this.property = property;
      this.allowInReportingDb = allowInReportingDb;
    }

    public EnhancedExternalConditionSummaryField(
      EnhancedExternalConditionSummaryField parent,
      string conditionType)
      : base((VirtualField) parent, (object) conditionType)
    {
      this.property = parent.property;
      this.allowInReportingDb = parent.AllowInReportingDatabase;
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.EnhancedConditionFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new EnhancedExternalConditionSummaryField(this, string.Concat(instanceSpecifier));
    }

    public override int ReportingDatabaseColumnSize => 4096;

    private static string generateFieldId(EnhancedExternalConditionProperty property)
    {
      string fieldId = "ENHCOND.";
      switch (property)
      {
        case EnhancedExternalConditionProperty.AllExternalType:
          fieldId += "EXT.TYPE";
          break;
        case EnhancedExternalConditionProperty.AllExternalOpenType:
          fieldId += "EXT.OPEN.TYPE";
          break;
        case EnhancedExternalConditionProperty.AllExternalCount:
          fieldId += "EXT.TYPE.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.AllExternalOpenCount:
          fieldId += "EXT.OPEN.TYPE.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.ExternalOpenTypeCount:
          fieldId += "EXT.OPEN.TYPE.COUNT";
          break;
        case EnhancedExternalConditionProperty.ExternalTypeCount:
          fieldId += "EXT.TYPE.COUNT";
          break;
        case EnhancedExternalConditionProperty.AllConditionExternalOpenType:
          fieldId += "EXT.OPEN.TYPE.ALL";
          break;
        case EnhancedExternalConditionProperty.AllConditionExternalType:
          fieldId += "EXT.TYPE.ALL";
          break;
        case EnhancedExternalConditionProperty.AllInternalType:
          fieldId += "INT.TYPE";
          break;
        case EnhancedExternalConditionProperty.AllInternalOpenType:
          fieldId += "INT.OPEN.TYPE";
          break;
        case EnhancedExternalConditionProperty.AllConditionInternalType:
          fieldId += "INT.TYPE.ALL";
          break;
        case EnhancedExternalConditionProperty.AllConditionInternalOpenType:
          fieldId += "INT.OPEN.TYPE.ALL";
          break;
        case EnhancedExternalConditionProperty.InternalTypeCount:
          fieldId += "INT.TYPE.COUNT";
          break;
        case EnhancedExternalConditionProperty.InternalOpenTypeCount:
          fieldId += "INT.OPEN.TYPE.COUNT";
          break;
        case EnhancedExternalConditionProperty.AllInternalCount:
          fieldId += "INT.TYPE.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.AllInternalOpenCount:
          fieldId += "INT.OPEN.TYPE.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.AllOpenCount:
          fieldId += "OPEN.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.AllCount:
          fieldId += "COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.AllOpenType:
          fieldId += "OPEN.ALL";
          break;
        case EnhancedExternalConditionProperty.AllType:
          fieldId += "ALL";
          break;
        case EnhancedExternalConditionProperty.AllExternalPulibshedCount:
          fieldId += "EXT.OPEN.PUB.TYPE.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.ExternalTypeSatisfied:
          fieldId += "EXT.SATISFIED.TYPE";
          break;
        case EnhancedExternalConditionProperty.AllExternalSatisfied:
          fieldId += "EXT.SATISFIED.TYPE.ALL";
          break;
        case EnhancedExternalConditionProperty.ExternalSatisfiedCount:
          fieldId += "EXT.SATISFIED.TYPE.COUNT";
          break;
        case EnhancedExternalConditionProperty.AllExternalSatisfiedCount:
          fieldId += "EXT.SATISFIED.TYPE.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.InternalTypeSatisfied:
          fieldId += "INT.SATISFIED.TYPE";
          break;
        case EnhancedExternalConditionProperty.AllInternalTypeSatisfied:
          fieldId += "INT.SATISFIED.TYPE.ALL";
          break;
        case EnhancedExternalConditionProperty.InternalSatisfiedCount:
          fieldId += "INT.SATISFIED.TYPE.COUNT";
          break;
        case EnhancedExternalConditionProperty.AllInternalSatisfiedCount:
          fieldId += "INT.SATISFIED.TYPE.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.AllSatisfiedCount:
          fieldId += "SATISFIED.COUNT.ALL";
          break;
        case EnhancedExternalConditionProperty.AllSatisfied:
          fieldId += "SATISFIED.ALL";
          break;
      }
      return fieldId;
    }

    protected override string Evaluate(LoanData loan)
    {
      string str = string.Empty;
      int num1 = 1;
      EnhancedConditionLog[] enhancedConditions = loan.GetLogList().GetAllEnhancedConditions(false);
      switch (this.property)
      {
        case EnhancedExternalConditionProperty.AllExternalType:
        case EnhancedExternalConditionProperty.AllExternalOpenType:
        case EnhancedExternalConditionProperty.AllConditionExternalOpenType:
        case EnhancedExternalConditionProperty.AllConditionExternalType:
        case EnhancedExternalConditionProperty.AllInternalType:
        case EnhancedExternalConditionProperty.AllInternalOpenType:
        case EnhancedExternalConditionProperty.AllConditionInternalType:
        case EnhancedExternalConditionProperty.AllConditionInternalOpenType:
        case EnhancedExternalConditionProperty.ExternalTypeSatisfied:
        case EnhancedExternalConditionProperty.AllExternalSatisfied:
        case EnhancedExternalConditionProperty.InternalTypeSatisfied:
        case EnhancedExternalConditionProperty.AllInternalTypeSatisfied:
          foreach (EnhancedConditionLog enhancedConditionLog in enhancedConditions)
          {
            bool flag1 = false;
            bool? nullable = enhancedConditionLog.ExternalPrint;
            bool flag2 = true;
            if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue)
            {
              if (this.property == EnhancedExternalConditionProperty.AllExternalType && string.Compare(enhancedConditionLog.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.AllExternalOpenType && enhancedConditionLog.StatusOpen && string.Compare(enhancedConditionLog.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.AllConditionExternalOpenType && enhancedConditionLog.StatusOpen)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.AllConditionExternalType)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.AllExternalSatisfied && !enhancedConditionLog.StatusOpen)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.ExternalTypeSatisfied && !enhancedConditionLog.StatusOpen && string.Compare(enhancedConditionLog.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0)
                flag1 = true;
            }
            nullable = enhancedConditionLog.InternalPrint;
            bool flag3 = true;
            if (nullable.GetValueOrDefault() == flag3 & nullable.HasValue)
            {
              if (this.property == EnhancedExternalConditionProperty.AllInternalType && string.Compare(enhancedConditionLog.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.AllInternalOpenType && enhancedConditionLog.StatusOpen && string.Compare(enhancedConditionLog.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.AllConditionInternalType)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.AllConditionInternalOpenType && enhancedConditionLog.StatusOpen)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.InternalTypeSatisfied && !enhancedConditionLog.StatusOpen && string.Compare(enhancedConditionLog.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0)
                flag1 = true;
              else if (this.property == EnhancedExternalConditionProperty.AllInternalTypeSatisfied && !enhancedConditionLog.StatusOpen)
                flag1 = true;
            }
            if (flag1)
            {
              if (str != string.Empty)
                str += "\r\n";
              str = str + (object) num1 + "  " + enhancedConditionLog.Status + "    " + (this.property.ToString().Contains("External") ? (object) enhancedConditionLog.ExternalDescription : (object) enhancedConditionLog.InternalDescription);
              ++num1;
            }
          }
          return str;
        case EnhancedExternalConditionProperty.AllExternalCount:
          int num2 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? externalPrint = x.ExternalPrint;
            bool flag = true;
            return externalPrint.GetValueOrDefault() == flag & externalPrint.HasValue;
          })).Count<EnhancedConditionLog>();
          return num2 != 0 ? num2.ToString() : "";
        case EnhancedExternalConditionProperty.AllExternalOpenCount:
          int num3 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? externalPrint = x.ExternalPrint;
            bool flag = true;
            return externalPrint.GetValueOrDefault() == flag & externalPrint.HasValue && x.StatusOpen;
          })).Count<EnhancedConditionLog>();
          return num3 != 0 ? num3.ToString() : "";
        case EnhancedExternalConditionProperty.ExternalOpenTypeCount:
          int num4 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? externalPrint = x.ExternalPrint;
            bool flag = true;
            return externalPrint.GetValueOrDefault() == flag & externalPrint.HasValue && x.StatusOpen && string.Compare(x.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0;
          })).Count<EnhancedConditionLog>();
          return num4 != 0 ? num4.ToString() : "";
        case EnhancedExternalConditionProperty.ExternalTypeCount:
          int num5 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? externalPrint = x.ExternalPrint;
            bool flag = true;
            return externalPrint.GetValueOrDefault() == flag & externalPrint.HasValue && string.Compare(x.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0;
          })).Count<EnhancedConditionLog>();
          return num5 != 0 ? num5.ToString() : "";
        case EnhancedExternalConditionProperty.InternalTypeCount:
          int num6 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? internalPrint = x.InternalPrint;
            bool flag = true;
            return internalPrint.GetValueOrDefault() == flag & internalPrint.HasValue && string.Compare(x.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0;
          })).Count<EnhancedConditionLog>();
          return num6 != 0 ? num6.ToString() : "";
        case EnhancedExternalConditionProperty.InternalOpenTypeCount:
          int num7 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? internalPrint = x.InternalPrint;
            bool flag = true;
            return internalPrint.GetValueOrDefault() == flag & internalPrint.HasValue && x.StatusOpen && string.Compare(x.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0;
          })).Count<EnhancedConditionLog>();
          return num7 != 0 ? num7.ToString() : "";
        case EnhancedExternalConditionProperty.AllInternalCount:
          int num8 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? internalPrint = x.InternalPrint;
            bool flag = true;
            return internalPrint.GetValueOrDefault() == flag & internalPrint.HasValue;
          })).Count<EnhancedConditionLog>();
          return num8 != 0 ? num8.ToString() : "";
        case EnhancedExternalConditionProperty.AllInternalOpenCount:
          int num9 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? internalPrint = x.InternalPrint;
            bool flag = true;
            return internalPrint.GetValueOrDefault() == flag & internalPrint.HasValue && x.StatusOpen;
          })).Count<EnhancedConditionLog>();
          return num9 != 0 ? num9.ToString() : "";
        case EnhancedExternalConditionProperty.AllOpenCount:
          int num10 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x => x.StatusOpen)).Count<EnhancedConditionLog>();
          return num10 != 0 ? num10.ToString() : "";
        case EnhancedExternalConditionProperty.AllCount:
          int num11 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? internalPrint = x.InternalPrint;
            bool flag4 = true;
            if (internalPrint.GetValueOrDefault() == flag4 & internalPrint.HasValue)
              return true;
            bool? externalPrint = x.ExternalPrint;
            bool flag5 = true;
            return externalPrint.GetValueOrDefault() == flag5 & externalPrint.HasValue;
          })).Count<EnhancedConditionLog>();
          return num11 != 0 ? num11.ToString() : "";
        case EnhancedExternalConditionProperty.AllOpenType:
        case EnhancedExternalConditionProperty.AllType:
        case EnhancedExternalConditionProperty.AllSatisfied:
          foreach (EnhancedConditionLog enhancedConditionLog in enhancedConditions)
          {
            bool flag = false;
            if (this.property == EnhancedExternalConditionProperty.AllOpenType && enhancedConditionLog.StatusOpen)
              flag = true;
            else if (this.property == EnhancedExternalConditionProperty.AllType)
              flag = true;
            else if (this.property == EnhancedExternalConditionProperty.AllSatisfied && !enhancedConditionLog.StatusOpen)
              flag = true;
            if (flag)
            {
              if (str != string.Empty)
                str += "\r\n";
              str = str + (object) num1 + "  " + enhancedConditionLog.Status + "    " + enhancedConditionLog.ExternalDescription;
              ++num1;
            }
          }
          return str;
        case EnhancedExternalConditionProperty.AllExternalPulibshedCount:
          int num12 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? externalPrint = x.ExternalPrint;
            bool flag = true;
            return externalPrint.GetValueOrDefault() == flag & externalPrint.HasValue && x.StatusOpen && x.PublishedDate.HasValue;
          })).Count<EnhancedConditionLog>();
          return num12 != 0 ? num12.ToString() : "";
        case EnhancedExternalConditionProperty.ExternalSatisfiedCount:
          int num13 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? externalPrint = x.ExternalPrint;
            bool flag = true;
            return externalPrint.GetValueOrDefault() == flag & externalPrint.HasValue && !x.StatusOpen && string.Compare(x.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0;
          })).Count<EnhancedConditionLog>();
          return num13 != 0 ? num13.ToString() : "";
        case EnhancedExternalConditionProperty.AllExternalSatisfiedCount:
          int num14 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? externalPrint = x.ExternalPrint;
            bool flag = true;
            return externalPrint.GetValueOrDefault() == flag & externalPrint.HasValue && !x.StatusOpen;
          })).Count<EnhancedConditionLog>();
          return num14 != 0 ? num14.ToString() : "";
        case EnhancedExternalConditionProperty.InternalSatisfiedCount:
          int num15 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? internalPrint = x.InternalPrint;
            bool flag = true;
            return internalPrint.GetValueOrDefault() == flag & internalPrint.HasValue && !x.StatusOpen && string.Compare(x.EnhancedConditionType, string.Concat(this.InstanceSpecifier), true) == 0;
          })).Count<EnhancedConditionLog>();
          return num15 != 0 ? num15.ToString() : "";
        case EnhancedExternalConditionProperty.AllInternalSatisfiedCount:
          int num16 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? internalPrint = x.InternalPrint;
            bool flag = true;
            return internalPrint.GetValueOrDefault() == flag & internalPrint.HasValue && !x.StatusOpen;
          })).Count<EnhancedConditionLog>();
          return num16 != 0 ? num16.ToString() : "";
        case EnhancedExternalConditionProperty.AllSatisfiedCount:
          int num17 = ((IEnumerable<EnhancedConditionLog>) enhancedConditions).Where<EnhancedConditionLog>((Func<EnhancedConditionLog, bool>) (x =>
          {
            bool? internalPrint = x.InternalPrint;
            bool flag6 = true;
            if (!(internalPrint.GetValueOrDefault() == flag6 & internalPrint.HasValue))
            {
              bool? externalPrint = x.ExternalPrint;
              bool flag7 = true;
              if (!(externalPrint.GetValueOrDefault() == flag7 & externalPrint.HasValue))
                return false;
            }
            return !x.StatusOpen;
          })).Count<EnhancedConditionLog>();
          return num17 != 0 ? num17.ToString() : "";
        default:
          return str;
      }
    }
  }
}
