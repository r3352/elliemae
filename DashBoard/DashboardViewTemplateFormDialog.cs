// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardViewTemplateFormDialog
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientSession.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class DashboardViewTemplateFormDialog : Form
  {
    private IContainer components;
    private Panel panelMain;
    private DashboardViewTemplateControl objDashboardViewTemplateForm;

    public DashboardView DashboardView => this.objDashboardViewTemplateForm.DashboardView;

    public FileSystemEntry DashboardViewFileSystemEntry
    {
      get => this.objDashboardViewTemplateForm.DashboardViewFileSystemEntry;
    }

    public FileSystemEntry OriFileSystemEntry
    {
      get => this.objDashboardViewTemplateForm.OriFileSystemEntry;
    }

    public DashboardViewTemplateFormDialog(
      Sessions.Session session,
      DashboardView dashboardView,
      FileSystemEntry fileSystem,
      DashboardViewTemplateFormDialog.ProcessingMode processingMode,
      List<ExternalOriginatorManagementData> externalOrgsList)
    {
      this.objDashboardViewTemplateForm = new DashboardViewTemplateControl(session, dashboardView, fileSystem, (DashboardViewTemplateControl.ProcessingMode) processingMode, externalOrgsList);
      this.InitializeComponent();
      if (processingMode == DashboardViewTemplateFormDialog.ProcessingMode.SelectTemplate)
      {
        this.Size = this.objDashboardViewTemplateForm.GetSelectTemplateSize;
      }
      else
      {
        if (processingMode != DashboardViewTemplateFormDialog.ProcessingMode.ManageTemplates)
          return;
        this.Size = this.objDashboardViewTemplateForm.GetManageTemplateSize;
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
      this.panelMain = new Panel();
      this.SuspendLayout();
      this.panelMain.Dock = DockStyle.Fill;
      this.panelMain.Location = new Point(0, 0);
      this.panelMain.Name = "panelMain";
      this.panelMain.TabIndex = 0;
      this.panelMain.Controls.Add((Control) this.objDashboardViewTemplateForm);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Window;
      this.ClientSize = new Size(914, 655);
      this.Controls.Add((Control) this.panelMain);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DashboardViewTemplateFormDialog);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Dashboard View";
      this.ResumeLayout(false);
    }

    public enum ProcessingMode
    {
      Unspecified,
      SelectTemplate,
      EditTemplate,
      ManageTemplates,
    }
  }
}
