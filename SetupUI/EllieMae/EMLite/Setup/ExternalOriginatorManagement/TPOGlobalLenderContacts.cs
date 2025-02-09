// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOGlobalLenderContacts
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class TPOGlobalLenderContacts : UserControl
  {
    private IConfigurationManager confManager;
    private Sessions.Session session;
    private IList<ExternalOrgLenderContact> lenderContacts;
    private int MaxDisplayOrder = -1;
    private static bool cleanupChecked;
    private IContainer components;
    private GroupContainer gcLenderContacts;
    private GridView gvLenderContacts;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnUp;
    private StandardIconButton stdIconBtnDown;
    private StandardIconButton stdIconBtnDelete;

    public TPOGlobalLenderContacts() => this.InitializeComponent();

    public TPOGlobalLenderContacts(Sessions.Session session, SetUpContainer setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.confManager = session.ConfigurationManager;
      this.stdIconBtnEdit.Enabled = false;
      this.initForm();
    }

    private void initForm()
    {
      this.gvLenderContacts.Items.Clear();
      this.lenderContacts = this.confManager.GetGlobalLenderContacts();
      this.CleanupDisplayOrder();
      if (this.lenderContacts != null)
      {
        foreach (ExternalOrgLenderContact lenderContact in (IEnumerable<ExternalOrgLenderContact>) this.lenderContacts)
          this.gvLenderContacts.Items.Add(this.GetRowItem(lenderContact));
      }
      this.gvLenderContacts.EndUpdate();
    }

    private GVItem GetRowItem(ExternalOrgLenderContact contact)
    {
      GVItem rowItem = new GVItem()
      {
        SubItems = {
          [0] = {
            Checked = contact.isWholesaleChannelEnabled,
            CheckBoxEnabled = false
          },
          [1] = {
            Checked = contact.isNonDelegatedChannelEnabled,
            CheckBoxEnabled = false
          },
          [2] = {
            Checked = contact.isDelegatedChannelEnabled,
            CheckBoxEnabled = false
          }
        }
      };
      rowItem.SubItems.Add((object) contact.Title);
      rowItem.SubItems.Add((object) contact.Name);
      rowItem.SubItems.Add((object) contact.Phone);
      rowItem.SubItems.Add((object) contact.Email);
      rowItem.Tag = (object) contact;
      if (contact.DisplayOrder > this.MaxDisplayOrder)
        this.MaxDisplayOrder = contact.DisplayOrder;
      return rowItem;
    }

    private void CleanupDisplayOrder()
    {
      if (TPOGlobalLenderContacts.cleanupChecked || this.lenderContacts == null)
        return;
      TPOGlobalLenderContacts.cleanupChecked = true;
      bool flag = false;
      Dictionary<int, ExternalOrgLenderContact> dictionary = new Dictionary<int, ExternalOrgLenderContact>();
      foreach (ExternalOrgLenderContact lenderContact in (IEnumerable<ExternalOrgLenderContact>) this.lenderContacts)
      {
        if (dictionary.ContainsKey(lenderContact.DisplayOrder))
        {
          flag = true;
          break;
        }
        dictionary.Add(lenderContact.DisplayOrder, lenderContact);
      }
      if (!flag)
        return;
      for (int index = 0; index < this.lenderContacts.Count; ++index)
        this.lenderContacts[index].DisplayOrder = index + 1;
      this.confManager.UpdateLenderContacts(this.lenderContacts.ToArray<ExternalOrgLenderContact>());
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Deleting this contact will permanently remove the entry from the settings. Do you wish to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      foreach (GVItem selectedItem in this.gvLenderContacts.SelectedItems)
        this.confManager.DeleteLenderContact(((ExternalOrgLenderContact) selectedItem.Tag).ContactID);
      this.initForm();
    }

    private void gvLenderContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SetButtonProp();
    }

    private void stdIconBtnUp_Click(object sender, EventArgs e)
    {
      this.ChangeOrder((ExternalOrgLenderContact) this.gvLenderContacts.SelectedItems[0].Tag, (ExternalOrgLenderContact) this.gvLenderContacts.Items[this.gvLenderContacts.SelectedItems[0].Index - 1].Tag);
      ExternalOrgLenderContact tag = (ExternalOrgLenderContact) this.gvLenderContacts.SelectedItems[0].Tag;
      int nItemIndex = this.gvLenderContacts.SelectedItems[0].Index - 1;
      this.gvLenderContacts.Items.Insert(this.gvLenderContacts.SelectedItems[0].Index - 1, this.GetRowItem(tag));
      this.gvLenderContacts.Items.RemoveAt(this.gvLenderContacts.SelectedItems[0].Index);
      this.gvLenderContacts.Items[nItemIndex].Selected = true;
      this.SetButtonProp();
    }

    private void stdIconBtnDown_Click(object sender, EventArgs e)
    {
      this.ChangeOrder((ExternalOrgLenderContact) this.gvLenderContacts.Items[this.gvLenderContacts.SelectedItems[0].Index + 1].Tag, (ExternalOrgLenderContact) this.gvLenderContacts.SelectedItems[0].Tag);
      ExternalOrgLenderContact tag = (ExternalOrgLenderContact) this.gvLenderContacts.Items[this.gvLenderContacts.SelectedItems[0].Index].Tag;
      int num = this.gvLenderContacts.SelectedItems[0].Index + 1;
      this.gvLenderContacts.Items.RemoveAt(this.gvLenderContacts.SelectedItems[0].Index);
      this.gvLenderContacts.Items.Insert(num, this.GetRowItem(tag));
      this.gvLenderContacts.Items[num].Selected = true;
      this.SetButtonProp();
    }

    private void ChangeOrder(
      ExternalOrgLenderContact bottomLender,
      ExternalOrgLenderContact topLender)
    {
      int displayOrder = bottomLender.DisplayOrder;
      bottomLender.DisplayOrder = topLender.DisplayOrder;
      topLender.DisplayOrder = displayOrder;
      this.confManager.UpdateLenderContacts(topLender, bottomLender);
    }

    private void SetButtonProp()
    {
      this.stdIconBtnDelete.Enabled = this.gvLenderContacts.SelectedItems.Count > 0;
      this.stdIconBtnEdit.Enabled = this.gvLenderContacts.SelectedItems.Count == 1;
      if (this.gvLenderContacts.SelectedItems.Count == 1)
      {
        this.stdIconBtnUp.Enabled = this.gvLenderContacts.SelectedItems[0].Index != 0;
        this.stdIconBtnDown.Enabled = this.gvLenderContacts.SelectedItems[0].Index + 1 != this.gvLenderContacts.Items.Count;
      }
      else
      {
        this.stdIconBtnUp.Enabled = false;
        this.stdIconBtnDown.Enabled = false;
      }
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      using (AddLenderInvestorContactForm investorContactForm = new AddLenderInvestorContactForm(this.session, this.MaxDisplayOrder + 1, new int?(), this.Parent.Text))
      {
        if (investorContactForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.initForm();
      }
    }

    private void gvLenderContacts_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (this.gvLenderContacts.SelectedItems == null || this.gvLenderContacts.SelectedItems.Count <= 0)
        return;
      this.editSelectedItem();
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void editSelectedItem()
    {
      if (this.gvLenderContacts.SelectedItems == null || this.gvLenderContacts.SelectedItems.Count != 1)
        return;
      using (AddLenderInvestorContactForm investorContactForm = new AddLenderInvestorContactForm(this.session, (ExternalOrgLenderContact) this.gvLenderContacts.SelectedItems[0].Tag, this.Parent.Text))
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
      this.gcLenderContacts = new GroupContainer();
      this.gvLenderContacts = new GridView();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnUp = new StandardIconButton();
      this.stdIconBtnDown = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.gcLenderContacts.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUp).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDown).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.gcLenderContacts.Controls.Add((Control) this.gvLenderContacts);
      this.gcLenderContacts.Controls.Add((Control) this.stdIconBtnNew);
      this.gcLenderContacts.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcLenderContacts.Controls.Add((Control) this.stdIconBtnUp);
      this.gcLenderContacts.Controls.Add((Control) this.stdIconBtnDown);
      this.gcLenderContacts.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcLenderContacts.Dock = DockStyle.Fill;
      this.gcLenderContacts.HeaderForeColor = SystemColors.ControlText;
      this.gcLenderContacts.Location = new Point(0, 0);
      this.gcLenderContacts.Name = "gcLenderContacts";
      this.gcLenderContacts.Size = new Size(886, 517);
      this.gcLenderContacts.TabIndex = 2;
      this.gcLenderContacts.Text = "Lender/Investor Contacts";
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
      this.gvLenderContacts.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvLenderContacts.Dock = DockStyle.Fill;
      this.gvLenderContacts.HeaderHeight = 22;
      this.gvLenderContacts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLenderContacts.Location = new Point(1, 26);
      this.gvLenderContacts.Name = "gvLenderContacts";
      this.gvLenderContacts.Size = new Size(884, 490);
      this.gvLenderContacts.SortOption = GVSortOption.None;
      this.gvLenderContacts.TabIndex = 0;
      this.gvLenderContacts.SelectedIndexChanged += new EventHandler(this.gvLenderContacts_SelectedIndexChanged);
      this.gvLenderContacts.ItemDoubleClick += new GVItemEventHandler(this.gvLenderContacts_ItemDoubleClick);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(778, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 79;
      this.stdIconBtnNew.TabStop = false;
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(800, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 77;
      this.stdIconBtnEdit.TabStop = false;
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUp.BackColor = Color.Transparent;
      this.stdIconBtnUp.Enabled = false;
      this.stdIconBtnUp.Location = new Point(822, 5);
      this.stdIconBtnUp.MouseDownImage = (Image) null;
      this.stdIconBtnUp.Name = "stdIconBtnUp";
      this.stdIconBtnUp.Size = new Size(16, 16);
      this.stdIconBtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUp.TabIndex = 76;
      this.stdIconBtnUp.TabStop = false;
      this.stdIconBtnUp.Click += new EventHandler(this.stdIconBtnUp_Click);
      this.stdIconBtnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDown.BackColor = Color.Transparent;
      this.stdIconBtnDown.Enabled = false;
      this.stdIconBtnDown.Location = new Point(844, 5);
      this.stdIconBtnDown.MouseDownImage = (Image) null;
      this.stdIconBtnDown.Name = "stdIconBtnDown";
      this.stdIconBtnDown.Size = new Size(16, 16);
      this.stdIconBtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDown.TabIndex = 75;
      this.stdIconBtnDown.TabStop = false;
      this.stdIconBtnDown.Click += new EventHandler(this.stdIconBtnDown_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Enabled = false;
      this.stdIconBtnDelete.Location = new Point(866, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 74;
      this.stdIconBtnDelete.TabStop = false;
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcLenderContacts);
      this.Name = nameof (TPOGlobalLenderContacts);
      this.Size = new Size(886, 517);
      this.gcLenderContacts.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnUp).EndInit();
      ((ISupportInitialize) this.stdIconBtnDown).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
