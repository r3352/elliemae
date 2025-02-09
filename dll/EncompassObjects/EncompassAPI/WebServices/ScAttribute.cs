// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.WebServices.ScAttribute
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EncompassAPI.WebServices
{
  [GeneratedCode("System.Xml", "4.8.3752.0")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [XmlType(Namespace = "http://hosted.elliemae.com/")]
  [Serializable]
  public class ScAttribute
  {
    private string userIdField;
    private string appSuiteNameField;
    private string appNameField;
    private string attrNameField;
    private string attrValueField;
    private string commentField;
    private string lastModifiedByField;
    private string lastModifiedDateField;
    private string createdByField;
    private string createdDateField;

    public ScAttribute()
    {
      this.appSuiteNameField = "*";
      this.appNameField = "*";
      this.attrValueField = "";
      this.lastModifiedByField = "SmartClientSvc";
    }

    public string UserId
    {
      get => this.userIdField;
      set => this.userIdField = value;
    }

    [DefaultValue("*")]
    public string AppSuiteName
    {
      get => this.appSuiteNameField;
      set => this.appSuiteNameField = value;
    }

    [DefaultValue("*")]
    public string AppName
    {
      get => this.appNameField;
      set => this.appNameField = value;
    }

    public string AttrName
    {
      get => this.attrNameField;
      set => this.attrNameField = value;
    }

    [DefaultValue("")]
    public string AttrValue
    {
      get => this.attrValueField;
      set => this.attrValueField = value;
    }

    public string Comment
    {
      get => this.commentField;
      set => this.commentField = value;
    }

    [DefaultValue("SmartClientSvc")]
    public string LastModifiedBy
    {
      get => this.lastModifiedByField;
      set => this.lastModifiedByField = value;
    }

    public string LastModifiedDate
    {
      get => this.lastModifiedDateField;
      set => this.lastModifiedDateField = value;
    }

    public string CreatedBy
    {
      get => this.createdByField;
      set => this.createdByField = value;
    }

    public string CreatedDate
    {
      get => this.createdDateField;
      set => this.createdDateField = value;
    }
  }
}
