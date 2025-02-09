// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.BorContactsPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class BorContactsPage : PersonaTreePageBase
  {
    private Label label1;
    private IContainer components;

    public BorContactsPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new BorContactsSecurityHelper(session, personaId);
      this.init();
    }

    public BorContactsPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.bIsUserSetup = true;
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
      this.securityHelper = (IFeatureSecurityHelper) new BorContactsSecurityHelper(session, userId, personas);
      this.init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BorContactsPage));
      this.label1 = new Label();
      this.SuspendLayout();
      this.treeViewTabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.treeViewTabs.LineColor = Color.Black;
      this.treeViewTabs.Location = new Point(8, 24);
      this.treeViewTabs.Scrollable = false;
      this.treeViewTabs.Size = new Size(531, 296);
      this.imgListTv.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgListTv.ImageStream");
      this.imgListTv.Images.SetKeyName(0, "");
      this.imgListTv.Images.SetKeyName(1, "");
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(148, 12);
      this.label1.TabIndex = 1;
      this.label1.Text = "Borrower Contacts";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(551, 361);
      this.Name = nameof (BorContactsPage);
      this.ResumeLayout(false);
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public void Reset() => this.ResetTree();

    public bool NeedToSaveData() => this.hasBeenModified();
  }
}
