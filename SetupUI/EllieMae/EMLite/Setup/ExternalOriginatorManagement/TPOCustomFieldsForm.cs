// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOCustomFieldsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOCustomFieldsForm : ContactCustomFieldsForm, IOnlineHelpTarget
  {
    private Sessions.Session session;
    private bool isSettingsSync;

    public TPOCustomFieldsForm(
      Sessions.Session session,
      SetUpContainer setupContainer,
      ContactType contactType)
      : this(session, setupContainer, contactType, false)
    {
    }

    public TPOCustomFieldsForm(
      Sessions.Session session,
      SetUpContainer setupContainer,
      ContactType contactType,
      bool isSettingsSync)
      : base(setupContainer, contactType, session, isSettingsSync)
    {
      this.contactType = contactType;
      this.isSettingsSync = isSettingsSync;
      this.session = session;
    }

    public override void Reset()
    {
      ContactCustomFieldInfoCollection customFieldInfo = this.session.ConfigurationManager.GetCustomFieldInfo();
      ContactCustomFieldInfo[] contactCustomFieldInfoArray1 = customFieldInfo != null ? customFieldInfo.Items : new ContactCustomFieldInfo[0];
      this.loanFields = StandardFields.All;
      this.loanCustomFields = this.session.ConfigurationManager.GetLoanCustomFields();
      List<CustomFieldMappingInfo> fieldMappingInfoList = new List<CustomFieldMappingInfo>();
      if (customFieldInfo.Items != null)
      {
        foreach (ContactCustomFieldInfo contactCustomFieldInfo in customFieldInfo.Items)
        {
          if (string.Empty != contactCustomFieldInfo.LoanFieldId)
            fieldMappingInfoList.Add(new CustomFieldMappingInfo(CustomFieldsType.None, 0, contactCustomFieldInfo.LabelID, 0, contactCustomFieldInfo.FieldType, contactCustomFieldInfo.LoanFieldId, contactCustomFieldInfo.TwoWayTransfer));
        }
      }
      this.customFieldMappingCollection = CustomFieldMappingCollection.GetCustomFieldMappingCollection(new CustomFieldsMappingInfo(CustomFieldsType.None, fieldMappingInfoList.ToArray()), new CustomFieldMappingCollection.Criteria(CustomFieldsType.None, false));
      this.loanFieldMappingDlg = new LoanFieldMappingDialog(this.loanFields, this.loanCustomFields, this.contactType, this.customFieldMappingCollection, this.categories);
      this._LabelIDToFieldHash = new Hashtable(contactCustomFieldInfoArray1.Length);
      for (int index = 0; index < contactCustomFieldInfoArray1.Length; ++index)
        this._LabelIDToFieldHash.Add((object) contactCustomFieldInfoArray1[index].LabelID, (object) contactCustomFieldInfoArray1[index]);
      ContactCustomFieldInfo[] contactCustomFieldInfoArray2 = customFieldInfo != null ? new ContactCustomFieldInfoCollection()
      {
        Items = Utils.DeepClone<ContactCustomFieldInfo[]>(customFieldInfo.Items)
      }.Items : new ContactCustomFieldInfo[0];
      this._LabelIDToFieldHashInit = new Hashtable(contactCustomFieldInfoArray2.Length);
      for (int index = 0; index < contactCustomFieldInfoArray2.Length; ++index)
        this._LabelIDToFieldHashInit.Add((object) contactCustomFieldInfoArray2[index].LabelID, (object) contactCustomFieldInfoArray2[index]);
      if (customFieldInfo != null)
      {
        this.pageNames[0] = customFieldInfo.Page1Name;
        this.pageNames[1] = customFieldInfo.Page2Name;
        this.pageNames[2] = customFieldInfo.Page3Name;
        this.pageNames[3] = customFieldInfo.Page4Name;
        this.pageNames[4] = customFieldInfo.Page5Name;
      }
      this.displayCustomFieldTab(this.tabCustomFields.SelectedIndex);
      this.setDirtyFlag(false);
    }

    public void clearUIControls(int fieldIndex)
    {
      this.chkTwoWayTransfers[fieldIndex].Visible = false;
    }

    protected override void displayContactCustomFieldTab(int tabIndex)
    {
      base.displayContactCustomFieldTab(tabIndex);
      int num = tabIndex * 20;
      for (int fieldIndex = 0; fieldIndex < 20; ++fieldIndex)
        this.clearUIControls(fieldIndex);
      this.showMapping = true;
      this.splitter1.Visible = true;
      this.lblLoanFieldMapping.Text = "";
      this.lblDirection.Text = "";
      this.lblDirections.Text = "The custom tab name displays on Company Details > Custom Fields tab.";
    }

    protected override void updateCustomFieldMappingCollection(
      CustomFieldsType customFieldsType,
      int categoryId,
      int fieldNumber,
      int recordId,
      FieldFormat fieldFormat,
      string loanFieldId,
      bool twoWayTransfer)
    {
      base.updateCustomFieldMappingCollection(CustomFieldsType.None, categoryId, fieldNumber, recordId, fieldFormat, loanFieldId, twoWayTransfer);
    }

    protected void getContactDataUpdates()
    {
      foreach (ContactCustomFieldInfo contactCustomFieldInfo1 in (IEnumerable) this._LabelIDToFieldHashInit.Values)
      {
        if (this._LabelIDToFieldHash.Contains((object) contactCustomFieldInfo1.LabelID))
        {
          ContactCustomFieldInfo contactCustomFieldInfo2 = (ContactCustomFieldInfo) this._LabelIDToFieldHash[(object) contactCustomFieldInfo1.LabelID];
          if (contactCustomFieldInfo2.FieldType == contactCustomFieldInfo1.FieldType && contactCustomFieldInfo2.Label.Equals(contactCustomFieldInfo1.Label) && contactCustomFieldInfo2.LoanFieldId.Equals(contactCustomFieldInfo1.LoanFieldId) && contactCustomFieldInfo2.FieldType != FieldFormat.DROPDOWN && contactCustomFieldInfo2.FieldType != FieldFormat.DROPDOWNLIST)
            this._LabelIDToFieldHash.Remove((object) contactCustomFieldInfo1.LabelID);
        }
      }
      foreach (ContactCustomFieldInfo contactCustomFieldInfo in (IEnumerable) this._LabelIDToFieldHash.Values)
      {
        if (contactCustomFieldInfo.FieldType != FieldFormat.DROPDOWN && contactCustomFieldInfo.FieldType != FieldFormat.DROPDOWNLIST)
          contactCustomFieldInfo.FieldOptions = (string[]) null;
      }
    }

    protected override void saveContactFields(ArrayList invalidFieldIds)
    {
      try
      {
        this.getContactDataUpdates();
        ContactCustomFieldInfoCollection customFields = new ContactCustomFieldInfoCollection();
        ArrayList arrayList = new ArrayList(this._LabelIDToFieldHash.Values);
        customFields.Items = (ContactCustomFieldInfo[]) arrayList.ToArray(typeof (ContactCustomFieldInfo));
        this.pageNames[this.tabCustomFields.SelectedIndex] = this.txtPageName.Text;
        customFields.Page1Name = this.pageNames[0];
        customFields.Page2Name = this.pageNames[1];
        customFields.Page3Name = this.pageNames[2];
        customFields.Page4Name = this.pageNames[3];
        customFields.Page5Name = this.pageNames[4];
        this.session.ConfigurationManager.UpdateCustomFieldInfo(customFields, invalidFieldIds);
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Custom Fields cannot be saved, please contact your Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\TPO Custom Fields";
  }
}
