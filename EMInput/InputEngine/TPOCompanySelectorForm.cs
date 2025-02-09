// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TPOCompanySelectorForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TPOCompanySelectorForm : Form
  {
    private Sessions.Session session;
    private GridViewFilterManager filterMnger;
    private List<ExternalOriginatorManagementData> orgs;
    private bool branchSelector;
    private IContainer components;
    private GroupContainer groupList;
    private GridView grd3rdParties;
    private Button btnCancel;
    private Button btnSelect;

    public TPOCompanySelectorForm(
      List<ExternalOriginatorManagementData> orgs,
      Sessions.Session session,
      bool branchSelector)
    {
      this.session = session;
      this.orgs = orgs;
      this.branchSelector = branchSelector;
      this.InitializeComponent();
      this.filterMnger = new GridViewFilterManager(this.session, this.grd3rdParties, false);
      if (this.branchSelector)
      {
        this.grd3rdParties.Columns.RemoveAt(0);
        this.grd3rdParties.Columns.RemoveAt(0);
      }
      this.filterMnger.CreateColumnFilter(0, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(1, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(2, GridViewFilterControlType.Text);
      if (!this.branchSelector)
      {
        this.filterMnger.CreateColumnFilter(3, GridViewFilterControlType.Text);
        this.filterMnger.CreateColumnFilter(4, GridViewFilterControlType.Text);
      }
      this.filterMnger.FilterChanged += new EventHandler(this.filterMnger_FilterChanged);
      this.filterMnger_FilterChanged((object) null, (EventArgs) null);
      if (!this.branchSelector)
        this.grd3rdParties.Sort(2, SortOrder.Ascending);
      else
        this.grd3rdParties.Sort(0, SortOrder.Ascending);
    }

    private void filterMnger_FilterChanged(object sender, EventArgs e)
    {
      this.initForm();
      this.grd3rdParties_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void initForm()
    {
      if (this.orgs == null || this.orgs.Count == 0)
        return;
      this.grd3rdParties.BeginUpdate();
      this.grd3rdParties.Items.Clear();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      foreach (ExternalOriginatorManagementData org in this.orgs)
      {
        GVItem gvItem;
        if (this.branchSelector)
        {
          gvItem = new GVItem(org.OrganizationName == null ? "" : org.OrganizationName);
        }
        else
        {
          gvItem = new GVItem(org.OrgID);
          gvItem.SubItems.Add((object) org.ExternalID);
          gvItem.SubItems.Add(org.OrganizationName == null ? (object) "" : (object) org.OrganizationName);
        }
        string str = org.Address + " " + org.City + ", " + org.State + " " + org.Zip;
        if (str.Trim() == ",")
          str = string.Empty;
        gvItem.SubItems.Add((object) str);
        gvItem.SubItems.Add(org.PhoneNumber == null ? (object) "" : (object) org.PhoneNumber);
        gvItem.Tag = (object) org;
        int index1 = 0;
        if (!this.branchSelector)
        {
          string valueDescription1 = this.filterMnger.GetFilterForColumn(index1) == null ? "" : this.filterMnger.GetFilterForColumn(index1).ValueDescription;
          if (!(valueDescription1 != "") || !((org.OrgID ?? "") == string.Empty) && org.OrgID.ToLower().Contains(valueDescription1.ToLower()))
          {
            int index2 = index1 + 1;
            string valueDescription2 = this.filterMnger.GetFilterForColumn(index2) == null ? "" : this.filterMnger.GetFilterForColumn(index2).ValueDescription;
            if (!(valueDescription2 != "") || !((org.ExternalID ?? "") == string.Empty) && org.ExternalID.ToLower().Contains(valueDescription2.ToLower()))
              index1 = index2 + 1;
            else
              continue;
          }
          else
            continue;
        }
        string valueDescription3 = this.filterMnger.GetFilterForColumn(index1) == null ? "" : this.filterMnger.GetFilterForColumn(index1).ValueDescription;
        if (!(valueDescription3 != "") || !((org.OrganizationName ?? "") == string.Empty) && org.OrganizationName.ToLower().Contains(valueDescription3.ToLower()))
        {
          int index3 = index1 + 1;
          string valueDescription4 = this.filterMnger.GetFilterForColumn(index3) == null ? "" : this.filterMnger.GetFilterForColumn(index3).ValueDescription;
          if (!(valueDescription4 != "") || str.ToLower().Contains(valueDescription4.ToLower()) && !(str == string.Empty))
          {
            int index4 = index3 + 1;
            string valueDescription5 = this.filterMnger.GetFilterForColumn(index4) == null ? "" : this.filterMnger.GetFilterForColumn(index4).ValueDescription;
            if (!(valueDescription5 != "") || !((org.PhoneNumber ?? "") == string.Empty) && org.PhoneNumber.ToLower().Contains(valueDescription5.ToLower()))
              this.grd3rdParties.Items.Add(gvItem);
          }
        }
      }
      this.grd3rdParties.EndUpdate();
    }

    private void btnSelect_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void grd3rdParties_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.grd3rdParties.SelectedItems.Count == 1;
    }

    public ExternalOriginatorManagementData SelectedOrganization
    {
      get => (ExternalOriginatorManagementData) this.grd3rdParties.SelectedItems[0].Tag;
    }

    private void grd3rdParties_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnSelect_Click((object) null, (EventArgs) null);
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
      this.btnCancel = new Button();
      this.btnSelect = new Button();
      this.groupList = new GroupContainer();
      this.grd3rdParties = new GridView();
      this.groupList.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(874, 568);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(793, 568);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 8;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.groupList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupList.Controls.Add((Control) this.grd3rdParties);
      this.groupList.HeaderForeColor = SystemColors.ControlText;
      this.groupList.Location = new Point(12, 12);
      this.groupList.Name = "groupList";
      this.groupList.Size = new Size(937, 542);
      this.groupList.TabIndex = 6;
      this.groupList.Text = "Third Party Originators";
      this.grd3rdParties.AllowMultiselect = false;
      this.grd3rdParties.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnOrgID";
      gvColumn1.Text = "Org ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnTPOID";
      gvColumn2.Text = "TPO ID";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnLegalName";
      gvColumn3.Text = "Organization Name";
      gvColumn3.Width = 250;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnAddress";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Address";
      gvColumn4.Width = 335;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnPhone";
      gvColumn5.Text = "Phone";
      gvColumn5.Width = 150;
      this.grd3rdParties.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.grd3rdParties.Dock = DockStyle.Fill;
      this.grd3rdParties.FilterVisible = true;
      this.grd3rdParties.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grd3rdParties.Location = new Point(1, 26);
      this.grd3rdParties.Name = "grd3rdParties";
      this.grd3rdParties.Size = new Size(935, 515);
      this.grd3rdParties.TabIndex = 3;
      this.grd3rdParties.SelectedIndexChanged += new EventHandler(this.grd3rdParties_SelectedIndexChanged);
      this.grd3rdParties.ItemDoubleClick += new GVItemEventHandler(this.grd3rdParties_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(961, 603);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupList);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TPOCompanySelectorForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Third Party Originator Selector";
      this.groupList.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
