// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.EditCompanyLenderContactsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class EditCompanyLenderContactsControl : UserControl
  {
    private Sessions.Session session;
    private int orgID = -1;
    private int companyOrgId = -1;
    private IList<ExternalOrgLenderContact> CompanyContacts;
    private ExternalOriginatorManagementData externalContact;
    private bool companyIsWholesaleEnabled;
    private bool companyIsDelegateEnabled;
    private bool companyIsNonDelegateEnabled;
    private bool enableEdits;
    private bool usingCustomOrdering;
    private int MaxDisplayOrder = -1;
    private IContainer components;
    private GridView gvLenderContacts;
    private GroupContainer grpLenderContacts;
    private GroupContainer grpLenderInvestorContacts;
    private Panel panelHeader;
    private Label sublabel;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnUp;
    private StandardIconButton stdIconBtnDown;
    private StandardIconButton stdIconBtnDelete;

    public EditCompanyLenderContactsControl() => this.InitializeComponent();

    public EditCompanyLenderContactsControl(Sessions.Session session, int orgID, int companyOrgId)
    {
      this.InitializeComponent();
      this.orgID = orgID;
      this.companyOrgId = companyOrgId;
      this.session = session;
      this.RefreshCompanyLenderContactsControl(session, orgID, companyOrgId);
      this.Dock = DockStyle.Fill;
    }

    public void RefreshCompanyLenderContactsControl(
      Sessions.Session session,
      int orgID,
      int companyOrgId)
    {
      this.orgID = orgID;
      this.companyOrgId = companyOrgId <= 0 ? orgID : companyOrgId;
      this.initForm();
    }

    private void initForm()
    {
      this.enableEdits = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.ExternalSettings_EditLenderContacts);
      this.externalContact = this.session.ConfigurationManager.GetByoid(false, this.orgID);
      this.companyIsWholesaleEnabled = this.externalContact.entityType == ExternalOriginatorEntityType.Both || this.externalContact.entityType == ExternalOriginatorEntityType.Broker;
      if (this.externalContact.entityType == ExternalOriginatorEntityType.Correspondent || this.externalContact.entityType == ExternalOriginatorEntityType.Both)
      {
        if (this.externalContact.UnderwritingType == ExternalOriginatorUnderwritingType.Both)
        {
          this.companyIsDelegateEnabled = true;
          this.companyIsNonDelegateEnabled = true;
        }
        else if (this.externalContact.UnderwritingType == ExternalOriginatorUnderwritingType.Delegated)
        {
          this.companyIsDelegateEnabled = true;
          this.companyIsNonDelegateEnabled = false;
        }
        else if (this.externalContact.UnderwritingType == ExternalOriginatorUnderwritingType.NonDelegated)
        {
          this.companyIsDelegateEnabled = false;
          this.companyIsNonDelegateEnabled = true;
        }
      }
      this.CompanyContacts = this.session.ConfigurationManager.GetTPOCompanyLenderContacts(this.companyOrgId <= 0 ? this.orgID : this.companyOrgId);
      this.gvLenderContacts.BeginUpdate();
      this.gvLenderContacts.Items.Clear();
      if (this.CompanyContacts != null && this.CompanyContacts.Count > 0)
      {
        foreach (ExternalOrgLenderContact companyContact in (IEnumerable<ExternalOrgLenderContact>) this.CompanyContacts)
        {
          if (!companyContact.isWholesaleChannelEnabled && !companyContact.isDelegatedChannelEnabled && !companyContact.isNonDelegatedChannelEnabled || this.companyIsWholesaleEnabled && companyContact.isWholesaleChannelEnabled || this.companyIsDelegateEnabled && companyContact.isDelegatedChannelEnabled || this.companyIsNonDelegateEnabled && companyContact.isNonDelegatedChannelEnabled)
            this.AddGridRow(companyContact);
          this.usingCustomOrdering |= companyContact.DisplayOrder > 0;
          if (companyContact.DisplayOrder > this.MaxDisplayOrder)
            this.MaxDisplayOrder = companyContact.DisplayOrder;
        }
      }
      this.gvLenderContacts.EndUpdate();
      this.stdIconBtnEdit.Enabled = this.enableEdits;
      this.stdIconBtnNew.Enabled = this.enableEdits;
      this.stdIconBtnDelete.Enabled = this.enableEdits;
      this.stdIconBtnDown.Enabled = this.enableEdits;
      this.stdIconBtnUp.Enabled = this.enableEdits;
    }

    private void AddGridRow(ExternalOrgLenderContact contact)
    {
      GVItem gvItem = new GVItem();
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems[0].Checked = contact.isWholesaleChannelEnabled;
      gvItem.SubItems[0].CheckBoxEnabled = false;
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems[1].Checked = contact.isNonDelegatedChannelEnabled;
      gvItem.SubItems[1].CheckBoxEnabled = false;
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems[2].Checked = contact.isDelegatedChannelEnabled;
      gvItem.SubItems[2].CheckBoxEnabled = false;
      gvItem.SubItems.Add((object) contact.Title);
      gvItem.SubItems.Add((object) contact.Name);
      gvItem.SubItems.Add((object) contact.Phone);
      gvItem.SubItems.Add((object) contact.Email);
      gvItem.SubItems.Add((object) "");
      if (contact.isPrimarySalesRep)
      {
        gvItem.SubItems[7].Checked = true;
        gvItem.SubItems[7].CheckBoxVisible = true;
        gvItem.SubItems[7].CheckBoxEnabled = false;
      }
      else
        gvItem.SubItems[7].CheckBoxVisible = false;
      gvItem.SubItems.Add((object) "");
      gvItem.SubItems[8].Checked = contact.isHidden;
      gvItem.SubItems[8].CheckBoxEnabled = this.enableEdits;
      gvItem.Tag = (object) contact;
      if (contact.Source != ExternalOrgCompanyContactSourceTable.ExternalOrgSalesReps)
      {
        int? externalOrgId = contact.ExternalOrgID;
        int companyOrgId = this.companyOrgId;
        if (externalOrgId.GetValueOrDefault() == companyOrgId & externalOrgId.HasValue)
          goto label_6;
      }
      gvItem.ForeColor = Color.Gray;
label_6:
      this.gvLenderContacts.Items.Add(gvItem);
    }

    public void DisableControls() => this.disableControl(this.Controls);

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
      }
    }

    private void gvLenderContacts_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.Index != 8 || !(e.SubItem.Item.Tag is ExternalOrgLenderContact tag))
        return;
      tag.isHidden = e.SubItem.Checked;
      this.session.ConfigurationManager.UpdateTPOCompanyLenderContact(this.companyOrgId == 0 ? this.orgID : this.companyOrgId, tag.ContactID, tag.Source, tag.isHidden ? 1 : 0, tag.DisplayOrder);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
    }

    private void gvLenderContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.EnableButtons();
    }

    private void EnableButtons()
    {
      if (this.gvLenderContacts.SelectedItems != null && this.gvLenderContacts.SelectedItems.Count == 1)
      {
        GVItem selectedItem = this.gvLenderContacts.SelectedItems[0];
        if (selectedItem != null)
        {
          ExternalOrgLenderContact tag = selectedItem.Tag as ExternalOrgLenderContact;
          bool flag = this.gvLenderContacts.SelectedItems.Count == 1;
          if (tag != null && tag.ExternalOrgID.HasValue)
          {
            this.stdIconBtnDelete.Enabled = true;
            this.stdIconBtnEdit.Enabled = flag;
          }
          int index = selectedItem.Index;
          if (index > 0)
            this.stdIconBtnUp.Enabled = flag;
          if (index < this.gvLenderContacts.Items.Count - 1)
            this.stdIconBtnDown.Enabled = flag;
        }
      }
      this.stdIconBtnEdit.Enabled = this.enableEdits;
      this.stdIconBtnNew.Enabled = this.enableEdits;
      this.stdIconBtnDelete.Enabled = this.enableEdits;
      this.stdIconBtnDown.Enabled = this.enableEdits;
      this.stdIconBtnUp.Enabled = this.enableEdits;
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvLenderContacts.SelectedItems == null || this.gvLenderContacts.SelectedItems.Count < 1 || Utils.Dialog((IWin32Window) this, "Deleting this contact will permanently remove the entry from the settings. Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      GVItem selectedItem = this.gvLenderContacts.SelectedItems[0];
      if (selectedItem == null || !(selectedItem.Tag is ExternalOrgLenderContact tag))
        return;
      this.session.ConfigurationManager.DeleteLenderContact(tag.ContactID);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
      this.initForm();
    }

    private void stdIconBtnDown_Click(object sender, EventArgs e)
    {
      if (this.gvLenderContacts.SelectedItems == null || this.gvLenderContacts.SelectedItems.Count != 1)
        return;
      int index = this.gvLenderContacts.SelectedItems[0].Index;
      this.SwapItemPositions(this.gvLenderContacts.Items[index], this.gvLenderContacts.Items[index + 1], true);
      this.EnableButtons();
    }

    private void stdIconBtnUp_Click(object sender, EventArgs e)
    {
      if (this.gvLenderContacts.SelectedItems == null || this.gvLenderContacts.SelectedItems.Count != 1)
        return;
      int index = this.gvLenderContacts.SelectedItems[0].Index;
      this.SwapItemPositions(this.gvLenderContacts.Items[index - 1], this.gvLenderContacts.Items[index], false);
      this.EnableButtons();
    }

    private void SwapItemPositions(GVItem top, GVItem bottom, bool selectTop)
    {
      ExternalOrgLenderContact tag1 = top.Tag as ExternalOrgLenderContact;
      ExternalOrgLenderContact tag2 = bottom.Tag as ExternalOrgLenderContact;
      int index = bottom.Index;
      this.gvLenderContacts.BeginUpdate();
      this.gvLenderContacts.Items.Remove(top);
      this.gvLenderContacts.Items.Insert(index, top);
      top.Selected = selectTop;
      bottom.Selected = !selectTop;
      this.gvLenderContacts.EndUpdate();
      int displayOrder = tag1.DisplayOrder;
      tag1.DisplayOrder = tag2.DisplayOrder;
      tag2.DisplayOrder = displayOrder;
      if (tag1.DisplayOrder < 0 || tag2.DisplayOrder < 0)
      {
        ExternalOrgLenderContact[] orgLenderContactArray = new ExternalOrgLenderContact[this.gvLenderContacts.Items.Count];
        for (int nItemIndex = 0; nItemIndex < this.gvLenderContacts.Items.Count; ++nItemIndex)
        {
          ExternalOrgLenderContact tag3 = this.gvLenderContacts.Items[nItemIndex].Tag as ExternalOrgLenderContact;
          tag3.DisplayOrder = nItemIndex + 1;
          orgLenderContactArray[nItemIndex] = tag3;
        }
        this.session.ConfigurationManager.UpdateTPOCompanyLenderContacts(this.companyOrgId, orgLenderContactArray);
      }
      else
        this.session.ConfigurationManager.UpdateTPOCompanyLenderContacts(this.companyOrgId, tag1, tag2);
      WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvLenderContacts.SelectedItems == null || this.gvLenderContacts.SelectedItems.Count != 1)
        return;
      using (AddLenderInvestorContactForm investorContactForm = new AddLenderInvestorContactForm(this.session, this.gvLenderContacts.SelectedItems[0].Tag as ExternalOrgLenderContact, this.Parent.Text))
      {
        if (investorContactForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.initForm();
      }
    }

    private void gvLenderContacts_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!this.enableEdits || this.gvLenderContacts.SelectedItems == null || this.gvLenderContacts.SelectedItems.Count != 1)
        return;
      GVItem selectedItem = this.gvLenderContacts.SelectedItems[0];
      if (selectedItem == null || !(selectedItem.Tag is ExternalOrgLenderContact tag) || tag.Source != ExternalOrgCompanyContactSourceTable.ExternalOrgLenderContacts)
        return;
      int? externalOrgId = tag.ExternalOrgID;
      int companyOrgId = this.companyOrgId;
      if (!(externalOrgId.GetValueOrDefault() == companyOrgId & externalOrgId.HasValue))
        return;
      this.stdIconBtnEdit_Click(source, (EventArgs) e);
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      using (AddLenderInvestorContactForm investorContactForm = new AddLenderInvestorContactForm(this.session, this.usingCustomOrdering ? ++this.MaxDisplayOrder : 0, new int?(this.companyOrgId), this.Parent.Text))
      {
        if (investorContactForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.initForm();
      }
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
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (EditCompanyLenderContactsControl));
      this.grpLenderContacts = new GroupContainer();
      this.grpLenderInvestorContacts = new GroupContainer();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnUp = new StandardIconButton();
      this.stdIconBtnDown = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.gvLenderContacts = new GridView();
      this.panelHeader = new Panel();
      this.sublabel = new Label();
      this.grpLenderContacts.SuspendLayout();
      this.grpLenderInvestorContacts.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUp).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDown).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.grpLenderContacts.Controls.Add((Control) this.grpLenderInvestorContacts);
      this.grpLenderContacts.Controls.Add((Control) this.panelHeader);
      this.grpLenderContacts.Dock = DockStyle.Fill;
      this.grpLenderContacts.HeaderForeColor = SystemColors.ControlText;
      this.grpLenderContacts.Location = new Point(5, 5);
      this.grpLenderContacts.Name = "grpLenderContacts";
      this.grpLenderContacts.Size = new Size(927, 674);
      this.grpLenderContacts.TabIndex = 10;
      this.grpLenderContacts.Text = "Lender Contacts";
      this.grpLenderInvestorContacts.Borders = AnchorStyles.Top;
      this.grpLenderInvestorContacts.Controls.Add((Control) this.stdIconBtnNew);
      this.grpLenderInvestorContacts.Controls.Add((Control) this.stdIconBtnEdit);
      this.grpLenderInvestorContacts.Controls.Add((Control) this.stdIconBtnUp);
      this.grpLenderInvestorContacts.Controls.Add((Control) this.stdIconBtnDown);
      this.grpLenderInvestorContacts.Controls.Add((Control) this.stdIconBtnDelete);
      this.grpLenderInvestorContacts.Controls.Add((Control) this.gvLenderContacts);
      this.grpLenderInvestorContacts.Dock = DockStyle.Fill;
      this.grpLenderInvestorContacts.HeaderForeColor = SystemColors.ControlText;
      this.grpLenderInvestorContacts.Location = new Point(1, 70);
      this.grpLenderInvestorContacts.Name = "grpLenderInvestorContacts";
      this.grpLenderInvestorContacts.Size = new Size(925, 603);
      this.grpLenderInvestorContacts.TabIndex = 2;
      this.grpLenderInvestorContacts.Text = "Lender/Investor Contacts";
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(814, 6);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 84;
      this.stdIconBtnNew.TabStop = false;
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(836, 6);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 83;
      this.stdIconBtnEdit.TabStop = false;
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUp.BackColor = Color.Transparent;
      this.stdIconBtnUp.Enabled = false;
      this.stdIconBtnUp.Location = new Point(858, 6);
      this.stdIconBtnUp.MouseDownImage = (Image) null;
      this.stdIconBtnUp.Name = "stdIconBtnUp";
      this.stdIconBtnUp.Size = new Size(16, 16);
      this.stdIconBtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUp.TabIndex = 82;
      this.stdIconBtnUp.TabStop = false;
      this.stdIconBtnUp.Click += new EventHandler(this.stdIconBtnUp_Click);
      this.stdIconBtnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDown.BackColor = Color.Transparent;
      this.stdIconBtnDown.Enabled = false;
      this.stdIconBtnDown.Location = new Point(880, 6);
      this.stdIconBtnDown.MouseDownImage = (Image) null;
      this.stdIconBtnDown.Name = "stdIconBtnDown";
      this.stdIconBtnDown.Size = new Size(16, 16);
      this.stdIconBtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDown.TabIndex = 81;
      this.stdIconBtnDown.TabStop = false;
      this.stdIconBtnDown.Click += new EventHandler(this.stdIconBtnDown_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Enabled = false;
      this.stdIconBtnDelete.Location = new Point(902, 6);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 80;
      this.stdIconBtnDelete.TabStop = false;
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.gvLenderContacts.AutoHeight = true;
      this.gvLenderContacts.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Wholesale";
      gvColumn1.Width = 100;
      gvColumn2.CheckBoxes = true;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Non-Delegated";
      gvColumn2.Width = 100;
      gvColumn3.CheckBoxes = true;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Delegated";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Title / Department";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Name";
      gvColumn5.Width = 200;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Phone";
      gvColumn6.Width = 200;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Email";
      gvColumn7.Width = 250;
      gvColumn8.CheckBoxes = true;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Text = "Primary";
      gvColumn8.Width = 75;
      gvColumn9.CheckBoxes = true;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Text = "Hide";
      gvColumn9.Width = 50;
      this.gvLenderContacts.Columns.AddRange(new GVColumn[9]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gvLenderContacts.Dock = DockStyle.Fill;
      this.gvLenderContacts.HeaderHeight = 22;
      this.gvLenderContacts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLenderContacts.Location = new Point(0, 26);
      this.gvLenderContacts.Name = "gvLenderContacts";
      this.gvLenderContacts.Size = new Size(925, 577);
      this.gvLenderContacts.TabIndex = 0;
      this.gvLenderContacts.SelectedIndexChanged += new EventHandler(this.gvLenderContacts_SelectedIndexChanged);
      this.gvLenderContacts.SubItemCheck += new GVSubItemEventHandler(this.gvLenderContacts_SubItemCheck);
      this.gvLenderContacts.ItemDoubleClick += new GVItemEventHandler(this.gvLenderContacts_ItemDoubleClick);
      this.panelHeader.Controls.Add((Control) this.sublabel);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(925, 44);
      this.panelHeader.TabIndex = 1;
      this.sublabel.Location = new Point(1, 4);
      this.sublabel.Name = "sublabel";
      this.sublabel.Padding = new Padding(4, 0, 0, 0);
      this.sublabel.Size = new Size(920, 35);
      this.sublabel.TabIndex = 35;
      this.sublabel.Text = componentResourceManager.GetString("sublabel.Text");
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpLenderContacts);
      this.Margin = new Padding(0);
      this.Name = nameof (EditCompanyLenderContactsControl);
      this.Padding = new Padding(5);
      this.Size = new Size(937, 684);
      this.grpLenderContacts.ResumeLayout(false);
      this.grpLenderInvestorContacts.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnUp).EndInit();
      ((ISupportInitialize) this.stdIconBtnDown).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
