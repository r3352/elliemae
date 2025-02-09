// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.InstallationUrlDialog
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.Properties;
using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  public class InstallationUrlDialog : Form
  {
    private InstallationUrlBrowserControl urlBrowser;
    private string installationURL = "";
    private IContainer components;
    private Panel pnlRightBottom;
    private Button btnCancel;
    private Button btnOpen;
    private Label lblSubTitle;
    private Label lblTitle;
    private Panel panelLeft;
    private Button btnWeb;
    private Button btnIIS;
    private Button btnFile;
    private SplitContainer splitContainer1;
    private Panel pnlRightTop;
    private TextBox txtBoxURL;
    private Label lblPath;
    private Panel pnlRightMiddle;
    private CheckBox chkBoxSSL;

    public string InstallationURL => this.installationURL;

    public InstallationUrlDialog(string installationUrl)
    {
      this.InitializeComponent();
      this.installationURL = installationUrl;
      this.initializePage();
    }

    private void initializePage()
    {
      if (BasicUtils.IsHttpOrHttps(this.installationURL))
        this.btnWeb_Click((object) this, (EventArgs) null);
      else
        this.btnFile_Click((object) this, (EventArgs) null);
    }

    private void enableButton(Button button)
    {
      this.btnFile.BackColor = SystemColors.Control;
      this.btnFile.BackgroundImage = (Image) Resources.File_Disabled;
      this.btnIIS.BackColor = SystemColors.Control;
      this.btnIIS.BackgroundImage = (Image) Resources.IIS_Disabled;
      this.btnWeb.BackColor = SystemColors.Control;
      this.btnWeb.BackgroundImage = (Image) Resources.Web_Disabled;
      this.pnlRightTop.Visible = false;
      this.pnlRightMiddle.Visible = false;
      if (button == this.btnFile)
      {
        this.btnFile.BackColor = Color.White;
        this.btnFile.BackgroundImage = (Image) Resources.File;
        this.lblTitle.Text = "File System";
        this.lblSubTitle.Text = "Select the folder you want to open.";
      }
      else if (button == this.btnIIS)
      {
        this.btnIIS.BackColor = Color.White;
        this.btnIIS.BackgroundImage = (Image) Resources.IIS;
        this.lblTitle.Text = "Local Internet Information Server";
        this.lblSubTitle.Text = "Select the Web site you want to open.";
      }
      else
      {
        if (button != this.btnWeb)
          return;
        this.btnWeb.BackColor = Color.White;
        this.btnWeb.BackgroundImage = (Image) Resources.Web;
        this.lblTitle.Text = "Remote Site";
        this.lblSubTitle.Text = "From the Web site location, enter the URL of a Web site.";
        this.pnlRightMiddle.Visible = true;
        this.pnlRightTop.Visible = true;
      }
    }

    private void createUrlBrowser(InstallationUrlBrowserControl.Type type)
    {
      this.pnlRightBottom.Controls.Clear();
      this.urlBrowser = new InstallationUrlBrowserControl(type, this.installationURL);
      this.urlBrowser.Dock = DockStyle.Left;
      this.pnlRightBottom.Controls.Add((Control) this.urlBrowser);
    }

    private void btnFile_Click(object sender, EventArgs e)
    {
      this.createUrlBrowser(InstallationUrlBrowserControl.Type.FileSystem);
      this.enableButton(this.btnFile);
      this.btnFile.Select();
    }

    private void btnIIS_Click(object sender, EventArgs e)
    {
      this.createUrlBrowser(InstallationUrlBrowserControl.Type.LocalIIS);
      this.enableButton(this.btnIIS);
      this.btnIIS.Select();
    }

    private void btnWeb_Click(object sender, EventArgs e)
    {
      this.urlBrowser = (InstallationUrlBrowserControl) null;
      this.pnlRightBottom.Controls.Clear();
      this.pnlRightBottom.Controls.Add((Control) this.chkBoxSSL);
      this.txtBoxURL.Text = BasicUtils.IsHttpOrHttps(this.installationURL) ? this.installationURL ?? "" : "";
      this.enableButton(this.btnWeb);
      this.btnWeb.Select();
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnOpen_Click(object sender, EventArgs e)
    {
      this.installationURL = this.urlBrowser == null ? this.txtBoxURL.Text : this.urlBrowser.InstallationURL;
      this.Close();
    }

    private void chkBoxSSL_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkBoxSSL.Checked)
      {
        if (!this.txtBoxURL.Text.ToLower().StartsWith(ResolverConsts.HttpPrefix))
          return;
        this.txtBoxURL.Text = this.txtBoxURL.Text.Insert(4, "s");
      }
      else
      {
        if (!this.txtBoxURL.Text.ToLower().StartsWith(ResolverConsts.HttpsPrefix))
          return;
        this.txtBoxURL.Text = this.txtBoxURL.Text.Remove(4, 1);
      }
    }

    private void txtBoxURL_TextChanged(object sender, EventArgs e)
    {
      if (this.txtBoxURL.Text.ToLower().StartsWith(ResolverConsts.HttpPrefix))
      {
        this.chkBoxSSL.Checked = false;
      }
      else
      {
        if (!this.txtBoxURL.Text.ToLower().StartsWith(ResolverConsts.HttpsPrefix))
          return;
        this.chkBoxSSL.Checked = true;
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
      this.splitContainer1 = new SplitContainer();
      this.panelLeft = new Panel();
      this.btnWeb = new Button();
      this.btnIIS = new Button();
      this.btnFile = new Button();
      this.pnlRightBottom = new Panel();
      this.pnlRightMiddle = new Panel();
      this.chkBoxSSL = new CheckBox();
      this.pnlRightTop = new Panel();
      this.txtBoxURL = new TextBox();
      this.lblPath = new Label();
      this.lblSubTitle = new Label();
      this.lblTitle = new Label();
      this.btnOpen = new Button();
      this.btnCancel = new Button();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.panelLeft.SuspendLayout();
      this.pnlRightBottom.SuspendLayout();
      this.pnlRightMiddle.SuspendLayout();
      this.pnlRightTop.SuspendLayout();
      this.SuspendLayout();
      this.splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.splitContainer1.FixedPanel = FixedPanel.Panel1;
      this.splitContainer1.IsSplitterFixed = true;
      this.splitContainer1.Location = new Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Panel1.BackColor = SystemColors.Control;
      this.splitContainer1.Panel1.Controls.Add((Control) this.panelLeft);
      this.splitContainer1.Panel2.Controls.Add((Control) this.pnlRightBottom);
      this.splitContainer1.Panel2.Controls.Add((Control) this.pnlRightMiddle);
      this.splitContainer1.Panel2.Controls.Add((Control) this.pnlRightTop);
      this.splitContainer1.Panel2.Controls.Add((Control) this.lblSubTitle);
      this.splitContainer1.Panel2.Controls.Add((Control) this.lblTitle);
      this.splitContainer1.Size = new Size(625, 375);
      this.splitContainer1.SplitterDistance = 97;
      this.splitContainer1.SplitterWidth = 8;
      this.splitContainer1.TabIndex = 0;
      this.panelLeft.BorderStyle = BorderStyle.Fixed3D;
      this.panelLeft.Controls.Add((Control) this.btnWeb);
      this.panelLeft.Controls.Add((Control) this.btnIIS);
      this.panelLeft.Controls.Add((Control) this.btnFile);
      this.panelLeft.Dock = DockStyle.Fill;
      this.panelLeft.Location = new Point(0, 0);
      this.panelLeft.Name = "panelLeft";
      this.panelLeft.Size = new Size(97, 375);
      this.panelLeft.TabIndex = 3;
      this.btnWeb.BackColor = Color.White;
      this.btnWeb.BackgroundImageLayout = ImageLayout.Center;
      this.btnWeb.Dock = DockStyle.Top;
      this.btnWeb.FlatAppearance.BorderColor = Color.White;
      this.btnWeb.Location = new Point(0, 116);
      this.btnWeb.Name = "btnWeb";
      this.btnWeb.Size = new Size(93, 53);
      this.btnWeb.TabIndex = 2;
      this.btnWeb.UseVisualStyleBackColor = false;
      this.btnWeb.Click += new EventHandler(this.btnWeb_Click);
      this.btnIIS.BackColor = Color.White;
      this.btnIIS.BackgroundImageLayout = ImageLayout.Center;
      this.btnIIS.Dock = DockStyle.Top;
      this.btnIIS.Location = new Point(0, 57);
      this.btnIIS.Name = "btnIIS";
      this.btnIIS.Size = new Size(93, 59);
      this.btnIIS.TabIndex = 0;
      this.btnIIS.UseVisualStyleBackColor = false;
      this.btnIIS.Click += new EventHandler(this.btnIIS_Click);
      this.btnFile.BackColor = Color.White;
      this.btnFile.BackgroundImageLayout = ImageLayout.Center;
      this.btnFile.Dock = DockStyle.Top;
      this.btnFile.Location = new Point(0, 0);
      this.btnFile.Name = "btnFile";
      this.btnFile.Size = new Size(93, 57);
      this.btnFile.TabIndex = 1;
      this.btnFile.UseVisualStyleBackColor = false;
      this.btnFile.Click += new EventHandler(this.btnFile_Click);
      this.pnlRightBottom.Controls.Add((Control) this.chkBoxSSL);
      this.pnlRightBottom.Dock = DockStyle.Fill;
      this.pnlRightBottom.Location = new Point(0, 103);
      this.pnlRightBottom.Name = "pnlRightBottom";
      this.pnlRightBottom.Size = new Size(520, 272);
      this.pnlRightBottom.TabIndex = 2;
      this.pnlRightMiddle.Controls.Add((Control) this.txtBoxURL);
      this.pnlRightMiddle.Dock = DockStyle.Top;
      this.pnlRightMiddle.Location = new Point(0, 84);
      this.pnlRightMiddle.Name = "pnlRightMiddle";
      this.pnlRightMiddle.Size = new Size(520, 19);
      this.pnlRightMiddle.TabIndex = 3;
      this.chkBoxSSL.AutoSize = true;
      this.chkBoxSSL.Location = new Point(4, 6);
      this.chkBoxSSL.Name = "chkBoxSSL";
      this.chkBoxSSL.Size = new Size(156, 17);
      this.chkBoxSSL.TabIndex = 0;
      this.chkBoxSSL.Text = "User Secure Sockets Layer";
      this.chkBoxSSL.UseVisualStyleBackColor = true;
      this.chkBoxSSL.CheckedChanged += new EventHandler(this.chkBoxSSL_CheckedChanged);
      this.pnlRightTop.Controls.Add((Control) this.lblPath);
      this.pnlRightTop.Dock = DockStyle.Top;
      this.pnlRightTop.Location = new Point(0, 59);
      this.pnlRightTop.Name = "pnlRightTop";
      this.pnlRightTop.Size = new Size(520, 25);
      this.pnlRightTop.TabIndex = 4;
      this.txtBoxURL.Anchor = AnchorStyles.Right;
      this.txtBoxURL.Location = new Point(3, -1);
      this.txtBoxURL.Name = "txtBoxURL";
      this.txtBoxURL.Size = new Size(504, 20);
      this.txtBoxURL.TabIndex = 1;
      this.txtBoxURL.TextChanged += new EventHandler(this.txtBoxURL_TextChanged);
      this.lblPath.Location = new Point(2, 6);
      this.lblPath.Name = "lblPath";
      this.lblPath.Size = new Size(109, 16);
      this.lblPath.TabIndex = 0;
      this.lblPath.Text = "Web site location:";
      this.lblSubTitle.Dock = DockStyle.Top;
      this.lblSubTitle.Location = new Point(0, 34);
      this.lblSubTitle.Name = "lblSubTitle";
      this.lblSubTitle.Size = new Size(520, 25);
      this.lblSubTitle.TabIndex = 1;
      this.lblSubTitle.Text = "Subtitle";
      this.lblTitle.Dock = DockStyle.Top;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(0, 0);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Padding = new Padding(0, 0, 0, 2);
      this.lblTitle.Size = new Size(520, 34);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Title";
      this.lblTitle.TextAlign = ContentAlignment.MiddleLeft;
      this.btnOpen.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOpen.DialogResult = DialogResult.OK;
      this.btnOpen.Location = new Point(455, 381);
      this.btnOpen.Name = "btnOpen";
      this.btnOpen.Size = new Size(75, 23);
      this.btnOpen.TabIndex = 1;
      this.btnOpen.Text = "Open";
      this.btnOpen.UseVisualStyleBackColor = true;
      this.btnOpen.Click += new EventHandler(this.btnOpen_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(537, 381);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AcceptButton = (IButtonControl) this.btnOpen;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(623, 413);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOpen);
      this.Controls.Add((Control) this.splitContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InstallationUrlDialog);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.panelLeft.ResumeLayout(false);
      this.pnlRightBottom.ResumeLayout(false);
      this.pnlRightBottom.PerformLayout();
      this.pnlRightMiddle.ResumeLayout(false);
      this.pnlRightMiddle.PerformLayout();
      this.pnlRightTop.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
