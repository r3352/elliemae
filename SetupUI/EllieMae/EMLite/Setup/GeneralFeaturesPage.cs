// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.GeneralFeaturesPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class GeneralFeaturesPage : PersonaTreePageBase
  {
    private Label label2;
    private IContainer components;

    public GeneralFeaturesPage(
      Sessions.Session session,
      int personaId,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.bIsUserSetup = false;
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.securityHelper = (IFeatureSecurityHelper) new GeneralFeaturesSecurityHelper(this.session, personaId);
      this.init();
    }

    public GeneralFeaturesPage(
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
      this.securityHelper = (IFeatureSecurityHelper) new GeneralFeaturesSecurityHelper(this.session, userId, personas);
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
      ResourceManager resourceManager = new ResourceManager(typeof (GeneralFeaturesPage));
      this.label2 = new Label();
      this.SuspendLayout();
      this.treeViewTabs.Location = new Point(8, 28);
      this.treeViewTabs.Name = "treeViewTabs";
      this.treeViewTabs.Size = new Size(256, 292);
      this.imgListTv.ImageStream = (ImageListStreamer) resourceManager.GetObject("imgListTv.ImageStream");
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(224, 16);
      this.label2.TabIndex = 1;
      this.label2.Text = "Access to Features";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(528, 361);
      this.Name = nameof (GeneralFeaturesPage);
      this.ResumeLayout(false);
    }
  }
}
