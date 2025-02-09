// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanDuplicationTemplate
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class LoanDuplicationTemplate : FieldDataTemplate
  {
    private string[] additionalFields;

    public LoanDuplicationTemplate()
    {
    }

    public LoanDuplicationTemplate(XmlSerializationInfo info)
      : base(info)
    {
      XmlList<string> xmlList = (XmlList<string>) info.GetValue("AdditionalFields", typeof (XmlList<string>), (object) null);
      if (xmlList == null)
        return;
      this.additionalFields = xmlList.ToArray();
    }

    public void SetAdditionalFields(string[] fieldIDs) => this.additionalFields = fieldIDs;

    public string[] GetAdditionalFields() => this.additionalFields;

    public static explicit operator LoanDuplicationTemplate(BinaryObject obj)
    {
      return (LoanDuplicationTemplate) BinaryConvertibleObject.Parse(obj, typeof (LoanDuplicationTemplate));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      if (this.additionalFields != null)
        info.AddValue("AdditionalFields", (object) new XmlList<string>((IEnumerable<string>) this.additionalFields));
      else
        info.AddValue("AdditionalFields", (object) null);
    }
  }
}
