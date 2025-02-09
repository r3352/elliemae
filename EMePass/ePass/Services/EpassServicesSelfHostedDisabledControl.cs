// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.Services.EpassServicesSelfHostedDisabledControl
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass.Services
{
  public class EpassServicesSelfHostedDisabledControl : UserControl
  {
    private IContainer components;
    private Label label2;
    private Label label1;
    private LinkLabel linkLabel1;

    public EpassServicesSelfHostedDisabledControl() => this.InitializeComponent();

    private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.linkLabel1.LinkVisited = true;
      Process.Start("https://resourcecenter.elliemae.com/resourcecenter/knowledgebase.aspx?t=shencompasstermination");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EpassServicesSelfHostedDisabledControl));
      this.label2 = new Label();
      this.label1 = new Label();
      this.linkLabel1 = new LinkLabel();
      this.SuspendLayout();
      this.label2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(15, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(203, 202);
      this.label2.TabIndex = 18;
      this.label2.Text = componentResourceManager.GetString("label2.Text");
      this.label1.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.Red;
      this.label1.Location = new Point(15, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(203, 43);
      this.label1.TabIndex = 19;
      this.label1.Text = "Self-hosted Encompass no longer supported";
      this.label1.TextAlign = ContentAlignment.TopCenter;
      this.linkLabel1.AutoSize = true;
      this.linkLabel1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.linkLabel1.Location = new Point(72, 137);
      this.linkLabel1.Name = "linkLabel1";
      this.linkLabel1.Size = new Size(32, 15);
      this.linkLabel1.TabIndex = 20;
      this.linkLabel1.TabStop = true;
      this.linkLabel1.Text = "here";
      this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked_1);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.linkLabel1);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label2);
      this.Name = nameof (EpassServicesSelfHostedDisabledControl);
      this.Size = new Size(243, 267);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
