// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.AddBlankEnhancedConditionDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.LoanUtils.EnhancedConditions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class AddBlankEnhancedConditionDialog : Form
  {
    private const string className = "AddBlankEnhancedConditionDialog";
    private static readonly string sw = Tracing.SwEFolder;
    private LoanDataMgr loanDataMgr;
    private eFolderManager eFolderMgr;
    private eFolderAccessRights rights;
    private EnhancedConditionType[] enhancedConditionTypes;
    private EnhancedConditionTemplate[] conditionTemplates;
    private List<EnhancedConditionTemplate> templatesToAddList = new List<EnhancedConditionTemplate>();
    private bool openEditDetailDialog;
    private bool isAdHocCondition;
    private IContainer components;
    private Button btnCancel;
    private Button btnAdd;
    private ComboBox cboBorrower;
    private Label label2;
    private Label label1;
    private ComboBox cboConditionType;
    private ComboBox cboConditionName;
    private Label label3;
    private ToolTip tooltip;

    public bool IsAdHocCondition => this.isAdHocCondition;

    public bool OpenEditDetailDialog => this.openEditDetailDialog;

    public AddBlankEnhancedConditionDialog(
      LoanDataMgr loanDataMgr,
      EnhancedConditionTemplate[] conditionTemplates,
      EnhancedConditionType[] conditionTypes)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.conditionTemplates = conditionTemplates;
      this.enhancedConditionTypes = conditionTypes;
      this.eFolderMgr = new eFolderManager();
      this.rights = new eFolderAccessRights(this.loanDataMgr);
      this.initBorrowerField();
      this.initConditionTypeField();
      this.initConditionNameField();
    }

    public EnhancedConditionTemplate[] TemplatesToAdd => this.templatesToAddList.ToArray();

    public string PairId => ((BorrowerPair) this.cboBorrower.SelectedItem).Id;

    private void initBorrowerField()
    {
      this.cboBorrower.Items.AddRange((object[]) this.loanDataMgr.LoanData.GetBorrowerPairs());
      this.cboBorrower.Items.Add((object) BorrowerPair.All);
      this.cboBorrower.SelectedItem = (object) BorrowerPair.All;
    }

    private void initConditionTypeField()
    {
      foreach (EnhancedConditionType enhancedConditionType in this.enhancedConditionTypes)
      {
        foreach (EnhancedConditionTemplate conditionTemplate in this.conditionTemplates)
        {
          if (this.isTemplateActive(conditionTemplate) && enhancedConditionType.title == conditionTemplate.ConditionType && this.rights.CanAddBlankEnhancedCondition(enhancedConditionType.title) && !this.cboConditionType.Items.Contains((object) enhancedConditionType))
            this.cboConditionType.Items.Add((object) enhancedConditionType);
        }
      }
    }

    private void initConditionNameField()
    {
      this.cboConditionName.Items.Clear();
      this.cboConditionName.Text = string.Empty;
      if (this.cboConditionType.SelectedItem != null)
      {
        EnhancedConditionType selectedItem = (EnhancedConditionType) this.cboConditionType.SelectedItem;
        foreach (EnhancedConditionTemplate conditionTemplate in this.conditionTemplates)
        {
          if (this.isTemplateActive(conditionTemplate) && selectedItem.title == conditionTemplate.ConditionType && this.rights.CanAddBlankEnhancedCondition(conditionTemplate.ConditionType) && this.allowedOnLoan(conditionTemplate))
            this.cboConditionName.Items.Add((object) conditionTemplate);
        }
      }
      else
        this.cboConditionName.Enabled = false;
    }

    private bool isTemplateActive(EnhancedConditionTemplate template)
    {
      return this.eFolderMgr.IsEnhancedConditionTemplateActive(template);
    }

    private bool allowedOnLoan(EnhancedConditionTemplate template)
    {
      return this.eFolderMgr.IsEnhancedConditionAllowedOnLoan(this.loanDataMgr, template);
    }

    private void cboBorrower_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.setButtons();
    }

    private void cboConditionName_MouseHover(object sender, EventArgs e)
    {
      this.tooltip.SetToolTip((Control) this.cboConditionName, this.cboConditionName.Text);
    }

    private void cboConditionType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.initConditionNameField();
      this.setButtons();
    }

    private void cboConditionName_TextChanged(object sender, EventArgs e) => this.setButtons();

    private void setButtons()
    {
      this.btnAdd.Enabled = this.cboBorrower.SelectedIndex != -1 && this.cboConditionType.SelectedIndex != -1 && !string.IsNullOrEmpty(this.cboConditionName.Text);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.openEditDetailDialog = false;
      EnhancedConditionTemplate selectedItem = (EnhancedConditionTemplate) this.cboConditionName.SelectedItem;
      if (selectedItem != null)
      {
        this.templatesToAddList.Add(selectedItem);
        this.openEditDetailDialog = true;
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        string templateName = this.cboConditionName.Text.Trim();
        if (string.IsNullOrEmpty(templateName))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select or enter a valid Condition Name.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          foreach (EnhancedConditionTemplate conditionTemplate in this.cboConditionName.Items)
          {
            if (templateName == conditionTemplate.Title)
            {
              this.templatesToAddList.Add(conditionTemplate);
              this.openEditDetailDialog = true;
              this.DialogResult = DialogResult.OK;
              return;
            }
          }
          foreach (EnhancedConditionTemplate conditionTemplate in this.conditionTemplates)
          {
            if (templateName == conditionTemplate.Title)
            {
              if (!this.isTemplateActive(conditionTemplate))
              {
                int num2 = (int) Utils.Dialog((IWin32Window) this, "Condition Name entered is for an inactive template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
              if (!this.rights.CanAddBlankEnhancedCondition(conditionTemplate.ConditionType))
              {
                int num3 = (int) Utils.Dialog((IWin32Window) this, "Adding Condition for Name entered is not permitted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
              if (!this.allowedOnLoan(conditionTemplate))
              {
                int num4 = (int) Utils.Dialog((IWin32Window) this, "Condition Name entered cannot be duplicated on the loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
              }
              this.templatesToAddList.Add(conditionTemplate);
              this.openEditDetailDialog = true;
              this.DialogResult = DialogResult.OK;
              return;
            }
          }
          this.openEditDetailDialog = true;
          this.isAdHocCondition = true;
          this.templatesToAddList.Add(this.getNewTemplateDefinition(templateName, (EnhancedConditionType) this.cboConditionType.SelectedItem));
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private EnhancedConditionTemplate getNewTemplateDefinition(
      string templateName,
      EnhancedConditionType condType)
    {
      EnhancedConditionTemplate templateDefinition = new EnhancedConditionTemplate();
      templateDefinition.Active = new bool?(true);
      templateDefinition.ConditionType = condType.title;
      templateDefinition.Title = templateName;
      templateDefinition.CustomizeTypeDefinition = new bool?(false);
      templateDefinition.ConnectSettings = new ConnectSettingsContract()
      {
        DocumentOption = ConnectSettingsDocumentOptions.MatchConditionName.ToString(),
        DocumentTemplate = (EntityReferenceContract) null
      };
      if (condType.definitions == null)
        condType = new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTypeDetails(condType.id);
      if (condType != null && condType.definitions != null)
      {
        if (condType.definitions.categoryDefinitions != null && condType.definitions.categoryDefinitions.Length == 1)
          templateDefinition.Category = condType.definitions.categoryDefinitions[0].name;
        if (condType.definitions.priorToDefinitions != null && condType.definitions.priorToDefinitions.Length == 1)
          templateDefinition.PriorTo = condType.definitions.priorToDefinitions[0].name;
        if (condType.definitions.recipientDefinitions != null && condType.definitions.recipientDefinitions.Length == 1)
          templateDefinition.Recipient = condType.definitions.recipientDefinitions[0].name;
        if (condType.definitions.sourceDefinitions != null && condType.definitions.sourceDefinitions.Length == 1)
          templateDefinition.Source = condType.definitions.sourceDefinitions[0].name;
      }
      return templateDefinition;
    }

    private void cboConditionType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.cboConditionName.Enabled = !string.IsNullOrEmpty(this.cboConditionType.Text);
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
      this.btnCancel = new Button();
      this.btnAdd = new Button();
      this.cboBorrower = new ComboBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.cboConditionType = new ComboBox();
      this.cboConditionName = new ComboBox();
      this.label3 = new Label();
      this.tooltip = new ToolTip(this.components);
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(143, 183);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnAdd.Enabled = false;
      this.btnAdd.Location = new Point(63, 183);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 2;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.cboBorrower.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Location = new Point(12, 25);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(206, 22);
      this.cboBorrower.TabIndex = 79;
      this.cboBorrower.SelectionChangeCommitted += new EventHandler(this.cboBorrower_SelectionChangeCommitted);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 9);
      this.label2.Name = "label2";
      this.label2.Size = new Size(94, 14);
      this.label2.TabIndex = 78;
      this.label2.Text = "For Borrower Pair";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 60);
      this.label1.Name = "label1";
      this.label1.Size = new Size(77, 14);
      this.label1.TabIndex = 81;
      this.label1.Text = "Condition Type";
      this.cboConditionType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConditionType.FormattingEnabled = true;
      this.cboConditionType.Location = new Point(12, 77);
      this.cboConditionType.Name = "cboConditionType";
      this.cboConditionType.Size = new Size(206, 22);
      this.cboConditionType.TabIndex = 82;
      this.cboConditionType.SelectedIndexChanged += new EventHandler(this.cboConditionType_SelectedIndexChanged);
      this.cboConditionType.SelectionChangeCommitted += new EventHandler(this.cboConditionType_SelectionChangeCommitted);
      this.cboConditionName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboConditionName.FormattingEnabled = true;
      this.cboConditionName.Location = new Point(12, 135);
      this.cboConditionName.MaxLength = (int) byte.MaxValue;
      this.cboConditionName.Name = "cboConditionName";
      this.cboConditionName.Size = new Size(206, 22);
      this.cboConditionName.TabIndex = 84;
      this.cboConditionName.TextChanged += new EventHandler(this.cboConditionName_TextChanged);
      this.cboConditionName.MouseHover += new EventHandler(this.cboConditionName_MouseHover);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 118);
      this.label3.Name = "label3";
      this.label3.Size = new Size(81, 14);
      this.label3.TabIndex = 83;
      this.label3.Text = "Condition Name";
      this.AcceptButton = (IButtonControl) this.btnAdd;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(230, 226);
      this.Controls.Add((Control) this.cboConditionName);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.cboConditionType);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboBorrower);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnAdd);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(2, 3, 2, 3);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddBlankEnhancedConditionDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Blank Condition";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
