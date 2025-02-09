// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.LoanCenterDocument
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
  public class LoanCenterDocument
  {
    private int packageEntityIDField;
    private string packageEntityGUIDField;
    private string documentGUIDField;
    private string titleField;
    private LoanDetails loanDetailsField;
    private PackageDetails packageDetailsField;

    public int PackageEntityID
    {
      get => this.packageEntityIDField;
      set => this.packageEntityIDField = value;
    }

    public string PackageEntityGUID
    {
      get => this.packageEntityGUIDField;
      set => this.packageEntityGUIDField = value;
    }

    public string DocumentGUID
    {
      get => this.documentGUIDField;
      set => this.documentGUIDField = value;
    }

    public string Title
    {
      get => this.titleField;
      set => this.titleField = value;
    }

    public LoanDetails loanDetails
    {
      get => this.loanDetailsField;
      set => this.loanDetailsField = value;
    }

    public PackageDetails packageDetails
    {
      get => this.packageDetailsField;
      set => this.packageDetailsField = value;
    }
  }
}
