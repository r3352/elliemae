// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.FieldTypeUtilities
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  public static class FieldTypeUtilities
  {
    public static DataConversion GetDataConversionForType(FieldTypes fieldType)
    {
      switch (fieldType)
      {
        case FieldTypes.IsNumeric:
          return DataConversion.Numeric;
        case FieldTypes.IsDate:
        case FieldTypes.IsMonthDay:
        case FieldTypes.IsDateTime:
          return DataConversion.DateTime;
        default:
          return DataConversion.None;
      }
    }

    public static OperatorTypes[] GetOperatorTypesForFieldType(FieldTypes fieldType)
    {
      switch (fieldType)
      {
        case FieldTypes.IsNothing:
          return new OperatorTypes[1];
        case FieldTypes.IsString:
        case FieldTypes.IsPhone:
          return new OperatorTypes[6]
          {
            OperatorTypes.IsExact,
            OperatorTypes.IsNot,
            OperatorTypes.StartsWith,
            OperatorTypes.DoesnotStartWith,
            OperatorTypes.Contains,
            OperatorTypes.DoesnotContain
          };
        case FieldTypes.IsNumeric:
          return new OperatorTypes[8]
          {
            OperatorTypes.Equals,
            OperatorTypes.NotEqual,
            OperatorTypes.GreaterThan,
            OperatorTypes.NotGreaterThan,
            OperatorTypes.LessThan,
            OperatorTypes.NotLessThan,
            OperatorTypes.Between,
            OperatorTypes.NotBetween
          };
        case FieldTypes.IsDate:
        case FieldTypes.IsDateTime:
          return new OperatorTypes[34]
          {
            OperatorTypes.Today,
            OperatorTypes.CurrentWeek,
            OperatorTypes.CurrentMonth,
            OperatorTypes.YearToDate,
            OperatorTypes.PreviousWeek,
            OperatorTypes.PreviousMonth,
            OperatorTypes.PreviousYear,
            OperatorTypes.NextWeek,
            OperatorTypes.NextMonth,
            OperatorTypes.NextYear,
            OperatorTypes.Last7Days,
            OperatorTypes.Last15Days,
            OperatorTypes.Last30Days,
            OperatorTypes.Last60Days,
            OperatorTypes.Last90Days,
            OperatorTypes.Last180Days,
            OperatorTypes.Last365Days,
            OperatorTypes.Next7Days,
            OperatorTypes.Next15Days,
            OperatorTypes.Next30Days,
            OperatorTypes.Next60Days,
            OperatorTypes.Next90Days,
            OperatorTypes.Next180Days,
            OperatorTypes.Next365Days,
            OperatorTypes.Equals,
            OperatorTypes.NotEqual,
            OperatorTypes.DateBefore,
            OperatorTypes.DateOnOrBefore,
            OperatorTypes.DateAfter,
            OperatorTypes.DateOnOrAfter,
            OperatorTypes.DateBetween,
            OperatorTypes.DateNotBetween,
            OperatorTypes.EmptyDate,
            OperatorTypes.NotEmptyDate
          };
        case FieldTypes.IsMonthDay:
          return new OperatorTypes[16]
          {
            OperatorTypes.Equals,
            OperatorTypes.NotEqual,
            OperatorTypes.DateBetween,
            OperatorTypes.DateNotBetween,
            OperatorTypes.DateBefore,
            OperatorTypes.DateOnOrBefore,
            OperatorTypes.DateAfter,
            OperatorTypes.DateOnOrAfter,
            OperatorTypes.EmptyDate,
            OperatorTypes.NotEmptyDate,
            OperatorTypes.CurrentMonth,
            OperatorTypes.CurrentWeek,
            OperatorTypes.NextWeek,
            OperatorTypes.NextMonth,
            OperatorTypes.PreviousWeek,
            OperatorTypes.PreviousMonth
          };
        case FieldTypes.IsOptionList:
          return new OperatorTypes[2]
          {
            OperatorTypes.IsAnyOf,
            OperatorTypes.IsNotAnyOf
          };
        case FieldTypes.IsYesNo:
          return new OperatorTypes[2]
          {
            OperatorTypes.IsYes,
            OperatorTypes.IsNotYes
          };
        case FieldTypes.IsCheckbox:
          return new OperatorTypes[2]
          {
            OperatorTypes.IsChecked,
            OperatorTypes.IsNotChecked
          };
        default:
          throw new Exception("Invalid field type specified");
      }
    }

    public static OperatorTypes ConvertForFieldType(OperatorTypes opType, FieldTypes fieldType)
    {
      switch (fieldType)
      {
        case FieldTypes.IsNumeric:
          return FieldTypeUtilities.ConvertToNumericOperator(opType);
        case FieldTypes.IsDate:
        case FieldTypes.IsMonthDay:
        case FieldTypes.IsDateTime:
          return FieldTypeUtilities.ConvertToDateOperator(opType);
        default:
          return opType;
      }
    }

    public static OperatorTypes ConvertToDateOperator(OperatorTypes opType)
    {
      switch (opType)
      {
        case OperatorTypes.IsExact:
          return OperatorTypes.Equals;
        case OperatorTypes.IsNot:
          return OperatorTypes.NotEqual;
        case OperatorTypes.GreaterThan:
          return OperatorTypes.DateAfter;
        case OperatorTypes.NotGreaterThan:
          return OperatorTypes.DateOnOrBefore;
        case OperatorTypes.LessThan:
          return OperatorTypes.DateBefore;
        case OperatorTypes.NotLessThan:
          return OperatorTypes.DateOnOrAfter;
        default:
          return opType;
      }
    }

    public static OperatorTypes ConvertToNumericOperator(OperatorTypes opType)
    {
      if (opType == OperatorTypes.IsExact)
        return OperatorTypes.Equals;
      return opType == OperatorTypes.IsNot ? OperatorTypes.NotEqual : opType;
    }

    public static FieldFormat FieldTypeToFieldFormat(FieldTypes fieldType)
    {
      switch (fieldType)
      {
        case FieldTypes.IsNothing:
          return FieldFormat.UNDEFINED;
        case FieldTypes.IsNumeric:
          return FieldFormat.DECIMAL;
        case FieldTypes.IsDate:
          return FieldFormat.DATE;
        case FieldTypes.IsPhone:
          return FieldFormat.PHONE;
        case FieldTypes.IsMonthDay:
          return FieldFormat.MONTHDAY;
        case FieldTypes.IsOptionList:
          return FieldFormat.DROPDOWNLIST;
        case FieldTypes.IsYesNo:
          return FieldFormat.YN;
        case FieldTypes.IsCheckbox:
          return FieldFormat.X;
        case FieldTypes.IsDateTime:
          return FieldFormat.DATETIME;
        default:
          return FieldFormat.STRING;
      }
    }

    public static FieldTypes FieldFormatToFieldType(FieldFormat format)
    {
      switch (format)
      {
        case FieldFormat.YN:
          return FieldTypes.IsYesNo;
        case FieldFormat.X:
          return FieldTypes.IsCheckbox;
        case FieldFormat.PHONE:
          return FieldTypes.IsPhone;
        case FieldFormat.INTEGER:
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return FieldTypes.IsNumeric;
        case FieldFormat.DATE:
          return FieldTypes.IsDate;
        case FieldFormat.MONTHDAY:
          return FieldTypes.IsMonthDay;
        case FieldFormat.DATETIME:
          return FieldTypes.IsDateTime;
        case FieldFormat.DROPDOWNLIST:
          return FieldTypes.IsOptionList;
        default:
          return FieldTypes.IsString;
      }
    }
  }
}
