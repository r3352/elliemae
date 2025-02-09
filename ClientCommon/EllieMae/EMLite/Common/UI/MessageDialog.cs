// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.MessageDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class MessageDialog : Form
  {
    private Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
    private Image _image;
    private bool _additionalInfoExpanded;
    private int _padding = 10;
    private IContainer components;
    private PictureBox iconBox;
    private Label lblMessage;
    private Button btnMoreInfo;
    private TextBox tbxMoreInfo;
    private FlowLayoutPanel flpButtons;
    private Button btnYes;
    private Button btnNo;
    private Button btnCancel;
    private FlowLayoutPanel flpMoreInfo;
    private Button btnOK;
    private Label lblAdditionalInfoHeader;

    private MessageDialog()
    {
      this.InitializeComponent();
      this._buttons.Add("Yes", this.btnYes);
      this._buttons.Add("No", this.btnNo);
      this._buttons.Add("Cancel", this.btnCancel);
      this._buttons.Add("MoreInfo", this.btnMoreInfo);
      this._buttons.Add("OK", this.btnOK);
      this._image = (Image) SystemIcons.Application.ToBitmap();
      this.iconBox.Paint += new PaintEventHandler(this.iconBox_Paint);
    }

    private void iconBox_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.DrawImage(this._image, this.iconBox.DisplayRectangle);
    }

    public void SetCaption(string caption) => this.Text = caption;

    public void SetMessage(string message) => this.lblMessage.Text = message;

    public void SetButtons(MessageDialogButtons buttons, MessageBoxDefaultButton defaultButton)
    {
      this.flpButtons.Controls.Clear();
      this.flpMoreInfo.Controls.Clear();
      Button button1 = (Button) null;
      Button button2 = (Button) null;
      Button button3 = (Button) null;
      switch (buttons)
      {
        case MessageDialogButtons.YesNoCancelMoreInfo:
          this.flpMoreInfo.Controls.Add((Control) this._buttons["MoreInfo"]);
          this.flpButtons.Controls.Add((Control) this._buttons["Yes"]);
          this.flpButtons.Controls.Add((Control) this._buttons["No"]);
          this.flpButtons.Controls.Add((Control) this._buttons["Cancel"]);
          button1 = this._buttons["Yes"];
          button2 = this._buttons["No"];
          button3 = this._buttons["Cancel"];
          break;
        case MessageDialogButtons.YesNoMoreInfo:
          this.flpMoreInfo.Controls.Add((Control) this._buttons["MoreInfo"]);
          this.flpButtons.Controls.Add((Control) this._buttons["Yes"]);
          this.flpButtons.Controls.Add((Control) this._buttons["No"]);
          button1 = this._buttons["Yes"];
          button2 = this._buttons["No"];
          break;
        case MessageDialogButtons.OK:
          this.flpButtons.Controls.Add((Control) this._buttons["OK"]);
          button1 = this._buttons["OK"];
          break;
        case MessageDialogButtons.OKMoreInfo:
          this.flpMoreInfo.Controls.Add((Control) this._buttons["MoreInfo"]);
          this.flpButtons.Controls.Add((Control) this._buttons["OK"]);
          button1 = this._buttons["OK"];
          break;
      }
      switch (defaultButton)
      {
        case MessageBoxDefaultButton.Button1:
          this.AcceptButton = (IButtonControl) button1;
          break;
        case MessageBoxDefaultButton.Button2:
          this.AcceptButton = (IButtonControl) button2;
          break;
        case MessageBoxDefaultButton.Button3:
          this.AcceptButton = (IButtonControl) button3;
          break;
      }
    }

    public void SetIcon(MessageBoxIcon icon)
    {
      switch (icon)
      {
        case MessageBoxIcon.Hand:
          this._image = (Image) SystemIcons.Error.ToBitmap();
          break;
        case MessageBoxIcon.Question:
          this._image = (Image) SystemIcons.Question.ToBitmap();
          break;
        case MessageBoxIcon.Exclamation:
          this._image = (Image) SystemIcons.Exclamation.ToBitmap();
          break;
        case MessageBoxIcon.Asterisk:
          this._image = (Image) SystemIcons.Asterisk.ToBitmap();
          break;
      }
      this.iconBox.Size = this._image.Size;
    }

    public void SetAdditionalInfo(string additionalInfoHeader, string additionalInfo)
    {
      this.lblAdditionalInfoHeader.Text = additionalInfoHeader;
      if (string.IsNullOrEmpty(additionalInfo))
        return;
      this.tbxMoreInfo.Text = additionalInfo;
    }

    public static DialogResult Show(
      IWin32Window parent,
      string message,
      string caption,
      string additionalInfoHeader,
      string additionalInfo,
      MessageDialogButtons buttons,
      MessageBoxIcon icon,
      MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2)
    {
      using (MessageDialog messageDialog = new MessageDialog())
      {
        messageDialog.SetMessage(message);
        messageDialog.SetCaption(caption);
        messageDialog.SetAdditionalInfo(additionalInfoHeader, additionalInfo);
        messageDialog.SetButtons(buttons, defaultButton);
        messageDialog.SetIcon(icon);
        return parent == null ? messageDialog.ShowDialog() : messageDialog.ShowDialog(parent);
      }
    }

    private void btnMoreInfo_Click(object sender, EventArgs e)
    {
      if (!this._additionalInfoExpanded)
      {
        this.Height = this.Height + this.tbxMoreInfo.Height + this._padding;
        this._additionalInfoExpanded = true;
      }
      else
      {
        this.Height = this.Height - this.tbxMoreInfo.Height - this._padding;
        this._additionalInfoExpanded = false;
      }
      this.ToggleMoreInfoText();
    }

    private void ToggleMoreInfoText()
    {
      if (this._additionalInfoExpanded)
        this.btnMoreInfo.Text = "Hide &Details <<";
      else
        this.btnMoreInfo.Text = "Show &Details >>";
    }

    private void btnYes_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Yes;
      this.Close();
    }

    private void btnNo_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.No;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void MessageDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      this._image.Dispose();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.iconBox = new PictureBox();
      this.lblMessage = new Label();
      this.btnMoreInfo = new Button();
      this.tbxMoreInfo = new TextBox();
      this.flpButtons = new FlowLayoutPanel();
      this.btnYes = new Button();
      this.btnNo = new Button();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.flpMoreInfo = new FlowLayoutPanel();
      this.lblAdditionalInfoHeader = new Label();
      ((ISupportInitialize) this.iconBox).BeginInit();
      this.flpButtons.SuspendLayout();
      this.flpMoreInfo.SuspendLayout();
      this.SuspendLayout();
      this.iconBox.Location = new Point(18, 21);
      this.iconBox.Name = "iconBox";
      this.iconBox.Size = new Size(32, 32);
      this.iconBox.TabIndex = 0;
      this.iconBox.TabStop = false;
      this.lblMessage.AutoEllipsis = true;
      this.lblMessage.Location = new Point(68, 21);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(431, 46);
      this.lblMessage.TabIndex = 1;
      this.lblMessage.Text = "Message goes here";
      this.btnMoreInfo.Location = new Point(3, 3);
      this.btnMoreInfo.Name = "btnMoreInfo";
      this.btnMoreInfo.Size = new Size(106, 23);
      this.btnMoreInfo.TabIndex = 2;
      this.btnMoreInfo.Text = "Show &Details >>";
      this.btnMoreInfo.UseVisualStyleBackColor = true;
      this.btnMoreInfo.Click += new EventHandler(this.btnMoreInfo_Click);
      this.tbxMoreInfo.Location = new Point(15, 132);
      this.tbxMoreInfo.Multiline = true;
      this.tbxMoreInfo.Name = "tbxMoreInfo";
      this.tbxMoreInfo.ReadOnly = true;
      this.tbxMoreInfo.ScrollBars = ScrollBars.Vertical;
      this.tbxMoreInfo.Size = new Size(484, 108);
      this.tbxMoreInfo.TabIndex = 3;
      this.flpButtons.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flpButtons.AutoSize = true;
      this.flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flpButtons.Controls.Add((Control) this.btnYes);
      this.flpButtons.Controls.Add((Control) this.btnNo);
      this.flpButtons.Controls.Add((Control) this.btnOK);
      this.flpButtons.Controls.Add((Control) this.btnCancel);
      this.flpButtons.Location = new Point(175, 83);
      this.flpButtons.Name = "flpButtons";
      this.flpButtons.Size = new Size(324, 29);
      this.flpButtons.TabIndex = 4;
      this.btnYes.Location = new Point(3, 3);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 0;
      this.btnYes.Text = "Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.btnNo.Location = new Point(84, 3);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 1;
      this.btnNo.Text = "No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnNo.Click += new EventHandler(this.btnNo_Click);
      this.btnOK.Location = new Point(165, 3);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Location = new Point(246, 3);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.flpMoreInfo.Controls.Add((Control) this.btnMoreInfo);
      this.flpMoreInfo.Location = new Point(12, 83);
      this.flpMoreInfo.Name = "flpMoreInfo";
      this.flpMoreInfo.Size = new Size(109, 27);
      this.flpMoreInfo.TabIndex = 3;
      this.lblAdditionalInfoHeader.AutoSize = true;
      this.lblAdditionalInfoHeader.Location = new Point(15, 118);
      this.lblAdditionalInfoHeader.Name = "lblAdditionalInfoHeader";
      this.lblAdditionalInfoHeader.Size = new Size(0, 13);
      this.lblAdditionalInfoHeader.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(511, 118);
      this.Controls.Add((Control) this.lblAdditionalInfoHeader);
      this.Controls.Add((Control) this.flpButtons);
      this.Controls.Add((Control) this.tbxMoreInfo);
      this.Controls.Add((Control) this.flpMoreInfo);
      this.Controls.Add((Control) this.lblMessage);
      this.Controls.Add((Control) this.iconBox);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MessageDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Message Dialog";
      this.FormClosing += new FormClosingEventHandler(this.MessageDialog_FormClosing);
      ((ISupportInitialize) this.iconBox).EndInit();
      this.flpButtons.ResumeLayout(false);
      this.flpMoreInfo.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
