// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ImportBusinessRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.BusinessRuleBase;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ImportBusinessRuleDialog : Form
  {
    protected Sessions.Session session;
    private const string REQUIRED_DEPENDENCY_NODE_CONDITIONS = "Conditions";
    private const string OPTIONAL_DEPENDENCY_NODE_MILESTONERULES = "MilestoneRequirements";
    private string importUrl;
    private BizRuleType bizRuleType;
    private int step;
    private XDocument ruleDoc;
    private string currentDependency = string.Empty;
    private List<string> advancedConditionXRefIDs = new List<string>();
    private IContainer components;
    private PanelEx panelExTop;
    private PanelEx panelExBottom;
    private Button btnValidateImport;
    private Button btnCancel;
    private Label lblRuleName;
    private Panel panelHeader;
    private Label lblStep;
    private Label lblStepInstruction;
    private GridView gridViewDependencies;
    private ImageList imageListValidationStatus;

    private string changeStep => string.Format("Step {0} of 2:", (object) ++this.step);

    public ImportBusinessRuleDialog(
      Sessions.Session session,
      string importUrl,
      string filePath,
      BizRuleType bizRuleType)
    {
      try
      {
        this.InitializeComponent();
        this.session = session;
        this.importUrl = importUrl;
        this.bizRuleType = bizRuleType;
        this.ruleDoc = XDocument.Load(filePath);
        this.populateGrid();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void populateGrid()
    {
      this.lblStep.Text = this.changeStep;
      this.lblStepInstruction.Text = "View the rule and dependencies then click 'Validate Dependencies' button to verify if dependencies exists on the system being imported to.";
      this.lblRuleName.Text = string.Format(this.lblRuleName.Text, (object) this.ruleDoc.Root.Attribute((XName) "Name").Value);
      this.populateDependencies(this.ruleDoc, "Conditions", ImportBusinessRuleDialog.dependencyType.Required);
      switch (this.bizRuleType)
      {
        case BizRuleType.MilestoneRules:
          this.populateDependencies(this.ruleDoc, "MilestoneRequirements", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.LoanAccess:
          this.populateDependencies(this.ruleDoc, "PersonaAccesstoLoans", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.FieldAccess:
          this.populateDependencies(this.ruleDoc, "FieldAcccessRights", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.FieldRules:
          this.populateDependencies(this.ruleDoc, "FieldRules", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.InputForms:
          this.populateDependencies(this.ruleDoc, "InputFormList", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.Triggers:
          this.populateDependencies(this.ruleDoc, "FieldEvents", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.PrintForms:
          this.populateDependencies(this.ruleDoc, "PrintSuppresessionBusinessRules", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.PrintSelection:
          this.populateDependencies(this.ruleDoc, "PrintAutoSelectedForms", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.AutomatedConditions:
          this.populateDependencies(this.ruleDoc, "AutomatedConditions", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
        case BizRuleType.LoanActionAccess:
          this.populateDependencies(this.ruleDoc, "PersonaAccessToLoanActions", ImportBusinessRuleDialog.dependencyType.Optional);
          break;
      }
    }

    private void populateDependencies(
      XDocument ruleDoc,
      string dependencyParent,
      ImportBusinessRuleDialog.dependencyType dependencyType)
    {
      IEnumerable<XElement> xelements;
      switch (dependencyParent)
      {
        case "Conditions":
          xelements = this.sortConditionsDependencies();
          break;
        case "MilestoneRequirements":
          xelements = this.sortMilestoneRulesDependencies();
          break;
        default:
          xelements = ruleDoc.Descendants((XName) dependencyParent).Descendants<XElement>((XName) "XRef");
          break;
      }
      foreach (XElement element in xelements)
        this.addGridViewItem(element, dependencyType, this.isAdvancedCondition(element));
      this.currentDependency = dependencyParent;
    }

    private IEnumerable<XElement> sortConditionsDependencies()
    {
      List<XElement> xelementList = new List<XElement>();
      switch (this.ruleDoc.Descendants((XName) "Conditions").Descendants<XElement>((XName) "Condition").Attributes((XName) "conditionType").FirstOrDefault<XAttribute>().Value)
      {
        case "Current Role":
          IEnumerable<XElement> source1 = this.ruleDoc.Descendants((XName) "Conditions").Descendants<XElement>((XName) "Condition").Where<XElement>((Func<XElement, bool>) (rule => rule.Attribute((XName) "conditionType").Value == "Current Role"));
          if (source1.Count<XElement>() > 0)
          {
            xelementList.Add(source1.Descendants<XElement>((XName) "AffectedRole").Descendants<XElement>((XName) "XRef").ElementAt<XElement>(0));
            xelementList.Add(source1.Descendants<XElement>((XName) "AffectedMilestone").Descendants<XElement>((XName) "XRef").ElementAt<XElement>(0));
            break;
          }
          break;
        case "Advanced Conditions":
          IEnumerable<XElement> source2 = this.ruleDoc.Descendants((XName) "Conditions").Descendants<XElement>((XName) "Condition").Where<XElement>((Func<XElement, bool>) (rule => rule.Attribute((XName) "conditionType").Value == "Advanced Conditions"));
          if (source2.Count<XElement>() > 0)
          {
            using (IEnumerator<XElement> enumerator = source2.Descendants<XElement>((XName) "AdvancedCodeDependencies").Descendants<XElement>((XName) "AffectedField").Descendants<XElement>((XName) "XRef").GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                XElement current = enumerator.Current;
                xelementList.Add(current);
                this.advancedConditionXRefIDs.Add(current.Attribute((XName) "RefID").Value);
              }
              break;
            }
          }
          else
            break;
      }
      return (IEnumerable<XElement>) xelementList;
    }

    private IEnumerable<XElement> sortMilestoneRulesDependencies()
    {
      List<XElement> xelementList = new List<XElement>();
      foreach (XElement element in this.ruleDoc.Descendants((XName) "MilestoneRequirements").Descendants<XElement>((XName) "RequiredDocuments").Elements<XElement>())
      {
        xelementList.Add(element.Descendants((XName) "AffectedDocument").Descendants<XElement>((XName) "XRef").ElementAt<XElement>(0));
        xelementList.Add(element.Descendants((XName) "AffectedMilestone").Descendants<XElement>((XName) "XRef").ElementAt<XElement>(0));
      }
      foreach (XElement element in this.ruleDoc.Descendants((XName) "MilestoneRequirements").Descendants<XElement>((XName) "RequiredTasks").Elements<XElement>())
      {
        XElement xelement = element.Descendants((XName) "AffectedTask").Descendants<XElement>((XName) "XRef").ElementAt<XElement>(0);
        if (xelement.Attribute((XName) "EntityType").Value.ToLower() != "workflowtasks" || Session.StartupInfo.EnableWorkFlowTasks)
        {
          xelementList.Add(xelement);
          xelementList.Add(element.Descendants((XName) "AffectedMilestone").Descendants<XElement>((XName) "XRef").ElementAt<XElement>(0));
        }
      }
      foreach (XElement element in this.ruleDoc.Descendants((XName) "MilestoneRequirements").Descendants<XElement>((XName) "RequiredFields").Elements<XElement>())
      {
        xelementList.Add(element.Descendants((XName) "AffectedField").Descendants<XElement>((XName) "XRef").ElementAt<XElement>(0));
        xelementList.Add(element.Descendants((XName) "AffectedMilestone").Descendants<XElement>((XName) "XRef").ElementAt<XElement>(0));
      }
      foreach (XElement descendant in this.ruleDoc.Descendants((XName) "MilestoneRequirements").Descendants<XElement>((XName) "AdvancedConditions").Descendants<XElement>((XName) "XRef"))
      {
        xelementList.Add(descendant);
        this.advancedConditionXRefIDs.Add(descendant.Attribute((XName) "RefID").Value);
      }
      return (IEnumerable<XElement>) xelementList;
    }

    private bool isAdvancedCondition(XElement element)
    {
      return this.advancedConditionXRefIDs.Contains(element.Attribute((XName) "RefID").Value);
    }

    private void addGridViewItem(
      XElement element,
      ImportBusinessRuleDialog.dependencyType dependencyType,
      bool forAdvancedCondition = false)
    {
      GVItem gvItem = new GVItem((object) (this.gridViewDependencies.Items.Count + 1));
      string str = forAdvancedCondition ? element.Attribute((XName) "EntityType").Value + " for Advanced Condition" : element.Attribute((XName) "EntityType").Value;
      if (str.ToLower() == "workflowtasks")
        gvItem.SubItems.Add((object) "Workflow Task");
      else
        gvItem.SubItems.Add((object) str);
      gvItem.SubItems.Add((object) element.Attribute((XName) "EntityID").Value);
      gvItem.SubItems.Add((object) element.Attribute((XName) "EntityUID").Value);
      gvItem.SubItems.Add((object) dependencyType.ToString());
      if (Session.StartupInfo.EnableWorkFlowTasks && str.ToLower() == "workflowtasks")
        gvItem.SubItems[3].ImageIndex = 3;
      gvItem.Tag = (object) element;
      this.gridViewDependencies.Items.Add(gvItem);
    }

    private void btnValidateImport_Click(object sender, EventArgs e)
    {
      try
      {
        if (this.step > 2)
          return;
        switch (this.step)
        {
          case 1:
            if (!this.validateDependencies(this.getExternalEntities()))
              break;
            this.lblStep.Text = this.changeStep;
            this.lblStepInstruction.Text = "Review the results from dependency validation and click 'Import rule'.";
            this.btnValidateImport.Text = "Import Rule";
            this.btnValidateImport.Width = 75;
            this.btnValidateImport.Location = new Point(906, this.btnValidateImport.Location.Y);
            this.btnCancel.Location = new Point(825, this.btnCancel.Location.Y);
            break;
          case 2:
            this.importBusinessRules();
            ++this.step;
            this.Close();
            break;
        }
      }
      catch (AggregateException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Flatten().InnerExceptions[0].Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private bool validateDependencies(ExternalEntities externalEntities)
    {
      bool flag = false;
      if (string.Compare(externalEntities.RuleAlreadyExists, "True") == 0)
      {
        if (MessageBox.Show("The Business Rule already exists. Do you want to overwrite it?", "Rule Import", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
          this.updateDependency(externalEntities);
          flag = true;
        }
      }
      else
      {
        this.updateDependency(externalEntities);
        flag = true;
      }
      return flag;
    }

    private void updateDependency(ExternalEntities externalEntities)
    {
      Dictionary<string, string> dictionary1 = externalEntities.externalEntities.ToDictionary<XrefEntity, string, string>((Func<XrefEntity, string>) (item => item.XRefId), (Func<XrefEntity, string>) (item => item.IsResolved));
      Dictionary<string, string> dictionary2 = externalEntities.externalEntities.ToDictionary<XrefEntity, string, string>((Func<XrefEntity, string>) (item => item.XRefId), (Func<XrefEntity, string>) (item => item.IsHardDependency));
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewDependencies.Items)
      {
        string key = ((XElement) gvItem.Tag).Attribute((XName) "RefID").Value;
        if (dictionary1.ContainsKey(key))
        {
          bool isValid = dictionary1[key] == "true" || dictionary1[key] == "True";
          bool isHardDependency = dictionary2[key] == "true" || dictionary2[key] == "True";
          this.updateGridViewWithValidationResult(gvItem, isValid, isHardDependency);
        }
      }
    }

    private void updateGridViewWithValidationResult(
      GVItem item,
      bool isValid,
      bool isHardDependency)
    {
      item.SubItems[5].ImageIndex = isValid ? 0 : (isHardDependency ? 2 : 1);
      if (isValid)
        return;
      item.SubItems[6] = this.getLinkLableSubItem(item);
      if (!isHardDependency)
        return;
      this.btnValidateImport.Enabled = false;
    }

    private GVSubItem getLinkLableSubItem(GVItem item)
    {
      EllieMae.EMLite.UI.LinkLabel linkLabel = new EllieMae.EMLite.UI.LinkLabel();
      linkLabel.Text = "View Info";
      linkLabel.ForeColor = Color.FromArgb(29, 110, 174);
      linkLabel.Tag = (object) item;
      linkLabel.Click += new EventHandler(this.llStatusInfo_Click);
      return new GVSubItem((object) linkLabel);
    }

    private void llStatusInfo_Click(object sender, EventArgs e)
    {
      GVItem tag = (GVItem) ((Control) sender).Tag;
      using (DependencyStatusInfoDialog statusInfoDialog = new DependencyStatusInfoDialog(string.Format("{0}: {1}", (object) tag.SubItems[1], (object) tag.SubItems[2]), this.getDependencyInfo(((XElement) tag.Tag).Attribute((XName) "RefID").Value)))
      {
        int num = (int) statusInfoDialog.ShowDialog();
      }
    }

    private string getDependencyInfo(string refID)
    {
      StringBuilder stringBuilder = new StringBuilder();
      IEnumerable<XElement> source1 = this.ruleDoc.Descendants((XName) "Conditions").Descendants<XElement>((XName) "XRef").Where<XElement>((Func<XElement, bool>) (rule => rule.Attribute((XName) "RefID").Value == refID));
      if (source1.Count<XElement>() == 0)
        source1 = this.ruleDoc.Descendants((XName) this.currentDependency).Descendants<XElement>((XName) "XRef").Where<XElement>((Func<XElement, bool>) (rule => rule.Attribute((XName) "RefID").Value == refID));
      IEnumerable<XElement> source2 = source1.Ancestors<XElement>().Select<XElement, XElement>((Func<XElement, XElement>) (rule => rule)).Reverse<XElement>().Skip<XElement>(2);
      int index1 = source2.Count<XElement>() - 1;
      for (int index2 = 0; index2 < index1; ++index2)
        stringBuilder.Append(new string(Convert.ToString((object) source2.ElementAt<XElement>(index2)).TakeWhile<char>((Func<char, bool>) (c => c != '\n')).ToArray<char>()));
      stringBuilder.Append(string.Format("<DependsOn RefID=\"{0}\"  />{1}", (object) refID, (object) Environment.NewLine));
      stringBuilder.AppendLine(source2.ElementAt<XElement>(index1).ToString());
      XElement xelement = source2.ElementAt<XElement>(index1 - 1).Element((XName) "Value");
      if (xelement != null)
        stringBuilder.AppendLine(xelement.ToString());
      return stringBuilder.ToString();
    }

    private ExternalEntities getExternalEntities()
    {
      string input = BusinessRuleRestApiHelper.ValidateBusinessRule(this.ruleDoc.ToString(), this.importUrl);
      ExternalEntities externalEntities = (ExternalEntities) this.getJavaScriptSerializer().Deserialize(input, typeof (ExternalEntities));
      if (this.bizRuleType == BizRuleType.MilestoneRules)
        this.updateExternalEntityRefs(externalEntities);
      return externalEntities;
    }

    private void updateExternalEntityRefs(ExternalEntities externalEntities)
    {
      IEnumerable<XrefEntity> xrefEntities = externalEntities.externalEntities.Where<XrefEntity>((Func<XrefEntity, bool>) (e => e.EntityType == "WorkflowTasks"));
      if (xrefEntities == null)
        return;
      foreach (XrefEntity xrefEntity in xrefEntities)
      {
        if (this.session.SessionObjects.CachedWorkflowTaskTemplates[(object) xrefEntity.SourceEntityId] != null)
          xrefEntity.IsResolved = "true";
      }
    }

    private JavaScriptSerializer getJavaScriptSerializer()
    {
      return new JavaScriptSerializer()
      {
        MaxJsonLength = 104857600
      };
    }

    private void importBusinessRules()
    {
      BusinessRuleRestApiHelper.ImportBusinessRule(this.ruleDoc.ToString(), this.importUrl);
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
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ImportBusinessRuleDialog));
      this.panelExTop = new PanelEx();
      this.gridViewDependencies = new GridView();
      this.imageListValidationStatus = new ImageList(this.components);
      this.panelHeader = new Panel();
      this.lblStepInstruction = new Label();
      this.lblStep = new Label();
      this.lblRuleName = new Label();
      this.panelExBottom = new PanelEx();
      this.btnValidateImport = new Button();
      this.btnCancel = new Button();
      this.panelExTop.SuspendLayout();
      this.panelHeader.SuspendLayout();
      this.panelExBottom.SuspendLayout();
      this.SuspendLayout();
      this.panelExTop.Controls.Add((Control) this.gridViewDependencies);
      this.panelExTop.Controls.Add((Control) this.panelHeader);
      this.panelExTop.Controls.Add((Control) this.lblRuleName);
      this.panelExTop.Dock = DockStyle.Fill;
      this.panelExTop.Location = new Point(0, 0);
      this.panelExTop.Margin = new Padding(4, 5, 4, 5);
      this.panelExTop.Name = "panelExTop";
      this.panelExTop.Size = new Size(1490, 677);
      this.panelExTop.TabIndex = 0;
      this.gridViewDependencies.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "gvclSerialNumber";
      gvColumn1.Text = "#";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 35;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "gvclDependencyOn";
      gvColumn2.Text = "Dependency On";
      gvColumn2.Width = 200;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "gvclIdentifier";
      gvColumn3.Text = "Identifier";
      gvColumn3.Width = 200;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "gvclName";
      gvColumn4.Text = "Name";
      gvColumn4.Width = 230;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "gvclReqOpt";
      gvColumn5.Text = "Required / Optional";
      gvColumn5.Width = 110;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "gvclValidationStatus";
      gvColumn6.Text = "Validation Status";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "gvclSourceInfo";
      gvColumn7.Text = "Source Info";
      gvColumn7.Width = 90;
      this.gridViewDependencies.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gridViewDependencies.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewDependencies.ImageList = this.imageListValidationStatus;
      this.gridViewDependencies.Location = new Point(18, 183);
      this.gridViewDependencies.Margin = new Padding(4, 5, 4, 5);
      this.gridViewDependencies.Name = "gridViewDependencies";
      this.gridViewDependencies.Size = new Size(1454, 485);
      this.gridViewDependencies.TabIndex = 3;
      this.imageListValidationStatus.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageListValidationStatus.ImageStream");
      this.imageListValidationStatus.TransparentColor = Color.Transparent;
      this.imageListValidationStatus.Images.SetKeyName(0, "check-mark-green.png");
      this.imageListValidationStatus.Images.SetKeyName(1, "status-warning.png");
      this.imageListValidationStatus.Images.SetKeyName(2, "stop.png");
      this.imageListValidationStatus.Images.SetKeyName(3, "tasks.png");
      this.panelHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.panelHeader.BackColor = SystemColors.Window;
      this.panelHeader.Controls.Add((Control) this.lblStepInstruction);
      this.panelHeader.Controls.Add((Control) this.lblStep);
      this.panelHeader.Location = new Point(18, 66);
      this.panelHeader.Margin = new Padding(4, 5, 4, 5);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(1454, 117);
      this.panelHeader.TabIndex = 2;
      this.lblStepInstruction.Location = new Point(22, 40);
      this.lblStepInstruction.Margin = new Padding(4, 0, 4, 0);
      this.lblStepInstruction.Name = "lblStepInstruction";
      this.lblStepInstruction.Size = new Size(542, 63);
      this.lblStepInstruction.TabIndex = 1;
      this.lblStepInstruction.Text = "Instruction";
      this.lblStep.AutoSize = true;
      this.lblStep.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblStep.Location = new Point(18, 14);
      this.lblStep.Margin = new Padding(4, 0, 4, 0);
      this.lblStep.Name = "lblStep";
      this.lblStep.Size = new Size(121, 20);
      this.lblStep.TabIndex = 0;
      this.lblStep.Text = "Step {0} of 2:";
      this.lblRuleName.AutoSize = true;
      this.lblRuleName.Location = new Point(36, 14);
      this.lblRuleName.Margin = new Padding(4, 0, 4, 0);
      this.lblRuleName.Name = "lblRuleName";
      this.lblRuleName.Size = new Size(219, 20);
      this.lblRuleName.TabIndex = 0;
      this.lblRuleName.Text = "Follow the steps to import '{0}'";
      this.panelExBottom.Controls.Add((Control) this.btnValidateImport);
      this.panelExBottom.Controls.Add((Control) this.btnCancel);
      this.panelExBottom.Dock = DockStyle.Bottom;
      this.panelExBottom.Location = new Point(0, 677);
      this.panelExBottom.Margin = new Padding(4, 5, 4, 5);
      this.panelExBottom.Name = "panelExBottom";
      this.panelExBottom.Size = new Size(1490, 80);
      this.panelExBottom.TabIndex = 1;
      this.btnValidateImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnValidateImport.Location = new Point(1224, 18);
      this.btnValidateImport.Margin = new Padding(4, 5, 4, 5);
      this.btnValidateImport.Name = "btnValidateImport";
      this.btnValidateImport.Size = new Size(248, 35);
      this.btnValidateImport.TabIndex = 1;
      this.btnValidateImport.Text = "Validate Dependencies";
      this.btnValidateImport.UseVisualStyleBackColor = true;
      this.btnValidateImport.Click += new EventHandler(this.btnValidateImport_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(1102, 18);
      this.btnCancel.Margin = new Padding(4, 5, 4, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(112, 35);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(9f, 20f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(1490, 757);
      this.Controls.Add((Control) this.panelExTop);
      this.Controls.Add((Control) this.panelExBottom);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(4, 5, 4, 5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportBusinessRuleDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Business Rule Import";
      this.panelExTop.ResumeLayout(false);
      this.panelExTop.PerformLayout();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.panelExBottom.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private enum dependencyType
    {
      Required,
      Optional,
    }
  }
}
