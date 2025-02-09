// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Provides access to the set of custome fields for a contact.
  /// </summary>
  /// <include file="ContactCustomFields.xml" path="Examples/Example[@name=&quot;ContactCustomFields.GetFieldByName&quot;]/*" />
  public class ContactCustomFields : SessionBoundObject, IContactCustomFields, IEnumerable
  {
    private Contact contact;
    private EllieMae.Encompass.BusinessEnums.BizCategory bizCategory;
    private Hashtable fieldMap = CollectionsUtil.CreateCaseInsensitiveHashtable();

    internal ContactCustomFields(Contact contact)
      : base(contact.Session)
    {
      this.contact = contact;
      this.Refresh();
    }

    internal ContactCustomFields(EllieMae.Encompass.BusinessEnums.BizCategory bizCategory, Contact contact)
      : base(contact.Session)
    {
      this.bizCategory = bizCategory;
      this.contact = contact;
      this.Refresh();
    }

    /// <summary>Retrieves a custom field using the field's name.</summary>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactCustomField" /> corresponding to this name, or <c>null</c>
    /// is no such custom field is defined.</returns>
    /// <example>
    /// The following code lists the favorite color of each Borrower Contact in the
    /// database assuming a custom field with the name <c>FavoriteColor</c> has been defined.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Retrieve all borrower contacts from the server
    ///       ContactList contacts = session.Contacts.GetAll(ContactType.Borrower);
    /// 
    ///       foreach (Contact contact in contacts)
    ///       {
    ///          // Write the contact's name and favorit color
    ///          Console.WriteLine(contact.FirstName + " " + contact.LastName + ": " +
    ///             contact.CustomFields["FavoriteColor"].Value);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ContactCustomField this[string fieldName]
    {
      get => (ContactCustomField) this.fieldMap[(object) fieldName];
    }

    /// <summary>
    /// Gets the number of custom fields defined for the current contact.
    /// </summary>
    public int Count => this.fieldMap.Count;

    /// <summary>
    /// Allows for enumeration over the custom fields defined for this contact.
    /// </summary>
    /// <returns>An enumerator for the set of custom fields.</returns>
    public IEnumerator GetEnumerator() => this.fieldMap.Values.GetEnumerator();

    internal void Commit()
    {
      EllieMae.EMLite.ClientServer.Contacts.ContactCustomField[] fieldList = new EllieMae.EMLite.ClientServer.Contacts.ContactCustomField[this.fieldMap.Count];
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
      EllieMae.EMLite.ClientServer.Contacts.ContactCustomField[] customFieldValues = this.getContactCustomFieldValues();
      Hashtable hashtable = new Hashtable(customFieldValues.Length);
      for (int index = 0; index < customFieldValues.Length; ++index)
        hashtable.Add((object) customFieldValues[index].FieldID, (object) customFieldValues[index]);
      for (int index = 0; index < fieldInfoCollection.Items.Length; ++index)
      {
        ContactCustomFieldInfo fieldInfo = fieldInfoCollection.Items[index];
        if (fieldInfo.Label != "")
        {
          ContactCustomField contactCustomField = new ContactCustomField(this.contact, (EllieMae.EMLite.ClientServer.Contacts.ContactCustomField) hashtable[(object) fieldInfo.LabelID], fieldInfo);
          this.fieldMap.Add((object) fieldInfo.Label, (object) contactCustomField);
        }
      }
    }

    private void commitCustomFields(EllieMae.EMLite.ClientServer.Contacts.ContactCustomField[] fieldList)
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

    private EllieMae.EMLite.ClientServer.Contacts.ContactCustomField[] getContactCustomFieldValues()
    {
      if (this.contact.IsNew)
        return new EllieMae.EMLite.ClientServer.Contacts.ContactCustomField[0];
      if ((EnumItem) this.bizCategory == (EnumItem) null)
        return this.Session.Contacts.ContactManager.GetCustomFieldsForContact(this.contact.ID, this.contact.ContactType);
      CustomFieldsValueInfo customFieldsValues = this.Session.Contacts.ContactManager.GetCustomFieldsValues(this.contact.ID, this.bizCategory.ID);
      EllieMae.EMLite.ClientServer.Contacts.ContactCustomField[] customFieldValues = new EllieMae.EMLite.ClientServer.Contacts.ContactCustomField[customFieldsValues.CustomFieldValues.Length];
      for (int index = 0; index < customFieldsValues.CustomFieldValues.Length; ++index)
      {
        CustomFieldValueInfo customFieldValue = customFieldsValues.CustomFieldValues[index];
        customFieldValues[index] = new EllieMae.EMLite.ClientServer.Contacts.ContactCustomField(customFieldValue.ContactId, customFieldValue.FieldId, "", customFieldValue.FieldValue);
      }
      return customFieldValues;
    }

    private ContactCustomFieldInfoCollection getCustomFieldInfoCollection()
    {
      if ((EnumItem) this.bizCategory == (EnumItem) null)
        return this.Session.Contacts.GetCustomFieldDefinitions(this.contact.ContactType);
      CustomFieldsDefinitionInfo fieldsDefinition = this.Session.Contacts.ContactManager.GetCustomFieldsDefinition(CustomFieldsType.BizCategoryCustom | CustomFieldsType.BizCategoryStandard, this.bizCategory.ID);
      ContactCustomFieldInfo[] contactCustomFieldInfoArray = new ContactCustomFieldInfo[fieldsDefinition.CustomFieldDefinitions.Length];
      for (int index1 = 0; index1 < fieldsDefinition.CustomFieldDefinitions.Length; ++index1)
      {
        CustomFieldDefinitionInfo customFieldDefinition = fieldsDefinition.CustomFieldDefinitions[index1];
        string[] fieldOptions = (string[]) null;
        if (customFieldDefinition.CustomFieldOptionDefinitions != null)
        {
          fieldOptions = new string[customFieldDefinition.CustomFieldOptionDefinitions.Length];
          for (int index2 = 0; index2 < customFieldDefinition.CustomFieldOptionDefinitions.Length; ++index2)
            fieldOptions[index2] = customFieldDefinition.CustomFieldOptionDefinitions[index2].OptionValue;
        }
        contactCustomFieldInfoArray[index1] = new ContactCustomFieldInfo(customFieldDefinition.FieldId, "", customFieldDefinition.FieldDescription, customFieldDefinition.FieldFormat, customFieldDefinition.LoanFieldId, customFieldDefinition.TwoWayTransfer, fieldOptions);
      }
      return new ContactCustomFieldInfoCollection()
      {
        Items = contactCustomFieldInfoArray
      };
    }
  }
}
