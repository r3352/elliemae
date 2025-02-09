// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.DocumentSetsDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class DocumentSetsDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private FileSystemEntry fileEntry;
    private DocumentTrackingSetup docSetup;
    private List<DocumentLog> docList = new List<DocumentLog>();
    private IContainer components;
    private Label lblSet;
    private GridView gvAvailable;
    private GridView gvSelected;
    private ComboBox cboBorrower;
    private Label lblBorrower;
    private Button btnAdd;
    private Button btnRemove;
    private Button btnCancel;
    private Button btnOK;
    private ToolTip tooltip;
    private StandardIconButton btnTemplate;
    private BorderPanel pnlTemplate;
    private Label lblTemplate;
    private PictureBox pctTemplate;

    public DocumentSetsDialog(LoanDataMgr loanDataMgr, FileSystemEntry fileEntry)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.fileEntry = fileEntry;
      this.docSetup = loanDataMgr.SystemConfiguration.DocumentTrackingSetup;
      this.gvAvailable.Sort(0, SortOrder.Ascending);
      this.gvSelected.Sort(0, SortOrder.Ascending);
      this.loadBorrowerList();
      this.loadAvailableList();
    }

    public DocumentLog[] Documents => this.docList.ToArray();

    private void loadBorrowerList()
    {
      BorrowerPair[] borrowerPairs = this.loanDataMgr.LoanData.GetBorrowerPairs();
      this.cboBorrower.Items.AddRange((object[]) borrowerPairs);
      this.cboBorrower.Items.Add((object) BorrowerPair.All);
      this.cboBorrower.SelectedItem = (object) borrowerPairs[0];
    }

    private void loadAvailableList()
    {
      this.lblTemplate.Text = this.fileEntry.Name;
      this.gvAvailable.Items.Clear();
      DocumentSetTemplate templateSettings = (DocumentSetTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet, this.fileEntry);
      if (templateSettings == null)
        return;
      Hashtable docList = templateSettings.DocList;
      if (docList == null)
        return;
      MilestoneTemplatesBpmManager bpmManager = (MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones);
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      foreach (string key in (IEnumerable) docList.Keys)
      {
        if (!string.IsNullOrEmpty(key))
        {
          EllieMae.EMLite.Workflow.Milestone milestoneByName = bpmManager.GetMilestoneByName(key);
          if (milestoneByName != null && logList.GetMilestoneByID(milestoneByName.MilestoneID) != null)
          {
            MilestoneLabel milestoneLabel = new MilestoneLabel(milestoneByName);
            ArrayList arrayList = new ArrayList();
            foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelected.Items)
            {
              if (((MilestoneLabel) gvItem.SubItems[1].Value).MilestoneName == key)
                arrayList.Add(gvItem.Tag);
            }
            foreach (string name in templateSettings.GetDocumentsByMilestone(key))
            {
              DocumentTemplate byName = this.docSetup.GetByName(name);
              if (byName != null && !arrayList.Contains((object) byName))
              {
                GVItem gvItem = this.gvAvailable.Items.Add(byName.Name);
                gvItem.SubItems[1].Value = (object) milestoneLabel;
                gvItem.Tag = (object) byName;
              }
            }
          }
        }
      }
      this.gvAvailable.ReSort();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvAvailable.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gvAvailable.Items.Remove(gvItem);
        gvItem.Selected = false;
        this.gvSelected.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.gvSelected.ReSort();
      this.gvSelected.Focus();
      this.btnOK.Enabled = this.gvSelected.Items.Count > 0;
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvSelected.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gvSelected.Items.Remove(gvItem);
        gvItem.Selected = false;
        this.gvAvailable.Items.Add(gvItem);
        gvItem.Selected = true;
      }
      this.gvAvailable.ReSort();
      this.gvAvailable.Focus();
      this.btnOK.Enabled = this.gvSelected.Items.Count > 0;
    }

    private void gvAvailable_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.gvAvailable.SelectedItems.Count > 0;
    }

    private void gvSelected_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnRemove.Enabled = this.gvSelected.SelectedItems.Count > 0;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      MilestoneTemplatesBpmManager bpmManager = (MilestoneTemplatesBpmManager) Session.BPM.GetBpmManager(BpmCategory.Milestones);
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      BorrowerPair selectedItem = (BorrowerPair) this.cboBorrower.SelectedItem;
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
        this.loanDataMgr.SetEnhancedConditionTemplates();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvSelected.Items)
      {
        DocumentTemplate tag = (DocumentTemplate) gvItem.Tag;
        MilestoneLabel milestoneLabel = (MilestoneLabel) gvItem.SubItems[1].Value;
        EllieMae.EMLite.Workflow.Milestone milestoneByName = bpmManager.GetMilestoneByName(milestoneLabel.MilestoneName);
        MilestoneLog milestoneById = logList.GetMilestoneByID(milestoneByName.MilestoneID);
        DocumentLog logEntry = tag.CreateLogEntry(Session.UserID, selectedItem.Id);
        logEntry.Stage = milestoneById.Stage;
        logList.AddRecord((LogRecordBase) logEntry);
        this.docList.Add(logEntry);
      }
      this.DialogResult = DialogResult.OK;
    }

    private void btnTemplate_Click(object sender, EventArgs e)
    {
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(Session.DefaultInstance, EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet, (FileSystemEntry) null, false))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.fileEntry = templateSelectionDialog.SelectedItem;
        this.loadAvailableList();
      }
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.lblSet = new Label();
      this.gvAvailable = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.btnTemplate = new StandardIconButton();
      this.gvSelected = new GridView();
      this.cboBorrower = new ComboBox();
      this.lblBorrower = new Label();
      this.btnAdd = new Button();
      this.btnRemove = new Button();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.pnlTemplate = new BorderPanel();
      this.pctTemplate = new PictureBox();
      this.lblTemplate = new Label();
      ((ISupportInitialize) this.btnTemplate).BeginInit();
      this.pnlTemplate.SuspendLayout();
      ((ISupportInitialize) this.pctTemplate).BeginInit();
      this.SuspendLayout();
      this.lblSet.AutoSize = true;
      this.lblSet.Location = new Point(12, 12);
      this.lblSet.Name = "lblSet";
      this.lblSet.Size = new Size(45, 14);
      this.lblSet.TabIndex = 0;
      this.lblSet.Text = "Doc Set";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colName";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 165;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colMilestone";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "For Milestone";
      gvColumn2.Width = 121;
      this.gvAvailable.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvAvailable.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAvailable.Location = new Point(12, 36);
      this.gvAvailable.Name = "gvAvailable";
      this.gvAvailable.Size = new Size(288, 352);
      this.gvAvailable.TabIndex = 2;
      this.gvAvailable.SelectedIndexChanged += new EventHandler(this.gvAvailable_SelectedIndexChanged);
      this.btnTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTemplate.BackColor = Color.Transparent;
      this.btnTemplate.Location = new Point(284, 11);
      this.btnTemplate.Margin = new Padding(4, 3, 0, 3);
      this.btnTemplate.MouseDownImage = (Image) null;
      this.btnTemplate.Name = "btnTemplate";
      this.btnTemplate.Size = new Size(16, 16);
      this.btnTemplate.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnTemplate.TabIndex = 13;
      this.btnTemplate.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnTemplate, "Add Document");
      this.btnTemplate.Click += new EventHandler(this.btnTemplate_Click);
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "colName";
      gvColumn3.Text = "Name";
      gvColumn3.Width = 165;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "colMilestone";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "For Milestone";
      gvColumn4.Width = 121;
      this.gvSelected.Columns.AddRange(new GVColumn[2]
      {
        gvColumn3,
        gvColumn4
      });
      this.gvSelected.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSelected.Location = new Point(380, 36);
      this.gvSelected.Name = "gvSelected";
      this.gvSelected.Size = new Size(288, 352);
      this.gvSelected.TabIndex = 7;
      this.gvSelected.SelectedIndexChanged += new EventHandler(this.gvSelected_SelectedIndexChanged);
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(473, 8);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(196, 22);
      this.cboBorrower.TabIndex = 6;
      this.lblBorrower.AutoSize = true;
      this.lblBorrower.Location = new Point(377, 12);
      this.lblBorrower.Margin = new Padding(0);
      this.lblBorrower.Name = "lblBorrower";
      this.lblBorrower.Size = new Size(94, 14);
      this.lblBorrower.TabIndex = 5;
      this.lblBorrower.Text = "For Borrower Pair";
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(308, 188);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(64, 22);
      this.btnAdd.TabIndex = 3;
      this.btnAdd.Text = "Add >";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnRemove.Enabled = false;
      this.btnRemove.Location = new Point(308, 212);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(64, 22);
      this.btnRemove.TabIndex = 4;
      this.btnRemove.Text = "< Remove";
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(594, 396);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(518, 396);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 8;
      this.btnOK.Text = "Add";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.pnlTemplate.BackColor = SystemColors.Window;
      this.pnlTemplate.Controls.Add((Control) this.pctTemplate);
      this.pnlTemplate.Controls.Add((Control) this.lblTemplate);
      this.pnlTemplate.Location = new Point(60, 8);
      this.pnlTemplate.Name = "pnlTemplate";
      this.pnlTemplate.Size = new Size(220, 22);
      this.pnlTemplate.TabIndex = 15;
      this.pctTemplate.Image = (Image) Resources.tempate;
      this.pctTemplate.Location = new Point(3, 3);
      this.pctTemplate.Name = "pctTemplate";
      this.pctTemplate.Size = new Size(16, 16);
      this.pctTemplate.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pctTemplate.TabIndex = 16;
      this.pctTemplate.TabStop = false;
      this.lblTemplate.AutoEllipsis = true;
      this.lblTemplate.BackColor = Color.Transparent;
      this.lblTemplate.Dock = DockStyle.Right;
      this.lblTemplate.Location = new Point(21, 1);
      this.lblTemplate.Name = "lblTemplate";
      this.lblTemplate.Size = new Size(198, 20);
      this.lblTemplate.TabIndex = 4;
      this.lblTemplate.Text = "Drag a document and drop in a condition below";
      this.lblTemplate.TextAlign = ContentAlignment.MiddleLeft;
      this.lblTemplate.UseCompatibleTextRendering = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(679, 427);
      this.Controls.Add((Control) this.pnlTemplate);
      this.Controls.Add((Control) this.btnTemplate);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.gvSelected);
      this.Controls.Add((Control) this.cboBorrower);
      this.Controls.Add((Control) this.lblBorrower);
      this.Controls.Add((Control) this.gvAvailable);
      this.Controls.Add((Control) this.lblSet);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentSetsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add from Document Sets";
      ((ISupportInitialize) this.btnTemplate).EndInit();
      this.pnlTemplate.ResumeLayout(false);
      this.pnlTemplate.PerformLayout();
      ((ISupportInitialize) this.pctTemplate).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
