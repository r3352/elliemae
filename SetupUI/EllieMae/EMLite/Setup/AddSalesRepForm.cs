// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddSalesRepForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddSalesRepForm : Form
  {
    private UserInfo[] allInternalUsers;
    private List<string> existingUserIDs;
    private List<UserInfo> selectedUsers = new List<UserInfo>();
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private GridView gridViewSales;

    public AddSalesRepForm(UserInfo[] allInternalUsers, List<string> existingUserIDs)
    {
      this.allInternalUsers = allInternalUsers;
      this.existingUserIDs = existingUserIDs;
      this.InitializeComponent();
      this.initForm();
      this.gridViewSales_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void initForm()
    {
      this.gridViewSales.BeginUpdate();
      for (int index = 0; index < this.allInternalUsers.Length; ++index)
      {
        if (!this.existingUserIDs.Contains(this.allInternalUsers[index].Userid))
          this.gridViewSales.Items.Add(new GVItem(this.allInternalUsers[index].FullName)
          {
            SubItems = {
              (object) this.allInternalUsers[index].Phone,
              (object) this.allInternalUsers[index].Email
            },
            Tag = (object) this.allInternalUsers[index]
          });
      }
      this.gridViewSales.EndUpdate();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      for (int index = 0; index < this.gridViewSales.SelectedItems.Count; ++index)
        this.selectedUsers.Add((UserInfo) this.gridViewSales.SelectedItems[index].Tag);
      this.selectedUsers.ToArray();
      this.DialogResult = DialogResult.OK;
    }

    private void gridViewSales_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.gridViewSales.SelectedItems.Count > 0;
    }

    public UserInfo[] SelectedUsers => this.selectedUsers.ToArray();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.gridViewSales = new GridView();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(302, 291);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(383, 291);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.gridViewSales.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnName";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnPhone";
      gvColumn2.Text = "Phone #";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnEmail";
      gvColumn3.Text = "Email";
      gvColumn3.Width = 150;
      this.gridViewSales.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewSales.Location = new Point(8, 8);
      this.gridViewSales.Name = "gridViewSales";
      this.gridViewSales.Size = new Size(454, 277);
      this.gridViewSales.TabIndex = 10;
      this.gridViewSales.SelectedIndexChanged += new EventHandler(this.gridViewSales_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(470, 326);
      this.Controls.Add((Control) this.gridViewSales);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddSalesRepForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Sales Rep";
      this.ResumeLayout(false);
    }
  }
}
