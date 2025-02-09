// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SpecialFeatureCodesSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
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
namespace EllieMae.EMLite.Setup
{
  public class SpecialFeatureCodesSetup : UserControl
  {
    protected Sessions.Session Session;
    private Dictionary<string, Func<SpecialFeatureCodeDefinition, object>> tagToFieldMap = new Dictionary<string, Func<SpecialFeatureCodeDefinition, object>>()
    {
      {
        "Code",
        (Func<SpecialFeatureCodeDefinition, object>) (sfc => (object) sfc.Code)
      },
      {
        "Description",
        (Func<SpecialFeatureCodeDefinition, object>) (sfc => (object) sfc.Description)
      },
      {
        "Comment",
        (Func<SpecialFeatureCodeDefinition, object>) (sfc => (object) sfc.Comment)
      },
      {
        "Source",
        (Func<SpecialFeatureCodeDefinition, object>) (sfc => (object) sfc.Source)
      },
      {
        "Status",
        (Func<SpecialFeatureCodeDefinition, object>) (sfc => !sfc.IsActive ? (object) "Inactive" : (object) "Active")
      }
    };
    private IContainer components;
    private GroupContainer gridSpecialFeatureCodes;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnNew;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDelete;
    private Button btnActivate;
    private Button btnDeactivate;
    private GridView gvSpecialFeatureCodesList;
    private FlowLayoutPanel flowLayoutActions;

    protected SpecialFeatureCodePermissions Permissions { get; set; }

    protected ISpecialFeatureCodeManager Service { get; set; }

    protected IList<SpecialFeatureCodeDefinition> GridViewList { get; set; }

    protected SpecialFeatureCodeDefinition Selected { get; set; }

    protected VScrollBarDirector VScrollBar { get; set; }

    public SpecialFeatureCodesSetup(Sessions.Session session)
    {
      this.InitializeComponent();
      this.Session = session;
      this.Service = (ISpecialFeatureCodeManager) new SessionSpecialFeatureCodeService(session);
      this.GridViewList = (IList<SpecialFeatureCodeDefinition>) new List<SpecialFeatureCodeDefinition>();
      this.SetAccessRights(this.Session.ACL.GetAclManager(AclCategory.Features) as FeaturesAclManager);
      this.InitGridView();
    }

    protected void SetAccessRights(FeaturesAclManager featuresAclManager)
    {
      this.Permissions = new SpecialFeatureCodePermissions(featuresAclManager);
      this.btnNew.Enabled = this.Permissions.CanAdd;
    }

    protected void InitGridView()
    {
      GridView featureCodesList = this.gvSpecialFeatureCodesList;
      GVColumnSort[] sortOrder = new GVColumnSort[3]
      {
        new GVColumnSort(4, SortOrder.Ascending),
        new GVColumnSort(0, SortOrder.Ascending),
        new GVColumnSort(3, SortOrder.Ascending)
      };
      featureCodesList.SortingType = SortingType.AlphaNumeric;
      featureCodesList.Sort(sortOrder);
      this.VScrollBar = new VScrollBarDirector(featureCodesList);
      this.RefreshGrid();
    }

    protected void RefreshGrid()
    {
      if (!this.Permissions.CanView)
        return;
      IList<SpecialFeatureCodeDefinition> featureCodeDefinitionList = this.FetchList();
      this.gridSpecialFeatureCodes.Text = string.Format("Special Feature Codes ({0})", (object) (featureCodeDefinitionList != null ? featureCodeDefinitionList.Count : 0));
      this.GridViewList = featureCodeDefinitionList;
      string id = this.Selected?.ID;
      GridView featureCodesList = this.gvSpecialFeatureCodesList;
      featureCodesList.Items.Clear();
      foreach (SpecialFeatureCodeDefinition featureCodeDefinition in (IEnumerable<SpecialFeatureCodeDefinition>) featureCodeDefinitionList)
      {
        GVItem gvItem = new GVItem()
        {
          Tag = (object) featureCodeDefinition.ID,
          Selected = featureCodeDefinition.ID.Equals(id)
        };
        foreach (GVColumn column in featureCodesList.Columns)
        {
          string key = column.Tag.ToString();
          gvItem.SubItems[column.Index].Value = this.tagToFieldMap[key](featureCodeDefinition);
        }
        featureCodesList.Items.Add(gvItem);
      }
      featureCodesList.ReSort();
      if (id == null && featureCodeDefinitionList.Count > 0)
        featureCodesList.Items[0].Selected = true;
      this.VScrollBar.EnsureVisible(featureCodesList.Items.FirstOrDefault<GVItem>((Func<GVItem, bool>) (i => i.Selected))?.DisplayIndex ?? 0);
      this.HandleSelection();
    }

