// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ePass.AppraisalManagementControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using Elli.Web.Host.BrowserControls;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ePass
{
  public class AppraisalManagementControl : UserControl
  {
    private IContainer components;
    private GroupContainer gcAppraisal;
    private Button btnSettings;
    private EncWebFormBrowserControl ieBrowser;

    public AppraisalManagementControl()
    {
      this.InitializeComponent();
      this.ieBrowser.Navigate("https://appraisalcenter.elliemae.com/appraisal-settings.html");
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (!(Session.UserID != "admin") || aclManager.GetUserApplicationRight(AclFeature.SettingsTab_AppraisalOrderManagement))
        return;
      this.btnSettings.Enabled = false;
    }

    private void btnSettings_Click(object sender, EventArgs e)
    {
      if (!EpassLogin.LoginRequired(true))
        return;
      Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;APPRAISALRULES;2;SKIPWELCOME");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcAppraisal = new GroupContainer();
      this.ieBrowser = BrowserFactory.GetWebBrowserInstance();
      this.btnSettings = new Button();
      this.gcAppraisal.SuspendLayout();
      this.SuspendLayout();
      this.gcAppraisal.Controls.Add((Control) this.ieBrowser);
      this.gcAppraisal.Controls.Add((Control) this.btnSettings);
      this.gcAppraisal.Dock = DockStyle.Fill;
      this.gcAppraisal.HeaderForeColor = SystemColors.ControlText;
      this.gcAppraisal.Location = new Point(0, 0);
      this.gcAppraisal.Name = "gcAppraisal";
      this.gcAppraisal.Size = new Size(625, 409);
      this.gcAppraisal.TabIndex = 0;
      this.gcAppraisal.Text = "Appraisal Order Management";
      this.ieBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ieBrowser.Location = new Point(4, 27);
      this.ieBrowser.MinimumSize = new Size(20, 20);
      this.ieBrowser.Name = "ieBrowser";
      this.ieBrowser.Size = new Size(617, 379);
      this.ieBrowser.TabIndex = 6;
      this.btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSettings.Location = new Point(495, 2);
      this.btnSettings.Name = "btnSettings";
      this.btnSettings.Size = new Size(120, 21);
      this.btnSettings.TabIndex = 2;
      this.btnSettings.Text = "Change Settings...";
      this.btnSettings.UseVisualStyleBackColor = true;
      this.btnSettings.Click += new EventHandler(this.btnSettings_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcAppraisal);
      this.Name = nameof (AppraisalManagementControl);
      this.Size = new Size(625, 409);
      this.gcAppraisal.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
