// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TablePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TablePanel : UserControl, IOnlineHelpTarget
  {
    private ImageList imageList;
    private TablePanel.TableID tableID;
    private string containerHeader;
    private TableFeeListBase tablePur;
    private TableFeeListBase tableRef;
    private Button setDefaultBtn;
    private GridView listView;
    private Sessions.Session session;
    private GroupContainer gContainer;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private ToolTip toolTip1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton stdIconBtnDuplicate;
    private IContainer components;

    public TablePanel(TablePanel.TableID tableID)
      : this(tableID, Session.DefaultInstance)
    {
    }

    public string[] SelectedTableNames
    {
      get
      {
        return this.listView.SelectedItems.Count == 0 ? (string[]) null : this.listView.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[1].Text)).ToArray<string>();
      }
    }

    public string[] SelectedDefaultFeeNames
    {
      get
      {
        return this.listView.SelectedItems.Count == 0 ? (string[]) null : this.listView.SelectedItems.Where<GVItem>((Func<GVItem, bool>) (item => item.ImageIndex == 0)).Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[1].Text)).ToArray<string>();
      }
    }

    public TablePanel(TablePanel.TableID tableID, Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.tableID = tableID;
      this.imageList = new ImageList();
      this.imageList.Images.Add(Image.FromFile(AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.ImageRelDir, "checkmark.bmp"), SystemSettings.LocalAppDir)));
      this.listView.ImageList = this.imageList;
      switch (this.tableID)
      {
        case TablePanel.TableID.Escrow:
          this.containerHeader = "Escrow";
          this.listView.Columns.Remove(this.listView.Columns.GetColumnByName("ColumnType"));
          break;
        case TablePanel.TableID.Title:
          this.containerHeader = "Title";
          break;
      }
      this.initForm();
      this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
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
      this.setDefaultBtn = new Button();
      this.listView = new GridView();
      this.gContainer = new GroupContainer();
      this.stdIconBtnDuplicate = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.gContainer.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnDuplicate).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.setDefaultBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.setDefaultBtn.Location = new Point(614, 2);
      this.setDefaultBtn.Name = "setDefaultBtn";
      this.setDefaultBtn.Size = new Size(93, 22);
      this.setDefaultBtn.TabIndex = 4;
      this.setDefaultBtn.Text = "Set As &Default";
      this.setDefaultBtn.Click += new EventHandler(this.setDefaultBtn_Click);
      this.listView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Default";
      gvColumn1.Width = 75;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Table Name";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnPurpose";
      gvColumn3.Text = "Purpose";
      gvColumn3.Width = 80;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnType";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column3";
      gvColumn5.Text = "Calc. Based On";
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column4";
      gvColumn6.Text = "Rounding";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column5";
      gvColumn7.Text = "To Nearest";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column6";
      gvColumn8.Text = "With Offset";
      gvColumn8.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn8.Width = 100;
      this.listView.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.listView.Dock = DockStyle.Fill;
      this.listView.Location = new Point(1, 26);
      this.listView.Name = "listView";
      this.listView.Size = new Size(711, 370);
      this.listView.TabIndex = 7;
      this.listView.ItemDoubleClick += new GVItemEventHandler(this.listView_ItemDoubleClick);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDuplicate);
      this.gContainer.Controls.Add((Control) this.verticalSeparator1);
      this.gContainer.Controls.Add((Control) this.stdIconBtnEdit);
      this.gContainer.Controls.Add((Control) this.stdIconBtnNew);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDelete);
      this.gContainer.Controls.Add((Control) this.listView);
      this.gContainer.Controls.Add((Control) this.setDefaultBtn);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(713, 397);
      this.gContainer.TabIndex = 8;
      this.stdIconBtnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDuplicate.BackColor = Color.Transparent;
      this.stdIconBtnDuplicate.Location = new Point(536, 5);
      this.stdIconBtnDuplicate.Name = "stdIconBtnDuplicate";
      this.stdIconBtnDuplicate.Size = new Size(16, 16);
      this.stdIconBtnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.stdIconBtnDuplicate.TabIndex = 13;
      this.stdIconBtnDuplicate.TabStop = false;
      this.stdIconBtnDuplicate.Click += new EventHandler(this.stdIconBtnDuplicate_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(606, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 12;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(560, 5);
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 11;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.editBtn_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(512, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 10;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.newBtn_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(584, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 9;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.Controls.Add((Control) this.gContainer);
      this.Name = nameof (TablePanel);
      this.Size = new Size(713, 397);
      this.gContainer.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnDuplicate).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      switch (this.tableID)
      {
        case TablePanel.TableID.Escrow:
          this.tablePur = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblEscrowPurList));
          this.tableRef = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblEscrowRefiList));
          break;
        case TablePanel.TableID.Title:
          this.tablePur = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitlePurList));
          this.tableRef = (TableFeeListBase) this.session.GetSystemSettings(typeof (TblTitleRefiList));
          break;
      }
      this.listView.Items.Clear();
      this.listView.BeginUpdate();
      for (int index = 1; index <= 2; ++index)
      {
        TableFeeListBase tableFeeListBase = index == 1 ? this.tablePur : this.tableRef;
        for (int i = 0; i < tableFeeListBase.Count; ++i)
          this.listView.Items.Add(this.buildGVItem(tableFeeListBase.GetTableAt(i), index == 1, false));
      }
      this.listView.Sort(2, SortOrder.Ascending);
      this.listView.EndUpdate();
      this.listView_SelectedIndexChanged((object) null, (EventArgs) null);
      this.refreshListViewHeader();
    }

    private GVItem buildGVItem(TableFeeListBase.FeeTable t, bool isForPurchase, bool selected)
    {
      GVItem gvItem = new GVItem("");
      gvItem.SubItems.Add((object) t.TableName);
      gvItem.SubItems.Add(isForPurchase ? (object) "Purchase" : (object) "Refi");
      if (this.tableID == TablePanel.TableID.Title)
        gvItem.SubItems.Add(t.FeeType == string.Empty ? (object) "2009" : (object) t.FeeType);
      gvItem.SubItems.Add((object) t.CalcBasedOn);
      gvItem.SubItems.Add((object) t.Rounding);
      gvItem.SubItems.Add((object) t.Nearest);
      gvItem.SubItems.Add((object) t.Offset);
      if (t.UseThis)
        gvItem.ImageIndex = 0;
      else
        gvItem.Text = "";
      gvItem.Tag = (object) t;
      gvItem.Selected = selected;
      return gvItem;
    }

    private void updateGVItem(GVItem gvItem, TableFeeListBase.FeeTable t, bool isForPurchase)
    {
      gvItem.ImageIndex = !t.UseThis ? -1 : 0;
      int num1 = 1;
      GVSubItemCollection subItems1 = gvItem.SubItems;
      int nItemIndex1 = num1;
      int num2 = nItemIndex1 + 1;
      subItems1[nItemIndex1].Text = t.TableName;
      GVSubItemCollection subItems2 = gvItem.SubItems;
      int nItemIndex2 = num2;
      int num3 = nItemIndex2 + 1;
      subItems2[nItemIndex2].Text = isForPurchase ? "Purchase" : "Refi";
      if (this.tableID == TablePanel.TableID.Title)
        gvItem.SubItems[num3++].Text = t.FeeType == string.Empty ? "2009" : t.FeeType;
      GVSubItemCollection subItems3 = gvItem.SubItems;
      int nItemIndex3 = num3;
      int num4 = nItemIndex3 + 1;
      subItems3[nItemIndex3].Text = t.CalcBasedOn;
      GVSubItemCollection subItems4 = gvItem.SubItems;
      int nItemIndex4 = num4;
      int num5 = nItemIndex4 + 1;
      subItems4[nItemIndex4].Text = t.Rounding;
      GVSubItemCollection subItems5 = gvItem.SubItems;
      int nItemIndex5 = num5;
      int num6 = nItemIndex5 + 1;
      subItems5[nItemIndex5].Text = t.Nearest;
      GVSubItemCollection subItems6 = gvItem.SubItems;
      int nItemIndex6 = num6;
      int num7 = nItemIndex6 + 1;
      subItems6[nItemIndex6].Text = t.Offset;
      gvItem.Tag = (object) t;
    }

    private void refreshListViewHeader()
    {
      this.gContainer.Text = this.containerHeader + " (" + (object) this.listView.Items.Count + ")";
    }

    private void newBtn_Click(object sender, EventArgs e)
    {
      this.listView.SelectedItems.Clear();
      this.setTableContents((TableFeeListBase.FeeTable) null, true);
    }

    private void editSelectedItem()
    {
      if (this.listView.SelectedItems.Count == 0)
        return;
      bool isForPurchase = this.isPurchaseInListViewItem(this.listView.SelectedItems[0]);
      this.setTableContents(!isForPurchase ? this.tableRef.GetTable(this.listView.SelectedItems[0].SubItems[1].Text) : this.tablePur.GetTable(this.listView.SelectedItems[0].SubItems[1].Text), isForPurchase);
    }

    private void setTableContents(TableFeeListBase.FeeTable t, bool isForPurchase)
    {
      using (TableGroupDialog tableGroupDialog = new TableGroupDialog(this.tableID, t, isForPurchase, this.tablePur, this.tableRef, this.session))
      {
        if (tableGroupDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (t == null)
        {
          if (tableGroupDialog.IsForPurchase)
          {
            this.tablePur.InsertFee(tableGroupDialog.TableName, tableGroupDialog.UseThis, tableGroupDialog.CalcBasedOn, tableGroupDialog.Rounding, tableGroupDialog.Nearest, tableGroupDialog.Offset, tableGroupDialog.RateList, tableGroupDialog.FeeType);
            this.listView.Items.Add(this.buildGVItem(this.tablePur.GetTable(tableGroupDialog.TableName), tableGroupDialog.IsForPurchase, true));
          }
          else
          {
            this.tableRef.InsertFee(tableGroupDialog.TableName, tableGroupDialog.UseThis, tableGroupDialog.CalcBasedOn, tableGroupDialog.Rounding, tableGroupDialog.Nearest, tableGroupDialog.Offset, tableGroupDialog.RateList, tableGroupDialog.FeeType);
            this.listView.Items.Add(this.buildGVItem(this.tableRef.GetTable(tableGroupDialog.TableName), tableGroupDialog.IsForPurchase, true));
          }
          this.refreshListViewHeader();
        }
        else if (isForPurchase != tableGroupDialog.IsForPurchase)
        {
          if (isForPurchase)
            this.tablePur.RemoveTable(t.TableName);
          else
            this.tableRef.RemoveTable(t.TableName);
          this.saveTableSettings(isForPurchase);
          if (tableGroupDialog.IsForPurchase)
          {
            this.tablePur.InsertFee(tableGroupDialog.TableName, !this.tablePur.HasDefault(t.FeeType) && tableGroupDialog.UseThis, tableGroupDialog.CalcBasedOn, tableGroupDialog.Rounding, tableGroupDialog.Nearest, tableGroupDialog.Offset, tableGroupDialog.RateList, tableGroupDialog.FeeType);
            this.updateGVItem(this.listView.SelectedItems[0], this.tablePur.GetTable(tableGroupDialog.TableName), tableGroupDialog.IsForPurchase);
          }
          else
          {
            this.tableRef.InsertFee(tableGroupDialog.TableName, !this.tableRef.HasDefault(t.FeeType) && tableGroupDialog.UseThis, tableGroupDialog.CalcBasedOn, tableGroupDialog.Rounding, tableGroupDialog.Nearest, tableGroupDialog.Offset, tableGroupDialog.RateList, tableGroupDialog.FeeType);
            this.updateGVItem(this.listView.SelectedItems[0], this.tableRef.GetTable(tableGroupDialog.TableName), tableGroupDialog.IsForPurchase);
          }
        }
        else if (tableGroupDialog.IsForPurchase)
        {
          this.tablePur.UpdateFee(t.TableName, tableGroupDialog.TableName, tableGroupDialog.UseThis, tableGroupDialog.CalcBasedOn, tableGroupDialog.Rounding, tableGroupDialog.Nearest, tableGroupDialog.Offset, tableGroupDialog.RateList, tableGroupDialog.FeeType);
          this.updateGVItem(this.listView.SelectedItems[0], this.tablePur.GetTable(tableGroupDialog.TableName), tableGroupDialog.IsForPurchase);
        }
        else
        {
          this.tableRef.UpdateFee(t.TableName, tableGroupDialog.TableName, tableGroupDialog.UseThis, tableGroupDialog.CalcBasedOn, tableGroupDialog.Rounding, tableGroupDialog.Nearest, tableGroupDialog.Offset, tableGroupDialog.RateList, tableGroupDialog.FeeType);
          this.updateGVItem(this.listView.SelectedItems[0], this.tableRef.GetTable(tableGroupDialog.TableName), tableGroupDialog.IsForPurchase);
        }
        this.saveTableSettings(tableGroupDialog.IsForPurchase);
      }
    }

    private void setDefaultBtn_Click(object sender, EventArgs e)
    {
      if (this.listView.SelectedItems.Count == 0)
        return;
      string text = this.listView.SelectedItems[0].SubItems[1].Text;
      if (text == string.Empty)
        return;
      bool isForPurchase = this.isPurchaseInListViewItem(this.listView.SelectedItems[0]);
      TableFeeListBase.FeeTable feeTable = isForPurchase ? this.tablePur.GetTable(text) : this.tableRef.GetTable(text);
      if (feeTable.UseThis)
      {
        feeTable.UseThis = false;
        if (isForPurchase)
        {
          this.tablePur.UpdateFee(feeTable.TableName, feeTable.TableName, feeTable.UseThis, feeTable.CalcBasedOn, feeTable.Rounding, feeTable.Nearest, feeTable.Offset, feeTable.RateList, feeTable.FeeType);
          this.updateGVItem(this.listView.SelectedItems[0], this.tablePur.GetTable(feeTable.TableName), isForPurchase);
        }
        else
        {
          this.tableRef.UpdateFee(feeTable.TableName, feeTable.TableName, feeTable.UseThis, feeTable.CalcBasedOn, feeTable.Rounding, feeTable.Nearest, feeTable.Offset, feeTable.RateList, feeTable.FeeType);
          this.updateGVItem(this.listView.SelectedItems[0], this.tableRef.GetTable(feeTable.TableName), isForPurchase);
        }
      }
      else
      {
        if (isForPurchase)
          this.tablePur.RemoveDefaultTable(feeTable.FeeType);
        else
          this.tableRef.RemoveDefaultTable(feeTable.FeeType);
        for (int nItemIndex = 0; nItemIndex < this.listView.Items.Count; ++nItemIndex)
        {
          if (isForPurchase == this.isPurchaseInListViewItem(this.listView.Items[nItemIndex]) && this.listView.Items[nItemIndex].ImageIndex == 0)
          {
            if (this.tableID == TablePanel.TableID.Escrow || string.Compare(feeTable.FeeType, this.listView.Items[nItemIndex].SubItems[3].Text, true) == 0)
              this.listView.Items[nItemIndex].ImageIndex = -1;
            else if (this.tableID == TablePanel.TableID.Title && string.Compare(feeTable.FeeType, this.listView.Items[nItemIndex].SubItems[3].Text.Equals("2009") ? string.Empty : this.listView.Items[nItemIndex].SubItems[3].Text, true) == 0)
              this.listView.Items[nItemIndex].ImageIndex = -1;
          }
        }
        feeTable.UseThis = true;
        if (isForPurchase)
        {
          this.tablePur.UpdateFee(feeTable.TableName, feeTable.TableName, feeTable.UseThis, feeTable.CalcBasedOn, feeTable.Rounding, feeTable.Nearest, feeTable.Offset, feeTable.RateList, feeTable.FeeType);
          this.updateGVItem(this.listView.SelectedItems[0], this.tablePur.GetTable(feeTable.TableName), isForPurchase);
        }
        else
        {
          this.tableRef.UpdateFee(feeTable.TableName, feeTable.TableName, feeTable.UseThis, feeTable.CalcBasedOn, feeTable.Rounding, feeTable.Nearest, feeTable.Offset, feeTable.RateList, feeTable.FeeType);
          this.updateGVItem(this.listView.SelectedItems[0], this.tableRef.GetTable(feeTable.TableName), isForPurchase);
        }
      }
      this.saveTableSettings(isForPurchase);
    }

    private void saveTableSettings(bool isForPurchase)
    {
      if (this.tableID == TablePanel.TableID.Escrow)
      {
        if (isForPurchase)
          this.session.SaveSystemSettings((object) (TblEscrowPurList) this.tablePur);
        else
          this.session.SaveSystemSettings((object) (TblEscrowRefiList) this.tableRef);
      }
      else if (isForPurchase)
        this.session.SaveSystemSettings((object) (TblTitlePurList) this.tablePur);
      else
        this.session.SaveSystemSettings((object) (TblTitleRefiList) this.tableRef);
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      if (this.listView.SelectedItems.Count == 0 || Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected item(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      int index = this.listView.SelectedItems[0].Index;
      int num = this.listView.Items.Count - 1;
      bool flag1 = false;
      bool flag2 = false;
      for (int nItemIndex = num; nItemIndex >= 0; --nItemIndex)
      {
        if (this.listView.Items[nItemIndex].Selected)
        {
          if (this.isPurchaseInListViewItem(this.listView.Items[nItemIndex]))
          {
            this.tablePur.RemoveTable(this.listView.Items[nItemIndex].SubItems[1].Text);
            flag1 = true;
          }
          else
          {
            this.tableRef.RemoveTable(this.listView.Items[nItemIndex].SubItems[1].Text);
            flag2 = true;
          }
          this.listView.Items.RemoveAt(nItemIndex);
        }
      }
      if (flag1)
        this.saveTableSettings(true);
      if (flag2)
        this.saveTableSettings(false);
      this.refreshListViewHeader();
      if (this.listView.Items.Count == 0)
        return;
      if (index > this.listView.Items.Count - 1)
        this.listView.Items[this.listView.Items.Count - 1].Selected = true;
      else
        this.listView.Items[index].Selected = true;
    }

    private bool isPurchaseInListViewItem(GVItem item)
    {
      return string.Compare(item.SubItems[2].Text, "Purchase", true) == 0;
    }

    private void listView_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void listView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.setDefaultBtn.Enabled = this.stdIconBtnDuplicate.Enabled = this.listView.SelectedItems.Count == 1;
      this.stdIconBtnDelete.Enabled = this.listView.SelectedItems.Count > 0;
    }

    private void editBtn_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void stdIconBtnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.listView.SelectedItems.Count == 0)
        return;
      bool isForPurchase = this.isPurchaseInListViewItem(this.listView.SelectedItems[0]);
      TableFeeListBase.FeeTable currentFee = (TableFeeListBase.FeeTable) (!isForPurchase ? this.tableRef.GetTable(this.listView.SelectedItems[0].SubItems[1].Text) : this.tablePur.GetTable(this.listView.SelectedItems[0].SubItems[1].Text)).Clone();
      currentFee.UseThis = false;
      currentFee.TableName = string.Empty;
      using (TableGroupDialog tableGroupDialog = new TableGroupDialog(this.tableID, currentFee, isForPurchase, this.tablePur, this.tableRef, this.session))
      {
        if (tableGroupDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.listView.SelectedItems.Clear();
        if (tableGroupDialog.IsForPurchase)
        {
          this.tablePur.InsertFee(tableGroupDialog.TableName, tableGroupDialog.UseThis, tableGroupDialog.CalcBasedOn, tableGroupDialog.Rounding, tableGroupDialog.Nearest, tableGroupDialog.Offset, tableGroupDialog.RateList, tableGroupDialog.FeeType);
          this.listView.Items.Add(this.buildGVItem(this.tablePur.GetTable(tableGroupDialog.TableName), tableGroupDialog.IsForPurchase, true));
        }
        else
        {
          this.tableRef.InsertFee(tableGroupDialog.TableName, tableGroupDialog.UseThis, tableGroupDialog.CalcBasedOn, tableGroupDialog.Rounding, tableGroupDialog.Nearest, tableGroupDialog.Offset, tableGroupDialog.RateList, tableGroupDialog.FeeType);
          this.listView.Items.Add(this.buildGVItem(this.tableRef.GetTable(tableGroupDialog.TableName), tableGroupDialog.IsForPurchase, true));
        }
        this.saveTableSettings(tableGroupDialog.IsForPurchase);
        this.refreshListViewHeader();
      }
    }

    public void SetSelectedTableNames(List<string> selectedTableNames)
    {
      foreach (GVItem gvItem in this.listView.Items.Where<GVItem>((Func<GVItem, bool>) (item => selectedTableNames.Contains(item.SubItems[1].Text))))
        gvItem.Selected = true;
    }

    string IOnlineHelpTarget.GetHelpTargetName()
    {
      return this.tableID != TablePanel.TableID.Escrow ? "Setup\\Title" : "Setup\\Escrow";
    }

    public enum TableID
    {
      EscrowPurchase,
      EscrowRefi,
      Escrow,
      TitlePurchase,
      TitleRefi,
      Title,
    }
  }
}
