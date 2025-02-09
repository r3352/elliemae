// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MilestoneTaskDetailSetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MilestoneTaskDetailSetupForm : Form
  {
    private Sessions.Session session;
    private MilestoneTaskDefinition task;
    private IContainer components;
    private Button btnCancel;
    private TextBox txtName;
    private Label label1;
    private TextBox txtDescription;
    private Label label2;
    private Button btnOK;
    private Label label3;
    private TextBox txtDays;
    private Label label4;
    private ComboBox cboPriority;

    public MilestoneTaskDetailSetupForm(Sessions.Session session, MilestoneTaskDefinition task)
    {
      this.session = session;
      this.task = task;
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtDays, TextBoxContentRule.NonNegativeInteger, "##0");
      this.cboPriority.SelectedIndex = 1;
      if (this.task == null)
        return;
      this.txtName.Text = task.TaskName;
      this.txtDescription.Text = task.TaskDescription;
      this.txtDays.Text = task.DaysToComplete > 0 ? string.Concat((object) task.DaysToComplete) : "";
      if (task.TaskPriority == MilestoneTaskDefinition.MilestoneTaskPriority.Normal)
        this.cboPriority.SelectedIndex = 1;
      else if (task.TaskPriority == MilestoneTaskDefinition.MilestoneTaskPriority.High)
        this.cboPriority.SelectedIndex = 2;
      else
        this.cboPriority.SelectedIndex = 0;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a name for the task.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.txtDescription.Text.Trim() == string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter a description for the task.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if ((this.task == null || this.txtName.Text.Trim().ToLower() != this.task.TaskName.ToLower()) && this.session.ConfigurationManager.IsDuplicatedMilestoneTasks(this.txtName.Text.Trim()))
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The task name has been used. Please use different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.task == null)
        {
          this.task = new MilestoneTaskDefinition(this.txtName.Text.Trim(), this.txtDescription.Text.Trim(), Utils.ParseInt((object) this.txtDays.Text.Trim()), (MilestoneTaskDefinition.MilestoneTaskPriority) Enum.Parse(typeof (MilestoneTaskDefinition.MilestoneTaskPriority), this.cboPriority.Text, true));
        }
        else
        {
          this.task.TaskName = this.txtName.Text.Trim();
          this.task.TaskDescription = this.txtDescription.Text.Trim();
          this.task.DaysToComplete = Utils.ParseInt(this.txtDays.Text.Trim() == string.Empty ? (object) "0" : (object) this.txtDays.Text.Trim());
          this.task.TaskPriority = (MilestoneTaskDefinition.MilestoneTaskPriority) Enum.Parse(typeof (MilestoneTaskDefinition.MilestoneTaskPriority), this.cboPriority.Text, true);
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    public MilestoneTaskDefinition Task => this.task;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.txtName = new TextBox();
      this.label1 = new Label();
      this.txtDescription = new TextBox();
      this.label2 = new Label();
      this.btnOK = new Button();
      this.label3 = new Label();
      this.txtDays = new TextBox();
      this.label4 = new Label();
      this.cboPriority = new ComboBox();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(473, 179);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.txtName.Location = new Point(105, 12);
      this.txtName.MaxLength = 256;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(443, 20);
      this.txtName.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Name";
      this.txtDescription.Location = new Point(105, 41);
      this.txtDescription.MaxLength = 4096;
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Both;
      this.txtDescription.Size = new Size(443, 78);
      this.txtDescription.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 44);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Description";
      this.btnOK.Location = new Point(392, 179);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 134);
      this.label3.Name = "label3";
      this.label3.Size = new Size(90, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Days to Complete";
      this.txtDays.Location = new Point(106, 131);
      this.txtDays.MaxLength = 3;
      this.txtDays.Name = "txtDays";
      this.txtDays.Size = new Size(53, 20);
      this.txtDays.TabIndex = 2;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(175, 134);
      this.label4.Name = "label4";
      this.label4.Size = new Size(38, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Priority";
      this.cboPriority.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPriority.FormattingEnabled = true;
      this.cboPriority.Items.AddRange(new object[3]
      {
        (object) "Low",
        (object) "Normal",
        (object) "High"
      });
      this.cboPriority.Location = new Point(219, 131);
      this.cboPriority.MaxDropDownItems = 3;
      this.cboPriority.Name = "cboPriority";
      this.cboPriority.Size = new Size(121, 21);
      this.cboPriority.TabIndex = 3;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(560, 216);
      this.Controls.Add((Control) this.cboPriority);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtDays);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MilestoneTaskDetailSetupForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Task Setup";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
