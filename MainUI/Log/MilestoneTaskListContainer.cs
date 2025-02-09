// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.MilestoneTaskListContainer
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class MilestoneTaskListContainer : Form
  {
    private MilestoneTaskListControl taskListControl;
    private IContainer components;
    private Panel panel1;
    private Button btnClose;
    private Panel panelList;
    protected EMHelpLink emHelpLink1;

    public MilestoneTaskListContainer(LoanDataMgr loanMgr)
    {
      this.InitializeComponent();
      this.taskListControl = new MilestoneTaskListControl(loanMgr, true);
      this.panelList.Controls.Add((Control) this.taskListControl);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel1 = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.btnClose = new Button();
      this.panelList = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.emHelpLink1);
      this.panel1.Controls.Add((Control) this.btnClose);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 502);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(840, 40);
      this.panel1.TabIndex = 0;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Milestone Tasks";
      this.emHelpLink1.Location = new Point(12, 15);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 114;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(754, 8);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.panelList.Dock = DockStyle.Fill;
      this.panelList.Location = new Point(0, 0);
      this.panelList.Name = "panelList";
      this.panelList.Size = new Size(840, 502);
      this.panelList.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(840, 542);
      this.Controls.Add((Control) this.panelList);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (MilestoneTaskListContainer);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Task List";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
