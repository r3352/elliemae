// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PurchaseAdviceFormSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PurchaseAdviceFormSetup : SettingsUserControl
  {
    private TemplateIFSExplorer ifsExplorer;
    private FileSystemEntry currentFolder = FileSystemEntry.PublicRoot;
    private Sessions.Session session;
    private IContainer components;
    private GridView listViewPayout;
    private Label lblPayouts;
    private FSExplorer tempExplorer;
    private GroupContainer gcPayouts;
    private StandardIconButton stdIconBtnDown;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnUp;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnDelete;
    private Splitter splitter1;
    private StandardIconButton stdIconBtnSave;
    private StandardIconButton stdIconBtnReset;
    private GradientPanel gradientPanel1;

    public PurchaseAdviceFormSetup(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public PurchaseAdviceFormSetup(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.tempExplorer = new FSExplorer(this.session);
      this.InitializeComponent();
      this.initForm();
      this.reset();
      this.setDirtyFlag(false);
      this.listViewPayout.AllowMultiselect = allowMultiSelect;
      this.tempExplorer.SingleSelection = !allowMultiSelect;
    }

    private void initForm()
    {
      this.ifsExplorer = new TemplateIFSExplorer(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.PurchaseAdvice);
      this.tempExplorer.FileType = FSExplorer.FileTypes.PurchaseAdvice;
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.SetProperties(false, false, false, 15, true);
      this.tempExplorer.Init((IFSExplorerBase) this.ifsExplorer, FileSystemEntry.PublicRoot, true);
      bool canCreateEdit = true;
      FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      if (!this.session.UserInfo.IsSuperAdministrator() && !aclManager.GetUserApplicationRight(AclFeature.ToolsTab_PA_CreateEditTemplate))
        canCreateEdit = false;
      this.tempExplorer.SetupForPurchaseAdvice(canCreateEdit);
    }

    private void initListView(ArrayList fields, GridView listview)
    {
      listview.Items.Clear();
      if (fields == null || fields.Count == 0)
        return;
      listview.BeginUpdate();
      for (int index = 0; index < fields.Count; ++index)
      {
        GVItem gvItem = new GVItem(fields[index].ToString());
        listview.Items.Add(gvItem);
      }
      listview.EndUpdate();
      this.listViewPayout_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    public override void Save()
    {
      this.session.ConfigurationManager.SetSecondaryFields(this.collectListView(this.listViewPayout, -1), SecondaryFieldTypes.Payouts);
      this.setDirtyFlag(false);
    }

    private void reset()
    {
      this.initListView(this.session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.Payouts), this.listViewPayout);
      this.listViewPayout_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private ArrayList collectListView(GridView listview, int excludeMe)
    {
      ArrayList arrayList = new ArrayList();
      for (int nItemIndex = 0; nItemIndex < listview.Items.Count; ++nItemIndex)
      {
        if (nItemIndex != excludeMe)
          arrayList.Add((object) listview.Items[nItemIndex].Text);
      }
      return arrayList;
    }

    private void buttonNew_Click(object sender, EventArgs e)
    {
      this.addNewOption(this.listViewPayout, SecondaryFieldTypes.Payouts);
    }

    private void addNewOption(GridView listview, SecondaryFieldTypes type)
    {
      using (SecondaryFieldDatilForm secondaryFieldDatilForm = new SecondaryFieldDatilForm(type, "", this.collectListView(this.listViewPayout, -1)))
      {
        if (secondaryFieldDatilForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        listview.BeginUpdate();
        listview.Items.Add(new GVItem(secondaryFieldDatilForm.NewOption)
        {
          Selected = true
        });
        listview.EndUpdate();
        this.setDirtyFlag(true);
      }
    }

    private void buttonEdit_Click(object sender, EventArgs e)
    {
      if (this.listViewPayout.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (SecondaryFieldDatilForm secondaryFieldDatilForm = new SecondaryFieldDatilForm(SecondaryFieldTypes.Payouts, this.listViewPayout.SelectedItems[0].Text, this.collectListView(this.listViewPayout, this.listViewPayout.SelectedItems[0].Index)))
        {
          if (secondaryFieldDatilForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.listViewPayout.BeginUpdate();
          this.listViewPayout.SelectedItems[0].Text = secondaryFieldDatilForm.NewOption;
          this.listViewPayout.EndUpdate();
          this.setDirtyFlag(true);
        }
      }
    }

    private void buttonUp_Click(object sender, EventArgs e)
    {
      if (this.listViewPayout.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.listViewPayout.SelectedItems[0].Index == 0)
          return;
        string text = this.listViewPayout.Items[this.listViewPayout.SelectedItems[0].Index - 1].Text;
        this.listViewPayout.Items[this.listViewPayout.SelectedItems[0].Index - 1].Text = this.listViewPayout.SelectedItems[0].Text;
        this.listViewPayout.SelectedItems[0].Text = text;
        GVItem selectedItem = this.listViewPayout.SelectedItems[0];
        this.listViewPayout.Items[this.listViewPayout.SelectedItems[0].Index - 1].Selected = true;
        selectedItem.Selected = false;
        this.setDirtyFlag(true);
      }
    }

    private void buttonDown_Click(object sender, EventArgs e)
    {
      if (this.listViewPayout.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.listViewPayout.SelectedItems[0].Index == this.listViewPayout.Items.Count - 1)
          return;
        string text = this.listViewPayout.Items[this.listViewPayout.SelectedItems[0].Index + 1].Text;
        this.listViewPayout.Items[this.listViewPayout.SelectedItems[0].Index + 1].Text = this.listViewPayout.SelectedItems[0].Text;
        this.listViewPayout.SelectedItems[0].Text = text;
        GVItem selectedItem = this.listViewPayout.SelectedItems[0];
        this.listViewPayout.Items[this.listViewPayout.SelectedItems[0].Index + 1].Selected = true;
        selectedItem.Selected = false;
        this.setDirtyFlag(true);
      }
    }

    private void buttonDelete1_Click(object sender, EventArgs e)
    {
      if (this.listViewPayout.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int index1 = this.listViewPayout.SelectedItems[0].Index;
        GVItem[] gvItemArray = new GVItem[this.listViewPayout.SelectedItems.Count];
        for (int index2 = 0; index2 < this.listViewPayout.SelectedItems.Count; ++index2)
          gvItemArray[index2] = this.listViewPayout.SelectedItems[index2];
        this.listViewPayout.BeginUpdate();
        for (int index3 = 0; index3 < gvItemArray.Length; ++index3)
          this.listViewPayout.Items.Remove(gvItemArray[index3]);
        this.listViewPayout.EndUpdate();
        this.setDirtyFlag(true);
        if (this.listViewPayout.Items.Count == 0)
          return;
        if (index1 + 1 > this.listViewPayout.Items.Count)
          this.listViewPayout.Items[this.listViewPayout.Items.Count - 1].Selected = true;
        else
          this.listViewPayout.Items[index1].Selected = true;
      }
    }

    private void listViewPayout_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDelete.Enabled = this.listViewPayout.SelectedItems.Count > 0;
      this.stdIconBtnUp.Enabled = this.listViewPayout.SelectedItems.Count == 1 && this.listViewPayout.SelectedItems[0].Index > 0;
      this.stdIconBtnDown.Enabled = this.listViewPayout.SelectedItems.Count == 1 && this.listViewPayout.SelectedItems[0].Index < this.listViewPayout.Items.Count - 1;
      this.stdIconBtnEdit.Enabled = this.listViewPayout.SelectedItems.Count == 1;
    }

    protected override void setDirtyFlag(bool val)
    {
      base.setDirtyFlag(val);
      if (this.stdIconBtnSave == null || this.stdIconBtnReset == null)
        return;
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = val;
    }

    private void stdIconBtnSave_Click(object sender, EventArgs e) => this.Save();

    private void stdIconBtnReset_Click(object sender, EventArgs e)
    {
      if (ResetConfirmDialog.ShowDialog((IWin32Window) this.setupContainer, (string) null) == DialogResult.No)
        return;
      this.reset();
      this.setDirtyFlag(false);
    }

    public string[] SelectedPurchaseAdviceTemplate
    {
      get
      {
        return this.tempExplorer.SelectedItems.Count == 0 ? (string[]) null : this.tempExplorer.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.tempExplorer.GVItems.Count; ++nItemIndex)
          {
            if (this.tempExplorer.GVItems[nItemIndex].Text == value[index])
            {
              this.tempExplorer.GVItems[nItemIndex].Selected = true;
              break;
            }
          }
        }
      }
    }

    public string[] SelectedPayout
    {
      get
      {
        return this.listViewPayout.SelectedItems.Count == 0 ? (string[]) null : this.listViewPayout.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text + "_" + 3.ToString())).ToArray<string>();
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.listViewPayout.Items.Count; ++nItemIndex)
          {
            if (this.listViewPayout.Items[nItemIndex].Text == value[index].Replace("_" + (object) 3, ""))
            {
              this.listViewPayout.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
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
      GVColumn gvColumn = new GVColumn();
      this.listViewPayout = new GridView();
      this.lblPayouts = new Label();
      this.gcPayouts = new GroupContainer();
      this.gradientPanel1 = new GradientPanel();
      this.stdIconBtnSave = new StandardIconButton();
      this.stdIconBtnReset = new StandardIconButton();
      this.stdIconBtnDown = new StandardIconButton();
      this.stdIconBtnUp = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.toolTip1 = new ToolTip(this.components);
      this.splitter1 = new Splitter();
      this.gcPayouts.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDown).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUp).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.SuspendLayout();
      this.tempExplorer.Dock = DockStyle.Fill;
      this.tempExplorer.FolderComboSelectedIndex = -1;
      this.tempExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.tempExplorer.HasPublicRight = true;
      this.tempExplorer.Location = new Point(310, 0);
      this.tempExplorer.Name = "tempExplorer";
      this.tempExplorer.RenameButtonSize = new Size(62, 22);
      this.tempExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.tempExplorer.Size = new Size(637, 520);
      this.tempExplorer.TabIndex = 68;
      this.listViewPayout.BorderStyle = BorderStyle.None;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.Text = "Values";
      gvColumn.Width = 300;
      this.listViewPayout.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.listViewPayout.Dock = DockStyle.Fill;
      this.listViewPayout.Location = new Point(1, 57);
      this.listViewPayout.Name = "listViewPayout";
      this.listViewPayout.Size = new Size(305, 462);
      this.listViewPayout.TabIndex = 69;
      this.listViewPayout.SelectedIndexChanged += new EventHandler(this.listViewPayout_SelectedIndexChanged);
      this.lblPayouts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblPayouts.BackColor = Color.Transparent;
      this.lblPayouts.Location = new Point(6, 0);
      this.lblPayouts.Name = "lblPayouts";
      this.lblPayouts.Size = new Size(295, 31);
      this.lblPayouts.TabIndex = 77;
      this.lblPayouts.Text = "Create and edit descriptions for the payout options that users can select on the Purchase Advice Form.";
      this.lblPayouts.TextAlign = ContentAlignment.MiddleLeft;
      this.gcPayouts.Controls.Add((Control) this.listViewPayout);
      this.gcPayouts.Controls.Add((Control) this.gradientPanel1);
      this.gcPayouts.Controls.Add((Control) this.stdIconBtnSave);
      this.gcPayouts.Controls.Add((Control) this.stdIconBtnReset);
      this.gcPayouts.Controls.Add((Control) this.stdIconBtnDown);
      this.gcPayouts.Controls.Add((Control) this.stdIconBtnUp);
      this.gcPayouts.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcPayouts.Controls.Add((Control) this.stdIconBtnNew);
      this.gcPayouts.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcPayouts.Dock = DockStyle.Left;
      this.gcPayouts.Location = new Point(0, 0);
      this.gcPayouts.Name = "gcPayouts";
      this.gcPayouts.Size = new Size(307, 520);
      this.gcPayouts.TabIndex = 41;
      this.gcPayouts.Text = "Payouts Dropdown List";
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.lblPayouts);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(1, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(305, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 86;
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(263, 5);
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(16, 17);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 85;
      this.stdIconBtnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnSave, "Save");
      this.stdIconBtnSave.Click += new EventHandler(this.stdIconBtnSave_Click);
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(285, 5);
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(16, 17);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 84;
      this.stdIconBtnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnReset, "Reset");
      this.stdIconBtnReset.Click += new EventHandler(this.stdIconBtnReset_Click);
      this.stdIconBtnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDown.BackColor = Color.Transparent;
      this.stdIconBtnDown.Location = new Point(219, 5);
      this.stdIconBtnDown.Name = "stdIconBtnDown";
      this.stdIconBtnDown.Size = new Size(16, 17);
      this.stdIconBtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDown.TabIndex = 83;
      this.stdIconBtnDown.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDown, "Down");
      this.stdIconBtnDown.Click += new EventHandler(this.buttonDown_Click);
      this.stdIconBtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUp.BackColor = Color.Transparent;
      this.stdIconBtnUp.Location = new Point(197, 5);
      this.stdIconBtnUp.Name = "stdIconBtnUp";
      this.stdIconBtnUp.Size = new Size(16, 17);
      this.stdIconBtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUp.TabIndex = 82;
      this.stdIconBtnUp.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnUp, "Up");
      this.stdIconBtnUp.Click += new EventHandler(this.buttonUp_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(175, 5);
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 17);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 81;
      this.stdIconBtnEdit.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnEdit, "Edit");
      this.stdIconBtnEdit.Click += new EventHandler(this.buttonEdit_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(153, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 17);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 80;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.buttonNew_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(241, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 17);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 79;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.buttonDelete1_Click);
      this.splitter1.BackColor = Color.WhiteSmoke;
      this.splitter1.Location = new Point(307, 0);
      this.splitter1.Name = "splitter1";
      this.splitter1.Size = new Size(3, 520);
      this.splitter1.TabIndex = 42;
      this.splitter1.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.tempExplorer);
      this.Controls.Add((Control) this.splitter1);
      this.Controls.Add((Control) this.gcPayouts);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (PurchaseAdviceFormSetup);
      this.Size = new Size(947, 520);
      this.gcPayouts.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      ((ISupportInitialize) this.stdIconBtnDown).EndInit();
      ((ISupportInitialize) this.stdIconBtnUp).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
