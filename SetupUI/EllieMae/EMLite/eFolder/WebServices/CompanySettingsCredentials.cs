// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.WebServices.CompanySettingsCredentials
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.eFolder.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://loancenter.elliemae.com/eFolder")]
  [XmlRoot(Namespace = "http://loancenter.elliemae.com/eFolder", IsNullable = false)]
  [Serializable]
  public class CompanySettingsCredentials : SoapHeader
  {
    private string clientIDField;
    private string userIDField;
    private string passwordField;
    private XmlAttribute[] anyAttrField;

    public string ClientID
    {
      get => this.clientIDField;
      set => this.clientIDField = value;
    }

    public string UserID
    {
      get => this.userIDField;
      set => this.userIDField = value;
    }

    public string Password
    {
      get => this.passwordField;
      set => this.passwordField = value;
    }

    [XmlAnyAttribute]
    public XmlAttribute[] AnyAttr
    {
      get => this.anyAttrField;
      set => this.anyAttrField = value;
    }
  }
}
