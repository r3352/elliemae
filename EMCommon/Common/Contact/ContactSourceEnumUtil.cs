// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.ContactSourceEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.ContactUI;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class ContactSourceEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;
    private static Hashtable _ValueStringToValue;

    static ContactSourceEnumUtil()
    {
      ContactSourceEnumUtil._NameToValue.Add((object) "Entered", (object) ContactSource.Entered);
      ContactSourceEnumUtil._NameToValue.Add((object) "Imported from Lead Center", (object) ContactSource.LeadCenter);
      ContactSourceEnumUtil._NameToValue.Add((object) "Imported from Point", (object) ContactSource.Point);
      ContactSourceEnumUtil._NameToValue.Add((object) "Imported from Genesis", (object) ContactSource.Genesis);
      ContactSourceEnumUtil._NameToValue.Add((object) "Imported from Contour", (object) ContactSource.Contour);
      ContactSourceEnumUtil._NameToValue.Add((object) "Imported from Fannie Mae", (object) ContactSource.FannieMae);
      ContactSourceEnumUtil._NameToValue.Add((object) "Imported from Outlook", (object) ContactSource.Outlook);
      ContactSourceEnumUtil._NameToValue.Add((object) "Imported from CSV", (object) ContactSource.CSV);
      ContactSourceEnumUtil._NameToValue.Add((object) "Imported from ACT!", (object) ContactSource.Act);
      ContactSourceEnumUtil._NameToValue.Add((object) "No event details available", (object) ContactSource.NotAvailable);
      ContactSourceEnumUtil._ValueToName = new Hashtable();
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.Entered, (object) "Entered");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.LeadCenter, (object) "Imported from Lead Center");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.Point, (object) "Imported from Point");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.Genesis, (object) "Imported from Genesis");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.Contour, (object) "Imported from Contour");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.FannieMae, (object) "Imported from Fannie Mae");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.Outlook, (object) "Imported from Outlook");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.CSV, (object) "Imported from CSV");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.Act, (object) "Imported from ACT!");
      ContactSourceEnumUtil._ValueToName.Add((object) ContactSource.NotAvailable, (object) "No event details available");
      ContactSourceEnumUtil._ValueStringToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "Entered", (object) ContactSource.Entered);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "LeadCenter", (object) ContactSource.LeadCenter);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "Point", (object) ContactSource.Point);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "Genesis", (object) ContactSource.Genesis);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "Contour", (object) ContactSource.Contour);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "FannieMae", (object) ContactSource.FannieMae);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "Outlook", (object) ContactSource.Outlook);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "CSV", (object) ContactSource.CSV);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "Act", (object) ContactSource.Act);
      ContactSourceEnumUtil._ValueStringToValue.Add((object) "NotAvailable", (object) ContactSource.NotAvailable);
    }

    public static object[] GetDisplayNames()
    {
      ContactSource[] values = (ContactSource[]) Enum.GetValues(typeof (ContactSource));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) ContactSourceEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static object[] GetDisplayNames(ContactType contactType)
    {
      return contactType == ContactType.Borrower ? ContactSourceEnumUtil.GetDisplayNames() : ContactSourceEnumUtil.GetDisplayNamesForBizContacts();
    }

    public static object[] GetDisplayNamesForBizContacts()
    {
      ContactSource[] values = (ContactSource[]) Enum.GetValues(typeof (ContactSource));
      object[] namesForBizContacts = new object[values.Length - 4];
      int index1 = 0;
      int index2 = 0;
      for (; index1 < values.Length; ++index1)
      {
        if (values[index1] != ContactSource.LeadCenter && values[index1] != ContactSource.FannieMae && values[index1] != ContactSource.Contour && values[index1] != ContactSource.Genesis)
        {
          namesForBizContacts[index2] = (object) ContactSourceEnumUtil.ValueToName(values[index1]);
          ++index2;
        }
      }
      return namesForBizContacts;
    }

    public static string ValueToName(ContactSource val)
    {
      return (string) ContactSourceEnumUtil._ValueToName[(object) val];
    }

    public static ContactSource NameToValue(string name)
    {
      return !ContactSourceEnumUtil._NameToValue.Contains((object) name) ? ContactSource.NotAvailable : (ContactSource) ContactSourceEnumUtil._NameToValue[(object) name];
    }

    public static ContactSource ValueStringToValue(string name)
    {
      return !ContactSourceEnumUtil._ValueStringToValue.Contains((object) name) ? ContactSource.NotAvailable : (ContactSource) ContactSourceEnumUtil._ValueStringToValue[(object) name];
    }

    public static bool ContainsName(string name)
    {
      return ContactSourceEnumUtil._NameToValue.Contains((object) name);
    }
  }
}
