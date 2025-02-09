// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.FormIdentity
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [XmlType(Namespace = "http://loancenter.elliemae.com/eFolder")]
  public class FormIdentity
  {
    public string AttachmentId;
    public string TemplateID;
    public string Title;
    public string DocumentType;
    public string DocumentSource;
    public int Confidence;
  }
}
