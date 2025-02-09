// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOClientUtils
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOClientUtils
  {
    private static string sw = Tracing.SwOutsideLoan;
    private const string className = "TPOClientUtils";
    private static FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
    private static bool hasContactEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditContacts);
    private static bool hasSendWelcomeEmailRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_SendWelcomeEmail);
    private static bool hasResetPasswordRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_ResetPassword);
    private static bool hasOrganizationEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditCompanyInformation);
    private static bool hasNotesEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditNotes);
    private static bool hasNotesDeleteRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DeleteNotes);
    private static bool hasSalesRepEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditSalesReps);
    private static bool hasWebCenterEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditWebCenterSetup);
    private static bool hasTPOContactsEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditTPOContacts);
    private static bool hasLenderContactsEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditLenderContacts);
    private static bool hasLonTypesEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditLoanTypes);
    private static bool hasAttachmentsEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditAttachments);
    private static bool hasAttachmentsDeleteRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DeleteAttachments);
    private static bool hasLicenseEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditLicenseInformation);
    private static bool hasLOCompEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditLOComp);
    private static bool hasTradeMgmtEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditTradeMgmt);
    private static bool hasONRPEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditONRP);
    private static bool hasCommitmentEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditCommitment);
    private static bool hasFeesEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditTPOFeesTab);
    private static bool hasFeesDeleteRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DeleteTPOFeesTab);
    private static bool hasWarehouseEditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditWarehouseTab);
    private static bool hasCustomFieldsTab1EditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditCustomFieldsTab1);
    private static bool hasCustomFieldsTab2EditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditCustomFieldsTab2);
    private static bool hasCustomFieldsTab3EditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditCustomFieldsTab3);
    private static bool hasCustomFieldsTab4EditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditCustomFieldsTab4);
    private static bool hasCustomFieldsTab5EditRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_EditCustomFieldsTab5);
    private static bool hasCompanyInformationTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CompanyInformation);
    private static bool hasLicenseTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_License);
    private static bool hasLoanTypeTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_LoanType);
    private static bool hasTPOContactsTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOContacts);
    private static bool hasFeesRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TPOFeesTab);
    private static bool hasLOCompTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_LOComp);
    private static bool hasTradeMgmtTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_TradeMgmt);
    private static bool hasONRPTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_ONRP);
    private static bool hasCommitmentRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Commitment);
    private static bool hasNotesTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Notes);
    private static bool hasTPOWebCenterTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_WebCenterSetup);
    private static bool hasAttachmentsTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_Attachments);
    private static bool hasTPOSalesRepsTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_SalesReps);
    private static bool hasLenderContactsTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_LenderContacts);
    private static bool hasDBARight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_DBATab);
    private static bool hasCustomFieldsTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_CustomFields);
    private static bool hasWarehouseTabRight = TPOClientUtils.aclManager.GetUserApplicationRight(AclFeature.ExternalSettings_WarehouseTab);

    public static bool HasCompanyTabAccess()
    {
      return TPOClientUtils.hasCompanyInformationTabRight || TPOClientUtils.hasLicenseTabRight || TPOClientUtils.hasLoanTypeTabRight || TPOClientUtils.hasTPOContactsTabRight || TPOClientUtils.hasLOCompTabRight || TPOClientUtils.hasTradeMgmtTabRight || TPOClientUtils.hasONRPTabRight || TPOClientUtils.hasNotesTabRight || TPOClientUtils.hasTPOWebCenterTabRight || TPOClientUtils.hasAttachmentsTabRight || TPOClientUtils.hasTPOSalesRepsTabRight || TPOClientUtils.hasLenderContactsTabRight || TPOClientUtils.hasCustomFieldsTabRight || TPOClientUtils.hasWarehouseTabRight;
    }

    public static bool HasCompanyEditAccess()
    {
      return TPOClientUtils.hasOrganizationEditRight || TPOClientUtils.hasNotesEditRight || TPOClientUtils.hasNotesDeleteRight || TPOClientUtils.hasSalesRepEditRight || TPOClientUtils.hasLenderContactsEditRight || TPOClientUtils.hasWebCenterEditRight || TPOClientUtils.hasTPOContactsEditRight || TPOClientUtils.hasLonTypesEditRight || TPOClientUtils.hasAttachmentsEditRight || TPOClientUtils.hasAttachmentsDeleteRight || TPOClientUtils.hasLicenseEditRight || TPOClientUtils.hasFeesRight || TPOClientUtils.hasFeesEditRight || TPOClientUtils.hasFeesDeleteRight || TPOClientUtils.hasCommitmentRight || TPOClientUtils.hasLOCompEditRight || TPOClientUtils.hasTradeMgmtEditRight || TPOClientUtils.hasONRPEditRight || TPOClientUtils.hasCustomFieldsTab1EditRight || TPOClientUtils.hasCustomFieldsTab2EditRight || TPOClientUtils.hasCustomFieldsTab3EditRight || TPOClientUtils.hasCustomFieldsTab4EditRight || TPOClientUtils.hasCustomFieldsTab5EditRight || TPOClientUtils.hasWarehouseEditRight;
    }

    public static void DisableFormControls(Form form)
    {
      Control control1 = form.Controls.Find("btnReset", true)[0];
      Control control2 = form.Controls.Find("btnPreviewSend", true)[0];
      if (!TPOClientUtils.hasContactEditRight)
        TPOClientUtils.SecureControls((Control) form);
      control1.Enabled = TPOClientUtils.hasResetPasswordRight;
      if (!TPOClientUtils.hasSendWelcomeEmailRight)
        control2.Enabled = false;
      else
        control2.Enabled = true;
    }

    public static void DisableControls(UserControl userControl, bool hierarchyAccess)
    {
      if (hierarchyAccess)
      {
        if (userControl.Name.Equals("EditCompanyInfoControl") && !TPOClientUtils.hasOrganizationEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyLoanTypeControl") && !TPOClientUtils.hasLonTypesEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditOrgLicenseControl") && !TPOClientUtils.hasLicenseEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyCommitmentControl") && !TPOClientUtils.hasCommitmentEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyFeesControl"))
        {
          if (!TPOClientUtils.hasFeesEditRight)
          {
            TPOClientUtils.SecureControls((Control) userControl);
          }
          else
          {
            if (TPOClientUtils.hasFeesDeleteRight)
              return;
            TPOClientUtils.SecureGridControls((Control) userControl);
          }
        }
        else if (userControl.Name.Equals("EditCompanyWarehouseControl"))
        {
          if (TPOClientUtils.hasWarehouseEditRight)
            return;
          TPOClientUtils.SecureControls((Control) userControl);
        }
        else if (userControl.Name.Equals("EditCompanySalesRepControl") && !TPOClientUtils.hasSalesRepEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyWebcenterControl") && !TPOClientUtils.hasWebCenterEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyContactControl") && !TPOClientUtils.hasTPOContactsEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyLenderContactControl") && !TPOClientUtils.hasLenderContactsEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyLOCompControl") && !TPOClientUtils.hasLOCompEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyTradeMgmtControl") && !TPOClientUtils.hasTradeMgmtEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyONRPControl") && !TPOClientUtils.hasONRPEditRight)
          TPOClientUtils.SecureControls((Control) userControl);
        else if (userControl.Name.Equals("EditCompanyNoteControl"))
        {
          if (!TPOClientUtils.hasNotesEditRight)
          {
            TPOClientUtils.SecureControls((Control) userControl);
            TPOClientUtils.EnableNotesPreviewControl((Control) userControl);
          }
          else
          {
            if (TPOClientUtils.hasNotesDeleteRight)
              return;
            TPOClientUtils.SecureGridControls((Control) userControl);
          }
        }
        else if (userControl.Name.Equals("EditCompanyAttachmentControl"))
        {
          if (!TPOClientUtils.hasAttachmentsEditRight)
          {
            TPOClientUtils.SecureControls((Control) userControl);
            TPOClientUtils.EnableAttachmentPreviewControl((Control) userControl);
          }
          else
          {
            if (TPOClientUtils.hasAttachmentsDeleteRight)
              return;
            TPOClientUtils.SecureGridControls((Control) userControl);
          }
        }
        else
        {
          if (!userControl.Name.Equals("EditCompanyCustomTabs"))
            return;
          if (!TPOClientUtils.hasCustomFieldsTab1EditRight)
            TPOClientUtils.SecureTabs((Control) userControl, "Tab1");
          if (!TPOClientUtils.hasCustomFieldsTab2EditRight)
            TPOClientUtils.SecureTabs((Control) userControl, "Tab2");
          if (!TPOClientUtils.hasCustomFieldsTab3EditRight)
            TPOClientUtils.SecureTabs((Control) userControl, "Tab3");
          if (!TPOClientUtils.hasCustomFieldsTab4EditRight)
            TPOClientUtils.SecureTabs((Control) userControl, "Tab4");
          if (TPOClientUtils.hasCustomFieldsTab5EditRight)
            return;
          TPOClientUtils.SecureTabs((Control) userControl, "Tab5");
        }
      }
      else if (userControl.Name.Equals("EditCompanyCustomTabs"))
      {
        TPOClientUtils.SecureTabs((Control) userControl, "Tab1");
        TPOClientUtils.SecureTabs((Control) userControl, "Tab2");
        TPOClientUtils.SecureTabs((Control) userControl, "Tab3");
        TPOClientUtils.SecureTabs((Control) userControl, "Tab4");
        TPOClientUtils.SecureTabs((Control) userControl, "Tab5");
        userControl.Controls.Find("chkUseParentInfo", true)[0].Enabled = false;
      }
      else
        TPOClientUtils.SecureControls((Control) userControl);
    }

    public static void SecureGridControls(Control control)
    {
      foreach (Control control1 in (ArrangedElementCollection) control.Controls)
      {
        if (control1.GetType() == typeof (GridView))
          ((GridView) control1).SelectedIndexChanged += (EventHandler) ((sender, e) => control.Controls.Find("btnDelete", true)[0].Enabled = false);
        else
          TPOClientUtils.SecureGridControls(control1);
      }
    }

    public static void EnableAttachmentPreviewControl(Control control)
    {
      foreach (Control control1 in (ArrangedElementCollection) control.Controls)
      {
        if (control1.GetType() == typeof (GridView))
        {
          GridView gr = (GridView) control1;
          gr.SelectedIndexChanged += (EventHandler) ((sender, e) => control.Controls.Find("btnView", true)[0].Enabled = gr.SelectedItems.Count == 1);
        }
        else
          TPOClientUtils.EnableAttachmentPreviewControl(control1);
      }
    }

    public static void EnableNotesPreviewControl(Control control)
    {
      foreach (Control control1 in (ArrangedElementCollection) control.Controls)
      {
        if (control1.GetType() == typeof (GridView))
          ((GridView) control1).SelectedIndexChanged += (EventHandler) ((sender, e) =>
          {
            control.Controls.Find("btnView", true)[0].Enabled = true;
            control.Controls.Find("btnExport", true)[0].Enabled = true;
          });
        else
          TPOClientUtils.EnableNotesPreviewControl(control1);
      }
    }

    public static void SecureControls(Control control)
    {
      foreach (Control control1 in (ArrangedElementCollection) control.Controls)
      {
        if (control1.GetType() == typeof (TextBox) || control1.GetType() == typeof (ComboBox) || control1.GetType() == typeof (CheckBox) || control1.GetType() == typeof (RadioButton) || control1.GetType() == typeof (Button) || control1.GetType() == typeof (StandardIconButton) || control1.GetType() == typeof (DatePicker))
        {
          if (control1.GetType().GetProperty("ReadOnly") != (PropertyInfo) null)
            control1.GetType().GetProperty("ReadOnly").SetValue((object) control1, (object) true, (object[]) null);
          else if (control1.GetType().GetProperty("Enabled") != (PropertyInfo) null)
            control1.GetType().GetProperty("Enabled").SetValue((object) control1, (object) false, (object[]) null);
        }
        else if (control1.GetType() == typeof (GridView))
        {
          GridView gridView = (GridView) control1;
          for (int nItemIndex = 0; nItemIndex < gridView.Items.Count; ++nItemIndex)
          {
            gridView.Items[nItemIndex].SubItems[0].CheckBoxEnabled = false;
            gridView.Items[nItemIndex].SubItems[1].CheckBoxEnabled = false;
          }
          FieldInfo field1 = gridView.GetType().GetField("SelectedIndexChanged", BindingFlags.Instance | BindingFlags.NonPublic);
          if (field1 != (FieldInfo) null)
          {
            object obj = field1.GetValue((object) gridView);
            if (obj is EventHandler)
            {
              EventHandler eventHandler = (EventHandler) obj;
              gridView.SelectedIndexChanged -= eventHandler;
            }
          }
          FieldInfo field2 = gridView.GetType().GetField("EventDoubleClick", BindingFlags.Instance | BindingFlags.NonPublic);
          if (field2 != (FieldInfo) null)
          {
            object obj = field2.GetValue((object) gridView);
            if (obj is EventHandler)
            {
              EventHandler eventHandler = (EventHandler) obj;
              gridView.Click -= eventHandler;
            }
          }
          FieldInfo field3 = gridView.GetType().GetField("ItemDoubleClick", BindingFlags.Instance | BindingFlags.NonPublic);
          if (field3 != (FieldInfo) null)
          {
            object obj = field3.GetValue((object) gridView);
            if (obj is GVItemEventHandler)
            {
              GVItemEventHandler itemEventHandler = (GVItemEventHandler) obj;
              gridView.ItemDoubleClick -= itemEventHandler;
            }
          }
          gridView.EditorOpening += (GVSubItemEditingEventHandler) ((sender, e) => e.Cancel = true);
        }
        else
          TPOClientUtils.SecureControls(control1);
      }
    }

    public static void SecureTabs(Control control, string tabName)
    {
      foreach (Control control1 in (ArrangedElementCollection) control.Controls)
      {
        if (control1.GetType() == typeof (TabControl))
          TPOClientUtils.SecureTabs(control1, tabName);
        else if (control1.GetType() == typeof (TabPage))
        {
          if (control1.Name.Equals(tabName))
            TPOClientUtils.SecureTabs(control1, tabName);
        }
        else if (control1.GetType() == typeof (TextBox) || control1.GetType() == typeof (ComboBox) || control1.GetType() == typeof (CheckBox) || control1.GetType() == typeof (RadioButton) || control1.GetType() == typeof (Button) || control1.GetType() == typeof (StandardIconButton) || control1.GetType() == typeof (DatePicker))
        {
          if (control1.GetType().GetProperty("ReadOnly") != (PropertyInfo) null)
            control1.GetType().GetProperty("ReadOnly").SetValue((object) control1, (object) true, (object[]) null);
          else if (control1.GetType().GetProperty("Enabled") != (PropertyInfo) null)
            control1.GetType().GetProperty("Enabled").SetValue((object) control1, (object) false, (object[]) null);
        }
        else
          TPOClientUtils.SecureTabs(control1, tabName);
      }
    }

    public static bool sendEmailTriggerTemplate(
      TriggerEmailTemplate template,
      Sessions.Session session)
    {
      MailMessage message = new MailMessage();
      message.IsBodyHtml = true;
      try
      {
        message.From = new MailAddress(Session.UserInfo.Email, Session.UserInfo.FullName);
        message.Subject = FieldReplacementRegex.ReplaceLiteral(template.Subject, Session.LoanData);
        message.Body = FieldReplacementRegex.ReplaceLiteral(template.Body, Session.LoanData);
      }
      catch (Exception ex)
      {
        Tracing.Log(TPOClientUtils.sw, nameof (TPOClientUtils), TraceLevel.Info, "Error initializing MailMessage object for '" + template.Subject + "' -- sender address is invalid or subject/body are corrupt. (" + ex.Message + ")");
        return false;
      }
      List<string> source = new List<string>();
      source.AddRange((IEnumerable<string>) template.RecipientUsers);
      if (source.Count == 0)
      {
        Tracing.Log(TPOClientUtils.sw, nameof (TPOClientUtils), TraceLevel.Info, "Cannot send trigger email '" + template.Subject + "' -- no recipients found");
        return false;
      }
      bool flag = false;
      foreach (string address in source.ToList<string>())
      {
        if (address != null)
        {
          if ((address ?? "") == "")
          {
            Tracing.Log(TPOClientUtils.sw, nameof (TPOClientUtils), TraceLevel.Warning, "Cannot send trigger email '" + template.Subject + "' to user '" + address + "' -- no email address available.");
            source.Remove(address);
          }
          else
          {
            message.To.Add(new MailAddress(address, ""));
            flag = true;
          }
        }
      }
      if (!flag)
      {
        Tracing.Log(TPOClientUtils.sw, nameof (TPOClientUtils), TraceLevel.Info, "Cannot send trigger email '" + template.Subject + "' -- no valid recipients found");
        return false;
      }
      try
      {
        ContactUtils.SendMail(message);
        Tracing.Log(TPOClientUtils.sw, nameof (TPOClientUtils), TraceLevel.Info, "Successfully sent trigger email '" + template.Subject + "' to recipients: " + string.Join(", ", source.ToArray()));
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(TPOClientUtils.sw, nameof (TPOClientUtils), TraceLevel.Error, "Failed to send trigger email '" + template.Subject + "' to recipients " + string.Join(", ", source.ToArray()) + ": " + (object) ex);
        return false;
      }
    }
  }
}
