// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddMilestonesForLoanActionCompletion
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddMilestonesForLoanActionCompletion : Form
  {
    private LoanActionMilestonesListPanel milestoneListPanel;
    private MilestoneLoanActionPair[] milestonesForRule;
    private IContainer components;
    private Panel panelForListView;
    private Label label1;
    private ComboBox cboLoanAction;
    private DialogButtons dialogButtons1;

    public MilestoneLoanActionPair[] MilestonesForRule => this.milestonesForRule;

    public AddMilestonesForLoanActionCompletion(
      Sessions.Session session,
      string[] loanActionList,
      Dictionary<string, string> msList)
    {
      this.InitializeComponent();
      this.milestoneListPanel = new LoanActionMilestonesListPanel(msList);
      this.milestoneListPanel.Dock = DockStyle.Fill;
      this.panelForListView.Controls.Add((Control) this.milestoneListPanel);
      this.populateLoanActions(loanActionList);
    }

    private void populateLoanActions(string[] loanActionList)
    {
      this.cboLoanAction.Items.AddRange((object[]) loanActionList);
      this.cboLoanAction.SelectedIndex = 0;
    }

    public string SelectedLoanAction => this.cboLoanAction.Text;

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.milestoneListPanel.SelectedMilestoneCount == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You have to select Milestones first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.cboLoanAction.Text == string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select Loan Action first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.milestonesForRule = new MilestoneLoanActionPair[this.milestoneListPanel.SelectedMilestoneCount];
        string loanActionID = new TriggerActivationNameProvider().GetValue(this.cboLoanAction.Text).ToString();
        for (int index = 0; index < this.milestoneListPanel.SelectedMilestoneCount; ++index)
          this.milestonesForRule[index] = new MilestoneLoanActionPair(this.milestoneListPanel.SelectedMilestones[index], loanActionID);
        this.DialogResult = DialogResult.OK;
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
      this.panelForListView = new Panel();
      this.label1 = new Label();
      this.cboLoanAction = new ComboBox();
      this.dialogButtons1 = new DialogButtons();
      this.SuspendLayout();
      this.panelForListView.Dock = DockStyle.Top;
      this.panelForListView.Location = new Point(0, 0);
      this.panelForListView.Name = "panelForListView";
      this.panelForListView.Size = new Size(439, 411);
      this.panelForListView.TabIndex = 6;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 424);
      this.label1.Name = "label1";
      this.label1.Size = new Size(85, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "For Loan Action:";
      this.cboLoanAction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLoanAction.FormattingEnabled = true;
      this.cboLoanAction.Location = new Point(103, 421);
      this.cboLoanAction.Name = "cboLoanAction";
      this.cboLoanAction.Size = new Size(180, 21);
      this.cboLoanAction.TabIndex = 7;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 453);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(439, 44);
      this.dialogButtons1.TabIndex = 9;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(439, 497);
      this.Controls.Add((Control) this.dialogButtons1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboLoanAction);
      this.Controls.Add((Control) this.panelForListView);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddMilestonesForLoanActionCompletion);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Milestones";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
