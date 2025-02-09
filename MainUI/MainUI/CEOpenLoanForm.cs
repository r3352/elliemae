// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.CEOpenLoanForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class CEOpenLoanForm : Form
  {
    private IContainer components;
    private Label lblMsg;
    private Button btnNo;
    private Button btnYes;
    private LinkLabel lnkLblLearnMore;
    private PictureBox pictureBox1;

    public CEOpenLoanForm(UserShortInfoList users)
    {
      this.InitializeComponent();
      if (users == null || users.Count == 0)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (UserShortInfo userShortInfo in users.UserShortInfos)
      {
        if (flag)
          flag = false;
        else
          stringBuilder.Append(", ");
        stringBuilder.Append(userShortInfo.FirstName + " " + userShortInfo.LastName);
        string[] uniqueLoanroles = userShortInfo.UniqueLoanroles;
        if (uniqueLoanroles != null && uniqueLoanroles.Length != 0)
        {
          stringBuilder.Append(" (");
          string str = string.Join(",", uniqueLoanroles);
          stringBuilder.Append(str);
          stringBuilder.Append(")");
        }
      }
      string str1 = "user is";
      if (users.Count > 1)
        str1 = "users are";
      this.lblMsg.Text = "The following " + str1 + " currently editing this loan:\r\n\r\n" + stringBuilder.ToString() + "\r\n\r\nThe changes made by other users will be merged with your changes when you save the loan.\r\nDo you still want to open this loan?";
    }

    private void lnkLblLearnMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Session.Application.DisplayHelp("Multi-User Editing");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblMsg = new Label();
      this.btnNo = new Button();
      this.btnYes = new Button();
      this.lnkLblLearnMore = new LinkLabel();
      this.pictureBox1 = new PictureBox();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lblMsg.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblMsg.Location = new Point(12, 9);
      this.lblMsg.Name = "lblMsg";
      this.lblMsg.Size = new Size(467, 117);
      this.lblMsg.TabIndex = 0;
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Location = new Point(401, 129);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 2;
      this.btnNo.Text = "&No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.DialogResult = DialogResult.Yes;
      this.btnYes.Location = new Point(320, 129);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 1;
      this.btnYes.Text = "&Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.lnkLblLearnMore.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lnkLblLearnMore.AutoSize = true;
      this.lnkLblLearnMore.ImageAlign = ContentAlignment.MiddleLeft;
      this.lnkLblLearnMore.LinkBehavior = LinkBehavior.NeverUnderline;
      this.lnkLblLearnMore.LinkColor = Color.SteelBlue;
      this.lnkLblLearnMore.Location = new Point(34, 134);
      this.lnkLblLearnMore.Name = "lnkLblLearnMore";
      this.lnkLblLearnMore.Size = new Size(61, 13);
      this.lnkLblLearnMore.TabIndex = 3;
      this.lnkLblLearnMore.TabStop = true;
      this.lnkLblLearnMore.Text = "Learn More";
      this.lnkLblLearnMore.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkLblLearnMore_LinkClicked);
      this.pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pictureBox1.BackColor = Color.Transparent;
      this.pictureBox1.Image = (Image) Resources.help;
      this.pictureBox1.Location = new Point(15, 133);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(16, 16);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 4;
      this.pictureBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnNo;
      this.ClientSize = new Size(491, 161);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.lnkLblLearnMore);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.lblMsg);
      this.Name = nameof (CEOpenLoanForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
