// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BizPartnerCustomFields
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
  public sealed class BizPartnerCustomFields
  {
    private const string className = "BizPartnerCustomFields�";
    private const string xmlFileRelativePath = "Users\\BizCustomFields.xml�";

    public static ContactCustomFieldInfoCollection Get()
    {
      return ContactAccessor.GetBusinessCustomFields();
    }

    public static void Set(ContactCustomFieldInfoCollection newValue)
    {
      using (ClientContext.GetCurrent().Cache.Lock(nameof (BizPartnerCustomFields)))
      {
        ContactAccessor.SetBusinessCustomFields(newValue);
        FieldSearchRule fieldSearchRule = new FieldSearchRule(newValue, FieldSearchRuleType.BusinessCustomFields);
        string identifier = "Page Fields";
        int parentFsRuleId = FieldSearchDbAccessor.FindParentFSRuleId(0, FieldSearchRuleType.BusinessCustomFields, identifier);
        if (parentFsRuleId != 0)
          fieldSearchRule.ParentFSRuleId = new int?(parentFsRuleId);
        fieldSearchRule.Identifier = identifier;
        if (fieldSearchRule.FieldSearchFields.Count > 0)
          FieldSearchDbAccessor.UpdateFieldSearchInfo(fieldSearchRule);
        else
          FieldSearchDbAccessor.DeleteFieldSearchInfoWhithDataCheck(0, FieldSearchRuleType.BusinessCustomFields, identifier);
      }
    }

    public static ContactCustomFieldInfoCollection DeserializeResource(ClientContext context)
    {
      ContactCustomFieldInfoCollection o = (ContactCustomFieldInfoCollection) context.Cache.Get(nameof (BizPartnerCustomFields));
      if (o != null)
        return o;
      using (context.Cache.Lock(nameof (BizPartnerCustomFields)))
      {
        o = (ContactCustomFieldInfoCollection) XmlDataStore.Deserialize(typeof (ContactCustomFieldInfoCollection), BizPartnerCustomFields.getFilePath(context));
        if (o.Items == null)
          o.Items = new ContactCustomFieldInfo[0];
        context.Cache.Put(nameof (BizPartnerCustomFields), (object) o, CacheSetting.Low);
      }
      return o;
    }

    private static string getFilePath(ClientContext context)
    {
      return context.Settings.GetDataFilePath("Users\\BizCustomFields.xml");
    }
  }
}
