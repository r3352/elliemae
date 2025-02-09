// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.SelfHostedMessage
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  public class SelfHostedMessage : Form
  {
    private IContainer components;
    private Button OKbutton;
    private Label label2;
    private Panel panel1;
    private LinkLabel linkLabel2;

    public SelfHostedMessage() => this.InitializeComponent();

    private void OKbutton_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.linkLabel2.LinkVisited = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SelfHostedMessage));
      this.OKbutton = new Button();
      this.label2 = new Label();
      this.panel1 = new Panel();
      this.linkLabel2 = new LinkLabel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.OKbutton.Location = new Point(178, 170);
      this.OKbutton.Name = "OKbutton";
      this.OKbutton.Size = new Size(75, 23);
      this.OKbutton.TabIndex = 4;
      this.OKbutton.Text = "OK";
      this.OKbutton.UseVisualStyleBackColor = true;
      this.OKbutton.Click += new EventHandler(this.OKbutton_Click);
      this.label2.Location = new Point(15, 12);
      this.label2.Name = "label2";
      this.label2.Size = new Size(238, 133);
      this.label2.TabIndex = 17;
      this.label2.Text = componentResourceManager.GetString("label2.Text");
      this.panel1.BackColor = SystemColors.InactiveBorder;
      this.panel1.Controls.Add((Control) this.linkLabel2);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(277, 154);
      this.panel1.TabIndex = 18;
      this.linkLabel2.AutoSize = true;
      this.linkLabel2.Location = new Point(152, 90);
      this.linkLabel2.Name = "linkLabel2";
      this.linkLabel2.Size = new Size(28, 13);
      this.linkLabel2.TabIndex = 18;
      this.linkLabel2.TabStop = true;
      this.linkLabel2.Text = "here";
      this.linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(277, 207);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.OKbutton);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelfHostedMessage);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Self-hosted Encompass no longer supported";
      this.TopMost = true;
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
