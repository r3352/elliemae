// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.DataTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [CLSCompliant(true)]
  [Serializable]
  public class DataTemplate : FieldDataTemplate
  {
    private XmlStringTable dtVersion;
    private XmlStringTable ddmdtVersion;
    private bool usedForGeneralDataInput;
    private bool readOnly;
    private string respaVersion = "";

    public DataTemplate()
    {
    }

    public DataTemplate(XmlSerializationInfo info)
      : base(info)
    {
      try
      {
        this.dtVersion = (XmlStringTable) info.GetValue("version", typeof (XmlStringTable));
      }
      catch (Exception ex)
      {
      }
      try
      {
        this.ddmdtVersion = (XmlStringTable) info.GetValue("ddmdtversion", typeof (XmlStringTable));
      }
      catch (Exception ex)
      {
      }
      if (this.dtVersion == null || !this.dtVersion.ContainsKey("2.3.0"))
      {
        for (int index = 252; index <= 292; index += 2)
          this.dataMigrationForPTB("SYS.X" + index.ToString());
        this.dataMigrationForPTB("SYS.X297");
        for (int index = 302; index <= 366; index += 2)
          this.dataMigrationForPTB("SYS.X" + index.ToString());
        for (int index = 371; index <= 387; index += 2)
          this.dataMigrationForPTB("SYS.X" + index.ToString());
        this.dataMigrationForPTB("SYS.X392");
        this.dataMigrationForPTB("SYS.X404");
        this.dataMigrationForPTB("SYS.X406");
        this.dataMigrationForPTB("SYS.X408");
        this.RemoveField("CCVersion");
      }
      try
      {
        this.respaVersion = info.GetString("RESPAVERSION");
      }
      catch (Exception ex)
      {
      }
    }

    public string DdmDtVersion
    {
      get
      {
        return this.ddmdtVersion != null && this.ddmdtVersion.ContainsKey("18.2.0") ? "18.2.0" : "0.0.0";
      }
    }

    public override FormDataBase.TemplateType GetTemplateType()
    {
      return FormDataBase.TemplateType.DataTemplate;
    }

    public override string GetDataTemplateVersion(XmlSerializationInfo info)
    {
      if (this.ddmdtVersion != null && this.ddmdtVersion.ContainsKey("18.2.0"))
        return "18.2.0";
      try
      {
        this.ddmdtVersion = (XmlStringTable) info.GetValue("ddmdtversion", typeof (XmlStringTable));
      }
      catch (Exception ex)
      {
      }
      return this.ddmdtVersion != null && this.ddmdtVersion.ContainsKey("18.2.0") ? "18.2.0" : "0.0.0";
    }

    private void dataMigrationForPTB(string id)
    {
      string simpleField = this.GetSimpleField(id);
      if (string.Compare(simpleField, "Y", true) == 0)
      {
        this.SetCurrentField(id, "Broker");
      }
      else
      {
        if (string.Compare(simpleField, "N", true) != 0)
          return;
        this.SetCurrentField(id, "");
      }
    }

    public static explicit operator DataTemplate(BinaryObject obj)
    {
      return (DataTemplate) BinaryConvertibleObject.Parse(obj, typeof (DataTemplate));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      if (this.dtVersion == null || !this.dtVersion.ContainsKey("2.3.0"))
      {
        if (this.dtVersion == null)
          this.dtVersion = new XmlStringTable();
        this.dtVersion.Add("2.3.0", (object) "");
        info.AddValue("version", (object) this.dtVersion);
      }
      else
        info.AddValue("version", (object) this.dtVersion);
      if (this.ddmdtVersion == null || !this.ddmdtVersion.ContainsKey("18.2.0"))
      {
        if (this.ddmdtVersion == null)
          this.ddmdtVersion = new XmlStringTable();
        this.ddmdtVersion.Add("18.2.0", (object) "");
        info.AddValue("ddmdtversion", (object) this.ddmdtVersion);
      }
      else
        info.AddValue("ddmdtversion", (object) this.ddmdtVersion);
      info.AddValue("RESPAVERSION", (object) this.respaVersion);
    }

    public override Hashtable GetProperties()
    {
      Hashtable properties = base.GetProperties();
      properties[(object) "RESPAVERSION"] = (object) this.respaVersion;
      return properties;
    }

    public bool UsedForGeneralDataInput
    {
      get => this.usedForGeneralDataInput;
      set => this.usedForGeneralDataInput = value;
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set => this.readOnly = value;
    }

    public string RESPAVersion
    {
      get => this.respaVersion;
      set
      {
        this.respaVersion = value;
        if (this.respaVersion == "2009")
          this.SetCurrentField("3969", "Old GFE and HUD-1");
        else if (this.respaVersion == "2010")
          this.SetCurrentField("3969", "RESPA 2010 GFE and HUD-1");
        else
          this.SetCurrentField("3969", "RESPA-TILA 2015 LE and CD");
      }
    }
  }
}
