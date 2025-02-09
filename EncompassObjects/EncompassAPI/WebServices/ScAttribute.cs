// Decompiled with JetBrains decompiler
// Type: EllieMae.EncompassAPI.WebServices.ScAttribute
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EncompassAPI.WebServices
{
  /// <remarks />
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

    /// <remarks />
    public string UserId
    {
      get => this.userIdField;
      set => this.userIdField = value;
    }

    /// <remarks />
    [DefaultValue("*")]
    public string AppSuiteName
    {
      get => this.appSuiteNameField;
      set => this.appSuiteNameField = value;
    }

    /// <remarks />
    [DefaultValue("*")]
    public string AppName
    {
      get => this.appNameField;
      set => this.appNameField = value;
    }

    /// <remarks />
    public string AttrName
    {
      get => this.attrNameField;
      set => this.attrNameField = value;
    }

    /// <remarks />
    [DefaultValue("")]
    public string AttrValue
    {
      get => this.attrValueField;
      set => this.attrValueField = value;
    }

    /// <remarks />
    public string Comment
    {
      get => this.commentField;
      set => this.commentField = value;
    }

    /// <remarks />
    [DefaultValue("SmartClientSvc")]
    public string LastModifiedBy
    {
      get => this.lastModifiedByField;
      set => this.lastModifiedByField = value;
    }

    /// <remarks />
    public string LastModifiedDate
    {
      get => this.lastModifiedDateField;
      set => this.lastModifiedDateField = value;
    }

    /// <remarks />
    public string CreatedBy
    {
      get => this.createdByField;
      set => this.createdByField = value;
    }

    /// <remarks />
    public string CreatedDate
    {
      get => this.createdDateField;
      set => this.createdDateField = value;
    }
  }
}
