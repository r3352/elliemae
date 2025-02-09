// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.MailMergeJobParameters
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.JobService;
using EllieMae.EMLite.Serialization;
using System;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class MailMergeJobParameters : ConnectedJobParameters
  {
    public int[] ContactIDs;
    public ContactType ContactType;
    public string TemplatePath;
    public string Subject;
    public string[] EmailAddressOption;
    public string SenderUserID;

    public MailMergeJobParameters(string userId, string password, string serverUri)
      : base(userId, password, serverUri)
    {
    }

    public MailMergeJobParameters(XmlSerializationInfo info)
      : base(info)
    {
      string[] strArray = info.GetString("cids").Split(',');
      this.ContactIDs = new int[strArray.Length];
      for (int index = 0; index < strArray.Length; ++index)
        this.ContactIDs[index] = int.Parse(strArray[index]);
      this.ContactType = (ContactType) Enum.Parse(typeof (ContactType), info.GetString("type"), true);
      this.TemplatePath = info.GetString("template");
      this.Subject = info.GetString("subject");
      try
      {
        this.EmailAddressOption = info.GetString("emailAddressOption").Split(',');
        this.SenderUserID = info.GetString("senderUserID");
      }
      catch
      {
        this.EmailAddressOption = new string[0];
        this.SenderUserID = "";
      }
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      StringBuilder stringBuilder1 = new StringBuilder();
      for (int index = 0; index < this.ContactIDs.Length; ++index)
        stringBuilder1.Append(this.ContactIDs[index].ToString() + (index < this.ContactIDs.Length - 1 ? (object) "," : (object) ""));
      info.AddValue("cids", (object) stringBuilder1.ToString());
      info.AddValue("type", (object) this.ContactType.ToString());
      info.AddValue("template", (object) this.TemplatePath);
      info.AddValue("subject", (object) this.Subject);
      StringBuilder stringBuilder2 = new StringBuilder();
      for (int index = 0; index < this.EmailAddressOption.Length; ++index)
        stringBuilder2.Append(this.EmailAddressOption[index] + (index < this.EmailAddressOption.Length - 1 ? "," : ""));
      info.AddValue("emailAddressOption", (object) stringBuilder2.ToString());
      info.AddValue("senderUserID", (object) this.SenderUserID);
    }
  }
}
