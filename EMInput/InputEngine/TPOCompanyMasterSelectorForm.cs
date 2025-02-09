// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TPOCompanyMasterSelectorForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TPOCompanyMasterSelectorForm : Form
  {
    private Sessions.Session session;
    private GridViewFilterManager filterMnger;
    private List<ExternalOriginatorManagementData> orgs;
    private IContainer components;
    private GroupContainer groupList;
    private GridView grd3rdParties;
    private Button btnCancel;
    private Button btnSelect;

    public TPOCompanyMasterSelectorForm(
      List<ExternalOriginatorManagementData> orgs,
      Sessions.Session session,
      bool isQAOnly = true)
    {
      this.session = session;
      this.orgs = orgs;
      this.InitializeComponent();
      this.filterMnger = new GridViewFilterManager(this.session, this.grd3rdParties, false);
      this.filterMnger.CreateColumnFilter(0, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(1, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(2, GridViewFilterControlType.Text);
      this.filterMnger.CreateColumnFilter(3, GridViewFilterControlType.Decimal);
      this.filterMnger.CreateColumnFilter(4, GridViewFilterControlType.Decimal);
      this.filterMnger.FilterChanged += new EventHandler(this.filterMnger_FilterChanged);
      this.filterMnger_FilterChanged((object) null, (EventArgs) null);
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
      Hashtable availableCommitment = this.session.ConfigurationManager.GetExternalOrganizationsAvailableCommitment(this.orgs.Select<ExternalOriginatorManagementData, int>((Func<ExternalOriginatorManagementData, int>) (x => x.oid)).ToArray<int>());
      foreach (ExternalOriginatorManagementData org in this.orgs)
      {
        GVItem gvItem = new GVItem(org.OrganizationName == null ? "" : org.OrganizationName);
        gvItem.SubItems.Add((object) org.ExternalID);
        gvItem.SubItems.Add(org.OrgID == null ? (object) "" : (object) org.OrgID);
        gvItem.SubItems.Add((object) org.MaxCommitmentAmount.ToString("#,##0"));
        double num = 0.0;
        if (availableCommitment[(object) org.oid] != null)
          num = double.Parse(availableCommitment[(object) org.oid].ToString());
        gvItem.SubItems.Add((object) num.ToString("#,##0"));
        gvItem.Tag = (object) org;
        string valueDescription1 = this.filterMnger.GetFilterForColumn(0) == null ? "" : this.filterMnger.GetFilterForColumn(0).ValueDescription;
        if (!(valueDescription1 != "") || !((org.OrganizationName ?? "") == string.Empty) && org.OrganizationName.ToLower().Contains(valueDescription1.ToLower()))
        {
          string valueDescription2 = this.filterMnger.GetFilterForColumn(1) == null ? "" : this.filterMnger.GetFilterForColumn(1).ValueDescription;
          if (!(valueDescription2 != "") || org.ExternalID.ToLower().Contains(valueDescription2.ToLower()) && !(org.ExternalID == string.Empty))
          {
            string valueDescription3 = this.filterMnger.GetFilterForColumn(2) == null ? "" : this.filterMnger.GetFilterForColumn(2).ValueDescription;
            if (!(valueDescription3 != "") || !((org.OrgID ?? "") == string.Empty) && org.OrgID.ToLower().Contains(valueDescription3.ToLower()))
            {
              string valueDescription4 = this.filterMnger.GetFilterForColumn(3) == null ? "" : this.filterMnger.GetFilterForColumn(3).ValueDescription;
              if (!(valueDescription4 != "") || this.checkDecimalfilter(this.filterMnger.GetFilterForColumn(3).OperatorType, org.MaxCommitmentAmount, Decimal.Parse(valueDescription4)))
              {
                string valueDescription5 = this.filterMnger.GetFilterForColumn(4) == null ? "" : this.filterMnger.GetFilterForColumn(4).ValueDescription;
                if (!(valueDescription5 != "") || availableCommitment[(object) org.oid] == null || this.checkDecimalfilter(this.filterMnger.GetFilterForColumn(4).OperatorType, Decimal.Parse(availableCommitment[(object) org.oid].ToString()), Decimal.Parse(valueDescription5)))
                  this.grd3rdParties.Items.Add(gvItem);
              }
            }
          }
        }
      }
      this.grd3rdParties.EndUpdate();
    }

    private bool checkDecimalfilter(OperatorTypes op, Decimal val1, Decimal val2)
    {
      switch (op)
      {
        case OperatorTypes.Equals:
          return val1 == val2;
        case OperatorTypes.NotEqual:
          return val1 != val2;
        case OperatorTypes.GreaterThan:
          return val1 > val2;
        case OperatorTypes.NotGreaterThan:
          return val1 <= val2;
        case OperatorTypes.LessThan:
          return val1 < val2;
        case OperatorTypes.NotLessThan:
          return val1 >= val2;
        default:
          return false;
      }
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.grd3rdParties.SelectedItems.Count == 1)
        this.DialogResult = DialogResult.OK;
      else
        this.DialogResult = DialogResult.None;
    }

    private void grd3rdParties_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.grd3rdParties.SelectedItems.Count == 1;
    }

    public ExternalOriginatorManagementData SelectedOrganization
    {
      get => (ExternalOriginatorManagementData) this.grd3rdParties.SelectedItems[0].Tag;
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
      this.btnCancel.Location = new Point(874, 556);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(793, 556);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 8;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.groupList.Controls.Add((Control) this.grd3rdParties);
      this.groupList.HeaderForeColor = SystemColors.ControlText;
      this.groupList.Location = new Point(12, 12);
      this.groupList.Name = "groupList";
      this.groupList.Size = new Size(937, 530);
      this.groupList.TabIndex = 6;
      this.groupList.Text = "Third Party Originators";
      this.grd3rdParties.AllowMultiselect = false;
      this.grd3rdParties.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnLegalName";
      gvColumn1.Text = "Organization Name";
      gvColumn1.Width = 250;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnTPOId";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "TPO ID";
      gvColumn2.Width = 235;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnOrgId";
      gvColumn3.Text = "Organization ID";
      gvColumn3.Width = 150;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnMaxAuthority";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Max Commitment Authority";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 180;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnAvailableAmount";
      gvColumn5.Text = "Available Amount";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 120;
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
      this.grd3rdParties.Size = new Size(935, 503);
      this.grd3rdParties.TabIndex = 3;
      this.grd3rdParties.SelectedIndexChanged += new EventHandler(this.grd3rdParties_SelectedIndexChanged);
      this.grd3rdParties.DoubleClick += new EventHandler(this.btnSelect_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(961, 591);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupList);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TPOCompanyMasterSelectorForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Third Party Originator Selector";
      this.groupList.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
