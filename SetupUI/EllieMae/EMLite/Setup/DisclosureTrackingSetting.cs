// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DisclosureTrackingSetting
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DisclosureTrackingSetting : SettingsUserControl
  {
    private bool suspendEvent;
    private List<FormItemInfo> formList;
    private List<FormItemInfo> formList2015;
    private IContainer components;
    private GroupContainer gcEvents;
    private CheckBox chkDiscloseeFolder;
    private CheckBox chkDisclosePrint;
    private GroupContainer gcFormList;
    private StandardIconButton siBtnNew;
    private StandardIconButton stBtnDelete;
    private CheckBox chkPrintPreview;
    private GridView gvForms;
    private CheckBox chkDiscloseManually;
    private GroupContainer gcTimeline;
    private CheckBox chkboxInitDiscDueDate;
    private Label label3;
    private CheckBox chkAutoPrintPreview;
    private CheckBox chkAutoDisclosePrint;
    private CheckBox chkboxRediscloseDueDate;
    private Label label6;
    private Panel pnlMiddle;
    private GroupContainer gcCopies;
    private CheckBox chkCopyeFolder;
    private CheckBox chkCopyPrint;
    private GroupContainer gcLECD;
    private Label label7;
    private ComboBox cmbTime;
    private TextBox txtMinute;
    private TextBox txtHour;
    private Label label8;
    private CheckBox chkboxGFEExpirationDate;
    private CheckBox chkboxEarliestClosingDate;
    private Label label5;
    private Label label4;
    private Label label2;
    private TextBox txtDaystoExpire;
    private Label label1;
    private Label label9;
    private TextBox txtLEDaystoExpire;
    private Label label12;
    private Label label10;
    private Label label11;
    private ComboBox cmbCCTimeZone;
    private GroupContainer gcCoc;
    private CheckBox chkCoCPriorDisclosure;
    private CheckBox chkboxRESPAOptInForSunHolidayClosings;
    private CheckBox chkboxRESPAOptInForFedHolidayClosings;

    public DisclosureTrackingSetting(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.initialPageValue();
      this.gcCoc.Visible = Session.StartupInfo.EnableCoC;
    }

    private void initialPageValue()
    {
      this.suspendEvent = true;
      IDictionary serverSettings1 = Session.ServerManager.GetServerSettings("Policies");
      DisclosureRecordSetting disclosureRecordSetting1 = DisclosureRecordSetting.DonotCreate;
      if (serverSettings1.Contains((object) "Policies.DisclosePrint"))
        disclosureRecordSetting1 = (DisclosureRecordSetting) serverSettings1[(object) "Policies.DisclosePrint"];
      switch (disclosureRecordSetting1)
      {
        case DisclosureRecordSetting.DonotCreate:
          this.chkDisclosePrint.Checked = false;
          this.chkAutoDisclosePrint.Checked = false;
          break;
        case DisclosureRecordSetting.PromptUser:
          this.chkDisclosePrint.Checked = true;
          goto default;
        default:
          this.chkAutoDisclosePrint.Checked = true;
          break;
      }
      this.chkDiscloseeFolder.Checked = serverSettings1.Contains((object) "Policies.DiscloseeFolder") && (bool) serverSettings1[(object) "Policies.DiscloseeFolder"];
      DisclosureRecordSetting disclosureRecordSetting2 = DisclosureRecordSetting.DonotCreate;
      if (serverSettings1.Contains((object) "Policies.DisclosePrintPreview"))
        disclosureRecordSetting2 = (DisclosureRecordSetting) serverSettings1[(object) "Policies.DisclosePrintPreview"];
      switch (disclosureRecordSetting2)
      {
        case DisclosureRecordSetting.DonotCreate:
          this.chkPrintPreview.Checked = false;
          this.chkAutoPrintPreview.Checked = false;
          break;
        case DisclosureRecordSetting.PromptUser:
          this.chkPrintPreview.Checked = true;
          goto default;
        default:
          this.chkAutoPrintPreview.Checked = true;
          break;
      }
      this.chkDiscloseManually.Checked = serverSettings1.Contains((object) "Policies.DiscloseManually") && (bool) serverSettings1[(object) "Policies.DiscloseManually"];
      this.chkCopyPrint.Checked = serverSettings1.Contains((object) "Policies.SaveDisclosuresPrintMenu") && (bool) serverSettings1[(object) "Policies.SaveDisclosuresPrintMenu"];
      this.chkCopyeFolder.Checked = serverSettings1.Contains((object) "Policies.SaveDisclosureseFolder") && (bool) serverSettings1[(object) "Policies.SaveDisclosureseFolder"];
      this.chkCoCPriorDisclosure.Checked = serverSettings1.Contains((object) "Policies.RequireCoCPriorDisclosure") && (bool) serverSettings1[(object) "Policies.RequireCoCPriorDisclosure"];
      IDictionary serverSettings2 = Session.ServerManager.GetServerSettings("Compliance");
      this.chkboxInitDiscDueDate.Checked = serverSettings2.Contains((object) "Compliance.CountDay0ForDisclosureDate") && (bool) serverSettings2[(object) "Compliance.CountDay0ForDisclosureDate"];
      this.chkboxEarliestClosingDate.Checked = serverSettings2.Contains((object) "Compliance.CountDay0ForClosingDate") && (bool) serverSettings2[(object) "Compliance.CountDay0ForClosingDate"];
      this.chkboxGFEExpirationDate.Checked = serverSettings2.Contains((object) "Compliance.CountDay0ForGFEExpirationDate") && (bool) serverSettings2[(object) "Compliance.CountDay0ForGFEExpirationDate"];
      this.chkboxRediscloseDueDate.Checked = serverSettings2.Contains((object) "Compliance.CountDay0ForRediscloseDueDate") && (bool) serverSettings2[(object) "Compliance.CountDay0ForRediscloseDueDate"];
      this.chkboxRESPAOptInForSunHolidayClosings.Checked = serverSettings2.Contains((object) "Compliance.CountDay0ForRESPASunHoliday") && (bool) serverSettings2[(object) "Compliance.CountDay0ForRESPASunHoliday"];
      this.chkboxRESPAOptInForFedHolidayClosings.Checked = serverSettings2.Contains((object) "Compliance.CountDay0ForRESPAFedHoliday") && (bool) serverSettings2[(object) "Compliance.CountDay0ForRESPAFedHoliday"];
      string companySetting1 = Session.ConfigurationManager.GetCompanySetting("Policies", "GFEDaysToExpire");
      if (Utils.IsInt((object) companySetting1))
        this.txtDaystoExpire.Text = companySetting1;
      else
        this.txtDaystoExpire.Text = "10";
      string companySetting2 = Session.ConfigurationManager.GetCompanySetting("Policies", "LEDaysToExpire");
      if (Utils.IsInt((object) companySetting2))
        this.txtLEDaystoExpire.Text = companySetting2;
      else
        this.txtLEDaystoExpire.Text = "10";
      string str = (string) serverSettings1[(object) "Policies.CDExpirationTime"];
      if (str != null)
      {
        string[] strArray1 = str.Split(' ');
        string[] strArray2 = strArray1[0].Split(':');
        this.txtHour.Text = strArray2[0];
        this.txtMinute.Text = strArray2[1];
        this.cmbTime.Text = strArray1[1];
      }
      else
      {
        this.txtHour.Text = "5";
        this.txtMinute.Text = "00";
        this.cmbTime.Text = "PM";
      }
      this.populateTimeZoneDropDown();
      this.cmbCCTimeZone.SelectedValue = (object) (string) serverSettings1[(object) "Policies.CDExpirationTimeZone"];
      this.formList = this.converToFormItemInfo(Session.ConfigurationManager.GetAllDisclosureFroms());
      this.formList2015 = this.converToFormItemInfo(Session.ConfigurationManager.GetAllDisclosure2015Forms());
      this.populateFormList();
      this.setDirtyFlag(false);
      this.suspendEvent = false;
    }

    private void populateTimeZoneDropDown()
    {
      this.cmbCCTimeZone.DataSource = (object) null;
      this.cmbCCTimeZone.Items.Clear();
      this.cmbCCTimeZone.DataSource = (object) new List<object>()
      {
        (object) new
        {
          Value = "(UTC -10:00) Hawaii Time",
          Text = "(UTC -10:00) Hawaii Time"
        },
        (object) new
        {
          Value = "(UTC -09:00) Alaska Time",
          Text = "(UTC -08:00 / 09:00) Alaska Time"
        },
        (object) new
        {
          Value = "(UTC -08:00) Pacific Time",
          Text = "(UTC -07:00 / 08:00) Pacific Time"
        },
        (object) new
        {
          Value = "(UTC -07:00) Arizona Time",
          Text = "(UTC -07:00) Arizona Time"
        },
        (object) new
        {
          Value = "(UTC -07:00) Mountain Time",
          Text = "(UTC -06:00 / 07:00) Mountain Time"
        },
        (object) new
        {
          Value = "(UTC -06:00) Central Time",
          Text = "(UTC -05:00 / 06:00) Central Time"
        },
        (object) new
        {
          Value = "(UTC -05:00) Eastern Time",
          Text = "(UTC -04:00 / 05:00) Eastern Time"
        }
      };
      this.cmbCCTimeZone.ValueMember = "Value";
      this.cmbCCTimeZone.DisplayMember = "Text";
    }

    private void populateFormList()
    {
      this.gvForms.Items.Clear();
      foreach (FormItemInfo form in this.formList)
        this.gvForms.Items.Add(new GVItem(form.FormName)
        {
          Tag = (object) form
        });
      foreach (FormItemInfo formItemInfo in this.formList2015)
      {
        if (!this.formList.Contains(formItemInfo))
          this.gvForms.Items.Add(new GVItem(formItemInfo.FormName)
          {
            Tag = (object) formItemInfo
          });
      }
      this.gcFormList.Text = "Select the disclosures that will be recorded in Disclosure Tracking (" + (object) this.gvForms.Items.Count + ")";
    }

    public override void Reset() => this.initialPageValue();

    public override void Save()
    {
      if (!this.IsDirty || !this.validateValue())
        return;
      Hashtable settings = new Hashtable();
      if (this.chkDisclosePrint.Checked && this.chkAutoDisclosePrint.Checked)
        settings.Add((object) "Policies.DisclosePrint", (object) DisclosureRecordSetting.PromptUser);
      else if (this.chkAutoDisclosePrint.Checked)
        settings.Add((object) "Policies.DisclosePrint", (object) DisclosureRecordSetting.AutoCreate);
      else
        settings.Add((object) "Policies.DisclosePrint", (object) DisclosureRecordSetting.DonotCreate);
      if (this.chkDiscloseeFolder.Checked)
        settings.Add((object) "Policies.DiscloseeFolder", (object) true);
      else
        settings.Add((object) "Policies.DiscloseeFolder", (object) false);
      if (this.chkPrintPreview.Checked && this.chkAutoPrintPreview.Checked)
        settings.Add((object) "Policies.DisclosePrintPreview", (object) DisclosureRecordSetting.PromptUser);
      else if (this.chkAutoPrintPreview.Checked)
        settings.Add((object) "Policies.DisclosePrintPreview", (object) DisclosureRecordSetting.AutoCreate);
      else
        settings.Add((object) "Policies.DisclosePrintPreview", (object) DisclosureRecordSetting.DonotCreate);
      if (this.chkDiscloseManually.Checked)
        settings.Add((object) "Policies.DiscloseManually", (object) true);
      else
        settings.Add((object) "Policies.DiscloseManually", (object) false);
      if (this.chkCopyPrint.Checked)
        settings.Add((object) "Policies.SaveDisclosuresPrintMenu", (object) true);
      else
        settings.Add((object) "Policies.SaveDisclosuresPrintMenu", (object) false);
      if (this.chkCopyeFolder.Checked)
        settings.Add((object) "Policies.SaveDisclosureseFolder", (object) true);
      else
        settings.Add((object) "Policies.SaveDisclosureseFolder", (object) false);
      settings.Add((object) "Policies.GFEDaysToExpire", (object) this.txtDaystoExpire.Text);
      settings.Add((object) "Policies.LEDaysToExpire", (object) this.txtLEDaystoExpire.Text);
      settings.Add((object) "Policies.CDExpirationTime", (object) (this.txtHour.Text + ":" + this.txtMinute.Text + " " + this.cmbTime.SelectedItem.ToString()));
      settings.Add((object) "Policies.CDExpirationTimeZone", (object) this.cmbCCTimeZone.SelectedValue.ToString());
      if (this.chkboxInitDiscDueDate.Checked)
        settings.Add((object) "Compliance.CountDay0ForDisclosureDate", (object) true);
      else
        settings.Add((object) "Compliance.CountDay0ForDisclosureDate", (object) false);
      if (this.chkboxEarliestClosingDate.Checked)
        settings.Add((object) "Compliance.CountDay0ForClosingDate", (object) true);
      else
        settings.Add((object) "Compliance.CountDay0ForClosingDate", (object) false);
      if (this.chkboxGFEExpirationDate.Checked)
        settings.Add((object) "Compliance.CountDay0ForGFEExpirationDate", (object) true);
      else
        settings.Add((object) "Compliance.CountDay0ForGFEExpirationDate", (object) false);
      if (this.chkboxRediscloseDueDate.Checked)
        settings.Add((object) "Compliance.CountDay0ForRediscloseDueDate", (object) true);
      else
        settings.Add((object) "Compliance.CountDay0ForRediscloseDueDate", (object) false);
      if (this.chkCoCPriorDisclosure.Checked)
        settings.Add((object) "Policies.RequireCoCPriorDisclosure", (object) true);
      else
        settings.Add((object) "Policies.RequireCoCPriorDisclosure", (object) false);
      if (this.chkboxRESPAOptInForSunHolidayClosings.Checked)
        settings.Add((object) "Compliance.CountDay0ForRESPASunHoliday", (object) true);
      else
        settings.Add((object) "Compliance.CountDay0ForRESPASunHoliday", (object) false);
      if (this.chkboxRESPAOptInForFedHolidayClosings.Checked)
        settings.Add((object) "Compliance.CountDay0ForRESPAFedHoliday", (object) true);
      else
        settings.Add((object) "Compliance.CountDay0ForRESPAFedHoliday", (object) false);
      Session.ServerManager.UpdateServerSettings((IDictionary) settings, AclFeature.SettingsTab_DisclosureTrackingSettings);
      Session.ConfigurationManager.UpdateDisclosureTrackingFroms(this.convertToDisclosureFormSetting());
      Session.ConfigurationManager.UpdateDisclosureTracking2015Forms(this.convertToDisclosureForm2015Setting());
      this.setDirtyFlag(false);
    }

    private void Data_Changed(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
      if (sender == this.txtDaystoExpire)
      {
        bool needsUpdate = false;
        int newCursorPos = 0;
        string str = Utils.FormatInput(this.txtDaystoExpire.Text, FieldFormat.INTEGER, ref needsUpdate, this.txtDaystoExpire.SelectionStart, ref newCursorPos);
        if (needsUpdate)
        {
          this.txtDaystoExpire.Text = str;
          this.txtDaystoExpire.SelectionStart = newCursorPos;
        }
      }
      if (sender == this.txtLEDaystoExpire)
      {
        bool needsUpdate = false;
        int newCursorPos = 0;
        string str = Utils.FormatInput(this.txtLEDaystoExpire.Text, FieldFormat.INTEGER, ref needsUpdate, this.txtLEDaystoExpire.SelectionStart, ref newCursorPos);
        if (needsUpdate)
        {
          this.txtLEDaystoExpire.Text = str;
          this.txtLEDaystoExpire.SelectionStart = newCursorPos;
        }
      }
      this.suspendEvent = false;
    }

    private void siBtnNew_Click(object sender, EventArgs e)
    {
      List<FormItemInfo> formList1 = new List<FormItemInfo>((IEnumerable<FormItemInfo>) this.formList);
      foreach (FormItemInfo formItemInfo in this.formList2015)
      {
        if (!formList1.Contains(formItemInfo))
          formList1.Add(formItemInfo);
      }
      using (DisclosureTrackingForms disclosureTrackingForms = new DisclosureTrackingForms(formList1))
      {
        if (DialogResult.OK != disclosureTrackingForms.ShowDialog((IWin32Window) this))
          return;
        List<FormItemInfo> formList2 = disclosureTrackingForms.FormList;
        List<FormItemInfo> formItemInfoList = new List<FormItemInfo>((IEnumerable<FormItemInfo>) formList2.ToArray());
        foreach (FormItemInfo formItemInfo1 in formList1)
        {
          if (DisclosureTrackingConsts.NewGFEFormNames.Contains(formItemInfo1.FormName) || DisclosureTrackingConsts.TILFormNames.Contains(formItemInfo1.FormName) || DisclosureTrackingConsts.LEFormNames.Contains(formItemInfo1.FormName) || DisclosureTrackingConsts.CDFormNames.Contains(formItemInfo1.FormName) || DisclosureTrackingConsts.SSPLFormNames.Contains(formItemInfo1.FormName) || DisclosureTrackingConsts.SafeHarbor2015.Contains(formItemInfo1.FormName))
          {
            bool flag = false;
            foreach (FormItemInfo formItemInfo2 in formList2)
            {
              if (formItemInfo2.FormName == formItemInfo1.FormName && formItemInfo2.FormType == OutputFormType.PdfForms)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              formItemInfoList.Add(formItemInfo1);
              int num = (int) Utils.Dialog((IWin32Window) this, "Form '" + formItemInfo1.FormName + "' cannot be removed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
        }
        this.formList.Clear();
        this.formList2015.Clear();
        foreach (FormItemInfo formItemInfo in formItemInfoList)
        {
          if (DisclosureTrackingConsts.NewGFEFormNames.Contains(formItemInfo.FormName) || DisclosureTrackingConsts.TILFormNames.Contains(formItemInfo.FormName))
            this.formList.Add(formItemInfo);
          else if (DisclosureTrackingConsts.NewSafeHarborFormNames.Contains(formItemInfo.FormName))
          {
            this.formList.Add(formItemInfo);
            this.formList2015.Add(formItemInfo);
          }
          else if (DisclosureTrackingConsts.LEFormNames.Contains(formItemInfo.FormName) || DisclosureTrackingConsts.CDFormNames.Contains(formItemInfo.FormName) || DisclosureTrackingConsts.SSPLFormNames.Contains(formItemInfo.FormName) || DisclosureTrackingConsts.SafeHarbor2015.Contains(formItemInfo.FormName))
          {
            this.formList2015.Add(formItemInfo);
          }
          else
          {
            this.formList.Add(formItemInfo);
            this.formList2015.Add(formItemInfo);
          }
        }
        this.populateFormList();
        this.setDirtyFlag(true);
      }
    }

    private List<FormItemInfo> converToFormItemInfo(
      Dictionary<string, DisclosureTrackingFormItem.FormType> formList)
    {
      List<FormItemInfo> formItemInfo1 = new List<FormItemInfo>();
      foreach (string key in formList.Keys)
      {
        FormItemInfo formItemInfo2 = !FileSystemEntry.IsValidPath(key, Session.UserID) ? new FormItemInfo("Print", OutputFormNameMap.GetFormKeyToName(key, Session.DefaultInstance), OutputFormType.PdfForms) : new FormItemInfo("Print", key, OutputFormType.CustomLetters);
        formItemInfo1.Add(formItemInfo2);
      }
      return formItemInfo1;
    }

    private Dictionary<string, DisclosureTrackingFormItem.FormType> convertToDisclosureFormSetting()
    {
      Dictionary<string, DisclosureTrackingFormItem.FormType> disclosureFormSetting = new Dictionary<string, DisclosureTrackingFormItem.FormType>();
      foreach (FormItemInfo form in this.formList)
      {
        if (form.FormType == OutputFormType.PdfForms)
          disclosureFormSetting.Add(form.FormName, DisclosureTrackingFormItem.FormType.StandardForm);
        else
          disclosureFormSetting.Add(form.FormName, DisclosureTrackingFormItem.FormType.CustomForm);
      }
      return disclosureFormSetting;
    }

    private Dictionary<string, DisclosureTrackingFormItem.FormType> convertToDisclosureForm2015Setting()
    {
      Dictionary<string, DisclosureTrackingFormItem.FormType> disclosureForm2015Setting = new Dictionary<string, DisclosureTrackingFormItem.FormType>();
      foreach (FormItemInfo formItemInfo in this.formList2015)
      {
        if (formItemInfo.FormType == OutputFormType.PdfForms)
          disclosureForm2015Setting.Add(formItemInfo.FormName, DisclosureTrackingFormItem.FormType.StandardForm);
        else
          disclosureForm2015Setting.Add(formItemInfo.FormName, DisclosureTrackingFormItem.FormType.CustomForm);
      }
      return disclosureForm2015Setting;
    }

    private void stBtnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected ouput forms?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
        return;
      foreach (GVItem selectedItem in this.gvForms.SelectedItems)
      {
        FormItemInfo tag = (FormItemInfo) selectedItem.Tag;
        if (DisclosureTrackingConsts.NewGFEFormNames.Contains(tag.FormName) || DisclosureTrackingConsts.TILFormNames.Contains(tag.FormName) || DisclosureTrackingConsts.LEFormNames.Contains(tag.FormName) || DisclosureTrackingConsts.CDFormNames.Contains(tag.FormName) || DisclosureTrackingConsts.SSPLFormNames.Contains(tag.FormName) || DisclosureTrackingConsts.SafeHarbor2015.Contains(tag.FormName))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Form '" + tag.FormName + "' cannot be removed.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          this.formList.Remove(tag);
          this.formList2015.Remove(tag);
        }
      }
      this.populateFormList();
      this.setDirtyFlag(true);
    }

    private void gvForms_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvForms.SelectedItems.Count > 0)
        this.stBtnDelete.Enabled = true;
      else
        this.stBtnDelete.Enabled = false;
    }

    private bool validateValue()
    {
      if (!Utils.IsInt((object) this.txtDaystoExpire.Text) || Utils.IsInt((object) this.txtDaystoExpire.Text) && Utils.ParseInt((object) this.txtDaystoExpire.Text) > 999)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a value less than 1000.");
        this.txtDaystoExpire.Focus();
        return false;
      }
      if (Utils.ParseInt((object) this.txtDaystoExpire.Text) < 10)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "GFE Expiration Date: Minimum 10 days are required by law.");
        this.txtDaystoExpire.Focus();
        return false;
      }
      if (!Utils.IsInt((object) this.txtLEDaystoExpire.Text) || Utils.IsInt((object) this.txtLEDaystoExpire.Text) && Utils.ParseInt((object) this.txtLEDaystoExpire.Text) > 999)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide a value less than 1000.");
        this.txtLEDaystoExpire.Focus();
        return false;
      }
      if (Utils.ParseInt((object) this.txtLEDaystoExpire.Text) < 10)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "LE Expiration Date: Minimum 10 days are required by law.");
        this.txtLEDaystoExpire.Focus();
        return false;
      }
      if (this.txtHour.Text.Trim() == "" || this.txtMinute.Text.Trim() == "" || this.cmbTime.SelectedItem.ToString().Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Expiration time is required.");
        this.txtHour.Focus();
        return false;
      }
      if (this.cmbCCTimeZone.SelectedItem.ToString().Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Expiration timezone is required.");
        this.cmbCCTimeZone.Focus();
        return false;
      }
      if (!this.IsWithin(Convert.ToInt32(this.txtHour.Text), 1, 12))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Expiration hour needs to be between 1 and 12.");
        this.txtHour.Focus();
        return false;
      }
      if (this.IsWithin(Convert.ToInt32(this.txtMinute.Text), 0, 59))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Expiration minute needs to be between 0 and 59.");
      this.txtMinute.Focus();
      return false;
    }

    private void chkAutoDisclosePrint_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.chkDisclosePrint.Checked = this.chkAutoDisclosePrint.Checked;
      this.Data_Changed((object) this.chkAutoDisclosePrint, (EventArgs) null);
    }

    private void chkAutoPrintPreview_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.chkPrintPreview.Checked = this.chkAutoPrintPreview.Checked;
      this.Data_Changed((object) this.chkAutoPrintPreview, (EventArgs) null);
    }

    private void txtHour_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        if (this.txtHour.Text.Length < 2)
          return;
        e.Handled = true;
      }
    }

    private void txtMinute_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
      {
        this.setDirtyFlag(true);
      }
      else
      {
        if (!char.IsNumber(e.KeyChar))
          e.Handled = true;
        if (this.txtMinute.Text.Length < 2)
          return;
        e.Handled = true;
      }
    }

    private void txtMinute_Leave(object sender, EventArgs e)
    {
      if (this.txtMinute.Text.Length != 1)
        return;
      this.txtMinute.Text = "0" + this.txtMinute.Text;
    }

    private void pnlMiddle_ClientSizeChanged(object sender, EventArgs e)
    {
      this.gcEvents.Width = this.pnlMiddle.ClientSize.Width / 2;
    }

    private bool IsWithin(int value, int minimum, int maximum)
    {
      return value >= minimum && value <= maximum;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.gcEvents = new GroupContainer();
      this.chkAutoPrintPreview = new CheckBox();
      this.chkAutoDisclosePrint = new CheckBox();
      this.chkDiscloseManually = new CheckBox();
      this.chkPrintPreview = new CheckBox();
      this.chkDiscloseeFolder = new CheckBox();
      this.chkDisclosePrint = new CheckBox();
      this.gcFormList = new GroupContainer();
      this.gvForms = new GridView();
      this.siBtnNew = new StandardIconButton();
      this.stBtnDelete = new StandardIconButton();
      this.gcTimeline = new GroupContainer();
      this.chkboxRESPAOptInForFedHolidayClosings = new CheckBox();
      this.cmbCCTimeZone = new ComboBox();
      this.label10 = new Label();
      this.label11 = new Label();
      this.chkboxRESPAOptInForSunHolidayClosings = new CheckBox();
      this.label9 = new Label();
      this.txtLEDaystoExpire = new TextBox();
      this.label12 = new Label();
      this.chkboxRediscloseDueDate = new CheckBox();
      this.label6 = new Label();
      this.chkboxInitDiscDueDate = new CheckBox();
      this.label3 = new Label();
      this.label8 = new Label();
      this.cmbTime = new ComboBox();
      this.label7 = new Label();
      this.txtMinute = new TextBox();
      this.txtHour = new TextBox();
      this.pnlMiddle = new Panel();
      this.gcCopies = new GroupContainer();
      this.chkCopyeFolder = new CheckBox();
      this.chkCopyPrint = new CheckBox();
      this.gcLECD = new GroupContainer();
      this.chkboxGFEExpirationDate = new CheckBox();
      this.chkboxEarliestClosingDate = new CheckBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.txtDaystoExpire = new TextBox();
      this.label1 = new Label();
      this.gcCoc = new GroupContainer();
      this.chkCoCPriorDisclosure = new CheckBox();
      this.gcEvents.SuspendLayout();
      this.gcFormList.SuspendLayout();
      ((ISupportInitialize) this.siBtnNew).BeginInit();
      ((ISupportInitialize) this.stBtnDelete).BeginInit();
      this.gcTimeline.SuspendLayout();
      this.pnlMiddle.SuspendLayout();
      this.gcCopies.SuspendLayout();
      this.gcLECD.SuspendLayout();
      this.gcCoc.SuspendLayout();
      this.SuspendLayout();
      this.gcEvents.Borders = AnchorStyles.Top | AnchorStyles.Left;
      this.gcEvents.Controls.Add((Control) this.chkAutoPrintPreview);
      this.gcEvents.Controls.Add((Control) this.chkAutoDisclosePrint);
      this.gcEvents.Controls.Add((Control) this.chkDiscloseManually);
      this.gcEvents.Controls.Add((Control) this.chkPrintPreview);
      this.gcEvents.Controls.Add((Control) this.chkDiscloseeFolder);
      this.gcEvents.Controls.Add((Control) this.chkDisclosePrint);
      this.gcEvents.Dock = DockStyle.Left;
      this.gcEvents.HeaderForeColor = SystemColors.ControlText;
      this.gcEvents.Location = new Point(0, 0);
      this.gcEvents.Name = "gcEvents";
      this.gcEvents.Size = new Size(348, 141);
      this.gcEvents.TabIndex = 0;
      this.gcEvents.Text = "Select the events that will trigger recording of disclosures";
      this.chkAutoPrintPreview.AutoSize = true;
      this.chkAutoPrintPreview.Location = new Point(11, 67);
      this.chkAutoPrintPreview.Name = "chkAutoPrintPreview";
      this.chkAutoPrintPreview.Size = new Size(266, 18);
      this.chkAutoPrintPreview.TabIndex = 2;
      this.chkAutoPrintPreview.Text = "Create a record when disclosures are previewed";
      this.chkAutoPrintPreview.UseVisualStyleBackColor = true;
      this.chkAutoPrintPreview.CheckedChanged += new EventHandler(this.chkAutoPrintPreview_CheckedChanged);
      this.chkAutoDisclosePrint.AutoSize = true;
      this.chkAutoDisclosePrint.Location = new Point(11, 31);
      this.chkAutoDisclosePrint.Name = "chkAutoDisclosePrint";
      this.chkAutoDisclosePrint.Size = new Size(247, 18);
      this.chkAutoDisclosePrint.TabIndex = 0;
      this.chkAutoDisclosePrint.Text = "Create a record when disclosures are printed";
      this.chkAutoDisclosePrint.UseVisualStyleBackColor = true;
      this.chkAutoDisclosePrint.CheckedChanged += new EventHandler(this.chkAutoDisclosePrint_CheckedChanged);
      this.chkDiscloseManually.AutoSize = true;
      this.chkDiscloseManually.Location = new Point(11, 121);
      this.chkDiscloseManually.Name = "chkDiscloseManually";
      this.chkDiscloseManually.Size = new Size(249, 18);
      this.chkDiscloseManually.TabIndex = 5;
      this.chkDiscloseManually.Text = "Users can manually create disclosure records";
      this.chkDiscloseManually.UseVisualStyleBackColor = true;
      this.chkDiscloseManually.CheckedChanged += new EventHandler(this.Data_Changed);
      this.chkPrintPreview.AutoSize = true;
      this.chkPrintPreview.Location = new Point(27, 85);
      this.chkPrintPreview.Name = "chkPrintPreview";
      this.chkPrintPreview.Size = new Size(220, 18);
      this.chkPrintPreview.TabIndex = 3;
      this.chkPrintPreview.Text = "Prompt users before a record is created";
      this.chkPrintPreview.UseVisualStyleBackColor = true;
      this.chkPrintPreview.CheckedChanged += new EventHandler(this.Data_Changed);
      this.chkDiscloseeFolder.AutoSize = true;
      this.chkDiscloseeFolder.Location = new Point(11, 103);
      this.chkDiscloseeFolder.Name = "chkDiscloseeFolder";
      this.chkDiscloseeFolder.Size = new Size(321, 18);
      this.chkDiscloseeFolder.TabIndex = 4;
      this.chkDiscloseeFolder.Text = "Create a record when disclosures are sent using the eFolder";
      this.chkDiscloseeFolder.UseVisualStyleBackColor = true;
      this.chkDiscloseeFolder.CheckedChanged += new EventHandler(this.Data_Changed);
      this.chkDisclosePrint.AutoSize = true;
      this.chkDisclosePrint.Location = new Point(27, 49);
      this.chkDisclosePrint.Name = "chkDisclosePrint";
      this.chkDisclosePrint.Size = new Size(220, 18);
      this.chkDisclosePrint.TabIndex = 1;
      this.chkDisclosePrint.Text = "Prompt users before a record is created";
      this.chkDisclosePrint.UseVisualStyleBackColor = true;
      this.chkDisclosePrint.CheckedChanged += new EventHandler(this.Data_Changed);
      this.gcFormList.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcFormList.Controls.Add((Control) this.gvForms);
      this.gcFormList.Controls.Add((Control) this.siBtnNew);
      this.gcFormList.Controls.Add((Control) this.stBtnDelete);
      this.gcFormList.Dock = DockStyle.Fill;
      this.gcFormList.HeaderForeColor = SystemColors.ControlText;
      this.gcFormList.Location = new Point(0, 0);
      this.gcFormList.Name = "gcFormList";
      this.gcFormList.Size = new Size(742, 121);
      this.gcFormList.TabIndex = 0;
      this.gcFormList.Text = "Select the disclosures that will be recorded in Disclosure Tracking";
      this.gvForms.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Form Name";
      gvColumn.Width = 740;
      this.gvForms.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvForms.Dock = DockStyle.Fill;
      this.gvForms.HeaderHeight = 0;
      this.gvForms.HeaderVisible = false;
      this.gvForms.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvForms.Location = new Point(1, 26);
      this.gvForms.Name = "gvForms";
      this.gvForms.Size = new Size(740, 95);
      this.gvForms.TabIndex = 0;
      this.gvForms.SelectedIndexChanged += new EventHandler(this.gvForms_SelectedIndexChanged);
      this.siBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.siBtnNew.BackColor = Color.Transparent;
      this.siBtnNew.Location = new Point(700, 4);
      this.siBtnNew.MouseDownImage = (Image) null;
      this.siBtnNew.Name = "siBtnNew";
      this.siBtnNew.Size = new Size(16, 17);
      this.siBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.siBtnNew.TabIndex = 1;
      this.siBtnNew.TabStop = false;
      this.siBtnNew.Click += new EventHandler(this.siBtnNew_Click);
      this.stBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stBtnDelete.BackColor = Color.Transparent;
      this.stBtnDelete.Location = new Point(722, 4);
      this.stBtnDelete.MouseDownImage = (Image) null;
      this.stBtnDelete.Name = "stBtnDelete";
      this.stBtnDelete.Size = new Size(16, 17);
      this.stBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stBtnDelete.TabIndex = 0;
      this.stBtnDelete.TabStop = false;
      this.stBtnDelete.Click += new EventHandler(this.stBtnDelete_Click);
      this.gcTimeline.Controls.Add((Control) this.chkboxRESPAOptInForFedHolidayClosings);
      this.gcTimeline.Controls.Add((Control) this.cmbCCTimeZone);
      this.gcTimeline.Controls.Add((Control) this.label10);
      this.gcTimeline.Controls.Add((Control) this.label11);
      this.gcTimeline.Controls.Add((Control) this.chkboxRESPAOptInForSunHolidayClosings);
      this.gcTimeline.Controls.Add((Control) this.label9);
      this.gcTimeline.Controls.Add((Control) this.txtLEDaystoExpire);
      this.gcTimeline.Controls.Add((Control) this.label12);
      this.gcTimeline.Controls.Add((Control) this.chkboxRediscloseDueDate);
      this.gcTimeline.Controls.Add((Control) this.label6);
      this.gcTimeline.Controls.Add((Control) this.chkboxInitDiscDueDate);
      this.gcTimeline.Controls.Add((Control) this.label3);
      this.gcTimeline.Controls.Add((Control) this.label8);
      this.gcTimeline.Controls.Add((Control) this.cmbTime);
      this.gcTimeline.Controls.Add((Control) this.label7);
      this.gcTimeline.Controls.Add((Control) this.txtMinute);
      this.gcTimeline.Controls.Add((Control) this.txtHour);
      this.gcTimeline.Dock = DockStyle.Bottom;
      this.gcTimeline.HeaderForeColor = SystemColors.ControlText;
      this.gcTimeline.Location = new Point(0, 262);
      this.gcTimeline.Name = "gcTimeline";
      this.gcTimeline.Size = new Size(742, 210);
      this.gcTimeline.TabIndex = 2;
      this.gcTimeline.Text = "Compliance Timeline Calculation for RESPA-TILA";
      this.chkboxRESPAOptInForFedHolidayClosings.AutoSize = true;
      this.chkboxRESPAOptInForFedHolidayClosings.Location = new Point(190, 180);
      this.chkboxRESPAOptInForFedHolidayClosings.Name = "chkboxRESPAOptInForFedHolidayClosings";
      this.chkboxRESPAOptInForFedHolidayClosings.Size = new Size(191, 18);
      this.chkboxRESPAOptInForFedHolidayClosings.TabIndex = 36;
      this.chkboxRESPAOptInForFedHolidayClosings.Text = "Opt in for Federal Holiday closings";
      this.chkboxRESPAOptInForFedHolidayClosings.UseVisualStyleBackColor = true;
      this.chkboxRESPAOptInForFedHolidayClosings.CheckedChanged += new EventHandler(this.Data_Changed);
      this.cmbCCTimeZone.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbCCTimeZone.FormattingEnabled = true;
      this.cmbCCTimeZone.Location = new Point(190, (int) sbyte.MaxValue);
      this.cmbCCTimeZone.Name = "cmbCCTimeZone";
      this.cmbCCTimeZone.Size = new Size(200, 22);
      this.cmbCCTimeZone.TabIndex = 30;
      this.cmbCCTimeZone.SelectedIndexChanged += new EventHandler(this.Data_Changed);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(8, 130);
      this.label10.Name = "label10";
      this.label10.Size = new Size(176, 14);
      this.label10.TabIndex = 29;
      this.label10.Text = "Closing Costs Expiration Time Zone";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(8, 163);
      this.label11.Name = "label11";
      this.label11.Size = new Size(105, 14);
      this.label11.TabIndex = 31;
      this.label11.Text = "Earliest Closing Date";
      this.chkboxRESPAOptInForSunHolidayClosings.AutoSize = true;
      this.chkboxRESPAOptInForSunHolidayClosings.Location = new Point(190, 160);
      this.chkboxRESPAOptInForSunHolidayClosings.Name = "chkboxRESPAOptInForSunHolidayClosings";
      this.chkboxRESPAOptInForSunHolidayClosings.Size = new Size(154, 18);
      this.chkboxRESPAOptInForSunHolidayClosings.TabIndex = 34;
      this.chkboxRESPAOptInForSunHolidayClosings.Text = "Opt in for Sunday closings";
      this.chkboxRESPAOptInForSunHolidayClosings.UseVisualStyleBackColor = true;
      this.chkboxRESPAOptInForSunHolidayClosings.CheckedChanged += new EventHandler(this.Data_Changed);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(320, 72);
      this.label9.Name = "label9";
      this.label9.Size = new Size(222, 14);
      this.label9.TabIndex = 28;
      this.label9.Text = "days (minimum 10 days are required by law)";
      this.txtLEDaystoExpire.Location = new Point(258, 70);
      this.txtLEDaystoExpire.Name = "txtLEDaystoExpire";
      this.txtLEDaystoExpire.Size = new Size(56, 20);
      this.txtLEDaystoExpire.TabIndex = 27;
      this.txtLEDaystoExpire.TextAlign = HorizontalAlignment.Right;
      this.txtLEDaystoExpire.TextChanged += new EventHandler(this.Data_Changed);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(187, 72);
      this.label12.Name = "label12";
      this.label12.Size = new Size(69, 14);
      this.label12.TabIndex = 26;
      this.label12.Text = "LE Expires in";
      this.chkboxRediscloseDueDate.AutoSize = true;
      this.chkboxRediscloseDueDate.Location = new Point(190, 52);
      this.chkboxRediscloseDueDate.Name = "chkboxRediscloseDueDate";
      this.chkboxRediscloseDueDate.Size = new Size(440, 18);
      this.chkboxRediscloseDueDate.TabIndex = 3;
      this.chkboxRediscloseDueDate.Text = "Include changed circumstance dates (fields 3165 and CD1.X62) in the date calculation";
      this.chkboxRediscloseDueDate.UseVisualStyleBackColor = true;
      this.chkboxRediscloseDueDate.CheckedChanged += new EventHandler(this.Data_Changed);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 55);
      this.label6.Name = "label6";
      this.label6.Size = new Size(117, 14);
      this.label6.TabIndex = 2;
      this.label6.Text = "Redisclosure Due Date";
      this.chkboxInitDiscDueDate.AutoSize = true;
      this.chkboxInitDiscDueDate.Location = new Point(190, 32);
      this.chkboxInitDiscDueDate.Name = "chkboxInitDiscDueDate";
      this.chkboxInitDiscDueDate.Size = new Size(303, 18);
      this.chkboxInitDiscDueDate.TabIndex = 1;
      this.chkboxInitDiscDueDate.Text = "Include application date (field 3142) in the date calculation";
      this.chkboxInitDiscDueDate.UseVisualStyleBackColor = true;
      this.chkboxInitDiscDueDate.CheckedChanged += new EventHandler(this.Data_Changed);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 35);
      this.label3.Name = "label3";
      this.label3.Size = new Size(131, 14);
      this.label3.TabIndex = 0;
      this.label3.Text = "Initial Disclosure Due Date";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(224, 98);
      this.label8.Name = "label8";
      this.label8.Size = new Size(10, 14);
      this.label8.TabIndex = 18;
      this.label8.Text = ":";
      this.cmbTime.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbTime.FormattingEnabled = true;
      this.cmbTime.Items.AddRange(new object[2]
      {
        (object) "AM",
        (object) "PM"
      });
      this.cmbTime.Location = new Point(286, 97);
      this.cmbTime.Name = "cmbTime";
      this.cmbTime.Size = new Size(42, 22);
      this.cmbTime.TabIndex = 17;
      this.cmbTime.SelectedIndexChanged += new EventHandler(this.Data_Changed);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 100);
      this.label7.Name = "label7";
      this.label7.Size = new Size(148, 14);
      this.label7.TabIndex = 14;
      this.label7.Text = "Closing Costs Expiration Time";
      this.txtMinute.Location = new Point(239, 97);
      this.txtMinute.Name = "txtMinute";
      this.txtMinute.Size = new Size(28, 20);
      this.txtMinute.TabIndex = 16;
      this.txtMinute.TextChanged += new EventHandler(this.Data_Changed);
      this.txtMinute.KeyPress += new KeyPressEventHandler(this.txtMinute_KeyPress);
      this.txtMinute.Leave += new EventHandler(this.txtMinute_Leave);
      this.txtHour.Location = new Point(190, 97);
      this.txtHour.Name = "txtHour";
      this.txtHour.Size = new Size(28, 20);
      this.txtHour.TabIndex = 15;
      this.txtHour.TextChanged += new EventHandler(this.Data_Changed);
      this.txtHour.KeyPress += new KeyPressEventHandler(this.txtHour_KeyPress);
      this.pnlMiddle.Controls.Add((Control) this.gcCopies);
      this.pnlMiddle.Controls.Add((Control) this.gcEvents);
      this.pnlMiddle.Dock = DockStyle.Bottom;
      this.pnlMiddle.Location = new Point(0, 121);
      this.pnlMiddle.Name = "pnlMiddle";
      this.pnlMiddle.Size = new Size(742, 141);
      this.pnlMiddle.TabIndex = 1;
      this.pnlMiddle.ClientSizeChanged += new EventHandler(this.pnlMiddle_ClientSizeChanged);
      this.gcCopies.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcCopies.Controls.Add((Control) this.chkCopyeFolder);
      this.gcCopies.Controls.Add((Control) this.chkCopyPrint);
      this.gcCopies.Dock = DockStyle.Fill;
      this.gcCopies.HeaderForeColor = SystemColors.ControlText;
      this.gcCopies.Location = new Point(348, 0);
      this.gcCopies.Name = "gcCopies";
      this.gcCopies.Size = new Size(394, 141);
      this.gcCopies.TabIndex = 1;
      this.gcCopies.Text = "Copy of disclosures";
      this.chkCopyeFolder.AutoSize = true;
      this.chkCopyeFolder.Location = new Point(11, 49);
      this.chkCopyeFolder.Name = "chkCopyeFolder";
      this.chkCopyeFolder.Size = new Size(296, 18);
      this.chkCopyeFolder.TabIndex = 1;
      this.chkCopyeFolder.Text = "Save copy of disclosures when disclosing from eFolder";
      this.chkCopyeFolder.UseVisualStyleBackColor = true;
      this.chkCopyeFolder.CheckedChanged += new EventHandler(this.Data_Changed);
      this.chkCopyPrint.AutoSize = true;
      this.chkCopyPrint.Location = new Point(11, 31);
      this.chkCopyPrint.Name = "chkCopyPrint";
      this.chkCopyPrint.Size = new Size(310, 18);
      this.chkCopyPrint.TabIndex = 0;
      this.chkCopyPrint.Text = "Save copy of disclosures when disclosing from Print Menu";
      this.chkCopyPrint.UseVisualStyleBackColor = true;
      this.chkCopyPrint.CheckedChanged += new EventHandler(this.Data_Changed);
      this.gcLECD.Controls.Add((Control) this.chkboxGFEExpirationDate);
      this.gcLECD.Controls.Add((Control) this.chkboxEarliestClosingDate);
      this.gcLECD.Controls.Add((Control) this.label5);
      this.gcLECD.Controls.Add((Control) this.label4);
      this.gcLECD.Controls.Add((Control) this.label2);
      this.gcLECD.Controls.Add((Control) this.txtDaystoExpire);
      this.gcLECD.Controls.Add((Control) this.label1);
      this.gcLECD.Dock = DockStyle.Bottom;
      this.gcLECD.HeaderForeColor = SystemColors.ControlText;
      this.gcLECD.Location = new Point(0, 472);
      this.gcLECD.Name = "gcLECD";
      this.gcLECD.Size = new Size(742, 90);
      this.gcLECD.TabIndex = 3;
      this.gcLECD.Text = "Compliance Timeline Calculation for GFE/TIL";
      this.chkboxGFEExpirationDate.AutoSize = true;
      this.chkboxGFEExpirationDate.Location = new Point(145, 48);
      this.chkboxGFEExpirationDate.Name = "chkboxGFEExpirationDate";
      this.chkboxGFEExpirationDate.Size = new Size(324, 18);
      this.chkboxGFEExpirationDate.TabIndex = 22;
      this.chkboxGFEExpirationDate.Text = "Include Initial GFE Sent Date (field 3148) in the date calculation";
      this.chkboxGFEExpirationDate.UseVisualStyleBackColor = true;
      this.chkboxGFEExpirationDate.CheckedChanged += new EventHandler(this.Data_Changed);
      this.chkboxEarliestClosingDate.AutoSize = true;
      this.chkboxEarliestClosingDate.Location = new Point(145, 29);
      this.chkboxEarliestClosingDate.Name = "chkboxEarliestClosingDate";
      this.chkboxEarliestClosingDate.Size = new Size(596, 18);
      this.chkboxEarliestClosingDate.TabIndex = 20;
      this.chkboxEarliestClosingDate.Text = "Include Initial TIL Sent Date (field 3152) and Borrower Received Date for Revised TIL (field 3155) in the date calculation";
      this.chkboxEarliestClosingDate.UseVisualStyleBackColor = true;
      this.chkboxEarliestClosingDate.CheckedChanged += new EventHandler(this.Data_Changed);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 49);
      this.label5.Name = "label5";
      this.label5.Size = new Size(102, 14);
      this.label5.TabIndex = 21;
      this.label5.Text = "GFE Expiration Date";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 30);
      this.label4.Name = "label4";
      this.label4.Size = new Size(105, 14);
      this.label4.TabIndex = 19;
      this.label4.Text = "Earliest Closing Date";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(288, 69);
      this.label2.Name = "label2";
      this.label2.Size = new Size(222, 14);
      this.label2.TabIndex = 25;
      this.label2.Text = "days (minimum 10 days are required by law)";
      this.txtDaystoExpire.Location = new Point(226, 67);
      this.txtDaystoExpire.Name = "txtDaystoExpire";
      this.txtDaystoExpire.Size = new Size(56, 20);
      this.txtDaystoExpire.TabIndex = 24;
      this.txtDaystoExpire.TextAlign = HorizontalAlignment.Right;
      this.txtDaystoExpire.TextChanged += new EventHandler(this.Data_Changed);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(143, 69);
      this.label1.Name = "label1";
      this.label1.Size = new Size(77, 14);
      this.label1.TabIndex = 23;
      this.label1.Text = "GFE Expires in";
      this.gcCoc.Controls.Add((Control) this.chkCoCPriorDisclosure);
      this.gcCoc.Dock = DockStyle.Bottom;
      this.gcCoc.HeaderForeColor = SystemColors.ControlText;
      this.gcCoc.Location = new Point(0, 562);
      this.gcCoc.Name = "gcCoc";
      this.gcCoc.Size = new Size(742, 59);
      this.gcCoc.TabIndex = 26;
      this.gcCoc.Text = "Changed Circumstances";
      this.chkCoCPriorDisclosure.AutoSize = true;
      this.chkCoCPriorDisclosure.Location = new Point(11, 31);
      this.chkCoCPriorDisclosure.Name = "chkCoCPriorDisclosure";
      this.chkCoCPriorDisclosure.Size = new Size(318, 18);
      this.chkCoCPriorDisclosure.TabIndex = 20;
      this.chkCoCPriorDisclosure.Text = "Require fee level Changed Circumstances prior to disclosure";
      this.chkCoCPriorDisclosure.UseVisualStyleBackColor = true;
      this.chkCoCPriorDisclosure.CheckedChanged += new EventHandler(this.Data_Changed);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcFormList);
      this.Controls.Add((Control) this.pnlMiddle);
      this.Controls.Add((Control) this.gcTimeline);
      this.Controls.Add((Control) this.gcLECD);
      this.Controls.Add((Control) this.gcCoc);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DisclosureTrackingSetting);
      this.Size = new Size(742, 621);
      this.gcEvents.ResumeLayout(false);
      this.gcEvents.PerformLayout();
      this.gcFormList.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnNew).EndInit();
      ((ISupportInitialize) this.stBtnDelete).EndInit();
      this.gcTimeline.ResumeLayout(false);
      this.gcTimeline.PerformLayout();
      this.pnlMiddle.ResumeLayout(false);
      this.gcCopies.ResumeLayout(false);
      this.gcCopies.PerformLayout();
      this.gcLECD.ResumeLayout(false);
      this.gcLECD.PerformLayout();
      this.gcCoc.ResumeLayout(false);
      this.gcCoc.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
