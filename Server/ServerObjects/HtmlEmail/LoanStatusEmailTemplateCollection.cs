// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.HtmlEmail.LoanStatusEmailTemplateCollection
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.HtmlEmail
{
  internal class LoanStatusEmailTemplateCollection : IXmlSerializable
  {
    private LoanStatusEmailTemplate[] templateList;

    public LoanStatusEmailTemplateCollection()
    {
      this.templateList = new LoanStatusEmailTemplate[0];
    }

    public LoanStatusEmailTemplateCollection(XmlSerializationInfo info)
    {
      this.templateList = (LoanStatusEmailTemplate[]) info.GetValue("Templates", typeof (LoanStatusEmailTemplate[]));
      if (this.templateList != null)
        return;
      this.templateList = new LoanStatusEmailTemplate[0];
    }

    public HtmlEmailTemplateCollection MigrateData(string ownerID)
    {
      HtmlEmailTemplateCollection templateCollection = new HtmlEmailTemplateCollection();
      foreach (LoanStatusEmailTemplate template1 in this.templateList)
      {
        HtmlEmailTemplate template2 = template1.MigrateData(ownerID);
        templateCollection.Add(template2);
      }
      return templateCollection;
    }

    public void GetXmlObjectData(XmlSerializationInfo info) => throw new NotSupportedException();
  }
}
