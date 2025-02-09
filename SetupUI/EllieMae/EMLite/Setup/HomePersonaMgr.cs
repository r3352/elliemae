// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.HomePersonaMgr
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
  public class HomePersonaMgr : PersonaTreePageBase
  {
    private int personaID;
    private string userID;
    private Persona[] personaList;
    private IContainer components;

    public HomePersonaMgr(Sessions.Session session, int personaId, EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.InitializeComponent();
      this.personaID = personaId;
      this.bIsUserSetup = false;
      this.securityHelper = (IFeatureSecurityHelper) new HomePersonaMgrSecurityHelper(this.session, personaId);
      this.contextMenu1.MenuItems.Remove(this.menuItemLinkWithPersona);
      this.contextMenu1.MenuItems.Remove(this.menuItemDisconnectFromPersona);
      this.myInit();
      this.init();
    }

    public HomePersonaMgr(
      Sessions.Session session,
      string userId,
      Persona[] personas,
      EventHandler dirtyFlagChanged)
      : base(session, dirtyFlagChanged)
    {
      this.bIsUserSetup = true;
      this.userID = userId;
      this.personaList = personas;
      this.securityHelper = (IFeatureSecurityHelper) new HomePersonaMgrSecurityHelper(this.session, userId, personas);
      this.myInit();
      this.init();
      this.treeViewTabs.ImageList = this.imgListTv;
      this.contextMenu1.MenuItems.Remove(this.menuItemCheckAll);
      this.contextMenu1.MenuItems.Remove(this.menuItemUncheckAll);
    }

    private void myInit()
    {
      this.Title = "Home Page";
      this.FormBorderStyle = FormBorderStyle.None;
      this.ShowGroupContainer = false;
      this.treeViewTabs.ShowLines = false;
      this.treeViewTabs.ShowRootLines = false;
    }

    public void Save()
    {
      if (!this.NeedToSaveData())
        return;
      this.UpdatePermissions();
    }

    public void Reset() => this.ResetTree();

    public int PersonaID
    {
      get => this.personaID;
      set
      {
        if (this.personaID != value)
        {
          this.personaID = value;
          this.Reset();
        }
        else
          this.personaID = value;
      }
    }

    public bool NeedToSaveData() => this.hasBeenModified();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(307, 92);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (HomePersonaMgr);
      this.Text = nameof (HomePersonaMgr);
      this.ResumeLayout(false);
    }
  }
}
