// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddKeyContact3rdPartyForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
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
  public class AddKeyContact3rdPartyForm : Form
  {
    private Sessions.Session session;
    private int orgID = -1;
    private List<string> externalUserIDs = new List<string>();
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private GridView gridViewContacts;

    public AddKeyContact3rdPartyForm(Sessions.Session session, int orgID)
    {
      this.session = session;
      this.orgID = orgID;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      this.gridViewContacts_SelectedIndexChanged((object) null, (EventArgs) null);
      ExternalUserInfo[] externalUserInfos = this.session.ConfigurationManager.GetExternalUserInfos(this.orgID, true);
      foreach (ExternalUserInfo externalUserInfo in externalUserInfos)
        this.gridViewContacts.Items.Add(new GVItem(new string[6]
        {
          externalUserInfo.LastName,
          externalUserInfo.FirstName,
          TPOUtils.returnRoles(externalUserInfo.Roles).Trim(),
          externalUserInfo.Email,
          externalUserInfo.Phone,
          externalUserInfo.DisabledLogin ? "Disabled" : "Enabled"
        })
        {
          Tag = (object) externalUserInfo
        });
      this.Text = "Import Third Party Originator Contacts (" + (object) externalUserInfos.Length + ")";
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
        return;
      for (int index = 0; index < this.gridViewContacts.SelectedItems.Count; ++index)
        this.externalUserIDs.Add(((ExternalUserInfo) this.gridViewContacts.SelectedItems[index].Tag).ExternalUserID);
      this.DialogResult = DialogResult.OK;
    }

    public string[] ExternalUserIDs => this.externalUserIDs.ToArray();

    private void gridViewContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.gridViewContacts.SelectedItems.Count > 0;
    }

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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.gridViewContacts = new GridView();
      this.SuspendLayout();
      this.btnOK.Location = new Point(668, 417);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&Select";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(749, 417);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.gridViewContacts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gridViewContacts.BorderStyle = BorderStyle.FixedSingle;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnLastName";
      gvColumn1.Text = "Last Name";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnFirst Name";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnRoles";
      gvColumn3.Text = "Roles";
      gvColumn3.Width = 120;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnEmail";
      gvColumn4.Text = "Email";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnPhone";
      gvColumn5.Text = "Phone #";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnLogin";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Login";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 88;
      this.gridViewContacts.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gridViewContacts.Location = new Point(12, 12);
      this.gridViewContacts.Name = "gridViewContacts";
      this.gridViewContacts.Size = new Size(810, 399);
      this.gridViewContacts.TabIndex = 9;
      this.gridViewContacts.SelectedIndexChanged += new EventHandler(this.gridViewContacts_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(834, 452);
      this.Controls.Add((Control) this.gridViewContacts);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddKeyContact3rdPartyForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.ResumeLayout(false);
    }
  }
}
