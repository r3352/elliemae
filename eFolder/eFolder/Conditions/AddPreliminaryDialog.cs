// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AddPreliminaryDialog
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
  public class AddPreliminaryDialog : Form
  {
    private LoanDataMgr loanDataMgr;
    private ConditionLog[] condList;
    private bool isImportCondtions;
    private bool displayingImportCondition;
    private IContainer components;
    private RadioButton rdoFannieMae;
    private Button btnOK;
    private Button btnCancel;
    private RadioButton rdoSet;
    private RadioButton rdoNew;
    private RadioButton rdoRule;
    private RadioButton rdoFreddieMac;
    private RadioButton rdoEarlyCheck;
    private RadioButton rdoFHACatalyst;

    public bool IsImportConditions
    {
      get => this.isImportCondtions;
      set => this.isImportCondtions = value;
    }

    public AddPreliminaryDialog(LoanDataMgr loanDataMgr)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      if (Session.IsBrokerEdition())
      {
        this.rdoSet.Visible = false;
        this.rdoFannieMae.Top = this.rdoSet.Top;
        this.rdoRule.Visible = false;
      }
      else
      {
        if (new eFolderAccessRights(this.loanDataMgr).CanUsePreliminaryAutomatedCondition)
          return;
        this.rdoRule.Visible = false;
      }
    }

    public ConditionLog[] Conditions => this.condList;

    public bool ShowPreliminaryConditions => this.rdoNew.Checked;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.rdoNew.Checked)
        this.addCondition();
      else if (this.rdoSet.Checked)
        this.addConditionSet();
      else if (this.rdoFannieMae.Checked)
        this.addFannieMaeDu();
      else if (this.rdoEarlyCheck.Checked)
        this.addFannieMaeEarlyCheck();
      else if (this.rdoFreddieMac.Checked)
        this.addFreddieMacLp();
      else if (this.rdoRule.Checked)
      {
        this.addAutomatedCondition();
      }
      else
      {
        if (!this.rdoFHACatalyst.Checked)
          return;
        this.addFHACatalystCondition();
      }
    }

    private void addCondition()
    {
      LoanData loanData = this.loanDataMgr.LoanData;
      LogList logList = loanData.GetLogList();
      PreliminaryConditionLog rec = new PreliminaryConditionLog(Session.UserID, loanData.PairId);
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
      using (ConditionSetsDialog conditionSetsDialog = new ConditionSetsDialog(this.loanDataMgr, ConditionType.Preliminary))
      {
        DialogResult dialogResult = conditionSetsDialog.ShowDialog((IWin32Window) this);
        if (dialogResult == DialogResult.OK)
          this.condList = conditionSetsDialog.Conditions;
        this.DialogResult = dialogResult;
      }
    }

    private void addFannieMaeDu()
    {
      ConditionLog[] allConditions = this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.Preliminary);
      IEPass service = Session.Application.GetService<IEPass>();
      if (service == null)
        return;
      service.ProcessURL("_EPASS_SIGNATURE;GEN2DU;2;IMPORTFINDINGS", false);
      this.UpdatePreliminaryDialogResult(allConditions);
    }

    private void addFannieMaeEarlyCheck()
    {
      ConditionLog[] allConditions = this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.Preliminary);
      IEPass service = Session.Application.GetService<IEPass>();
      if (service == null)
        return;
      service.ProcessURL("_EPASS_SIGNATURE;EarlyCheck;2;IMPORTFINDINGS", false);
      this.UpdatePreliminaryDialogResult(allConditions);
    }

    private void addFreddieMacLp()
    {
      ConditionLog[] allConditions = this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.Preliminary);
      IEPass service = Session.Application.GetService<IEPass>();
      if (service == null)
        return;
      service.ProcessURL("_EPASS_SIGNATURE;LOANPROSPECTOR;2;IMPORTFINDINGS", true);
      this.UpdatePreliminaryDialogResult(allConditions);
    }

    private void addFHACatalystCondition()
    {
      this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.Preliminary);
      Session.Application.GetService<IEPass>()?.ProcessURL("_EPASS_SIGNATURE;FHACATALYSTIMPORT;", true);
    }

    private void UpdatePreliminaryDialogResult(ConditionLog[] beforeList)
    {
      this.condList = ((IEnumerable<ConditionLog>) this.loanDataMgr.LoanData.GetLogList().GetAllConditions(ConditionType.Preliminary)).Where<ConditionLog>((Func<ConditionLog, bool>) (cond => Array.IndexOf<ConditionLog>(beforeList, cond) < 0)).ToArray<ConditionLog>();
      this.DialogResult = this.condList.Length != 0 ? DialogResult.OK : DialogResult.Cancel;
    }

    private void addAutomatedCondition()
    {
      this.loanDataMgr.LoanData.GetLogList();
      using (AutomatedConditionDialog automatedConditionDialog = new AutomatedConditionDialog(ConditionType.Preliminary, this.loanDataMgr.LoanData))
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

    private void AddPreliminaryDialog_VisibleChanged(object sender, EventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || this.Visible || !this.ShowPreliminaryConditions || !(this.Conditions[0].ConditionType.ToString().ToLower() == "preliminary"))
        return;
      PreliminaryDetailsDialog.ShowInstance(this.loanDataMgr, (PreliminaryConditionLog) this.Conditions[0]);
    }

    private void importCondition()
    {
      if (this.displayingImportCondition)
        return;
      try
      {
        this.displayingImportCondition = true;
        this.isImportCondtions = true;
        ImportConditionFactory conditionFactory = new ImportConditionFactory(ConditionType.Preliminary, this.loanDataMgr.LoanData);
        using (Form importConditionForm = conditionFactory.GetImportConditionForm())
        {
          int num = (int) importConditionForm.ShowDialog((IWin32Window) this);
        }
        List<ConditionLog> result = conditionFactory.GetResult();
        if (result != null && result.Count > 0)
        {
          this.condList = result.ToArray();
          foreach (ConditionLog rec in result)
            this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
        }
        this.DialogResult = DialogResult.OK;
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
      this.rdoFannieMae = new RadioButton();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.rdoSet = new RadioButton();
      this.rdoNew = new RadioButton();
      this.rdoRule = new RadioButton();
      this.rdoFreddieMac = new RadioButton();
      this.rdoEarlyCheck = new RadioButton();
      this.rdoFHACatalyst = new RadioButton();
      this.SuspendLayout();
      this.rdoFannieMae.AutoSize = true;
      this.rdoFannieMae.Location = new Point(12, 66);
      this.rdoFannieMae.Name = "rdoFannieMae";
      this.rdoFannieMae.Size = new Size(180, 18);
      this.rdoFannieMae.TabIndex = 2;
      this.rdoFannieMae.Text = "Add conditions from DU findings";
      this.rdoFannieMae.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom;
      this.btnOK.Location = new Point(131, 219);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(212, 219);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.rdoSet.AutoSize = true;
      this.rdoSet.Location = new Point(12, 39);
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
      this.rdoRule.Location = new Point(12, 147);
      this.rdoRule.Name = "rdoRule";
      this.rdoRule.Size = new Size(152, 18);
      this.rdoRule.TabIndex = 5;
      this.rdoRule.Text = "Add Automated Conditions";
      this.rdoRule.UseVisualStyleBackColor = true;
      this.rdoFreddieMac.AutoSize = true;
      this.rdoFreddieMac.Location = new Point(12, 120);
      this.rdoFreddieMac.Name = "rdoFreddieMac";
      this.rdoFreddieMac.Size = new Size(184, 18);
      this.rdoFreddieMac.TabIndex = 6;
      this.rdoFreddieMac.Text = "Add conditions from LPA findings";
      this.rdoFreddieMac.UseVisualStyleBackColor = true;
      this.rdoEarlyCheck.AutoSize = true;
      this.rdoEarlyCheck.Location = new Point(12, 93);
      this.rdoEarlyCheck.Name = "rdoEarlyCheck";
      this.rdoEarlyCheck.Size = new Size(220, 18);
      this.rdoEarlyCheck.TabIndex = 7;
      this.rdoEarlyCheck.Text = "Add conditions from EarlyCheck findings";
      this.rdoEarlyCheck.UseVisualStyleBackColor = true;
      this.rdoFHACatalyst.AutoSize = true;
      this.rdoFHACatalyst.Location = new Point(12, 174);
      this.rdoFHACatalyst.Name = "rdoFHACatalyst";
      this.rdoFHACatalyst.Size = new Size(188, 18);
      this.rdoFHACatalyst.TabIndex = 8;
      this.rdoFHACatalyst.Text = "Add Conditions from FHA Catalyst";
      this.rdoFHACatalyst.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(309, 259);
      this.Controls.Add((Control) this.rdoFHACatalyst);
      this.Controls.Add((Control) this.rdoEarlyCheck);
      this.Controls.Add((Control) this.rdoFreddieMac);
      this.Controls.Add((Control) this.rdoRule);
      this.Controls.Add((Control) this.rdoFannieMae);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rdoSet);
      this.Controls.Add((Control) this.rdoNew);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddPreliminaryDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Condition";
      this.VisibleChanged += new EventHandler(this.AddPreliminaryDialog_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
