// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AddPostClosingDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class AddPostClosingDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private ConditionLog[] condList;
    private bool isImportCondtions;
    private bool displayingImportCondition;
    private IContainer components;
    private RadioButton rdoUnderwriting;
    private Button btnOK;
    private Button btnCancel;
    private RadioButton rdoSet;
    private RadioButton rdoNew;
    private RadioButton rdoRule;
    private RadioButton rdoFannieMaeUCD;
    private RadioButton rdoLCLA;

    public bool IsImportConditions
    {
      get => this.isImportCondtions;
      set => this.isImportCondtions = value;
    }

    public AddPostClosingDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      if (Session.IsBrokerEdition())
      {
        this.rdoRule.Visible = false;
      }
      else
      {
        if (new eFolderAccessRights(this.loanDataMgr).CanUsePostClosingAutomatedCondition)
          return;
        this.rdoRule.Visible = false;
      }
    }

    public ConditionLog[] Conditions => this.condList;

    public bool ShowPostClosingConditions => this.rdoNew.Checked;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoNew.Checked)
        this.addCondition();
      else if (this.rdoSet.Checked)
        this.addConditionSet();
      else if (this.rdoUnderwriting.Checked)
        this.addUnderwriting();
      else if (this.rdoRule.Checked)
        this.addAutomatedCondition();
      else if (this.rdoFannieMaeUCD.Checked)
      {
        this.ProcessURL("_EPASS_SIGNATURE;FNUCD;2;IMPORTFINDINGS", true);
      }
      else
      {
        if (!this.rdoLCLA.Checked)
          return;
        this.ProcessURL("_EPASS_SIGNATURE;LCLA;2;IMPORTFINDINGS", true);
      }
    }

    private void addCondition()
    {
      LoanData loanData = this.loanDataMgr.LoanData;
      LogList logList = loanData.GetLogList();
      PostClosingConditionLog rec = new PostClosingConditionLog(Session.UserID, loanData.PairId);
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
      using (ConditionSetsDialog conditionSetsDialog = new ConditionSetsDialog(this.loanDataMgr, ConditionType.PostClosing))
      {
        DialogResult dialogResult = conditionSetsDialog.ShowDialog((IWin32Window) this);
        if (dialogResult == DialogResult.OK)
          this.condList = conditionSetsDialog.Conditions;
        this.DialogResult = dialogResult;
      }
    }

    private void addUnderwriting()
    {
      using (ImportUnderwritingDialog underwritingDialog = new ImportUnderwritingDialog(this.loanDataMgr))
      {
        DialogResult dialogResult = underwritingDialog.ShowDialog((IWin32Window) this);
        if (dialogResult == DialogResult.OK)
          this.condList = underwritingDialog.Conditions;
        this.DialogResult = dialogResult;
      }
    }

    private void addAutomatedCondition()
    {
      this.loanDataMgr.LoanData.GetLogList();
      using (AutomatedConditionDialog automatedConditionDialog = new AutomatedConditionDialog(ConditionType.PostClosing, this.loanDataMgr.LoanData))
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

    private void ProcessURL(string url, bool checkAccess)
    {
      ConditionLog[] allConditions = this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.PostClosing);
      IEPass service = Session.Application.GetService<IEPass>();
      if (service == null)
        return;
      service.ProcessURL(url, checkAccess);
      this.UpdatePostClosingDialogResult(allConditions);
    }

    private void UpdatePostClosingDialogResult(ConditionLog[] beforeList)
    {
      this.condList = ((IEnumerable<ConditionLog>) this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.PostClosing)).Where<ConditionLog>((Func<ConditionLog, bool>) (cond => Array.IndexOf<ConditionLog>(beforeList, cond) < 0)).ToArray<ConditionLog>();
      this.DialogResult = this.condList.Length != 0 ? DialogResult.OK : DialogResult.Cancel;
    }

    private void AddPostClosingDialog_VisibleChanged(object sender, EventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || this.Visible || !this.ShowPostClosingConditions || !(this.Conditions[0].ConditionType.ToString().ToLower() == "postclosing"))
        return;
      PostClosingDetailsDialog.ShowInstance(this.loanDataMgr, (PostClosingConditionLog) this.Conditions[0]);
    }

    private void importCondition()
    {
      if (this.displayingImportCondition)
        return;
      try
      {
        this.displayingImportCondition = true;
        this.isImportCondtions = true;
        ImportConditionFactory conditionFactory = new ImportConditionFactory(ConditionType.PostClosing, this.loanDataMgr.LoanData);
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
      this.rdoUnderwriting = new RadioButton();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rdoSet = new RadioButton();
      this.rdoNew = new RadioButton();
      this.rdoRule = new RadioButton();
      this.rdoFannieMaeUCD = new RadioButton();
      this.rdoLCLA = new RadioButton();
      this.SuspendLayout();
      this.rdoUnderwriting.AutoSize = true;
      this.rdoUnderwriting.Location = new Point(12, 60);
      this.rdoUnderwriting.Name = "rdoUnderwriting";
      this.rdoUnderwriting.Size = new Size(240, 18);
      this.rdoUnderwriting.TabIndex = 2;
      this.rdoUnderwriting.Text = "Add conditions from Underwriting Conditions";
      this.rdoUnderwriting.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(201, 178);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(277, 178);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
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
      this.rdoRule.TabIndex = 7;
      this.rdoRule.Text = "Add Automated Conditions";
      this.rdoRule.UseVisualStyleBackColor = true;
      this.rdoFannieMaeUCD.AutoSize = true;
      this.rdoFannieMaeUCD.Location = new Point(12, 108);
      this.rdoFannieMaeUCD.Name = "rdoFannieMaeUCD";
      this.rdoFannieMaeUCD.Size = new Size(253, 18);
      this.rdoFannieMaeUCD.TabIndex = 8;
      this.rdoFannieMaeUCD.Text = "Add conditions from Fannie Mae UCD Collection";
      this.rdoFannieMaeUCD.UseVisualStyleBackColor = true;
      this.rdoLCLA.AutoSize = true;
      this.rdoLCLA.Location = new Point(12, 132);
      this.rdoLCLA.Name = "rdoLCLA";
      this.rdoLCLA.Size = new Size(194, 18);
      this.rdoLCLA.TabIndex = 8;
      this.rdoLCLA.Text = "Add conditions from LCLA Findings";
      this.rdoLCLA.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(370, 208);
      this.Controls.Add((Control) this.rdoFannieMaeUCD);
      this.Controls.Add((Control) this.rdoLCLA);
      this.Controls.Add((Control) this.rdoRule);
      this.Controls.Add((Control) this.rdoUnderwriting);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoSet);
      this.Controls.Add((Control) this.rdoNew);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddPostClosingDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Condition";
      this.VisibleChanged += new EventHandler(this.AddPostClosingDialog_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
