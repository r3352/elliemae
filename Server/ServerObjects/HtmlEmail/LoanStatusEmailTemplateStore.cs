// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.HtmlEmail.LoanStatusEmailTemplateStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.HtmlEmail
{
  internal class LoanStatusEmailTemplateStore
  {
    public static LoanStatusEmailTemplateCollection Get(string userID)
    {
      return (LoanStatusEmailTemplateCollection) XmlDataStore.Deserialize(typeof (LoanStatusEmailTemplateCollection), LoanStatusEmailTemplateStore.getXmlFilePath(userID));
    }

    private static string getXmlFilePath(string userID)
    {
      ClientContext current = ClientContext.GetCurrent();
      return string.IsNullOrEmpty(userID) ? current.Settings.GetDataFilePath("LoanStatusEmailTemplates.xml") : current.Settings.GetUserDataFilePath(userID, "LoanStatusEmailTemplates.xml");
    }
  }
}
