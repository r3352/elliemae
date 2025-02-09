// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AutomatedConditionDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class AutomatedConditionDialog : Form
  {
    private LoanData loan;
    private AutomatedConditionBpmManager automatedConditionManager;
    private ConditionType conditionType;
    private List<ConditionLog> newConditions;
    private IContainer components;
    private GridView gridViewRules;
    private Button btnOK;
    private Button btnCancel;
    private CheckBox chkAll;

    public AutomatedConditionDialog(ConditionType conditionType, LoanData loan)
    {
      this.conditionType = conditionType;
      this.loan = loan;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      this.automatedConditionManager = (AutomatedConditionBpmManager) Session.BPM.GetBpmManager(BpmCategory.AutomatedConditions);
      string[] conditions = this.automatedConditionManager.GetConditions(new LoanBusinessRuleInfo(this.loan).CurrentLoanForBusinessRule(), this.loan, this.conditionType);
      if (conditions == null || conditions.Length == 0)
        return;
      this.gridViewRules.BeginUpdate();
      this.gridViewRules.SubItemCheck -= new GVSubItemEventHandler(this.gridViewRules_SubItemCheck);
      for (int index = 0; index < conditions.Length; ++index)
        this.gridViewRules.Items.Add(new GVItem(conditions[index]));
      this.gridViewRules.SubItemCheck += new GVSubItemEventHandler(this.gridViewRules_SubItemCheck);
      this.chkAll.Checked = true;
      this.gridViewRules.EndUpdate();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      UnderwritingConditionTrackingSetup conditionTrackingSetup1 = (UnderwritingConditionTrackingSetup) Session.ConfigurationManager.GetConditionTrackingSetup(ConditionType.Underwriting);
      PostClosingConditionTrackingSetup conditionTrackingSetup2 = (PostClosingConditionTrackingSetup) Session.ConfigurationManager.GetConditionTrackingSetup(ConditionType.PostClosing);
      LogList logList = this.loan.GetLogList();
      ConditionLog[] allConditions = logList.GetAllConditions(this.conditionType);
      List<string> source = new List<string>();
      if (allConditions != null && allConditions.Length != 0)
      {
        for (int index = 0; index < allConditions.Length; ++index)
        {
          if (!source.Contains<string>(allConditions[index].Title + "|" + allConditions[index].Source, (IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase))
            source.Add(allConditions[index].Title + "|" + allConditions[index].Source);
        }
      }
      this.newConditions = new List<ConditionLog>();
      ConditionLog rec = (ConditionLog) null;
      for (int nItemIndex = 0; nItemIndex < this.gridViewRules.Items.Count; ++nItemIndex)
      {
        if (this.gridViewRules.Items[nItemIndex].Checked && !source.Contains<string>(this.gridViewRules.Items[nItemIndex].Text + "|Automated Conditions", (IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase))
        {
          switch (this.conditionType)
          {
            case ConditionType.Underwriting:
              UnderwritingConditionLog underwritingConditionLog = new UnderwritingConditionLog(Session.UserID, this.loan.PairId);
              UnderwritingConditionTemplate byName1 = conditionTrackingSetup1?.GetByName(this.gridViewRules.Items[nItemIndex].Text);
              if (byName1 != null)
              {
                underwritingConditionLog.Category = byName1.Category;
                underwritingConditionLog.PriorTo = byName1.PriorTo;
                underwritingConditionLog.Description = byName1.Description;
                underwritingConditionLog.DaysTillDue = byName1.DaysTillDue;
                underwritingConditionLog.IsExternal = byName1.IsExternal;
                underwritingConditionLog.IsInternal = byName1.IsInternal;
                underwritingConditionLog.AllowToClear = byName1.AllowToClear;
                underwritingConditionLog.ForRoleID = byName1.ForRoleID;
              }
              rec = (ConditionLog) underwritingConditionLog;
              break;
            case ConditionType.PostClosing:
              PostClosingConditionLog closingConditionLog = new PostClosingConditionLog(Session.UserID, this.loan.PairId);
              PostClosingConditionTemplate byName2 = conditionTrackingSetup2?.GetByName(this.gridViewRules.Items[nItemIndex].Text);
              if (byName2 != null)
              {
                closingConditionLog.Description = byName2.Description;
                closingConditionLog.DaysTillDue = byName2.DaysTillDue;
                closingConditionLog.Recipient = byName2.Recipient;
              }
              rec = (ConditionLog) closingConditionLog;
              break;
            case ConditionType.Preliminary:
              PreliminaryConditionLog preliminaryConditionLog = new PreliminaryConditionLog(Session.UserID, this.loan.PairId);
              UnderwritingConditionTemplate byName3 = conditionTrackingSetup1?.GetByName(this.gridViewRules.Items[nItemIndex].Text);
              if (byName3 != null)
              {
                preliminaryConditionLog.Category = byName3.Category;
                preliminaryConditionLog.PriorTo = byName3.PriorTo;
                preliminaryConditionLog.Description = byName3.Description;
                preliminaryConditionLog.DaysTillDue = byName3.DaysTillDue;
              }
              rec = (ConditionLog) preliminaryConditionLog;
              break;
          }
          rec.Title = this.gridViewRules.Items[nItemIndex].Text;
          rec.Source = "Automated Conditions";
          logList.AddRecord((LogRecordBase) rec);
          this.newConditions.Add(rec);
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private void gridViewRules_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      GVItem[] checkedItems = this.gridViewRules.GetCheckedItems(0);
      if (checkedItems != null && checkedItems.Length != 0)
        this.btnOK.Enabled = true;
      else
        this.btnOK.Enabled = false;
      this.chkAll.CheckedChanged -= new EventHandler(this.chkAll_CheckedChanged);
      this.gridViewRules.SubItemCheck -= new GVSubItemEventHandler(this.gridViewRules_SubItemCheck);
      this.chkAll.Checked = checkedItems.Length == this.gridViewRules.Items.Count;
      this.gridViewRules.SubItemCheck += new GVSubItemEventHandler(this.gridViewRules_SubItemCheck);
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
    }

    public ConditionLog[] NewConditions => this.newConditions.ToArray();

    public bool RuleMatched => this.gridViewRules.Items.Count > 0;

    private void chkAll_CheckedChanged(object sender, EventArgs e)
    {
      this.gridViewRules.SubItemCheck -= new GVSubItemEventHandler(this.gridViewRules_SubItemCheck);
      for (int nItemIndex = 0; nItemIndex < this.gridViewRules.Items.Count; ++nItemIndex)
        this.gridViewRules.Items[nItemIndex].Checked = this.chkAll.Checked;
      this.btnOK.Enabled = this.chkAll.Checked;
      this.gridViewRules.SubItemCheck += new GVSubItemEventHandler(this.gridViewRules_SubItemCheck);
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
      this.gridViewRules = new GridView();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.chkAll = new CheckBox();
      this.SuspendLayout();
      gvColumn.CheckBoxes = true;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "       Name";
      gvColumn.Width = 717;
      this.gridViewRules.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridViewRules.Location = new Point(12, 12);
      this.gridViewRules.Name = "gridViewRules";
      this.gridViewRules.Size = new Size(719, 321);
      this.gridViewRules.TabIndex = 0;
      this.gridViewRules.SubItemCheck += new GVSubItemEventHandler(this.gridViewRules_SubItemCheck);
      this.btnOK.Location = new Point(557, 348);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(91, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&Add Selected";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(654, 347);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.chkAll.AutoSize = true;
      this.chkAll.BackColor = Color.Transparent;
      this.chkAll.Location = new Point(18, 15);
      this.chkAll.Name = "chkAll";
      this.chkAll.Size = new Size(15, 14);
      this.chkAll.TabIndex = 3;
      this.chkAll.UseVisualStyleBackColor = false;
      this.chkAll.CheckedChanged += new EventHandler(this.chkAll_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(743, 383);
      this.Controls.Add((Control) this.chkAll);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.gridViewRules);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AutomatedConditionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Automated Conditions";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
