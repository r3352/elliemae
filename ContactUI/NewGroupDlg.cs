// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.NewGroupDlg
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class NewGroupDlg : Form
  {
    private Label label1;
    private TextBox txtBoxGroupName;
    private Button btnOK;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private ContactType contactType;
    private ContactGroupInfo groupInfo;

    public ContactGroupInfo newGroupInfo => this.groupInfo;

    public NewGroupDlg(ContactType contactType)
    {
      this.InitializeComponent();
      this.contactType = contactType;
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
      this.txtBoxGroupName = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(8, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(260, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Enter the name of the new group:";
      this.txtBoxGroupName.Location = new Point(8, 32);
      this.txtBoxGroupName.MaxLength = 23;
      this.txtBoxGroupName.Name = "txtBoxGroupName";
      this.txtBoxGroupName.Size = new Size(272, 20);
      this.txtBoxGroupName.TabIndex = 1;
      this.txtBoxGroupName.Text = "";
      this.btnOK.Location = new Point(124, 68);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(72, 22);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(204, 68);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(72, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(290, 103);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtBoxGroupName);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (NewGroupDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "New Contact Group ";
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      string groupName = this.txtBoxGroupName.Text.Trim();
      if (groupName == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Group name cannot be blank. Please enter a valid group name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtBoxGroupName.Focus();
      }
      else
      {
        ContactGroupInfo[] contactGroupsForUser = Session.ContactGroupManager.GetContactGroupsForUser(new ContactGroupCollectionCriteria(Session.UserID, this.contactType, new ContactGroupType[1]));
        if (contactGroupsForUser != null)
        {
          ArrayList arrayList = new ArrayList(contactGroupsForUser.Length);
          for (int index = 0; index < contactGroupsForUser.Length; ++index)
            arrayList.Add((object) contactGroupsForUser[index].GroupName);
          if (arrayList.Contains((object) groupName))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The group name you entered already exists. Please enter a different name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            this.txtBoxGroupName.Focus();
            return;
          }
        }
        this.groupInfo = new ContactGroupInfo(0, Session.UserID, this.contactType, ContactGroupType.ContactGroup, groupName, string.Empty, DateTime.Now, new int[0]);
        this.groupInfo = Session.ContactGroupManager.SaveContactGroup(this.groupInfo);
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }
  }
}
