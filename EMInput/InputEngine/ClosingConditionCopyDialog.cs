// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ClosingConditionCopyDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ClosingConditionCopyDialog : Form
  {
    private Button cancelBtn;
    private Button okBtn;
    private RadioButton radioCopyTemp;
    private CheckBox checkBoxAppend;
    private RadioButton radioCopyCondition;
    private System.ComponentModel.Container components;
    private LoanData loan;
    private string condList = string.Empty;

    public ClosingConditionCopyDialog(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.radioCopyTemp.Checked = true;
      this.radioCopyCondition.Enabled = !this.loan.IsTemplate && this.loan != null;
      if (!Session.IsBrokerEdition())
        return;
      this.radioCopyTemp.Visible = false;
      this.radioCopyCondition.Checked = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public bool IsListAppended => this.checkBoxAppend.Checked;

    public string CondList => this.condList;

    private void InitializeComponent()
    {
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.radioCopyTemp = new RadioButton();
      this.checkBoxAppend = new CheckBox();
      this.radioCopyCondition = new RadioButton();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(229, 87);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(150, 87);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 9;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.radioCopyTemp.Checked = true;
      this.radioCopyTemp.Location = new Point(12, 12);
      this.radioCopyTemp.Name = "radioCopyTemp";
      this.radioCopyTemp.Size = new Size(232, 20);
      this.radioCopyTemp.TabIndex = 10;
      this.radioCopyTemp.TabStop = true;
      this.radioCopyTemp.Text = "Add conditions from Condition Sets";
      this.checkBoxAppend.Checked = true;
      this.checkBoxAppend.CheckState = CheckState.Checked;
      this.checkBoxAppend.Location = new Point(11, 92);
      this.checkBoxAppend.Name = "checkBoxAppend";
      this.checkBoxAppend.Size = new Size(128, 16);
      this.checkBoxAppend.TabIndex = 12;
      this.checkBoxAppend.Text = "Append to current list";
      this.radioCopyCondition.Location = new Point(12, 31);
      this.radioCopyCondition.Name = "radioCopyCondition";
      this.radioCopyCondition.Size = new Size(264, 20);
      this.radioCopyCondition.TabIndex = 15;
      this.radioCopyCondition.Text = "Add conditions from Underwriter Conditions";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(316, 123);
      this.Controls.Add((Control) this.radioCopyCondition);
      this.Controls.Add((Control) this.radioCopyTemp);
      this.Controls.Add((Control) this.checkBoxAppend);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ClosingConditionCopyDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Condition";
      this.KeyPress += new KeyPressEventHandler(this.ClosingConditionCopyDialog_KeyPress);
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.radioCopyTemp.Checked)
      {
        using (ClosingConditionAddDialog conditionAddDialog = new ClosingConditionAddDialog())
        {
          if (conditionAddDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.condList = conditionAddDialog.CondList;
        }
      }
      else
      {
        using (ClosingConditionUnderwriteAddDialog underwriteAddDialog = new ClosingConditionUnderwriteAddDialog(Session.LoanDataMgr))
        {
          if (underwriteAddDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          foreach (UnderwritingConditionLog condition in underwriteAddDialog.Conditions)
            this.condList = this.condList + condition.Title + ":" + condition.Description + "\r\n\r\n";
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private void ClosingConditionCopyDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }
  }
}
