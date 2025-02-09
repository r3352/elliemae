// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.EmploymentStatusEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class EmploymentStatusEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;

    static EmploymentStatusEnumUtil()
    {
      EmploymentStatusEnumUtil._NameToValue.Add((object) "", (object) EmploymentStatus.Blank);
      EmploymentStatusEnumUtil._NameToValue.Add((object) "Employed", (object) EmploymentStatus.Employed);
      EmploymentStatusEnumUtil._NameToValue.Add((object) "Self-Employed", (object) EmploymentStatus.SelfEmployed);
      EmploymentStatusEnumUtil._NameToValue.Add((object) "SelfEmployed", (object) EmploymentStatus.SelfEmployed);
      EmploymentStatusEnumUtil._NameToValue.Add((object) "Unemployed", (object) EmploymentStatus.Unemployed);
      EmploymentStatusEnumUtil._ValueToName = new Hashtable();
      EmploymentStatusEnumUtil._ValueToName.Add((object) EmploymentStatus.Blank, (object) "");
      EmploymentStatusEnumUtil._ValueToName.Add((object) EmploymentStatus.Employed, (object) "Employed");
      EmploymentStatusEnumUtil._ValueToName.Add((object) EmploymentStatus.SelfEmployed, (object) "Self-Employed");
      EmploymentStatusEnumUtil._ValueToName.Add((object) EmploymentStatus.Unemployed, (object) "Unemployed");
    }

    public static IDictionary ValueToNameMap => (IDictionary) EmploymentStatusEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      EmploymentStatus[] values = (EmploymentStatus[]) Enum.GetValues(typeof (EmploymentStatus));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) EmploymentStatusEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(EmploymentStatus val)
    {
      return (string) EmploymentStatusEnumUtil._ValueToName[(object) val];
    }

    public static EmploymentStatus NameToValue(string name)
    {
      return EmploymentStatusEnumUtil._NameToValue.Contains((object) name) ? (EmploymentStatus) EmploymentStatusEnumUtil._NameToValue[(object) name] : EmploymentStatus.Blank;
    }
  }
}
