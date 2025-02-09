// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.ValidationViewer
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
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
  public class ValidationViewer : UserControl
  {
    private const int ValidationResultColumn = 6;
    private const string ValidationResult_Valid = "Valid";
    private const string ValidationResult_Invalid = "Invalid";
    private const string ValidationResult_Duplicate = "Duplicate";
    public static readonly StringComparison s_comparison = StringComparison.InvariantCultureIgnoreCase;
    private IContainer components;
    private Panel panelRoot;
    private Button btnSelectAllConditions;
    private GroupContainer gcValidationSummary;
    private GridView gvValidationSummary;
    private Label lblSelectSourceInstructions;

    public event EventHandler<int> OnSelectionChanged;

    public PackageValidator Validator { get; set; }

    private ExportablePackage SyncPackage { get; set; }

    private GridViewDataManager ViewManager { get; set; }

    public ValidationViewer() => this.InitializeComponent();

    public void SetMessage(string message) => this.lblSelectSourceInstructions.Text = message;

    public IList<Guid?> GetSelectedItemIDs()
    {
      return (IList<Guid?>) this.gvValidationSummary.SelectedItems.Select<GVItem, Guid?>((Func<GVItem, Guid?>) (i => !(i.Tag is EnhancedConditionTemplate tag) ? new Guid?() : tag.Id)).ToList<Guid?>();
    }

    public ExportablePackage GetSyncPackage()
    {
      ExportablePackage syncPackage = new ExportablePackage();
      syncPackage.Documents = this.SyncPackage.Documents;
      syncPackage.Roles = this.SyncPackage.Roles;
      syncPackage.Types = this.SyncPackage.Types;
      syncPackage.Owner = this.SyncPackage.Owner;
      List<string> selectedTemplateIDs = this.gvValidationSummary.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (i =>
      {
        if (!(i.Tag is EnhancedConditionTemplate tag2))
          return (string) null;
        Guid? id = tag2.Id;
        ref Guid? local = ref id;
        return !local.HasValue ? (string) null : local.GetValueOrDefault().ToString();
      })).ToList<string>();
      syncPackage.Templates = (IEnumerable<EnhancedConditionTemplate>) this.SyncPackage.Templates.Where<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => selectedTemplateIDs.Contains(t.Id.ToString()))).Select<EnhancedConditionTemplate, EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, EnhancedConditionTemplate>) (t => t.DeepClone(true) as EnhancedConditionTemplate)).ToList<EnhancedConditionTemplate>();
      return syncPackage;
    }

    public void Init(
      ExportablePackage toValidate,
      PackageValidator validator,
      Sessions.Session session)
    {
      this.SyncPackage = toValidate;
      this.Validator = validator;
      List<ValidationError> errors = validator.ValidateImport(toValidate);
      IEnumerable<EnhancedConditionTemplate> templates = toValidate.Templates;
      this.ViewManager = new GridViewDataManager(session, this.gvValidationSummary, (LoanDataMgr) null);
      this.ViewManager.ClearItems();
      templates.ToList<EnhancedConditionTemplate>().ForEach((Action<EnhancedConditionTemplate>) (t => this.ViewManager.AddItem(t, !errors.Any<ValidationError>((Func<ValidationError, bool>) (err =>
      {
        Guid? templateId = err.TemplateID;
        Guid? id = t.Id;
        if (templateId.HasValue != id.HasValue)
          return false;
        return !templateId.HasValue || templateId.GetValueOrDefault() == id.GetValueOrDefault();
      })))));
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvValidationSummary.Items)
        this.SetValidationSummary(gvItem, (IEnumerable<ValidationError>) errors, validator.Target);
      this.gvValidationSummary.Sort(6, SortOrder.Descending);
      this.RaiseSelectionChanged();
    }

    private void SetValidationSummary(
      GVItem item,
      IEnumerable<ValidationError> errors,
      ExportablePackage target)
    {
      EnhancedConditionTemplate template = item.Tag as EnhancedConditionTemplate;
      Guid? templateID = template.Id;
      int num = !templateID.HasValue ? 0 : (errors.Any<ValidationError>((Func<ValidationError, bool>) (e =>
      {
        Guid? templateId = e.TemplateID;
        Guid? nullable = templateID;
        if (templateId.HasValue != nullable.HasValue)
          return false;
        return !templateId.HasValue || templateId.GetValueOrDefault() == nullable.GetValueOrDefault();
      })) ? 1 : 0);
      bool flag = target.Templates.Any<EnhancedConditionTemplate>((Func<EnhancedConditionTemplate, bool>) (t => t.Title.Equals(template.Title, ValidationViewer.s_comparison) && t.ConditionType.Equals(template.ConditionType, ValidationViewer.s_comparison)));
      string text = num != 0 ? "Invalid" : (flag ? "Duplicate" : "Valid");
      Bitmap bitmap = num != 0 ? Resources.audit_red_icon : (flag ? Resources.audit_orange_icon : Resources.audit_green_icon);
      string str = string.Join("\n", errors.Where<ValidationError>((Func<ValidationError, bool>) (e =>
      {
        Guid? templateId = e.TemplateID;
        Guid? nullable = templateID;
        if (templateId.HasValue != nullable.HasValue)
          return false;
        return !templateId.HasValue || templateId.GetValueOrDefault() == nullable.GetValueOrDefault();
      })).Select<ValidationError, string>((Func<ValidationError, string>) (e => e.Message)));
      item.SubItems[6].Value = (object) new ObjectWithImage(text, (Image) bitmap);
      item.SubItems[6].SortValue = text != "Valid" ? (object) text : (object) "";
      item.SubItems[7].Text = str;
      if ((num | (flag ? 1 : 0)) == 0)
        return;
      item.Selected = false;
    }

    private void RaiseSelectionChanged()
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvValidationSummary.Items)
      {
        if (gvItem.SubItems[6].Text == "Invalid")
          gvItem.Selected = false;
      }
      int count1 = this.gvValidationSummary.VisibleItems.Count;
      int count2 = this.gvValidationSummary.SelectedItems.Count;
      this.gcValidationSummary.Text = string.Format("Enhanced Conditions to sync ({0} of {1} selected)", (object) count2, (object) count1);
      EventHandler<int> selectionChanged = this.OnSelectionChanged;
      if (selectionChanged == null)
        return;
      selectionChanged((object) this, count2);
    }

    private void gvConditionTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.RaiseSelectionChanged();
    }

    private void btnSelectAllConditions_Click(object sender, EventArgs e)
    {
      this.gvValidationSummary.VisibleItems.SelectAll();
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
      this.panelRoot = new Panel();
      this.btnSelectAllConditions = new Button();
      this.gcValidationSummary = new GroupContainer();
      this.gvValidationSummary = new GridView();
      this.lblSelectSourceInstructions = new Label();
      this.panelRoot.SuspendLayout();
      this.gcValidationSummary.SuspendLayout();
      this.SuspendLayout();
      this.panelRoot.Controls.Add((Control) this.btnSelectAllConditions);
      this.panelRoot.Controls.Add((Control) this.gcValidationSummary);
      this.panelRoot.Controls.Add((Control) this.lblSelectSourceInstructions);
      this.panelRoot.Dock = DockStyle.Fill;
      this.panelRoot.Location = new Point(0, 0);
      this.panelRoot.Name = "panelRoot";
      this.panelRoot.Size = new Size(1864, 904);
      this.panelRoot.TabIndex = 9;
      this.btnSelectAllConditions.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSelectAllConditions.Location = new Point(1735, 3);
      this.btnSelectAllConditions.Name = "btnSelectAllConditions";
      this.btnSelectAllConditions.Size = new Size(129, 33);
      this.btnSelectAllConditions.TabIndex = 7;
      this.btnSelectAllConditions.Text = "Select All";
      this.btnSelectAllConditions.UseVisualStyleBackColor = true;
      this.btnSelectAllConditions.Click += new EventHandler(this.btnSelectAllConditions_Click);
      this.gcValidationSummary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcValidationSummary.AutoScroll = true;
      this.gcValidationSummary.Controls.Add((Control) this.gvValidationSummary);
      this.gcValidationSummary.HeaderForeColor = SystemColors.ControlText;
      this.gcValidationSummary.Location = new Point(4, 44);
      this.gcValidationSummary.Margin = new Padding(0);
      this.gcValidationSummary.Name = "gcValidationSummary";
      this.gcValidationSummary.Size = new Size(1860, 860);
      this.gcValidationSummary.TabIndex = 5;
      this.gcValidationSummary.Text = "List of Conditions ";
      this.gvValidationSummary.BorderStyle = BorderStyle.None;
      this.gvValidationSummary.ClearSelectionsOnEmptyRowClick = false;
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
      gvColumn6.Name = "Customized";
      gvColumn6.Tag = (object) "CUSTOMIZED";
      gvColumn6.Text = "Customized";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ValidationStatus";
      gvColumn7.Tag = (object) "VALIDATIONSTATUS";
      gvColumn7.Text = "Validation Status";
      gvColumn7.Width = 108;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Errors";
      gvColumn8.Tag = (object) "VALIDATIONERRORS";
      gvColumn8.Text = "Errors";
      gvColumn8.Width = 100;
      this.gvValidationSummary.Columns.AddRange(new GVColumn[8]
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
      this.gvValidationSummary.Dock = DockStyle.Fill;
      this.gvValidationSummary.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvValidationSummary.Location = new Point(1, 26);
      this.gvValidationSummary.Margin = new Padding(4, 5, 4, 5);
      this.gvValidationSummary.Name = "gvValidationSummary";
      this.gvValidationSummary.Size = new Size(1858, 833);
      this.gvValidationSummary.TabIndex = 1;
      this.gvValidationSummary.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvValidationSummary.SelectedIndexChanged += new EventHandler(this.gvConditionTemplates_SelectedIndexChanged);
      this.lblSelectSourceInstructions.AutoSize = true;
      this.lblSelectSourceInstructions.Location = new Point(0, 3);
      this.lblSelectSourceInstructions.Margin = new Padding(0);
      this.lblSelectSourceInstructions.Name = "lblSelectSourceInstructions";
      this.lblSelectSourceInstructions.Size = new Size(471, 20);
      this.lblSelectSourceInstructions.TabIndex = 6;
      this.lblSelectSourceInstructions.Text = "Please review and confirm condition templates to be synchronized";
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelRoot);
      this.Name = nameof (ValidationViewer);
      this.Size = new Size(1864, 904);
      this.panelRoot.ResumeLayout(false);
      this.panelRoot.PerformLayout();
      this.gcValidationSummary.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
