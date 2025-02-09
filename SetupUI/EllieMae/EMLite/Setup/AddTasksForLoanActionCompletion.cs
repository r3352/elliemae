// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddTasksForLoanActionCompletion
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddTasksForLoanActionCompletion : Form
  {
    private Sessions.Session session;
    private LoanActionTaskListPanel taskList;
    private TaskLoanActionPair[] tasksForRule;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private ComboBox cboLoanAction;
    private Label label1;
    private Panel panelForListView;

    public AddTasksForLoanActionCompletion(
      Sessions.Session session,
      string[] loanActionList,
      Hashtable existDocs)
    {
      this.InitializeComponent();
      this.session = session;
      this.cboLoanAction.Items.AddRange((object[]) loanActionList);
      this.cboLoanAction.SelectedIndex = 0;
      this.initForm();
    }

    private void initForm()
    {
      this.taskList = new LoanActionTaskListPanel(this.session, true);
      this.taskList.Dock = DockStyle.Fill;
      this.panelForListView.Controls.Add((Control) this.taskList);
    }

    public TaskLoanActionPair[] TasksForRule => this.tasksForRule;

    public string SelectedLoanAction => this.cboLoanAction.Text;

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.taskList.SelectedTasks.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select a task first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.tasksForRule = new TaskLoanActionPair[this.taskList.SelectedTasks.Length];
        TriggerActivationType triggerActivationType = (TriggerActivationType) new TriggerActivationNameProvider().GetValue(this.cboLoanAction.Text);
        for (int index = 0; index < this.taskList.SelectedTasks.Length; ++index)
        {
          if (this.cboLoanAction.Text != string.Empty)
            this.tasksForRule[index] = new TaskLoanActionPair(this.taskList.SelectedTasks[index].TaskGUID, triggerActivationType.ToString());
        }
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
      this.dialogButtons1 = new DialogButtons();
      this.cboLoanAction = new ComboBox();
      this.label1 = new Label();
      this.panelForListView = new Panel();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 455);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(562, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.cboLoanAction.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cboLoanAction.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboLoanAction.FormattingEnabled = true;
      this.cboLoanAction.Location = new Point(101, 429);
      this.cboLoanAction.Name = "cboLoanAction";
      this.cboLoanAction.Size = new Size(180, 21);
      this.cboLoanAction.TabIndex = 3;
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 433);
      this.label1.Name = "label1";
      this.label1.Size = new Size(85, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "For Loan Action:";
      this.panelForListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelForListView.Location = new Point(13, 12);
      this.panelForListView.Name = "panelForListView";
      this.panelForListView.Size = new Size(537, 411);
      this.panelForListView.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(562, 499);
      this.Controls.Add((Control) this.panelForListView);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboLoanAction);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (AddTasksForLoanActionCompletion);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Tasks";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
