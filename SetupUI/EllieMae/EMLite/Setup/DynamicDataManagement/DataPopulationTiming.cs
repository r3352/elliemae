// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DataPopulationTiming
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DataPopulationTiming : SettingsUserControl, IOnlineHelpTarget
  {
    private Sessions.Session session;
    private ReportFieldDefs fieldDefs;
    private string advancedCodeXml;
    private bool loanSaveChk;
    private bool fieldChangesChk;
    private List<DDMDataPopTimingField> fieldList;
    private bool afterLoanInitEst;
    private bool loanCondMet;
    private string loanCondMetCon;
    private ToolTip toolTip;
    private IContainer components;
    private GroupContainer groupContainer1;
    private GroupContainer gcFields;
    private StandardIconButton stdIconBtnRefresh;
    private VerticalSeparator verticalSeparator1;
    private GridView listViewOptions;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private CheckBox fieldChanges;
    private CheckBox loanSave;
    private GroupContainer groupContainer2;
    private StandardIconButton btnSearch;
    private TextBox afterCondition;
    private CheckBox advCond;
    private CheckBox afterEstSent;
    private Label label3;
    private Label label4;
    private DDMDataPopTimingBpmManager dataPopTimingsBpmMgr;

    public DataPopulationTiming(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.fieldDefs = (ReportFieldDefs) LoanReportFieldDefs.GetFieldDefs(this.session, LoanReportFieldFlags.BasicLoanDataFields);
      this.fieldList = new List<DDMDataPopTimingField>();
      this.dataPopTimingsBpmMgr = (DDMDataPopTimingBpmManager) null;
      this.RefreshPage();
      this.setDirtyFlag(false);
    }

    public override void Save() => this.SaveRtnBool();

    public override bool SaveRtnBool()
    {
      if (this.fieldList != null)
        this.fieldList.Clear();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewOptions.Items)
        this.AddFieldsToList(gvItem.SubItems[0].Text, gvItem.SubItems[1].Text, Convert.ToInt32(gvItem.SubItems[2].Text));
      this.loanSaveChk = this.loanSave.Checked;
      this.fieldChangesChk = this.fieldChanges.Checked;
      this.afterLoanInitEst = this.afterEstSent.Checked;
      this.loanCondMet = this.advCond.Checked;
      this.loanCondMetCon = this.afterCondition.Text;
      if (!this.Validate())
        return false;
      DDMDataPopulationTiming ddmDataPopTiming = new DDMDataPopulationTiming(this.session.UserID, this.loanSaveChk, this.fieldChangesChk, this.fieldList, this.afterLoanInitEst, this.loanCondMet, this.loanCondMetCon, this.advancedCodeXml);
      this.dataPopTimingsBpmMgr = (DDMDataPopTimingBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataPopTiming);
      this.dataPopTimingsBpmMgr.UpdateDDMDataPopulationTiming(ddmDataPopTiming, true);
      this.session.dataPopTiming = ddmDataPopTiming;
      this.setDirtyFlag(false);
      return true;
    }

    private new bool Validate()
    {
      bool flag = true;
      if (this.fieldChanges.Checked && this.listViewOptions.Items.Count == 0)
      {
        flag = false;
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide at least one field under 'Field Changes' to start automated data population");
      }
      if (this.advCond.Checked && string.IsNullOrEmpty(this.afterCondition.Text))
      {
        flag = false;
        int num = (int) Utils.Dialog((IWin32Window) this, "Please provide conditions to stop automated data population");
      }
      return flag;
    }

    private void RefreshReferenceCount()
    {
      if (this.dataPopTimingsBpmMgr == null)
        this.dataPopTimingsBpmMgr = (DDMDataPopTimingBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataPopTiming);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewOptions.Items)
      {
        string str = this.dataPopTimingsBpmMgr.GetNumberReferences(gvItem.SubItems[0].Text, true).ToString();
        if (str != gvItem.SubItems[2].Text)
        {
          gvItem.SubItems[2].Text = str;
          this.setDirtyFlag(true);
        }
      }
    }

    public override void Reset()
    {
      this.RefreshPage();
      base.Reset();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.toolTip = new ToolTip(this.components);
      this.btnSearch = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.groupContainer2 = new GroupContainer();
      this.label4 = new Label();
      this.label3 = new Label();
      this.afterCondition = new TextBox();
      this.advCond = new CheckBox();
      this.afterEstSent = new CheckBox();
      this.groupContainer1 = new GroupContainer();
      this.gcFields = new GroupContainer();
      this.stdIconBtnRefresh = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.listViewOptions = new GridView();
      this.fieldChanges = new CheckBox();
      this.loanSave = new CheckBox();
      ((ISupportInitialize) this.btnSearch).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.groupContainer2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.gcFields.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnRefresh).BeginInit();
      this.SuspendLayout();
      this.btnSearch.BackColor = Color.Transparent;
      this.btnSearch.Enabled = false;
      this.btnSearch.Location = new Point(579, 135);
      this.btnSearch.MouseDownImage = (Image) null;
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(16, 16);
      this.btnSearch.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnSearch.TabIndex = 19;
      this.btnSearch.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnSearch, "Edit Condition");
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Enabled = false;
      this.stdIconBtnNew.Location = new Point(490, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 79;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Enabled = false;
      this.stdIconBtnDelete.Location = new Point(512, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 74;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.groupContainer2.Controls.Add((Control) this.label4);
      this.groupContainer2.Controls.Add((Control) this.label3);
      this.groupContainer2.Controls.Add((Control) this.btnSearch);
      this.groupContainer2.Controls.Add((Control) this.afterCondition);
      this.groupContainer2.Controls.Add((Control) this.advCond);
      this.groupContainer2.Controls.Add((Control) this.afterEstSent);
      this.groupContainer2.Dock = DockStyle.Bottom;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 338);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(937, 448);
      this.groupContainer2.TabIndex = 15;
      this.groupContainer2.Text = "Stop applying Dynamic Data Management Rules";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(33, 83);
      this.label4.Name = "label4";
      this.label4.Size = new Size(420, 13);
      this.label4.TabIndex = 22;
      this.label4.Text = "The DDM Rules will stop firing after Initial Loan Estimate is sent and Field ID 3152 is set.";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 38);
      this.label3.Name = "label3";
      this.label3.Size = new Size(775, 13);
      this.label3.TabIndex = 21;
      this.label3.Text = "The settings below are default settings that apply when a new DDM Fee rule or Field rule is created. For a DDM rule to not be applied to a loan, it must satisfy the below stop condition and the stop condition at the rule level.";
      this.afterCondition.Location = new Point(15, 135);
      this.afterCondition.Multiline = true;
      this.afterCondition.Name = "afterCondition";
      this.afterCondition.ReadOnly = true;
      this.afterCondition.Size = new Size(548, 90);
      this.afterCondition.TabIndex = 20;
      this.afterCondition.TabStop = false;
      this.advCond.AutoSize = true;
      this.advCond.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.advCond.Location = new Point(15, 110);
      this.advCond.Name = "advCond";
      this.advCond.Size = new Size(211, 17);
      this.advCond.TabIndex = 8;
      this.advCond.Text = "When the condition below is met";
      this.advCond.UseVisualStyleBackColor = true;
      this.advCond.CheckedChanged += new EventHandler(this.after_CheckedChanged);
      this.afterEstSent.AutoSize = true;
      this.afterEstSent.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.afterEstSent.Location = new Point(15, 63);
      this.afterEstSent.Name = "afterEstSent";
      this.afterEstSent.Size = new Size(213, 17);
      this.afterEstSent.TabIndex = 7;
      this.afterEstSent.Text = "After Initial Loan Estimate is sent";
      this.afterEstSent.UseVisualStyleBackColor = true;
      this.afterEstSent.CheckedChanged += new EventHandler(this.afterEstSent_CheckedChanged);
      this.groupContainer1.Borders = AnchorStyles.None;
      this.groupContainer1.Controls.Add((Control) this.gcFields);
      this.groupContainer1.Controls.Add((Control) this.fieldChanges);
      this.groupContainer1.Controls.Add((Control) this.loanSave);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(937, 338);
      this.groupContainer1.TabIndex = 14;
      this.groupContainer1.Text = "Start applying Dynamic Data Management Rules";
      this.gcFields.AutoScroll = true;
      this.gcFields.Controls.Add((Control) this.stdIconBtnRefresh);
      this.gcFields.Controls.Add((Control) this.verticalSeparator1);
      this.gcFields.Controls.Add((Control) this.listViewOptions);
      this.gcFields.Controls.Add((Control) this.stdIconBtnNew);
      this.gcFields.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcFields.HeaderForeColor = SystemColors.ControlText;
      this.gcFields.Location = new Point(24, 113);
      this.gcFields.Name = "gcFields";
      this.gcFields.Size = new Size(565, 207);
      this.gcFields.TabIndex = 11;
      this.gcFields.Text = "Fields";
      this.stdIconBtnRefresh.BackColor = Color.Transparent;
      this.stdIconBtnRefresh.Enabled = false;
      this.stdIconBtnRefresh.Location = new Point(543, 5);
      this.stdIconBtnRefresh.MouseDownImage = (Image) null;
      this.stdIconBtnRefresh.Name = "stdIconBtnRefresh";
      this.stdIconBtnRefresh.Size = new Size(16, 16);
      this.stdIconBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.stdIconBtnRefresh.TabIndex = 81;
      this.stdIconBtnRefresh.TabStop = false;
      this.stdIconBtnRefresh.Click += new EventHandler(this.stdIconBtnRefresh_Click);
      this.verticalSeparator1.Location = new Point(535, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 80;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.listViewOptions.AutoHeight = true;
      this.listViewOptions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 120;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Description";
      gvColumn2.Width = 303;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "# Scenarios Referenced";
      gvColumn3.Width = 140;
      this.listViewOptions.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.listViewOptions.Dock = DockStyle.Fill;
      this.listViewOptions.HeaderHeight = 22;
      this.listViewOptions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewOptions.Location = new Point(1, 26);
      this.listViewOptions.Name = "listViewOptions";
      this.listViewOptions.Size = new Size(563, 180);
      this.listViewOptions.SortingType = SortingType.AlphaNumeric;
      this.listViewOptions.TabIndex = 4;
      this.fieldChanges.AutoSize = true;
      this.fieldChanges.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.fieldChanges.Location = new Point(5, 80);
      this.fieldChanges.Name = "fieldChanges";
      this.fieldChanges.Size = new Size(312, 17);
      this.fieldChanges.TabIndex = 3;
      this.fieldChanges.Text = "When the user modifies any of the following fields.";
      this.fieldChanges.UseVisualStyleBackColor = true;
      this.fieldChanges.CheckedChanged += new EventHandler(this.fieldChanges_CheckedChanged);
      this.loanSave.AutoSize = true;
      this.loanSave.Checked = true;
      this.loanSave.CheckState = CheckState.Checked;
      this.loanSave.Enabled = false;
      this.loanSave.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.loanSave.Location = new Point(5, 43);
      this.loanSave.Name = "loanSave";
      this.loanSave.Size = new Size(222, 17);
      this.loanSave.TabIndex = 1;
      this.loanSave.Text = "When the loan is created or saved";
      this.loanSave.UseVisualStyleBackColor = true;
      this.loanSave.CheckedChanged += new EventHandler(this.LoanSave_CheckedChanged);
      this.AutoScroll = true;
      this.BorderStyle = BorderStyle.FixedSingle;
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (DataPopulationTiming);
      this.Size = new Size(937, 752);
      this.Leave += new EventHandler(this.DataPopulationTiming_Leave);
      ((ISupportInitialize) this.btnSearch).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.gcFields.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnRefresh).EndInit();
      this.ResumeLayout(false);
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      using (AddEditFilterDlg addEditFilterDlg = new AddEditFilterDlg(this.fieldDefs))
      {
        if (addEditFilterDlg.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        ReportFieldDef def = addEditFilterDlg.FieldDef;
        GVItem gvItem = new GVItem();
        gvItem.SubItems[0].Text = def.FieldID;
        gvItem.SubItems[1].Text = def.Description;
        if (!this.listViewOptions.Items.All<GVItem>((Func<GVItem, bool>) (x => x.SubItems[0].Text != def.FieldID)))
          return;
        DDMDataPopTimingBpmManager bpmManager = (DDMDataPopTimingBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataPopTiming);
        gvItem.SubItems[2].Text = bpmManager.GetNumberReferences(def.FieldID, true).ToString();
        gvItem.Tag = (object) def;
        this.listViewOptions.Items.Add(gvItem);
        this.setDirtyFlag(true);
      }
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.Items.Count <= 0 || this.listViewOptions.SelectedItems.Count <= 0)
        return;
      this.listViewOptions.Items.Remove(this.listViewOptions.SelectedItems[0]);
      this.listViewOptions.Refresh();
      this.setDirtyFlag(true);
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
      using (AdvConditionEditor advConditionEditor = new AdvConditionEditor(this.session, this.advancedCodeXml, true))
      {
        if (advConditionEditor.GetConditionScript() != this.afterCondition.Text)
          advConditionEditor.ClearFilters();
        if (advConditionEditor.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.afterCondition.Text = advConditionEditor.GetConditionScript();
        this.advancedCodeXml = advConditionEditor.GetConditionXml();
        this.setDirtyFlag(true);
      }
    }

    private void usrRequest_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void LoanSave_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void fieldChanges_CheckedChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
      this.stdIconBtnDelete.Enabled = this.stdIconBtnNew.Enabled = this.stdIconBtnRefresh.Enabled = this.fieldChanges.Checked;
    }

    private void afterEstSent_CheckedChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void after_CheckedChanged(object sender, EventArgs e)
    {
      this.btnSearch.Enabled = this.advCond.Checked;
      if (!this.advCond.Checked)
        this.afterCondition.Text = "";
      this.setDirtyFlag(true);
    }

    private void AddFieldsToList(string fieldID, string fieldDesc, int numReferences)
    {
      this.fieldList.Add(new DDMDataPopTimingField(this.session.UserID, fieldID, fieldDesc, numReferences));
    }

    private void RefreshPage()
    {
      if (this.session.dataPopTiming == null)
      {
        this.dataPopTimingsBpmMgr = (DDMDataPopTimingBpmManager) this.session.BPM.GetBpmManager(BpmCategory.DDMDataPopTiming);
        this.session.dataPopTiming = this.dataPopTimingsBpmMgr.GetDDMDataPopulationTiming(true);
      }
      this.fieldChanges.Checked = this.session.dataPopTiming.FieldChanges;
      this.afterEstSent.Checked = this.session.dataPopTiming.AfterLoanInitEst;
      this.advCond.Checked = this.session.dataPopTiming.LoanCondMet;
      this.afterCondition.Text = this.session.dataPopTiming.LoanCondMetCond;
      this.advancedCodeXml = this.session.dataPopTiming.LoanCondMetCondXml;
      this.listViewOptions.Items.Clear();
      foreach (DDMDataPopTimingField field in this.session.dataPopTiming.FieldList)
        this.listViewOptions.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = field.FieldID
            },
            [1] = {
              Text = field.FieldDescription
            },
            [2] = {
              Text = field.NumReferenced.ToString()
            }
          }
        });
    }

    private void stdIconBtnRefresh_Click(object sender, EventArgs e)
    {
      this.RefreshReferenceCount();
    }

    private void DataPopulationTiming_Leave(object sender, EventArgs e)
    {
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Data Population Timing";
  }
}
