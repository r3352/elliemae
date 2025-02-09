// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class CustomFields : IEnumerable<CustomField>, IEnumerable
  {
    private List<CustomField> customFields = new List<CustomField>();
    private List<ContactCustomField> fieldValues;
    private ContactCustomFieldInfoCollection fieldInfos;
    private IConfigurationManager mngr;
    private ExternalOriginatorManagementData extOrg;

    private int GetTopID()
    {
      ExternalOriginatorManagementData originatorManagementData = this.extOrg;
      while (originatorManagementData.Parent != 0 && originatorManagementData.InheritCustomFields)
        originatorManagementData = this.mngr.GetExternalOrganization(false, originatorManagementData.Parent);
      return originatorManagementData.oid;
    }

    internal CustomFields(IConfigurationManager mngr, ExternalOriginatorManagementData extOrg)
    {
      this.mngr = mngr;
      this.extOrg = extOrg;
      this.fieldInfos = this.mngr.GetCustomFieldInfo();
      this.fieldValues = ((IEnumerable<ContactCustomField>) this.mngr.GetCustomFieldValues(this.GetTopID())).ToList<ContactCustomField>();
      if (extOrg.Parent == 0 && extOrg.InheritCustomFields)
        extOrg.InheritCustomFields = false;
      List<ContactCustomField> contactCustomFieldList = new List<ContactCustomField>();
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in this.fieldInfos.Items)
      {
        ContactCustomFieldInfo fieldInfo = contactCustomFieldInfo;
        if (this.fieldValues.FirstOrDefault<ContactCustomField>((Func<ContactCustomField, bool>) (fv => fv.FieldID == fieldInfo.LabelID)) == null)
        {
          ContactCustomField contactCustomField = new ContactCustomField()
          {
            FieldID = fieldInfo.LabelID,
            FieldValue = string.Empty
          };
          contactCustomFieldList.Add(contactCustomField);
        }
      }
      if (contactCustomFieldList.Count > 0)
      {
        this.mngr.UpdateCustomFieldValues(extOrg.oid, contactCustomFieldList.ToArray());
        List<ContactCustomField> list = ((IEnumerable<ContactCustomField>) this.mngr.GetCustomFieldValues(extOrg.oid)).ToList<ContactCustomField>();
        this.fieldValues.Clear();
        foreach (ContactCustomField contactCustomField in list)
          this.fieldValues.Add(contactCustomField);
      }
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in this.fieldInfos.Items)
      {
        ContactCustomFieldInfo fieldInfo = contactCustomFieldInfo;
        this.customFields.Add(new CustomField(this.fieldValues.FirstOrDefault<ContactCustomField>((Func<ContactCustomField, bool>) (fv => fv.FieldID == fieldInfo.LabelID)), fieldInfo.Label, extOrg, fieldInfo.FieldOptions, fieldInfo.FieldType));
      }
    }

    public bool UseParentInfo
    {
      get => this.extOrg.InheritCustomFields;
      set
      {
        if (this.extOrg.Parent == 0)
          throw new Exception("Setting UseParentInfo on parent company not allowed.");
        this.extOrg.InheritCustomFields = value;
        if (!value)
          return;
        ContactCustomField[] customFieldValues = this.mngr.GetCustomFieldValues(this.GetTopID());
        foreach (ContactCustomField fieldValue in this.fieldValues)
        {
          ContactCustomField contactCustomField = fieldValue;
          ContactCustomField contactCustomField1 = ((IEnumerable<ContactCustomField>) customFieldValues).FirstOrDefault<ContactCustomField>((Func<ContactCustomField, bool>) (fv => fv.FieldID == contactCustomField.FieldID));
          if (contactCustomField1 != null)
            contactCustomField.FieldValue = contactCustomField1.FieldValue;
        }
      }
    }

    public CustomField this[string fieldName]
    {
      get
      {
        return this.customFields.FirstOrDefault<CustomField>((Func<CustomField, bool>) (v => v.FieldName == fieldName));
      }
    }

    public CustomField this[int index] => this.customFields[index];

    internal ContactCustomField[] FieldsCollection => this.fieldValues.ToArray();

    public IEnumerator<CustomField> GetEnumerator()
    {
      return (IEnumerator<CustomField>) this.customFields.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
