// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldDataTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public abstract class FieldDataTemplate : FormDataBase, ITemplateSetting
  {
    private string name = "";
    private string description = "";
    private string additionalInfo = "";
    private bool ignoreBusinessRules;

    protected FieldDataTemplate()
    {
    }

    protected FieldDataTemplate(DataRow[] arrDr)
      : base(arrDr)
    {
    }

    protected FieldDataTemplate(XmlSerializationInfo info)
      : base(info)
    {
      this.name = info.GetString(nameof (name), "");
      this.description = info.GetString("desc", "");
      this.additionalInfo = info.GetString(nameof (info), "");
      this.ignoreBusinessRules = info.GetBoolean("ignorebr", false);
      if (string.IsNullOrEmpty(this.name) && this.FieldData.ContainsKey("DTNAME"))
      {
        this.name = string.Concat(this.FieldData["DTNAME"]);
        this.FieldData.Remove("DTNAME");
      }
      if (string.IsNullOrEmpty(this.description) && this.FieldData.ContainsKey("DTDESC"))
      {
        this.description = string.Concat(this.FieldData["DTDESC"]);
        this.FieldData.Remove("DTDESC");
      }
      if (!this.FieldData.ContainsKey("IgnoreBusinessRule"))
        return;
      this.ignoreBusinessRules = string.Concat(this.FieldData["IgnoreBusinessRule"]) == "Y";
      this.FieldData.Remove("IgnoreBusinessRule");
    }

    public virtual string TemplateName
    {
      get => this.name;
      set
      {
        this.name = value;
        this.MarkAsDirty();
      }
    }

    public virtual string Description
    {
      get => this.description;
      set
      {
        this.description = value;
        this.MarkAsDirty();
      }
    }

    public virtual string AdditionalInfo
    {
      get => this.additionalInfo;
      set
      {
        this.additionalInfo = value;
        this.MarkAsDirty();
      }
    }

    public virtual bool IgnoreBusinessRules
    {
      get => this.ignoreBusinessRules;
      set
      {
        this.ignoreBusinessRules = value;
        this.MarkAsDirty();
      }
    }

    public virtual ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public virtual string ToLoanFieldID(string templateFieldID) => templateFieldID;

    public virtual bool AlwaysApply(string templateFieldID) => false;

    public virtual Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Description", (object) this.Description);
      insensitiveHashtable.Add((object) "AdditionalInfo", (object) this.AdditionalInfo);
      insensitiveHashtable.Add((object) "Name", (object) this.TemplateName);
      return insensitiveHashtable;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("name", (object) this.name);
      info.AddValue("desc", (object) this.description);
      info.AddValue(nameof (info), (object) this.additionalInfo);
      info.AddValue("ignorebr", (object) this.ignoreBusinessRules);
      base.GetXmlObjectData(info);
    }
  }
}
