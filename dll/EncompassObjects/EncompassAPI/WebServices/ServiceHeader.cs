// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.WebServices.ServiceHeader
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EncompassAPI.WebServices
{
  [GeneratedCode("System.Xml", "4.8.3752.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://hosted.elliemae.com/")]
  [XmlRoot(Namespace = "http://hosted.elliemae.com/", IsNullable = false)]
  [Serializable]
  public class ServiceHeader : SoapHeader
  {
    private string accessTokenField;
    private XmlAttribute[] anyAttrField;

    public string AccessToken
    {
      get => this.accessTokenField;
      set => this.accessTokenField = value;
    }

    [XmlAnyAttribute]
    public XmlAttribute[] AnyAttr
    {
      get => this.anyAttrField;
      set => this.anyAttrField = value;
    }
  }
}
