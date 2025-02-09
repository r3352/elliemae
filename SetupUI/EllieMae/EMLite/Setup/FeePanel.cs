// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FeePanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
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
  public class FeePanel : UserControl
  {
    private FeePanel.FeeType feeType;
    private string header;
    private FeeListBase feeList;
    private IContainer components;
    private GridView listView;
    private ToolTip toolTip1;
    private GroupContainer gContainer;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private Sessions.Session session;

    public string[] SelectedFeeNames
    {
      get
      {
        return this.listView.SelectedItems.Count == 0 ? (string[]) null : this.listView.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[0].Text)).ToArray<string>();
      }
    }

    public FeePanel(FeePanel.FeeType feeType)
      : this(feeType, Session.DefaultInstance, false)
    {
    }

    public FeePanel(FeePanel.FeeType feeType, Sessions.Session session, bool allowMultiSelect)
    {
      this.InitializeComponent();
      this.session = session;
      this.feeType = feeType;
      switch (feeType)
      {
        case FeePanel.FeeType.City:
          this.header = "City Tax";
          break;
        case FeePanel.FeeType.State:
          this.header = "State Tax";
          break;
        case FeePanel.FeeType.UserDefined:
          this.header = "User Defined Fee";
          break;
      }
      this.refreshFeeList();
      this.listView.Sort(0, SortOrder.Ascending);
      this.listView.AllowMultiselect = allowMultiSelect;
      this.setContainerHeader();
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
      this.listView = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.gContainer = new GroupContainer();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      this.gContainer.SuspendLayout();
      this.SuspendLayout();
      this.listView.AllowMultiselect = false;
      this.listView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Fee Name";
      gvColumn1.Width = 214;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Calc. Based On";
      gvColumn2.Width = 103;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Rate";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 112;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Additional Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 142;
      this.listView.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listView.Dock = DockStyle.Fill;
      this.listView.Location = new Point(1, 26);
      this.listView.Name = "listView";
      this.listView.Size = new Size(599, 386);
      this.listView.TabIndex = 7;
      this.listView.ItemDoubleClick += new GVItemEventHandler(this.listView_ItemDoubleClick);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(579, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 8;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.deleteBtn_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(535, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 9;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.newBtn_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(557, 5);
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 10;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.editBtn_Click);
      this.gContainer.Controls.Add((Control) this.stdIconBtnEdit);
      this.gContainer.Controls.Add((Control) this.stdIconBtnNew);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDelete);
      this.gContainer.Controls.Add((Control) this.listView);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(601, 413);
      this.gContainer.TabIndex = 8;
      this.Controls.Add((Control) this.gContainer);
      this.Name = nameof (FeePanel);
      this.Size = new Size(601, 413);
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      this.gContainer.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void setContainerHeader()
    {
      this.gContainer.Text = this.header + " (" + (object) this.listView.Items.Count + ")";
    }

    private void listView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.stdIconBtnDelete.Enabled = this.listView.SelectedItems.Count == 1;
    }

    private void refreshFeeList()
    {
      this.listView.Items.Clear();
      switch (this.feeType)
      {
        case FeePanel.FeeType.City:
          this.feeList = (FeeListBase) this.session.GetSystemSettings(typeof (FeeCityList));
          break;
        case FeePanel.FeeType.State:
          this.feeList = (FeeListBase) this.session.GetSystemSettings(typeof (FeeStateList));
          break;
        case FeePanel.FeeType.UserDefined:
          this.feeList = (FeeListBase) this.session.GetSystemSettings(typeof (FeeUserList));
          break;
      }
      if (this.feeList == null)
        return;
      this.listView.BeginUpdate();
      for (int i = 0; i < this.feeList.Count; ++i)
      {
        FeeListBase.FeeTable tableAt = this.feeList.GetTableAt(i);
        this.listView.Items.Add(new GVItem(tableAt.FeeName)
        {
          SubItems = {
            (object) tableAt.CalcBasedOn,
            (object) tableAt.Rate,
            (object) tableAt.Additional
          }
        });
      }
      this.listView.EndUpdate();
      this.listView_SelectedIndexChanged((object) null, (EventArgs) null);
      this.setContainerHeader();
    }

    private void newBtn_Click(object sender, EventArgs e) => this.setTableContents("");

    private void editBtn_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void listView_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void editSelectedItem()
    {
      if (this.listView.SelectedItems.Count != 0)
      {
        this.setTableContents(this.listView.SelectedItems[0].Text);
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an item first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void setTableContents(string feeName)
    {
      string calcBasedOn = string.Empty;
      string rate = string.Empty;
      string additional = string.Empty;
      if (feeName != "")
      {
        FeeListBase.FeeTable table = this.feeList.GetTable(feeName);
        feeName = table.FeeName;
        calcBasedOn = table.CalcBasedOn;
        rate = table.Rate;
        additional = table.Additional;
      }
      using (FeeDialog feeDialog = new FeeDialog(this.feeType, feeName, calcBasedOn, rate, additional, this.session))
      {
        if (feeDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (feeName == string.Empty)
          this.feeList.InsertFee(feeDialog.FeeName, feeDialog.CalcBasedOn, feeDialog.Rate, feeDialog.Additional);
        else
          this.feeList.UpdateFee(feeName, feeDialog.FeeName, feeDialog.CalcBasedOn, feeDialog.Rate, feeDialog.Additional);
        this.saveFeeSettings();
        this.refreshFeeList();
        this.session.SessionObjects.RefreshCityStateCache = true;
      }
    }

    private void deleteBtn_Click(object sender, EventArgs e)
    {
      GVSelectedItemCollection selectedItems = this.listView.SelectedItems;
      if (selectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You have to select an item first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected item?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
          return;
        this.feeList.RemoveTable(selectedItems[0].Text);
        this.saveFeeSettings();
        this.refreshFeeList();
        this.session.SessionObjects.RefreshCityStateCache = true;
      }
    }

    private void saveFeeSettings()
    {
      switch (this.feeType)
      {
        case FeePanel.FeeType.City:
          this.session.SaveSystemSettings((object) (FeeCityList) this.feeList);
          break;
        case FeePanel.FeeType.State:
          this.session.SaveSystemSettings((object) (FeeStateList) this.feeList);
          break;
        case FeePanel.FeeType.UserDefined:
          this.session.SaveSystemSettings((object) (FeeUserList) this.feeList);
          break;
      }
    }

    public void SetSelectedFeeNames(List<string> selectedFeeNames)
    {
      foreach (GVItem gvItem in this.listView.Items.Where<GVItem>((Func<GVItem, bool>) (item => selectedFeeNames.Contains(item.SubItems[0].Text))))
        gvItem.Selected = true;
    }

    public enum FeeType
    {
      City,
      State,
      UserDefined,
    }
  }
}
