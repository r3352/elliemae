// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.EnhancedConditionsSelector
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class EnhancedConditionsSelector : UserControl
  {
    private GridViewDataManager _gvDataManager;
    private IContainer components;
    private GroupContainer gcConditionTemplates;
    private GridView gvConditionTemplates;
    private Label lblSelectSourceInstructions;
    private Button btnSelectAllConditions;
    private Panel panelRoot;

    public event EventHandler<int> OnSelectionChanged;

    public EnhancedConditionsSelector() => this.InitializeComponent();

    public void SetMessage(string message) => this.lblSelectSourceInstructions.Text = message;

    public IList<Guid?> GetSelectedItemIDs()
    {
      return (IList<Guid?>) this.gvConditionTemplates.SelectedItems.Select<GVItem, Guid?>((Func<GVItem, Guid?>) (i => !(i.Tag is EnhancedConditionTemplate tag) ? new Guid?() : tag.Id)).ToList<Guid?>();
    }

    public void Init(IEnumerable<EnhancedConditionTemplate> templates, Sessions.Session session)
    {
      List<EnhancedConditionTemplate> list = templates != null ? templates.ToList<EnhancedConditionTemplate>() : (List<EnhancedConditionTemplate>) null;
      this._gvDataManager = new GridViewDataManager(session, this.gvConditionTemplates, (LoanDataMgr) null);
      this._gvDataManager.ClearItems();
      Action<EnhancedConditionTemplate> action = (Action<EnhancedConditionTemplate>) (i => this._gvDataManager.AddItem(i));
      list.ForEach(action);
      this.gvConditionTemplates.Sort(12, SortOrder.Descending);
      this.RaiseSelectionChanged();
    }

    private void RaiseSelectionChanged()
    {
      this.gcConditionTemplates.Text = string.Format("List of Enhanced Conditions ({0} of {1} selected)", (object) this.gvConditionTemplates.SelectedItems.Count, (object) this.gvConditionTemplates.VisibleItems.Count);
      EventHandler<int> selectionChanged = this.OnSelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged((object) this, this.gvConditionTemplates.SelectedItems.Count);
    }

    private void gvConditionTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.RaiseSelectionChanged();
    }

    private void btnSelectAllConditions_Click(object sender, EventArgs e)
    {
      this.gvConditionTemplates.VisibleItems.SelectAll();
      this.RaiseSelectionChanged();
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
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      this.gcConditionTemplates = new GroupContainer();
      this.gvConditionTemplates = new GridView();
      this.lblSelectSourceInstructions = new Label();
      this.btnSelectAllConditions = new Button();
      this.panelRoot = new Panel();
      this.gcConditionTemplates.SuspendLayout();
      this.panelRoot.SuspendLayout();
      this.SuspendLayout();
      this.gcConditionTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcConditionTemplates.AutoScroll = true;
      this.gcConditionTemplates.Controls.Add((Control) this.gvConditionTemplates);
      this.gcConditionTemplates.HeaderForeColor = SystemColors.ControlText;
      this.gcConditionTemplates.Location = new Point(4, 44);
      this.gcConditionTemplates.Margin = new Padding(0);
      this.gcConditionTemplates.Name = "gcConditionTemplates";
      this.gcConditionTemplates.Size = new Size(1895, 516);
      this.gcConditionTemplates.TabIndex = 5;
      this.gcConditionTemplates.Text = "List of Conditions ";
      this.gvConditionTemplates.BorderStyle = BorderStyle.None;
      this.gvConditionTemplates.ClearSelectionsOnEmptyRowClick = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "InternalId";
      gvColumn1.Tag = (object) "InternalId";
      gvColumn1.Text = "Internal ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Name";
      gvColumn2.Tag = (object) "NAME";
      gvColumn2.Text = "Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "InternalDescription";
      gvColumn3.Tag = (object) "INTERNALDESCRIPTION";
      gvColumn3.Text = "Internal Description";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Type";
      gvColumn4.Tag = (object) "TYPE";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Category";
      gvColumn5.Tag = (object) "CATEGORY";
      gvColumn5.Text = "Category";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "PriorTo";
      gvColumn6.Tag = (object) "PRIORTOENHANCED";
      gvColumn6.Text = "Prior To";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "PrintInternal";
      gvColumn7.Tag = (object) "PRINTINTERNALLY";
      gvColumn7.Text = "Print Internal";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "PrintExternal";
      gvColumn8.Tag = (object) "PRINTEXTERNALLY";
      gvColumn8.Text = "Print External";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "DaysToReceive";
      gvColumn9.SortMethod = GVSortMethod.Numeric;
      gvColumn9.Tag = (object) "DAYSTILLDUE";
      gvColumn9.Text = "Days To Receive";
      gvColumn9.Width = 100;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Customized";
      gvColumn10.Tag = (object) "CUSTOMIZED";
      gvColumn10.Text = "Customized";
      gvColumn10.Width = 100;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "ConditionStatus";
      gvColumn11.Tag = (object) "CONDITIONSTATUS";
      gvColumn11.Text = "Condition Status";
      gvColumn11.Width = 100;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "LastModifiedBy";
      gvColumn12.Tag = (object) "LASTMODIFIEDBY";
      gvColumn12.Text = "Last Modified By";
      gvColumn12.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "LastModifiedDateTime";
      gvColumn13.SortMethod = GVSortMethod.DateTime;
      gvColumn13.Tag = (object) "LASTMODIFIEDDATETIME";
      gvColumn13.Text = "Last Modified Date/Time";
      gvColumn13.Width = 100;
      this.gvConditionTemplates.Columns.AddRange(new GVColumn[13]
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
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gvConditionTemplates.Dock = DockStyle.Fill;
      this.gvConditionTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvConditionTemplates.Location = new Point(1, 26);
      this.gvConditionTemplates.Margin = new Padding(4, 5, 4, 5);
      this.gvConditionTemplates.Name = "gvConditionTemplates";
      this.gvConditionTemplates.Size = new Size(1893, 489);
      this.gvConditionTemplates.TabIndex = 1;
      this.gvConditionTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditionTemplates.SelectedIndexChanged += new EventHandler(this.gvConditionTemplates_SelectedIndexChanged);
      this.lblSelectSourceInstructions.AutoSize = true;
      this.lblSelectSourceInstructions.Location = new Point(0, 3);
      this.lblSelectSourceInstructions.Margin = new Padding(0);
      this.lblSelectSourceInstructions.Name = "lblSelectSourceInstructions";
      this.lblSelectSourceInstructions.Size = new Size(326, 20);
      this.lblSelectSourceInstructions.TabIndex = 6;
      this.lblSelectSourceInstructions.Text = "Please select condition templates to be synchronized";
      this.btnSelectAllConditions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelectAllConditions.Location = new Point(1770, 3);
      this.btnSelectAllConditions.Name = "btnSelectAllConditions";
      this.btnSelectAllConditions.Size = new Size(129, 33);
      this.btnSelectAllConditions.TabIndex = 7;
      this.btnSelectAllConditions.Text = "Select All";
      this.btnSelectAllConditions.UseVisualStyleBackColor = true;
      this.btnSelectAllConditions.Click += new EventHandler(this.btnSelectAllConditions_Click);
      this.panelRoot.Controls.Add((Control) this.btnSelectAllConditions);
      this.panelRoot.Controls.Add((Control) this.gcConditionTemplates);
      this.panelRoot.Controls.Add((Control) this.lblSelectSourceInstructions);
      this.panelRoot.Dock = DockStyle.Fill;
      this.panelRoot.Location = new Point(0, 0);
      this.panelRoot.Name = "panelRoot";
      this.panelRoot.Size = new Size(1899, 560);
      this.panelRoot.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelRoot);
      this.Name = nameof (EnhancedConditionsSelector);
      this.Size = new Size(1899, 560);
      this.gcConditionTemplates.ResumeLayout(false);
      this.panelRoot.ResumeLayout(false);
      this.panelRoot.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
