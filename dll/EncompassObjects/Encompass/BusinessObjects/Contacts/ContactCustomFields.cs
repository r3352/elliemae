// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class ContactCustomFields : SessionBoundObject, IContactCustomFields, IEnumerable
  {
    private Contact contact;
    private BizCategory bizCategory;
    private Hashtable fieldMap = CollectionsUtil.CreateCaseInsensitiveHashtable();

    internal ContactCustomFields(Contact contact)
      : base(contact.Session)
    {
      this.contact = contact;
      this.Refresh();
    }

    internal ContactCustomFields(BizCategory bizCategory, Contact contact)
      : base(contact.Session)
    {
      this.bizCategory = bizCategory;
      this.contact = contact;
      this.Refresh();
    }

    public ContactCustomField this[string fieldName]
    {
      get => (ContactCustomField) this.fieldMap[(object) fieldName];
    }

    public int Count => this.fieldMap.Count;

    public IEnumerator GetEnumerator() => this.fieldMap.Values.GetEnumerator();

    internal void Commit()
    {
      ContactCustomField[] fieldList = new ContactCustomField[this.fieldMap.Count];
      int num = 0;
      foreach (ContactCustomField contactCustomField in (IEnumerable) this.fieldMap.Values)
        fieldList[num++] = contactCustomField.Unwrap();
      if (fieldList.Length == 0)
        return;
      this.commitCustomFields(fieldList);
    }

    internal void Refresh()
    {
      this.fieldMap.Clear();
      ContactCustomFieldInfoCollection fieldInfoCollection = this.getCustomFieldInfoCollection();
      ContactCustomField[] customFieldValues = this.getContactCustomFieldValues();
      Hashtable hashtable = new Hashtable(customFieldValues.Length);
      for (int index = 0; index < customFieldValues.Length; ++index)
        hashtable.Add((object) customFieldValues[index].FieldID, (object) customFieldValues[index]);
      for (int index = 0; index < fieldInfoCollection.Items.Length; ++index)
      {
        ContactCustomFieldInfo fieldInfo = fieldInfoCollection.Items[index];
        if (fieldInfo.Label != "")
        {
          ContactCustomField contactCustomField = new ContactCustomField(this.contact, (ContactCustomField) hashtable[(object) fieldInfo.LabelID], fieldInfo);
          this.fieldMap.Add((object) fieldInfo.Label, (object) contactCustomField);
        }
      }
    }

    private void commitCustomFields(ContactCustomField[] fieldList)
    {
      if ((EnumItem) this.bizCategory == (EnumItem) null)
      {
        this.Session.Contacts.ContactManager.UpdateCustomFieldsForContact(this.contact.ID, this.contact.ContactType, fieldList);
      }
      else
      {
        Hashtable hashtable = new Hashtable();
        foreach (CustomFieldValueInfo customFieldValue in this.Session.Contacts.ContactManager.GetCustomFieldsValues(this.contact.ID, this.bizCategory.ID).CustomFieldValues)
          hashtable[(object) customFieldValue.FieldId] = (object) customFieldValue;
        CustomFieldValueInfo[] customFieldValueInfoArray = new CustomFieldValueInfo[fieldList.Length];
        for (int index = 0; index < fieldList.Length; ++index)
        {
          CustomFieldValueInfo customFieldValueInfo = (CustomFieldValueInfo) hashtable[(object) fieldList[index].FieldID];
          if (customFieldValueInfo != null)
          {
            customFieldValueInfo.FieldValue = fieldList[index].FieldValue;
            customFieldValueInfo.IsDirty = true;
          }
          else
          {
            customFieldValueInfo = new CustomFieldValueInfo();
            customFieldValueInfo.ContactId = fieldList[index].ContactID;
            customFieldValueInfo.FieldId = fieldList[index].FieldID;
            customFieldValueInfo.FieldValue = fieldList[index].FieldValue;
            customFieldValueInfo.IsNew = true;
          }
          customFieldValueInfoArray[index] = customFieldValueInfo;
        }
        this.Session.Contacts.ContactManager.UpdateCustomFieldsValues(new CustomFieldsValueInfo()
        {
          ContactId = this.contact.ID,
          CategoryId = this.bizCategory.ID,
          CustomFieldValues = customFieldValueInfoArray
        });
      }
    }

    private ContactCustomField[] getContactCustomFieldValues()
    {
      if (this.contact.IsNew)
        return new ContactCustomField[0];
      if ((EnumItem) this.bizCategory == (EnumItem) null)
        return this.Session.Contacts.ContactManager.GetCustomFieldsForContact(this.contact.ID, this.contact.ContactType);
      CustomFieldsValueInfo customFieldsValues = this.Session.Contacts.ContactManager.GetCustomFieldsValues(this.contact.ID, this.bizCategory.ID);
      ContactCustomField[] customFieldValues = new ContactCustomField[customFieldsValues.CustomFieldValues.Length];
      for (int index = 0; index < customFieldsValues.CustomFieldValues.Length; ++index)
      {
        CustomFieldValueInfo customFieldValue = customFieldsValues.CustomFieldValues[index];
        customFieldValues[index] = new ContactCustomField(customFieldValue.ContactId, customFieldValue.FieldId, "", customFieldValue.FieldValue);
      }
      return customFieldValues;
    }

    private ContactCustomFieldInfoCollection getCustomFieldInfoCollection()
    {
      if ((EnumItem) this.bizCategory == (EnumItem) null)
        return this.Session.Contacts.GetCustomFieldDefinitions(this.contact.ContactType);
      CustomFieldsDefinitionInfo fieldsDefinition = this.Session.Contacts.ContactManager.GetCustomFieldsDefinition((CustomFieldsType) 12, this.bizCategory.ID);
      ContactCustomFieldInfo[] contactCustomFieldInfoArray = new ContactCustomFieldInfo[fieldsDefinition.CustomFieldDefinitions.Length];
      for (int index1 = 0; index1 < fieldsDefinition.CustomFieldDefinitions.Length; ++index1)
      {
        CustomFieldDefinitionInfo customFieldDefinition = fieldsDefinition.CustomFieldDefinitions[index1];
        string[] strArray = (string[]) null;
        if (customFieldDefinition.CustomFieldOptionDefinitions != null)
        {
          strArray = new string[customFieldDefinition.CustomFieldOptionDefinitions.Length];
          for (int index2 = 0; index2 < customFieldDefinition.CustomFieldOptionDefinitions.Length; ++index2)
            strArray[index2] = customFieldDefinition.CustomFieldOptionDefinitions[index2].OptionValue;
        }
        contactCustomFieldInfoArray[index1] = new ContactCustomFieldInfo(customFieldDefinition.FieldId, "", customFieldDefinition.FieldDescription, customFieldDefinition.FieldFormat, customFieldDefinition.LoanFieldId, customFieldDefinition.TwoWayTransfer, strArray);
      }
      return new ContactCustomFieldInfoCollection()
      {
        Items = contactCustomFieldInfoArray
      };
    }
  }
}
