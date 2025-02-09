// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LoanActionLogDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.ElliEnum;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LoanActionLogDialog : Form
  {
    private IContainer components;
    private Panel panelTop;
    private DialogButtons dialogButtons1;
    private ComboBox cboActionTypes;
    private Label lblActionType;

    public LoanActionLogDialog()
    {
      this.InitializeComponent();
      this.populateActionTypes();
    }

    private void populateActionTypes()
    {
      foreach (LoanActionType loanActionType in Enum.GetValues(typeof (LoanActionType)))
        this.cboActionTypes.Items.Add((object) loanActionType);
      this.cboActionTypes.SelectedIndex = 0;
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      LoanActionLog rec = new LoanActionLog((LoanActionType) this.cboActionTypes.SelectedItem, Session.UserID);
      Session.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
      Session.Application.GetService<ILoanEditor>().RefreshContents();
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
      this.panelTop = new Panel();
      this.cboActionTypes = new ComboBox();
      this.lblActionType = new Label();
      this.dialogButtons1 = new DialogButtons();
      this.panelTop.SuspendLayout();
      this.SuspendLayout();
      this.panelTop.Controls.Add((Control) this.cboActionTypes);
      this.panelTop.Controls.Add((Control) this.lblActionType);
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(484, 52);
      this.panelTop.TabIndex = 0;
      this.cboActionTypes.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboActionTypes.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboActionTypes.FormattingEnabled = true;
      this.cboActionTypes.Location = new Point(83, 9);
      this.cboActionTypes.Name = "cboActionTypes";
      this.cboActionTypes.Size = new Size(389, 21);
      this.cboActionTypes.TabIndex = 1;
      this.lblActionType.AutoSize = true;
      this.lblActionType.Location = new Point(12, 9);
      this.lblActionType.Name = "lblActionType";
      this.lblActionType.Size = new Size(64, 13);
      this.lblActionType.TabIndex = 0;
      this.lblActionType.Text = "Action Type";
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 52);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(484, 44);
      this.dialogButtons1.TabIndex = 1;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(484, 96);
      this.Controls.Add((Control) this.panelTop);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanActionLogDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Loan Action Log";
      this.panelTop.ResumeLayout(false);
      this.panelTop.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
