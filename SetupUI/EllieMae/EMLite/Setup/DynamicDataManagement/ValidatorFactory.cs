// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.ValidatorFactory
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class ValidatorFactory
  {
    private static Dictionary<FieldFormat, IValidator> _validators = new Dictionary<FieldFormat, IValidator>();

    public static IValidator GetValidator(FieldFormat fieldFormat)
    {
      IValidator validator = (IValidator) null;
      if (ValidatorFactory._validators.TryGetValue(fieldFormat, out validator))
        return validator;
      ValidatorFactory._validators.Add(fieldFormat, ValidatorFactory.CreateValidator(fieldFormat));
      return ValidatorFactory._validators[fieldFormat];
    }

    private static IValidator CreateValidator(FieldFormat fieldFormat)
    {
      switch (fieldFormat)
      {
        case FieldFormat.NONE:
          return (IValidator) new OutputValidator();
        case FieldFormat.STRING:
        case FieldFormat.RA_STRING:
        case FieldFormat.RA_INTEGER:
        case FieldFormat.RA_DECIMAL_2:
        case FieldFormat.RA_DECIMAL_3:
          return (IValidator) new StringValidator();
        case FieldFormat.YN:
          return (IValidator) new YesNoValidator();
        case FieldFormat.X:
          return (IValidator) new XValidator();
        case FieldFormat.ZIPCODE:
          return (IValidator) new ZipCodeValidator();
        case FieldFormat.STATE:
          return (IValidator) new StateValidator();
        case FieldFormat.PHONE:
          return (IValidator) new PhoneNumberValidator();
        case FieldFormat.SSN:
          return (IValidator) new SSNValidator();
        case FieldFormat.INTEGER:
          return (IValidator) new IntegerValidator();
        case FieldFormat.DECIMAL_1:
          return (IValidator) new Decimal1Validator();
        case FieldFormat.DECIMAL_2:
          return (IValidator) new Decimal2Validator();
        case FieldFormat.DECIMAL_3:
          return (IValidator) new Decimal3Validator();
        case FieldFormat.DECIMAL_4:
          return (IValidator) new Decimal4Validator();
        case FieldFormat.DECIMAL_6:
          return (IValidator) new Decimal6Validator();
        case FieldFormat.DECIMAL_5:
          return (IValidator) new Decimal5Validator();
        case FieldFormat.DECIMAL_7:
          return (IValidator) new Decimal7Validator();
        case FieldFormat.DECIMAL:
          return (IValidator) new DecimalValidator();
        case FieldFormat.DECIMAL_10:
          return (IValidator) new Decimal10Validator();
        case FieldFormat.DATE:
          return (IValidator) new DateValidator();
        case FieldFormat.MONTHDAY:
          return (IValidator) new MonthDayValidator();
        case FieldFormat.DATETIME:
          return (IValidator) new DateTimeValidator();
        case FieldFormat.DROPDOWNLIST:
          return (IValidator) new DropDownListValidator();
        case FieldFormat.DROPDOWN:
          return (IValidator) new DropDownValidator();
        case FieldFormat.AUDIT:
          return (IValidator) new AuditValidator();
        default:
          throw new ArgumentException(string.Format("A validator for field format {0} does not exist", (object) fieldFormat.ToString()));
      }
    }
  }
}
