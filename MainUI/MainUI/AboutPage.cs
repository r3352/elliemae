// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.AboutPage
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class AboutPage : Form
  {
    private Size originalSize = new Size(439, 306);
    private string scVersion;
    private string serverPatch;
    private string lblBuildText;
    private IContainer components;
    private Label lblClientIdTitle;
    private Label lblClientId;
    private Label lblVersion;
    private Label lblLicensee;
    private Label lblCopyright;
    private EllieMae.EMLite.UI.LinkLabel lnkTraining;
    private EllieMae.EMLite.UI.LinkLabel lnkSupport;
    private Button btnOK;
    private Label label1;
    private Label label2;
    private Label lblBuild;
    private EllieMae.EMLite.UI.LinkLabel lnkPrivacy;
    private Panel panel1;
    private Panel panel2;
    private Label lblServicePack;
    private Label lblMajorRelease;
    private Label lblMajorRelease1;
    private Label lblServicePack1;
    private Label lblCriticalPatch1;
    private Label lblMajorRelease2;
    private Label lblServicePack2;
    private Label lblCriticalPatch2;

    public AboutPage()
    {
      this.InitializeComponent();
      this.lblVersion.Text = Session.EncompassEdition.ToString();
      string serverDllVersion = Session.ServerManager.GetServerDllVersion();
      string str = serverDllVersion.Substring(serverDllVersion.LastIndexOf('.') + 1);
      this.lblBuildText = !(str != "0") ? VersionInformation.CurrentVersion.DisplayVersionString : VersionInformation.CurrentVersion.DisplayVersionString + " server patch " + str;
      this.lblBuild.Text = "Build " + this.lblBuildText;
      if ((BillingModel) Session.StartupInfo.LicenseSettings[(object) "License.BillingModel"] == BillingModel.ClosedLoan)
        this.lblBuild.Text += " (SBP)";
      CompanyInfo companyInfo = Session.ConfigurationManager.GetCompanyInfo();
      this.lblLicensee.Text = companyInfo.Name;
      this.lblClientId.Text = companyInfo.ClientID;
      this.setCopyrightText();
      this.originalSize = this.Size;
      this.scVersion = VersionInformation.CurrentVersion.DisplayVersionString;
      this.serverPatch = str;
      this.getSPCP();
    }

    private void setCopyrightText()
    {
      this.lblCopyright.Text = string.Format("© {0} ICE Mortgage Technology. All rights reserved. Encompass® and the ICE Mortgage", (object) DateTime.Now.Year) + " Technology logos are trademarks or registered trademarks of ICE Mortgage Technology or its subsidiaries.";
    }

    private string trim(string str) => (str ?? "").Trim();

    private void setReleaseInfo(string spcp)
    {
      bool hide = string.IsNullOrWhiteSpace(spcp);
      if (!hide)
      {
        string[] strArray1 = spcp.Split(new char[1]{ ';' }, StringSplitOptions.None);
        if (strArray1.Length == 3)
        {
          string[] strArray2 = strArray1[0].Split(new char[1]
          {
            ','
          }, StringSplitOptions.None);
          string[] strArray3 = strArray1[1].Split(new char[1]
          {
            ','
          }, StringSplitOptions.None);
          string[] strArray4 = strArray1[2].Split(new char[1]
          {
            ','
          }, StringSplitOptions.None);
          if (strArray2.Length == 2 && strArray3.Length == 2 && strArray4.Length == 2)
          {
            this.lblMajorRelease1.Text = this.trim(strArray2[0]);
            this.lblMajorRelease2.Text = this.trim(strArray2[1]);
            if (this.lblMajorRelease1.Text == "@")
            {
              System.Version version = new System.Version(Session.ServerManager.GetServerDllVersion());
              this.lblMajorRelease1.Text = version.Major.ToString() + "." + (object) version.Minor + (version.Build == 0 ? (object) "" : (object) ("." + (object) version.Build));
            }
            this.lblServicePack1.Text = this.trim(strArray3[0]);
            this.lblServicePack2.Text = this.trim(strArray3[1]);
            this.lblCriticalPatch1.Text = this.trim(strArray4[0]);
            this.lblCriticalPatch2.Text = this.trim(strArray4[1]);
            if (this.lblCriticalPatch2.Text == "@")
              this.lblCriticalPatch2.Text = "(" + this.lblBuildText + ")";
          }
          else
            hide = true;
        }
        else
          hide = true;
      }
      this.setVisibility(hide);
    }

    private void setVisibility(bool hide)
    {
      this.Size = hide ? this.originalSize : new Size(this.originalSize.Width, this.originalSize.Height + 50);
      this.lblMajorRelease.Visible = this.lblMajorRelease1.Visible = this.lblMajorRelease2.Visible = !hide;
      this.lblServicePack.Visible = this.lblServicePack1.Visible = this.lblServicePack2.Visible = !hide;
      this.lblCriticalPatch1.Visible = this.lblCriticalPatch2.Visible = !hide;
      if (!hide)
      {
        this.lblBuild.Text = "Critical Patch:";
        this.lblBuild.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      }
      else
        this.lblBuild.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    }

    private string getSPCP()
    {
      string spcp1 = (string) null;
      try
      {
        spcp1 = SmartClientUtils.GetAttribute("", "sc-" + this.scVersion, this.serverPatch);
        if (string.IsNullOrWhiteSpace(spcp1))
          spcp1 = SmartClientUtils.GetAttribute("", "sc-" + this.scVersion, "*");
        if (string.IsNullOrWhiteSpace(spcp1))
          spcp1 = VersionInformation.CurrentVersion.AboutPageVersionInfo;
        this.setReleaseInfo(spcp1);
        return spcp1;
      }
      catch (Exception ex)
      {
        string spcp2 = "Error getting/setting SP CP '" + spcp1 + "': " + ex.Message;
        this.setVisibility(true);
        return spcp2;
      }
    }

    public static void ShowSupportPage()
    {
      Session.Application.GetService<IHomePage>().Navigate("https://encompass.elliemae.com/homepage/_HOMEPAGE_SIGNATURE;EXECUTE;3010;SUPPORT2");
      Session.Application.GetService<IEncompassApplication>().SetCurrentActivity(EncompassActivity.Home);
    }

    public static void ShowTrainingPage()
    {
      Session.Application.GetService<IHomePage>().Navigate("https://encompass.elliemae.com/homepage/_HOMEPAGE_SIGNATURE;EXECUTE;3010;TRAINING2");
      Session.Application.GetService<IEncompassApplication>().SetCurrentActivity(EncompassActivity.Home);
    }

    private void lnkSupport_LinkClicked(object sender, EventArgs e) => AboutPage.ShowSupportPage();

    private void lnkTraining_LinkClicked(object sender, EventArgs e)
    {
      AboutPage.ShowTrainingPage();
    }

    private void llblLicense_LinkClicked(object sender, EventArgs e)
    {
      if (!File.Exists(AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.DocDirRelPath, "Help\\EncompassLicenseAgmt.rtf"), SystemSettings.LocalAppDir)))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Encompass can't load the license agreement file.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int num2 = (int) new LicenseDialog().ShowDialog((IWin32Window) this);
      }
    }

    private void lnkPrivacy_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<IHomePage>().Navigate("https://encompass.elliemae.com/homepage/ResourceCenter.aspx?target=privacy.aspx");
      Session.Application.GetService<IEncompassApplication>().SetCurrentActivity(EncompassActivity.Home);
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();

    private void lblBuild_DoubleClick(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show(this.getSPCP() ?? "", "Encompass SP CP");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblClientIdTitle = new Label();
      this.lblClientId = new Label();
      this.lblVersion = new Label();
      this.lblLicensee = new Label();
      this.lblCopyright = new Label();
      this.btnOK = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.lblBuild = new Label();
      this.panel1 = new Panel();
      this.panel2 = new Panel();
      this.lblServicePack = new Label();
      this.lblMajorRelease = new Label();
      this.lblMajorRelease1 = new Label();
      this.lblServicePack1 = new Label();
      this.lblCriticalPatch1 = new Label();
      this.lblMajorRelease2 = new Label();
      this.lblServicePack2 = new Label();
      this.lblCriticalPatch2 = new Label();
      this.lnkTraining = new EllieMae.EMLite.UI.LinkLabel();
      this.lnkSupport = new EllieMae.EMLite.UI.LinkLabel();
      this.lnkPrivacy = new EllieMae.EMLite.UI.LinkLabel();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.lblClientIdTitle.BackColor = Color.Transparent;
      this.lblClientIdTitle.Location = new Point(17, 85);
      this.lblClientIdTitle.Name = "lblClientIdTitle";
      this.lblClientIdTitle.Size = new Size(57, 19);
      this.lblClientIdTitle.TabIndex = 17;
      this.lblClientIdTitle.Text = "Client ID:";
      this.lblClientIdTitle.TextAlign = ContentAlignment.MiddleLeft;
      this.lblClientId.BackColor = Color.Transparent;
      this.lblClientId.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblClientId.Location = new Point(74, 85);
      this.lblClientId.Name = "lblClientId";
      this.lblClientId.Size = new Size(326, 19);
      this.lblClientId.TabIndex = 16;
      this.lblClientId.Text = "<Client ID>";
      this.lblClientId.TextAlign = ContentAlignment.MiddleLeft;
      this.lblVersion.BackColor = Color.Transparent;
      this.lblVersion.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblVersion.Location = new Point(74, 48);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new Size(326, 19);
      this.lblVersion.TabIndex = 11;
      this.lblVersion.Text = "<Edition>";
      this.lblVersion.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLicensee.BackColor = Color.Transparent;
      this.lblLicensee.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLicensee.Location = new Point(74, 66);
      this.lblLicensee.Name = "lblLicensee";
      this.lblLicensee.Size = new Size(326, 19);
      this.lblLicensee.TabIndex = 12;
      this.lblLicensee.Text = "<Company Name>";
      this.lblLicensee.TextAlign = ContentAlignment.MiddleLeft;
      this.lblCopyright.BackColor = Color.Transparent;
      this.lblCopyright.Dock = DockStyle.Fill;
      this.lblCopyright.Location = new Point(0, 0);
      this.lblCopyright.Name = "lblCopyright";
      this.lblCopyright.Size = new Size(380, 55);
      this.lblCopyright.TabIndex = 20;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(338, 236);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 21;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(17, 48);
      this.label1.Name = "label1";
      this.label1.Size = new Size(57, 19);
      this.label1.TabIndex = 22;
      this.label1.Text = "Edition:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(17, 66);
      this.label2.Name = "label2";
      this.label2.Size = new Size(57, 19);
      this.label2.TabIndex = 23;
      this.label2.Text = "Company:";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBuild.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblBuild.AutoSize = true;
      this.lblBuild.BackColor = Color.Transparent;
      this.lblBuild.Location = new Point(18, 240);
      this.lblBuild.Name = "lblBuild";
      this.lblBuild.Size = new Size(104, 19);
      this.lblBuild.TabIndex = 24;
      this.lblBuild.Text = "Build ######";
      this.lblBuild.DoubleClick += new EventHandler(this.lblBuild_DoubleClick);
      this.panel1.BackColor = Color.Transparent;
      this.panel1.BackgroundImage = (Image) Resources.Ice_logo;
      this.panel1.BackgroundImageLayout = ImageLayout.None;
      this.panel1.Location = new Point(21, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(41, 33);
      this.panel1.TabIndex = 26;
      this.panel2.AutoScroll = true;
      this.panel2.BackColor = Color.Transparent;
      this.panel2.Controls.Add((Control) this.lblCopyright);
      this.panel2.Location = new Point(15, 107);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(380, 55);
      this.panel2.TabIndex = 27;
      this.lblServicePack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblServicePack.AutoSize = true;
      this.lblServicePack.BackColor = Color.Transparent;
      this.lblServicePack.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblServicePack.Location = new Point(17, 222);
      this.lblServicePack.Name = "lblServicePack";
      this.lblServicePack.Size = new Size(116, 19);
      this.lblServicePack.TabIndex = 28;
      this.lblServicePack.Text = "Service Pack:";
      this.lblServicePack.Visible = false;
      this.lblMajorRelease.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblMajorRelease.AutoSize = true;
      this.lblMajorRelease.BackColor = Color.Transparent;
      this.lblMajorRelease.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMajorRelease.Location = new Point(19, 204);
      this.lblMajorRelease.Name = "lblMajorRelease";
      this.lblMajorRelease.Size = new Size(124, 19);
      this.lblMajorRelease.TabIndex = 29;
      this.lblMajorRelease.Text = "Major Release:";
      this.lblMajorRelease1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblMajorRelease1.AutoSize = true;
      this.lblMajorRelease1.BackColor = Color.Transparent;
      this.lblMajorRelease1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMajorRelease1.Location = new Point(105, 204);
      this.lblMajorRelease1.Name = "lblMajorRelease1";
      this.lblMajorRelease1.Size = new Size(48, 19);
      this.lblMajorRelease1.TabIndex = 30;
      this.lblMajorRelease1.Text = "MR 1";
      this.lblServicePack1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblServicePack1.AutoSize = true;
      this.lblServicePack1.BackColor = Color.Transparent;
      this.lblServicePack1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblServicePack1.Location = new Point(105, 222);
      this.lblServicePack1.Name = "lblServicePack1";
      this.lblServicePack1.Size = new Size(45, 19);
      this.lblServicePack1.TabIndex = 31;
      this.lblServicePack1.Text = "SP 1";
      this.lblCriticalPatch1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblCriticalPatch1.AutoSize = true;
      this.lblCriticalPatch1.BackColor = Color.Transparent;
      this.lblCriticalPatch1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCriticalPatch1.Location = new Point(105, 240);
      this.lblCriticalPatch1.Name = "lblCriticalPatch1";
      this.lblCriticalPatch1.Size = new Size(46, 19);
      this.lblCriticalPatch1.TabIndex = 32;
      this.lblCriticalPatch1.Text = "CP 1";
      this.lblMajorRelease2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblMajorRelease2.AutoSize = true;
      this.lblMajorRelease2.BackColor = Color.Transparent;
      this.lblMajorRelease2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMajorRelease2.Location = new Point(169, 204);
      this.lblMajorRelease2.Name = "lblMajorRelease2";
      this.lblMajorRelease2.Size = new Size(122, 19);
      this.lblMajorRelease2.TabIndex = 33;
      this.lblMajorRelease2.Text = "MajorRelease2";
      this.lblServicePack2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblServicePack2.AutoSize = true;
      this.lblServicePack2.BackColor = Color.Transparent;
      this.lblServicePack2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblServicePack2.Location = new Point(169, 222);
      this.lblServicePack2.Name = "lblServicePack2";
      this.lblServicePack2.Size = new Size(114, 19);
      this.lblServicePack2.TabIndex = 34;
      this.lblServicePack2.Text = "ServicePack2";
      this.lblCriticalPatch2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblCriticalPatch2.AutoSize = true;
      this.lblCriticalPatch2.BackColor = Color.Transparent;
      this.lblCriticalPatch2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCriticalPatch2.Location = new Point(169, 240);
      this.lblCriticalPatch2.Name = "lblCriticalPatch2";
      this.lblCriticalPatch2.Size = new Size(118, 19);
      this.lblCriticalPatch2.TabIndex = 35;
      this.lblCriticalPatch2.Text = "CriticalPatch2";
      this.lnkTraining.AutoSize = true;
      this.lnkTraining.BackColor = Color.Transparent;
      this.lnkTraining.Location = new Point(17, 186);
      this.lnkTraining.Name = "lnkTraining";
      this.lnkTraining.Size = new Size(157, 19);
      this.lnkTraining.TabIndex = 15;
      this.lnkTraining.TabStop = true;
      this.lnkTraining.Text = "Encompass Training";
      this.lnkTraining.Click += new EventHandler(this.lnkTraining_LinkClicked);
      this.lnkSupport.AutoSize = true;
      this.lnkSupport.BackColor = Color.Transparent;
      this.lnkSupport.Location = new Point(17, 168);
      this.lnkSupport.Name = "lnkSupport";
      this.lnkSupport.Size = new Size(156, 19);
      this.lnkSupport.TabIndex = 14;
      this.lnkSupport.TabStop = true;
      this.lnkSupport.Text = "Encompass Support";
      this.lnkSupport.Click += new EventHandler(this.lnkSupport_LinkClicked);
      this.lnkPrivacy.AutoSize = true;
      this.lnkPrivacy.BackColor = Color.Transparent;
      this.lnkPrivacy.Location = new Point(17, 204);
      this.lnkPrivacy.Name = "lnkPrivacy";
      this.lnkPrivacy.Size = new Size(126, 19);
      this.lnkPrivacy.TabIndex = 25;
      this.lnkPrivacy.TabStop = true;
      this.lnkPrivacy.Text = "Privacy Policies";
      this.lnkPrivacy.Click += new EventHandler(this.lnkPrivacy_Click);
      this.AutoScaleDimensions = new SizeF(9f, 19f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.BackgroundImage = (Image) Resources.about_background;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.ClientSize = new Size(423, 268);
      this.Controls.Add((Control) this.lblCriticalPatch2);
      this.Controls.Add((Control) this.lblServicePack2);
      this.Controls.Add((Control) this.lblMajorRelease2);
      this.Controls.Add((Control) this.lblCriticalPatch1);
      this.Controls.Add((Control) this.lblServicePack1);
      this.Controls.Add((Control) this.lblMajorRelease1);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.lblClientId);
      this.Controls.Add((Control) this.lblVersion);
      this.Controls.Add((Control) this.lblLicensee);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblClientIdTitle);
      this.Controls.Add((Control) this.lnkTraining);
      this.Controls.Add((Control) this.lnkSupport);
      this.Controls.Add((Control) this.lblBuild);
      this.Controls.Add((Control) this.lnkPrivacy);
      this.Controls.Add((Control) this.lblMajorRelease);
      this.Controls.Add((Control) this.lblServicePack);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AboutPage);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "About Encompass";
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
