// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BorrowerCustomFields
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Server.ServerObjects.Bpm;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class BorrowerCustomFields
  {
    private const string className = "BorrowerCustomFields�";
    private const string xmlFileRelativePath = "Users\\BorCustomFields.xml�";

    public static ContactCustomFieldInfoCollection Get()
    {
      return ContactAccessor.GetBorrowerCustomFields();
    }

    public static void Set(ContactCustomFieldInfoCollection newValue)
    {
      using (ClientContext.GetCurrent().Cache.Lock(nameof (BorrowerCustomFields)))
      {
        ContactAccessor.SetBorrowerCustomFields(newValue);
        FieldSearchRule fieldSearchRule = new FieldSearchRule(newValue, FieldSearchRuleType.BorrowerCustomFields);
        if (fieldSearchRule.FieldSearchFields.Count > 0)
          FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule);
        else
          FieldSearchDbAccessor.DeleteFieldSearchInfo(0, FieldSearchRuleType.BorrowerCustomFields);
      }
    }

    public static ContactCustomFieldInfoCollection DeserializeResource(ClientContext context)
    {
      ContactCustomFieldInfoCollection o = (ContactCustomFieldInfoCollection) context.Cache.Get(nameof (BorrowerCustomFields));
      if (o != null)
        return o;
      using (context.Cache.Lock(nameof (BorrowerCustomFields)))
      {
        o = (ContactCustomFieldInfoCollection) XmlDataStore.Deserialize(typeof (ContactCustomFieldInfoCollection), BorrowerCustomFields.getFilePath(context));
        if (o.Items == null)
          o.Items = new ContactCustomFieldInfo[0];
        context.Cache.Put(nameof (BorrowerCustomFields), (object) o, CacheSetting.Low);
      }
      return o;
    }

    private static string getFilePath(ClientContext context)
    {
      return context.Settings.GetDataFilePath("Users\\BorCustomFields.xml");
    }
  }
}
