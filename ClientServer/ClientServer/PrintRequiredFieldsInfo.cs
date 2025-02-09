// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PrintRequiredFieldsInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PrintRequiredFieldsInfo : IXmlSerializable
  {
    public static string PRINTBLANKID = "printblank";
    public static string ADVANCEDCODINGID = "advancedcodes";
    private string formID = string.Empty;
    private OutputFormType formType;
    private string[] fieldIDs;
    private string advancedCoding = string.Empty;

    public PrintRequiredFieldsInfo(
      string formID,
      OutputFormType formType,
      string[] fieldIDs,
      string advancedCoding)
    {
      this.formID = formID;
      this.formType = formType;
      this.fieldIDs = fieldIDs;
      this.advancedCoding = advancedCoding;
    }

    public PrintRequiredFieldsInfo(XmlSerializationInfo info)
    {
      this.formID = info.GetString(nameof (FormID));
      this.formType = info.GetEnum<OutputFormType>(nameof (FormType));
      this.fieldIDs = info.GetValue<XmlStringList>(nameof (FieldIDs)).ToArray();
      this.advancedCoding = info.GetString("AdvancedCode");
    }

    public string FormID => this.formID;

    public OutputFormType FormType => this.formType;

    public string[] FieldIDs => this.fieldIDs;

    public string AdvancedCoding => this.advancedCoding;

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("FormID", (object) this.formID);
      info.AddValue("FormType", (object) this.formType);
      info.AddValue("FieldIDs", (object) new XmlStringList(this.fieldIDs));
      info.AddValue("AdvancedCode", (object) this.advancedCoding);
    }
  }
}
