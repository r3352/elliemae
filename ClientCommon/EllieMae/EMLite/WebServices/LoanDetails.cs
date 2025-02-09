// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.LoanDetails
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "4.0.30319.1")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://loancenter.elliemae.com/eVaultRetrieveService//")]
  [Serializable]
  public class LoanDetails
  {
    private string clientIDField;
    private string loanGUIDField;

    public string ClientID
    {
      get => this.clientIDField;
      set => this.clientIDField = value;
    }

    public string LoanGUID
    {
      get => this.loanGUIDField;
      set => this.loanGUIDField = value;
    }
  }
}
