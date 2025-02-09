// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.StandardConditionsConverter
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class StandardConditionsConverter : UserControl
  {
    private GridViewDataManager _gvDataManager;
    private const int AssignedTypeColumn = 0;
    private const int CategoryColumn = 2;
    private const int MaxTitleLength = 128;
    private IList<string> ecTemplateNames;
    private IList<EnhancedConditionType> selectableTypes;
    private IDictionary<ConditionTemplate, EnhancedConditionType> assignedTypes;
    private static readonly Dictionary<string, string> s_categoryMap = new Dictionary<string, string>()
    {
      {
        "Misc",
        "Miscellaneous"
      }
    };
    private static readonly Dictionary<string, string> s_priorToMap = new Dictionary<string, string>()
    {
      {
        "PTA",
        "Approval"
      },
      {
        "AC",
        "Closing"
      },
      {
        "PTD",
        "Docs"
      },
      {
        "PTF",
        "Funding"
      },
      {
        "PTP",
        "Purchase"
      }
    };
    private static StringComparison s_comparison = StringComparison.OrdinalIgnoreCase;
    private IContainer components;
    private GroupContainer gcConditionTemplates;
    private GridView gvConditionTemplates;
    private Label lblSelectSourceInstructions;
    private Panel panelRoot;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label lblStep1;
    private Label lblStep2;
    private ComboBox cmbType;
    private Button btnAssign;
    private Label lblStep3;
    private StandardConditionsConverter.StepLabel nbStep1;
    private StandardConditionsConverter.StepLabel nbStep2;
    private StandardConditionsConverter.StepLabel nbStep3;

    public event EventHandler<int> OnAssignmentChanged;

    public StandardConditionsConverter() => this.InitializeComponent();

    private bool IsSelectableType(EnhancedConditionType type)
    {
      return !type.title.Equals("Investor Delivery", StringComparison.OrdinalIgnoreCase);
    }

    public void Init(
      IEnumerable<ConditionTemplate> templates,
      ExportablePackage package,
      Sessions.Session session)
    {
      this.assignedTypes = (IDictionary<ConditionTemplate, EnhancedConditionType>) new Dictionary<ConditionTemplate, EnhancedConditionType>();
      this.ecTemplateNames = (IList<string>) package.Templates.Select<EnhancedConditionTemplate, string>((Func<EnhancedConditionTemplate, string>) (t => t.Title)).ToList<string>();
      IEnumerable<EnhancedConditionType> types = package.Types;
      this.selectableTypes = types != null ? (IList<EnhancedConditionType>) types.Where<EnhancedConditionType>(new Func<EnhancedConditionType, bool>(this.IsSelectableType)).OrderBy<EnhancedConditionType, string>((Func<EnhancedConditionType, string>) (t => t.title)).ToList<EnhancedConditionType>() : (IList<EnhancedConditionType>) null;
      this.cmbType.Items.Clear();
      this.cmbType.Items.Add((object) "<Unassigned>");
      foreach (EnhancedConditionType selectableType in (IEnumerable<EnhancedConditionType>) this.selectableTypes)
        this.cmbType.Items.Add((object) selectableType.title);
      List<ConditionTemplate> list = templates != null ? templates.ToList<ConditionTemplate>() : (List<ConditionTemplate>) null;
      this._gvDataManager = new GridViewDataManager(session, this.gvConditionTemplates, (LoanDataMgr) null);
      this._gvDataManager.CreateLayout(new TableLayout.Column[11]
      {
        new TableLayout.Column("ECTYPE", "Type", HorizontalAlignment.Left, 100),
        GridViewDataManager.NameColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.PriorToColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.RecipientColumn,
        GridViewDataManager.OwnerColumn,
        GridViewDataManager.AllowToClearColumn,
        GridViewDataManager.PrintInternallyColumn,
        GridViewDataManager.GetPrintExternallyColumn(session),
        GridViewDataManager.DaysTillDueColumn
      });
      this._gvDataManager.ClearItems();
      list.ForEach((Action<ConditionTemplate>) (i => this._gvDataManager.AddItem(i)));
      this.gvConditionTemplates.Sort(1, SortOrder.Ascending);
      this.RaiseAssignmentsChanged();
    }

    public IList<Guid> GetAssignedItemIDs()
    {
      List<Guid> assignedItemIds = new List<Guid>();
      bool flag1 = false;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditionTemplates.Items)
      {
        GVItem item = gvItem;
        bool flag2 = this.assignedTypes.ContainsKey(item.Tag as ConditionTemplate);
        bool flag3 = !Enumerable.Range(1, 5).Any<int>((Func<int, bool>) (i => item.SubItems[i].Value is ObjectWithImage objectWithImage && (bool) objectWithImage.Tag));
        if (flag2 && !flag3)
        {
          flag1 = true;
        }
        else
        {
          Guid result;
          // ISSUE: explicit non-virtual call
          if (flag2 & flag3 && Guid.TryParse(item.Tag is ConditionTemplate tag ? __nonvirtual (tag.Guid) : (string) null, out result))
            assignedItemIds.Add(result);
        }
      }
      if (flag1)
        assignedItemIds.Clear();
      return (IList<Guid>) assignedItemIds;
    }

    public ExportablePackage GetConversionPackage(IDictionary<string, string> documents)
    {
      List<EnhancedConditionType> enhancedConditionTypeList = new List<EnhancedConditionType>(this.assignedTypes.Values.Distinct<EnhancedConditionType>());
      List<string> source = new List<string>();
      List<EnhancedConditionTemplate> conditionTemplateList = new List<EnhancedConditionTemplate>();
      string empty = string.Empty;
      foreach (ConditionTemplate key in (IEnumerable<ConditionTemplate>) this.assignedTypes.Keys)
      {
        string[] documentIds = key.GetDocumentIDs();
        List<EntityReferenceContract> list = ((IEnumerable<string>) documentIds).Select<string, EntityReferenceContract>((Func<string, EntityReferenceContract>) (id => new EntityReferenceContract()
        {
          entityId = id
        })).ToList<EntityReferenceContract>();
        string targetTitle = this.GetTargetTitle(key.Name);
        string title = this.assignedTypes[key].title;
        EnhancedConditionTemplate conditionTemplate1 = new EnhancedConditionTemplate()
        {
          Active = new bool?(false),
          AllowDuplicate = new bool?(true),
          AssignedTo = list,
          ConditionType = title,
          customDefinitions = (ConditionDefinitionContract) null,
          CustomizeTypeDefinition = new bool?(false),
          EndDate = empty,
          ExternalDescription = empty,
          ExternalId = empty,
          Id = new Guid?(),
          InternalDescription = key.Description,
          InternalId = empty,
          StartDate = empty,
          Title = targetTitle
        };
        source.AddRange((IEnumerable<string>) documentIds);
        if (key is UnderwritingConditionTemplate)
        {
          UnderwritingConditionTemplate conditionTemplate2 = key as UnderwritingConditionTemplate;
          string category = conditionTemplate2.Category;
          string tpoCondDocGuid = conditionTemplate2.TPOCondDocGuid;
          ConnectSettingsContract settingsContract1 = new ConnectSettingsContract();
          settingsContract1.DocumentOption = conditionTemplate2.TPOCondDocType == "DefaultDoc" ? ConnectSettingsDocumentOptions.DefaultDocument.ToString() : ConnectSettingsDocumentOptions.MatchConditionName.ToString();
          EntityReferenceContract referenceContract;
          if (!string.IsNullOrEmpty(tpoCondDocGuid))
            referenceContract = new EntityReferenceContract()
            {
              entityId = conditionTemplate2.TPOCondDocGuid
            };
          else
            referenceContract = (EntityReferenceContract) null;
          settingsContract1.DocumentTemplate = referenceContract;
          ConnectSettingsContract settingsContract2 = settingsContract1;
          string priorTo = conditionTemplate2.PriorTo;
          string str1 = StandardConditionsConverter.s_categoryMap.ContainsKey(category) ? StandardConditionsConverter.s_categoryMap[category] : category;
          string str2 = string.IsNullOrEmpty(priorTo) ? empty : StandardConditionsConverter.s_priorToMap[priorTo];
          if (!string.IsNullOrEmpty(tpoCondDocGuid))
            source.Add(tpoCondDocGuid);
          if (conditionTemplate2.ForRoleID != 0)
            conditionTemplate1.Owner = new EntityReferenceContract()
            {
              entityId = conditionTemplate2.ForRoleID.ToString()
            };
          conditionTemplate1.Category = str1;
          conditionTemplate1.ConnectSettings = settingsContract2;
          conditionTemplate1.IsExternalPrint = conditionTemplate2.IsExternal;
          conditionTemplate1.IsInternalPrint = conditionTemplate2.IsInternal;
          conditionTemplate1.PriorTo = str2;
          conditionTemplate1.Recipient = empty;
          conditionTemplate1.Source = empty;
          if (conditionTemplate2.DaysTillDue != 0)
            conditionTemplate1.DaysToReceive = new int?(conditionTemplate2.DaysTillDue);
        }
        else
        {
          PostClosingConditionTemplate conditionTemplate3 = key as PostClosingConditionTemplate;
          conditionTemplate1.Category = empty;
          conditionTemplate1.ConnectSettings = (ConnectSettingsContract) null;
          conditionTemplate1.IsExternalPrint = conditionTemplate3.IsExternal;
          conditionTemplate1.IsInternalPrint = conditionTemplate3.IsInternal;
          conditionTemplate1.PriorTo = empty;
          conditionTemplate1.Recipient = conditionTemplate3.Recipient;
          conditionTemplate1.Source = conditionTemplate3.Source;
        }
        conditionTemplateList.Add(conditionTemplate1);
      }
      Dictionary<string, string> dictionary = source.Distinct<string>().ToDictionary<string, string, string>((Func<string, string>) (id => id), (Func<string, string>) (id => !documents.ContainsKey(id) ? id : documents[id]));
      return new ExportablePackage()
      {
        Types = (IEnumerable<EnhancedConditionType>) enhancedConditionTypeList,
        Templates = (IEnumerable<EnhancedConditionTemplate>) conditionTemplateList,
        Documents = (IDictionary<string, string>) dictionary
      };
    }

    private string GetTargetTitle(string sourceTitle)
    {
      return this.EnsureUnique(sourceTitle, this.ecTemplateNames, "Standard Condition - ");
    }

    private void RaiseAssignmentsChanged()
    {
      bool flag = true;
      foreach (GVItem row in (IEnumerable<GVItem>) this.gvConditionTemplates.Items)
        flag &= this.ValidateRow(row);
      this.gvConditionTemplates.Refresh();
      this.gcConditionTemplates.Text = string.Format("List of Standard Conditions ({0} of {1} assigned)", (object) this.assignedTypes.Count, (object) this.gvConditionTemplates.VisibleItems.Count);
      EventHandler<int> assignmentChanged = this.OnAssignmentChanged;
      if (assignmentChanged == null)
        return;
      assignmentChanged((object) this, flag ? 1 : 0);
    }

    private bool ValidateRow(GVItem row)
    {
      EnhancedConditionType type = this.SetAssignedTypeName(row);
      ConditionDefinitionContract definitions = type?.definitions;
      Func<int, GVSubItem> func = (Func<int, GVSubItem>) (offset => row.SubItems[2 + offset]);
      return this.ValidateTitle(func(-1), type) & this.ValidateOption(func(0), type, definitions?.categoryDefinitions, StandardConditionsConverter.s_categoryMap) & this.ValidateOption(func(1), type, definitions?.priorToDefinitions) & this.ValidateOption(func(2), type, definitions?.sourceDefinitions) & this.ValidateOption(func(3), type, definitions?.recipientDefinitions);
    }

    private EnhancedConditionType SetAssignedTypeName(GVItem row)
    {
      ConditionTemplate tag = row.Tag as ConditionTemplate;
      string str = string.Empty;
      EnhancedConditionType enhancedConditionType;
      if (this.assignedTypes.TryGetValue(tag, out enhancedConditionType))
        str = enhancedConditionType.title;
      row.SubItems[0].Text = str;
      return enhancedConditionType;
    }

    private bool ValidateTitle(GVSubItem column, EnhancedConditionType type)
    {
      string text = column.Text;
      string targetTitle = this.GetTargetTitle(text);
      bool isValid = type == null || targetTitle.Length <= 128;
      string hoverText = (type == null ? 1 : (targetTitle.Length == text.Length ? 1 : 0)) != 0 ? (string) null : "Template name already exists, and converted condition will be auto-renamed to '" + targetTitle + "'";
      if (!isValid)
        hoverText = text.Length > 128 ? string.Format("Template name of converted condition exceeds limit of {0} characters.", (object) 128) : string.Format("Template name already exists, and auto-rename will exceed limit of {0} characters.", (object) 128);
      this.SetItemText(column, text, hoverText, isValid);
      return isValid;
    }

    private bool ValidateOption(
      GVSubItem column,
      EnhancedConditionType type,
      OptionDefinitionContract[] options,
      Dictionary<string, string> aliases = null)
    {
      options = options ?? new OptionDefinitionContract[0];
      aliases = aliases ?? new Dictionary<string, string>();
      string text = column.Text;
      bool isValid = type == null || string.IsNullOrEmpty(text) || ((IEnumerable<OptionDefinitionContract>) options).Any<OptionDefinitionContract>((Func<OptionDefinitionContract, bool>) (c => c.name.Equals(text, StandardConditionsConverter.s_comparison))) || aliases.Any<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (kvp => kvp.Key.Equals(text, StandardConditionsConverter.s_comparison) && ((IEnumerable<OptionDefinitionContract>) options).Any<OptionDefinitionContract>((Func<OptionDefinitionContract, bool>) (c => c.name.Equals(kvp.Value, StandardConditionsConverter.s_comparison)))));
      string hoverText = isValid ? (string) null : "Invalid value for assigned condition type.\nEither unassign, or assign to another condition type.";
      this.SetItemText(column, text, hoverText, isValid);
      return isValid;
    }

    private void SetItemText(GVSubItem column, string text, string hoverText, bool isValid)
    {
      if (isValid && string.IsNullOrEmpty(hoverText))
      {
        column.Value = (object) null;
        column.Value = (object) text;
        column.HoverText = (string) null;
      }
      else
      {
        Bitmap bitmap = isValid ? Resources.audit_orange_icon : Resources.audit_red_icon;
        ObjectWithImage objectWithImage1 = new ObjectWithImage(text, (Image) bitmap);
        objectWithImage1.Tag = (object) !isValid;
        ObjectWithImage objectWithImage2 = objectWithImage1;
        column.Value = (object) null;
        column.Value = (object) objectWithImage2;
        column.HoverText = hoverText;
      }
      column.SortValue = (object) text;
    }

    private void btnAssign_Click(object sender, EventArgs e)
    {
      string typeName = this.cmbType.SelectedItem?.ToString();
      EnhancedConditionType enhancedConditionType = this.cmbType.SelectedIndex < 1 ? (EnhancedConditionType) null : this.selectableTypes.First<EnhancedConditionType>((Func<EnhancedConditionType, bool>) (t => t.title.Equals(typeName)));
      foreach (GVItem selectedItem in this.gvConditionTemplates.SelectedItems)
      {
        ConditionTemplate tag = selectedItem.Tag as ConditionTemplate;
        if (this.assignedTypes.ContainsKey(tag))
          this.assignedTypes.Remove(tag);
        if (enhancedConditionType != null)
          this.assignedTypes.Add(tag, enhancedConditionType);
      }
      this.RaiseAssignmentsChanged();
    }

    internal string EnsureUnique(string proposed, IList<string> existing, string prefix = "")
    {
      return !existing.Contains<string>(proposed, (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase) ? proposed : this.Disambiguate(prefix + proposed, existing);
    }

    internal string Disambiguate(string proposed, IList<string> existing, string pattern = "$0 ($1)")
    {
      int val2 = 0;
      Regex regex = new Regex(Regex.Escape(pattern.Replace("$0", proposed)).Replace("\\$1", "(\\d+)"));
      foreach (string input in (IEnumerable<string>) existing)
      {
        if (input.Equals(proposed, StringComparison.OrdinalIgnoreCase))
        {
          val2 = Math.Max(1, val2);
        }
        else
        {
          Match match = regex.Match(input);
          int result;
          if (match.Success && match.Groups.Count == 2 && int.TryParse(match.Groups[1].Value, out result))
            val2 = Math.Max(1 + result, val2);
        }
      }
      return val2 != 0 ? pattern.Replace("$0", proposed).Replace("$1", val2.ToString()) : proposed;
    }

    public bool TestEnsureUnique()
    {
      this.TestDisambiguate();
      return true;
    }

    public bool TestDisambiguate() => true;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gcConditionTemplates = new GroupContainer();
      this.gvConditionTemplates = new GridView();
      this.lblSelectSourceInstructions = new Label();
      this.panelRoot = new Panel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.nbStep1 = new StandardConditionsConverter.StepLabel();
      this.lblStep1 = new Label();
      this.nbStep2 = new StandardConditionsConverter.StepLabel();
      this.lblStep2 = new Label();
      this.cmbType = new ComboBox();
      this.btnAssign = new Button();
      this.nbStep3 = new StandardConditionsConverter.StepLabel();
      this.lblStep3 = new Label();
      this.gcConditionTemplates.SuspendLayout();
      this.panelRoot.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.gcConditionTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.gcConditionTemplates.AutoScroll = true;
      this.gcConditionTemplates.Controls.Add((Control) this.gvConditionTemplates);
      this.gcConditionTemplates.HeaderForeColor = SystemColors.ControlText;
      this.gcConditionTemplates.Location = new Point(4, 87);
      this.gcConditionTemplates.Margin = new Padding(0);
      this.gcConditionTemplates.Name = "gcConditionTemplates";
      this.gcConditionTemplates.Size = new Size(1895, 473);
      this.gcConditionTemplates.TabIndex = 1;
      this.gcConditionTemplates.Text = "List of Conditions ";
      this.gvConditionTemplates.BorderStyle = BorderStyle.None;
      this.gvConditionTemplates.ClearSelectionsOnEmptyRowClick = false;
      this.gvConditionTemplates.Dock = DockStyle.Fill;
      this.gvConditionTemplates.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvConditionTemplates.Location = new Point(1, 26);
      this.gvConditionTemplates.Margin = new Padding(4, 5, 4, 5);
      this.gvConditionTemplates.Name = "gvConditionTemplates";
      this.gvConditionTemplates.Size = new Size(1893, 446);
      this.gvConditionTemplates.TabIndex = 1;
      this.gvConditionTemplates.TextTrimming = StringTrimming.EllipsisCharacter;
      this.lblSelectSourceInstructions.AutoSize = true;
      this.lblSelectSourceInstructions.Location = new Point(0, 3);
      this.lblSelectSourceInstructions.Margin = new Padding(0);
      this.lblSelectSourceInstructions.Name = "lblSelectSourceInstructions";
      this.lblSelectSourceInstructions.Size = new Size(907, 20);
      this.lblSelectSourceInstructions.TabIndex = 2;
      this.lblSelectSourceInstructions.Text = "Please assign Enhanced Condition Types for all Standard Conditions that are to be upgraded to Enhanced Condition Templates";
      this.panelRoot.Controls.Add((Control) this.flowLayoutPanel1);
      this.panelRoot.Controls.Add((Control) this.gcConditionTemplates);
      this.panelRoot.Controls.Add((Control) this.lblSelectSourceInstructions);
      this.panelRoot.Dock = DockStyle.Fill;
      this.panelRoot.Location = new Point(0, 0);
      this.panelRoot.Name = "panelRoot";
      this.panelRoot.Size = new Size(1899, 560);
      this.panelRoot.TabIndex = 0;
      this.flowLayoutPanel1.Controls.Add((Control) this.nbStep1);
      this.flowLayoutPanel1.Controls.Add((Control) this.lblStep1);
      this.flowLayoutPanel1.Controls.Add((Control) this.nbStep2);
      this.flowLayoutPanel1.Controls.Add((Control) this.lblStep2);
      this.flowLayoutPanel1.Controls.Add((Control) this.cmbType);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAssign);
      this.flowLayoutPanel1.Controls.Add((Control) this.nbStep3);
      this.flowLayoutPanel1.Controls.Add((Control) this.lblStep3);
      this.flowLayoutPanel1.Location = new Point(0, 42);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(1899, 42);
      this.flowLayoutPanel1.TabIndex = 0;
      this.nbStep1.Font = new Font("Segoe UI", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.nbStep1.ImageIndex = 0;
      this.nbStep1.Location = new Point(2, 0);
      this.nbStep1.Margin = new Padding(2, 0, 2, 0);
      this.nbStep1.MinimumSize = new Size(32, 32);
      this.nbStep1.Name = "nbStep1";
      this.nbStep1.Size = new Size(32, 32);
      this.nbStep1.TabIndex = 0;
      this.nbStep1.Text = "1";
      this.nbStep1.TextAlign = ContentAlignment.MiddleCenter;
      this.lblStep1.AutoSize = true;
      this.lblStep1.Location = new Point(36, 0);
      this.lblStep1.Margin = new Padding(0);
      this.lblStep1.Name = "lblStep1";
      this.lblStep1.Padding = new Padding(0, 6, 0, 6);
      this.lblStep1.Size = new Size(238, 32);
      this.lblStep1.TabIndex = 1;
      this.lblStep1.Text = "Select conditions to be assigned";
      this.nbStep2.Font = new Font("Segoe UI", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.nbStep2.ImageIndex = 0;
      this.nbStep2.Location = new Point(290, 0);
      this.nbStep2.Margin = new Padding(16, 0, 2, 0);
      this.nbStep2.MinimumSize = new Size(32, 32);
      this.nbStep2.Name = "nbStep2";
      this.nbStep2.Size = new Size(32, 32);
      this.nbStep2.TabIndex = 2;
      this.nbStep2.Text = "2";
      this.nbStep2.TextAlign = ContentAlignment.MiddleCenter;
      this.lblStep2.AutoSize = true;
      this.lblStep2.Location = new Point(324, 0);
      this.lblStep2.Margin = new Padding(0);
      this.lblStep2.Name = "lblStep2";
      this.lblStep2.Padding = new Padding(0, 6, 0, 6);
      this.lblStep2.Size = new Size(228, 32);
      this.lblStep2.TabIndex = 3;
      this.lblStep2.Text = "Choose a type and click Assign";
      this.cmbType.FormattingEnabled = true;
      this.cmbType.Location = new Point(554, 2);
      this.cmbType.Margin = new Padding(2);
      this.cmbType.Name = "cmbType";
      this.cmbType.Size = new Size(240, 28);
      this.cmbType.TabIndex = 2;
      this.btnAssign.Location = new Point(798, 0);
      this.btnAssign.Margin = new Padding(2, 0, 2, 0);
      this.btnAssign.Name = "btnAssign";
      this.btnAssign.Size = new Size(75, 36);
      this.btnAssign.TabIndex = 3;
      this.btnAssign.Text = "Assign";
      this.btnAssign.UseVisualStyleBackColor = true;
      this.btnAssign.Click += new EventHandler(this.btnAssign_Click);
      this.nbStep3.Font = new Font("Segoe UI", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.nbStep3.ImageIndex = 0;
      this.nbStep3.Location = new Point(891, 0);
      this.nbStep3.Margin = new Padding(16, 0, 2, 0);
      this.nbStep3.MinimumSize = new Size(32, 32);
      this.nbStep3.Name = "nbStep3";
      this.nbStep3.Size = new Size(32, 32);
      this.nbStep3.TabIndex = 4;
      this.nbStep3.Text = "3";
      this.nbStep3.TextAlign = ContentAlignment.MiddleCenter;
      this.lblStep3.AutoSize = true;
      this.lblStep3.Location = new Point(925, 0);
      this.lblStep3.Margin = new Padding(0);
      this.lblStep3.Name = "lblStep3";
      this.lblStep3.Padding = new Padding(0, 6, 0, 6);
      this.lblStep3.Size = new Size(369, 32);
      this.lblStep3.TabIndex = 5;
      this.lblStep3.Text = "Repeat steps 1 and 2 as necessary, then click Next";
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.panelRoot);
      this.Name = nameof (StandardConditionsConverter);
      this.Size = new Size(1899, 560);
      this.gcConditionTemplates.ResumeLayout(false);
      this.panelRoot.ResumeLayout(false);
      this.panelRoot.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    public class StepLabel : Label
    {
      private int Rgb2Argb(int c) => (int) (4278190080L + (long) c);

      protected override void OnPaint(PaintEventArgs e)
      {
        Graphics graphics = e.Graphics;
        Rectangle clipRectangle = e.ClipRectangle;
        Rectangle rect = new Rectangle(clipRectangle.X, clipRectangle.Y, clipRectangle.Width - 1, clipRectangle.Height - 1);
        Pen pen = new Pen((Brush) new SolidBrush(Color.FromArgb(this.Rgb2Argb(0))), 1f);
        SolidBrush solidBrush = new SolidBrush(Color.FromArgb(this.Rgb2Argb(11992832)));
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        graphics.FillEllipse((Brush) solidBrush, rect);
        graphics.DrawEllipse(pen, rect);
        base.OnPaint(e);
      }
    }
  }
}
