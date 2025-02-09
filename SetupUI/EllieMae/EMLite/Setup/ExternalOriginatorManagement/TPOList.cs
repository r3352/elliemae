// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.TPOList
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Properties;
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
  public class TPOList : Form
  {
    public List<ExternalOriginatorManagementData> selectedCompanies = new List<ExternalOriginatorManagementData>();
    private List<ExternalOriginatorManagementData> allCompanies;
    private List<ExternalOriginatorManagementData> assignedCompanies;
    private bool isDirty;
    private IContainer components;
    private Button btnOK;
    private Button btnCancel;
    private Button btnAdd;
    private Button btnRemove;
    private Button btnReset;
    private GroupContainer groupContainer1;
    private GridView chooseTpoGridView;
    private GroupContainer groupContainer2;
    private GridView selectedTpoGridView;

    public TPOList(
      List<ExternalOriginatorManagementData> allCompanies,
      List<ExternalOriginatorManagementData> assignedCompanies)
    {
      this.assignedCompanies = assignedCompanies;
      this.allCompanies = allCompanies;
      this.InitializeComponent();
      this.ResetGridViewData();
    }

    private void ResetGridViewData()
    {
      this.chooseTpoGridView.Items.Clear();
      this.selectedTpoGridView.Items.Clear();
      this.isDirty = false;
      foreach (ExternalOriginatorManagementData allCompany in this.allCompanies)
      {
        ExternalOriginatorManagementData company = allCompany;
        if (!this.assignedCompanies.Any<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (s => string.Equals(s.OrganizationName, company.OrganizationName, StringComparison.InvariantCultureIgnoreCase))))
          this.chooseTpoGridView.Items.Add(new GVItem()
          {
            SubItems = {
              (object) company.OrganizationName
            },
            Tag = (object) company
          });
      }
      foreach (ExternalOriginatorManagementData assignedCompany in this.assignedCompanies)
        this.selectedTpoGridView.Items.Add(new GVItem()
        {
          SubItems = {
            (object) assignedCompany.OrganizationName
          },
          Tag = (object) assignedCompany
        });
      this.chooseTpoGridView.ReSort();
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      this.ResetGridViewData();
      this.btnReset.Enabled = false;
      this.btnOK.Enabled = false;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.selectedTpoGridView.Items)
        this.selectedCompanies.Add((ExternalOriginatorManagementData) gvItem.Tag);
      this.isDirty = false;
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnAdd_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.chooseTpoGridView.SelectedItems)
      {
        ExternalOriginatorManagementData tag = (ExternalOriginatorManagementData) selectedItem.Tag;
        this.selectedTpoGridView.Items.Add(new GVItem()
        {
          SubItems = {
            (object) tag.OrganizationName
          },
          Tag = (object) tag
        });
        this.chooseTpoGridView.Items.Remove(selectedItem);
      }
      this.chooseTpoGridView.ReSort();
      this.EnableActions();
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.selectedTpoGridView.SelectedItems)
      {
        ExternalOriginatorManagementData tag = (ExternalOriginatorManagementData) selectedItem.Tag;
        this.chooseTpoGridView.Items.Add(new GVItem()
        {
          SubItems = {
            (object) tag.OrganizationName
          },
          Tag = (object) tag
        });
        this.selectedTpoGridView.Items.Remove(selectedItem);
      }
      this.chooseTpoGridView.ReSort();
      this.EnableActions();
    }

    private void chooseTpoGridView_SelectionChanged(object sender, EventArgs e)
    {
      Button btnAdd = this.btnAdd;
      GVSelectedItemCollection selectedItems1 = this.chooseTpoGridView.SelectedItems;
      // ISSUE: explicit non-virtual call
      int num = selectedItems1 != null ? (__nonvirtual (selectedItems1.Count) > 0 ? 1 : 0) : 0;
      btnAdd.Enabled = num != 0;
      if (!this.btnAdd.Enabled)
        return;
      GVSelectedItemCollection selectedItems2 = this.selectedTpoGridView.SelectedItems;
      // ISSUE: explicit non-virtual call
      if ((selectedItems2 != null ? (__nonvirtual (selectedItems2.Count) > 0 ? 1 : 0) : 0) == 0)
        return;
      this.selectedTpoGridView.SelectedItems.Clear();
    }

    private void selectedTpoGridView_SelectionChanged(object sender, EventArgs e)
    {
      Button btnRemove = this.btnRemove;
      GVSelectedItemCollection selectedItems1 = this.selectedTpoGridView.SelectedItems;
      // ISSUE: explicit non-virtual call
      int num = selectedItems1 != null ? (__nonvirtual (selectedItems1.Count) > 0 ? 1 : 0) : 0;
      btnRemove.Enabled = num != 0;
      if (!this.btnRemove.Enabled)
        return;
      GVSelectedItemCollection selectedItems2 = this.chooseTpoGridView.SelectedItems;
      // ISSUE: explicit non-virtual call
      if ((selectedItems2 != null ? (__nonvirtual (selectedItems2.Count) > 0 ? 1 : 0) : 0) == 0)
        return;
      this.chooseTpoGridView.SelectedItems.Clear();
    }

    private void EnableActions()
    {
      this.btnReset.Enabled = true;
      this.btnOK.Enabled = true;
      this.isDirty = true;
    }

    private void TPOList_Closing(object sender, FormClosingEventArgs e)
    {
      if (!this.isDirty || MessageBox.Show("There are unsaved changes to the TPO List", "Encompass", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.Cancel)
        return;
      e.Cancel = true;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (TPOList));
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.btnAdd = new Button();
      this.btnRemove = new Button();
      this.btnReset = new Button();
      this.groupContainer1 = new GroupContainer();
      this.chooseTpoGridView = new GridView();
      this.groupContainer2 = new GroupContainer();
      this.selectedTpoGridView = new GridView();
      this.groupContainer1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Enabled = false;
      this.btnOK.Location = new Point(734, 556);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "Update";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(816, 556);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnAdd.Enabled = false;
      this.btnAdd.Image = (Image) Resources.arrow_forward;
      this.btnAdd.Location = new Point(409, 234);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 23);
      this.btnAdd.TabIndex = 5;
      this.btnAdd.Text = "Add";
      this.btnAdd.TextAlign = ContentAlignment.MiddleRight;
      this.btnAdd.TextImageRelation = TextImageRelation.TextBeforeImage;
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnRemove.Enabled = false;
      this.btnRemove.Image = (Image) Resources.arrow_back;
      this.btnRemove.Location = new Point(409, 263);
      this.btnRemove.Name = "btnRemove";
      this.btnRemove.Size = new Size(75, 23);
      this.btnRemove.TabIndex = 6;
      this.btnRemove.Text = "Remove";
      this.btnRemove.TextAlign = ContentAlignment.MiddleRight;
      this.btnRemove.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.btnRemove.UseVisualStyleBackColor = true;
      this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
      this.btnReset.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnReset.Enabled = false;
      this.btnReset.Location = new Point(652, 556);
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(75, 23);
      this.btnReset.TabIndex = 7;
      this.btnReset.Text = "Reset";
      this.btnReset.UseVisualStyleBackColor = true;
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.groupContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer1.Controls.Add((Control) this.chooseTpoGridView);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(4, 4);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(390, 536);
      this.groupContainer1.TabIndex = 10;
      this.groupContainer1.Text = "Choose TPO Companies";
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Company Name";
      gvColumn1.Width = 350;
      this.chooseTpoGridView.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.chooseTpoGridView.Dock = DockStyle.Fill;
      this.chooseTpoGridView.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chooseTpoGridView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.chooseTpoGridView.Location = new Point(1, 26);
      this.chooseTpoGridView.Name = "chooseTpoGridView";
      this.chooseTpoGridView.Size = new Size(388, 509);
      this.chooseTpoGridView.TabIndex = 0;
      this.chooseTpoGridView.SelectedIndexChanged += new EventHandler(this.chooseTpoGridView_SelectionChanged);
      this.groupContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.selectedTpoGridView);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(500, 4);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(399, 536);
      this.groupContainer2.TabIndex = 11;
      this.groupContainer2.Text = "Selected TPO Companies";
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "Company Name";
      gvColumn2.Width = 350;
      this.selectedTpoGridView.Columns.AddRange(new GVColumn[1]
      {
        gvColumn2
      });
      this.selectedTpoGridView.Dock = DockStyle.Fill;
      this.selectedTpoGridView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.selectedTpoGridView.Location = new Point(1, 26);
      this.selectedTpoGridView.Name = "selectedTpoGridView";
      this.selectedTpoGridView.Size = new Size(397, 509);
      this.selectedTpoGridView.TabIndex = 0;
      this.selectedTpoGridView.SelectedIndexChanged += new EventHandler(this.selectedTpoGridView_SelectionChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(903, 591);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.btnReset);
      this.Controls.Add((Control) this.btnRemove);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (TPOList);
      this.Text = "TPO List";
      this.FormClosing += new FormClosingEventHandler(this.TPOList_Closing);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
