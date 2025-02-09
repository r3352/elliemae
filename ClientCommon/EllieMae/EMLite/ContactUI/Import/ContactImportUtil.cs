// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.ContactImportUtil
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class ContactImportUtil
  {
    public static ContactImportDupOption SaveBizPartnerContactInfo(
      IWin32Window owner,
      BizPartnerInfo info,
      ContactCustomField[] customFields,
      CustomFieldValue[] standardCategoryFields,
      CustomFieldValue[] customCategoryFields,
      ContactGroupInfo[] groupList,
      bool boolReplaceAll,
      ContactSource contactSource)
    {
      int contactId1 = 0;
      ContactImportDupOption contactImportDupOption = ContactImportDupOption.CreateNew;
      QueryCriterion queryCriterion1 = (QueryCriterion) new StringValueCriterion("Contact.FirstName", info.FirstName, StringMatchType.Exact);
      QueryCriterion queryCriterion2 = (QueryCriterion) new StringValueCriterion("Contact.LastName", info.LastName, StringMatchType.Exact);
      QueryCriterion queryCriterion3 = (QueryCriterion) new StringValueCriterion("Contact.BizAddress1", info.BizAddress.Street1, StringMatchType.Exact);
      QueryCriterion queryCriterion4 = (QueryCriterion) new StringValueCriterion("Contact.CompanyName", info.CompanyName, StringMatchType.Exact);
      BizPartnerInfo[] bizPartnerInfoArray;
      if (info.AccessLevel == ContactAccess.Private)
      {
        QueryCriterion queryCriterion5 = (QueryCriterion) new StringValueCriterion("Contact.OwnerID", Session.UserID, StringMatchType.Exact);
        QueryCriterion queryCriterion6 = (QueryCriterion) new StringValueCriterion("Contact.AccessLevel", "Private", StringMatchType.Exact);
        bizPartnerInfoArray = Session.ContactManager.QueryBizPartners(new QueryCriterion[6]
        {
          queryCriterion1,
          queryCriterion2,
          queryCriterion3,
          queryCriterion4,
          queryCriterion5,
          queryCriterion6
        }, RelatedLoanMatchType.None);
      }
      else
      {
        QueryCriterion queryCriterion7 = (QueryCriterion) new StringValueCriterion("Contact.AccessLevel", "Public", StringMatchType.Exact);
        bizPartnerInfoArray = Session.ContactManager.QueryBizPartners(new QueryCriterion[5]
        {
          queryCriterion1,
          queryCriterion2,
          queryCriterion3,
          queryCriterion4,
          queryCriterion7
        }, RelatedLoanMatchType.None);
      }
      if (bizPartnerInfoArray.Length != 0)
        contactImportDupOption = !boolReplaceAll ? ContactImportUtil.displayBizPartnerDuplicateDialog(owner, bizPartnerInfoArray[0], info) : ContactImportDupOption.ReplaceAll;
      if (contactImportDupOption == ContactImportDupOption.Replace || contactImportDupOption == ContactImportDupOption.ReplaceAll)
      {
        info.ContactID = bizPartnerInfoArray[0].ContactID;
        if (info.AccessLevel == ContactAccess.Public)
          info.OwnerID = "";
        Session.ContactManager.UpdateBizPartner(info);
        contactId1 = info.ContactID;
      }
      else if (contactImportDupOption == ContactImportDupOption.CreateNew)
      {
        if (info.AccessLevel == ContactAccess.Public)
          info.OwnerID = "";
        info.ContactID = Session.ContactManager.CreateBizPartner(info, DateTime.Now, contactSource);
        contactId1 = info.ContactID;
      }
      if (contactImportDupOption != ContactImportDupOption.Skip && contactImportDupOption != ContactImportDupOption.Abort && contactId1 > 0 && info.AccessLevel == ContactAccess.Public && groupList != null && groupList.Length != 0)
      {
        for (int index = 0; index < groupList.Length; ++index)
        {
          bool flag = false;
          if (groupList[index].ContactIds != null)
          {
            foreach (int contactId2 in groupList[index].ContactIds)
            {
              if (contactId2 == contactId1)
              {
                flag = true;
                break;
              }
            }
          }
          if (!flag)
          {
            groupList[index].AddedContactIds = new int[1]
            {
              contactId1
            };
            Session.ContactGroupManager.SaveContactGroup(groupList[index]);
          }
        }
      }
      if (contactImportDupOption == ContactImportDupOption.ReplaceAll || contactImportDupOption == ContactImportDupOption.Replace || contactImportDupOption == ContactImportDupOption.CreateNew)
      {
        if (customFields != null && customFields.Length != 0)
        {
          for (int index = 0; index < customFields.Length; ++index)
            customFields[index].ContactID = contactId1;
          Session.ContactManager.UpdateCustomFieldsForContact(contactId1, ContactType.BizPartner, customFields);
        }
        if (standardCategoryFields != null && standardCategoryFields.Length != 0)
        {
          CustomFieldsDefinition fieldsDefinition = CustomFieldsDefinition.GetCustomFieldsDefinition(Session.SessionObjects, CustomFieldsType.BizCategoryStandard, info.CategoryID);
          if (0 < fieldsDefinition.CustomFieldDefinitions.Count)
          {
            CustomFieldValueCollection fieldValueCollection = CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(info.ContactID, info.CategoryID));
            foreach (CustomFieldValue standardCategoryField in standardCategoryFields)
            {
              if (fieldsDefinition.CustomFieldDefinitions.ContainsFieldId(standardCategoryField.FieldId))
              {
                if (fieldValueCollection.Contains(standardCategoryField.FieldId))
                {
                  CustomFieldValue customFieldValue = fieldValueCollection.Find(standardCategoryField.FieldId);
                  if (string.Empty == standardCategoryField.FieldValue)
                    fieldValueCollection.Remove(customFieldValue);
                  else
                    customFieldValue.FieldValue = standardCategoryField.FieldValue;
                }
                else if (!(string.Empty == standardCategoryField.FieldValue))
                {
                  CustomFieldValue customFieldValue = CustomFieldValue.NewCustomFieldValue(standardCategoryField.FieldId, contactId1, standardCategoryField.FieldFormat);
                  customFieldValue.FieldValue = standardCategoryField.FieldValue;
                  fieldValueCollection.Add(customFieldValue);
                }
              }
            }
            fieldValueCollection.Save();
          }
        }
        if (customCategoryFields != null && customCategoryFields.Length != 0)
        {
          CustomFieldsDefinition fieldsDefinition = CustomFieldsDefinition.GetCustomFieldsDefinition(Session.SessionObjects, CustomFieldsType.BizCategoryCustom, info.CategoryID);
          if (0 < fieldsDefinition.CustomFieldDefinitions.Count)
          {
            CustomFieldValueCollection fieldValueCollection = CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(info.ContactID, info.CategoryID));
            foreach (CustomFieldValue customCategoryField in customCategoryFields)
            {
              int fieldId = customCategoryField.FieldId;
              if (fieldsDefinition.CustomFieldDefinitions.ContainsFieldId(fieldId))
              {
                if (fieldValueCollection.Contains(customCategoryField.FieldId))
                {
                  CustomFieldValue customFieldValue = fieldValueCollection.Find(customCategoryField.FieldId);
                  if (string.Empty == customCategoryField.FieldValue)
                    fieldValueCollection.Remove(customFieldValue);
                  else
                    customFieldValue.FieldValue = customCategoryField.FieldValue;
                }
                else if (!(string.Empty == customCategoryField.FieldValue))
                {
                  CustomFieldValue customFieldValue = CustomFieldValue.NewCustomFieldValue(customCategoryField.FieldId, contactId1, customCategoryField.FieldFormat);
                  customFieldValue.FieldValue = customCategoryField.FieldValue;
                  fieldValueCollection.Add(customFieldValue);
                }
              }
            }
            fieldValueCollection.Save();
          }
        }
      }
      return contactImportDupOption;
    }

    private static ContactImportDupOption displayBizPartnerDuplicateDialog(
      IWin32Window owner,
      BizPartnerInfo oldContact,
      BizPartnerInfo newContact)
    {
      if (((Control) owner).InvokeRequired)
      {
        ContactImportUtil.DuplicateBizPartnerDialogCallback method = new ContactImportUtil.DuplicateBizPartnerDialogCallback(ContactImportUtil.displayBizPartnerDuplicateDialog);
        return (ContactImportDupOption) ((Control) owner).Invoke((Delegate) method, (object) owner, (object) oldContact, (object) newContact);
      }
      DupContactDialog dupContactDialog = new DupContactDialog(oldContact, newContact);
      int num = (int) dupContactDialog.ShowDialog(owner);
      return dupContactDialog.DupOption;
    }

    private static ContactImportDupOption displayBorrowerDuplicateDialog(
      IWin32Window owner,
      BorrowerInfo oldContact,
      BorrowerInfo newContact)
    {
      if (((Control) owner).InvokeRequired)
      {
        ContactImportUtil.DuplicateBorrowerDialogCallback method = new ContactImportUtil.DuplicateBorrowerDialogCallback(ContactImportUtil.displayBorrowerDuplicateDialog);
        return (ContactImportDupOption) ((Control) owner).Invoke((Delegate) method, (object) owner, (object) oldContact, (object) newContact);
      }
      DupContactDialog dupContactDialog = new DupContactDialog(oldContact, newContact);
      int num = (int) dupContactDialog.ShowDialog(owner);
      return dupContactDialog.DupOption;
    }

    private static ContactImportDupOption displayGlobalBorrowerDuplicateDialog(
      IWin32Window owner,
      BorrowerInfo oldContact,
      BorrowerInfo newContact)
    {
      if (((Control) owner).InvokeRequired)
      {
        ContactImportUtil.GlobalDuplicateBorrowerDialogCallback method = new ContactImportUtil.GlobalDuplicateBorrowerDialogCallback(ContactImportUtil.displayGlobalBorrowerDuplicateDialog);
        return (ContactImportDupOption) ((Control) owner).Invoke((Delegate) method, (object) owner, (object) oldContact, (object) newContact);
      }
      DupContactDialog dupContactDialog = new DupContactDialog(oldContact, newContact, true);
      int num = (int) dupContactDialog.ShowDialog(owner);
      return dupContactDialog.DupOption;
    }

    public static ContactImportDupOption SaveBorrowerContactInfo(
      IWin32Window owner,
      BorrowerInfo info,
      Opportunity opp,
      ContactCustomField[] customFields,
      bool boolReplaceAll,
      bool boolCreateAll,
      ContactSource contactSource)
    {
      int contactId = 0;
      ContactImportDupOption contactImportDupOption1 = ContactImportDupOption.None;
      ContactImportDupOption contactImportDupOption2 = ContactImportDupOption.None;
      QueryCriterion queryCriterion1 = (QueryCriterion) new StringValueCriterion("Contact.FirstName", info.FirstName, StringMatchType.Exact);
      QueryCriterion queryCriterion2 = (QueryCriterion) new StringValueCriterion("Contact.LastName", info.LastName, StringMatchType.Exact);
      QueryCriterion queryCriterion3 = (QueryCriterion) new StringValueCriterion("Contact.HomeAddress1", info.HomeAddress.Street1, StringMatchType.Exact);
      QueryCriterion queryCriterion4 = (QueryCriterion) new StringValueCriterion("Contact.OwnerID", info.OwnerID, StringMatchType.Exact);
      BorrowerInfo[] borrowerInfoArray1 = Session.ContactManager.QueryBorrowers(new QueryCriterion[4]
      {
        queryCriterion1,
        queryCriterion2,
        queryCriterion3,
        queryCriterion4
      });
      BorrowerInfo[] borrowerInfoArray2 = Session.ContactManager.QueryBorrowersConflict(new QueryCriterion[3]
      {
        queryCriterion1,
        queryCriterion2,
        queryCriterion3
      });
      if (borrowerInfoArray1.Length != 0)
        contactImportDupOption1 = !boolReplaceAll ? ContactImportUtil.displayBorrowerDuplicateDialog(owner, borrowerInfoArray1[0], info) : ContactImportDupOption.ReplaceAll;
      else if (borrowerInfoArray2.Length != 0)
        contactImportDupOption2 = !boolCreateAll ? ContactImportUtil.displayGlobalBorrowerDuplicateDialog(owner, borrowerInfoArray2[0], info) : ContactImportDupOption.CreateNewAll;
      else
        contactImportDupOption1 = ContactImportDupOption.CreateNew;
      switch (contactImportDupOption1)
      {
        case ContactImportDupOption.ReplaceAll:
        case ContactImportDupOption.Replace:
          info.ContactID = borrowerInfoArray1[0].ContactID;
          Session.ContactManager.UpdateBorrower(info);
          contactId = info.ContactID;
          if (opp != null)
          {
            opp.ContactID = contactId;
            Session.ContactManager.UpdateBorrowerOpportunity(opp);
            break;
          }
          break;
        case ContactImportDupOption.CreateNew:
          info.ContactID = Session.ContactManager.CreateBorrower(info, DateTime.Now, contactSource);
          contactId = info.ContactID;
          if (opp != null)
          {
            opp.ContactID = contactId;
            Session.ContactManager.CreateBorrowerOpportunity(opp);
            break;
          }
          break;
        default:
          if (contactImportDupOption2 == ContactImportDupOption.CreateNew || contactImportDupOption2 == ContactImportDupOption.CreateNewAll)
            goto case ContactImportDupOption.CreateNew;
          else
            break;
      }
      if (customFields != null && customFields.Length != 0 && (contactImportDupOption1 == ContactImportDupOption.ReplaceAll || contactImportDupOption1 == ContactImportDupOption.Replace || contactImportDupOption1 == ContactImportDupOption.CreateNew || contactImportDupOption2 == ContactImportDupOption.CreateNew || contactImportDupOption2 == ContactImportDupOption.CreateNewAll))
      {
        for (int index = 0; index < customFields.Length; ++index)
          customFields[index].ContactID = contactId;
        Session.ContactManager.UpdateCustomFieldsForContact(contactId, ContactType.Borrower, customFields);
      }
      return contactImportDupOption1 != ContactImportDupOption.None ? contactImportDupOption1 : contactImportDupOption2;
    }

    public static ContactImportDupOption SaveTPOOrganization(
      IWin32Window owner,
      ExternalOriginatorManagementData info,
      bool replaceAll,
      bool createAll,
      ContactSource contactSource)
    {
      ExternalOriginatorManagementData[] originatorManagementDataArray = (ExternalOriginatorManagementData[]) null;
      if ((info.ExternalID ?? "") != string.Empty)
        originatorManagementDataArray = Session.ConfigurationManager.QueryExternalOrganizations(new QueryCriterion[1]
        {
          (QueryCriterion) new StringValueCriterion("org.ExternalID", info.ExternalID, StringMatchType.Exact)
        });
      else if (info.CompanyLegalName != string.Empty)
        originatorManagementDataArray = Session.ConfigurationManager.QueryExternalOrganizations(new QueryCriterion[1]
        {
          (QueryCriterion) new StringValueCriterion("org.CompanyLegalName", info.CompanyLegalName, StringMatchType.Exact)
        });
      ContactImportDupOption contactImportDupOption = originatorManagementDataArray == null || originatorManagementDataArray.Length == 0 ? ContactImportDupOption.CreateNew : (!createAll ? ContactImportUtil.displayGlobalTPOCompanyDuplicateDialog(owner, originatorManagementDataArray[0], info) : ContactImportDupOption.CreateNewAll);
      switch (contactImportDupOption)
      {
        case ContactImportDupOption.ReplaceAll:
        case ContactImportDupOption.Replace:
          Session.ConfigurationManager.UpdateExternalContact(false, info);
          break;
        case ContactImportDupOption.CreateNew:
        case ContactImportDupOption.CreateNewAll:
          int num = Session.ConfigurationManager.ImportExternalContact(info);
          info.oid = num;
          if ((info.CompanyDBAName ?? "") != "")
          {
            try
            {
              Session.ConfigurationManager.InsertDBANames(new ExternalOrgDBAName(info.oid, info.CompanyDBAName, true), info.oid);
              break;
            }
            catch (Exception ex)
            {
              break;
            }
          }
          else
            break;
      }
      return contactImportDupOption;
    }

    public static ContactImportDupOption SaveTPOExternalUser(
      IWin32Window owner,
      ExternalUserInfo info,
      List<long> allContactIDs,
      bool replaceAll,
      bool createAll,
      ContactSource contactSource)
    {
      ExternalUserInfo[] externalUserInfoArray = (ExternalUserInfo[]) null;
      if ((info.ExternalUserID ?? "") != string.Empty)
        externalUserInfoArray = Session.ConfigurationManager.QueryExternalUsers(new QueryCriterion[1]
        {
          (QueryCriterion) new StringValueCriterion("Contact.external_userid", info.ExternalUserID, StringMatchType.Exact)
        });
      else if (info.SSN != string.Empty)
        externalUserInfoArray = Session.ConfigurationManager.QueryExternalUsers(new QueryCriterion[1]
        {
          (QueryCriterion) new StringValueCriterion("Contact.SSN", info.SSN, StringMatchType.Exact)
        });
      else if (info.FirstName + info.LastName != string.Empty)
        externalUserInfoArray = Session.ConfigurationManager.QueryExternalUsers(new QueryCriterion[2]
        {
          (QueryCriterion) new StringValueCriterion("Contact.First_name", info.FirstName, StringMatchType.Exact),
          (QueryCriterion) new StringValueCriterion("Contact.Last_name", info.LastName, StringMatchType.Exact)
        });
      ContactImportDupOption contactImportDupOption = externalUserInfoArray == null || externalUserInfoArray.Length == 0 ? ContactImportDupOption.CreateNew : (!createAll ? ContactImportUtil.displayGlobalTPODuplicateDialog(owner, externalUserInfoArray[0], info) : ContactImportDupOption.CreateNewAll);
      if (allContactIDs == null)
        allContactIDs = new List<long>();
      info.UpdatedByExternalAdmin = string.Empty;
      switch (contactImportDupOption)
      {
        case ContactImportDupOption.ReplaceAll:
        case ContactImportDupOption.Replace:
          info.ContactID = externalUserInfoArray[0].ContactID;
          info.ExternalUserID = externalUserInfoArray[0].ExternalUserID;
          info.ExternalOrgID = externalUserInfoArray[0].ExternalOrgID;
          Session.ConfigurationManager.SaveExternalUserInfo(info);
          break;
        case ContactImportDupOption.CreateNew:
        case ContactImportDupOption.CreateNewAll:
          long num = ExternalUserInfo.NewContactID(allContactIDs);
          info.ContactID = string.Concat((object) num);
          info = Session.ConfigurationManager.SaveExternalUserInfo(info, false, true);
          allContactIDs.Add(num);
          break;
      }
      return contactImportDupOption;
    }

    private static ContactImportDupOption displayGlobalTPODuplicateDialog(
      IWin32Window owner,
      ExternalUserInfo oldContact,
      ExternalUserInfo newContact)
    {
      if (((Control) owner).InvokeRequired)
      {
        ContactImportUtil.GlobalDuplicateTPODialogCallback method = new ContactImportUtil.GlobalDuplicateTPODialogCallback(ContactImportUtil.displayGlobalTPODuplicateDialog);
        return (ContactImportDupOption) ((Control) owner).Invoke((Delegate) method, (object) owner, (object) oldContact, (object) newContact);
      }
      DupContactDialog dupContactDialog = new DupContactDialog(oldContact, newContact);
      int num = (int) dupContactDialog.ShowDialog(owner);
      return dupContactDialog.DupOption;
    }

    private static ContactImportDupOption displayGlobalTPOCompanyDuplicateDialog(
      IWin32Window owner,
      ExternalOriginatorManagementData oldContact,
      ExternalOriginatorManagementData newContact)
    {
      if (((Control) owner).InvokeRequired)
      {
        ContactImportUtil.GlobalDuplicateTPOCompanyDialogCallback method = new ContactImportUtil.GlobalDuplicateTPOCompanyDialogCallback(ContactImportUtil.displayGlobalTPOCompanyDuplicateDialog);
        return (ContactImportDupOption) ((Control) owner).Invoke((Delegate) method, (object) owner, (object) oldContact, (object) newContact);
      }
      DupContactDialog dupContactDialog = new DupContactDialog(oldContact, newContact);
      int num = (int) dupContactDialog.ShowDialog(owner);
      return dupContactDialog.DupOption;
    }

    private delegate ContactImportDupOption DuplicateBizPartnerDialogCallback(
      IWin32Window owner,
      BizPartnerInfo oldContact,
      BizPartnerInfo newContact);

    private delegate ContactImportDupOption DuplicateBorrowerDialogCallback(
      IWin32Window owner,
      BorrowerInfo oldContact,
      BorrowerInfo newContact);

    private delegate ContactImportDupOption GlobalDuplicateBorrowerDialogCallback(
      IWin32Window owner,
      BorrowerInfo oldContact,
      BorrowerInfo newContact);

    private delegate ContactImportDupOption GlobalDuplicateTPODialogCallback(
      IWin32Window owner,
      ExternalUserInfo oldContact,
      ExternalUserInfo newContact);

    private delegate ContactImportDupOption GlobalDuplicateTPOCompanyDialogCallback(
      IWin32Window owner,
      ExternalOriginatorManagementData oldContact,
      ExternalOriginatorManagementData newContact);
  }
}
