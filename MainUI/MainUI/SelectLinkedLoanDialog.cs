// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.SelectLinkedLoanDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class SelectLinkedLoanDialog : Form
  {
    private Panel panelBottom;
    private Panel panelTop;
    private System.ComponentModel.Container components;
    private Button cancelBtn;
    private Button selectBtn;
    private string currentGUID = string.Empty;
    private PipelineScreen pipelinePage;
    private PipelineInfo selectedPipelineInfo;

    public SelectLinkedLoanDialog(string currentGUID)
    {
      this.currentGUID = currentGUID;
      this.InitializeComponent();
      this.pipelinePage = new PipelineScreen();
      this.pipelinePage.Parent = (Control) this;
      this.pipelinePage.DisabledButtons();
      this.pipelinePage.Initialize();
      this.pipelinePage.Dock = DockStyle.Fill;
      this.panelTop.Controls.Add((Control) this.pipelinePage);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        this.pipelinePage.Dispose();
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    public PipelineInfo SelectedPipelineInfo => this.selectedPipelineInfo;

    private void InitializeComponent()
    {
      this.panelBottom = new Panel();
      this.selectBtn = new Button();
      this.cancelBtn = new Button();
      this.panelTop = new Panel();
      this.panelBottom.SuspendLayout();
      this.SuspendLayout();
      this.panelBottom.Controls.Add((Control) this.selectBtn);
      this.panelBottom.Controls.Add((Control) this.cancelBtn);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 478);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(796, 40);
      this.panelBottom.TabIndex = 0;
      this.selectBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.selectBtn.Location = new Point(628, 9);
      this.selectBtn.Name = "selectBtn";
      this.selectBtn.Size = new Size(75, 23);
      this.selectBtn.TabIndex = 6;
      this.selectBtn.Text = "&Select";
      this.selectBtn.Click += new EventHandler(this.selectBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(712, 9);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 5;
      this.cancelBtn.Text = "&Cancel";
      this.panelTop.Dock = DockStyle.Fill;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(796, 478);
      this.panelTop.TabIndex = 1;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(796, 518);
      this.Controls.Add((Control) this.panelTop);
      this.Controls.Add((Control) this.panelBottom);
      this.MinimizeBox = false;
      this.Name = nameof (SelectLinkedLoanDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Loan";
      this.panelBottom.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void selectBtn_Click(object sender, EventArgs e)
    {
      this.selectedPipelineInfo = this.pipelinePage.GetSelectedPipelineInfo();
      if (this.selectedPipelineInfo == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a loan first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.currentGUID == this.selectedPipelineInfo.GUID)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot select the current opened loan. Please select a different loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }
  }
}
