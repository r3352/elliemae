// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.RecipientInfo
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
  public class RecipientInfo
  {
    private string packageGUIDField;
    private string triggerGUIDField;
    private string fromField;
    private string fromNameField;
    private string toField;
    private string subjectField;
    private string bodyField;
    private bool useHTMLBodyField;
    private bool isTPOConnectLoanField;

    public string packageGUID
    {
      get => this.packageGUIDField;
      set => this.packageGUIDField = value;
    }

    public string triggerGUID
    {
      get => this.triggerGUIDField;
      set => this.triggerGUIDField = value;
    }

    public string from
    {
      get => this.fromField;
      set => this.fromField = value;
    }

    public string fromName
    {
      get => this.fromNameField;
      set => this.fromNameField = value;
    }

    public string to
    {
      get => this.toField;
      set => this.toField = value;
    }

    public string subject
    {
      get => this.subjectField;
      set => this.subjectField = value;
    }

    public string body
    {
      get => this.bodyField;
      set => this.bodyField = value;
    }

    public bool useHTMLBody
    {
      get => this.useHTMLBodyField;
      set => this.useHTMLBodyField = value;
    }

    public bool isTPOConnectLoan
    {
      get => this.isTPOConnectLoanField;
      set => this.isTPOConnectLoanField = value;
    }
  }
}
