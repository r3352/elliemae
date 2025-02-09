// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.IRS4506TFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class IRS4506TFields : FieldDataTemplate, IHtmlInput
  {
    public bool IsNew;
    public bool InEditMode;
    private static string[] YearsRequest4506FieldIDs = new string[4]
    {
      "IR0125",
      "IR0126",
      "IR0129",
      "IR0130"
    };
    private static string[] YearsRequest8821FieldIDs = new string[3]
    {
      "IR01A068",
      "IR01A072",
      "IR01A076"
    };
    public int TemplateID;
    public string LastModifiedBy;
    public DateTime LastModifiedDateTime;

    public IRS4506TFields() => this.IsNew = true;

    public IRS4506TFields(XmlSerializationInfo info)
      : base(info)
    {
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("TemplateID", (object) this.TemplateID);
      info.AddValue("LastModifiedBy", (object) this.LastModifiedBy);
      info.AddValue("LastModifiedDateTime", (object) this.LastModifiedDateTime);
    }

    public static explicit operator IRS4506TFields(BinaryObject obj)
    {
      return (IRS4506TFields) BinaryConvertibleObject.Parse(obj, typeof (IRS4506TFields));
    }

    public IRS4506TTemplate GetTemplate()
    {
      return new IRS4506TTemplate(this.ToXml())
      {
        TemplateID = this.TemplateID,
        TemplateName = this.TemplateName,
        RequestVersion = this.Version,
        RequestYears = this.YearsRequested,
        LastModifiedBy = this.LastModifiedBy,
        LastModifiedDateTime = this.LastModifiedDateTime
      };
    }

    public string Version => this.GetSimpleField("IR0193");

    public string ParticipantID => !(this.Version == "8821") ? this.GetSimpleField("IR0109") : "";

    public string YearsRequested
    {
      get
      {
        string yearsRequested = "";
        string[] strArray = this.Version == "8821" ? IRS4506TFields.YearsRequest8821FieldIDs : IRS4506TFields.YearsRequest4506FieldIDs;
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (!string.IsNullOrEmpty(this.GetSimpleField(strArray[index])))
            yearsRequested = yearsRequested + (!string.IsNullOrEmpty(yearsRequested) ? "," : "") + (this.Version == "8821" ? this.GetSimpleField(strArray[index]) : Utils.ParseDate((object) this.GetSimpleField(strArray[index])).Year.ToString());
        }
        return yearsRequested;
      }
    }

    public static string VersionTextConversion(string val, bool toUI)
    {
      FieldDefinition field = EncompassFields.GetField("IR0093");
      for (int index = 0; index < field.Options.Count; ++index)
      {
        if (toUI && string.Compare(field.Options[index].Value, val, true) == 0)
          return field.Options[index].Text;
        if (!toUI && string.Compare(field.Options[index].Text, val, true) == 0)
          return field.Options[index].Value;
      }
      return !toUI ? "4506-COct2022" : "4506-C Oct 2022";
    }
  }
}
