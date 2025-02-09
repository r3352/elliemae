// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardTemplateFormDialog
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class DashboardTemplateFormDialog : Form
  {
    private IContainer components;
    private Panel pnlExplorer;
    private DashboardTemplateControl objDashboardTemplateForm;

    public DashboardTemplate SelectedTemplate => this.objDashboardTemplateForm.SelectedTemplate;

    public FileSystemEntry SelectedFileSystemEntry
    {
      get => this.objDashboardTemplateForm.SelectedFileSystemEntry;
    }

    public DashboardTemplate EditedTemplate => this.objDashboardTemplateForm.EditedTemplate;

    public DashboardTemplateFormDialog(
      SnapshotBaseControl snapshot,
      bool isViewGlobal,
      bool isViewReadOnly)
      : this(snapshot, isViewGlobal, isViewReadOnly, Session.DefaultInstance)
    {
    }

    public DashboardTemplateFormDialog(
      SnapshotBaseControl snapshot,
      bool isViewGlobal,
      bool isViewReadOnly,
      Sessions.Session session)
    {
      this.objDashboardTemplateForm = new DashboardTemplateControl(snapshot, isViewGlobal, isViewReadOnly, session);
      this.InitializeComponent();
      this.setControls(DashboardTemplateFormDialog.ProcessingMode.EditTemplate);
    }

    public DashboardTemplateFormDialog(
      DashboardTemplateFormDialog.ProcessingMode processingMode,
      Sessions.Session session)
      : this(processingMode, false, session)
    {
    }

    public DashboardTemplateFormDialog(
      DashboardTemplateFormDialog.ProcessingMode processingMode)
      : this(processingMode, false)
    {
    }

    public DashboardTemplateFormDialog(
      DashboardTemplateFormDialog.ProcessingMode processingMode,
      bool showPublicOnly)
      : this(processingMode, showPublicOnly, Session.DefaultInstance)
    {
    }

    public DashboardTemplateFormDialog(
      DashboardTemplateFormDialog.ProcessingMode processingMode,
      bool showPublicOnly,
      Sessions.Session session)
    {
      this.objDashboardTemplateForm = new DashboardTemplateControl((DashboardTemplateControl.ProcessingMode) processingMode, showPublicOnly, session);
      this.InitializeComponent();
      this.setControls(processingMode);
    }

    private void setControls(
      DashboardTemplateFormDialog.ProcessingMode processingMode)
    {
      switch (processingMode)
      {
        case DashboardTemplateFormDialog.ProcessingMode.SelectTemplate:
          this.Size = this.objDashboardTemplateForm.GetSelectTemplateSize;
          break;
        case DashboardTemplateFormDialog.ProcessingMode.EditTemplate:
        case DashboardTemplateFormDialog.ProcessingMode.ManageTemplates:
          this.Size = this.objDashboardTemplateForm.GetManageTemplateSize;
          break;
      }
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DashboardTemplateFormDialog));
      this.pnlExplorer = new Panel();
      this.SuspendLayout();
      this.pnlExplorer.Controls.Add((Control) this.objDashboardTemplateForm);
      this.pnlExplorer.Dock = DockStyle.Fill;
      this.pnlExplorer.Location = new Point(0, 0);
      this.pnlExplorer.Name = "pnlExplorer";
      this.pnlExplorer.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(969, 638);
      this.Controls.Add((Control) this.pnlExplorer);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new Size(312, 560);
      this.Name = nameof (DashboardTemplateFormDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Manage Snapshots";
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
