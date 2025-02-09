// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.BothFileSystemPostgresMasterAttachmentXmlProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public sealed class BothFileSystemPostgresMasterAttachmentXmlProvider : 
    AttachmentXmlBase,
    IAttachmentXmlProvider,
    IAttachmentProviderBase
  {
    public XmlDocument GetAttachmentXml(Loan loan)
    {
      return new PostgresAttachmentXmlProvider().GetAttachmentXml(loan);
    }

    public void SaveAttachmentXml(Loan loan, XmlDocument xmlDoc)
    {
      Exception exception1 = (Exception) null;
      try
      {
        new PostgresAttachmentXmlProvider().SaveAttachmentXml(loan, xmlDoc);
      }
      catch (Exception ex)
      {
        exception1 = ex;
      }
      Exception exception2 = (Exception) null;
      try
      {
        new FileSystemAttachmentXmlProvider().SaveAttachmentXml(loan, xmlDoc);
      }
      catch (Exception ex)
      {
        exception2 = ex;
      }
      if (exception1 != null || exception2 != null)
        throw new Exception(exception1.ToString() + "\n" + (object) exception2);
    }
  }
}
