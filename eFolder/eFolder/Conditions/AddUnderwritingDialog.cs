// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AddUnderwritingDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class AddUnderwritingDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private ConditionLog[] condList;
    private bool isImportCondtions;
    private bool displayingImportCondition;
    private IContainer components;
    private RadioButton rdoPreliminary;
    private Button btnOK;
    private Button btnCancel;
    private RadioButton rdoSet;
    private RadioButton rdoNew;
    private RadioButton rdoRule;

    public bool IsImportConditions
    {
      get => this.isImportCondtions;
      set => this.isImportCondtions = value;
    }

    public AddUnderwritingDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      if (Session.IsBrokerEdition())
      {
        this.rdoSet.Visible = false;
        this.rdoPreliminary.Top = this.rdoSet.Top;
        this.rdoRule.Visible = false;
      }
      else
      {
        if (new eFolderAccessRights(this.loanDataMgr).CanUseUnderwritingAutomatedCondition)
          return;
        this.rdoRule.Visible = false;
      }
    }

    public ConditionLog[] Conditions => this.condList;

    public bool ShowUnderwritingConditions => this.rdoNew.Checked;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoNew.Checked)
        this.addCondition();
      else if (this.rdoSet.Checked)
        this.addConditionSet();
      else if (this.rdoPreliminary.Checked)
      {
        this.addPreliminary();
      }
      else
      {
        if (!this.rdoRule.Checked)
          return;
        this.addAutomatedCondition();
      }
    }

    private void addCondition()
    {
      LoanData loanData = this.loanDataMgr.LoanData;
      LogList logList = loanData.GetLogList();
      UnderwritingConditionLog rec = new UnderwritingConditionLog(Session.UserID, loanData.PairId);
      rec.Title = "Untitled";
      rec.Source = "Manual";
      logList.AddRecord((LogRecordBase) rec);
      this.condList = new ConditionLog[1]
      {
        (ConditionLog) rec
      };
      this.DialogResult = DialogResult.OK;
    }

    private void addConditionSet()
    {
      using (ConditionSetsDialog conditionSetsDialog = new ConditionSetsDialog(this.loanDataMgr, ConditionType.Underwriting))
      {
        DialogResult dialogResult = conditionSetsDialog.ShowDialog((IWin32Window) this);
        if (dialogResult == DialogResult.OK)
          this.condList = conditionSetsDialog.Conditions;
        this.DialogResult = dialogResult;
      }
    }

    private void addPreliminary()
    {
      using (ImportPreliminaryDialog preliminaryDialog = new ImportPreliminaryDialog(this.loanDataMgr))
      {
        DialogResult dialogResult = preliminaryDialog.ShowDialog((IWin32Window) this);
        if (dialogResult == DialogResult.OK)
          this.condList = preliminaryDialog.Conditions;
        this.DialogResult = dialogResult;
      }
    }

    private void addAutomatedCondition()
    {
      this.loanDataMgr.LoanData.GetLogList();
      using (AutomatedConditionDialog automatedConditionDialog = new AutomatedConditionDialog(ConditionType.Underwriting, this.loanDataMgr.LoanData))
      {
        if (!automatedConditionDialog.RuleMatched)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "There is no Automated Conditions Rule matched.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        if (automatedConditionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.condList = automatedConditionDialog.NewConditions;
      }
      this.DialogResult = DialogResult.OK;
    }

    private void AddUnderwritingDialog_VisibleChanged(object sender, EventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || this.Visible || !this.ShowUnderwritingConditions || !(this.Conditions[0].ConditionType.ToString().ToLower() == "underwriting"))
        return;
      UnderwritingDetailsDialog.ShowInstance(this.loanDataMgr, (UnderwritingConditionLog) this.Conditions[0]);
    }

    private void importCondition()
    {
      if (this.displayingImportCondition)
        return;
      try
      {
        this.displayingImportCondition = true;
        this.isImportCondtions = true;
        ImportConditionFactory conditionFactory = new ImportConditionFactory(ConditionType.Underwriting, this.loanDataMgr.LoanData);
        using (Form importConditionForm = conditionFactory.GetImportConditionForm())
        {
          int num = (int) importConditionForm.ShowDialog((IWin32Window) this);
        }
        List<ConditionLog> result = conditionFactory.GetResult();
        if (result == null || result.Count <= 0)
          return;
        this.condList = result.ToArray();
        foreach (ConditionLog rec in result)
          this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
      }
      catch
      {
        throw;
      }
      finally
      {
        this.displayingImportCondition = false;
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
      this.rdoPreliminary = new RadioButton();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rdoSet = new RadioButton();
      this.rdoNew = new RadioButton();
      this.rdoRule = new RadioButton();
      this.SuspendLayout();
      this.rdoPreliminary.AutoSize = true;
      this.rdoPreliminary.Location = new Point(12, 60);
      this.rdoPreliminary.Name = "rdoPreliminary";
      this.rdoPreliminary.Size = new Size(230, 18);
      this.rdoPreliminary.TabIndex = 2;
      this.rdoPreliminary.Text = "Add conditions from Preliminary Conditions";
      this.rdoPreliminary.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(129, 125);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(205, 125);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.rdoSet.AutoSize = true;
      this.rdoSet.Location = new Point(12, 36);
      this.rdoSet.Name = "rdoSet";
      this.rdoSet.Size = new Size(194, 18);
      this.rdoSet.TabIndex = 1;
      this.rdoSet.Text = "Add conditions from Condition Sets";
      this.rdoSet.UseVisualStyleBackColor = true;
      this.rdoNew.AutoSize = true;
      this.rdoNew.Checked = true;
      this.rdoNew.Location = new Point(12, 12);
      this.rdoNew.Name = "rdoNew";
      this.rdoNew.Size = new Size(125, 18);
      this.rdoNew.TabIndex = 0;
      this.rdoNew.TabStop = true;
      this.rdoNew.Text = "Add a new condition";
      this.rdoNew.UseVisualStyleBackColor = true;
      this.rdoRule.AutoSize = true;
      this.rdoRule.Location = new Point(12, 84);
      this.rdoRule.Name = "rdoRule";
      this.rdoRule.Size = new Size(152, 18);
      this.rdoRule.TabIndex = 6;
      this.rdoRule.Text = "Add Automated Conditions";
      this.rdoRule.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(289, 159);
      this.Controls.Add((Control) this.rdoRule);
      this.Controls.Add((Control) this.rdoPreliminary);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoSet);
      this.Controls.Add((Control) this.rdoNew);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddUnderwritingDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Condition";
      this.VisibleChanged += new EventHandler(this.AddUnderwritingDialog_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
