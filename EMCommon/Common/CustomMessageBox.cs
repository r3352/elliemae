// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.CustomMessageBox
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common
{
  internal class CustomMessageBox : Form
  {
    internal string Result = string.Empty;
    private const int CP_NOCLOSE_BUTTON = 512;
    private IContainer components;
    internal PictureBox picImage;
    internal Label lblText;
    internal FlowLayoutPanel panelButtons;

    internal CustomMessageBox(MessageBoxIcon messageBoxIcon, string text, string[] Buttons)
    {
      this.InitializeComponent();
      this.Text = "Encompass";
      this.picImage.Image = (Image) this.GetBitmapFromMessageBoxIcon(messageBoxIcon);
      this.lblText.Text = text;
      if (Buttons == null || Buttons.Length == 0)
        return;
      for (int index = 0; index < Buttons.Length; ++index)
      {
        Button button = new Button();
        button.Name = "btn" + index.ToString();
        button.AutoSize = true;
        button.AutoSizeMode = AutoSizeMode.GrowOnly;
        button.Text = Buttons[index];
        button.Click += new EventHandler(this.btnOK_Click);
        this.panelButtons.Controls.Add((Control) button);
      }
    }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        createParams.ClassStyle |= 512;
        return createParams;
      }
    }

    private Bitmap GetBitmapFromMessageBoxIcon(MessageBoxIcon messageBoxIcon)
    {
      Icon icon = SystemIcons.Information;
      switch (messageBoxIcon)
      {
        case MessageBoxIcon.Hand:
          icon = SystemIcons.Error;
          break;
        case MessageBoxIcon.Asterisk:
          icon = SystemIcons.Asterisk;
          break;
        default:
          if (messageBoxIcon == MessageBoxIcon.Exclamation || messageBoxIcon == MessageBoxIcon.Exclamation)
          {
            icon = SystemIcons.Exclamation;
            break;
          }
          if (messageBoxIcon == MessageBoxIcon.Question)
          {
            icon = SystemIcons.Question;
            break;
          }
          if (messageBoxIcon == MessageBoxIcon.Hand || messageBoxIcon == MessageBoxIcon.Hand || messageBoxIcon == MessageBoxIcon.Asterisk || messageBoxIcon == MessageBoxIcon.None)
          {
            icon = SystemIcons.Information;
            break;
          }
          break;
      }
      return Bitmap.FromHicon(icon.Handle);
    }

    internal void btnOK_Click(object sender, EventArgs e)
    {
      this.Result = ((Control) sender).Text;
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.picImage = new PictureBox();
      this.lblText = new Label();
      this.panelButtons = new FlowLayoutPanel();
      ((ISupportInitialize) this.picImage).BeginInit();
      this.SuspendLayout();
      this.picImage.ErrorImage = (Image) null;
      this.picImage.InitialImage = (Image) null;
      this.picImage.Location = new Point(15, 15);
      this.picImage.Name = "picImage";
      this.picImage.Size = new Size(32, 32);
      this.picImage.TabIndex = 0;
      this.picImage.TabStop = false;
      this.lblText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblText.AutoSize = true;
      this.lblText.Location = new Point(56, 17);
      this.lblText.MaximumSize = new Size(600, 0);
      this.lblText.Name = "lblText";
      this.lblText.Size = new Size(314, 13);
      this.lblText.TabIndex = 0;
      this.lblText.Text = "No services can be ordered until the Service Setup is established";
      this.panelButtons.AutoSize = true;
      this.panelButtons.Location = new Point(220, 59);
      this.panelButtons.Name = "panelButtons";
      this.panelButtons.Size = new Size(208, 23);
      this.panelButtons.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(457, 96);
      this.Controls.Add((Control) this.panelButtons);
      this.Controls.Add((Control) this.picImage);
      this.Controls.Add((Control) this.lblText);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CustomMessageBox);
      this.Padding = new Padding(15);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Title";
      ((ISupportInitialize) this.picImage).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
