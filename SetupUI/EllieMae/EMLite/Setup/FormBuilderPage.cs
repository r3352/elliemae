// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FormBuilderPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.PersonaSetup.SecurityPage;
using System;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FormBuilderPage : PersonaTreePageBase
  {
    private IContainer components;

    public FormBuilderPage(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.bIsUserSetup = false;
      this.securityHelper = (IFeatureSecurityHelper) new FormBuilderSecurityHelper(session, personaId);
      this.myInit();
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
    }

    public FormBuilderPage(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.bIsUserSetup = true;
      this.securityHelper = (IFeatureSecurityHelper) new FormBuilderSecurityHelper(this.session, userId, personas);
      this.myInit();
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
    }

    private void myInit()
    {
      this.InitializeComponent();
      this.Title = "Other";
      this.treeViewTabs.ShowLines = this.treeViewTabs.ShowRootLines = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (PipelineLoanTabPage));
      this.gcTreeView.SuspendLayout();
      this.SuspendLayout();
      this.treeViewTabs.LineColor = Color.Black;
      this.treeViewTabs.Scrollable = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(528, 361);
      this.Name = "GeneralGlobalPage";
      this.gcTreeView.ResumeLayout(false);
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
