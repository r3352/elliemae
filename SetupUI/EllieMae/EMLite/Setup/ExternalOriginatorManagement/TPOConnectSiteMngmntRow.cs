// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOConnectSiteMngmntRow
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOConnectSiteMngmntRow : UserControl
  {
    private WebBrowser browser = new WebBrowser();
    private int urlId;
    private string siteId;
    private string associatedUrl;
    private bool dirty;
    private IContainer components;
    private Label lblSiteID;
    private TextBox tboxSiteId;
    private Label lblAssociatedUrl;
    private TextBox tboxAssociatedUrl;
    private Button btnTest;

    public event EventHandler OnUrlChanged;

    public bool RowUpdated
    {
      get => this.dirty;
      set => this.dirty = value;
    }

    public string URL
    {
      get => this.associatedUrl;
      set
      {
        this.associatedUrl = value;
        this.tboxAssociatedUrl.Text = value;
      }
    }

    public TPOConnectSiteMngmntRow(int urlId, string siteId, string associatedUrl)
    {
      this.InitializeComponent();
      this.urlId = urlId;
      this.siteId = siteId;
      this.associatedUrl = associatedUrl;
      this.tboxSiteId.Text = siteId;
      this.tboxAssociatedUrl.Text = associatedUrl;
    }

    private void btnTest_Click(object sender, EventArgs e)
    {
      if (this.tboxAssociatedUrl.Text.Trim().Length <= 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Associated URL can not be empty", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int num2 = (int) new TPOConnectAdminUrlBrowser(this.tboxAssociatedUrl.Text, "TPO Admin URL Link").ShowDialog((IWin32Window) this);
      }
    }

    private void tboxAssociatedUrl_TextChanged(object sender, EventArgs e)
    {
      this.dirty = true;
      this.associatedUrl = this.tboxAssociatedUrl.Text;
      if (this.OnUrlChanged == null)
        return;
      this.OnUrlChanged((object) this, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblSiteID = new Label();
      this.tboxSiteId = new TextBox();
      this.lblAssociatedUrl = new Label();
      this.tboxAssociatedUrl = new TextBox();
      this.btnTest = new Button();
      this.SuspendLayout();
      this.lblSiteID.AutoSize = true;
      this.lblSiteID.Location = new Point(3, 14);
      this.lblSiteID.Name = "lblSiteID";
      this.lblSiteID.Size = new Size(39, 13);
      this.lblSiteID.TabIndex = 0;
      this.lblSiteID.Text = "Site ID";
      this.tboxSiteId.Enabled = false;
      this.tboxSiteId.Location = new Point(49, 10);
      this.tboxSiteId.Name = "tboxSiteId";
      this.tboxSiteId.Size = new Size(133, 20);
      this.tboxSiteId.TabIndex = 1;
      this.lblAssociatedUrl.AutoSize = true;
      this.lblAssociatedUrl.Location = new Point(229, 14);
      this.lblAssociatedUrl.Name = "lblAssociatedUrl";
      this.lblAssociatedUrl.Size = new Size(84, 13);
      this.lblAssociatedUrl.TabIndex = 2;
      this.lblAssociatedUrl.Text = "Associated URL";
      this.tboxAssociatedUrl.Location = new Point(319, 10);
      this.tboxAssociatedUrl.Name = "tboxAssociatedUrl";
      this.tboxAssociatedUrl.Size = new Size(407, 20);
      this.tboxAssociatedUrl.TabIndex = 3;
      this.tboxAssociatedUrl.TextChanged += new EventHandler(this.tboxAssociatedUrl_TextChanged);
      this.btnTest.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.btnTest.Location = new Point(759, 8);
      this.btnTest.Name = "btnTest";
      this.btnTest.Size = new Size(75, 23);
      this.btnTest.TabIndex = 4;
      this.btnTest.Text = "Test";
      this.btnTest.UseVisualStyleBackColor = true;
      this.btnTest.Click += new EventHandler(this.btnTest_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btnTest);
      this.Controls.Add((Control) this.tboxAssociatedUrl);
      this.Controls.Add((Control) this.lblAssociatedUrl);
      this.Controls.Add((Control) this.tboxSiteId);
      this.Controls.Add((Control) this.lblSiteID);
      this.Name = nameof (TPOConnectSiteMngmntRow);
      this.Size = new Size(846, 40);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
