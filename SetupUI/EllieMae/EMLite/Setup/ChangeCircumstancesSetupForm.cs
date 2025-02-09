// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ChangeCircumstancesSetupForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
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
  public class ChangeCircumstancesSetupForm : SettingsUserControl
  {
    private Sessions.Session session;
    private bool isSettingsSync;
    private List<string> lineItems = new List<string>();
    public Dictionary<string, TreeNode> LineNoToTreeNodeMap = new Dictionary<string, TreeNode>();
    public Dictionary<TreeNode, string> TreeNodeMapToLineNo = new Dictionary<TreeNode, string>();
    private List<string> Reasons = new List<string>();
    private int editIndex = -1;
    private bool isNewSetting;
    private IContainer components;
    private GroupContainer gcBaseRate;
    private GridView listViewOptions;
    private StandardIconButton stdIconBtnNew;
    private StandardIconButton stdIconBtnEdit;
    private StandardIconButton stdIconBtnUp;
    private StandardIconButton stdIconBtnDown;
    private StandardIconButton stdIconBtnDelete;
    private GroupContainer groupContainer1;
    private Panel panel2;
    private Panel panel1;
    private TextBox txtComments;
    private Label label5;
    private TextBox txtCode;
    private Label label4;
    private ComboBox cmbReason;
    private Label label3;
    private TextBox txtDescription;
    private Label label1;
    private TreeView treeViewLineNumber;
    private Label label6;
    private StandardIconButton stdIconBtnReset;
    private StandardIconButton stdIconBtnSave;
    private StandardIconButton stdBtnMainSave;
    private StandardIconButton stdBtnCopy;
    private Panel panel3;

    public ChangeCircumstancesSetupForm(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public ChangeCircumstancesSetupForm(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool isSettingsSync)
      : base(setupContainer)
    {
      this.session = session;
      this.isSettingsSync = isSettingsSync;
      this.PopulateReasons();
      this.InitializeComponent();
      this.Reset();
      if (isSettingsSync)
      {
        this.stdIconBtnNew.Enabled = false;
        this.stdIconBtnEdit.Enabled = false;
        this.stdIconBtnDelete.Enabled = false;
        this.stdIconBtnUp.Enabled = false;
        this.stdIconBtnDown.Enabled = false;
        this.listViewOptions.DoubleClick -= new EventHandler(this.stdIconBtnEdit_Click);
      }
      this.SetInputControls(false);
      this.stdIconBtnSave.Enabled = false;
      this.stdIconBtnReset.Enabled = false;
      this.stdBtnMainSave.Enabled = false;
      this.stdBtnCopy.Enabled = false;
      this.cmbReason.Items.AddRange((object[]) this.Reasons.ToArray());
      this.buildTree();
      if (this.listViewOptions.Items.Count > 0)
        this.listViewOptions.Items[0].Selected = true;
      this.panel3.SetAutoScrollMargin(10, 10);
    }

    public void PopulateReasons()
    {
      this.Reasons.Add("");
      this.Reasons.Add("Changed Circumstance - Settlement Charges [LE & CD]");
      this.Reasons.Add("Changed Circumstance - Eligibility [LE & CD]");
      this.Reasons.Add("Revisions requested by the Consumer [LE & CD]");
      this.Reasons.Add("Interest Rate dependent charges (Rate Lock) [LE & CD]");
      this.Reasons.Add("Expiration (Intent to Proceed received after 10 business days) [LE only]");
      this.Reasons.Add("Delayed Settlement on Construction Loans [LE only]");
      this.Reasons.Add("Change in APR [CD only]");
      this.Reasons.Add("Change in Loan Product [CD only]");
      this.Reasons.Add("Prepayment Penalty Added [CD only]");
      this.Reasons.Add("24-hour Advanced Preview [CD only]");
      this.Reasons.Add("Tolerance Cure [CD only]");
      this.Reasons.Add("Clerical Error Correction [CD only]");
      this.Reasons.Add("Other [LE & CD]");
    }

    public void GetAllItemizationLines()
    {
      foreach (GFEItem gfeItem in GFEItemCollection.GFEItems2010)
      {
        string str1 = new string(gfeItem.Description.Where<char>(new Func<char, bool>(char.IsDigit)).ToArray<char>());
        string str2 = "";
        if (str1 == "")
          str2 = gfeItem.Description;
        TreeNode key = new TreeNode(gfeItem.LineNumber.ToString() + gfeItem.ComponentID + " " + str2);
        this.LineNoToTreeNodeMap.Add(gfeItem.LineNumber.ToString() + gfeItem.ComponentID, key);
        this.TreeNodeMapToLineNo.Add(key, gfeItem.LineNumber.ToString() + gfeItem.ComponentID);
      }
      this.AddtoMap("801", new TreeNode("801. Our Origination Charge"));
      this.LineNoToTreeNodeMap = this.LineNoToTreeNodeMap.OrderBy<KeyValuePair<string, TreeNode>, string>((Func<KeyValuePair<string, TreeNode>, string>) (i => i.Key)).ToDictionary<KeyValuePair<string, TreeNode>, string, TreeNode>((Func<KeyValuePair<string, TreeNode>, string>) (keyItem => keyItem.Key), (Func<KeyValuePair<string, TreeNode>, TreeNode>) (valueItem => valueItem.Value));
    }

    private void AddtoMap(string key, TreeNode node)
    {
      this.LineNoToTreeNodeMap.Add(key, node);
      this.TreeNodeMapToLineNo.Add(node, key);
    }

    private void buildTree()
    {
      this.GetAllItemizationLines();
      TreeNode node1 = new TreeNode("700. Total Sales/Broker Commission");
      this.AddtoMap("700", node1);
      TreeNode node2 = new TreeNode("800. Items Payable in Connection with Loan");
      this.AddtoMap("800", node2);
      TreeNode node3 = new TreeNode("900. Items Required by Lender to be Paid in Advance");
      this.AddtoMap("900", node3);
      TreeNode node4 = new TreeNode("1000. Reserve Deposited with Lender");
      this.AddtoMap("1000", node4);
      TreeNode node5 = new TreeNode("1100. Title Charges");
      this.AddtoMap("1100", node5);
      TreeNode node6 = new TreeNode("1200. Gov. Recording Transfer Changes");
      this.AddtoMap("1200", node6);
      TreeNode node7 = new TreeNode("1300. Additional Settlement Charges");
      this.AddtoMap("1300", node7);
      foreach (string key1 in this.LineNoToTreeNodeMap.Keys)
      {
        string key2 = new string(key1.Where<char>(new Func<char, bool>(char.IsDigit)).ToArray<char>());
        string str = new string(key1.Where<char>(new Func<char, bool>(char.IsLetter)).ToArray<char>());
        int int32 = Convert.ToInt32(key2);
        if (str != "" && str != "x")
        {
          if (this.LineNoToTreeNodeMap.ContainsKey(key2 + "x"))
            this.LineNoToTreeNodeMap[key2 + "x"].Nodes.Add(this.LineNoToTreeNodeMap[key1]);
          else
            this.LineNoToTreeNodeMap[key2].Nodes.Add(this.LineNoToTreeNodeMap[key1]);
        }
        else if (int32 > 700 && int32 < 800)
          node1.Nodes.Add(this.LineNoToTreeNodeMap[key1]);
        else if (int32 > 800 && int32 < 900)
          node2.Nodes.Add(this.LineNoToTreeNodeMap[key1]);
        else if (int32 > 900 && int32 < 1000)
          node3.Nodes.Add(this.LineNoToTreeNodeMap[key1]);
        else if (int32 > 1000 && int32 < 1100)
          node4.Nodes.Add(this.LineNoToTreeNodeMap[key1]);
        else if (int32 > 1100 && int32 < 1200)
          node5.Nodes.Add(this.LineNoToTreeNodeMap[key1]);
        else if (int32 > 1200 && int32 < 1300)
          node6.Nodes.Add(this.LineNoToTreeNodeMap[key1]);
        else if (int32 > 1300 && int32 < 1400)
          node7.Nodes.Add(this.LineNoToTreeNodeMap[key1]);
      }
      this.treeViewLineNumber.Nodes.AddRange(new TreeNode[7]
      {
        node1,
        node2,
        node3,
        node4,
        node5,
        node6,
        node7
      });
    }

    private string GetReasonDescription(int index) => index == -1 ? "" : this.Reasons[index];

    private string GetCoCType(int index)
    {
      if (this.cmbReason.SelectedIndex >= 0 && this.cmbReason.SelectedIndex <= 4 || this.cmbReason.SelectedIndex == 13)
        return "Both";
      if (this.cmbReason.SelectedIndex >= 5 && this.cmbReason.SelectedIndex <= 6)
        return "LE";
      return this.cmbReason.SelectedIndex >= 7 && this.cmbReason.SelectedIndex <= 12 ? "CD" : string.Empty;
    }

    public override void Reset()
    {
      List<ChangeCircumstanceSettings> circumstanceSettings = this.session.ConfigurationManager.GetAllChangeCircumstanceSettings();
      this.setDirtyFlag(false);
      this.listViewOptions.Items.Clear();
      this.listViewOptions.BeginUpdate();
      for (int index = 0; index < circumstanceSettings.Count; ++index)
        this.listViewOptions.Items.Add(new GVItem(circumstanceSettings[index].Description)
        {
          SubItems = {
            (object) this.GetReasonDescription(circumstanceSettings[index].Reason),
            (object) circumstanceSettings[index].Comment,
            (object) circumstanceSettings[index].Code
          },
          Tag = (object) circumstanceSettings[index]
        });
      this.listViewOptions.EndUpdate();
      this.listViewOptions_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    public override void Save()
    {
      List<ChangeCircumstanceSettings> changeCoCSettings = new List<ChangeCircumstanceSettings>();
      for (int nItemIndex = 0; nItemIndex < this.listViewOptions.Items.Count; ++nItemIndex)
        changeCoCSettings.Add((ChangeCircumstanceSettings) this.listViewOptions.Items[nItemIndex].Tag);
      ChangeCircumstanceSettings circumstanceSettings1 = new ChangeCircumstanceSettings();
      if (this.txtDescription.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter the Description.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.txtCode.Text == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter the code.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.isNewSetting)
        {
          foreach (ChangeCircumstanceSettings circumstanceSettings2 in changeCoCSettings)
          {
            if (circumstanceSettings2.Code == this.txtCode.Text)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "The list already contains the code '" + this.txtCode.Text.Trim() + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              this.txtCode.Text = "";
              return;
            }
          }
          changeCoCSettings.Add(new ChangeCircumstanceSettings()
          {
            Code = this.txtCode.Text,
            Description = this.txtDescription.Text,
            Comment = this.txtComments.Text,
            Reason = this.cmbReason.SelectedIndex,
            CocType = this.GetCoCType(this.cmbReason.SelectedIndex),
            LineNumbers = this.lineItems,
            optionId = -1
          });
          this.isNewSetting = false;
        }
        else if (this.editIndex != -1 && this.editIndex < this.listViewOptions.Items.Count)
        {
          circumstanceSettings1 = (ChangeCircumstanceSettings) this.listViewOptions.Items[this.editIndex].Tag;
          circumstanceSettings1.LineNumbers = this.lineItems;
        }
        this.session.ConfigurationManager.UpdateChangeCircumstance(changeCoCSettings);
        circumstanceSettings1.IsItemLinesUpdated = false;
        this.setDirtyFlag(false);
        this.Reset();
        this.editIndex = -1;
        this.stdIconBtnSave.Enabled = false;
        this.stdIconBtnReset.Enabled = false;
        this.stdBtnMainSave.Enabled = false;
        this.stdIconBtnNew.Enabled = true;
        this.SetInputControls(false);
      }
    }

    private void stdIconBtnNew_Click(object sender, EventArgs e)
    {
      this.isNewSetting = true;
      this.SetInputControls(true);
      this.lineItems.Clear();
      this.listViewOptions.SelectedItems.Clear();
      this.txtCode.Text = "";
      this.txtComments.Text = "";
      this.txtDescription.Text = "";
      this.cmbReason.SelectedIndex = -1;
      this.stdBtnCopy.Enabled = false;
      foreach (TreeNode key in this.TreeNodeMapToLineNo.Keys)
        key.Checked = false;
      this.stdIconBtnSave.Enabled = true;
      this.stdIconBtnReset.Enabled = true;
      this.stdBtnMainSave.Enabled = true;
      this.stdIconBtnDown.Enabled = false;
      this.stdIconBtnUp.Enabled = false;
      this.stdIconBtnEdit.Enabled = false;
      this.setDirtyFlag(true);
    }

    private void stdIconBtnEdit_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count == 0 || this.listViewOptions.SelectedItems.Count > 1)
        return;
      this.editIndex = this.listViewOptions.SelectedItems[0].Index;
      this.lineItems.Clear();
      this.isNewSetting = false;
      this.SetInputControls(true);
      this.stdIconBtnSave.Enabled = true;
      this.stdIconBtnReset.Enabled = true;
      this.stdBtnMainSave.Enabled = true;
      this.stdIconBtnDown.Enabled = false;
      this.stdIconBtnUp.Enabled = false;
      this.stdBtnCopy.Enabled = false;
      this.stdIconBtnNew.Enabled = false;
    }

    private void stdIconBtnUp_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems[0].Index == 0)
        return;
      this.listViewOptions.BeginUpdate();
      int index = this.listViewOptions.SelectedItems[0].Index;
      GVItem selectedItem = this.listViewOptions.SelectedItems[0];
      this.listViewOptions.Items.RemoveAt(index);
      this.listViewOptions.Items.Insert(index - 1, selectedItem);
      this.listViewOptions.Items[index - 1].Selected = true;
      this.listViewOptions.EndUpdate();
      this.stdBtnMainSave.Enabled = true;
      this.stdIconBtnReset.Enabled = true;
      this.setDirtyFlag(true);
    }

    private void stdIconBtnDown_Click(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems[0].Index == this.listViewOptions.Items.Count - 1)
        return;
      this.listViewOptions.BeginUpdate();
      int index = this.listViewOptions.SelectedItems[0].Index;
      GVItem selectedItem = this.listViewOptions.SelectedItems[0];
      this.listViewOptions.Items.RemoveAt(index);
      this.listViewOptions.Items.Insert(index + 1, selectedItem);
      this.listViewOptions.Items[index + 1].Selected = true;
      this.listViewOptions.EndUpdate();
      this.stdBtnMainSave.Enabled = true;
      this.stdIconBtnReset.Enabled = true;
      this.setDirtyFlag(true);
    }

    private void stdIconBtnDelete_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete selected " + (this.listViewOptions.SelectedItems.Count > 1 ? "options" : "option") + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      List<ChangeCircumstanceSettings> cocSettings = new List<ChangeCircumstanceSettings>();
      int index = this.listViewOptions.SelectedItems[0].Index;
      int num = this.listViewOptions.Items.Count - 1;
      this.listViewOptions.BeginUpdate();
      for (int nItemIndex = num; nItemIndex >= 0; --nItemIndex)
      {
        if (this.listViewOptions.Items[nItemIndex].Selected)
        {
          cocSettings.Add((ChangeCircumstanceSettings) this.listViewOptions.Items[nItemIndex].Tag);
          this.listViewOptions.Items.Remove(this.listViewOptions.Items[nItemIndex]);
        }
      }
      if (this.listViewOptions.Items.Count == 0)
      {
        this.listViewOptions.EndUpdate();
      }
      else
      {
        if (index + 1 >= this.listViewOptions.Items.Count)
          this.listViewOptions.Items[this.listViewOptions.Items.Count - 1].Selected = true;
        else
          this.listViewOptions.Items[index].Selected = true;
        this.listViewOptions.EndUpdate();
        this.session.ConfigurationManager.DeleteChangeCircumstanceSetting(cocSettings);
        this.Save();
      }
    }

    private void listViewOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.isSettingsSync)
      {
        this.stdIconBtnNew.Enabled = false;
        this.stdIconBtnEdit.Enabled = false;
        this.stdIconBtnDelete.Enabled = false;
        this.stdIconBtnUp.Enabled = false;
        this.stdIconBtnDown.Enabled = false;
      }
      else
      {
        this.stdIconBtnDelete.Enabled = this.listViewOptions.SelectedItems.Count > 0;
        this.stdBtnCopy.Enabled = this.stdIconBtnEdit.Enabled = this.listViewOptions.SelectedItems.Count == 1;
        this.stdIconBtnDown.Enabled = this.stdIconBtnUp.Enabled = this.listViewOptions.SelectedItems.Count == 1;
      }
      if (this.listViewOptions.SelectedItems.Count > 0)
      {
        int index = this.listViewOptions.SelectedItems[0].Index;
        if (this.IsDirty && (this.isNewSetting || this.editIndex >= 0))
        {
          if (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          {
            this.stdIconBtnSave.Enabled = false;
            this.stdIconBtnReset.Enabled = false;
            this.stdBtnMainSave.Enabled = false;
            this.SetInputControls(false);
            this.setDirtyFlag(false);
            this.stdIconBtnNew.Enabled = true;
            this.Reset();
            this.listViewOptions.Items[index].Selected = true;
          }
          else
          {
            this.Save();
            this.listViewOptions.Items[index].Selected = true;
          }
        }
        else
        {
          this.stdIconBtnSave.Enabled = false;
          this.stdIconBtnReset.Enabled = false;
          this.stdBtnMainSave.Enabled = false;
          this.SetInputControls(false);
          this.stdIconBtnNew.Enabled = true;
        }
        this.stdBtnCopy.Enabled = true;
        ChangeCircumstanceSettings tag = (ChangeCircumstanceSettings) this.listViewOptions.SelectedItems[0].Tag;
        this.txtCode.Text = tag.Code;
        this.txtDescription.Text = tag.Description;
        this.txtComments.Text = tag.Comment;
        this.cmbReason.SelectedIndex = tag.Reason;
        foreach (TreeNode key in this.TreeNodeMapToLineNo.Keys)
          key.Checked = false;
        foreach (string lineNumber in tag.LineNumbers)
          this.LineNoToTreeNodeMap[lineNumber].Checked = this.LineNoToTreeNodeMap.ContainsKey(lineNumber);
        this.treeViewLineNumber.Refresh();
      }
      else
      {
        if (this.IsDirty && (this.isNewSetting || this.editIndex >= 0))
        {
          if (Utils.Dialog((IWin32Window) this, "Do you want to save changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          {
            this.stdIconBtnSave.Enabled = false;
            this.stdIconBtnReset.Enabled = false;
            this.stdBtnMainSave.Enabled = false;
            this.SetInputControls(false);
            this.setDirtyFlag(false);
            this.stdIconBtnNew.Enabled = true;
            this.Reset();
          }
          else
            this.Save();
        }
        this.txtCode.Text = "";
        this.txtDescription.Text = "";
        this.txtComments.Text = "";
        this.cmbReason.SelectedIndex = -1;
      }
    }

    public string[] SelectedOptionNames
    {
      get
      {
        return this.listViewOptions.SelectedItems.Count == 0 ? (string[]) null : this.listViewOptions.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[3].Text)).ToArray<string>();
      }
    }

    public void SetSelectedOptionNames(List<string> selectedTableNames)
    {
      for (int index = 0; index < selectedTableNames.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.listViewOptions.Items.Count; ++nItemIndex)
        {
          if (this.listViewOptions.Items[nItemIndex].SubItems[2].Text == selectedTableNames[index])
          {
            this.listViewOptions.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
    }

    private void stdIconBtnSave_Click(object sender, EventArgs e) => this.Save();

    private void treeViewLineNumber_AfterCheck(object sender, TreeViewEventArgs e)
    {
      if (e.Node.Checked)
        this.lineItems.Add(this.TreeNodeMapToLineNo[e.Node]);
      if (this.isNewSetting || this.listViewOptions.SelectedItems.Count <= 0)
        return;
      ((ChangeCircumstanceSettings) this.listViewOptions.SelectedItems[0].Tag).IsItemLinesUpdated = true;
    }

    private void txtDescription_TextChanged(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count <= 0 || this.isNewSetting)
        return;
      ((ChangeCircumstanceSettings) this.listViewOptions.SelectedItems[0].Tag).Description = this.txtDescription.Text;
      if (!this.txtDescription.Enabled)
        return;
      this.setDirtyFlag(true);
    }

    private void txtComments_TextChanged(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count <= 0 || this.isNewSetting)
        return;
      ((ChangeCircumstanceSettings) this.listViewOptions.SelectedItems[0].Tag).Comment = this.txtComments.Text;
      if (!this.txtComments.Enabled)
        return;
      this.setDirtyFlag(true);
    }

    private void txtCode_TextChanged(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count <= 0 || this.isNewSetting)
        return;
      ((ChangeCircumstanceSettings) this.listViewOptions.SelectedItems[0].Tag).Code = this.txtCode.Text;
      if (!this.txtCode.Enabled)
        return;
      this.setDirtyFlag(true);
    }

    private void cmbReason_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.listViewOptions.SelectedItems.Count <= 0 || this.isNewSetting)
        return;
      ChangeCircumstanceSettings tag = (ChangeCircumstanceSettings) this.listViewOptions.SelectedItems[0].Tag;
      tag.Reason = this.cmbReason.SelectedIndex;
      tag.CocType = this.GetCoCType(this.cmbReason.SelectedIndex);
      if (!this.cmbReason.Enabled)
        return;
      this.setDirtyFlag(true);
    }

    private void SetInputControls(bool value)
    {
      this.txtDescription.Enabled = value;
      this.txtComments.Enabled = value;
      this.cmbReason.Enabled = value;
      this.treeViewLineNumber.Enabled = value;
      if (!this.isNewSetting)
        this.txtCode.Enabled = false;
      else
        this.txtCode.Enabled = value;
    }

    private void stdIconBtnReset_Click(object sender, EventArgs e)
    {
      this.Reset();
      this.setDirtyFlag(false);
      if (!this.isNewSetting)
        this.listViewOptions.Items[this.editIndex].Selected = true;
      this.stdIconBtnSave.Enabled = false;
      this.stdBtnMainSave.Enabled = false;
      this.stdIconBtnReset.Enabled = false;
      this.stdIconBtnNew.Enabled = true;
      this.SetInputControls(false);
      this.isNewSetting = false;
      this.editIndex = -1;
    }

    private void stdBtnCopy_Click(object sender, EventArgs e)
    {
      this.isNewSetting = true;
      if (this.listViewOptions.SelectedItems.Count > 0)
      {
        ChangeCircumstanceSettings tag = (ChangeCircumstanceSettings) this.listViewOptions.SelectedItems[0].Tag;
        this.txtCode.Text = "Copy of " + tag.Code;
        this.txtDescription.Text = "Copy of " + tag.Description;
        this.txtComments.Text = tag.Comment;
        this.cmbReason.SelectedIndex = tag.Reason;
        foreach (TreeNode key in this.TreeNodeMapToLineNo.Keys)
          key.Checked = false;
        foreach (string lineNumber in tag.LineNumbers)
          this.LineNoToTreeNodeMap[lineNumber].Checked = this.LineNoToTreeNodeMap.ContainsKey(lineNumber);
        this.treeViewLineNumber.Refresh();
      }
      this.stdIconBtnSave.Enabled = true;
      this.stdBtnMainSave.Enabled = true;
      this.stdIconBtnReset.Enabled = true;
      this.SetInputControls(true);
      this.setDirtyFlag(true);
    }

    private void stdBtnMainSave_Click(object sender, EventArgs e) => this.Save();

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
      this.gcBaseRate = new GroupContainer();
      this.listViewOptions = new GridView();
      this.groupContainer1 = new GroupContainer();
      this.panel3 = new Panel();
      this.panel1 = new Panel();
      this.txtComments = new TextBox();
      this.label5 = new Label();
      this.txtCode = new TextBox();
      this.label4 = new Label();
      this.cmbReason = new ComboBox();
      this.label3 = new Label();
      this.txtDescription = new TextBox();
      this.label1 = new Label();
      this.panel2 = new Panel();
      this.label6 = new Label();
      this.treeViewLineNumber = new TreeView();
      this.stdBtnMainSave = new StandardIconButton();
      this.stdBtnCopy = new StandardIconButton();
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnEdit = new StandardIconButton();
      this.stdIconBtnUp = new StandardIconButton();
      this.stdIconBtnDown = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.stdIconBtnReset = new StandardIconButton();
      this.stdIconBtnSave = new StandardIconButton();
      this.gcBaseRate.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      ((ISupportInitialize) this.stdBtnMainSave).BeginInit();
      ((ISupportInitialize) this.stdBtnCopy).BeginInit();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnEdit).BeginInit();
      ((ISupportInitialize) this.stdIconBtnUp).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDown).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      this.SuspendLayout();
      this.gcBaseRate.Controls.Add((Control) this.stdBtnMainSave);
      this.gcBaseRate.Controls.Add((Control) this.stdBtnCopy);
      this.gcBaseRate.Controls.Add((Control) this.listViewOptions);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnNew);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnEdit);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnUp);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnDown);
      this.gcBaseRate.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcBaseRate.Dock = DockStyle.Fill;
      this.gcBaseRate.HeaderForeColor = SystemColors.ControlText;
      this.gcBaseRate.Location = new Point(0, 0);
      this.gcBaseRate.Name = "gcBaseRate";
      this.gcBaseRate.Size = new Size(886, 117);
      this.gcBaseRate.TabIndex = 2;
      this.gcBaseRate.Text = "Changed Circumstance Options";
      this.listViewOptions.AutoHeight = true;
      this.listViewOptions.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Description";
      gvColumn1.Width = 500;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Text = "Reason";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column4";
      gvColumn3.Text = "Comments";
      gvColumn3.Width = 450;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column5";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Code";
      gvColumn4.Width = 3;
      this.listViewOptions.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listViewOptions.Dock = DockStyle.Fill;
      this.listViewOptions.HeaderHeight = 22;
      this.listViewOptions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewOptions.Location = new Point(1, 26);
      this.listViewOptions.Name = "listViewOptions";
      this.listViewOptions.Size = new Size(884, 90);
      this.listViewOptions.TabIndex = 0;
      this.listViewOptions.SelectedIndexChanged += new EventHandler(this.listViewOptions_SelectedIndexChanged);
      this.listViewOptions.DoubleClick += new EventHandler(this.stdIconBtnEdit_Click);
      this.groupContainer1.AutoScroll = true;
      this.groupContainer1.Controls.Add((Control) this.panel3);
      this.groupContainer1.Controls.Add((Control) this.stdIconBtnReset);
      this.groupContainer1.Controls.Add((Control) this.stdIconBtnSave);
      this.groupContainer1.Dock = DockStyle.Bottom;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 117);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(886, 400);
      this.groupContainer1.TabIndex = 80;
      this.groupContainer1.Text = "Detail";
      this.panel3.AutoScroll = true;
      this.panel3.Controls.Add((Control) this.panel1);
      this.panel3.Controls.Add((Control) this.panel2);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(1, 26);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(884, 373);
      this.panel3.TabIndex = 16;
      this.panel1.AutoScroll = true;
      this.panel1.Controls.Add((Control) this.txtComments);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.txtCode);
      this.panel1.Controls.Add((Control) this.label4);
      this.panel1.Controls.Add((Control) this.cmbReason);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.txtDescription);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(467, 373);
      this.panel1.TabIndex = 12;
      this.txtComments.Location = new Point(82, 184);
      this.txtComments.Multiline = true;
      this.txtComments.Name = "txtComments";
      this.txtComments.Size = new Size(367, 82);
      this.txtComments.TabIndex = 32;
      this.txtComments.TextChanged += new EventHandler(this.txtComments_TextChanged);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(24, 187);
      this.label5.Name = "label5";
      this.label5.Size = new Size(56, 13);
      this.label5.TabIndex = 31;
      this.label5.Text = "Comments";
      this.txtCode.Location = new Point(82, 149);
      this.txtCode.Name = "txtCode";
      this.txtCode.Size = new Size(367, 20);
      this.txtCode.TabIndex = 30;
      this.txtCode.TextChanged += new EventHandler(this.txtCode_TextChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(24, 156);
      this.label4.Name = "label4";
      this.label4.Size = new Size(32, 13);
      this.label4.TabIndex = 29;
      this.label4.Text = "Code";
      this.cmbReason.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbReason.FormattingEnabled = true;
      this.cmbReason.Location = new Point(82, 118);
      this.cmbReason.Name = "cmbReason";
      this.cmbReason.Size = new Size(367, 21);
      this.cmbReason.TabIndex = 28;
      this.cmbReason.SelectedIndexChanged += new EventHandler(this.cmbReason_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(24, 121);
      this.label3.Name = "label3";
      this.label3.Size = new Size(44, 13);
      this.label3.TabIndex = 27;
      this.label3.Text = "Reason";
      this.txtDescription.Location = new Point(82, 14);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(367, 95);
      this.txtDescription.TabIndex = 23;
      this.txtDescription.TextChanged += new EventHandler(this.txtDescription_TextChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(24, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(60, 13);
      this.label1.TabIndex = 22;
      this.label1.Text = "Description";
      this.panel2.BorderStyle = BorderStyle.FixedSingle;
      this.panel2.Controls.Add((Control) this.label6);
      this.panel2.Controls.Add((Control) this.treeViewLineNumber);
      this.panel2.Dock = DockStyle.Right;
      this.panel2.Location = new Point(467, 0);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(417, 373);
      this.panel2.TabIndex = 13;
      this.panel2.Visible = false;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(12, 9);
      this.label6.Name = "label6";
      this.label6.Size = new Size(222, 13);
      this.label6.TabIndex = 14;
      this.label6.Text = "Select Fee Lines that can be modified";
      this.treeViewLineNumber.BackColor = Color.WhiteSmoke;
      this.treeViewLineNumber.BorderStyle = BorderStyle.None;
      this.treeViewLineNumber.CheckBoxes = true;
      this.treeViewLineNumber.Dock = DockStyle.Fill;
      this.treeViewLineNumber.Location = new Point(0, 0);
      this.treeViewLineNumber.Name = "treeViewLineNumber";
      this.treeViewLineNumber.Size = new Size(415, 371);
      this.treeViewLineNumber.TabIndex = 0;
      this.treeViewLineNumber.AfterCheck += new TreeViewEventHandler(this.treeViewLineNumber_AfterCheck);
      this.stdBtnMainSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdBtnMainSave.BackColor = Color.Transparent;
      this.stdBtnMainSave.Location = new Point(799, 4);
      this.stdBtnMainSave.MouseDownImage = (Image) null;
      this.stdBtnMainSave.Name = "stdBtnMainSave";
      this.stdBtnMainSave.Size = new Size(16, 16);
      this.stdBtnMainSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdBtnMainSave.TabIndex = 82;
      this.stdBtnMainSave.TabStop = false;
      this.stdBtnMainSave.Click += new EventHandler(this.stdBtnMainSave_Click);
      this.stdBtnCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdBtnCopy.BackColor = Color.Transparent;
      this.stdBtnCopy.Location = new Point(781, 5);
      this.stdBtnCopy.MouseDownImage = (Image) null;
      this.stdBtnCopy.Name = "stdBtnCopy";
      this.stdBtnCopy.Size = new Size(16, 16);
      this.stdBtnCopy.StandardButtonType = StandardIconButton.ButtonType.CopyButton;
      this.stdBtnCopy.TabIndex = 81;
      this.stdBtnCopy.TabStop = false;
      this.stdBtnCopy.Click += new EventHandler(this.stdBtnCopy_Click);
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(745, 4);
      this.stdIconBtnNew.MouseDownImage = (Image) null;
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 79;
      this.stdIconBtnNew.TabStop = false;
      this.stdIconBtnNew.Click += new EventHandler(this.stdIconBtnNew_Click);
      this.stdIconBtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnEdit.BackColor = Color.Transparent;
      this.stdIconBtnEdit.Location = new Point(764, 5);
      this.stdIconBtnEdit.MouseDownImage = (Image) null;
      this.stdIconBtnEdit.Name = "stdIconBtnEdit";
      this.stdIconBtnEdit.Size = new Size(16, 16);
      this.stdIconBtnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.stdIconBtnEdit.TabIndex = 77;
      this.stdIconBtnEdit.TabStop = false;
      this.stdIconBtnEdit.Click += new EventHandler(this.stdIconBtnEdit_Click);
      this.stdIconBtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUp.BackColor = Color.Transparent;
      this.stdIconBtnUp.Location = new Point(819, 5);
      this.stdIconBtnUp.MouseDownImage = (Image) null;
      this.stdIconBtnUp.Name = "stdIconBtnUp";
      this.stdIconBtnUp.Size = new Size(16, 16);
      this.stdIconBtnUp.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.stdIconBtnUp.TabIndex = 76;
      this.stdIconBtnUp.TabStop = false;
      this.stdIconBtnUp.Click += new EventHandler(this.stdIconBtnUp_Click);
      this.stdIconBtnDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDown.BackColor = Color.Transparent;
      this.stdIconBtnDown.Location = new Point(841, 5);
      this.stdIconBtnDown.MouseDownImage = (Image) null;
      this.stdIconBtnDown.Name = "stdIconBtnDown";
      this.stdIconBtnDown.Size = new Size(16, 16);
      this.stdIconBtnDown.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.stdIconBtnDown.TabIndex = 75;
      this.stdIconBtnDown.TabStop = false;
      this.stdIconBtnDown.Click += new EventHandler(this.stdIconBtnDown_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(863, 5);
      this.stdIconBtnDelete.MouseDownImage = (Image) null;
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 74;
      this.stdIconBtnDelete.TabStop = false;
      this.stdIconBtnDelete.Click += new EventHandler(this.stdIconBtnDelete_Click);
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(862, 7);
      this.stdIconBtnReset.MouseDownImage = (Image) null;
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(16, 16);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 15;
      this.stdIconBtnReset.TabStop = false;
      this.stdIconBtnReset.Click += new EventHandler(this.stdIconBtnReset_Click);
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(842, 7);
      this.stdIconBtnSave.MouseDownImage = (Image) null;
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(16, 16);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 14;
      this.stdIconBtnSave.TabStop = false;
      this.stdIconBtnSave.Click += new EventHandler(this.stdIconBtnSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcBaseRate);
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (ChangeCircumstancesSetupForm);
      this.Size = new Size(886, 517);
      this.gcBaseRate.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      ((ISupportInitialize) this.stdBtnMainSave).EndInit();
      ((ISupportInitialize) this.stdBtnCopy).EndInit();
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnEdit).EndInit();
      ((ISupportInitialize) this.stdIconBtnUp).EndInit();
      ((ISupportInitialize) this.stdIconBtnDown).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      this.ResumeLayout(false);
    }
  }
}
