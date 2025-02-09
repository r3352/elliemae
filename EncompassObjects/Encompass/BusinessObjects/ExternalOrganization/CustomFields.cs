// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomFields
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomField">CustomField</see>
  /// for an <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrganization</see>
  /// </summary>
  /// <example>
  ///       The following code sets UseParentInfo to false, outputs the current value of the custom field,
  ///       sets a new value and commits the changes to the ExternalOrganization.
  ///       <code>
  ///         <![CDATA[
  /// using System;
  /// using System.IO;
  /// using System.Linq;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.Collections;
  /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
  /// 
  /// class CustomFields
  /// {
  ///   public static void Main()
  ///   {
  ///     // Open the session to the remote server. We will need to be logged
  ///     // in as an Administrator to modify the user accounts.
  ///     Session session = new Session();
  ///     session.Start("myserver", "admin", "adminpwd");
  /// 
  ///     //Use the organization name as method argument, and here using false to indicate this organization is not using its parent's information
  ///     ExternalOrganization_SetCustomFields(session, "Test Child 1", false);
  /// 
  ///     // End the session to gracefully disconnect from the server
  ///     session.End();
  ///   }
  /// 
  ///   private static void ExternalOrganization_SetCustomFields(Session session, string ExternalOrganizationName, bool UseParentInfo)
  ///   {
  ///     // Fetch all of the organizations, in order to get the ExternalID
  ///     ExternalOrganizationList orgs = session.Organizations.GetAllExternalOrganizations();
  /// 
  ///     // Get the Organziation by its organization name
  ///     ExternalOrganization externalOrg = orgs.Cast<ExternalOrganization>().FirstOrDefault(org => org.OrganizationName == ExternalOrganizationName);
  /// 
  ///     // Check if it is parent or not. Then followed by to see if it is going to use parent info, if yes , commit and return;
  ///     // Otherwise continue to retrieve and the value
  ///     if (externalOrg.ParentOrganizationID != 0)
  ///     {
  ///       if (UseParentInfo)
  ///       {
  ///         externalOrg.CustomFields.UseParentInfo = true;
  ///         externalOrg.Commit();
  ///         return;
  ///       }
  ///       else
  ///       {
  ///         externalOrg.CustomFields.UseParentInfo = false;
  ///       }
  ///     }
  /// 
  ///     // Retrieve the current organization's Zip Code value first
  ///     CustomFields customFields = externalOrg.CustomFields;
  /// 
  ///     // Retrieve the value of customized field:  CUSTOMIZED_ZIP_CODE_FIELD
  ///     CustomField targetField = customFields.FirstOrDefault(CF => CF.FieldName == "CUSTOMIZED_ZIP_CODE_FIELD");
  ///     Console.Write("targetField:" + targetField.FieldValue);
  /// 
  ///     // Set a new value for this field
  ///     targetField.FieldValue = "12345";
  /// 
  ///     // Commit the change for this field
  ///     externalOrg.Commit();
  /// 
  ///   }
  /// }
  /// 
  /// ]]>
  ///       </code>
  ///     </example>
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

    /// <summary>
    /// Gets and sets if the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomField">CustomField</see> is coming from
    /// the parent <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrganization</see>
    /// </summary>
    /// <remarks>Setting this value to true will update all fields with the values of the parent.
    /// </remarks>
    /// <exception cref="T:System.Exception">Throws an exception if attempting to set a top level
    /// company to use parent info.</exception>
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

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomField">CustomField</see>
    /// by field name.
    /// </summary>
    /// <param name="fieldName">Name of the field</param>
    /// <returns>A <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomField">CustomField</see></returns>
    public CustomField this[string fieldName]
    {
      get
      {
        return this.customFields.FirstOrDefault<CustomField>((Func<CustomField, bool>) (v => v.FieldName == fieldName));
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomField">CustomField</see>
    /// by the index in the collection.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>A <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.CustomField">CustomField</see></returns>
    public CustomField this[int index] => this.customFields[index];

    internal ContactCustomField[] FieldsCollection => this.fieldValues.ToArray();

    /// <summary>TBD</summary>
    /// <returns></returns>
    public IEnumerator<CustomField> GetEnumerator()
    {
      return (IEnumerator<CustomField>) this.customFields.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
