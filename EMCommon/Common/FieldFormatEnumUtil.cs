// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.FieldFormatEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class FieldFormatEnumUtil
  {
    private static Hashtable _NameToValue = new Hashtable();
    private static Hashtable _ValueToName;

    static FieldFormatEnumUtil()
    {
      FieldFormatEnumUtil._NameToValue.Add((object) "STRING", (object) FieldFormat.STRING);
      FieldFormatEnumUtil._NameToValue.Add((object) "Y/N", (object) FieldFormat.YN);
      FieldFormatEnumUtil._NameToValue.Add((object) "CHECK BOX", (object) FieldFormat.X);
      FieldFormatEnumUtil._NameToValue.Add((object) "ZIPCODE", (object) FieldFormat.ZIPCODE);
      FieldFormatEnumUtil._NameToValue.Add((object) "STATE", (object) FieldFormat.STATE);
      FieldFormatEnumUtil._NameToValue.Add((object) "PHONE", (object) FieldFormat.PHONE);
      FieldFormatEnumUtil._NameToValue.Add((object) "SSN", (object) FieldFormat.SSN);
      FieldFormatEnumUtil._NameToValue.Add((object) "RA_STRING", (object) FieldFormat.RA_STRING);
      FieldFormatEnumUtil._NameToValue.Add((object) "RA_INTEGER (NA or Integer)", (object) FieldFormat.RA_INTEGER);
      FieldFormatEnumUtil._NameToValue.Add((object) "RA_DECIMAL_2 (NA or x,xxx.xx)", (object) FieldFormat.RA_DECIMAL_2);
      FieldFormatEnumUtil._NameToValue.Add((object) "RA_DECIMAL_3 (NA or x,xxx.xxx)", (object) FieldFormat.RA_DECIMAL_3);
      FieldFormatEnumUtil._NameToValue.Add((object) "INTEGER", (object) FieldFormat.INTEGER);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_1 (x,xxx.x)", (object) FieldFormat.DECIMAL_1);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_2 (x,xxx.xx)", (object) FieldFormat.DECIMAL_2);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_3 (x,xxx.xxx)", (object) FieldFormat.DECIMAL_3);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_4 (x,xxx.xxxx)", (object) FieldFormat.DECIMAL_4);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_5 (x,xxx.xxxxx)", (object) FieldFormat.DECIMAL_5);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_6 (x,xxx.xxxxxx)", (object) FieldFormat.DECIMAL_6);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_7 (x,xxx.xxxxxxx)", (object) FieldFormat.DECIMAL_7);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_9 (x,xxx.xxxxxxxxx)", (object) FieldFormat.DECIMAL_9);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL_10 (x,xxx.xxxxxxxxxx)", (object) FieldFormat.DECIMAL_10);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL (x,xxx.xxxx...)", (object) FieldFormat.DECIMAL);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL   (x,xxx.xxxx...)", (object) FieldFormat.DECIMAL);
      FieldFormatEnumUtil._NameToValue.Add((object) "DECIMAL   (A number, but not formatted)", (object) FieldFormat.DECIMAL);
      FieldFormatEnumUtil._NameToValue.Add((object) "DATE (mm/dd/yy)", (object) FieldFormat.DATE);
      FieldFormatEnumUtil._NameToValue.Add((object) "MONTHDAY (mm/dd)", (object) FieldFormat.MONTHDAY);
      FieldFormatEnumUtil._NameToValue.Add((object) "", (object) FieldFormat.NONE);
      FieldFormatEnumUtil._NameToValue.Add((object) "DROPDOWN - Editable", (object) FieldFormat.DROPDOWN);
      FieldFormatEnumUtil._NameToValue.Add((object) "DROPDOWN", (object) FieldFormat.DROPDOWNLIST);
      FieldFormatEnumUtil._NameToValue.Add((object) "AUDIT", (object) FieldFormat.AUDIT);
      FieldFormatEnumUtil._NameToValue.Add((object) "COMMENT", (object) FieldFormat.COMMENT);
      FieldFormatEnumUtil._ValueToName = new Hashtable();
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.STRING, (object) "STRING");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.YN, (object) "Y/N");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.X, (object) "CHECK BOX");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.ZIPCODE, (object) "ZIPCODE");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.STATE, (object) "STATE");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.PHONE, (object) "PHONE");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.SSN, (object) "SSN");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.RA_STRING, (object) "RA_STRING");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.RA_INTEGER, (object) "RA_INTEGER (NA or Integer)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.RA_DECIMAL_2, (object) "RA_DECIMAL_2 (NA or x,xxx.xx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.RA_DECIMAL_3, (object) "RA_DECIMAL_3 (NA or x,xxx.xxx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.INTEGER, (object) "INTEGER");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_1, (object) "DECIMAL_1 (x,xxx.x)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_2, (object) "DECIMAL_2 (x,xxx.xx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_3, (object) "DECIMAL_3 (x,xxx.xxx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_4, (object) "DECIMAL_4 (x,xxx.xxxx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_5, (object) "DECIMAL_5 (x,xxx.xxxxx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_6, (object) "DECIMAL_6 (x,xxx.xxxxxx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_7, (object) "DECIMAL_7 (x,xxx.xxxxxxx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_9, (object) "DECIMAL_9 (x,xxx.xxxxxxxxx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL_10, (object) "DECIMAL_10 (x,xxx.xxxxxxxxxx)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DECIMAL, (object) "DECIMAL (x,xxx.xxxx...)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DATE, (object) "DATE (mm/dd/yy)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.MONTHDAY, (object) "MONTHDAY (mm/dd)");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.NONE, (object) "");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DROPDOWN, (object) "DROPDOWN - Editable");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.DROPDOWNLIST, (object) "DROPDOWN");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.AUDIT, (object) "AUDIT");
      FieldFormatEnumUtil._ValueToName.Add((object) FieldFormat.COMMENT, (object) "COMMENT");
    }

    public static object[] GetDisplayNames(bool bExcludeRAString)
    {
      return FieldFormatEnumUtil.GetDisplayNames(bExcludeRAString, true);
    }

    public static object[] GetDisplayNames(bool bExcludeRAString, bool bExcludeComment)
    {
      ArrayList arrayList = new ArrayList(FieldFormatEnumUtil._ValueToName.Values);
      if (bExcludeRAString)
      {
        arrayList.Remove(FieldFormatEnumUtil._ValueToName[(object) FieldFormat.RA_STRING]);
        arrayList.Remove(FieldFormatEnumUtil._ValueToName[(object) FieldFormat.RA_INTEGER]);
        arrayList.Remove(FieldFormatEnumUtil._ValueToName[(object) FieldFormat.RA_DECIMAL_2]);
        arrayList.Remove(FieldFormatEnumUtil._ValueToName[(object) FieldFormat.RA_DECIMAL_3]);
      }
      if (bExcludeComment)
        arrayList.Remove(FieldFormatEnumUtil._ValueToName[(object) FieldFormat.COMMENT]);
      return arrayList.ToArray();
    }

    public static string ValueToName(FieldFormat val)
    {
      return (string) FieldFormatEnumUtil._ValueToName[(object) val];
    }

    public static FieldFormat NameToValue(string name)
    {
      return (FieldFormat) FieldFormatEnumUtil._NameToValue[(object) name];
    }

    public static FieldFormat GetFieldFormatFromName(string name)
    {
      FieldFormat result;
      if (Enum.TryParse<FieldFormat>(name, out result))
        return result;
      throw new Exception("Field Format '" + name + "' is Invalid.");
    }

    public static bool IsNumeric(FieldFormat format)
    {
      switch (format)
      {
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
        case FieldFormat.DECIMAL_9:
          return true;
        default:
          return false;
      }
    }

    public static FieldFormat GetFieldFormatCategory(FieldFormat format)
    {
      switch (format)
      {
        case FieldFormat.STRING:
        case FieldFormat.YN:
        case FieldFormat.X:
        case FieldFormat.ZIPCODE:
        case FieldFormat.STATE:
        case FieldFormat.PHONE:
        case FieldFormat.SSN:
        case FieldFormat.RA_STRING:
        case FieldFormat.RA_INTEGER:
        case FieldFormat.RA_DECIMAL_2:
        case FieldFormat.RA_DECIMAL_3:
        case FieldFormat.DROPDOWNLIST:
        case FieldFormat.DROPDOWN:
        case FieldFormat.COMMENT:
          return FieldFormat.STRING;
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
        case FieldFormat.DECIMAL_9:
          return FieldFormat.DECIMAL;
        case FieldFormat.DATE:
        case FieldFormat.MONTHDAY:
        case FieldFormat.DATETIME:
          return FieldFormat.DATE;
        case FieldFormat.AUDIT:
          return FieldFormat.AUDIT;
        default:
          return FieldFormat.UNDEFINED;
      }
    }
  }
}
