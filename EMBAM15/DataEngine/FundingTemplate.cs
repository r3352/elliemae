// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FundingTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FundingTemplate : FieldDataTemplate
  {
    public static readonly string[] TemplateFields;
    private bool for2010gfe = true;
    private string respaVersion = "";

    static FundingTemplate()
    {
      ArrayList arrayList = new ArrayList();
      for (int index = 409; index <= 545; ++index)
        arrayList.Add((object) ("SYS.X" + index.ToString()));
      arrayList.AddRange((ICollection) new string[3]
      {
        "NEWHUD.X672",
        "NEWHUD.X716",
        "NEWHUD.X789"
      });
      for (int index = 1479; index <= 1524; ++index)
        arrayList.Add((object) ("NEWHUD.X" + index.ToString()));
      arrayList.AddRange((ICollection) new string[2]
      {
        "NEWHUD.X398",
        "NEWHUD.X790"
      });
      for (int index = 1231; index <= 1234; ++index)
        arrayList.Add((object) ("NEWHUD.X" + index.ToString()));
      for (int index = 1217; index <= 1224; ++index)
        arrayList.Add((object) ("NEWHUD.X" + index.ToString()));
      arrayList.AddRange((ICollection) new string[2]
      {
        "NEWHUD.X674",
        "NEWHUD.X681"
      });
      for (int index = 358; index <= 362; ++index)
        arrayList.Add((object) ("NEWHUD.X" + index.ToString()));
      for (int index = 378; index <= 382; ++index)
        arrayList.Add((object) ("NEWHUD.X" + index.ToString()));
      arrayList.AddRange((ICollection) new string[10]
      {
        "NEWHUD.X675",
        "NEWHUD.X676",
        "NEWHUD.X677",
        "NEWHUD.X1684",
        "NEWHUD.X1685",
        "NEWHUD.X682",
        "NEWHUD.X683",
        "NEWHUD.X684",
        "NEWHUD.X1694",
        "NEWHUD.X1695"
      });
      arrayList.AddRange((ICollection) new string[2]
      {
        "NEWHUD.X1712",
        "NEWHUD.X1715"
      });
      for (int index = 1033; index <= 1044; ++index)
        arrayList.Add((object) ("NEWHUD.X" + index.ToString()));
      arrayList.AddRange((ICollection) new string[14]
      {
        "NEWHUD.X369",
        "NEWHUD.X370",
        "NEWHUD.X371",
        "NEWHUD.X680",
        "NEWHUD.X372",
        "NEWHUD.X1686",
        "NEWHUD.X1687",
        "NEWHUD.X389",
        "NEWHUD.X390",
        "NEWHUD.X791",
        "NEWHUD.X685",
        "NEWHUD.X392",
        "NEWHUD.X1696",
        "NEWHUD.X1697"
      });
      arrayList.AddRange((ICollection) new string[6]
      {
        "NEWHUD.X373",
        "NEWHUD.X1688",
        "NEWHUD.X1689",
        "NEWHUD.X393",
        "NEWHUD.X1698",
        "NEWHUD.X1699"
      });
      arrayList.AddRange((ICollection) new string[6]
      {
        "NEWHUD.X375",
        "NEWHUD.X376",
        "NEWHUD.X377",
        "NEWHUD.X395",
        "NEWHUD.X396",
        "NEWHUD.X397"
      });
      for (int index = 1690; index <= 1693; ++index)
        arrayList.Add((object) ("NEWHUD.X" + index.ToString()));
      for (int index = 1700; index <= 1703; ++index)
        arrayList.Add((object) ("NEWHUD.X" + index.ToString()));
      for (int index = 141; index <= 164; ++index)
        arrayList.Add((object) ("NEWHUD2.X" + index.ToString()));
      for (int index = 4650; index <= 4659; ++index)
        arrayList.Add((object) ("NEWHUD2.X" + index.ToString()));
      FundingTemplate.TemplateFields = (string[]) arrayList.ToArray(typeof (string));
    }

    public FundingTemplate()
    {
    }

    public FundingTemplate(
      string name,
      string desc,
      bool for2010gfe,
      string respaVersion,
      DataRow[] dr)
      : base(dr)
    {
      this.TemplateName = name;
      this.Description = desc;
      this.For2010GFE = for2010gfe;
      this.respaVersion = respaVersion;
    }

    public FundingTemplate(XmlSerializationInfo info)
      : base(info)
    {
      try
      {
        this.respaVersion = info.GetString("RESPAVERSION");
      }
      catch (Exception ex)
      {
      }
      if ((this.respaVersion ?? "") == "")
      {
        this.for2010gfe = this.GetSimpleField("FOR2010") == "Y" || info.GetBoolean("GFE2010", false);
        this.respaVersion = this.for2010gfe ? "2010" : "2009";
      }
      else
        this.for2010gfe = !(this.respaVersion == "2015") && !(this.respaVersion == "2009");
      if (this.FieldData.ContainsKey("FOR2010"))
        this.FieldData.Remove("FOR2010");
      if (!(this.respaVersion != "2015"))
        return;
      for (int index = 141; index <= 164; ++index)
        this.RemoveField("NEWHUD2.X" + (object) index);
      for (int index = 4650; index <= 4659; ++index)
        this.RemoveField("NEWHUD2.X" + (object) index);
    }

    public bool For2010GFE
    {
      get => this.for2010gfe;
      set
      {
        this.for2010gfe = value;
        this.MarkAsDirty();
      }
    }

    public override string[] GetAllowedFieldIDs() => FundingTemplate.TemplateFields;

    public string RESPAVersion
    {
      get => this.respaVersion;
      set
      {
        this.respaVersion = value;
        this.MarkAsDirty();
      }
    }

    public override Hashtable GetProperties()
    {
      Hashtable properties = base.GetProperties();
      properties[(object) "For2010GFE"] = this.For2010GFE ? (object) "Yes" : (object) "No";
      properties[(object) "RESPAVERSION"] = (object) this.respaVersion;
      return properties;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("GFE2010", (object) this.for2010gfe);
      info.AddValue("RESPAVERSION", (object) this.respaVersion);
    }

    public static explicit operator FundingTemplate(BinaryObject obj)
    {
      return (FundingTemplate) BinaryConvertibleObject.Parse(obj, typeof (FundingTemplate));
    }
  }
}
