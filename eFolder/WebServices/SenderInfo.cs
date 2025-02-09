// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.SenderInfo
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
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
