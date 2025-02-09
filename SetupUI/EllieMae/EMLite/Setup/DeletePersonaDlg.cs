// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DeletePersonaDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DeletePersonaDlg : Form
  {
    private Button btnOK;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private ListView listViewUser;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private ColumnHeader columnHeader3;
    private Label lblHeader;
    private Persona personaToDelete;

    public DeletePersonaDlg(Persona personaToDelete, UserInfo[] usersToList)
    {
      this.InitializeComponent();
      this.personaToDelete = personaToDelete;
      this.lblHeader.Text = this.lblHeader.Text.Replace("LO", personaToDelete.Name);
      this.init(usersToList);
    }

    private void init(UserInfo[] usersToList)
    {
      this.listViewUser.Items.Clear();
      ListViewItem[] items = new ListViewItem[usersToList.Length];
      for (int index = 0; index < usersToList.Length; ++index)
        items[index] = new ListViewItem(new string[3]
        {
          usersToList[index].Userid,
          usersToList[index].FirstName,
          usersToList[index].LastName
        });
      this.listViewUser.Items.AddRange(items);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.listViewUser = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.columnHeader3 = new ColumnHeader();
      this.lblHeader = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.listViewUser.Columns.AddRange(new ColumnHeader[3]
      {
        this.columnHeader1,
        this.columnHeader2,
        this.columnHeader3
      });
      this.listViewUser.GridLines = true;
      this.listViewUser.Location = new Point(12, 64);
      this.listViewUser.Name = "listViewUser";
      this.listViewUser.Size = new Size(384, 164);
      this.listViewUser.TabIndex = 0;
      this.listViewUser.View = View.Details;
      this.columnHeader1.Text = "User ID";
      this.columnHeader1.Width = 100;
      this.columnHeader2.Text = "First Name";
      this.columnHeader2.Width = 140;
      this.columnHeader3.Text = "Last Name";
      this.columnHeader3.Width = 140;
      this.lblHeader.Location = new Point(12, 8);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(384, 52);
      this.lblHeader.TabIndex = 1;
      this.lblHeader.Text = "The following list of users may be affected if you delete the LO Persona. All users must be assigned one persona in order to login.";
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(252, 236);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(64, 24);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(324, 236);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(64, 24);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(410, 273);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.lblHeader);
      this.Controls.Add((Control) this.listViewUser);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (DeletePersonaDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Delete Persona";
      this.ResumeLayout(false);
    }
  }
}
