// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.NMLSReportSettingsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class NMLSReportSettingsControl : SettingsUserControl
  {
    private Sessions.Session session;
    private NMLSReportSettings reportSettings;
    private LoanXDBTableList xdbTables;
    private NMLSReportSettingsControl.FieldDropdownItem currentAppDateSelection;
    private IContainer components;
    private GroupContainer grpStates;
    private GridView gvStates;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnUncheckAllStates;
    private Button btnCheckAllStates;
    private BorderPanel borderPanel1;
    private Label label1;
    private ComboBox cboAppDate;
    private GroupContainer groupContainer1;
    private CheckBox chkExcludeCredits;

    public NMLSReportSettingsControl(Sessions.Session session, SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      this.Reset();
      this.gvStates.Sort(1, SortOrder.Ascending);
    }

    public override void Reset()
    {
      this.reportSettings = new NMLSReportSettings(this.session.SessionObjects);
      this.xdbTables = this.session.LoanManager.GetLoanXDBTableList(false);
      string[] configuredStateList = this.reportSettings.GetConfiguredStateList();
      this.gvStates.Items.Clear();
      foreach (string state in Utils.GetStates())
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems[0].Text = state;
        gvItem.SubItems[1].Text = Utils.GetFullStateName(state);
        gvItem.Tag = (object) state;
        this.gvStates.Items.Add(gvItem);
        if (Array.IndexOf<string>(configuredStateList, state) >= 0)
          gvItem.Checked = true;
      }
      this.gvStates.ReSort();
      this.loadApplicationSetting();
      base.Reset();
    }

    private void loadApplicationSetting()
    {
      this.cboAppDate.Items.Clear();
      this.cboAppDate.Items.Add((object) new NMLSReportSettingsControl.FieldDropdownItem("3261", "Date when the 1003 form was signed"));
      this.cboAppDate.Items.Add((object) new NMLSReportSettingsControl.FieldDropdownItem("3142", "GFE Application Date"));
      this.cboAppDate.Items.Add((object) new NMLSReportSettingsControl.FieldDropdownItem("745", "Application Date"));
      string applicationDateFieldId = this.reportSettings.ApplicationDateFieldID;
      if (this.reportSettings.ApplicationDateFieldID != "3261" && this.reportSettings.ApplicationDateFieldID != "3142" && this.reportSettings.ApplicationDateFieldID != "745")
      {
        FieldSettings fieldSettings = this.session.LoanManager.GetFieldSettings();
        FieldDefinition field = EncompassFields.GetField(applicationDateFieldId, fieldSettings, true);
        if (field != null)
          this.cboAppDate.Items.Add((object) new NMLSReportSettingsControl.FieldDropdownItem(field.FieldID, field.Description));
        else
          this.cboAppDate.Items.Add((object) new NMLSReportSettingsControl.FieldDropdownItem(field.FieldID, "Unknown field"));
      }
      this.cboAppDate.Items.Add((object) "Other...");
      foreach (object obj in this.cboAppDate.Items)
      {
        if (obj is NMLSReportSettingsControl.FieldDropdownItem && string.Compare(((NMLSReportSettingsControl.FieldDropdownItem) obj).FieldID, applicationDateFieldId, true) == 0)
        {
          this.cboAppDate.SelectedItem = obj;
          break;
        }
      }
      this.chkExcludeCredits.Checked = this.reportSettings.ExcludeOriginationCredit;
    }

    private void btnCheckAllStates_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStates.Items)
        gvItem.Checked = true;
      this.setDirtyFlag(true);
    }

    private void btnUncheckAllStates_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvStates.Items)
        gvItem.Checked = false;
      this.setDirtyFlag(true);
    }

    public override void Save()
    {
      GVItem[] checkedItems = this.gvStates.GetCheckedItems(0);
      if (!(this.cboAppDate.SelectedItem is NMLSReportSettingsControl.FieldDropdownItem selectedItem))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select the field to be used as the Application Date in the NMLS Call Report.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        List<string> stringList = new List<string>();
        foreach (GVItem gvItem in checkedItems)
          stringList.Add(gvItem.Tag.ToString());
        this.reportSettings.SetConfiguredStateList(stringList.ToArray());
        this.reportSettings.SetApplicationDateFieldID(selectedItem.FieldID);
        this.reportSettings.SetExcludeOriginationCredits(this.chkExcludeCredits.Checked);
        base.Save();
      }
    }

    private void gvStates_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void chkExcludeCredits_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void cboAppDate_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboAppDate.SelectedItem == null)
        return;
      if (!(this.cboAppDate.SelectedItem is NMLSReportSettingsControl.FieldDropdownItem fieldDropdownItem))
        fieldDropdownItem = this.selectField();
      if (fieldDropdownItem == null)
        fieldDropdownItem = this.currentAppDateSelection;
      if (!this.cboAppDate.Items.Contains((object) fieldDropdownItem))
        this.cboAppDate.Items.Insert(this.cboAppDate.Items.Count - 1, (object) fieldDropdownItem);
      if (!this.cboAppDate.SelectedItem.Equals((object) fieldDropdownItem))
        this.cboAppDate.SelectedItem = (object) fieldDropdownItem;
      if (fieldDropdownItem.Equals((object) this.currentAppDateSelection))
        return;
      this.currentAppDateSelection = fieldDropdownItem;
      this.setDirtyFlag(true);
    }

    private NMLSReportSettingsControl.FieldDropdownItem selectField()
    {
      using (ReportFieldSelector reportFieldSelector = new ReportFieldSelector((ReportFieldDefs) LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.LoanDataFieldsInDatabase), new EllieMae.EMLite.ClientServer.Reporting.FieldTypes[1]
      {
        EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate
      }, true, true, this.session))
        return reportFieldSelector.ShowDialog((IWin32Window) this) != DialogResult.OK ? (NMLSReportSettingsControl.FieldDropdownItem) null : new NMLSReportSettingsControl.FieldDropdownItem(reportFieldSelector.SelectedField.FieldID, reportFieldSelector.SelectedField.FieldDefinition.Description);
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
      this.grpStates = new GroupContainer();
      this.gvStates = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnUncheckAllStates = new Button();
      this.btnCheckAllStates = new Button();
      this.borderPanel1 = new BorderPanel();
      this.groupContainer1 = new GroupContainer();
      this.chkExcludeCredits = new CheckBox();
      this.cboAppDate = new ComboBox();
      this.label1 = new Label();
      this.grpStates.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.grpStates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
      this.grpStates.Controls.Add((Control) this.gvStates);
      this.grpStates.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpStates.HeaderForeColor = SystemColors.ControlText;
      this.grpStates.Location = new Point(10, 10);
      this.grpStates.Name = "grpStates";
      this.grpStates.Size = new Size(276, 506);
      this.grpStates.TabIndex = 0;
      this.grpStates.Text = "States";
      this.gvStates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Abbr";
      gvColumn1.Width = 60;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Name";
      gvColumn2.Width = 214;
      this.gvStates.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvStates.Dock = DockStyle.Fill;
      this.gvStates.Location = new Point(1, 26);
      this.gvStates.Name = "gvStates";
      this.gvStates.Size = new Size(274, 479);
      this.gvStates.TabIndex = 1;
      this.gvStates.SubItemCheck += new GVSubItemEventHandler(this.gvStates_SubItemCheck);
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnUncheckAllStates);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCheckAllStates);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(106, 2);
      this.flowLayoutPanel1.Margin = new Padding(3, 3, 2, 3);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(164, 22);
      this.flowLayoutPanel1.TabIndex = 0;
      this.btnUncheckAllStates.Location = new Point(85, 0);
      this.btnUncheckAllStates.Margin = new Padding(0);
      this.btnUncheckAllStates.Name = "btnUncheckAllStates";
      this.btnUncheckAllStates.Size = new Size(79, 22);
      this.btnUncheckAllStates.TabIndex = 0;
      this.btnUncheckAllStates.Text = "Uncheck All";
      this.btnUncheckAllStates.UseVisualStyleBackColor = true;
      this.btnUncheckAllStates.Click += new EventHandler(this.btnUncheckAllStates_Click);
      this.btnCheckAllStates.Location = new Point(6, 0);
      this.btnCheckAllStates.Margin = new Padding(0);
      this.btnCheckAllStates.Name = "btnCheckAllStates";
      this.btnCheckAllStates.Size = new Size(79, 22);
      this.btnCheckAllStates.TabIndex = 1;
      this.btnCheckAllStates.Text = "Check All";
      this.btnCheckAllStates.UseVisualStyleBackColor = true;
      this.btnCheckAllStates.Click += new EventHandler(this.btnCheckAllStates_Click);
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Controls.Add((Control) this.groupContainer1);
      this.borderPanel1.Controls.Add((Control) this.grpStates);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 0);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(718, 528);
      this.borderPanel1.TabIndex = 1;
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.chkExcludeCredits);
      this.groupContainer1.Controls.Add((Control) this.cboAppDate);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(291, 10);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(417, 506);
      this.groupContainer1.TabIndex = 3;
      this.groupContainer1.Text = "Report Settings";
      this.chkExcludeCredits.AutoSize = true;
      this.chkExcludeCredits.Location = new Point(12, 68);
      this.chkExcludeCredits.Name = "chkExcludeCredits";
      this.chkExcludeCredits.Size = new Size(254, 18);
      this.chkExcludeCredits.TabIndex = 2;
      this.chkExcludeCredits.Text = "Exclude Origination Credits from Fees Collected";
      this.chkExcludeCredits.UseVisualStyleBackColor = true;
      this.chkExcludeCredits.CheckedChanged += new EventHandler(this.chkExcludeCredits_CheckedChanged);
      this.cboAppDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboAppDate.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAppDate.FormattingEnabled = true;
      this.cboAppDate.Location = new Point(124, 36);
      this.cboAppDate.Name = "cboAppDate";
      this.cboAppDate.Size = new Size(283, 22);
      this.cboAppDate.TabIndex = 1;
      this.cboAppDate.SelectedIndexChanged += new EventHandler(this.cboAppDate_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 41);
      this.label1.Name = "label1";
      this.label1.Size = new Size(110, 14);
      this.label1.TabIndex = 2;
      this.label1.Text = "Initial Application Date";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.borderPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (NMLSReportSettingsControl);
      this.Size = new Size(718, 528);
      this.grpStates.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    private class FieldDropdownItem
    {
      private string fieldId;
      private string description;

      public FieldDropdownItem(string fieldId, string description)
      {
        this.fieldId = fieldId;
        this.description = description;
      }

      public string FieldID => this.fieldId;

      public override string ToString() => this.fieldId + " - " + this.description;

      public override bool Equals(object obj)
      {
        return obj is NMLSReportSettingsControl.FieldDropdownItem fieldDropdownItem && string.Compare(fieldDropdownItem.FieldID, this.FieldID, true) == 0;
      }

      public override int GetHashCode() => this.fieldId.ToLower().GetHashCode();
    }
  }
}
