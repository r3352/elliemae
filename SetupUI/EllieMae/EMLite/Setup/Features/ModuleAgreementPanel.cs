// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Features.ModuleAgreementPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using AxSHDocVw;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.WebServices;
using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Features
{
  public class ModuleAgreementPanel : UserControl
  {
    private Label label1;
    private Button btnAgree;
    private System.ComponentModel.Container components;
    private EncompassModule module;
    private AxWebBrowser axWebBrowser1;
    private ModuleLicense license;
    private Sessions.Session session;

    public ModuleAgreementPanel(
      EncompassModule module,
      ModuleLicense license,
      Sessions.Session session)
    {
      this.module = module;
      this.license = license;
      this.session = session;
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (ModuleAgreementPanel));
      this.label1 = new Label();
      this.btnAgree = new Button();
      this.axWebBrowser1 = new AxWebBrowser();
      this.axWebBrowser1.BeginInit();
      this.SuspendLayout();
      this.label1.Location = new Point(25, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(383, 18);
      this.label1.TabIndex = 2;
      this.label1.Text = "To being using this add-on you must first read and agree to the following:";
      this.btnAgree.Anchor = AnchorStyles.Bottom;
      this.btnAgree.Location = new Point(207, 278);
      this.btnAgree.Name = "btnAgree";
      this.btnAgree.TabIndex = 3;
      this.btnAgree.Text = "I Agree";
      this.btnAgree.Click += new EventHandler(this.btnAgree_Click);
      this.axWebBrowser1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.axWebBrowser1.Enabled = true;
      this.axWebBrowser1.Location = new Point(26, 38);
      this.axWebBrowser1.OcxState = (AxHost.State) resourceManager.GetObject("axWebBrowser1.OcxState");
      this.axWebBrowser1.Size = new Size(424, 226);
      this.axWebBrowser1.TabIndex = 4;
      this.Controls.Add((Control) this.axWebBrowser1);
      this.Controls.Add((Control) this.btnAgree);
      this.Controls.Add((Control) this.label1);
      this.Name = nameof (ModuleAgreementPanel);
      this.Size = new Size(488, 382);
      this.Resize += new EventHandler(this.ModuleAgreementPanel_Resize);
      this.Load += new EventHandler(this.ModuleAgreementPanel_Load);
      this.axWebBrowser1.EndInit();
      this.ResumeLayout(false);
    }

    private void ModuleAgreementPanel_Resize(object sender, EventArgs e)
    {
      this.btnAgree.Left = (this.ClientSize.Width - this.btnAgree.Width) / 2;
    }

    private void btnAgree_Click(object sender, EventArgs e)
    {
      FeaturePanel parent = (FeaturePanel) this.Parent;
      if (this.license.UserLimit >= 0)
        parent.LoadControl((Control) new UserAssignmentPanel(this.module, this.license));
      else
        parent.LoadControl((Control) new UnlimitedUserAccessPanel(this.module, this.license));
    }

    private void ModuleAgreementPanel_Load(object sender, EventArgs e)
    {
      string jumpUrl = WebLink.GetJumpURL("ModuleAgreement.asp?module=" + (object) this.module, this.session);
      object missing = System.Type.Missing;
      this.axWebBrowser1.Navigate(jumpUrl, ref missing, ref missing, ref missing, ref missing);
    }
  }
}
