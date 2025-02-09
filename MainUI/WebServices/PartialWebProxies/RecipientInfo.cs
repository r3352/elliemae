// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.PartialWebProxies.RecipientInfo
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
  }
}
