// Decompiled with JetBrains decompiler
// Type: RestoreAppLauncher.AppExperienceForm
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using EllieMae.Encompass.AsmResolver.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace RestoreAppLauncher
{
  public class AppExperienceForm : Form
  {
    private IContainer components;
    private Label lblMsg;
    private Button btnOK;
    private LinkLabel linkLabelArticle;

    public AppExperienceForm(string filename)
    {
      this.InitializeComponent();
      this.Text = Consts.EncompassSmartClient;
      this.lblMsg.Text = "Encompass SmartClient needs to update " + filename + ". If the Windows service 'Application Experience' (AeLookupSvc) is disabled, this may take a couple of minutes.\r\n\r\nIt is recommended that you set the startup type of the 'Application Experience' service to 'Manual' or 'Automatic'. Please refer to the following article for further details.";
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();

    private void linkLabelArticle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      this.linkLabelArticle.LinkVisited = true;
      Process.Start(this.linkLabelArticle.Text);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AppExperienceForm));
      this.lblMsg = new Label();
      this.btnOK = new Button();
      this.linkLabelArticle = new LinkLabel();
      this.SuspendLayout();
      this.lblMsg.Location = new Point(12, 12);
      this.lblMsg.Name = "lblMsg";
      this.lblMsg.Size = new Size(458, 70);
      this.lblMsg.TabIndex = 2;
      this.lblMsg.Text = "<msg>";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.Cancel;
      this.btnOK.Location = new Point(395, 107);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.linkLabelArticle.AutoSize = true;
      this.linkLabelArticle.LinkVisited = true;
      this.linkLabelArticle.Location = new Point(12, 88);
      this.linkLabelArticle.Name = "linkLabelArticle";
      this.linkLabelArticle.Size = new Size(254, 13);
      this.linkLabelArticle.TabIndex = 1;
      this.linkLabelArticle.TabStop = true;
      this.linkLabelArticle.Text = "https://support.microsoft.com/en-us/help/2503886/";
      this.linkLabelArticle.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelArticle_LinkClicked);
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnOK;
      this.ClientSize = new Size(482, 137);
      this.Controls.Add((Control) this.linkLabelArticle);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblMsg);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (AppExperienceForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass SmartClient";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
