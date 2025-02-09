// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.PostgresAttachmentXmlProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataAccess;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public sealed class PostgresAttachmentXmlProvider : 
    AttachmentXmlBase,
    IAttachmentXmlProvider,
    IAttachmentProviderBase
  {
    public XmlDocument GetAttachmentXml(Loan loan)
    {
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      Func<XmlDocument, FileAttachment[]> deserializer = (Func<XmlDocument, FileAttachment[]>) (x => FileAttachmentStore.BuildAttachments(x, true));
      FileAttachment[] fileAttachmentArray = (FileAttachment[]) null;
      using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
        fileAttachmentArray = service.GetLoanAttachments(new Guid(loan.Identity.Guid), deserializer);
      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.LoadXml("<Attachments/>");
      if (fileAttachmentArray != null)
      {
        foreach (FileAttachment attachment in fileAttachmentArray)
          FileAttachmentStore.writeAttachment(xmlDoc, attachment);
      }
      return xmlDoc;
    }

    public void SaveAttachmentXml(Loan loan, XmlDocument xmlDoc)
    {
      IServerSettings settings = ClientContext.GetCurrent().Settings;
      FileAttachment[] attachments = FileAttachmentStore.BuildAttachments(xmlDoc, true);
      using (ILoanDataAccessor service = DataAccessFramework.Runtime.CreateService<ILoanDataAccessor>())
        service.SaveLoanAttachments(new Guid(loan.Identity.Guid), attachments);
    }
  }
}
