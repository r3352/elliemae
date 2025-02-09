// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomField
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class ContactCustomField : SessionBoundObject, IContactCustomField
  {
    private Contact contact;
    private string fieldValue;
    private ContactCustomFieldInfo fieldInfo;
    private StringList valueList;

    internal ContactCustomField(
      Contact contact,
      ContactCustomField field,
      ContactCustomFieldInfo fieldInfo)
      : base(contact.Session)
    {
      this.contact = contact;
      this.fieldInfo = fieldInfo;
      this.fieldValue = field != null ? field.FieldValue : "";
      if (fieldInfo.FieldOptions == null)
        return;
      this.valueList = (StringList) new ContactCustomField.ReadOnlyStringList(fieldInfo.FieldOptions);
    }

    public string Value
    {
      get => this.fieldValue;
      set
      {
        value = value ?? "";
        if (this.Type == CustomFieldType.DROPDOWN && value != "")
        {
          bool flag = false;
          for (int index = 0; index < this.PossibleValues.Count && !flag; ++index)
          {
            if (value == this.PossibleValues[index])
              flag = true;
          }
          if (!flag)
            throw new ArgumentException("Specified value is not in the range of possible values.");
        }
        string str = value;
        if (value != "")
        {
          bool flag = false;
          str = Utils.FormatInput(value, this.fieldInfo.FieldType, ref flag);
          if (str == "")
            throw new ArgumentException("Specified value cannot be converted to the required type");
        }
        this.fieldValue = str;
      }
    }

    public string Name => this.fieldInfo.Label;

    public CustomFieldType Type => (CustomFieldType) this.fieldInfo.FieldType;

    public StringList PossibleValues => this.valueList;

    internal ContactCustomField Unwrap()
    {
      return new ContactCustomField(this.contact.ID, this.fieldInfo.LabelID, this.Session.UserID, this.fieldValue);
    }

    private class ReadOnlyStringList : StringList
    {
      private bool readOnly;

      public ReadOnlyStringList(string[] list)
        : base((IList) list)
      {
        this.readOnly = true;
      }

      protected override void OnValidate(object value)
      {
        if (this.readOnly)
          throw new InvalidOperationException("Object is read only");
        base.OnValidate(value);
      }
    }
  }
}
