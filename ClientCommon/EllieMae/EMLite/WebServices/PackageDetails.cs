// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.PackageDetails
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
  public class PackageDetails
  {
    private int packageIDField;
    private string packageGUIDField;
    private string senderField;
    private DateTime signedDateField;

    public int PackageID
    {
      get => this.packageIDField;
      set => this.packageIDField = value;
    }

    public string PackageGUID
    {
      get => this.packageGUIDField;
      set => this.packageGUIDField = value;
    }

    public string Sender
    {
      get => this.senderField;
      set => this.senderField = value;
    }

    public DateTime SignedDate
    {
      get => this.signedDateField;
      set => this.signedDateField = value;
    }
  }
}
