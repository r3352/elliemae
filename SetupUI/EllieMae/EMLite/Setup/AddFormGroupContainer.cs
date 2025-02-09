// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddFormGroupContainer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddFormGroupContainer : Form
  {
    private const string PROFILEID = "PrintFormGroups";
    private Sessions.Session session;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private FSExplorer formifsExplorer;
    private Label label1;

    public AddFormGroupContainer(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      if (this.formifsExplorer.GetSession() == null)
        this.formifsExplorer.SetSession(this.session);
      this.initForm();
    }

    private void initForm()
    {
      this.formifsExplorer.FileType = FSExplorer.FileTypes.PrintGroups;
      this.formifsExplorer.HasPublicRight = true;
      this.formifsExplorer.HideAllButtons = true;
      this.formifsExplorer.HideContextMenu();
      this.formifsExplorer.SetProperties(false, false, true, false);
      FileSystemEntry publicRoot;
      try
      {
        publicRoot = FileSystemEntry.Parse(this.session.GetPrivateProfileString("PrintFormGroups", "LastFolderViewed") ?? "");
      }
      catch (Exception ex)
      {
        publicRoot = FileSystemEntry.PublicRoot;
      }
      this.formifsExplorer.Init((IFSExplorerBase) new UserFormGroups(this.session), publicRoot, true);
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.formifsExplorer.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a form group.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public FormInfo[] SelectedFormGroups
    {
      get
      {
        FormInfo[] selectedFormGroups = new FormInfo[this.formifsExplorer.SelectedItems.Count];
        for (int index = 0; index < this.formifsExplorer.SelectedItems.Count; ++index)
        {
          FileSystemEntry tag = (FileSystemEntry) this.formifsExplorer.SelectedItems[index].Tag;
          selectedFormGroups[index] = new FormInfo(tag.ToString(), OutputFormType.FormGroup);
        }
        return selectedFormGroups;
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
      this.formifsExplorer = new FSExplorer();
      this.label1 = new Label();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 470);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(556, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.formifsExplorer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.formifsExplorer.FolderComboSelectedIndex = -1;
      this.formifsExplorer.HasPublicRight = true;
      this.formifsExplorer.Location = new Point(12, 12);
      this.formifsExplorer.Name = "formifsExplorer";
      this.formifsExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.formifsExplorer.Size = new Size(532, 425);
      this.formifsExplorer.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 444);
      this.label1.Name = "label1";
      this.label1.Size = new Size(398, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Any future changes made to this form group will be automatically applied to the rule.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(556, 514);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.formifsExplorer);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddFormGroupContainer);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Form Group";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
