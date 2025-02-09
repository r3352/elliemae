// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ConditionTypeSettingsDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ConditionTypeSettingsDialog : Form
  {
    private const string className = "ConditionTypeSettingsDialog";
    private static readonly string sw = Tracing.SwDataEngine;
    private Sessions.Session session;
    private EDisclosurePackage package;
    private string condition;
    private string[] ReservedTypes = new string[1]
    {
      "Investor Delivery"
    };
    private IContainer components;
    private Button btnCancel;
    private Label lblCondition;
    private ToolTip tooltip;
    private GridView gvTypes;
    private FlowLayoutPanel pnlToolbar;
    private Button btnDeactivate;
    private Button btnActivate;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton btnEdit;
    private StandardIconButton btnAddType;
    private Label lbltypecount;
    private GroupContainer gcType;
    private EMHelpLink emHelpLink1;
    private StandardIconButton btnDelete;

    private EnhancedConditionType ItemConditionType(GVItem item)
    {
      return item?.SubItems[0]?.Tag as EnhancedConditionType;
    }

    private GVItem CreateGVItem(EnhancedConditionType conditionType)
    {
      return new GVItem()
      {
        SubItems = {
          new GVSubItem((object) conditionType.title),
          new GVSubItem(conditionType.active ? (object) "Active" : (object) "Inactive")
        }
      };
    }

    private EnhancedConditionType SingleSelection
    {
      get
      {
        return this.gvTypes.SelectedItems.Count != 1 ? (EnhancedConditionType) null : this.ItemConditionType(this.gvTypes.SelectedItems[0]);
      }
    }

    public ConditionTypeSettingsDialog(EDisclosurePackage package, string condition)
      : this(Session.DefaultInstance, package, condition)
    {
    }

    public ConditionTypeSettingsDialog(
      Sessions.Session session,
      EDisclosurePackage package,
      string condition)
    {
      this.InitializeComponent();
      this.session = session;
      this.emHelpLink1.AssignSession(this.session);
      this.package = package;
      this.condition = condition;
      this.setConditionType();
    }

    private void setConditionType()
    {
      this.gvTypes.Items.Clear();
      try
      {
        foreach (EnhancedConditionType enhancedConditionType in EnhancedConditionRestApiHelper.GetEnhancedConditionTypes())
          this.gvTypes.Items.Add(this.CreateGVItem(enhancedConditionType));
        this.gvTypes.Sort(new GVColumnSort[1]
        {
          new GVColumnSort(0, SortOrder.Ascending)
        });
        this.setConditionTypeCount();
      }
      catch (Exception ex)
      {
        Tracing.Log(ConditionTypeSettingsDialog.sw, TraceLevel.Error, nameof (ConditionTypeSettingsDialog), "Error retrieving Condition Types, " + ex.InnerException?.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error retrieving Condition Types. " + ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void setConditionTypeCount()
    {
      this.lbltypecount.Text = this.gvTypes.Items.Count > 0 ? "(" + this.gvTypes.Items.Count.ToString() + ")" : "";
    }

    private void btnAddType_Click(object sender, EventArgs e)
    {
      ConditionTypeSettingsAddEditDialog settingsAddEditDialog = new ConditionTypeSettingsAddEditDialog(this.session, (IList<string>) this.gvTypes.Items.Select<GVItem, string>((Func<GVItem, string>) (i => i.Text)).ToList<string>());
      if (DialogResult.OK != settingsAddEditDialog.ShowDialog((IWin32Window) this) || settingsAddEditDialog.conditionType == null)
        return;
      GVItem gvItem = this.CreateGVItem(settingsAddEditDialog.conditionType);
      this.gvTypes.Items.Add(gvItem);
      this.setConditionTypeCount();
      this.gvTypes.ReSort();
      this.gvTypes.EnsureVisible(gvItem.DisplayIndex);
      this.gvTypes.Items.All<GVItem>((Func<GVItem, bool>) (item => (item.Selected = item == gvItem) || true));
    }

    private void btnEditType_Click(object sender, EventArgs e)
    {
      EnhancedConditionType singleSelection = this.SingleSelection;
      if (singleSelection != null)
      {
        int num1 = (int) new ConditionTypeSettingsAddEditDialog(this.session, singleSelection).ShowDialog((IWin32Window) this);
      }
      else
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Select one Condition type.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private void gvTypes_DoubleClick(object sender, EventArgs e)
    {
      this.btnEditType_Click(sender, e);
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
      ConditionTypeSettingsDialog owner = this;
      EnhancedConditionType singleSelection;
      if ((singleSelection = owner.SingleSelection) == null || !owner.CanDeleteType(singleSelection))
        return;
      string text = "Deleting an Enhanced Condition type will also delete any associated Enhanced Condition templates.  Would you like to continue?";
      if (Utils.Dialog((IWin32Window) owner, text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        return;
      try
      {
        int num = await EnhancedConditionRestApiHelper.DeleteEnhancedConditionTypes(singleSelection) ? 1 : 0;
        owner.gvTypes.Items.RemoveAt(owner.gvTypes.SelectedItems[0].DisplayIndex);
        owner.setConditionTypeCount();
      }
      catch (Exception ex)
      {
        string str = ex.InnerException?.Message ?? ex.Message;
        Tracing.Log(ConditionTypeSettingsDialog.sw, TraceLevel.Error, nameof (ConditionTypeSettingsDialog), "Error deleting Condition Type: " + str);
        int num = (int) Utils.Dialog((IWin32Window) owner, "Error deleting Condition Type.\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private bool CanDeleteType(EnhancedConditionType type)
    {
      return !((IEnumerable<string>) this.ReservedTypes).Contains<string>(type.title, (IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    }

    private void gvFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      EnhancedConditionType singleSelection;
      if ((singleSelection = this.SingleSelection) != null)
      {
        this.btnActivate.Enabled = !(this.btnDeactivate.Enabled = singleSelection.active);
        this.btnDelete.Enabled = this.CanDeleteType(singleSelection);
        this.btnEdit.Enabled = true;
      }
      else
      {
        ConditionTypeSettingsDialog.SameValue<bool> sameValue = ConditionTypeSettingsDialog.SameValue<bool>.Check(this.gvTypes.SelectedItems.Select<GVItem, bool>((Func<GVItem, bool>) (i => this.ItemConditionType(i).active)));
        this.btnActivate.Enabled = sameValue.AllSame && !sameValue.Value;
        this.btnDeactivate.Enabled = sameValue.AllSame && sameValue.Value;
        this.btnDelete.Enabled = this.btnEdit.Enabled = false;
      }
    }

    private void btnActivate_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvTypes.SelectedItems)
        this.activateDeactivateType(selectedItem, true);
    }

    private void btnDeactivate_Click(object sender, EventArgs e)
    {
      foreach (GVItem selectedItem in this.gvTypes.SelectedItems)
        this.activateDeactivateType(selectedItem, false);
    }

    private void activateDeactivateType(GVItem item, bool isActivate)
    {
      try
      {
        var conditionType = new
        {
          id = this.ItemConditionType(item).id,
          active = isActivate
        };
        if (!EnhancedConditionRestApiHelper.UpdateConditionTypes((object) conditionType))
          return;
        item.SubItems[1].Value = isActivate ? (object) "Active" : (object) "Inactive";
        this.btnActivate.Enabled = !(this.btnDeactivate.Enabled = isActivate);
      }
      catch (Exception ex)
      {
        Tracing.Log(ConditionTypeSettingsDialog.sw, TraceLevel.Error, nameof (ConditionTypeSettingsDialog), "Error in updating condition types, " + ex.InnerException.Message);
        int num = (int) Utils.Dialog((IWin32Window) this, "Error in updating condition types, " + ex.InnerException.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
      this.btnCancel = new Button();
      this.lblCondition = new Label();
      this.tooltip = new ToolTip(this.components);
      this.btnAddType = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.gvTypes = new GridView();
      this.pnlToolbar = new FlowLayoutPanel();
      this.btnDeactivate = new Button();
      this.btnActivate = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.lbltypecount = new Label();
      this.gcType = new GroupContainer();
      this.emHelpLink1 = new EMHelpLink();
      this.btnDelete = new StandardIconButton();
      ((ISupportInitialize) this.btnAddType).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      this.pnlToolbar.SuspendLayout();
      this.gcType.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(382, 404);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblCondition.AutoSize = true;
      this.lblCondition.Location = new Point(12, 12);
      this.lblCondition.Name = "lblCondition";
      this.lblCondition.Size = new Size(383, 19);
      this.lblCondition.TabIndex = 0;
      this.lblCondition.Text = "Manage type settings to help define your conditions";
      this.btnAddType.Anchor = AnchorStyles.None;
      this.btnAddType.BackColor = Color.Transparent;
      this.btnAddType.Location = new Point(33, 4);
      this.btnAddType.Margin = new Padding(4, 2, 0, 0);
      this.btnAddType.MouseDownImage = (Image) null;
      this.btnAddType.Name = "btnAddType";
      this.btnAddType.Size = new Size(16, 16);
      this.btnAddType.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAddType.TabIndex = 12;
      this.btnAddType.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAddType, "Add Type");
      this.btnAddType.Click += new EventHandler(this.btnAddType_Click);
      this.btnEdit.Anchor = AnchorStyles.None;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Enabled = false;
      this.btnEdit.Location = new Point(53, 4);
      this.btnEdit.Margin = new Padding(4, 2, 0, 0);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 14;
      this.btnEdit.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEdit, "Edit Type");
      this.btnEdit.Click += new EventHandler(this.btnEditType_Click);
      this.gvTypes.BorderStyle = BorderStyle.None;
      this.gvTypes.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "colType";
      gvColumn1.Text = "Type";
      gvColumn1.Width = 320;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "colStatus";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Status";
      gvColumn2.Width = 122;
      this.gvTypes.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvTypes.Dock = DockStyle.Fill;
      this.gvTypes.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTypes.Location = new Point(1, 26);
      this.gvTypes.Name = "gvTypes";
      this.gvTypes.Size = new Size(442, 334);
      this.gvTypes.TabIndex = 1;
      this.gvTypes.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTypes.SelectedIndexChanged += new EventHandler(this.gvFields_SelectedIndexChanged);
      this.gvTypes.DoubleClick += new EventHandler(this.gvTypes_DoubleClick);
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDeactivate);
      this.pnlToolbar.Controls.Add((Control) this.btnActivate);
      this.pnlToolbar.Controls.Add((Control) this.verticalSeparator1);
      this.pnlToolbar.Controls.Add((Control) this.btnDelete);
      this.pnlToolbar.Controls.Add((Control) this.btnEdit);
      this.pnlToolbar.Controls.Add((Control) this.btnAddType);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(185, 1);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(257, 26);
      this.pnlToolbar.TabIndex = 0;
      this.btnDeactivate.Anchor = AnchorStyles.None;
      this.btnDeactivate.Enabled = false;
      this.btnDeactivate.Location = new Point(182, 1);
      this.btnDeactivate.Margin = new Padding(4, 1, 0, 0);
      this.btnDeactivate.Name = "btnDeactivate";
      this.btnDeactivate.Size = new Size(75, 22);
      this.btnDeactivate.TabIndex = 16;
      this.btnDeactivate.Text = "Deactivate";
      this.btnDeactivate.TextAlign = ContentAlignment.TopCenter;
      this.btnDeactivate.UseVisualStyleBackColor = true;
      this.btnDeactivate.Click += new EventHandler(this.btnDeactivate_Click);
      this.btnActivate.Anchor = AnchorStyles.None;
      this.btnActivate.Enabled = false;
      this.btnActivate.Location = new Point(103, 1);
      this.btnActivate.Margin = new Padding(0, 1, 0, 0);
      this.btnActivate.Name = "btnActivate";
      this.btnActivate.Size = new Size(75, 22);
      this.btnActivate.TabIndex = 15;
      this.btnActivate.Text = "Activate";
      this.btnActivate.TextAlign = ContentAlignment.TopCenter;
      this.btnActivate.UseVisualStyleBackColor = true;
      this.btnActivate.Click += new EventHandler(this.btnActivate_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.None;
      this.verticalSeparator1.Location = new Point(95, 4);
      this.verticalSeparator1.Margin = new Padding(6, 2, 6, 0);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 17;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.lbltypecount.AutoSize = true;
      this.lbltypecount.BackColor = Color.Transparent;
      this.lbltypecount.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lbltypecount.Location = new Point(92, 7);
      this.lbltypecount.Name = "lbltypecount";
      this.lbltypecount.Size = new Size(18, 19);
      this.lbltypecount.TabIndex = 2;
      this.lbltypecount.Text = "1";
      this.gcType.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcType.Controls.Add((Control) this.lbltypecount);
      this.gcType.Controls.Add((Control) this.pnlToolbar);
      this.gcType.Controls.Add((Control) this.gvTypes);
      this.gcType.HeaderForeColor = SystemColors.ControlText;
      this.gcType.Location = new Point(12, 37);
      this.gcType.Name = "gcType";
      this.gcType.Size = new Size(444, 361);
      this.gcType.TabIndex = 5;
      this.gcType.Text = "Condition Type";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "ConditionTypeDialog";
      this.emHelpLink1.Location = new Point(12, 407);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 60;
      this.btnDelete.Anchor = AnchorStyles.None;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(73, 4);
      this.btnDelete.Margin = new Padding(4, 2, 0, 0);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 61;
      this.btnDelete.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDelete, "Delete");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.AutoScaleDimensions = new SizeF(9f, 19f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(469, 435);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.gcType);
      this.Controls.Add((Control) this.lblCondition);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionTypeSettingsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Condition Type Settings";
      ((ISupportInitialize) this.btnAddType).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      this.pnlToolbar.ResumeLayout(false);
      this.gcType.ResumeLayout(false);
      this.gcType.PerformLayout();
      ((ISupportInitialize) this.btnDelete).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private class SameValue<T> where T : struct, IEquatable<T>
    {
      private T? _same;

      private SameValue(T? same) => this._same = same;

      public bool AllSame => this._same.HasValue;

      public T Value => this._same.Value;

      public static ConditionTypeSettingsDialog.SameValue<T> Check(IEnumerable<T> values)
      {
        bool flag = false;
        T? nullable = new T?();
        foreach (T other in values)
        {
          if (!nullable.HasValue)
          {
            nullable = new T?(other);
            flag = true;
          }
          else
            flag &= nullable.Value.Equals(other);
        }
        return new ConditionTypeSettingsDialog.SameValue<T>(flag ? nullable : new T?());
      }
    }
  }
}