    private IList<SpecialFeatureCodeDefinition> FetchList()
    {
      IList<SpecialFeatureCodeDefinition> featureCodeDefinitionList = (IList<SpecialFeatureCodeDefinition>) null;
      try
      {
        featureCodeDefinitionList = this.Service.GetAll();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error while fetching special feature codes list\nThe error was: '" + ex.Message + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return featureCodeDefinitionList;
    }

    protected void HandleSelection()
    {
      SpecialFeatureCodeDefinition featureCodeDefinition = (SpecialFeatureCodeDefinition) null;
      GVSelectedItemCollection selectedItems = this.gvSpecialFeatureCodesList.SelectedItems;
      bool flag = selectedItems.Count > 0;
      if (flag)
      {
        string id = selectedItems[0].Tag as string;
        this.Selected = featureCodeDefinition = this.GridViewList.FirstOrDefault<SpecialFeatureCodeDefinition>((Func<SpecialFeatureCodeDefinition, bool>) (sfc => sfc.ID == id));
      }
      bool canActivate = this.Permissions.CanActivate;
      this.btnActivate.Enabled = ((!flag ? 0 : (!featureCodeDefinition.IsActive ? 1 : 0)) & (canActivate ? 1 : 0)) != 0;
      this.btnDeactivate.Enabled = ((!flag ? 0 : (featureCodeDefinition.IsActive ? 1 : 0)) & (canActivate ? 1 : 0)) != 0;
      this.btnDelete.Enabled = flag && this.Permissions.CanDelete;
      this.btnEdit.Enabled = flag && this.Permissions.CanEdit;
    }

    protected void DoAddOrUpdate(bool update = true)
    {
      SpecialFeatureCodeEditor featureCodeEditor = new SpecialFeatureCodeEditor(this.Service, update ? this.Selected : (SpecialFeatureCodeDefinition) null);
      if (DialogResult.OK != featureCodeEditor.ShowDialog())
        return;
      this.Selected = featureCodeEditor.SpecialFeatureCode;
      this.RefreshGrid();
    }

    protected void DoActivation(bool activate)
    {
      try
      {
        if (this.Selected == null)
          return;
        if (activate)
        {
          if (this.GridViewList.IsOtherActiveCodeSource(this.Selected) && DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "An existing entry matching the selected code and source is currently active.  Would you like to continue activating this entry?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
            return;
          this.Service.Activate(this.Selected);
        }
        else
          this.Service.Deactivate(this.Selected);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error while " + (activate ? "" : "de") + "activating\nThe error was: '" + ex.Message + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.RefreshGrid();
    }

    protected void DoDelete()
    {
      try
      {
        if (this.Selected == null)
          return;
        string text = "Are you sure you would like to delete?";
        MessageBoxIcon icon = MessageBoxIcon.Question;
        if (this.Service.IsUsedinFieldTriggerRule(this.Selected.ID))
        {
          text = "This Special Feature Code is currently referenced in a field trigger business rule.\n" + text;
          icon = MessageBoxIcon.Exclamation;
        }
        else if (this.Selected.IsActive)
        {
          text = "This entry is currently active.\n" + text;
          icon = MessageBoxIcon.Exclamation;
        }
        if (DialogResult.Cancel == Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OKCancel, icon))
          return;
        this.Service.Delete(this.Selected);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error while deleting\nThe error was: '" + ex.Message + "'", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      this.Selected = (SpecialFeatureCodeDefinition) null;
      this.RefreshGrid();
    }

    private void gvSpecialFeatureCodesList_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.HandleSelection();
    }

    private void btnActivate_Click(object sender, EventArgs e) => this.DoActivation(true);

    private void btnDeactivate_Click(object sender, EventArgs e) => this.DoActivation(false);

    private void btnNew_Click(object sender, EventArgs e) => this.DoAddOrUpdate(false);

    private void btnEdit_Click(object sender, EventArgs e) => this.DoAddOrUpdate();

    private void gvSpecialFeatureCodesList_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      string id = e.Item.Tag.ToString();
      this.Selected = this.GridViewList.FirstOrDefault<SpecialFeatureCodeDefinition>((Func<SpecialFeatureCodeDefinition, bool>) (i => i.ID == id));
      this.DoAddOrUpdate();
    }

    private void btnDelete_Click(object sender, EventArgs e) => this.DoDelete();

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
      this.gridSpecialFeatureCodes = new GroupContainer();
      this.flowLayoutActions = new FlowLayoutPanel();
      this.btnNew = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnActivate = new Button();
      this.btnDeactivate = new Button();
      this.gvSpecialFeatureCodesList = new GridView();
      this.gridSpecialFeatureCodes.SuspendLayout();
      this.flowLayoutActions.SuspendLayout();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.SuspendLayout();
      this.gridSpecialFeatureCodes.Controls.Add((Control) this.flowLayoutActions);
      this.gridSpecialFeatureCodes.Controls.Add((Control) this.gvSpecialFeatureCodesList);
      this.gridSpecialFeatureCodes.Dock = DockStyle.Fill;
      this.gridSpecialFeatureCodes.HeaderForeColor = SystemColors.ControlText;
      this.gridSpecialFeatureCodes.Location = new Point(0, 0);
      this.gridSpecialFeatureCodes.Name = "gridSpecialFeatureCodes";
      this.gridSpecialFeatureCodes.Size = new Size(758, 494);
      this.gridSpecialFeatureCodes.TabIndex = 100;
      this.gridSpecialFeatureCodes.Text = "All Special Feature Codes";
      this.flowLayoutActions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutActions.AutoSize = true;
      this.flowLayoutActions.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flowLayoutActions.BackColor = Color.Transparent;
      this.flowLayoutActions.Controls.Add((Control) this.btnNew);
      this.flowLayoutActions.Controls.Add((Control) this.btnEdit);
      this.flowLayoutActions.Controls.Add((Control) this.btnDelete);
      this.flowLayoutActions.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutActions.Controls.Add((Control) this.btnActivate);
      this.flowLayoutActions.Controls.Add((Control) this.btnDeactivate);
      this.flowLayoutActions.Location = new Point(531, 1);
      this.flowLayoutActions.Margin = new Padding(2, 2, 2, 2);
      this.flowLayoutActions.Name = "flowLayoutActions";
      this.flowLayoutActions.Size = new Size(224, 22);
      this.flowLayoutActions.TabIndex = 100;
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Enabled = false;
      this.btnNew.Location = new Point(3, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 1;
      this.btnNew.TabStop = false;
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(25, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 2;
      this.btnEdit.TabStop = false;
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(47, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 3;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(69, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 100;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnActivate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnActivate.Enabled = false;
      this.btnActivate.Location = new Point(77, 1);
      this.btnActivate.Margin = new Padding(3, 1, 3, 1);
      this.btnActivate.Name = "btnActivate";
      this.btnActivate.Size = new Size(69, 20);
      this.btnActivate.TabIndex = 4;
      this.btnActivate.Text = "Activate";
      this.btnActivate.UseVisualStyleBackColor = true;
      this.btnActivate.Click += new EventHandler(this.btnActivate_Click);
      this.btnDeactivate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeactivate.CausesValidation = false;
      this.btnDeactivate.Enabled = false;
      this.btnDeactivate.Location = new Point(152, 1);
      this.btnDeactivate.Margin = new Padding(3, 1, 3, 1);
      this.btnDeactivate.Name = "btnDeactivate";
      this.btnDeactivate.Size = new Size(69, 20);
      this.btnDeactivate.TabIndex = 5;
      this.btnDeactivate.Text = "Deactivate";
      this.btnDeactivate.UseVisualStyleBackColor = true;
      this.btnDeactivate.Click += new EventHandler(this.btnDeactivate_Click);
      this.gvSpecialFeatureCodesList.AllowMultiselect = false;
      this.gvSpecialFeatureCodesList.BorderStyle = BorderStyle.None;
      this.gvSpecialFeatureCodesList.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Code";
      gvColumn1.Tag = (object) "Code";
      gvColumn1.Text = "Code";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Description";
      gvColumn2.Tag = (object) "Description";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 240;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Comment";
      gvColumn3.Tag = (object) "Comment";
      gvColumn3.Text = "Comments";
      gvColumn3.Width = 160;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Source";
      gvColumn4.Tag = (object) "Source";
      gvColumn4.Text = "Source";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Status";
      gvColumn5.Tag = (object) "Status";
      gvColumn5.Text = "Status";
      gvColumn5.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn5.Width = 80;
      this.gvSpecialFeatureCodesList.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvSpecialFeatureCodesList.Dock = DockStyle.Fill;
      this.gvSpecialFeatureCodesList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvSpecialFeatureCodesList.Location = new Point(1, 26);
      this.gvSpecialFeatureCodesList.Name = "gvSpecialFeatureCodesList";
      this.gvSpecialFeatureCodesList.Size = new Size(756, 467);
      this.gvSpecialFeatureCodesList.SortHistory = 3;
      this.gvSpecialFeatureCodesList.TabIndex = 6;
      this.gvSpecialFeatureCodesList.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvSpecialFeatureCodesList.SelectedIndexChanged += new EventHandler(this.gvSpecialFeatureCodesList_SelectedIndexChanged);
      this.gvSpecialFeatureCodesList.ItemDoubleClick += new GVItemEventHandler(this.gvSpecialFeatureCodesList_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gridSpecialFeatureCodes);
      this.Margin = new Padding(2, 2, 2, 2);
      this.Name = nameof (SpecialFeatureCodesSetup);
      this.Size = new Size(758, 494);
      this.gridSpecialFeatureCodes.ResumeLayout(false);
      this.gridSpecialFeatureCodes.PerformLayout();
      this.flowLayoutActions.ResumeLayout(false);
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.ResumeLayout(false);
    }
  }
}
