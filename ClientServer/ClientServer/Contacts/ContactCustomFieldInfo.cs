// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.ContactCustomFieldInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class ContactCustomFieldInfo : IComparable, IXmlSerializable
  {
    public string[] FieldOptions = new string[0];
    public int LabelID = -1;
    public string OwnerID = string.Empty;
    public string Label = string.Empty;
    public FieldFormat FieldType;
    public string LoanFieldId = string.Empty;
    public bool TwoWayTransfer;

    public ContactCustomFieldInfo()
    {
    }

    public ContactCustomFieldInfo(
      int labelId,
      string ownerId,
      string label,
      FieldFormat fieldType,
      string loanFieldId,
      bool twoWayTransfer,
      string[] fieldOptions)
    {
      this.LabelID = labelId;
      this.OwnerID = ownerId;
      this.Label = label;
      this.FieldType = fieldType;
      this.LoanFieldId = loanFieldId;
      this.TwoWayTransfer = twoWayTransfer;
      this.FieldOptions = fieldOptions;
    }

    public ContactCustomFieldInfo(XmlSerializationInfo info)
    {
      this.LabelID = info.GetInteger(nameof (LabelID));
      this.OwnerID = info.GetString(nameof (OwnerID));
      this.Label = info.GetString(nameof (Label));
      this.FieldType = (FieldFormat) info.GetValue(nameof (FieldType), typeof (FieldFormat));
      try
      {
        this.LoanFieldId = info.GetString(nameof (LoanFieldId));
        this.TwoWayTransfer = info.GetBoolean(nameof (TwoWayTransfer));
      }
      catch (ApplicationException ex)
      {
        this.LoanFieldId = string.Empty;
        this.TwoWayTransfer = false;
      }
      this.FieldOptions = (string[]) info.GetValue(nameof (FieldOptions), typeof (string[]));
    }

    public override int GetHashCode() => this.LabelID.GetHashCode();

    public static bool operator ==(ContactCustomFieldInfo o1, ContactCustomFieldInfo o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(ContactCustomFieldInfo o1, ContactCustomFieldInfo o2)
    {
      return !(o1 == o2);
    }

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && object.Equals((object) this.LabelID, (object) ((ContactCustomFieldInfo) obj).LabelID);
    }

    public int CompareTo(object obj)
    {
      ContactCustomFieldInfo contactCustomFieldInfo = (ContactCustomFieldInfo) obj;
      if (this.LabelID > contactCustomFieldInfo.LabelID)
        return 1;
      return this.LabelID < contactCustomFieldInfo.LabelID ? -1 : 0;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("LabelID", (object) this.LabelID);
      info.AddValue("OwnerID", (object) this.OwnerID);
      info.AddValue("Label", (object) this.Label);
      info.AddValue("FieldType", (object) this.FieldType);
      info.AddValue("LoanFieldId", (object) this.LoanFieldId);
      info.AddValue("TwoWayTransfer", (object) this.TwoWayTransfer);
      info.AddValue("FieldOptions", (object) this.FieldOptions);
    }
  }
}
