// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Wizard.WizardItemWithHeader
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Wizard
{
  public class WizardItemWithHeader : WizardItem
  {
    public EventHandler SubheaderLink_Clicked;
    private GroupBox groupBox1;
    private Panel panel1;
    private Label lblHeader;
    private Label lblSubheader;
    private LinkLabel lblSubheaderLink;
    private IContainer components;

    public WizardItemWithHeader()
      : this((WizardItem) null)
    {
    }

    public WizardItemWithHeader(WizardItem prevItem)
      : base(prevItem)
    {
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
      this.groupBox1 = new GroupBox();
      this.panel1 = new Panel();
      this.lblSubheader = new Label();
      this.lblHeader = new Label();
      this.lblSubheaderLink = new LinkLabel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Dock = DockStyle.Top;
      this.groupBox1.Location = new Point(0, 58);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(496, 2);
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.panel1.Controls.Add((Control) this.lblSubheaderLink);
      this.panel1.Controls.Add((Control) this.lblSubheader);
      this.panel1.Controls.Add((Control) this.lblHeader);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(496, 58);
      this.panel1.TabIndex = 9;
      this.lblSubheader.Location = new Point(38, 33);
      this.lblSubheader.Name = "lblSubheader";
      this.lblSubheader.Size = new Size(418, 14);
      this.lblSubheader.TabIndex = 1;
      this.lblSubheader.Text = "(Subheader)";
      this.lblHeader.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblHeader.Location = new Point(24, 12);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(432, 14);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "(Header)";
      this.lblSubheaderLink.AutoSize = true;
      this.lblSubheaderLink.Location = new Point(313, 33);
      this.lblSubheaderLink.Name = "lblSubheaderLink";
      this.lblSubheaderLink.Size = new Size(88, 13);
      this.lblSubheaderLink.TabIndex = 2;
      this.lblSubheaderLink.TabStop = true;
      this.lblSubheaderLink.Text = "(Subheader Link)";
      this.lblSubheaderLink.Visible = false;
      this.lblSubheaderLink.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lblSubheaderLink_LinkClicked);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.panel1);
      this.Name = nameof (WizardItemWithHeader);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    public string Header
    {
      get => this.lblHeader.Text;
      set => this.lblHeader.Text = value;
    }

    public string Subheader
    {
      get => this.lblSubheader.Text;
      set => this.lblSubheader.Text = value;
    }

    public int SubheaderLocationX => this.lblSubheader.Left;

    public int SubheaderLocationY => this.lblSubheader.Top;

    public bool SubheaderAutoSize
    {
      set => this.lblSubheader.AutoSize = value;
    }

    public int SubheaderWidth => this.lblSubheader.Size.Width;

    private void lblSubheaderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      if (this.SubheaderLink_Clicked == null)
        return;
      this.SubheaderLink_Clicked(sender, new EventArgs());
    }

    public void SetSubheader(string text, Point p)
    {
      this.lblSubheader.Text = text;
      this.lblSubheader.Location = p;
    }

    public void SetSubheaderLink(string text, Point p)
    {
      this.lblSubheaderLink.Text = text;
      this.lblSubheaderLink.Location = p;
      this.lblSubheaderLink.Visible = true;
    }

    public bool SubheaderLinkVisible
    {
      get => this.lblSubheaderLink.Visible;
      set => this.lblSubheaderLink.Visible = value;
    }
  }
}
