// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemporaryBuydownPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Bpm;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TemporaryBuydownPanel : UserControl
  {
    private Sessions.Session session;
    private TemporaryBuydownTypeBpmManager bpmManager;
    private IContainer components;
    private ToolTip toolTip1;
    private GroupContainer gContainer;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private GridView listView;

    public TemporaryBuydownPanel()
      : this(Session.DefaultInstance, false)
    {
    }

    public TemporaryBuydownPanel(Sessions.Session session, bool allowMultiSelect)
    {
      this.session = session;
      this.bpmManager = new TemporaryBuydownTypeBpmManager(session);
      this.InitializeComponent();
      this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
      this.listView.DoubleClick += new EventHandler(this.listView_DoubleClick);
      this.stdIconBtnEdit.Enabled = this.stdIconBtnDelete.Enabled = false;
      this.refreshList();
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      using (TemporaryBuydownTypeSettingDialog typeSettingDialog = new TemporaryBuydownTypeSettingDialog(this.session))
      {
        typeSettingDialog.StartPosition = FormStartPosition.CenterParent;
        int num = (int) typeSettingDialog.ShowDialog((IWin32Window) this);
        if (typeSettingDialog.DialogResult == DialogResult.OK)
        {
          TemporaryBuydown buydown = typeSettingDialog.Buydown;
          if (buydown != null)
          {
            this.bpmManager.CreateTemporaryBuydownType(buydown);
            this.listView.BeginUpdate();
            this.createListItem(buydown);
            this.listView.EndUpdate();
          }
        }
        typeSettingDialog.Dispose();
      }
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e) => this.editTemporaryBuydown();

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      GVItem selectedItem = this.listView.SelectedItems[0];
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected record?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.bpmManager.DeleteTemporaryBuydownType((TemporaryBuydown) selectedItem.Tag);
      this.listView.Items.Remove(this.listView.SelectedItems[0]);
    }

    private void listView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnEdit.Enabled = this.stdIconBtnDelete.Enabled = this.listView.SelectedItems.Count > 0;
    }

    private void refreshList()
    {
      try
      {
        foreach (TemporaryBuydown temporaryBuydown in this.bpmManager.GetAllTemporaryBuydowns())
          this.createListItem(temporaryBuydown);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error on loading buydown list.  " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void createListItem(TemporaryBuydown buydown)
    {
      this.listView.Items.Add(new GVItem()
      {
        SubItems = {
          (object) buydown.BuydownType,
          (object) buydown.Description,
          (object) buydown.lastModifiedBy,
          (object) buydown.lastModifiedDateTime
        },
        Tag = (object) buydown
      });
    }

    private void listView_DoubleClick(object sender, EventArgs e)
    {
      if (this.listView.SelectedItems.Count <= 0)
        return;
      this.editTemporaryBuydown();
    }

    private void editTemporaryBuydown()
    {
      GVItem selectedItem = this.listView.SelectedItems[0];
      using (TemporaryBuydownTypeSettingDialog typeSettingDialog = new TemporaryBuydownTypeSettingDialog(this.session, (TemporaryBuydown) selectedItem.Tag))
      {
        typeSettingDialog.StartPosition = FormStartPosition.CenterParent;
        int num = (int) typeSettingDialog.ShowDialog((IWin32Window) this);
        if (typeSettingDialog.DialogResult == DialogResult.OK)
        {
          TemporaryBuydown buydown = typeSettingDialog.Buydown;
          this.bpmManager.UpdateTemporaryBuydownType(buydown);
          this.listView.BeginUpdate();
          selectedItem.SubItems[0].Value = (object) buydown.BuydownType;
          selectedItem.SubItems[1].Value = (object) buydown.Description;
          selectedItem.SubItems[2].Value = (object) buydown.lastModifiedBy;
          selectedItem.SubItems[3].Value = (object) buydown.lastModifiedDateTime;
          this.listView.EndUpdate();
        }
        typeSettingDialog.Dispose();
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
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.gContainer = new GroupContainer();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.listView = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.gContainer.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.gContainer.Controls.Add((Control) this.stdIconBtnEdit);
      this.gContainer.Controls.Add((Control) this.stdIconBtnNew);
      this.gContainer.Controls.Add((Control) this.stdIconBtnDelete);
      this.gContainer.Controls.Add((Control) this.listView);
      this.gContainer.Dock = DockStyle.Fill;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(681, 427);
      this.gContainer.TabIndex = 9;
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(637, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 10;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(615, 5);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 9;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(659, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 8;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.listView.AllowMultiselect = false;
      this.listView.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Buydown Type";
      gvColumn1.Width = 200;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Last Modified By";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 112;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Last Modified Date & Time";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 142;
      gvColumn4.SortMethod = GVSortMethod.DateTime;
      this.listView.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listView.Dock = DockStyle.Fill;
      this.listView.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listView.Location = new Point(1, 26);
      this.listView.Name = "listView";
      this.listView.Size = new Size(679, 400);
      this.listView.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gContainer);
      this.Name = nameof (TemporaryBuydownPanel);
      this.Size = new Size(681, 427);
      this.gContainer.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
