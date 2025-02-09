// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.StateLicenseSelectorForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
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
  public class StateLicenseSelectorForm : Form
  {
    private IContainer components;
    private GroupContainer grpLicenseList;
    private GridView gridViewStates;
    private Button btnCancel;
    private Button btnSelect;

    public StateLicenseSelectorForm(List<StateLicenseExtType> licenses, bool translateLicenseType)
    {
      this.InitializeComponent();
      bool flag = false;
      for (int index = 0; index < licenses.Count; ++index)
      {
        if (!(licenses[index].LicenseType == string.Empty) && !(licenses[index].StateAbbrevation == "GU") && !(licenses[index].StateAbbrevation == "PR") && !(licenses[index].StateAbbrevation == "VI"))
        {
          GVItem gvItem = new GVItem();
          gvItem.SubItems[0].Value = (object) null;
          if (licenses[index].Selected)
            gvItem.SubItems[0].Checked = true;
          gvItem.SubItems[0].CheckBoxEnabled = false;
          gvItem.SubItems[1].Value = (object) null;
          gvItem.SubItems[1].Checked = licenses[index].Exempt;
          gvItem.SubItems[1].CheckBoxEnabled = false;
          gvItem.SubItems[1].CheckBoxVisible = licenses[index].Approved;
          gvItem.SubItems.Add((object) licenses[index].StateAbbrevation);
          gvItem.SubItems.Add(translateLicenseType ? (object) MaventLicenseTypesUtils.GetLicenseName(licenses[index].LicenseType) : (object) licenses[index].LicenseType);
          gvItem.SubItems.Add((object) licenses[index].LicenseNo);
          GVSubItemCollection subItems1 = gvItem.SubItems;
          DateTime dateTime;
          string str1;
          if (!(licenses[index].IssueDate != DateTime.MinValue))
          {
            str1 = "";
          }
          else
          {
            dateTime = licenses[index].IssueDate;
            str1 = dateTime.ToString("d");
          }
          subItems1.Add((object) str1);
          GVSubItemCollection subItems2 = gvItem.SubItems;
          string str2;
          if (!(licenses[index].StartDate != DateTime.MinValue))
          {
            str2 = "";
          }
          else
          {
            dateTime = licenses[index].StartDate;
            str2 = dateTime.ToString("d");
          }
          subItems2.Add((object) str2);
          GVSubItemCollection subItems3 = gvItem.SubItems;
          string str3;
          if (!(licenses[index].EndDate != DateTime.MinValue))
          {
            str3 = "";
          }
          else
          {
            dateTime = licenses[index].EndDate;
            str3 = dateTime.ToString("d");
          }
          subItems3.Add((object) str3);
          gvItem.SubItems.Add((object) licenses[index].LicenseStatus);
          GVSubItemCollection subItems4 = gvItem.SubItems;
          string str4;
          if (!(licenses[index].StatusDate != DateTime.MinValue))
          {
            str4 = "";
          }
          else
          {
            dateTime = licenses[index].StatusDate;
            str4 = dateTime.ToString("d");
          }
          subItems4.Add((object) str4);
          GVSubItemCollection subItems5 = gvItem.SubItems;
          string str5;
          if (!(licenses[index].LastChecked != DateTime.MinValue))
          {
            str5 = "";
          }
          else
          {
            dateTime = licenses[index].LastChecked;
            str5 = dateTime.ToString("d");
          }
          subItems5.Add((object) str5);
          gvItem.Tag = (object) licenses[index];
          gvItem.Selected = licenses[index].Selected && !flag;
          if (licenses[index].Selected && !flag)
            flag = true;
          this.gridViewStates.Items.Add(gvItem);
        }
      }
      this.gridViewStates_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void btnSelect_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void gridViewStates_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.gridViewStates.SelectedItems.Count == 1;
    }

    public StateLicenseExtType SelectedLicense
    {
      get => (StateLicenseExtType) this.gridViewStates.SelectedItems[0].Tag;
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
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      this.btnCancel = new Button();
      this.btnSelect = new Button();
      this.grpLicenseList = new GroupContainer();
      this.gridViewStates = new GridView();
      this.grpLicenseList.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(628, 311);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(547, 311);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 5;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.grpLicenseList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpLicenseList.Controls.Add((Control) this.gridViewStates);
      this.grpLicenseList.HeaderForeColor = SystemColors.ControlText;
      this.grpLicenseList.Location = new Point(8, 8);
      this.grpLicenseList.Name = "grpLicenseList";
      this.grpLicenseList.Size = new Size(695, 293);
      this.grpLicenseList.TabIndex = 3;
      this.gridViewStates.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnSelect";
      gvColumn1.Text = "Select";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 60;
      gvColumn2.CheckBoxes = true;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnExempt";
      gvColumn2.Text = "Exempt";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 50;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnState";
      gvColumn3.Text = "State";
      gvColumn3.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn3.Width = 50;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnType";
      gvColumn4.Text = "License Type";
      gvColumn4.Width = 200;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnLicenseNo";
      gvColumn5.Text = "License #";
      gvColumn5.Width = 150;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnIssueDate";
      gvColumn6.Text = "Issue Date";
      gvColumn6.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn6.Width = 80;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnStartDate";
      gvColumn7.Text = "Start Date";
      gvColumn7.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn7.Width = 80;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnEndDate";
      gvColumn8.Text = "End Date";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 80;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnStatus";
      gvColumn9.Text = "Status";
      gvColumn9.Width = 250;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "ColumnStatusDate";
      gvColumn10.Text = "Status Date";
      gvColumn10.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn10.Width = 80;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "ColumnLastCheck";
      gvColumn11.Text = "Last Checked";
      gvColumn11.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn11.Width = 80;
      this.gridViewStates.Columns.AddRange(new GVColumn[11]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11
      });
      this.gridViewStates.Dock = DockStyle.Fill;
      this.gridViewStates.Location = new Point(1, 26);
      this.gridViewStates.Name = "gridViewStates";
      this.gridViewStates.Size = new Size(693, 266);
      this.gridViewStates.SortIconVisible = false;
      this.gridViewStates.SortOption = GVSortOption.None;
      this.gridViewStates.TabIndex = 2;
      this.gridViewStates.SelectedIndexChanged += new EventHandler(this.gridViewStates_SelectedIndexChanged);
      this.gridViewStates.DoubleClick += new EventHandler(this.btnSelect_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(715, 342);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.grpLicenseList);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (StateLicenseSelectorForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "State License Selection";
      this.grpLicenseList.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
