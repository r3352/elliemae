// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LicenseDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
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
  internal class LicenseDialog : Form
  {
    private IContainer components;
    private Button okBtn;
    private RichTextBox licenseBox;

    public LicenseDialog()
    {
      this.InitializeComponent();
      try
      {
        this.licenseBox.LoadFile(AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.DocDirRelPath, "Help\\EncompassLicenseAgmt.rtf"), SystemSettings.LocalAppDir));
      }
      catch (Exception ex)
      {
      }
    }

    private void licenseBox_LinkClicked(object sender, LinkClickedEventArgs e)
    {
      SystemUtil.ShellExecute(e.LinkText);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.okBtn = new Button();
      this.licenseBox = new RichTextBox();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(484, 457);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 7;
      this.okBtn.Text = "&OK";
      this.licenseBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.licenseBox.BackColor = Color.WhiteSmoke;
      this.licenseBox.BorderStyle = BorderStyle.None;
      this.licenseBox.BulletIndent = 1;
      this.licenseBox.Cursor = Cursors.Arrow;
      this.licenseBox.HideSelection = false;
      this.licenseBox.Location = new Point(10, 10);
      this.licenseBox.Name = "licenseBox";
      this.licenseBox.ReadOnly = true;
      this.licenseBox.Size = new Size(548, 438);
      this.licenseBox.TabIndex = 8;
      this.licenseBox.TabStop = false;
      this.licenseBox.Text = "";
      this.licenseBox.LinkClicked += new LinkClickedEventHandler(this.licenseBox_LinkClicked);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(568, 488);
      this.Controls.Add((Control) this.licenseBox);
      this.Controls.Add((Control) this.okBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MinimizeBox = false;
      this.Name = nameof (LicenseDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "License Agreement";
      this.ResumeLayout(false);
    }
  }
}
