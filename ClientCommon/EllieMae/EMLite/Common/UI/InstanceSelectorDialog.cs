// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.InstanceSelectorDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class InstanceSelectorDialog : Form
  {
    private const string className = "InstanceSelectorDialog";
    private static readonly string sw = Tracing.SwCommon;
    private Button cancelBtn;
    private Button okBtn;
    private Label labelTitle;
    private GridView gvItems;
    private System.ComponentModel.Container components;
    private Panel pnlList;
    private Panel pnlIndex;
    private Label label2;
    private NumericUpDown udIndex;
    private Label label1;
    private TextBox txtSearch;
    private Button btnFind;
    private AdvanceSelect moreSelect;
    private string selectedInstance = "";
    private string description = "";
    private int comortgagorPair;

    public InstanceSelectorDialog(AdvanceSelect moreSelect, string selectedFieldID)
    {
      this.moreSelect = moreSelect;
      this.InitializeComponent();
      this.gvItems.Items.Clear();
      this.gvItems.BeginUpdate();
      switch (moreSelect)
      {
        case AdvanceSelect.IsDocument:
          this.Text = "Select Document";
          this.labelTitle.Text = "Pick a document title:";
          DocumentTrackingSetup documentTrackingSetup = Session.ConfigurationManager.GetDocumentTrackingSetup();
          if (documentTrackingSetup != null)
          {
            using (Dictionary<string, DocumentTemplate>.ValueCollection.Enumerator enumerator = documentTrackingSetup.dictDocTrackByName.Values.GetEnumerator())
            {
              while (enumerator.MoveNext())
                this.gvItems.Items.Add(enumerator.Current.Name);
              break;
            }
          }
          else
            break;
        case AdvanceSelect.IsMilestone:
          this.Text = "Select Milestone Name";
          this.labelTitle.Text = "Pick a milestone name:";
          using (List<EllieMae.EMLite.Workflow.Milestone>.Enumerator enumerator = Session.StartupInfo.Milestones.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              EllieMae.EMLite.Workflow.Milestone current = enumerator.Current;
              if (!current.Archived && (!(selectedFieldID.ToLower() == "log.ms.duration") || !(current.Name == "Completion")))
              {
                string empty = string.Empty;
                this.gvItems.Items.Add(new GVItem(current.Name)
                {
                  Tag = (object) current.Name
                });
              }
            }
            break;
          }
        case AdvanceSelect.IsCondition:
        case AdvanceSelect.IsPostCondition:
          ConditionTrackingSetup conditionTrackingSetup;
          if (moreSelect == AdvanceSelect.IsPostCondition)
          {
            this.Text = "Select Post-Closing Condition";
            this.labelTitle.Text = "Pick a post-closing condition title:";
            conditionTrackingSetup = Session.ConfigurationManager.GetConditionTrackingSetup(ConditionType.PostClosing);
          }
          else
          {
            this.Text = "Select Condition";
            this.labelTitle.Text = "Pick a condition title:";
            conditionTrackingSetup = Session.ConfigurationManager.GetConditionTrackingSetup(ConditionType.Underwriting);
          }
          if (conditionTrackingSetup != null)
          {
            for (int index = 0; index < conditionTrackingSetup.Count; ++index)
              this.gvItems.Items.Add(conditionTrackingSetup.GetByIndex(index).Name);
            break;
          }
          break;
        case AdvanceSelect.IsLoanTeamMember:
          this.Text = "Select Loan Team Member";
          this.labelTitle.Text = "Pick a role name:";
          RoleInfo[] allRoleFunctions = ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions();
          this.gvItems.Items.Add(new GVItem("File Starter")
          {
            Tag = (object) "File Starter"
          });
          foreach (RoleInfo roleInfo in allRoleFunctions)
            this.gvItems.Items.Add(new GVItem(roleInfo.RoleName)
            {
              Tag = (object) roleInfo.RoleName
            });
          break;
        case AdvanceSelect.IsAuditTrail:
          this.Text = "Select Audit Field";
          this.labelTitle.Text = "Pick an audit field:";
          foreach (LoanXDBField loanXdbField in Session.LoanManager.GetAuditTrailLoanXDBField())
            this.gvItems.Items.Add(new GVItem(loanXdbField.Description)
            {
              Tag = (object) loanXdbField
            });
          break;
        case AdvanceSelect.IsIndex:
          this.Text = "Select Instance Index";
          this.pnlList.Visible = false;
          this.pnlIndex.Visible = true;
          this.MaximizeBox = false;
          this.Height -= this.pnlList.Height - this.pnlIndex.Height;
          break;
        case AdvanceSelect.IsRateLock:
          this.Text = "Select Rate Lock Fields";
          this.labelTitle.Text = "Pick a rate lock field";
          IEnumerator enumerator1 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator1.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.MostRecent)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.IsPreviousRateLock:
          this.Text = "Select Rate Lock Fields";
          this.labelTitle.Text = "Pick a rate lock field";
          IEnumerator enumerator2 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator2.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.Previous)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.Is2ndPreviousRateLock:
          this.Text = "Select Rate Lock Fields";
          this.labelTitle.Text = "Pick a rate lock field";
          IEnumerator enumerator3 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator3.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator3.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.Previous2)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator3 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.IsMilestoneTask:
          this.Text = "Select Task";
          this.labelTitle.Text = "Pick a task:";
          IEnumerator enumerator4 = Session.ConfigurationManager.GetMilestoneTasks().Values.GetEnumerator();
          try
          {
            while (enumerator4.MoveNext())
              this.gvItems.Items.Add(((MilestoneTaskDefinition) enumerator4.Current).TaskName);
            break;
          }
          finally
          {
            if (enumerator4 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.IsRateRequest:
          this.Text = "Select Rate Request Fields";
          this.labelTitle.Text = "Pick a most recent rate request field";
          IEnumerator enumerator5 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator5.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator5.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.MostRecentRequest)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator5 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.IsCustomAlert:
          this.Text = "Select Alert";
          this.labelTitle.Text = "Pick a the custom alert for the selected field";
          foreach (AlertConfig alertConfig in Session.AlertManager.GetAlertConfigList())
          {
            if (alertConfig.Definition is CustomAlert)
              this.gvItems.Items.Add(new GVItem(alertConfig.Definition.Name)
              {
                Tag = (object) alertConfig
              });
          }
          break;
        case AdvanceSelect.IsRateLockRequest:
          this.Text = "Select Rate Lock Request Fields";
          this.labelTitle.Text = "Pick a rate lock request field";
          IEnumerator enumerator6 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator6.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator6.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.MostRecentLockRequest)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator6 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.IsPreviousRateLockRequest:
          this.Text = "Select Rate Lock Request Fields";
          this.labelTitle.Text = "Pick a rate lock request field";
          IEnumerator enumerator7 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator7.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator7.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.PreviousLockRequest)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator7 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.Is2ndPreviousRateLockRequest:
          this.Text = "Select Rate Lock Request Fields";
          this.labelTitle.Text = "Pick a rate lock request field";
          IEnumerator enumerator8 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator8.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator8.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.Previous2LockRequest)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator8 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.IsRateLockDeny:
          this.Text = "Select Rate Lock Deny Fields";
          this.labelTitle.Text = "Pick a rate lock deny field";
          IEnumerator enumerator9 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator9.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator9.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.MostRecentDenied)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator9 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.IsPreviousRateLockDeny:
          this.Text = "Select Rate Lock Deny Fields";
          this.labelTitle.Text = "Pick a rate lock deny field";
          IEnumerator enumerator10 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator10.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator10.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.PreviousDenied)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator10 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.Is2ndPreviousRateLockDeny:
          this.Text = "Select Rate Lock Deny Fields";
          this.labelTitle.Text = "Pick a rate lock deny field";
          IEnumerator enumerator11 = EncompassFields.GetAllLockRequestFields(Session.LoanManager.GetFieldSettings()).GetEnumerator();
          try
          {
            while (enumerator11.MoveNext())
            {
              FieldDefinition current = (FieldDefinition) enumerator11.Current;
              if (current is RateLockField && ((RateLockField) current).GetRateLockOrder == RateLockField.RateLockOrder.Previous2Denied)
                this.gvItems.Items.Add(new GVItem(current.Description)
                {
                  Tag = (object) current
                });
            }
            break;
          }
          finally
          {
            if (enumerator11 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.EnhancedConditionSingleAttributeField:
          this.Text = "Select Enhanced Condition";
          this.labelTitle.Text = "Pick a condition title:";
          DataTable table1 = Session.ConfigurationManager.GetEnhanceConditionTemplateNameAndType(new Guid()).Tables[2];
          this.gvItems.Columns.Add("Condition Type", 350);
          this.gvItems.Columns[0].Width = 350;
          this.gvItems.Columns[0].Text = "Condition Title";
          this.gvItems.Columns[1].Text = "Condition Type";
          this.gvItems.Sort(0, SortOrder.Ascending);
          this.gvItems.Columns[0].SortMethod = GVSortMethod.Text;
          this.gvItems.HeaderVisible = true;
          this.gvItems.SortOption = GVSortOption.Auto;
          this.gvItems.HeaderHeight = 20;
          IEnumerator enumerator12 = table1.Rows.GetEnumerator();
          try
          {
            while (enumerator12.MoveNext())
            {
              DataRow current = (DataRow) enumerator12.Current;
              this.gvItems.Items.Add(new GVItem()
              {
                SubItems = {
                  [0] = {
                    Text = current["title"].ToString()
                  },
                  [1] = {
                    Text = current["conditiontype"].ToString()
                  }
                },
                Tag = (object) (current["conditiontype"].ToString() + "~%cbiz%~" + current["title"].ToString())
              });
            }
            break;
          }
          finally
          {
            if (enumerator12 is IDisposable disposable)
              disposable.Dispose();
          }
        case AdvanceSelect.EnhancedConditionMultipleAttributeFields:
          this.Text = "Select Enhanced Condition";
          this.labelTitle.Text = "Pick a condition type:";
          DataTable table2 = Session.ConfigurationManager.GetEnhanceConditionTemplateNameAndType(new Guid()).Tables[2];
          HashSet<string> stringSet = new HashSet<string>();
          this.gvItems.Columns[0].Text = "Condition Type";
          this.gvItems.Sort(0, SortOrder.Ascending);
          this.gvItems.Columns[0].SortMethod = GVSortMethod.Text;
          this.gvItems.HeaderVisible = true;
          this.gvItems.SortOption = GVSortOption.Auto;
          this.gvItems.HeaderHeight = 20;
          IEnumerator enumerator13 = table2.Rows.GetEnumerator();
          try
          {
            while (enumerator13.MoveNext())
            {
              DataRow current = (DataRow) enumerator13.Current;
              if (!stringSet.Contains(current["conditiontype"].ToString()))
              {
                stringSet.Add(current["conditiontype"].ToString());
                this.gvItems.Items.Add(new GVItem()
                {
                  SubItems = {
                    [0] = {
                      Text = current["conditiontype"].ToString()
                    }
                  },
                  Tag = (object) current["conditiontype"].ToString()
                });
              }
            }
            break;
          }
          finally
          {
            if (enumerator13 is IDisposable disposable)
              disposable.Dispose();
          }
      }
      if (this.gvItems.Items.Count > 0)
        this.gvItems.Items[0].Selected = true;
      this.gvItems.EndUpdate();
    }

    public InstanceSelectorDialog(AdvanceSelect moreSelect)
      : this(moreSelect, "")
    {
    }

    public InstanceSelectorDialog(FieldInstanceSpecifierType specifierType)
      : this(specifierType, "")
    {
    }

    public InstanceSelectorDialog(FieldInstanceSpecifierType specifierType, string selectedFieldID)
      : this(InstanceSelectorDialog.GetSelectionTypeFromSpecifierType(specifierType), selectedFieldID)
    {
    }

    public InstanceSelectorDialog(FieldInstanceSpecifierType specifierType, bool insertBtn)
      : this(specifierType, insertBtn, "")
    {
    }

    public InstanceSelectorDialog(
      FieldInstanceSpecifierType specifierType,
      bool insertBtn,
      string selectedFieldID)
      : this(InstanceSelectorDialog.GetSelectionTypeFromSpecifierType(specifierType), selectedFieldID)
    {
      if (!insertBtn)
        return;
      this.okBtn.Text = "Insert";
    }

    public InstanceSelectorDialog(
      FieldInstanceSpecifierType specifierType,
      string rateLockName,
      bool insertBtn)
      : this(InstanceSelectorDialog.GetSelectionTypeFromSpecifierType(rateLockName))
    {
      this.gvItems.SortOption = GVSortOption.None;
      this.gvItems.Columns[0].Text = "Field";
      if (!insertBtn)
        return;
      this.okBtn.Text = "Insert";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string SelectedInstance => this.selectedInstance;

    public string Description => this.description;

    public int ComortgagorPair => this.comortgagorPair;

    private void InitializeComponent()
    {
      GVColumn gvColumn = new GVColumn();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.labelTitle = new Label();
      this.gvItems = new GridView();
      this.pnlList = new Panel();
      this.txtSearch = new TextBox();
      this.btnFind = new Button();
      this.pnlIndex = new Panel();
      this.label2 = new Label();
      this.udIndex = new NumericUpDown();
      this.label1 = new Label();
      this.pnlList.SuspendLayout();
      this.pnlIndex.SuspendLayout();
      this.udIndex.BeginInit();
      this.SuspendLayout();
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(499, 456);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 6;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(418, 456);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&Select";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.labelTitle.AutoSize = true;
      this.labelTitle.Location = new Point(12, 47);
      this.labelTitle.Name = "labelTitle";
      this.labelTitle.Size = new Size(108, 14);
      this.labelTitle.TabIndex = 9;
      this.labelTitle.Text = "Pick document name:";
      this.gvItems.AllowMultiselect = false;
      this.gvItems.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Category";
      gvColumn.Width = 561;
      this.gvItems.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gvItems.HeaderHeight = 0;
      this.gvItems.HeaderVisible = false;
      this.gvItems.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvItems.Location = new Point(11, 63);
      this.gvItems.Name = "gvItems";
      this.gvItems.Size = new Size(563, 372);
      this.gvItems.TabIndex = 10;
      this.gvItems.DoubleClick += new EventHandler(this.listBoxDocs_DoubleClick);
      this.pnlList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlList.Controls.Add((Control) this.txtSearch);
      this.pnlList.Controls.Add((Control) this.btnFind);
      this.pnlList.Controls.Add((Control) this.gvItems);
      this.pnlList.Controls.Add((Control) this.labelTitle);
      this.pnlList.Location = new Point(0, 0);
      this.pnlList.Name = "pnlList";
      this.pnlList.Size = new Size(591, 445);
      this.pnlList.TabIndex = 11;
      this.txtSearch.Location = new Point(96, 14);
      this.txtSearch.Name = "txtSearch";
      this.txtSearch.Size = new Size(300, 20);
      this.txtSearch.TabIndex = 15;
      this.btnFind.Location = new Point(15, 12);
      this.btnFind.Name = "btnFind";
      this.btnFind.Size = new Size(75, 23);
      this.btnFind.TabIndex = 14;
      this.btnFind.Text = "Find";
      this.btnFind.UseVisualStyleBackColor = true;
      this.btnFind.Click += new EventHandler(this.btnFind_Click);
      this.pnlIndex.Controls.Add((Control) this.label2);
      this.pnlIndex.Controls.Add((Control) this.udIndex);
      this.pnlIndex.Controls.Add((Control) this.label1);
      this.pnlIndex.Location = new Point(0, 3);
      this.pnlIndex.Name = "pnlIndex";
      this.pnlIndex.Size = new Size(583, 89);
      this.pnlIndex.TabIndex = 12;
      this.pnlIndex.Visible = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(180, 49);
      this.label2.Name = "label2";
      this.label2.Size = new Size(86, 14);
      this.label2.TabIndex = 12;
      this.label2.Text = "Instance Index =";
      this.udIndex.Location = new Point(270, 47);
      this.udIndex.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        0
      });
      this.udIndex.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.udIndex.Name = "udIndex";
      this.udIndex.Size = new Size(68, 20);
      this.udIndex.TabIndex = 1;
      this.udIndex.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(545, 32);
      this.label1.TabIndex = 10;
      this.label1.Text = "The selected field requires an index to specify the desired instance. The first instance has index 1, the second instance has index 2, etc.";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(586, 487);
      this.Controls.Add((Control) this.pnlIndex);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.pnlList);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (InstanceSelectorDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Document Tracking";
      this.pnlList.ResumeLayout(false);
      this.pnlList.PerformLayout();
      this.pnlIndex.ResumeLayout(false);
      this.pnlIndex.PerformLayout();
      this.udIndex.EndInit();
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.pnlIndex.Visible)
      {
        this.selectedInstance = string.Concat((object) this.udIndex.Value);
      }
      else
      {
        if (this.gvItems.SelectedItems.Count == 0)
        {
          if (this.labelTitle.Text == "Pick a milestone name:")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You have to select one milestone name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          if (this.moreSelect == AdvanceSelect.EnhancedConditionMultipleAttributeFields || this.moreSelect == AdvanceSelect.EnhancedConditionSingleAttributeField)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "You have to select one contition name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select one document name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        if (this.moreSelect == AdvanceSelect.IsLoanTeamMember || this.moreSelect == AdvanceSelect.IsMilestone || this.moreSelect == AdvanceSelect.IsRateLock || this.moreSelect == AdvanceSelect.IsPreviousRateLock || this.moreSelect == AdvanceSelect.Is2ndPreviousRateLock || this.moreSelect == AdvanceSelect.IsRateRequest || this.moreSelect == AdvanceSelect.IsRateLockRequest || this.moreSelect == AdvanceSelect.IsPreviousRateLockRequest || this.moreSelect == AdvanceSelect.Is2ndPreviousRateLockRequest || this.moreSelect == AdvanceSelect.IsRateLockDeny || this.moreSelect == AdvanceSelect.IsPreviousRateLockDeny || this.moreSelect == AdvanceSelect.Is2ndPreviousRateLockDeny)
          this.selectedInstance = this.gvItems.SelectedItems[0].Tag.ToString();
        else if (this.moreSelect == AdvanceSelect.IsAuditTrail)
        {
          LoanXDBField tag = (LoanXDBField) this.gvItems.SelectedItems[0].Tag;
          this.selectedInstance = tag.FieldIDWithCoMortgagor;
          this.comortgagorPair = tag.ComortgagorPair;
        }
        else
          this.selectedInstance = this.moreSelect != AdvanceSelect.EnhancedConditionSingleAttributeField ? this.gvItems.SelectedItems[0].Text.Trim() : this.gvItems.SelectedItems[0].Tag.ToString();
        if (this.selectedInstance.StartsWith("Select Next"))
          this.selectedInstance = "";
        this.description = this.gvItems.SelectedItems[0].Text.Trim();
      }
      this.DialogResult = DialogResult.OK;
    }

    private void listBoxDocs_DoubleClick(object sender, EventArgs e)
    {
      this.okBtn_Click((object) null, (EventArgs) null);
    }

    private EllieMae.EMLite.Workflow.Milestone getMilestoneInfo(string id)
    {
      return ((MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByID(id);
    }

    public static AdvanceSelect GetSelectionTypeFromSpecifierType(
      FieldInstanceSpecifierType specifierType)
    {
      switch (specifierType)
      {
        case FieldInstanceSpecifierType.Index:
          return AdvanceSelect.IsIndex;
        case FieldInstanceSpecifierType.Role:
          return AdvanceSelect.IsLoanTeamMember;
        case FieldInstanceSpecifierType.Milestone:
          return AdvanceSelect.IsMilestone;
        case FieldInstanceSpecifierType.Document:
          return AdvanceSelect.IsDocument;
        case FieldInstanceSpecifierType.UnderwritingCondition:
          return AdvanceSelect.IsCondition;
        case FieldInstanceSpecifierType.PostClosingCondition:
          return AdvanceSelect.IsPostCondition;
        case FieldInstanceSpecifierType.MilestoneTask:
          return AdvanceSelect.IsMilestoneTask;
        case FieldInstanceSpecifierType.CustomAlert:
          return AdvanceSelect.IsCustomAlert;
        case FieldInstanceSpecifierType.PreliminaryCondition:
          return AdvanceSelect.IsCondition;
        case FieldInstanceSpecifierType.EnhancedCondition:
          return AdvanceSelect.EnhancedConditionMultipleAttributeFields;
        case FieldInstanceSpecifierType.EnhancedConditionSingleAttribute:
          return AdvanceSelect.EnhancedConditionSingleAttributeField;
        default:
          return AdvanceSelect.None;
      }
    }

    public static AdvanceSelect GetSelectionTypeFromSpecifierType(string rateLockName)
    {
      switch (rateLockName)
      {
        case "2nd Previous Lock Deny Fields":
          return AdvanceSelect.Is2ndPreviousRateLockDeny;
        case "2nd Previous Lock Fields":
          return AdvanceSelect.Is2ndPreviousRateLock;
        case "2nd Previous Lock Request Fields":
          return AdvanceSelect.Is2ndPreviousRateLockRequest;
        case "Most Recent Lock Deny Fields":
          return AdvanceSelect.IsRateLockDeny;
        case "Most Recent Lock Fields":
          return AdvanceSelect.IsRateLock;
        case "Most Recent Lock Request Fields":
          return AdvanceSelect.IsRateLockRequest;
        case "Most Recent Request Fields":
          return AdvanceSelect.IsRateRequest;
        case "Previous Lock Deny Fields":
          return AdvanceSelect.IsPreviousRateLockDeny;
        case "Previous Lock Fields":
          return AdvanceSelect.IsPreviousRateLock;
        case "Previous Lock Request Fields":
          return AdvanceSelect.IsPreviousRateLockRequest;
        default:
          return AdvanceSelect.None;
      }
    }

    private void btnFind_Click(object sender, EventArgs e) => this.findField(this.gvItems);

    private bool findField(GridView list)
    {
      string lower = this.txtSearch.Text.Trim().ToLower();
      if (lower == "" || list.Items.Count == 0)
        return false;
      bool flag = true;
      int num = -1;
      if (list.SelectedItems.Count > 0)
        num = list.SelectedItems[0].Index;
      list.SelectedItems.Clear();
      bool field = false;
      string empty = string.Empty;
      for (int index = 0; index < list.Items.Count; ++index)
      {
        ++num;
        if (num > list.Items.Count - 1)
          num = 0;
        for (int nItemIndex = 0; nItemIndex < list.Items[num].SubItems.Count; ++nItemIndex)
        {
          if (flag)
          {
            if (list.Items[num].SubItems[nItemIndex].Text.ToLower().IndexOf(lower) > -1)
            {
              field = true;
              break;
            }
          }
          else if ((" " + list.Items[num].SubItems[nItemIndex].Text.ToLower() + " ").IndexOf(lower) > -1)
          {
            field = true;
            break;
          }
        }
        if (field)
        {
          list.Items[num].Selected = true;
          list.EnsureVisible(num);
          break;
        }
      }
      return field;
    }
  }
}
