// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.ImportUnderwritingDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class ImportUnderwritingDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvConditionsMgr;
    private List<ConditionLog> condList = new List<ConditionLog>();
    private IContainer components;
    private Button btnAdd;
    private Button btnCancel;
    private GridView gvConditions;
    private ToolTip tooltip;

    public ImportUnderwritingDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.initConditionList();
      this.loadConditionList();
    }

    public ConditionLog[] Conditions => this.condList.ToArray();

    private void initConditionList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[9]
      {
        GridViewDataManager.HasDocumentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.OwnerColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.PriorToColumn,
        GridViewDataManager.CondStatusColumn,
        GridViewDataManager.DateColumn
      });
      this.gvConditions.Sort(1, SortOrder.Ascending);
    }

    private void loadConditionList()
    {
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      ConditionLog[] allConditions = logList.GetAllConditions(ConditionType.Underwriting);
      DocumentLog[] allDocuments = logList.GetAllDocuments();
      foreach (ConditionLog cond in allConditions)
        this.gvConditionsMgr.AddItem(cond, allDocuments);
      this.gvConditions.ReSort();
    }

    private void gvConditions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.gvConditions.SelectedItems.Count > 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      PostClosingConditionTrackingSetup conditionTrackingSetup = this.loanDataMgr.SystemConfiguration.PostClosingConditionTrackingSetup;
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      foreach (GVItem selectedItem in this.gvConditions.SelectedItems)
      {
        UnderwritingConditionLog tag = (UnderwritingConditionLog) selectedItem.Tag;
        PostClosingConditionLog closingConditionLog = new PostClosingConditionLog(Session.UserID, tag.PairId);
        closingConditionLog.Title = tag.Title;
        closingConditionLog.Description = tag.Description;
        closingConditionLog.Details = tag.Details;
        PostClosingConditionTemplate byName = conditionTrackingSetup.GetByName(tag.Title);
        if (byName != null)
        {
          closingConditionLog.Source = byName.Source;
          closingConditionLog.Recipient = byName.Recipient;
          closingConditionLog.DaysTillDue = byName.DaysTillDue;
        }
        logList.AddRecord((LogRecordBase) closingConditionLog);
        foreach (DocumentLog allDocument in logList.GetAllDocuments())
        {
          if (allDocument.Conditions.Contains((ConditionLog) tag))
            allDocument.Conditions.Add((ConditionLog) closingConditionLog);
          else
            allDocument.Conditions.Remove((ConditionLog) closingConditionLog);
        }
        this.condList.Add((ConditionLog) closingConditionLog);
      }
      this.DialogResult = DialogResult.OK;
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
      this.btnAdd = new Button();
      this.btnCancel = new Button();
      this.gvConditions = new GridView();
      this.tooltip = new ToolTip(this.components);
      this.SuspendLayout();
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(516, 364);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(592, 364);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.gvConditions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvConditions.ClearSelectionsOnEmptyRowClick = false;
      this.gvConditions.HoverToolTip = this.tooltip;
      this.gvConditions.Location = new Point(8, 8);
      this.gvConditions.Name = "gvConditions";
      this.gvConditions.Size = new Size(658, 348);
      this.gvConditions.TabIndex = 0;
      this.gvConditions.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditions.SelectedIndexChanged += new EventHandler(this.gvConditions_SelectedIndexChanged);
      this.AcceptButton = (IButtonControl) this.btnAdd;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(675, 395);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvConditions);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportUnderwritingDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Import Underwriting Conditions";
      this.ResumeLayout(false);
    }
  }
}
