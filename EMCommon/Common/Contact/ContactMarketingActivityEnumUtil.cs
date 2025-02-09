// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.ContactMarketingActivityEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class ContactMarketingActivityEnumUtil
  {
    private static Hashtable _NameToValue = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable _ValueToName;

    static ContactMarketingActivityEnumUtil()
    {
      ContactMarketingActivityEnumUtil._NameToValue.Add((object) "Mail Merge", (object) ContactMarketingActivity.MailMerge);
      ContactMarketingActivityEnumUtil._NameToValue.Add((object) "Email Merge", (object) ContactMarketingActivity.EmailMerge);
      ContactMarketingActivityEnumUtil._NameToValue.Add((object) "Call", (object) ContactMarketingActivity.Call);
      ContactMarketingActivityEnumUtil._NameToValue.Add((object) "Email", (object) ContactMarketingActivity.Email);
      ContactMarketingActivityEnumUtil._NameToValue.Add((object) "Fax", (object) ContactMarketingActivity.Fax);
      ContactMarketingActivityEnumUtil._NameToValue.Add((object) "First Contact", (object) ContactMarketingActivity.FirstContact);
      ContactMarketingActivityEnumUtil._ValueToName = new Hashtable();
      ContactMarketingActivityEnumUtil._ValueToName.Add((object) ContactMarketingActivity.MailMerge, (object) "Mail Merge");
      ContactMarketingActivityEnumUtil._ValueToName.Add((object) ContactMarketingActivity.EmailMerge, (object) "Email Merge");
      ContactMarketingActivityEnumUtil._ValueToName.Add((object) ContactMarketingActivity.Call, (object) "Call");
      ContactMarketingActivityEnumUtil._ValueToName.Add((object) ContactMarketingActivity.Email, (object) "Email");
      ContactMarketingActivityEnumUtil._ValueToName.Add((object) ContactMarketingActivity.Fax, (object) "Fax");
      ContactMarketingActivityEnumUtil._ValueToName.Add((object) ContactMarketingActivity.FirstContact, (object) "First Contact");
    }

    public static object[] GetDisplayNames()
    {
      ContactMarketingActivity[] values = (ContactMarketingActivity[]) Enum.GetValues(typeof (ContactMarketingActivity));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) ContactMarketingActivityEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string ValueToName(ContactMarketingActivity val)
    {
      return (string) ContactMarketingActivityEnumUtil._ValueToName[(object) val];
    }

    public static ContactMarketingActivity NameToValue(string name)
    {
      return (ContactMarketingActivity) ContactMarketingActivityEnumUtil._NameToValue[(object) name];
    }

    public static bool ContainsName(string name)
    {
      return ContactMarketingActivityEnumUtil._NameToValue.Contains((object) name);
    }
  }
}
