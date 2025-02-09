// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.ConditionTrackingSetupControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class ConditionTrackingSetupControl : UserControl
  {
    private Sessions.Session session;
    private ConditionTrackingSetup condSetup;
    private GridViewDataManager gvTemplatesMgr;
    private ConditionType conditionType;
    private IContainer components;
    private ToolTip toolTip;
    private StandardIconButton btnDeleteTemplate;
    private StandardIconButton btnEditTemplate;
    private StandardIconButton btnNewTemplate;
    private GroupContainer gcTemplates;
    private GridView gvTemplates;
    private StandardIconButton btnRefresh;

    public ConditionTrackingSetupControl(Sessions.Session session, ConditionType condType)
    {
      this.InitializeComponent();
      this.session = session;
      this.conditionType = condType;
      this.InitialiseControls();
    }

    private void InitialiseControls()
    {
      this.condSetup = this.session.ConfigurationManager.GetConditionTrackingSetup(this.conditionType);
      this.initTemplateList();
      this.loadTemplateList();
    }

    private void initTemplateList()
    {
      switch (this.condSetup.ConditionType)
      {
        case ConditionType.Underwriting:
          this.initUnderwritingList();
          break;
        case ConditionType.PostClosing:
          this.initPostClosingList();
          break;
        case ConditionType.Sell:
          this.initSellList();
          break;
      }
    }

    public List<string> SelectedTemplates
    {
      get
      {
        List<string> selectedTemplates = new List<string>();
        switch (this.condSetup.ConditionType)
        {
          case ConditionType.Underwriting:
            using (IEnumerator<GVItem> enumerator = this.gvTemplates.SelectedItems.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                UnderwritingConditionTemplate tag = (UnderwritingConditionTemplate) enumerator.Current.Tag;
                selectedTemplates.Add(tag.Name);
              }
              break;
            }
          case ConditionType.PostClosing:
            using (IEnumerator<GVItem> enumerator = this.gvTemplates.SelectedItems.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                PostClosingConditionTemplate tag = (PostClosingConditionTemplate) enumerator.Current.Tag;
                selectedTemplates.Add(tag.Name);
              }
              break;
            }
        }
        return selectedTemplates;
      }
    }

    public void SetSelectedTemplates(ConditionType type, List<string> selectedNames)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvTemplates.Items)
      {
        switch (type)
        {
          case ConditionType.Underwriting:
            gvItem.Selected = selectedNames.Contains(((ConditionTemplate) gvItem.Tag).Name);
            continue;
          case ConditionType.PostClosing:
            gvItem.Selected = selectedNames.Contains(((ConditionTemplate) gvItem.Tag).Name);
            continue;
          default:
            continue;
        }
      }
    }

    private void loadTemplateList()
    {
      this.gvTemplatesMgr.ClearItems();
      foreach (ConditionTemplate template in (CollectionBase) this.condSetup)
        this.gvTemplatesMgr.AddItem(template);
      this.gcTemplates.Text = "Conditions (" + (object) this.condSetup.Count + ")";
      this.gvTemplates.ReSort();
    }

    private void editSelectedItem()
    {
      if (this.gvTemplates.SelectedItems.Count != 1)
        return;
      switch (this.condSetup.ConditionType)
      {
        case ConditionType.Underwriting:
          this.editUnderwritingCondition();
          break;
        case ConditionType.PostClosing:
          this.editPostClosingCondition();
          break;
      }
    }

    private void gvTemplates_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.editSelectedItem();
    }

    private void gvTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvTemplates.SelectedItems.Count;
      this.btnEditTemplate.Enabled = count == 1;
      this.btnDeleteTemplate.Enabled = count > 0;
    }

    private void btnNewTemplate_Click(object sender, EventArgs e)
    {
      switch (this.condSetup.ConditionType)
      {
        case ConditionType.Underwriting:
          this.addUnderwritingCondition();
          break;
        case ConditionType.PostClosing:
          this.addPostClosingCondition();
          break;
      }
    }

    private void btnEditTemplate_Click(object sender, EventArgs e) => this.editSelectedItem();

    private void btnDeleteTemplate_Click(object sender, EventArgs e)
    {
      List<ConditionTemplate> conditionTemplates = new List<ConditionTemplate>();
      foreach (GVItem selectedItem in this.gvTemplates.SelectedItems)
        conditionTemplates.Add(selectedItem.Tag as ConditionTemplate);
      string str = string.Empty;
      foreach (ConditionTemplate conditionTemplate in conditionTemplates)
        str = str + conditionTemplate.Name + "\r\n";
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to permanently delete the following condition(s):\r\n\r\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No || conditionTemplates.Count <= 0)
        return;
      this.session.ConfigurationManager.DeleteConditionTrackingSetup(conditionTemplates);
      this.InitialiseControls();
    }

    private void initUnderwritingList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.session, this.gvTemplates, (LoanDataMgr) null);
      this.gvTemplatesMgr.CreateLayout(new TableLayout.Column[9]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.PriorToColumn,
        GridViewDataManager.OwnerColumn,
        GridViewDataManager.AllowToClearColumn,
        GridViewDataManager.PrintInternallyColumn,
        GridViewDataManager.GetPrintExternallyColumn(this.session),
        GridViewDataManager.DaysTillDueColumn
      });
      this.gvTemplates.Sort(0, SortOrder.Ascending);
    }

    private void addUnderwritingCondition()
    {
      using (UnderwritingTemplateDialog underwritingTemplateDialog = new UnderwritingTemplateDialog(this.condSetup, (UnderwritingConditionTemplate) null, this.session))
      {
        if (underwritingTemplateDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.InitialiseControls();
      }
    }

    private void editUnderwritingCondition()
    {
      using (UnderwritingTemplateDialog underwritingTemplateDialog = new UnderwritingTemplateDialog(this.condSetup, (UnderwritingConditionTemplate) this.gvTemplates.SelectedItems[0].Tag, this.session))
      {
        if (underwritingTemplateDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.InitialiseControls();
      }
    }

    private void initPostClosingList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.session, this.gvTemplates, (LoanDataMgr) null);
      this.gvTemplatesMgr.CreateLayout(new TableLayout.Column[7]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.RecipientColumn,
        GridViewDataManager.DaysTillDueColumn,
        GridViewDataManager.PrintInternallyColumn,
        GridViewDataManager.GetPrintExternallyColumn(this.session)
      });
      this.gvTemplates.Sort(0, SortOrder.Ascending);
    }

    private void addPostClosingCondition()
    {
      using (PostClosingTemplateDialog closingTemplateDialog = new PostClosingTemplateDialog(this.session, this.condSetup, (PostClosingConditionTemplate) null))
      {
        if (closingTemplateDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.InitialiseControls();
      }
    }

    private void editPostClosingCondition()
    {
      using (PostClosingTemplateDialog closingTemplateDialog = new PostClosingTemplateDialog(this.session, this.condSetup, (PostClosingConditionTemplate) this.gvTemplates.SelectedItems[0].Tag))
      {
        if (closingTemplateDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.InitialiseControls();
      }
    }

    private void initSellList()
    {
      this.gvTemplatesMgr = new GridViewDataManager(this.session, this.gvTemplates, (LoanDataMgr) null);
      this.gvTemplatesMgr.CreateLayout(new TableLayout.Column[7]
      {
        GridViewDataManager.NameColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.RecipientColumn,
        GridViewDataManager.DocStatusColumn,
        GridViewDataManager.DaysTillDueColumn
      });
      this.gvTemplates.Sort(0, SortOrder.Ascending);
    }

    private void addSellCondition()
    {
    }

    private void editSellCondition()
    {
    }

    private void btnRefresh_Click(object sender, EventArgs e) => this.InitialiseControls();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.toolTip = new ToolTip(this.components);
      this.btnDeleteTemplate = new StandardIconButton();
      this.btnEditTemplate = new StandardIconButton();
      this.btnNewTemplate = new StandardIconButton();
      this.btnRefresh = new StandardIconButton();
      this.gcTemplates = new GroupContainer();
      this.gvTemplates = new GridView();
      ((ISupportInitialize) this.btnDeleteTemplate).BeginInit();
      ((ISupportInitialize) this.btnEditTemplate).BeginInit();
      ((ISupportInitialize) this.btnNewTemplate).BeginInit();
      ((ISupportInitialize) this.btnRefresh).BeginInit();
      this.gcTemplates.SuspendLayout();
      this.SuspendLayout();
      this.btnDeleteTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeleteTemplate.BackColor = Color.Transparent;
      this.btnDeleteTemplate.Enabled = false;
      this.btnDeleteTemplate.Location = new Point(703, 4);
      this.btnDeleteTemplate.Margin = new Padding(4);
      this.btnDeleteTemplate.MouseDownImage = (Image) null;
      this.btnDeleteTemplate.Name = "btnDeleteTemplate";
      this.btnDeleteTemplate.Size = new Size(21, 20);
      this.btnDeleteTemplate.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteTemplate.TabIndex = 11;
      this.btnDeleteTemplate.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnDeleteTemplate, "Delete");
      this.btnDeleteTemplate.Click += new EventHandler(this.btnDeleteTemplate_Click);
      this.btnEditTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEditTemplate.BackColor = Color.Transparent;
      this.btnEditTemplate.Enabled = false;
      this.btnEditTemplate.Location = new Point(649, 4);
      this.btnEditTemplate.Margin = new Padding(4);
      this.btnEditTemplate.MouseDownImage = (Image) null;
      this.btnEditTemplate.Name = "btnEditTemplate";
      this.btnEditTemplate.Size = new Size(21, 20);
      this.btnEditTemplate.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditTemplate.TabIndex = 10;
      this.btnEditTemplate.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnEditTemplate, "Edit");
      this.btnEditTemplate.Click += new EventHandler(this.btnEditTemplate_Click);
      this.btnNewTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNewTemplate.BackColor = Color.Transparent;
      this.btnNewTemplate.Location = new Point(620, 4);
      this.btnNewTemplate.Margin = new Padding(4);
      this.btnNewTemplate.MouseDownImage = (Image) null;
      this.btnNewTemplate.Name = "btnNewTemplate";
      this.btnNewTemplate.Size = new Size(21, 20);
      this.btnNewTemplate.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNewTemplate.TabIndex = 9;
      this.btnNewTemplate.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnNewTemplate, "New");
      this.btnNewTemplate.Click += new EventHandler(this.btnNewTemplate_Click);
      this.btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRefresh.BackColor = Color.Transparent;
      this.btnRefresh.Location = new Point(677, 4);
      this.btnRefresh.Margin = new Padding(3, 3, 2, 3);
      this.btnRefresh.MouseDownImage = (Image) null;
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(21, 20);
      this.btnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.btnRefresh.TabIndex = 18;
      this.btnRefresh.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnRefresh, "Refresh");
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.gcTemplates.Controls.Add((Control) this.btnRefresh);
      this.gcTemplates.Controls.Add((Control) this.gvTemplates);
      this.gcTemplates.Controls.Add((Control) this.btnDeleteTemplate);
      this.gcTemplates.Controls.Add((Control) this.btnEditTemplate);
      this.gcTemplates.Controls.Add((Control) this.btnNewTemplate);
      this.gcTemplates.Dock = DockStyle.Fill;
      this.gcTemplates.HeaderForeColor = SystemColors.ControlText;
      this.gcTemplates.Location = new Point(0, 0);
      this.gcTemplates.Margin = new Padding(4);
      this.gcTemplates.Name = "gcTemplates";
      this.gcTemplates.Size = new Size(728, 368);
      this.gcTemplates.TabIndex = 13;
      this.gcTemplates.Text = "Conditions (#)";
      this.gvTemplates.BorderStyle = BorderStyle.None;
      this.gvTemplates.ClearSelectionsOnEmptyRowClick = false;
      this.gvTemplates.Dock = DockStyle.Fill;
      this.gvTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvTemplates.Location = new Point(1, 26);
      this.gvTemplates.Margin = new Padding(4);
      this.gvTemplates.Name = "gvTemplates";
      this.gvTemplates.Size = new Size(726, 341);
      this.gvTemplates.TabIndex = 12;
      this.gvTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvTemplates.SelectedIndexChanged += new EventHandler(this.gvTemplates_SelectedIndexChanged);
      this.gvTemplates.ItemDoubleClick += new GVItemEventHandler(this.gvTemplates_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcTemplates);
      this.Margin = new Padding(4);
      this.Name = nameof (ConditionTrackingSetupControl);
      this.Size = new Size(728, 368);
      ((ISupportInitialize) this.btnDeleteTemplate).EndInit();
      ((ISupportInitialize) this.btnEditTemplate).EndInit();
      ((ISupportInitialize) this.btnNewTemplate).EndInit();
      ((ISupportInitialize) this.btnRefresh).EndInit();
      this.gcTemplates.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
