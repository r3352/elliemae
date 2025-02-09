// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.PartialWebProxies.SenderInfo
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices.PartialWebProxies
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://loancenter.elliemae.com/ePackageWS/")]
  [Serializable]
  public class SenderInfo
  {
    private string clientidField;
    private string useridField;
    private string userPasswordField;

    public string clientid
    {
      get => this.clientidField;
      set => this.clientidField = value;
    }

    public string userid
    {
      get => this.useridField;
      set => this.useridField = value;
    }

    public string userPassword
    {
      get => this.userPasswordField;
      set => this.userPasswordField = value;
    }
  }
}
