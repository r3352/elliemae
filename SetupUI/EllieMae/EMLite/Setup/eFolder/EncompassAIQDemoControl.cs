// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.EncompassAIQDemoControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class EncompassAIQDemoControl : SettingsUserControl
  {
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private LinkLabel Lbl_Link_DemoPage;

    public EncompassAIQDemoControl(SetUpContainer setupContainer, Sessions.Session session)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
    }

    private void ShowSupportPage()
    {
      string str = "https://partnerportal.elliemae.com/s/partner-profile/aH01E000000018mSAA/1067";
      try
      {
        Process.Start(new ProcessStartInfo()
        {
          FileName = str,
          UseShellExecute = true
        });
      }
      catch (Win32Exception ex)
      {
        if (ex.ErrorCode != -2147467259)
          return;
        int num = (int) MessageBox.Show(ex.Message);
      }
      catch (Exception ex)
      {
        Tracing.Log(Tracing.SwEFolder, TraceLevel.Error, nameof (EncompassAIQDemoControl), string.Format("Error in Launching Data & Document Automation and Mortgage Analyzers Demo URL. Exception: {0}", (object) ex.Message));
      }
    }

    private void Lbl_Link_DemoPage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.ShowSupportPage();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.Lbl_Link_DemoPage = new LinkLabel();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(21, 20);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(810, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Check out Data && Document Automation and Mortgage Analyzers on ICE Mortgage Technology Resource Center";
      this.Lbl_Link_DemoPage.AutoSize = true;
      this.Lbl_Link_DemoPage.Location = new Point(839, 20);
      this.Lbl_Link_DemoPage.Margin = new Padding(4, 0, 4, 0);
      this.Lbl_Link_DemoPage.Name = "Lbl_Link_DemoPage";
      this.Lbl_Link_DemoPage.Size = new Size(41, 20);
      this.Lbl_Link_DemoPage.TabIndex = 1;
      this.Lbl_Link_DemoPage.TabStop = true;
      this.Lbl_Link_DemoPage.Text = "here";
      this.Lbl_Link_DemoPage.LinkClicked += new LinkLabelLinkClickedEventHandler(this.Lbl_Link_DemoPage_LinkClicked);
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.Lbl_Link_DemoPage);
      this.Controls.Add((Control) this.label1);
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (EncompassAIQDemoControl);
      this.Size = new Size(901, 480);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
