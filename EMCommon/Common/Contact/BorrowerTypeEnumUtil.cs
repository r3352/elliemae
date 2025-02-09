// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.BorrowerTypeEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class BorrowerTypeEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;

    static BorrowerTypeEnumUtil()
    {
      BorrowerTypeEnumUtil._NameToValue.Add((object) "", (object) BorrowerType.Blank);
      BorrowerTypeEnumUtil._NameToValue.Add((object) "Lead", (object) BorrowerType.Lead);
      BorrowerTypeEnumUtil._NameToValue.Add((object) "Prospect", (object) BorrowerType.Prospect);
      BorrowerTypeEnumUtil._NameToValue.Add((object) "Client", (object) BorrowerType.Client);
      BorrowerTypeEnumUtil._ValueToName = new Hashtable();
      BorrowerTypeEnumUtil._ValueToName.Add((object) BorrowerType.Blank, (object) "");
      BorrowerTypeEnumUtil._ValueToName.Add((object) BorrowerType.Lead, (object) "Lead");
      BorrowerTypeEnumUtil._ValueToName.Add((object) BorrowerType.Prospect, (object) "Prospect");
      BorrowerTypeEnumUtil._ValueToName.Add((object) BorrowerType.Client, (object) "Client");
    }

    public static IDictionary ValueToNameMap => (IDictionary) BorrowerTypeEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      BorrowerType[] values = (BorrowerType[]) Enum.GetValues(typeof (BorrowerType));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) BorrowerTypeEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(BorrowerType val)
    {
      return (string) BorrowerTypeEnumUtil._ValueToName[(object) val];
    }

    public static BorrowerType NameToValue(string name)
    {
      return (BorrowerType) BorrowerTypeEnumUtil._NameToValue[(object) name];
    }
  }
}
