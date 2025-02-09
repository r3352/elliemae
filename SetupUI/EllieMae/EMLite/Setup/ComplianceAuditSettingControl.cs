// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ComplianceAuditSettingControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ComplianceAuditSettingControl : SettingsUserControl
  {
    private const string className = "ComplianceAuditSettingControl";
    private static string sw = Tracing.SwEpass;
    private IContainer components;
    private GroupContainer grpClosingSettings;
    private FlowLayoutPanel flowLayoutPanel2;
    private CheckBox chkMDIA;
    private Label label3;
    private Panel panel1;
    private RadioButton radAPRIrregular;
    private RadioButton radAPRAll;
    private CheckBox chkFinanceCharge;
    private Label label2;
    private TextBox txtAdminEmail;
    private ComboBox cboChannel;
    private Label label1;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private CheckBox chkFederalHighCost;
    private GridView gvStates;

    public ComplianceAuditSettingControl(SetUpContainer container)
      : base(container)
    {
      this.InitializeComponent();
      this.initGVStates();
      this.refreshClosingDocSettings();
      this.refreshHighCostSettings();
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      this.refreshClosingDocSettings();
      this.refreshHighCostSettings();
      base.Reset();
    }

    private void refreshClosingDocSettings()
    {
      Hashtable docSettingsFromEcs = Session.Application.GetService<ILoanServices>().GetClosingDocSettingsFromECS();
      if (docSettingsFromEcs == null)
      {
        this.txtAdminEmail.Text = "";
        this.cboChannel.SelectedIndex = -1;
        this.chkFinanceCharge.Checked = false;
        this.chkMDIA.Checked = false;
        this.radAPRIrregular.Checked = true;
      }
      else
      {
        this.txtAdminEmail.Text = string.Concat(docSettingsFromEcs[(object) "AdminEmailAddress"]);
        ClientCommonUtils.PopulateDropdown(this.cboChannel, (object) string.Concat(docSettingsFromEcs[(object) "DefaultChannel"]), false);
        this.chkFinanceCharge.Checked = string.Concat(docSettingsFromEcs[(object) "UnmappedFeeHandlerIndicator"]) == "Y";
        this.chkMDIA.Checked = string.Concat(docSettingsFromEcs[(object) "TILAMDIAIndicator"]) == "Y";
        this.radAPRAll.Checked = string.Concat(docSettingsFromEcs[(object) "TILARateForAllLoansIndicator"]) == "Y";
        this.radAPRIrregular.Checked = !this.radAPRAll.Checked;
      }
    }

    private void initGVStates()
    {
      this.gvStates.Items.Clear();
      foreach (string fullStateName in Utils.GetFullStateNames())
        this.gvStates.Items.Add(new GVItem()
        {
          SubItems = {
            [1] = {
              Value = (object) fullStateName
            }
          },
          Tag = (object) fullStateName
        });
    }

    private void refreshHighCostSettings()
    {
      List<string> highCostStates = Session.ConfigurationManager.GetHighCostStates();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStates.Items)
        gvItem.Checked = highCostStates.Contains(Utils.GetStateAbbr(string.Concat(gvItem.Tag)));
      if (highCostStates.Contains("FD"))
        this.chkFederalHighCost.Checked = true;
      else
        this.chkFederalHighCost.Checked = false;
    }

    private void saveClosingDocSettings()
    {
      Hashtable settingsToSave = new Hashtable();
      settingsToSave[(object) "AdminEmailAddress"] = (object) this.txtAdminEmail.Text;
      if (this.cboChannel.SelectedItem != null)
        settingsToSave[(object) "DefaultChannel"] = (object) this.cboChannel.SelectedItem.ToString();
      settingsToSave[(object) "UnmappedFeeHandlerIndicator"] = this.chkFinanceCharge.Checked ? (object) "Y" : (object) "N";
      settingsToSave[(object) "TILAMDIAIndicator"] = this.chkMDIA.Checked ? (object) "Y" : (object) "N";
      settingsToSave[(object) "TILARateForAllLoansIndicator"] = this.radAPRAll.Checked ? (object) "Y" : (object) "N";
      try
      {
        Session.Application.GetService<ILoanServices>().SetClosingDocSettingsToECS(settingsToSave);
      }
      catch (Exception ex)
      {
        Tracing.Log(ComplianceAuditSettingControl.sw, nameof (ComplianceAuditSettingControl), TraceLevel.Error, "Error saving ECS settings: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to save your settings: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void saveHighCosts()
    {
      List<string> stateList = new List<string>();
      if (this.chkFederalHighCost.Checked)
        stateList.Add("FD");
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStates.Items)
      {
        if (gvItem.Checked)
          stateList.Add(Utils.GetStateAbbr(string.Concat(gvItem.Tag)));
      }
      Session.ConfigurationManager.UpdateHighCostStates(stateList);
    }

    public override void Save()
    {
      this.saveClosingDocSettings();
      this.saveHighCosts();
      base.Save();
    }

    private void onSettingsValueChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void gvStates_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.onSettingsValueChanged((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.grpClosingSettings = new GroupContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.chkMDIA = new CheckBox();
      this.label3 = new Label();
      this.panel1 = new Panel();
      this.radAPRIrregular = new RadioButton();
      this.radAPRAll = new RadioButton();
      this.chkFinanceCharge = new CheckBox();
      this.label2 = new Label();
      this.txtAdminEmail = new TextBox();
      this.cboChannel = new ComboBox();
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.chkFederalHighCost = new CheckBox();
      this.groupContainer2 = new GroupContainer();
      this.gvStates = new GridView();
      this.grpClosingSettings.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.grpClosingSettings.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpClosingSettings.Controls.Add((Control) this.flowLayoutPanel2);
      this.grpClosingSettings.Controls.Add((Control) this.chkMDIA);
      this.grpClosingSettings.Controls.Add((Control) this.label3);
      this.grpClosingSettings.Controls.Add((Control) this.panel1);
      this.grpClosingSettings.Controls.Add((Control) this.chkFinanceCharge);
      this.grpClosingSettings.Controls.Add((Control) this.label2);
      this.grpClosingSettings.Controls.Add((Control) this.txtAdminEmail);
      this.grpClosingSettings.Controls.Add((Control) this.cboChannel);
      this.grpClosingSettings.Controls.Add((Control) this.label1);
      this.grpClosingSettings.Dock = DockStyle.Top;
      this.grpClosingSettings.HeaderForeColor = SystemColors.ControlText;
      this.grpClosingSettings.Location = new Point(0, 0);
      this.grpClosingSettings.Name = "grpClosingSettings";
      this.grpClosingSettings.Size = new Size(935, 184);
      this.grpClosingSettings.TabIndex = 2;
      this.grpClosingSettings.Text = "Compliance Audit Settings";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.AutoSize = true;
      this.flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(929, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(0, 0);
      this.flowLayoutPanel2.TabIndex = 9;
      this.chkMDIA.AutoSize = true;
      this.chkMDIA.Location = new Point(97, 101);
      this.chkMDIA.Name = "chkMDIA";
      this.chkMDIA.Size = new Size(110, 17);
      this.chkMDIA.TabIndex = 8;
      this.chkMDIA.Text = "Run MDIA Check";
      this.chkMDIA.UseVisualStyleBackColor = true;
      this.chkMDIA.CheckedChanged += new EventHandler(this.onSettingsValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 129);
      this.label3.Name = "label3";
      this.label3.Size = new Size(179, 13);
      this.label3.TabIndex = 7;
      this.label3.Text = "APR and Finance Charge Tolerance";
      this.panel1.Controls.Add((Control) this.radAPRIrregular);
      this.panel1.Controls.Add((Control) this.radAPRAll);
      this.panel1.Location = new Point(194, (int) sbyte.MaxValue);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(314, 40);
      this.panel1.TabIndex = 6;
      this.radAPRIrregular.AutoSize = true;
      this.radAPRIrregular.Location = new Point(0, 20);
      this.radAPRIrregular.Name = "radAPRIrregular";
      this.radAPRIrregular.Size = new Size(274, 17);
      this.radAPRIrregular.TabIndex = 6;
      this.radAPRIrregular.TabStop = true;
      this.radAPRIrregular.Text = "0.125% for regular loans and 0.25% for irregular loans";
      this.radAPRIrregular.UseVisualStyleBackColor = true;
      this.radAPRIrregular.CheckedChanged += new EventHandler(this.onSettingsValueChanged);
      this.radAPRAll.AutoSize = true;
      this.radAPRAll.Location = new Point(0, 0);
      this.radAPRAll.Name = "radAPRAll";
      this.radAPRAll.Size = new Size(116, 17);
      this.radAPRAll.TabIndex = 5;
      this.radAPRAll.TabStop = true;
      this.radAPRAll.Text = "0.125% for all loans";
      this.radAPRAll.UseVisualStyleBackColor = true;
      this.radAPRAll.CheckedChanged += new EventHandler(this.onSettingsValueChanged);
      this.chkFinanceCharge.AutoSize = true;
      this.chkFinanceCharge.Location = new Point(97, 82);
      this.chkFinanceCharge.Name = "chkFinanceCharge";
      this.chkFinanceCharge.Size = new Size(321, 17);
      this.chkFinanceCharge.TabIndex = 4;
      this.chkFinanceCharge.Text = "Use Encompass Finance Charge Indicator for Unmapped Fees";
      this.chkFinanceCharge.UseVisualStyleBackColor = true;
      this.chkFinanceCharge.CheckedChanged += new EventHandler(this.onSettingsValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 63);
      this.label2.Name = "label2";
      this.label2.Size = new Size(64, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Admin Email";
      this.txtAdminEmail.Location = new Point(97, 60);
      this.txtAdminEmail.Name = "txtAdminEmail";
      this.txtAdminEmail.Size = new Size(192, 20);
      this.txtAdminEmail.TabIndex = 2;
      this.txtAdminEmail.TextChanged += new EventHandler(this.onSettingsValueChanged);
      this.cboChannel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboChannel.FormattingEnabled = true;
      this.cboChannel.Items.AddRange(new object[4]
      {
        (object) "Banked - Retail",
        (object) "Banked - Wholesale",
        (object) "Brokered",
        (object) "Correspondent"
      });
      this.cboChannel.Location = new Point(97, 36);
      this.cboChannel.Name = "cboChannel";
      this.cboChannel.Size = new Size(153, 21);
      this.cboChannel.TabIndex = 1;
      this.cboChannel.SelectedIndexChanged += new EventHandler(this.onSettingsValueChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 41);
      this.label1.Name = "label1";
      this.label1.Size = new Size(83, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Default Channel";
      this.groupContainer1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.chkFederalHighCost);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 184);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(935, 55);
      this.groupContainer1.TabIndex = 3;
      this.groupContainer1.Text = "Federal High-Cost Loans";
      this.chkFederalHighCost.AutoSize = true;
      this.chkFederalHighCost.Location = new Point(11, 30);
      this.chkFederalHighCost.Name = "chkFederalHighCost";
      this.chkFederalHighCost.Size = new Size(230, 17);
      this.chkFederalHighCost.TabIndex = 0;
      this.chkFederalHighCost.Text = "Allow Federal High-Cost (Section 32) Loans";
      this.chkFederalHighCost.UseVisualStyleBackColor = true;
      this.chkFederalHighCost.CheckedChanged += new EventHandler(this.onSettingsValueChanged);
      this.groupContainer2.Controls.Add((Control) this.gvStates);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 239);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(935, 314);
      this.groupContainer2.TabIndex = 4;
      this.groupContainer2.Text = "State/Local High-Cost Loans";
      this.gvStates.AllowColumnResize = false;
      this.gvStates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ColumnHeaderCheckbox = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column2";
      gvColumn1.SortMethod = GVSortMethod.None;
      gvColumn1.Text = "";
      gvColumn1.Width = 25;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "State";
      gvColumn2.Width = 908;
      this.gvStates.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvStates.Dock = DockStyle.Fill;
      this.gvStates.Location = new Point(1, 26);
      this.gvStates.Name = "gvStates";
      this.gvStates.Size = new Size(933, 287);
      this.gvStates.TabIndex = 0;
      this.gvStates.SubItemCheck += new GVSubItemEventHandler(this.gvStates_SubItemCheck);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.grpClosingSettings);
      this.Name = nameof (ComplianceAuditSettingControl);
      this.Size = new Size(935, 553);
      this.grpClosingSettings.ResumeLayout(false);
      this.grpClosingSettings.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
