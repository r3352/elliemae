// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ClosingConditionUnderwriteAddDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ClosingConditionUnderwriteAddDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private GridViewDataManager gvConditionsMgr;
    private List<UnderwritingConditionLog> condList = new List<UnderwritingConditionLog>();
    private IContainer components;
    private Button btnAdd;
    private Button btnCancel;
    private GridView gvConditions;

    public ClosingConditionUnderwriteAddDialog(LoanDataMgr loanDataMgr)
    {
      this.loanDataMgr = loanDataMgr;
      this.InitializeComponent();
      this.initConditionList();
      this.loadConditionList();
    }

    public UnderwritingConditionLog[] Conditions => this.condList.ToArray();

    private void initConditionList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[8]
      {
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
      logList.GetAllDocuments();
      foreach (ConditionLog cond in allConditions)
      {
        UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) cond;
        if (!underwritingConditionLog.Waived && !underwritingConditionLog.Cleared)
          this.gvConditionsMgr.AddItem(cond, (DocumentLog[]) null);
      }
      this.gvConditions.ReSort();
    }

    private void gvConditions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnAdd.Enabled = this.gvConditions.SelectedItems.Count > 0;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvConditions.SelectedItems)
        this.condList.Add((UnderwritingConditionLog) selectedItem.Tag);
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
      this.btnAdd = new Button();
      this.btnCancel = new Button();
      this.gvConditions = new GridView();
      this.SuspendLayout();
      this.btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(515, 363);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 4;
      this.btnAdd.Text = "&Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(591, 363);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.gvConditions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gvConditions.ClearSelectionsOnEmptyRowClick = false;
      this.gvConditions.Location = new Point(7, 7);
      this.gvConditions.Name = "gvConditions";
      this.gvConditions.Size = new Size(658, 348);
      this.gvConditions.TabIndex = 3;
      this.gvConditions.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditions.SelectedIndexChanged += new EventHandler(this.gvConditions_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(673, 393);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.gvConditions);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ClosingConditionUnderwriteAddDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Import Underwriting Conditions";
      this.ResumeLayout(false);
    }
  }
}
