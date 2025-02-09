// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.IRS4506TTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class IRS4506TTemplate
  {
    public int TemplateID;
    public string TemplateName;
    public string RequestVersion;
    public string RequestYears;
    public string LastModifiedBy;
    public DateTime LastModifiedDateTime;
    public string XmlData;

    public IRS4506TTemplate(string xmlData) => this.XmlData = xmlData;

    public IRS4506TFields GetTemplateData()
    {
      IRS4506TFields templateData = (IRS4506TFields) new BinaryObject(this.XmlData, Encoding.Default);
      templateData.TemplateID = this.TemplateID;
      templateData.TemplateName = this.TemplateName;
      templateData.LastModifiedBy = this.LastModifiedBy;
      templateData.LastModifiedDateTime = this.LastModifiedDateTime;
      return templateData;
    }
  }
}
