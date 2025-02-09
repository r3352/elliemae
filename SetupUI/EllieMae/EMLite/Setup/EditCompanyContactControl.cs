// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyContactControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyContactControl : UserControl
  {
    private Sessions.Session session;
    private int orgID = -1;
    private IContainer components;
    private GridView gridViewContacts;
    private StandardIconButton btnDownload;
    private StandardIconButton btnDelete;
    private StandardIconButton btnEdit;
    private StandardIconButton btnAdd;
    private GroupContainer grpAll;
    private GroupContainer groupContainer3;
    private Panel panelHeader;
    private Label label33;
    private StandardIconButton exportButton;

    public EditCompanyContactControl(Sessions.Session session, int orgID)
    {
      this.InitializeComponent();
      this.session = session;
      this.orgID = orgID;
      this.Dock = DockStyle.Fill;
      this.gridViewContacts_SelectedIndexChanged((object) null, (EventArgs) null);
      this.RefreshKeyContacts();
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
    }

    private void RefreshKeyContacts()
    {
      this.gridViewContacts.BeginUpdate();
      this.gridViewContacts.Items.Clear();
      List<ExternalOrgContact> externalOrgContacts = this.session.ConfigurationManager.GetExternalOrgContacts(this.orgID);
      foreach (ExternalOrgContact hs in externalOrgContacts)
        this.buildContactsGVItem(hs);
      this.groupContainer3.Text = "TPO Key Contacts (" + (object) externalOrgContacts.Count + ")";
      this.exportButton.Enabled = externalOrgContacts.Count > 0;
      this.gridViewContacts.EndUpdate();
    }

    private void buildContactsGVItem(ExternalOrgContact hs)
    {
      this.gridViewContacts.Items.Add(new GVItem(new string[4]
      {
        hs.Name,
        hs.Title,
        hs.Phone,
        hs.Email
      })
      {
        Tag = (object) hs
      });
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (AddKeyContactForm addKeyContactForm = new AddKeyContactForm(this.session, this.orgID))
      {
        if (addKeyContactForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        int num1 = this.session.ConfigurationManager.AddExternalOrgManualContact(addKeyContactForm.ExternalOrgContact);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
        if (num1 > -1)
        {
          ExternalOrgContact externalOrgContact = addKeyContactForm.ExternalOrgContact;
          externalOrgContact.ExternalOrgContactID = num1;
          this.buildContactsGVItem(externalOrgContact);
          this.groupContainer3.Text = "TPO Key Contacts (" + (object) this.gridViewContacts.Items.Count + ")";
          this.exportButton.Enabled = this.gridViewContacts.Items.Count > 0;
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "There was an error performing this action, please contact your Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
        return;
      ExternalOrgContact tag = (ExternalOrgContact) this.gridViewContacts.SelectedItems[0].Tag;
      if (tag.Type == ExternalOriginatorContactType.TPO)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "TPO users can't be edited here.");
      }
      else
      {
        using (AddKeyContactForm addKeyContactForm = new AddKeyContactForm(this.session, this.orgID, tag))
        {
          if (addKeyContactForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          if (this.session.ConfigurationManager.UpdateExternalOrgManualContact(addKeyContactForm.ExternalOrgContact))
          {
            this.gridViewContacts.SelectedItems[0].SubItems[0].Text = addKeyContactForm.ExternalOrgContact.Name;
            this.gridViewContacts.SelectedItems[0].SubItems[1].Text = addKeyContactForm.ExternalOrgContact.Title;
            this.gridViewContacts.SelectedItems[0].SubItems[2].Text = addKeyContactForm.ExternalOrgContact.Phone;
            this.gridViewContacts.SelectedItems[0].SubItems[3].Text = addKeyContactForm.ExternalOrgContact.Email;
            this.gridViewContacts.SelectedItems[0].Tag = (object) addKeyContactForm.ExternalOrgContact;
            WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
          }
          else
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "There was an error performing this action, please contact your Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
    }

    private void btnDownload_Click(object sender, EventArgs e)
    {
      using (AddKeyContact3rdPartyForm contact3rdPartyForm = new AddKeyContact3rdPartyForm(this.session, this.orgID))
      {
        if (contact3rdPartyForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (this.session.ConfigurationManager.AddTpoUserToExtOrgContact(contact3rdPartyForm.ExternalUserIDs, this.orgID))
        {
          WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
          this.RefreshKeyContacts();
        }
        else
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "There was an error performing this action, please contact your Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems == null || this.gridViewContacts.SelectedItems.Count != 1 || Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected key contacts?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      List<ExternalOrgContact> externalOrgContact = new List<ExternalOrgContact>();
      for (int index = 0; index < this.gridViewContacts.SelectedItems.Count; ++index)
      {
        ExternalOrgContact tag = (ExternalOrgContact) this.gridViewContacts.SelectedItems[index].Tag;
        externalOrgContact.Add(tag);
      }
      if (this.session.ConfigurationManager.DeleteExternalOrgContact(externalOrgContact))
      {
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
        this.RefreshKeyContacts();
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There was an error performing this action, please contact your Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void gridViewContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDelete.Enabled = this.gridViewContacts.SelectedItems.Count > 0;
      this.btnEdit.Enabled = this.gridViewContacts.SelectedItems.Count == 1;
    }

    private void exportButton_Click(object sender, EventArgs e)
    {
      ExcelHandler excelExport = new ExcelHandler();
      excelExport.Headers = new string[4]
      {
        "Name",
        "Title",
        "Phone",
        "Email"
      };
      this.gridViewContacts.Items.ToList<GVItem>().ForEach((Action<GVItem>) (x =>
      {
        ExternalOrgContact tag = (ExternalOrgContact) x.Tag;
        excelExport.AddDataRow(new string[4]
        {
          tag.Name,
          tag.Title,
          tag.Phone,
          tag.Email
        });
      }));
      excelExport.Export();
    }

    private void gridViewContacts_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click(source, (EventArgs) e);
    }

    public void DisableControls()
    {
      this.exportButton.Visible = this.btnAdd.Visible = this.btnDelete.Visible = this.btnDownload.Visible = this.btnEdit.Visible = false;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCompanyContactControl));
      this.grpAll = new GroupContainer();
      this.groupContainer3 = new GroupContainer();
      this.exportButton = new StandardIconButton();
      this.btnDownload = new StandardIconButton();
      this.gridViewContacts = new GridView();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.grpAll.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      ((ISupportInitialize) this.exportButton).BeginInit();
      ((ISupportInitialize) this.btnDownload).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.grpAll.Controls.Add((Control) this.groupContainer3);
      this.grpAll.Controls.Add((Control) this.panelHeader);
      this.grpAll.Dock = DockStyle.Fill;
      this.grpAll.HeaderForeColor = SystemColors.ControlText;
      this.grpAll.Location = new Point(5, 5);
      this.grpAll.Name = "grpAll";
      this.grpAll.Size = new Size(844, 674);
      this.grpAll.TabIndex = 10;
      this.grpAll.Text = "TPO Contacts";
      this.groupContainer3.Borders = AnchorStyles.Top;
      this.groupContainer3.Controls.Add((Control) this.exportButton);
      this.groupContainer3.Controls.Add((Control) this.btnDownload);
      this.groupContainer3.Controls.Add((Control) this.gridViewContacts);
      this.groupContainer3.Controls.Add((Control) this.btnDelete);
      this.groupContainer3.Controls.Add((Control) this.btnEdit);
      this.groupContainer3.Controls.Add((Control) this.btnAdd);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(1, 70);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(842, 603);
      this.groupContainer3.TabIndex = 2;
      this.exportButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.exportButton.BackColor = Color.Transparent;
      this.exportButton.Location = new Point(814, 4);
      this.exportButton.MouseDownImage = (Image) null;
      this.exportButton.Name = "exportButton";
      this.exportButton.Size = new Size(16, 16);
      this.exportButton.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.exportButton.TabIndex = 13;
      this.exportButton.TabStop = false;
      this.exportButton.Click += new EventHandler(this.exportButton_Click);
      this.btnDownload.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDownload.BackColor = Color.Transparent;
      this.btnDownload.Location = new Point(792, 5);
      this.btnDownload.MouseDownImage = (Image) null;
      this.btnDownload.Name = "btnDownload";
      this.btnDownload.Size = new Size(16, 16);
      this.btnDownload.StandardButtonType = StandardIconButton.ButtonType.ImportLoanButton;
      this.btnDownload.TabIndex = 12;
      this.btnDownload.TabStop = false;
      this.btnDownload.Click += new EventHandler(this.btnDownload_Click);
      this.gridViewContacts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnName";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnTitle";
      gvColumn2.Text = "Title";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnPhone";
      gvColumn3.Text = "Phone #";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 120;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnEmail";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Email";
      gvColumn4.Width = 322;
      this.gridViewContacts.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridViewContacts.Dock = DockStyle.Fill;
      this.gridViewContacts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewContacts.Location = new Point(0, 26);
      this.gridViewContacts.Name = "gridViewContacts";
      this.gridViewContacts.Size = new Size(842, 577);
      this.gridViewContacts.TabIndex = 8;
      this.gridViewContacts.SelectedIndexChanged += new EventHandler(this.gridViewContacts_SelectedIndexChanged);
      this.gridViewContacts.ItemDoubleClick += new GVItemEventHandler(this.gridViewContacts_ItemDoubleClick);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(770, 5);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 11;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(748, 5);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 10;
      this.btnEdit.TabStop = false;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(726, 5);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 9;
      this.btnAdd.TabStop = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(842, 44);
      this.panelHeader.TabIndex = 1;
      this.label33.Location = new Point(1, 4);
      this.label33.Name = "label33";
      this.label33.Padding = new Padding(4, 0, 0, 0);
      this.label33.Size = new Size(838, 35);
      this.label33.TabIndex = 35;
      this.label33.Text = componentResourceManager.GetString("label33.Text");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpAll);
      this.Margin = new Padding(0);
      this.Name = nameof (EditCompanyContactControl);
      this.Padding = new Padding(5);
      this.Size = new Size(854, 684);
      this.grpAll.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      ((ISupportInitialize) this.exportButton).EndInit();
      ((ISupportInitialize) this.btnDownload).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
