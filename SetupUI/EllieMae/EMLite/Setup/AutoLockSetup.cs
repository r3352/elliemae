// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutoLockSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutoLockSetup : SettingsUserControl
  {
    private AutoLockRulePanel rule;
    private Sessions.Session session;
    private ProductPricingSetting mpssetting;
    private bool suspendEvent;
    private bool showMessage;
    private bool setEPPS;
    private GroupContainer gcAutoLockEnable;
    private CheckBox chkAutoCancelLockRequest;
    private CheckBox chkAutoLockExtRequest;
    private CheckBox chkAutoLockRelock;
    private CheckBox chkAutoLockRequest;
    private GroupContainer gcAutoLockExCriteriaRules;
    private Panel pnlAutoLockRule;
    private Label label4;
    private Label label3;
    private ListView lvLoanType;
    private CheckBox chkECLoanType;
    private CheckBox chkECPropertyState;
    private ListView lvPropertyState;
    private CheckBox chkECChannel;
    private ListView lvChannel;
    private ListView lvLoanPurpose;
    private CheckBox chkECLoanPurpose;
    private CheckBox chkECAmorType;
    private ListView lvAmorType;
    private CheckBox chkECPropertyPurpose;
    private ListView lvPropertyPurpose;
    private CheckBox chkECLienPosition;
    private ListView lvLienPosition;
    private CheckBox chkECLoanProg;
    private CheckBox chkECLockPlanCode;
    private GroupContainer gcAutoLockExCriteria;
    private Panel panel2;
    private GridView gvLockPlanCode;
    private GridView gvLoanProg;
    private StandardIconButton stdIconBtnAddLockPlan;
    private StandardIconButton stdIconBtnEditLockPlan;
    private StandardIconButton stdIconBtnDeleteLockPlan;
    private StandardIconButton stdIconBtnAddLoanProgram;
    private StandardIconButton stdIconBtnEditLoanProgram;
    private StandardIconButton stdIconBtnDeleteLoanProgram;

    public AutoLockSetup(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.initPage();
    }

    private void initPage()
    {
      this.suspendEvent = true;
      if (!ProductPricingUtils.IsHistoricalPricingEnabled)
        this.chkAutoLockRelock.Visible = false;
      this.rule = new AutoLockRulePanel(Session.DefaultInstance, false);
      this.pnlAutoLockRule.Controls.Add((Control) this.rule);
      this.rule.Dock = DockStyle.Fill;
      this.setEPPS = this.loadAutoLockSettings();
      this.setDirtyFlag(false);
      this.suspendEvent = false;
    }

    public override void Reset() => this.initPage();

    private void chkAutoLockRequest_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkAutoLockRequest.Checked)
      {
        this.chkAutoLockRelock.Enabled = true;
        this.enableExclusionCriteria(true);
      }
      else
      {
        this.chkAutoLockRelock.Enabled = this.chkAutoLockRelock.Checked = false;
        this.enableExclusionCriteria(false);
      }
      this.chk_CheckedChanged(sender, e);
    }

    private void chk_CheckedChanged(object sender, EventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private void loadListView()
    {
      foreach (lvType type in Enum.GetValues(typeof (lvType)))
        this.loadListBox(type);
    }

    private void loadListBox(lvType type)
    {
      switch (type)
      {
        case lvType.LoanType:
          this.loadListViewByFieldDef("2952", this.lvLoanType);
          break;
        case lvType.PropertyState:
          this.loadListViewContent(Utils.GetStates(), this.lvPropertyState);
          break;
        case lvType.Channel:
          this.loadListViewByFieldDef("2626", this.lvChannel);
          break;
        case lvType.LoanPurpose:
          this.loadListViewByFieldDef("2951", this.lvLoanPurpose);
          break;
        case lvType.AmorizationType:
          this.loadListViewByFieldDef("2953", this.lvAmorType);
          break;
        case lvType.PropertyPurpose:
          this.loadListViewByFieldDef("2950", this.lvPropertyPurpose);
          break;
        case lvType.LienPosition:
          this.loadListViewByFieldDef("2958", this.lvLienPosition);
          break;
      }
    }

    private void loadListViewByFieldDef(string fieldID, ListView view)
    {
      FieldDefinition field = EncompassFields.GetField(fieldID);
      view.Items.Clear();
      if (fieldID == "2951")
      {
        string[] strArray = new string[5]
        {
          "Purchase",
          "Cash-Out Refinance",
          "NoCash-Out Refinance",
          "ConstructionOnly",
          "ConstructionToPermanent"
        };
        foreach (string str in strArray)
        {
          FieldOption optionByValue = field.Options.GetOptionByValue(str);
          view.Items.Add(new ListViewItem(optionByValue.Text.ToString())
          {
            Tag = (object) optionByValue.Value.ToString()
          });
        }
      }
      else
      {
        FieldOption[] array = field.Options.ToArray();
        if (fieldID == "2950" || fieldID == "2958")
          Array.Reverse((Array) array);
        foreach (FieldOption fieldOption in array)
        {
          if ((!(fieldID == "2952") || !(fieldOption.Value == "Other") && !(fieldOption.Value == "HELOC")) && (!(fieldID == "2953") || !(fieldOption.Value == "GraduatedPaymentMortgage") && !(fieldOption.Value == "OtherAmortizationType")))
            view.Items.Add(new ListViewItem(fieldOption.Text.ToString())
            {
              Tag = (object) fieldOption.Value.ToString()
            });
        }
      }
    }

    private void loadListViewContent(string[] src, ListView view)
    {
      view.Items.Clear();
      for (int index = 0; index < src.Length; ++index)
        view.Items.Add(new ListViewItem(src[index])
        {
          Tag = (object) src[index]
        });
    }

    private void checkListView(string src, ListView lv)
    {
      if (string.IsNullOrEmpty(src))
        return;
      HashSet<string> source = new HashSet<string>((IEnumerable<string>) ((IEnumerable<string>) src.Split(';')).Select<string, string>((Func<string, string>) (p => p.Trim())).ToList<string>());
      foreach (ListViewItem listViewItem in lv.Items)
      {
        if (((IEnumerable<object>) source).Contains<object>(listViewItem.Tag))
          listViewItem.Checked = true;
      }
    }

    private void uncheckListView(ListView lv, bool state)
    {
      if (lv == null)
        return;
      foreach (ListViewItem listViewItem in lv.Items)
        listViewItem.Checked = state;
    }

    private void listView_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (this.suspendEvent)
        return;
      this.setDirtyFlag(true);
    }

    private string storelvSetting(ListView lv, CheckBox chk)
    {
      string str1 = "";
      foreach (ListViewItem listViewItem in lv.Items)
      {
        if (listViewItem.Checked)
          str1 = str1 + listViewItem.Tag.ToString().Trim() + ";";
      }
      string str2;
      if (!chk.Checked)
      {
        if (!string.IsNullOrWhiteSpace(str1))
          this.showMessage = true;
        str2 = "#;" + str1;
      }
      else
        str2 = "$;" + str1;
      return str2;
    }

    private bool loadAutoLockSettings()
    {
      this.mpssetting = this.session.ConfigurationManager.GetActiveProductPricingPartner();
      if (this.mpssetting == null || !this.mpssetting.IsEPPS)
      {
        this.disabledAllSettings();
        return false;
      }
      this.chkAutoLockRequest.Enabled = this.chkAutoLockExtRequest.Enabled = this.chkAutoCancelLockRequest.Enabled = this.mpssetting.SupportEnableAutoLockRequest;
      this.chkAutoLockRequest.Checked = this.chkAutoLockRelock.Enabled = this.mpssetting.EnableAutoLockRequest;
      this.chkAutoLockRelock.Checked = this.mpssetting.EnableAutoLockRelock;
      this.chkAutoLockExtRequest.Checked = this.mpssetting.EnableAutoLockExtensionRequest;
      this.chkAutoCancelLockRequest.Checked = this.mpssetting.EnableAutoCancelRequest;
      this.loadListViewSetting(this.mpssetting.ExcludeAutoLockLoanType, this.lvLoanType, this.chkECLoanType);
      this.loadListViewSetting(this.mpssetting.ExcludeAutoLockPropertyState, this.lvPropertyState, this.chkECPropertyState);
      this.loadListViewSetting(this.mpssetting.ExcludeAutoLockChannel, this.lvChannel, this.chkECChannel);
      this.loadListViewSetting(this.mpssetting.ExcludeAutoLockLoanPurpose, this.lvLoanPurpose, this.chkECLoanPurpose);
      this.loadListViewSetting(this.mpssetting.ExcludeAutoLockAmortizationType, this.lvAmorType, this.chkECAmorType);
      this.loadListViewSetting(this.mpssetting.ExcludeAutoLockPropertyWillBe, this.lvPropertyPurpose, this.chkECPropertyPurpose);
      this.loadListViewSetting(this.mpssetting.ExcludeAutoLockLienPosition, this.lvLienPosition, this.chkECLienPosition);
      this.loadGridViewSetting(this.mpssetting.ExcludeAutoLockLoanProgram, this.gvLoanProg, this.chkECLoanProg);
      this.loadGridViewSetting(this.mpssetting.ExcludeAutoLockPlanCode, this.gvLockPlanCode, this.chkECLockPlanCode);
      this.enableExclusionCriteria(this.chkAutoLockRequest.Checked);
      return true;
    }

    private void loadListViewSetting(string setting, ListView lv, CheckBox chk)
    {
      if (string.IsNullOrEmpty(setting))
      {
        chk.Checked = false;
        lv.Enabled = false;
      }
      else
      {
        if (setting.StartsWith("$;"))
        {
          chk.Checked = true;
          lv.Enabled = true;
        }
        else if (setting.StartsWith("#;"))
        {
          chk.Checked = false;
          lv.Enabled = false;
        }
        this.checkListView(setting.Substring(2), lv);
      }
    }

    private void loadGridViewSetting(string setting, GridView view, CheckBox chk)
    {
      if (string.IsNullOrEmpty(setting))
      {
        chk.Checked = false;
        view.Enabled = false;
      }
      else
      {
        if (setting.StartsWith("$;"))
        {
          chk.Checked = true;
          view.Enabled = true;
        }
        else if (setting.StartsWith("#;"))
        {
          chk.Checked = false;
          view.Enabled = false;
        }
        view.Items.Clear();
        List<string> list = ((IEnumerable<string>) setting.Split(';')).Select<string, string>((Func<string, string>) (p => p.Trim())).ToList<string>();
        for (int index = 0; index < list.Count; ++index)
        {
          if (list[index] != "#" && list[index] != "$" && !string.IsNullOrWhiteSpace(list[index]))
            view.Items.Add(new GVItem()
            {
              SubItems = {
                new GVSubItem((object) list[index])
              },
              Tag = (object) list[index]
            });
        }
      }
    }

    private void chkLoanCriteria_CheckedChanged(object sender, EventArgs e)
    {
      if (sender == this.chkECLoanType)
        this.lvLoanType.Enabled = this.chkECLoanType.Checked;
      else if (sender == this.chkECPropertyState)
        this.lvPropertyState.Enabled = this.chkECPropertyState.Checked;
      else if (sender == this.chkECChannel)
        this.lvChannel.Enabled = this.chkECChannel.Checked;
      else if (sender == this.chkECLoanPurpose)
        this.lvLoanPurpose.Enabled = this.chkECLoanPurpose.Checked;
      else if (sender == this.chkECAmorType)
        this.lvAmorType.Enabled = this.chkECAmorType.Checked;
      else if (sender == this.chkECPropertyPurpose)
        this.lvPropertyPurpose.Enabled = this.chkECPropertyPurpose.Checked;
      else if (sender == this.chkECLienPosition)
        this.lvLienPosition.Enabled = this.chkECLienPosition.Checked;
      else if (sender == this.chkECLoanProg)
      {
        this.gvLoanProg.Enabled = this.stdIconBtnAddLoanProgram.Enabled = this.chkECLoanProg.Checked;
        if (!this.chkECLoanProg.Checked)
          this.gvLoanProg.SelectedItems.Clear();
        this.stdIconBtnEditLoanProgram.Enabled = this.stdIconBtnDeleteLoanProgram.Enabled = this.chkECLoanProg.Checked && this.gvLoanProg.SelectedItems.Count > 0;
      }
      else if (sender == this.chkECLockPlanCode)
      {
        this.gvLockPlanCode.Enabled = this.stdIconBtnAddLockPlan.Enabled = this.chkECLockPlanCode.Checked;
        if (!this.chkECLockPlanCode.Checked)
          this.gvLockPlanCode.SelectedItems.Clear();
        this.stdIconBtnEditLockPlan.Enabled = this.stdIconBtnDeleteLockPlan.Enabled = this.chkECLockPlanCode.Checked && this.gvLockPlanCode.SelectedItems.Count > 0;
      }
      this.chk_CheckedChanged(sender, e);
    }

    public override void Save()
    {
      List<ProductPricingSetting> productPricingSettings = this.session.ConfigurationManager.GetProductPricingSettings();
      List<ProductPricingSetting> settings = new List<ProductPricingSetting>();
      this.mpssetting.EnableAutoLockRequest = this.chkAutoLockRequest.Checked;
      this.mpssetting.EnableAutoLockExtensionRequest = this.chkAutoLockExtRequest.Checked;
      this.mpssetting.EnableAutoCancelRequest = this.chkAutoCancelLockRequest.Checked;
      this.mpssetting.EnableAutoLockRelock = this.chkAutoLockRelock.Checked;
      this.mpssetting.ExcludeAutoLockLoanType = this.storelvSetting(this.lvLoanType, this.chkECLoanType);
      this.mpssetting.ExcludeAutoLockPropertyState = this.storelvSetting(this.lvPropertyState, this.chkECPropertyState);
      this.mpssetting.ExcludeAutoLockChannel = this.storelvSetting(this.lvChannel, this.chkECChannel);
      this.mpssetting.ExcludeAutoLockLoanPurpose = this.storelvSetting(this.lvLoanPurpose, this.chkECLoanPurpose);
      this.mpssetting.ExcludeAutoLockAmortizationType = this.storelvSetting(this.lvAmorType, this.chkECAmorType);
      this.mpssetting.ExcludeAutoLockPropertyWillBe = this.storelvSetting(this.lvPropertyPurpose, this.chkECPropertyPurpose);
      this.mpssetting.ExcludeAutoLockLienPosition = this.storelvSetting(this.lvLienPosition, this.chkECLienPosition);
      this.mpssetting.ExcludeAutoLockLoanProgram = this.storeLoanProgramOrLockPlanCode(this.gvLoanProg, this.chkECLoanProg);
      this.mpssetting.ExcludeAutoLockPlanCode = this.storeLoanProgramOrLockPlanCode(this.gvLockPlanCode, this.chkECLockPlanCode);
      foreach (ProductPricingSetting productPricingSetting in productPricingSettings)
      {
        if (productPricingSetting.ProviderID != this.mpssetting.ProviderID)
          settings.Add(productPricingSetting);
      }
      settings.Add(this.mpssetting);
      this.dataValidation();
      this.session.ConfigurationManager.UpdateProductPricingSettings(settings);
      this.session.StartupInfo.ProductPricingPartner = this.mpssetting;
      this.setDirtyFlag(false);
      this.showMessage = false;
    }

    private void dataValidation()
    {
      if (!this.showMessage || !this.chkAutoLockRequest.Checked)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Disabling 'Parent' (section) settings will disable any 'Child' settings for Auto-Lock exclusion. However, the 'Child' settings will persist for ease of reinstatement.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private string storeLoanProgramOrLockPlanCode(GridView view, CheckBox chk)
    {
      string str1 = "";
      for (int index = 0; index < view.Items.Count; ++index)
      {
        if (!string.IsNullOrWhiteSpace(view.Items.ElementAt<GVItem>(index).Tag.ToString()))
          str1 = str1 + view.Items.ElementAt<GVItem>(index).Tag.ToString() + ";";
      }
      string str2;
      if (!chk.Checked)
      {
        if (!string.IsNullOrWhiteSpace(str1))
          this.showMessage = true;
        str2 = "#;" + str1;
      }
      else
        str2 = "$;" + str1;
      return str2;
    }

    private void AutoLockSetup_Load(object sender, EventArgs e)
    {
      this.lvPropertyState.Columns.Add(new ColumnHeader()
      {
        Text = "",
        Name = "col1"
      });
      this.loadListView();
      this.Reset();
      if (this.setEPPS)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "To use the Auto-Lock settings, you must first select ICE PPE as the Product and Pricing Provider.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void enableExclusionCriteria(bool status)
    {
      this.chkECLoanType.Enabled = status;
      this.lvLoanType.Enabled = status && this.chkECLoanType.Checked;
      this.chkECPropertyState.Enabled = status;
      this.lvPropertyState.Enabled = status && this.chkECPropertyState.Checked;
      this.chkECAmorType.Enabled = status;
      this.lvAmorType.Enabled = status && this.chkECAmorType.Checked;
      this.chkECChannel.Enabled = status;
      this.lvChannel.Enabled = status && this.chkECChannel.Checked;
      this.chkECLoanPurpose.Enabled = status;
      this.lvLoanPurpose.Enabled = status && this.chkECLoanPurpose.Checked;
      this.chkECPropertyPurpose.Enabled = status;
      this.lvPropertyPurpose.Enabled = status && this.chkECPropertyPurpose.Checked;
      this.chkECLienPosition.Enabled = status;
      this.lvLienPosition.Enabled = status && this.chkECLienPosition.Checked;
      this.chkECLoanProg.Enabled = status;
      this.gvLoanProg.Enabled = status && this.chkECLoanProg.Checked;
      this.gvLoanProg.SelectedItems.Clear();
      this.stdIconBtnAddLoanProgram.Enabled = status && this.chkECLoanProg.Checked;
      this.stdIconBtnEditLoanProgram.Enabled = this.stdIconBtnDeleteLoanProgram.Enabled = status && this.gvLoanProg.SelectedItems.Count > 0 && this.chkECLoanProg.Checked;
      this.chkECLockPlanCode.Enabled = status;
      this.gvLockPlanCode.Enabled = status && this.chkECLockPlanCode.Checked;
      this.gvLockPlanCode.SelectedItems.Clear();
      this.stdIconBtnAddLockPlan.Enabled = status && this.chkECLockPlanCode.Checked;
      this.stdIconBtnEditLockPlan.Enabled = this.stdIconBtnDeleteLockPlan.Enabled = status && this.gvLockPlanCode.SelectedItems.Count > 0 && this.chkECLockPlanCode.Checked;
      this.rule.Enabled = status;
      this.pnlAutoLockRule.Controls.Clear();
      this.pnlAutoLockRule.Controls.Add((Control) this.rule);
    }

    private void disabledAllSettings()
    {
      this.chkAutoLockRequest.Enabled = this.chkAutoLockRelock.Enabled = this.chkAutoLockExtRequest.Enabled = this.chkAutoCancelLockRequest.Enabled = false;
      this.chkAutoLockRequest.Checked = this.chkAutoLockRelock.Checked = this.chkAutoLockExtRequest.Checked = this.chkAutoCancelLockRequest.Checked = false;
      this.enableExclusionCriteria(false);
      this.uncheckListView(this.lvLoanType, false);
      this.uncheckListView(this.lvPropertyState, false);
      this.uncheckListView(this.lvAmorType, false);
      this.uncheckListView(this.lvChannel, false);
      this.uncheckListView(this.lvLoanPurpose, false);
      this.uncheckListView(this.lvPropertyPurpose, false);
      this.uncheckListView(this.lvLienPosition, false);
    }

    private void addLoanProgramOrLockPlanCode(object sender, EventArgs e)
    {
      GridView currentView = (GridView) null;
      if (sender == this.stdIconBtnAddLoanProgram)
        currentView = this.gvLoanProg;
      else if (sender == this.stdIconBtnAddLockPlan)
        currentView = this.gvLockPlanCode;
      using (AutoLockLoanProgLockPlanAddForm progLockPlanAddForm = new AutoLockLoanProgLockPlanAddForm(currentView))
      {
        if (progLockPlanAddForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (progLockPlanAddForm.Result.Count != 0)
        {
          HashSet<string> stringSet = this.combineResult(currentView, progLockPlanAddForm.Result);
          if (stringSet.Count != currentView.Items.Count)
          {
            currentView.Items.Clear();
            foreach (string str in stringSet)
              currentView.Items.Add(new GVItem()
              {
                SubItems = {
                  new GVSubItem((object) str)
                },
                Tag = (object) str
              });
            this.setDirtyFlag(true);
          }
        }
        if (!progLockPlanAddForm.isAddMore)
          return;
        this.addLoanProgramOrLockPlanCode(sender, e);
      }
    }

    private void editLoanProgramOrLockPlanCode(object sender, EventArgs e)
    {
      GridView currentView = (GridView) null;
      if (sender == this.stdIconBtnEditLoanProgram || sender == this.gvLoanProg)
        currentView = this.gvLoanProg;
      else if (sender == this.stdIconBtnEditLockPlan || sender == this.gvLockPlanCode)
        currentView = this.gvLockPlanCode;
      if (currentView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (currentView.SelectedItems.Count > 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Only one parameter at a time is allowed for editing. Please select the item you wish to edit then click the Edit icon.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        GVItem selectedItem = currentView.SelectedItems[0];
        using (AutoLockLoanProgLockPlanEditForm lockPlanEditForm = new AutoLockLoanProgLockPlanEditForm(currentView, selectedItem))
        {
          if (lockPlanEditForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          int index = currentView.SelectedItems[0].Index;
          currentView.Items.ElementAt<GVItem>(index).Value = (object) lockPlanEditForm.Item.Text;
          currentView.Items.ElementAt<GVItem>(index).Tag = (object) lockPlanEditForm.Item.Text;
          this.setDirtyFlag(true);
        }
      }
    }

    private void deleteLoanProgramOrLockPlanCode(object sender, EventArgs e)
    {
      GridView gridView = (GridView) null;
      if (sender == this.stdIconBtnDeleteLoanProgram)
        gridView = this.gvLoanProg;
      else if (sender == this.stdIconBtnDeleteLockPlan)
        gridView = this.gvLockPlanCode;
      if (gridView.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete these entries?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        {
          foreach (GVItem selectedItem in gridView.SelectedItems)
            gridView.Items.Remove(selectedItem);
          this.setDirtyFlag(true);
        }
        if (gridView == this.gvLoanProg)
        {
          this.stdIconBtnEditLoanProgram.Enabled = this.stdIconBtnDeleteLoanProgram.Enabled = false;
        }
        else
        {
          if (gridView != this.gvLockPlanCode)
            return;
          this.stdIconBtnEditLockPlan.Enabled = this.stdIconBtnDeleteLockPlan.Enabled = false;
        }
      }
    }

    private HashSet<string> combineResult(GridView currentView, List<GVItem> result)
    {
      HashSet<string> stringSet = new HashSet<string>();
      for (int index = 0; index < currentView.Items.Count; ++index)
      {
        if (!string.IsNullOrWhiteSpace(currentView.Items.ElementAt<GVItem>(index).Text))
          stringSet.Add(currentView.Items.ElementAt<GVItem>(index).Text);
      }
      foreach (GVItem gvItem in result)
      {
        if (!string.IsNullOrWhiteSpace(gvItem.Text))
          stringSet.Add(gvItem.Text);
      }
      return stringSet;
    }

    private void gvLockPlanCode_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEditLockPlan.Enabled = this.stdIconBtnDeleteLockPlan.Enabled = this.gvLockPlanCode.SelectedItems.Count > 0;
    }

    private void gvLoanProg_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEditLoanProgram.Enabled = this.stdIconBtnDeleteLoanProgram.Enabled = this.gvLoanProg.SelectedItems.Count > 0;
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.panel2 = new Panel();
      this.gcAutoLockExCriteriaRules = new GroupContainer();
      this.label4 = new Label();
      this.pnlAutoLockRule = new Panel();
      this.gcAutoLockExCriteria = new GroupContainer();
      this.stdIconBtnAddLockPlan = new StandardIconButton();
      this.stdIconBtnEditLockPlan = new StandardIconButton();
      this.stdIconBtnDeleteLockPlan = new StandardIconButton();
      this.stdIconBtnAddLoanProgram = new StandardIconButton();
      this.stdIconBtnEditLoanProgram = new StandardIconButton();
      this.stdIconBtnDeleteLoanProgram = new StandardIconButton();
      this.gvLockPlanCode = new GridView();
      this.gvLoanProg = new GridView();
      this.chkECLockPlanCode = new CheckBox();
      this.chkECLoanProg = new CheckBox();
      this.lvLienPosition = new ListView();
      this.chkECLienPosition = new CheckBox();
      this.lvPropertyPurpose = new ListView();
      this.chkECPropertyPurpose = new CheckBox();
      this.lvAmorType = new ListView();
      this.chkECAmorType = new CheckBox();
      this.chkECLoanPurpose = new CheckBox();
      this.lvLoanPurpose = new ListView();
      this.lvChannel = new ListView();
      this.chkECChannel = new CheckBox();
      this.lvPropertyState = new ListView();
      this.chkECPropertyState = new CheckBox();
      this.chkECLoanType = new CheckBox();
      this.lvLoanType = new ListView();
      this.label3 = new Label();
      this.gcAutoLockEnable = new GroupContainer();
      this.chkAutoCancelLockRequest = new CheckBox();
      this.chkAutoLockExtRequest = new CheckBox();
      this.chkAutoLockRelock = new CheckBox();
      this.chkAutoLockRequest = new CheckBox();
      this.panel2.SuspendLayout();
      this.gcAutoLockExCriteriaRules.SuspendLayout();
      this.gcAutoLockExCriteria.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnAddLockPlan).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditLockPlan).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteLockPlan).BeginInit();
      ((ISupportInitialize) this.stdIconBtnAddLoanProgram).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEditLoanProgram).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDeleteLoanProgram).BeginInit();
      this.gcAutoLockEnable.SuspendLayout();
      this.SuspendLayout();
      this.panel2.AutoScroll = true;
      this.panel2.BackColor = Color.Transparent;
      this.panel2.Controls.Add((Control) this.gcAutoLockExCriteriaRules);
      this.panel2.Controls.Add((Control) this.gcAutoLockExCriteria);
      this.panel2.Controls.Add((Control) this.gcAutoLockEnable);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(1008, 1000);
      this.panel2.TabIndex = 5;
      this.gcAutoLockExCriteriaRules.Controls.Add((Control) this.label4);
      this.gcAutoLockExCriteriaRules.Controls.Add((Control) this.pnlAutoLockRule);
      this.gcAutoLockExCriteriaRules.Dock = DockStyle.Top;
      this.gcAutoLockExCriteriaRules.HeaderForeColor = SystemColors.ControlText;
      this.gcAutoLockExCriteriaRules.Location = new Point(0, 557);
      this.gcAutoLockExCriteriaRules.Name = "gcAutoLockExCriteriaRules";
      this.gcAutoLockExCriteriaRules.Size = new Size(991, 464);
      this.gcAutoLockExCriteriaRules.TabIndex = 4;
      this.gcAutoLockExCriteriaRules.Text = "Auto-Lock Exclusion Criteria Rules";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 35);
      this.label4.Name = "label4";
      this.label4.Size = new Size(392, 13);
      this.label4.TabIndex = 1;
      this.label4.Text = "Create and manage rules that control the values that are not eligible for Auto-Lock";
      this.pnlAutoLockRule.BackColor = Color.Transparent;
      this.pnlAutoLockRule.Dock = DockStyle.Bottom;
      this.pnlAutoLockRule.Location = new Point(1, 57);
      this.pnlAutoLockRule.Name = "pnlAutoLockRule";
      this.pnlAutoLockRule.Size = new Size(989, 406);
      this.pnlAutoLockRule.TabIndex = 0;
      this.gcAutoLockExCriteria.AutoScroll = true;
      this.gcAutoLockExCriteria.Controls.Add((Control) this.stdIconBtnAddLockPlan);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.stdIconBtnEditLockPlan);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.stdIconBtnDeleteLockPlan);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.stdIconBtnAddLoanProgram);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.stdIconBtnEditLoanProgram);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.stdIconBtnDeleteLoanProgram);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.gvLockPlanCode);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.gvLoanProg);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECLockPlanCode);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECLoanProg);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.lvLienPosition);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECLienPosition);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.lvPropertyPurpose);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECPropertyPurpose);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.lvAmorType);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECAmorType);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECLoanPurpose);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.lvLoanPurpose);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.lvChannel);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECChannel);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.lvPropertyState);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECPropertyState);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.chkECLoanType);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.lvLoanType);
      this.gcAutoLockExCriteria.Controls.Add((Control) this.label3);
      this.gcAutoLockExCriteria.Dock = DockStyle.Top;
      this.gcAutoLockExCriteria.HeaderForeColor = SystemColors.ControlText;
      this.gcAutoLockExCriteria.Location = new Point(0, 132);
      this.gcAutoLockExCriteria.Name = "gcAutoLockExCriteria";
      this.gcAutoLockExCriteria.Size = new Size(991, 425);
      this.gcAutoLockExCriteria.TabIndex = 3;
      this.gcAutoLockExCriteria.Text = "Auto-Lock Exclusion Criteria";
      this.stdIconBtnAddLockPlan.BackColor = Color.Transparent;
      this.stdIconBtnAddLockPlan.Enabled = false;
      this.stdIconBtnAddLockPlan.Location = new Point(787, 231);
      this.stdIconBtnAddLockPlan.MouseDownImage = (Image) null;
      this.stdIconBtnAddLockPlan.Name = "stdIconBtnAddLockPlan";
      this.stdIconBtnAddLockPlan.Size = new Size(16, 16);
      this.stdIconBtnAddLockPlan.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnAddLockPlan.TabIndex = 79;
      this.stdIconBtnAddLockPlan.TabStop = false;
      this.stdIconBtnAddLockPlan.Click += new EventHandler(this.addLoanProgramOrLockPlanCode);
      this.stdIconBtnEditLockPlan.BackColor = Color.Transparent;
      this.stdIconBtnEditLockPlan.Enabled = false;
      this.stdIconBtnEditLockPlan.Location = new Point(809, 231);
      this.stdIconBtnEditLockPlan.MouseDownImage = (Image) null;
      this.stdIconBtnEditLockPlan.Name = "stdIconBtnEditLockPlan";
      this.stdIconBtnEditLockPlan.Size = new Size(16, 16);
      this.stdIconBtnEditLockPlan.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditLockPlan.TabIndex = 78;
      this.stdIconBtnEditLockPlan.TabStop = false;
      this.stdIconBtnEditLockPlan.Click += new EventHandler(this.editLoanProgramOrLockPlanCode);
      this.stdIconBtnDeleteLockPlan.BackColor = Color.Transparent;
      this.stdIconBtnDeleteLockPlan.Enabled = false;
      this.stdIconBtnDeleteLockPlan.Location = new Point(830, 231);
      this.stdIconBtnDeleteLockPlan.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteLockPlan.Name = "stdIconBtnDeleteLockPlan";
      this.stdIconBtnDeleteLockPlan.Size = new Size(16, 16);
      this.stdIconBtnDeleteLockPlan.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteLockPlan.TabIndex = 77;
      this.stdIconBtnDeleteLockPlan.TabStop = false;
      this.stdIconBtnDeleteLockPlan.Click += new EventHandler(this.deleteLoanProgramOrLockPlanCode);
      this.stdIconBtnAddLoanProgram.BackColor = Color.Transparent;
      this.stdIconBtnAddLoanProgram.Enabled = false;
      this.stdIconBtnAddLoanProgram.Location = new Point(358, 232);
      this.stdIconBtnAddLoanProgram.MouseDownImage = (Image) null;
      this.stdIconBtnAddLoanProgram.Name = "stdIconBtnAddLoanProgram";
      this.stdIconBtnAddLoanProgram.Size = new Size(16, 16);
      this.stdIconBtnAddLoanProgram.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnAddLoanProgram.TabIndex = 76;
      this.stdIconBtnAddLoanProgram.TabStop = false;
      this.stdIconBtnAddLoanProgram.Click += new EventHandler(this.addLoanProgramOrLockPlanCode);
      this.stdIconBtnEditLoanProgram.BackColor = Color.Transparent;
      this.stdIconBtnEditLoanProgram.Enabled = false;
      this.stdIconBtnEditLoanProgram.Location = new Point(380, 232);
      this.stdIconBtnEditLoanProgram.MouseDownImage = (Image) null;
      this.stdIconBtnEditLoanProgram.Name = "stdIconBtnEditLoanProgram";
      this.stdIconBtnEditLoanProgram.Size = new Size(16, 16);
      this.stdIconBtnEditLoanProgram.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEditLoanProgram.TabIndex = 75;
      this.stdIconBtnEditLoanProgram.TabStop = false;
      this.stdIconBtnEditLoanProgram.Click += new EventHandler(this.editLoanProgramOrLockPlanCode);
      this.stdIconBtnDeleteLoanProgram.BackColor = Color.Transparent;
      this.stdIconBtnDeleteLoanProgram.Enabled = false;
      this.stdIconBtnDeleteLoanProgram.Location = new Point(401, 232);
      this.stdIconBtnDeleteLoanProgram.MouseDownImage = (Image) null;
      this.stdIconBtnDeleteLoanProgram.Name = "stdIconBtnDeleteLoanProgram";
      this.stdIconBtnDeleteLoanProgram.Size = new Size(16, 16);
      this.stdIconBtnDeleteLoanProgram.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDeleteLoanProgram.TabIndex = 74;
      this.stdIconBtnDeleteLoanProgram.TabStop = false;
      this.stdIconBtnDeleteLoanProgram.Click += new EventHandler(this.deleteLoanProgramOrLockPlanCode);
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "LockPlanCode";
      gvColumn1.Text = "";
      gvColumn1.Width = 1550;
      this.gvLockPlanCode.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gvLockPlanCode.HeaderHeight = 0;
      this.gvLockPlanCode.HeaderVisible = false;
      this.gvLockPlanCode.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLockPlanCode.Location = new Point(438, 253);
      this.gvLockPlanCode.Name = "gvLockPlanCode";
      this.gvLockPlanCode.Size = new Size(408, 149);
      this.gvLockPlanCode.TabIndex = 21;
      this.gvLockPlanCode.SelectedIndexChanged += new EventHandler(this.gvLockPlanCode_SelectedIndexChanged);
      this.gvLockPlanCode.DoubleClick += new EventHandler(this.editLoanProgramOrLockPlanCode);
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ProgramName";
      gvColumn2.Text = "Column";
      gvColumn2.Width = 1550;
      this.gvLoanProg.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.gvLoanProg.HeaderHeight = 0;
      this.gvLoanProg.HeaderVisible = false;
      this.gvLoanProg.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLoanProg.Location = new Point(9, 253);
      this.gvLoanProg.Name = "gvLoanProg";
      this.gvLoanProg.Size = new Size(408, 150);
      this.gvLoanProg.TabIndex = 20;
      this.gvLoanProg.SelectedIndexChanged += new EventHandler(this.gvLoanProg_SelectedIndexChanged);
      this.gvLoanProg.DoubleClick += new EventHandler(this.editLoanProgramOrLockPlanCode);
      this.chkECLockPlanCode.AutoSize = true;
      this.chkECLockPlanCode.Location = new Point(438, 230);
      this.chkECLockPlanCode.Name = "chkECLockPlanCode";
      this.chkECLockPlanCode.Size = new Size(102, 17);
      this.chkECLockPlanCode.TabIndex = 17;
      this.chkECLockPlanCode.Text = "Lock Plan Code";
      this.chkECLockPlanCode.UseVisualStyleBackColor = true;
      this.chkECLockPlanCode.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.chkECLoanProg.AutoSize = true;
      this.chkECLoanProg.Location = new Point(9, 230);
      this.chkECLoanProg.Name = "chkECLoanProg";
      this.chkECLoanProg.Size = new Size(92, 17);
      this.chkECLoanProg.TabIndex = 16;
      this.chkECLoanProg.Text = "Loan Program";
      this.chkECLoanProg.UseVisualStyleBackColor = true;
      this.chkECLoanProg.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.lvLienPosition.CheckBoxes = true;
      this.lvLienPosition.Location = new Point(867, 82);
      this.lvLienPosition.Name = "lvLienPosition";
      this.lvLienPosition.Size = new Size(121, 124);
      this.lvLienPosition.TabIndex = 14;
      this.lvLienPosition.UseCompatibleStateImageBehavior = false;
      this.lvLienPosition.View = View.List;
      this.lvLienPosition.ItemChecked += new ItemCheckedEventHandler(this.listView_ItemChecked);
      this.chkECLienPosition.AutoSize = true;
      this.chkECLienPosition.Location = new Point(867, 59);
      this.chkECLienPosition.Name = "chkECLienPosition";
      this.chkECLienPosition.Size = new Size(86, 17);
      this.chkECLienPosition.TabIndex = 13;
      this.chkECLienPosition.Text = "Lien Position";
      this.chkECLienPosition.UseVisualStyleBackColor = true;
      this.chkECLienPosition.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.lvPropertyPurpose.CheckBoxes = true;
      this.lvPropertyPurpose.Location = new Point(725, 82);
      this.lvPropertyPurpose.Name = "lvPropertyPurpose";
      this.lvPropertyPurpose.Size = new Size(121, 124);
      this.lvPropertyPurpose.TabIndex = 12;
      this.lvPropertyPurpose.UseCompatibleStateImageBehavior = false;
      this.lvPropertyPurpose.View = View.List;
      this.lvPropertyPurpose.ItemChecked += new ItemCheckedEventHandler(this.listView_ItemChecked);
      this.chkECPropertyPurpose.AutoSize = true;
      this.chkECPropertyPurpose.Location = new Point(725, 59);
      this.chkECPropertyPurpose.Name = "chkECPropertyPurpose";
      this.chkECPropertyPurpose.Size = new Size(101, 17);
      this.chkECPropertyPurpose.TabIndex = 11;
      this.chkECPropertyPurpose.Text = "Property Will Be";
      this.chkECPropertyPurpose.UseVisualStyleBackColor = true;
      this.chkECPropertyPurpose.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.lvAmorType.CheckBoxes = true;
      this.lvAmorType.Location = new Point(581, 82);
      this.lvAmorType.Name = "lvAmorType";
      this.lvAmorType.Size = new Size(121, 124);
      this.lvAmorType.TabIndex = 10;
      this.lvAmorType.UseCompatibleStateImageBehavior = false;
      this.lvAmorType.View = View.List;
      this.lvAmorType.ItemChecked += new ItemCheckedEventHandler(this.listView_ItemChecked);
      this.chkECAmorType.AutoSize = true;
      this.chkECAmorType.Location = new Point(581, 59);
      this.chkECAmorType.Name = "chkECAmorType";
      this.chkECAmorType.Size = new Size(110, 17);
      this.chkECAmorType.TabIndex = 9;
      this.chkECAmorType.Text = "Amortization Type";
      this.chkECAmorType.UseVisualStyleBackColor = true;
      this.chkECAmorType.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.chkECLoanPurpose.AutoSize = true;
      this.chkECLoanPurpose.Location = new Point(438, 59);
      this.chkECLoanPurpose.Name = "chkECLoanPurpose";
      this.chkECLoanPurpose.Size = new Size(104, 17);
      this.chkECLoanPurpose.TabIndex = 8;
      this.chkECLoanPurpose.Text = "Purpose of Loan";
      this.chkECLoanPurpose.UseVisualStyleBackColor = true;
      this.chkECLoanPurpose.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.lvLoanPurpose.CheckBoxes = true;
      this.lvLoanPurpose.Location = new Point(438, 82);
      this.lvLoanPurpose.Name = "lvLoanPurpose";
      this.lvLoanPurpose.Size = new Size(121, 124);
      this.lvLoanPurpose.TabIndex = 7;
      this.lvLoanPurpose.UseCompatibleStateImageBehavior = false;
      this.lvLoanPurpose.View = View.List;
      this.lvLoanPurpose.ItemChecked += new ItemCheckedEventHandler(this.listView_ItemChecked);
      this.lvChannel.CheckBoxes = true;
      this.lvChannel.Location = new Point(296, 82);
      this.lvChannel.Name = "lvChannel";
      this.lvChannel.Size = new Size(121, 124);
      this.lvChannel.TabIndex = 6;
      this.lvChannel.UseCompatibleStateImageBehavior = false;
      this.lvChannel.View = View.List;
      this.lvChannel.ItemChecked += new ItemCheckedEventHandler(this.listView_ItemChecked);
      this.chkECChannel.AutoSize = true;
      this.chkECChannel.Location = new Point(296, 59);
      this.chkECChannel.Name = "chkECChannel";
      this.chkECChannel.Size = new Size(65, 17);
      this.chkECChannel.TabIndex = 5;
      this.chkECChannel.Text = "Channel";
      this.chkECChannel.UseVisualStyleBackColor = true;
      this.chkECChannel.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.lvPropertyState.CheckBoxes = true;
      this.lvPropertyState.FullRowSelect = true;
      this.lvPropertyState.HeaderStyle = ColumnHeaderStyle.None;
      this.lvPropertyState.Location = new Point(152, 82);
      this.lvPropertyState.Name = "lvPropertyState";
      this.lvPropertyState.Size = new Size(121, 124);
      this.lvPropertyState.TabIndex = 4;
      this.lvPropertyState.UseCompatibleStateImageBehavior = false;
      this.lvPropertyState.View = View.Details;
      this.lvPropertyState.ItemChecked += new ItemCheckedEventHandler(this.listView_ItemChecked);
      this.chkECPropertyState.AutoSize = true;
      this.chkECPropertyState.Location = new Point(152, 59);
      this.chkECPropertyState.Name = "chkECPropertyState";
      this.chkECPropertyState.Size = new Size(93, 17);
      this.chkECPropertyState.TabIndex = 3;
      this.chkECPropertyState.Text = "Property State";
      this.chkECPropertyState.UseVisualStyleBackColor = true;
      this.chkECPropertyState.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.chkECLoanType.AutoSize = true;
      this.chkECLoanType.Location = new Point(9, 59);
      this.chkECLoanType.Name = "chkECLoanType";
      this.chkECLoanType.Size = new Size(77, 17);
      this.chkECLoanType.TabIndex = 2;
      this.chkECLoanType.Text = "Loan Type";
      this.chkECLoanType.UseVisualStyleBackColor = true;
      this.chkECLoanType.CheckedChanged += new EventHandler(this.chkLoanCriteria_CheckedChanged);
      this.lvLoanType.CheckBoxes = true;
      this.lvLoanType.Location = new Point(9, 82);
      this.lvLoanType.Name = "lvLoanType";
      this.lvLoanType.Size = new Size(121, 124);
      this.lvLoanType.TabIndex = 1;
      this.lvLoanType.UseCompatibleStateImageBehavior = false;
      this.lvLoanType.View = View.List;
      this.lvLoanType.ItemChecked += new ItemCheckedEventHandler(this.listView_ItemChecked);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(4, 34);
      this.label3.Name = "label3";
      this.label3.Size = new Size(336, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Select which criteria should not be allowed for Auto-Lock and confirm.";
      this.gcAutoLockEnable.Controls.Add((Control) this.chkAutoCancelLockRequest);
      this.gcAutoLockEnable.Controls.Add((Control) this.chkAutoLockExtRequest);
      this.gcAutoLockEnable.Controls.Add((Control) this.chkAutoLockRelock);
      this.gcAutoLockEnable.Controls.Add((Control) this.chkAutoLockRequest);
      this.gcAutoLockEnable.Dock = DockStyle.Top;
      this.gcAutoLockEnable.HeaderForeColor = SystemColors.ControlText;
      this.gcAutoLockEnable.Location = new Point(0, 0);
      this.gcAutoLockEnable.Name = "gcAutoLockEnable";
      this.gcAutoLockEnable.Size = new Size(991, 132);
      this.gcAutoLockEnable.TabIndex = 2;
      this.gcAutoLockEnable.Text = "Auto-Lock Enablement";
      this.chkAutoCancelLockRequest.AutoSize = true;
      this.chkAutoCancelLockRequest.Location = new Point(9, 100);
      this.chkAutoCancelLockRequest.Name = "chkAutoCancelLockRequest";
      this.chkAutoCancelLockRequest.Size = new Size(273, 17);
      this.chkAutoCancelLockRequest.TabIndex = 5;
      this.chkAutoCancelLockRequest.Text = "Enable Auto Cancel Lock upon Cancellation request";
      this.chkAutoCancelLockRequest.UseVisualStyleBackColor = true;
      this.chkAutoCancelLockRequest.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkAutoLockExtRequest.AutoSize = true;
      this.chkAutoLockExtRequest.Location = new Point(9, 78);
      this.chkAutoLockExtRequest.Name = "chkAutoLockExtRequest";
      this.chkAutoLockExtRequest.Size = new Size(284, 17);
      this.chkAutoLockExtRequest.TabIndex = 4;
      this.chkAutoLockExtRequest.Text = "Enable Auto Lock and Confirm upon Extension request";
      this.chkAutoLockExtRequest.UseVisualStyleBackColor = true;
      this.chkAutoLockExtRequest.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkAutoLockRelock.AutoSize = true;
      this.chkAutoLockRelock.Location = new Point(27, 56);
      this.chkAutoLockRelock.Name = "chkAutoLockRelock";
      this.chkAutoLockRelock.Size = new Size(416, 17);
      this.chkAutoLockRelock.TabIndex = 3;
      this.chkAutoLockRelock.Text = "Enable Auto-Lock for Lock Updates (Active Locks) and Re-Locks (Inactive Locks)";
      this.chkAutoLockRelock.UseVisualStyleBackColor = true;
      this.chkAutoLockRelock.CheckedChanged += new EventHandler(this.chk_CheckedChanged);
      this.chkAutoLockRequest.AutoSize = true;
      this.chkAutoLockRequest.Location = new Point(9, 34);
      this.chkAutoLockRequest.Name = "chkAutoLockRequest";
      this.chkAutoLockRequest.Size = new Size(258, 17);
      this.chkAutoLockRequest.TabIndex = 2;
      this.chkAutoLockRequest.Text = "Enable Auto Lock and Confirm upon lock request";
      this.chkAutoLockRequest.UseVisualStyleBackColor = true;
      this.chkAutoLockRequest.CheckedChanged += new EventHandler(this.chkAutoLockRequest_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.AutoSize = true;
      this.Controls.Add((Control) this.panel2);
      this.Name = nameof (AutoLockSetup);
      this.Size = new Size(1008, 1000);
      this.Load += new EventHandler(this.AutoLockSetup_Load);
      this.panel2.ResumeLayout(false);
      this.gcAutoLockExCriteriaRules.ResumeLayout(false);
      this.gcAutoLockExCriteriaRules.PerformLayout();
      this.gcAutoLockExCriteria.ResumeLayout(false);
      this.gcAutoLockExCriteria.PerformLayout();
      ((ISupportInitialize) this.stdIconBtnAddLockPlan).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditLockPlan).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteLockPlan).EndInit();
      ((ISupportInitialize) this.stdIconBtnAddLoanProgram).EndInit();
      ((ISupportInitialize) this.stdIconBtnEditLoanProgram).EndInit();
      ((ISupportInitialize) this.stdIconBtnDeleteLoanProgram).EndInit();
      this.gcAutoLockEnable.ResumeLayout(false);
      this.gcAutoLockEnable.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
