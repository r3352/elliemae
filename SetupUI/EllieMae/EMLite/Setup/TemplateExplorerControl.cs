// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemplateExplorerControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TemplateExplorerControl : UserControl
  {
    private Sessions.Session session;
    private TemplateFileSystem fileSystem;
    private IContainer components;
    private FileSystemExplorer explorer;

    public TemplateExplorerControl(EllieMae.EMLite.ClientServer.TemplateSettingsType templateType, Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.fileSystem = TemplateFileSystem.Create(templateType, session);
      this.explorer.AttachFileSystem(session, (IFileSystem) this.fileSystem, this.fileSystem.GetLastVisitedFolder());
    }

    private void onExplorerFolderChanged(object sender, EventArgs e)
    {
      this.fileSystem.SetLastVisitedFolder(this.explorer.CurrentFolder);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.explorer = new FileSystemExplorer();
      this.SuspendLayout();
      this.explorer.Dock = DockStyle.Fill;
      this.explorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.explorer.Location = new Point(0, 0);
      this.explorer.Name = "explorer";
      this.explorer.Size = new Size(787, 600);
      this.explorer.TabIndex = 0;
      this.explorer.Title = "<title>";
      this.explorer.FolderChanged += new EventHandler(this.onExplorerFolderChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.explorer);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TemplateExplorerControl);
      this.Size = new Size(787, 600);
      this.ResumeLayout(false);
    }
  }
}
