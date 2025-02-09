// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.ServiceHeader
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.Common
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
