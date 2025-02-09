// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOConnectSiteMngmnt
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOConnectSiteMngmnt : SettingsUserControl, IOnlineHelpTarget
  {
    private const string className = "TPOWCSiteMngmnt";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private Sessions.Session session;
    private IContainer components;
    private GroupContainer gcUrlSetup;

    public TPOConnectSiteMngmnt(SetUpContainer setupCont, Sessions.Session session)
      : base(setupCont)
    {
      this.InitializeComponent();
      this.session = session;
      this.refreshPageValue();
    }

    private void refreshPageValue()
    {
      List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
      List<ExternalOrgURL> list = ((IEnumerable<ExternalOrgURL>) this.session.ConfigurationManager.GetExternalOrganizationURLs()).ToList<ExternalOrgURL>();
      int num = 0;
      foreach (ExternalOrgURL externalOrgUrl in list)
      {
        if (!externalOrgUrl.isDeleted && externalOrgUrl.TPOAdminLinkAccess)
        {
          TPOConnectSiteMngmntRow connectSiteMngmntRow = new TPOConnectSiteMngmntRow(externalOrgUrl.URLID, externalOrgUrl.siteId, externalOrgUrl.URL);
          connectSiteMngmntRow.Location = new Point(20, 35 + num * 40);
          connectSiteMngmntRow.Tag = (object) externalOrgUrl;
          connectSiteMngmntRow.OnUrlChanged += new EventHandler(this.OnUrlChanged);
          this.gcUrlSetup.Controls.Add((Control) connectSiteMngmntRow);
          ++num;
        }
      }
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      List<ExternalOrgURL> externalOrgUrlList = new List<ExternalOrgURL>();
      List<ExternalOrgURL> list = ((IEnumerable<ExternalOrgURL>) this.session.ConfigurationManager.GetExternalOrganizationURLs()).ToList<ExternalOrgURL>();
      foreach (Control control in (ArrangedElementCollection) this.gcUrlSetup.Controls)
      {
        Control c = control;
        if (c is TPOConnectSiteMngmntRow)
        {
          ExternalOrgURL externalOrgUrl = list.Where<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (x => x.URLID == ((ExternalOrgURL) c.Tag).URLID)).First<ExternalOrgURL>();
          ((TPOConnectSiteMngmntRow) c).URL = externalOrgUrl.URL;
        }
      }
      this.setDirtyFlag(false);
    }

    private void OnUrlChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    public override void Save()
    {
      if (!this.validate())
        return;
      foreach (Control control in (ArrangedElementCollection) this.gcUrlSetup.Controls)
      {
        if (control is TPOConnectSiteMngmntRow && ((TPOConnectSiteMngmntRow) control).RowUpdated)
        {
          ExternalOrgURL tag = (ExternalOrgURL) control.Tag;
          tag.URL = ((TPOConnectSiteMngmntRow) control).URL;
          this.session.ConfigurationManager.UpdateExternalOrganizationURL(tag);
        }
      }
      this.setDirtyFlag(false);
    }

    private bool validate()
    {
      bool flag = true;
      foreach (Control control in (ArrangedElementCollection) this.gcUrlSetup.Controls)
      {
        if (control is TPOConnectSiteMngmntRow && ((TPOConnectSiteMngmntRow) control).RowUpdated && ((TPOConnectSiteMngmntRow) control).URL.Trim().Length <= 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Associated URL can not be empty", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          flag = false;
          break;
        }
      }
      return flag;
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\TPO Connect Site Management";

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcUrlSetup = new GroupContainer();
      this.SuspendLayout();
      this.gcUrlSetup.AutoScroll = true;
      this.gcUrlSetup.Dock = DockStyle.Fill;
      this.gcUrlSetup.HeaderForeColor = SystemColors.ControlText;
      this.gcUrlSetup.Location = new Point(0, 0);
      this.gcUrlSetup.Name = "gcUrlSetup";
      this.gcUrlSetup.Size = new Size(1011, 484);
      this.gcUrlSetup.TabIndex = 1;
      this.gcUrlSetup.Text = "TPO Connect URL Setup";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcUrlSetup);
      this.Name = nameof (TPOConnectSiteMngmnt);
      this.Size = new Size(1011, 484);
      this.ResumeLayout(false);
    }
  }
}
