// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.ConditionSetTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class ConditionSetTemplate : BinaryConvertibleObject, ITemplateSetting
  {
    private string name = "";
    private string description = "";
    private ConditionType conditionType;
    private XmlStringList conditionIds;

    public ConditionSetTemplate(ConditionType conditionType)
    {
      this.conditionType = conditionType;
      this.conditionIds = new XmlStringList();
    }

    public ConditionSetTemplate(XmlSerializationInfo info)
    {
      this.name = info.GetString(nameof (name));
      this.description = info.GetString("desc");
      this.conditionIds = (XmlStringList) info.GetValue("conditions", typeof (XmlStringList));
      try
      {
        this.conditionType = (ConditionType) info.GetValue("type", typeof (ConditionType));
      }
      catch
      {
        this.conditionType = ConditionType.Underwriting;
      }
    }

    public string TemplateName
    {
      get => this.name;
      set => this.name = value;
    }

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public ConditionType ConditionType => this.conditionType;

    public ArrayList Conditions => (ArrayList) this.conditionIds;

    public ITemplateSetting Duplicate()
    {
      ITemplateSetting templateSetting = (ITemplateSetting) this.Clone();
      templateSetting.TemplateName = "";
      return templateSetting;
    }

    public Hashtable GetProperties()
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      insensitiveHashtable.Add((object) "Description", (object) this.Description);
      return insensitiveHashtable;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("name", (object) this.name);
      info.AddValue("type", (object) this.conditionType);
      info.AddValue("desc", (object) this.description);
      info.AddValue("conditions", (object) this.conditionIds);
    }

    public static explicit operator ConditionSetTemplate(BinaryObject obj)
    {
      return (ConditionSetTemplate) BinaryConvertibleObject.Parse(obj, typeof (ConditionSetTemplate));
    }
  }
}
