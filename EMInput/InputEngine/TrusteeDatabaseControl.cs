// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TrusteeDatabaseControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class TrusteeDatabaseControl : UserControl
  {
    private IWin32Window owner;
    private IContainer components;
    private GridView gridViewContacts;
    private ToolTip fieldToolTip;
    private Label label7;
    private TextBox txtBoxTrusteeName;
    private Button btnSearch;
    private GradientPanel pnlExFind;
    private GroupContainer gcTrustees;
    private StandardIconButton stdIconBtnDelete;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnEdit;

    public event EventHandler OnSelect;

    public event EventHandler OnSelectedItemCountChanged;

    public TrusteeDatabaseControl(IWin32Window owner, TrusteeRecord rec, bool multiSelect)
    {
      this.owner = owner;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.gridViewContacts.AllowMultiselect = multiSelect;
      this.initForm();
      if (rec != null)
      {
        bool flag = false;
        string lower = rec.ContactName.ToLower();
        for (int nItemIndex = 0; nItemIndex < this.gridViewContacts.Items.Count; ++nItemIndex)
        {
          if (lower == this.gridViewContacts.Items[nItemIndex].Text.ToLower())
          {
            flag = true;
            this.gridViewContacts.Items[nItemIndex].Selected = true;
            break;
          }
        }
        if (!flag && Utils.Dialog((IWin32Window) this, "This Trust Name '" + lower + "' does not exist in the Trustee List. Would you like to add it to list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          rec.Id = Session.ConfigurationManager.AddTrusteeRecord(rec);
          if (rec.Id > -1)
            this.gridViewContacts.Items.Add(this.buildListViewItem(rec, true));
        }
      }
      this.gridViewContacts.Sort(0, SortOrder.Ascending);
      this.listViewContacts_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void initForm()
    {
      this.gridViewContacts.Items.Clear();
      TrusteeRecord[] trusteeRecords = Session.ConfigurationManager.GetTrusteeRecords();
      if (trusteeRecords == null || trusteeRecords.Length == 0)
        return;
      this.gridViewContacts.BeginUpdate();
      for (int index = 0; index < trusteeRecords.Length; ++index)
        this.gridViewContacts.Items.Add(this.buildListViewItem(trusteeRecords[index], false));
      this.gridViewContacts.EndUpdate();
      this.setTitle();
    }

    private void setTitle()
    {
      this.gcTrustees.Text = "Trustees (" + (object) this.gridViewContacts.Items.Count + ")";
    }

    public TrusteeRecord SelectedTrustee
    {
      get
      {
        return this.gridViewContacts.SelectedItems.Count == 0 ? (TrusteeRecord) null : (TrusteeRecord) this.gridViewContacts.SelectedItems[0].Tag;
      }
    }

    public int SelectedItemCount => this.gridViewContacts.SelectedItems.Count;

    private void edit()
    {
      if (this.gridViewContacts.SelectedItems.Count != 1)
      {
        int num = (int) Utils.Dialog(this.owner, "Please select one trustee to edit.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        TrusteeDetailsEditor trusteeDetailsEditor = new TrusteeDetailsEditor((TrusteeRecord) this.gridViewContacts.SelectedItems[0].Tag);
        if (trusteeDetailsEditor.ShowDialog(this.owner) == DialogResult.Cancel)
          return;
        Session.ConfigurationManager.UpdateTrusteeRecord(trusteeDetailsEditor.TrusteeRecord);
        this.updateListViewItem(trusteeDetailsEditor.TrusteeRecord, this.gridViewContacts.SelectedItems[0]);
      }
    }

    private void listViewContacts_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (this.OnSelect != null)
        this.OnSelect(source, (EventArgs) e);
      else
        this.edit();
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e) => this.edit();

    private void btnNew_Click(object sender, EventArgs e)
    {
      TrusteeDetailsEditor trusteeDetailsEditor = new TrusteeDetailsEditor((TrusteeRecord) null);
      if (trusteeDetailsEditor.ShowDialog(this.owner) == DialogResult.Cancel)
        return;
      Session.ConfigurationManager.AddTrusteeRecord(trusteeDetailsEditor.TrusteeRecord);
      this.gridViewContacts.Items.Add(this.buildListViewItem(trusteeDetailsEditor.TrusteeRecord, true));
      this.setTitle();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewContacts.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a record to delete.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Do you want to delete the selected record(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        GVItem[] gvItemArray = new GVItem[this.gridViewContacts.SelectedItems.Count];
        int[] ids = new int[this.gridViewContacts.SelectedItems.Count];
        for (int index = 0; index < this.gridViewContacts.SelectedItems.Count; ++index)
        {
          TrusteeRecord tag = (TrusteeRecord) this.gridViewContacts.SelectedItems[index].Tag;
          ids[index] = tag.Id;
          gvItemArray[index] = this.gridViewContacts.SelectedItems[index];
        }
        Session.ConfigurationManager.DeleteTrusteeRecord(ids);
        for (int index = 0; index < gvItemArray.Length; ++index)
          this.gridViewContacts.Items.Remove(gvItemArray[index]);
        this.setTitle();
      }
    }

    private void btnSearch_Click(object sender, EventArgs e) => this.find(true);

    private void txtBoxTrusteeName_TextChanged(object sender, EventArgs e)
    {
      if (this.txtBoxTrusteeName.Text.Length <= 0)
        return;
      this.find(false);
    }

    private void find(bool findNext)
    {
      string lower = this.txtBoxTrusteeName.Text.ToLower();
      int nItemIndex = findNext ? -1 : 0;
      if (this.gridViewContacts.SelectedItems.Count > 0)
        nItemIndex = this.gridViewContacts.SelectedItems[0].Index;
      for (int index = 0; index < this.gridViewContacts.Items.Count; ++index)
      {
        if (findNext)
        {
          ++nItemIndex;
          if (nItemIndex >= this.gridViewContacts.Items.Count)
            nItemIndex = 0;
        }
        if (this.gridViewContacts.Items[nItemIndex].Text.ToLower().IndexOf(lower) > -1)
        {
          foreach (GVItem selectedItem in this.gridViewContacts.SelectedItems)
            selectedItem.Selected = false;
          this.gridViewContacts.Items[nItemIndex].Selected = true;
          break;
        }
        if (!findNext)
        {
          ++nItemIndex;
          if (nItemIndex >= this.gridViewContacts.Items.Count)
            nItemIndex = 0;
        }
      }
    }

    private GVItem buildListViewItem(TrusteeRecord rec, bool selected)
    {
      GVItem gvItem = new GVItem(rec.ContactName);
      gvItem.SubItems.Add((object) rec.Address);
      gvItem.SubItems.Add((object) rec.City);
      gvItem.SubItems.Add((object) rec.State);
      gvItem.SubItems.Add((object) rec.ZipCode);
      if (string.IsNullOrEmpty(rec.County) && !string.IsNullOrEmpty(rec.ZipCode) && rec.ZipCode.Length >= 5)
      {
        ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(rec.ZipCode.Trim().Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(rec.ZipCode.Trim().Substring(0, 5)), true);
        if (zipCodeInfo != null)
          gvItem.SubItems.Add((object) Utils.CapsConvert(zipCodeInfo.County, false));
        else
          gvItem.SubItems.Add((object) "");
      }
      else
        gvItem.SubItems.Add((object) (rec.County ?? ""));
      gvItem.SubItems.Add((object) rec.Phone);
      if (rec.TrustDate == DateTime.MinValue)
        gvItem.SubItems.Add((object) "");
      else
        gvItem.SubItems.Add((object) rec.TrustDate.ToString("MM/dd/yyyy"));
      gvItem.SubItems.Add((object) rec.OrgState);
      gvItem.SubItems.Add((object) rec.OrgType);
      gvItem.Tag = (object) rec;
      gvItem.Selected = selected;
      return gvItem;
    }

    private void updateListViewItem(TrusteeRecord rec, GVItem item)
    {
      item.Text = rec.ContactName;
      item.SubItems[1].Text = rec.Address;
      item.SubItems[2].Text = rec.City;
      item.SubItems[3].Text = rec.State;
      item.SubItems[4].Text = rec.ZipCode;
      item.SubItems[5].Text = rec.County;
      item.SubItems[6].Text = rec.Phone;
      item.SubItems[7].Text = !(rec.TrustDate == DateTime.MinValue) ? rec.TrustDate.ToString("MM/dd/yyyy") : "";
      item.SubItems[8].Text = rec.OrgState;
      item.SubItems[9].Text = rec.OrgType;
      item.Tag = (object) rec;
    }

    private void listViewContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDelete.Enabled = this.gridViewContacts.SelectedItems.Count > 0;
      this.stdIconBtnEdit.Enabled = this.gridViewContacts.SelectedItems.Count == 1;
      if (this.OnSelectedItemCountChanged == null)
        return;
      this.OnSelectedItemCountChanged(sender, e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
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
      this.gridViewContacts = new GridView();
      this.fieldToolTip = new ToolTip(this.components);
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.label7 = new Label();
      this.txtBoxTrusteeName = new TextBox();
      this.btnSearch = new Button();
      this.pnlExFind = new GradientPanel();
      this.gcTrustees = new GroupContainer();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      this.pnlExFind.SuspendLayout();
      this.gcTrustees.SuspendLayout();
      this.SuspendLayout();
      this.gridViewContacts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Trustee Name";
      gvColumn1.Width = 134;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Address";
      gvColumn2.Width = 198;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "City";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "State";
      gvColumn4.Width = 45;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Zip";
      gvColumn5.Width = 68;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column10";
      gvColumn6.Text = "County";
      gvColumn6.Width = 160;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column9";
      gvColumn7.Text = "Phone";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column6";
      gvColumn8.Text = "Trust Date";
      gvColumn8.Width = 70;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column7";
      gvColumn9.Text = "Org. State";
      gvColumn9.Width = 108;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column8";
      gvColumn10.Text = "Org. Type";
      gvColumn10.Width = 120;
      this.gridViewContacts.Columns.AddRange(new GVColumn[10]
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
        gvColumn10
      });
      this.gridViewContacts.Dock = DockStyle.Fill;
      this.gridViewContacts.Location = new Point(1, 26);
      this.gridViewContacts.Name = "gridViewContacts";
      this.gridViewContacts.Size = new Size(912, 523);
      this.gridViewContacts.TabIndex = 9;
      this.gridViewContacts.SelectedIndexChanged += new EventHandler(this.listViewContacts_SelectedIndexChanged);
      this.gridViewContacts.ItemDoubleClick += new GVItemEventHandler(this.listViewContacts_ItemDoubleClick);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(891, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 17);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 16;
      this.stdIconBtnDelete.TabStop = false;
      this.fieldToolTip.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(847, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 17);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 15;
      this.stdIconBtnNew.TabStop = false;
      this.fieldToolTip.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.btnNew_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(869, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 17);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 19;
      this.stdIconBtnEdit.TabStop = false;
      this.fieldToolTip.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(8, 10);
      this.label7.Name = "label7";
      this.label7.Size = new Size(97, 14);
      this.label7.TabIndex = 32;
      this.label7.Text = "Find Trustee Name";
      this.txtBoxTrusteeName.Location = new Point(113, 7);
      this.txtBoxTrusteeName.MaxLength = 256;
      this.txtBoxTrusteeName.Name = "txtBoxTrusteeName";
      this.txtBoxTrusteeName.Size = new Size(190, 20);
      this.txtBoxTrusteeName.TabIndex = 33;
      this.txtBoxTrusteeName.TextChanged += new EventHandler(this.txtBoxTrusteeName_TextChanged);
      this.btnSearch.BackColor = SystemColors.Control;
      this.btnSearch.Location = new Point(307, 6);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new Size(75, 22);
      this.btnSearch.TabIndex = 34;
      this.btnSearch.Text = "Find Next";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new EventHandler(this.btnSearch_Click);
      this.pnlExFind.BackColor = Color.WhiteSmoke;
      this.pnlExFind.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlExFind.Controls.Add((Control) this.btnSearch);
      this.pnlExFind.Controls.Add((Control) this.label7);
      this.pnlExFind.Controls.Add((Control) this.txtBoxTrusteeName);
      this.pnlExFind.Dock = DockStyle.Top;
      this.pnlExFind.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlExFind.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlExFind.Location = new Point(0, 0);
      this.pnlExFind.Name = "pnlExFind";
      this.pnlExFind.Size = new Size(914, 34);
      this.pnlExFind.TabIndex = 35;
      this.gcTrustees.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcTrustees.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcTrustees.Controls.Add((Control) this.stdIconBtnNew);
      this.gcTrustees.Controls.Add((Control) this.gridViewContacts);
      this.gcTrustees.Dock = DockStyle.Fill;
      this.gcTrustees.HeaderForeColor = SystemColors.ControlText;
      this.gcTrustees.Location = new Point(0, 34);
      this.gcTrustees.Name = "gcTrustees";
      this.gcTrustees.Size = new Size(914, 550);
      this.gcTrustees.TabIndex = 37;
      this.gcTrustees.Text = "Trustees (0)";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcTrustees);
      this.Controls.Add((Control) this.pnlExFind);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (TrusteeDatabaseControl);
      this.Size = new Size(914, 584);
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      this.pnlExFind.ResumeLayout(false);
      this.pnlExFind.PerformLayout();
      this.gcTrustees.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
