// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MilestoneTaskWorksheetContainer
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MilestoneTaskWorksheetContainer : Form
  {
    private MilestoneTaskWorksheet taskWorksheet;
    private MilestoneTaskLog taskLog;
    private IContainer components;
    private Panel panelWorksheet;
    protected EMHelpLink emHelpLink1;
    private Panel panel1;
    private Button btnOK;
    private Button btnCancel;

    public MilestoneTaskWorksheetContainer(MilestoneTaskLog taskLog, bool editable)
    {
      this.taskLog = taskLog;
      this.InitializeComponent();
      this.taskWorksheet = new MilestoneTaskWorksheet(this.taskLog, true, editable);
      this.panelWorksheet.Controls.Add((Control) this.taskWorksheet);
      this.btnOK.Visible = editable;
      if (editable)
        return;
      this.btnCancel.Text = "&OK";
    }

    public MilestoneTaskLog TaskLog => this.taskLog;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.taskWorksheet.ValidateMilestoneTask())
        return;
      if (this.taskLog.TaskGUID == string.Empty)
      {
        Hashtable milestoneTasks = Session.ConfigurationManager.GetMilestoneTasks();
        if (milestoneTasks != null)
        {
          foreach (DictionaryEntry dictionaryEntry in milestoneTasks)
          {
            MilestoneTaskDefinition milestoneTaskDefinition = (MilestoneTaskDefinition) dictionaryEntry.Value;
            if (string.Compare(milestoneTaskDefinition.TaskName, this.taskLog.TaskName, true) == 0)
            {
              this.taskLog.TaskGUID = milestoneTaskDefinition.TaskGUID;
              break;
            }
          }
        }
      }
      Session.LoanData.GetLogList().AddRecord((LogRecordBase) this.taskLog);
      Session.Application.GetService<ILoanEditor>()?.RefreshContents();
      this.DialogResult = DialogResult.OK;
    }

    private void MilestoneTaskWorksheetContainer_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelWorksheet = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.panel1 = new Panel();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panelWorksheet.Dock = DockStyle.Fill;
      this.panelWorksheet.Location = new Point(0, 0);
      this.panelWorksheet.Name = "panelWorksheet";
      this.panelWorksheet.Size = new Size(799, 542);
      this.panelWorksheet.TabIndex = 1;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Milestone Tasks";
      this.emHelpLink1.Location = new Point(12, 17);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 113;
      this.panel1.Controls.Add((Control) this.btnOK);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.emHelpLink1);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 542);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(799, 45);
      this.panel1.TabIndex = 0;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(631, 10);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 115;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(712, 10);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 114;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(799, 587);
      this.Controls.Add((Control) this.panelWorksheet);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (MilestoneTaskWorksheetContainer);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Task";
      this.KeyPress += new KeyPressEventHandler(this.MilestoneTaskWorksheetContainer_KeyPress);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
