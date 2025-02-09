// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AssignConditionsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.EnhancedConditions;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class AssignConditionsDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private DocumentLog doc;
    private GridViewDataManager gvConditionsMgr;
    private EnhancedConditionType[] enhancedConditionTypesForAdd;
    private IContainer components;
    private Button btnClose;
    private GridView gvConditions;
    private GroupContainer gcConditions;
    private StandardIconButton btnAddCondition;
    private ToolTip tooltip;
    private ComboBox cboCondType;

    public AssignConditionsDialog(LoanDataMgr loanDataMgr, DocumentLog doc)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.doc = doc;
      this.initConditionTypeList();
      this.initConditionList();
      this.loadConditionList();
    }

    private void initConditionTypeList()
    {
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
      {
        this.enhancedConditionTypesForAdd = this.getConditionTypesForAdd();
        this.cboCondType.Visible = false;
      }
      else
      {
        eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
        if (folderAccessRights.CanAccessPreliminaryTab)
          this.cboCondType.Items.Add((object) "Preliminary");
        if (folderAccessRights.CanAccessUnderwritingTab)
          this.cboCondType.Items.Add((object) "Underwriting");
        if (folderAccessRights.CanAccessPostClosingTab)
          this.cboCondType.Items.Add((object) "Post-Closing");
        if (folderAccessRights.CanAccessUnderwritingTab)
          this.cboCondType.SelectedItem = (object) "Underwriting";
        else if (folderAccessRights.CanAccessPostClosingTab)
        {
          this.cboCondType.SelectedItem = (object) "Post-Closing";
        }
        else
        {
          if (!folderAccessRights.CanAccessPreliminaryTab)
            return;
          this.cboCondType.SelectedItem = (object) "Preliminary";
        }
      }
    }

    private void initConditionList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[6]
      {
        GridViewDataManager.CheckBoxColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.CondStatusColumn,
        GridViewDataManager.DateColumn
      });
      this.gvConditions.Sort(1, SortOrder.Ascending);
    }

    private void loadConditionList()
    {
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      ConditionType selectedConditionType = this.getSelectedConditionType();
      ConditionLog[] allConditions = logList.GetAllConditions(selectedConditionType);
      DocumentLog[] allDocuments = logList.GetAllDocuments();
      this.gvConditions.BeginUpdate();
      this.gvConditionsMgr.ClearItems();
      foreach (ConditionLog conditionLog in allConditions)
      {
        if (conditionLog.ConditionType == ConditionType.Enhanced)
        {
          EnhancedConditionLog enhancedConditionLog = (EnhancedConditionLog) conditionLog;
          eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
          if (folderAccessRights.CanAccessEnhancedCondition(enhancedConditionLog.EnhancedConditionType))
          {
            GVItem gvItem = this.gvConditionsMgr.AddItem(conditionLog, allDocuments);
            gvItem.Checked = true;
            gvItem.Checked = this.doc.Conditions.Contains(conditionLog);
            gvItem.SubItems[0].CheckBoxEnabled = !gvItem.Checked ? folderAccessRights.CanAssignEnhancedConditionDocuments(enhancedConditionLog.EnhancedConditionType) : folderAccessRights.CanUnassignEnhancedConditionDocuments(enhancedConditionLog.EnhancedConditionType);
          }
        }
        else
        {
          GVItem gvItem = this.gvConditionsMgr.AddItem(conditionLog, allDocuments);
          gvItem.Checked = true;
          gvItem.Checked = this.doc.Conditions.Contains(conditionLog);
          eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) conditionLog);
          switch (conditionLog.ConditionType)
          {
            case ConditionType.Underwriting:
              gvItem.SubItems[0].CheckBoxEnabled = !gvItem.Checked ? folderAccessRights.CanAddUnderwritingConditionDocuments : folderAccessRights.CanRemoveUnderwritingConditionDocuments;
              continue;
            case ConditionType.PostClosing:
              gvItem.SubItems[0].CheckBoxEnabled = !gvItem.Checked ? folderAccessRights.CanAddPostClosingConditionDocuments : folderAccessRights.CanRemovePostClosingConditionDocuments;
              continue;
            case ConditionType.Preliminary:
              gvItem.SubItems[0].CheckBoxEnabled = !gvItem.Checked ? folderAccessRights.CanAddPreliminaryConditionDocuments : folderAccessRights.CanRemovePreliminaryConditionDocuments;
              continue;
            default:
              continue;
          }
        }
      }
      this.gvConditions.ReSort();
      this.gvConditions.EndUpdate();
    }

    private ConditionType getSelectedConditionType()
    {
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
        return ConditionType.Enhanced;
      string selectedItem = (string) this.cboCondType.SelectedItem;
      switch (this.cboCondType.Text)
      {
        case "Preliminary":
          return ConditionType.Preliminary;
        case "Underwriting":
          return ConditionType.Underwriting;
        case "Post-Closing":
          return ConditionType.PostClosing;
        default:
          return ConditionType.Unknown;
      }
    }

    private void cboCondType_SelectedIndexChanged(object sender, EventArgs e)
    {
      ConditionType selectedConditionType = this.getSelectedConditionType();
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      switch (selectedConditionType)
      {
        case ConditionType.Underwriting:
          this.btnAddCondition.Visible = folderAccessRights.CanAddUnderwritingConditions;
          break;
        case ConditionType.PostClosing:
          this.btnAddCondition.Visible = folderAccessRights.CanAddPostClosingConditions;
          break;
        case ConditionType.Preliminary:
          this.btnAddCondition.Visible = folderAccessRights.CanAddPreliminaryConditions;
          break;
      }
    }

    private void cboCondType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.loadConditionList();
    }

    private EnhancedConditionType[] getConditionTypesForAdd()
    {
      List<EnhancedConditionType> enhancedConditionTypeList = new List<EnhancedConditionType>();
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      foreach (EnhancedConditionType enhancedConditionType in new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTypes(true, false))
      {
        if (folderAccessRights.CanAddEnhancedCondition(enhancedConditionType.title))
          enhancedConditionTypeList.Add(enhancedConditionType);
      }
      this.btnAddCondition.Visible = enhancedConditionTypeList.Count > 0;
      return enhancedConditionTypeList.ToArray();
    }

    private void btnAddCondition_Click(object sender, EventArgs e)
    {
      switch (this.getSelectedConditionType())
      {
        case ConditionType.Underwriting:
          this.addUnderwritingCondition();
          break;
        case ConditionType.PostClosing:
          this.addPostClosingCondition();
          break;
        case ConditionType.Preliminary:
          this.addPreliminaryCondition();
          break;
        case ConditionType.Enhanced:
          this.addEnhancedCondition();
          break;
      }
    }

    private void addPreliminaryCondition()
    {
      using (AddPreliminaryDialog preliminaryDialog = new AddPreliminaryDialog(this.loanDataMgr))
      {
        if (preliminaryDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (preliminaryDialog.Conditions != null && preliminaryDialog.Conditions.Length != 0)
        {
          foreach (ConditionLog condition in preliminaryDialog.Conditions)
          {
            if (new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) condition).CanAddPreliminaryConditionDocuments)
              this.doc.Conditions.Add(condition);
          }
        }
        this.loadConditionList();
      }
    }

    private void addUnderwritingCondition()
    {
      using (AddUnderwritingDialog underwritingDialog = new AddUnderwritingDialog(this.loanDataMgr))
      {
        if (underwritingDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (underwritingDialog.Conditions != null && underwritingDialog.Conditions.Length != 0)
        {
          foreach (ConditionLog condition in underwritingDialog.Conditions)
          {
            if (new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) condition).CanAddUnderwritingConditionDocuments)
              this.doc.Conditions.Add(condition);
          }
        }
        this.loadConditionList();
      }
    }

    private void addPostClosingCondition()
    {
      using (AddPostClosingDialog postClosingDialog = new AddPostClosingDialog(this.loanDataMgr))
      {
        if (postClosingDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (postClosingDialog.Conditions != null && postClosingDialog.Conditions.Length != 0)
        {
          foreach (ConditionLog condition in postClosingDialog.Conditions)
          {
            if (new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) condition).CanAddPostClosingConditionDocuments)
              this.doc.Conditions.Add(condition);
          }
        }
        this.loadConditionList();
      }
    }

    private void addEnhancedCondition()
    {
      EnhancedConditionTemplate[] conditionTemplates = new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTemplates(true);
      if (conditionTemplates == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "No Condition Templates found for Add.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        using (AddEnhancedConditionsDialog conditionsDialog = new AddEnhancedConditionsDialog(this.loanDataMgr, conditionTemplates, this.enhancedConditionTypesForAdd))
        {
          if (conditionsDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
            return;
          if (conditionsDialog.Conditions != null && conditionsDialog.Conditions.Length != 0)
          {
            foreach (EnhancedConditionLog condition in conditionsDialog.Conditions)
            {
              if (new eFolderAccessRights(this.loanDataMgr).CanAssignEnhancedConditionDocuments(condition.EnhancedConditionType))
                this.doc.Conditions.Add((ConditionLog) condition);
            }
          }
          this.loadConditionList();
        }
      }
    }

    private void gvConditions_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      ConditionLog tag = (ConditionLog) e.SubItem.Item.Tag;
      if (e.SubItem.Checked)
        this.doc.Conditions.Add(tag);
      else
        this.doc.Conditions.Remove(tag);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.btnClose = new Button();
      this.gvConditions = new GridView();
      this.gcConditions = new GroupContainer();
      this.cboCondType = new ComboBox();
      this.btnAddCondition = new StandardIconButton();
      this.tooltip = new ToolTip(this.components);
      this.gcConditions.SuspendLayout();
      ((ISupportInitialize) this.btnAddCondition).BeginInit();
      this.SuspendLayout();
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(682, 392);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.gvConditions.AllowColumnReorder = true;
      this.gvConditions.BorderStyle = BorderStyle.None;
      this.gvConditions.Dock = DockStyle.Fill;
      this.gvConditions.Location = new Point(1, 26);
      this.gvConditions.Name = "gvConditions";
      this.gvConditions.Size = new Size(742, 345);
      this.gvConditions.TabIndex = 2;
      this.gvConditions.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditions.SubItemCheck += new GVSubItemEventHandler(this.gvConditions_SubItemCheck);
      this.gcConditions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcConditions.Controls.Add((Control) this.cboCondType);
      this.gcConditions.Controls.Add((Control) this.btnAddCondition);
      this.gcConditions.Controls.Add((Control) this.gvConditions);
      this.gcConditions.HeaderForeColor = SystemColors.ControlText;
      this.gcConditions.Location = new Point(12, 12);
      this.gcConditions.Name = "gcConditions";
      this.gcConditions.Size = new Size(744, 372);
      this.gcConditions.TabIndex = 0;
      this.gcConditions.Text = "Conditions";
      this.cboCondType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCondType.FormattingEnabled = true;
      this.cboCondType.Location = new Point(80, 2);
      this.cboCondType.Name = "cboCondType";
      this.cboCondType.Size = new Size(116, 22);
      this.cboCondType.TabIndex = 1;
      this.cboCondType.TabStop = false;
      this.cboCondType.SelectionChangeCommitted += new EventHandler(this.cboCondType_SelectionChangeCommitted);
      this.cboCondType.SelectedIndexChanged += new EventHandler(this.cboCondType_SelectedIndexChanged);
      this.btnAddCondition.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddCondition.BackColor = Color.Transparent;
      this.btnAddCondition.Location = new Point(724, 5);
      this.btnAddCondition.Margin = new Padding(4, 3, 0, 3);
      this.btnAddCondition.MouseDownImage = (Image) null;
      this.btnAddCondition.Name = "btnAddCondition";
      this.btnAddCondition.Size = new Size(16, 16);
      this.btnAddCondition.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddCondition.TabIndex = 2;
      this.btnAddCondition.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddCondition, "Add Condition");
      this.btnAddCondition.Click += new EventHandler(this.btnAddCondition_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(768, 423);
      this.Controls.Add((Control) this.gcConditions);
      this.Controls.Add((Control) this.btnClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AssignConditionsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Assign to Conditions";
      this.gcConditions.ResumeLayout(false);
      ((ISupportInitialize) this.btnAddCondition).EndInit();
      this.ResumeLayout(false);
    }
  }
}
