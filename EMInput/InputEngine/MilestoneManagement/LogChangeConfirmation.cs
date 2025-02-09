// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MilestoneManagement.LogChangeConfirmation
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.MilestoneManagement
{
  public class LogChangeConfirmation : Form
  {
    private Dictionary<UserInfo, List<string>> dontSendEmail = new Dictionary<UserInfo, List<string>>();
    private Dictionary<UserInfo, List<string>> sendEmail = new Dictionary<UserInfo, List<string>>();
    private Dictionary<UserInfo, List<string>> finalEmailList = new Dictionary<UserInfo, List<string>>();
    private List<LogRecordBase> logRecords = new List<LogRecordBase>();
    private IContainer components;
    private PictureBox pictureBox1;
    private Label label1;
    private Label label2;
    private Label label3;
    private Button btnYes;
    private Button btnNo;
    private GridView gridViewUsers;
    private Label label4;
    private GridView gvMilestones;
    private Label label5;
    private EMHelpLink emHelpLink1;

    public LogChangeConfirmation(
      Dictionary<UserInfo, List<string>> sendEmail,
      Dictionary<UserInfo, List<string>> dontSendEmail,
      List<LogRecordBase> logRecords,
      bool emailSend)
    {
      this.InitializeComponent();
      this.dontSendEmail = dontSendEmail;
      this.sendEmail = sendEmail;
      this.logRecords = logRecords;
      bool applicationRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.LoanTab_AllowEmailChange);
      this.populateData();
      if (!emailSend || !applicationRight)
      {
        this.gridViewUsers.Columns[1].CheckBoxes = false;
        this.label4.Visible = false;
      }
      if (this.gridViewUsers.Items.Count == 0)
      {
        this.ClientSize = new Size(556, this.gvMilestones.Location.Y + this.gvMilestones.Size.Height + 45);
        this.label3.Visible = this.label4.Visible = false;
        this.gridViewUsers.Visible = false;
      }
      else
        this.ClientSize = new Size(556, this.gridViewUsers.Location.Y + this.gridViewUsers.Size.Height + 50);
    }

    private void populateData()
    {
      foreach (MilestoneLog logRecord in this.logRecords)
        this.gvMilestones.Items.Add(new GVItem(logRecord.Stage)
        {
          ForeColor = Color.Blue
        });
      foreach (KeyValuePair<UserInfo, List<string>> user in this.sendEmail)
      {
        if (!user.Key.IsAdministrator())
        {
          GVItem gvItem = new GVItem()
          {
            SubItems = {
              [0] = {
                Text = this.getText(user)
              }
            },
            Tag = (object) user.Key
          };
          gvItem.SubItems[1].Checked = true;
          this.gridViewUsers.Items.Add(gvItem);
        }
      }
      foreach (KeyValuePair<UserInfo, List<string>> user in this.dontSendEmail)
      {
        GVItem gvItem = new GVItem();
        if (!user.Key.IsAdministrator())
        {
          gvItem.SubItems[0].Text = this.getText(user);
          gvItem.Tag = (object) user.Key;
          this.gridViewUsers.Items.Add(gvItem);
        }
      }
      if (this.gvMilestones.Items.Count != 0)
      {
        this.gvMilestones.Size = new Size(525, this.gvMilestones.Items.Count * 15 + 30);
        Size size = this.gvMilestones.Size;
        if (size.Height > 200)
          this.gvMilestones.Size = new Size(525, 200);
        Label label3 = this.label3;
        int y = this.gvMilestones.Location.Y;
        size = this.gvMilestones.Size;
        int height = size.Height;
        Point point = new Point(13, y + height + 10);
        label3.Location = point;
        this.gridViewUsers.Location = new Point(this.label3.Location.X, this.label3.Location.Y + 16);
      }
      else
      {
        this.label2.Visible = false;
        this.gvMilestones.Visible = false;
        this.label3.Location = new Point(8, 54);
        this.gridViewUsers.Location = new Point(13, 86);
      }
    }

    private string getText(KeyValuePair<UserInfo, List<string>> user)
    {
      string str1 = user.Key.FullName + " (";
      string str2;
      if (user.Value.Count > 1)
      {
        foreach (string str3 in user.Value)
          str1 = str1 + str3 + ", ";
        str2 = str1.Substring(0, str1.Length - 2);
      }
      else
        str2 = str1 + user.Value[0];
      return str2 + ")";
    }

    private void btnYes_Click(object sender, EventArgs e)
    {
      for (int nItemIndex = 0; nItemIndex < this.gridViewUsers.Items.Count; ++nItemIndex)
      {
        if (this.gridViewUsers.Items[nItemIndex].SubItems[1].Checked)
        {
          UserInfo tag = (UserInfo) this.gridViewUsers.Items[nItemIndex].Tag;
          if (!tag.IsAdministrator() && !this.finalEmailList.ContainsKey(tag))
          {
            if (this.sendEmail.ContainsKey(tag))
              this.finalEmailList.Add(tag, this.sendEmail[tag]);
            else
              this.finalEmailList.Add(tag, new List<string>()
              {
                ""
              });
          }
        }
      }
      this.DialogResult = DialogResult.OK;
      this.Hide();
    }

    private void btnNo_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Hide();
    }

    public Dictionary<UserInfo, List<string>> EmailList => this.finalEmailList;

    private void gvMilestones_MouseDoubleClick(object sender, MouseEventArgs e)
    {
      if (this.gvMilestones.SelectedItems.Count == 0)
        return;
      Session.Application.GetService<ILoanEditor>().OpenMilestoneLogReview((MilestoneLog) this.logRecords[this.gvMilestones.SelectedItems[0].Index]);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LogChangeConfirmation));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.btnYes = new Button();
      this.btnNo = new Button();
      this.pictureBox1 = new PictureBox();
      this.gridViewUsers = new GridView();
      this.label4 = new Label();
      this.gvMilestones = new GridView();
      this.label5 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f);
      this.label1.Location = new Point(60, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(375, 13);
      this.label1.TabIndex = 13;
      this.label1.Text = "The following changes will occur when you apply the new milestone template. ";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(8, 54);
      this.label2.Name = "label2";
      this.label2.Size = new Size(201, 13);
      this.label2.TabIndex = 14;
      this.label2.Text = "These milestones will be removed:";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(8, 289);
      this.label3.Name = "label3";
      this.label3.Size = new Size(501, 13);
      this.label3.TabIndex = 15;
      this.label3.Text = "These users will no longer be assigned to a milestone and may lose access to the loan:";
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.Location = new Point(382, 449);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 16;
      this.btnYes.Text = "OK";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.DialogResult = DialogResult.Cancel;
      this.btnNo.Location = new Point(463, 449);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 17;
      this.btnNo.Text = "Cancel";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnNo.Click += new EventHandler(this.btnNo_Click);
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(12, 12);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 12;
      this.pictureBox1.TabStop = false;
      this.gridViewUsers.BorderStyle = BorderStyle.FixedSingle;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Users and Groups";
      gvColumn1.Width = 223;
      gvColumn2.CheckBoxes = true;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Send Email Notification";
      gvColumn2.Width = 300;
      gvColumn2.SortMethod = GVSortMethod.Checkbox;
      this.gridViewUsers.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewUsers.Location = new Point(11, 305);
      this.gridViewUsers.Name = "gridViewUsers";
      this.gridViewUsers.Size = new Size(525, 130);
      this.gridViewUsers.SortOption = GVSortOption.Auto;
      this.gridViewUsers.TabIndex = 20;
      this.label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 405);
      this.label4.Name = "label4";
      this.label4.Size = new Size(321, 13);
      this.label4.TabIndex = 21;
      this.label4.Text = "Note: Select the check box to send a notification email to the user.";
      this.label4.Visible = false;
      this.gvMilestones.BorderStyle = BorderStyle.FixedSingle;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column1";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Milestones";
      gvColumn3.Width = 523;
      gvColumn3.SortMethod = GVSortMethod.Text;
      this.gvMilestones.Columns.AddRange(new GVColumn[1]
      {
        gvColumn3
      });
      this.gvMilestones.Location = new Point(12, 70);
      this.gvMilestones.Name = "gvMilestones";
      this.gvMilestones.Size = new Size(525, 200);
      this.gvMilestones.SortOption = GVSortOption.Auto;
      this.gvMilestones.TabIndex = 23;
      this.gvMilestones.MouseDoubleClick += new MouseEventHandler(this.gvMilestones_MouseDoubleClick);
      this.label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(12, 429);
      this.label5.Name = "label5";
      this.label5.Size = new Size(205, 13);
      this.label5.TabIndex = 22;
      this.label5.Text = "Are you sure you want to proceed?";
      this.label5.Visible = false;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "LogListChangeConfirmation";
      this.emHelpLink1.Location = new Point(11, 456);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 24;
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnNo;
      this.ClientSize = new Size(550, 484);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.gvMilestones);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.gridViewUsers);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pictureBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LogChangeConfirmation);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Milestone List Change Confirmation";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
