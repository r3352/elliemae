// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ServicingLateChargeSetup
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ServicingLateChargeSetup : SettingsUserControl
  {
    private Hashtable stateCharge;
    private Hashtable formNames;
    private Sessions.Session session;
    private bool isSyncMode;
    private IContainer components;
    private Label label4;
    private ComboBox comboBoxDays;
    private Label label1;
    private TextBox txtMinimum;
    private GroupContainer gcPrintForm;
    private GroupContainer gcLateFee;
    private Label label2;
    private StandardIconButton stdIconBtnNew;
    private ToolTip toolTip1;
    private StandardIconButton stdIconBtnDelete;
    private GridView gridViewStatements;
    private GridView gridViewState;
    private BorderPanel borderPanel2;
    private CollapsibleSplitter collapsibleSplitter1;
    private GradientPanel gradientPanel1;
    private GradientPanel gradientPanel2;

    public ServicingLateChargeSetup(SetUpContainer setupContainer)
      : this(setupContainer, Session.DefaultInstance, false)
    {
    }

    public ServicingLateChargeSetup(
      SetUpContainer setupContainer,
      Sessions.Session session,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.session = session;
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtMinimum, TextBoxContentRule.Decimal, "0.00");
      this.comboBoxDays.Items.Clear();
      for (int index = 1; index <= 30; ++index)
        this.comboBoxDays.Items.Add((object) index);
      this.reset();
      this.gridViewStatements.AllowMultiselect = allowMultiSelect;
      this.gridViewState.AllowMultiselect = allowMultiSelect;
      this.isSyncMode = allowMultiSelect;
      this.gridViewStatements_SelectedIndexChanged((object) this, (EventArgs) null);
      this.stdIconBtnNew.Visible = this.stdIconBtnDelete.Visible = this.comboBoxDays.Enabled = !allowMultiSelect;
    }

    private void reset()
    {
      int num1 = Utils.ParseInt((object) this.session.ConfigurationManager.GetCompanySetting("SERVICING", "DaysToPrint"));
      if (num1 < 0)
        num1 = 14;
      if (num1 != 0)
        this.comboBoxDays.Text = num1.ToString();
      this.gridViewStatements.Items.Clear();
      if (this.formNames != null)
        this.formNames.Clear();
      this.formNames = this.session.ConfigurationManager.GetServicingMortgageStatements();
      if (this.formNames != null && this.formNames.Count > 0)
      {
        this.gridViewStatements.BeginUpdate();
        int num2 = 0;
        foreach (DictionaryEntry formName in this.formNames)
        {
          this.gridViewStatements.Items.Add(new GVItem(formName.Key.ToString()));
          this.gridViewStatements.Items[num2++].Tag = (object) formName;
        }
        this.gridViewStatements.EndUpdate();
      }
      this.stateCharge = this.session.ConfigurationManager.GetServicingLateCharge((string) null);
      if (this.gridViewState.Items != null)
        this.gridViewState.Items.Clear();
      this.gridViewState.BeginUpdate();
      foreach (DictionaryEntry stateName in USPS.StateNames)
      {
        if (!(stateName.Value.ToString().ToUpper() == "UNKNOWN"))
        {
          string key = USPS.StateAbbrs[(object) (USPS.StateCode) stateName.Key].ToString();
          GVItem gvItem = new GVItem(stateName.Value.ToString());
          if (this.stateCharge != null && this.stateCharge.ContainsKey((object) key))
          {
            double[] numArray = (double[]) this.stateCharge[(object) key];
            if (numArray[0] != 0.0)
              gvItem.SubItems.Add((object) numArray[0].ToString("N2"));
            else
              gvItem.SubItems.Add((object) "");
            if (numArray[1] != 0.0)
              gvItem.SubItems.Add((object) numArray[1].ToString("N2"));
            else
              gvItem.SubItems.Add((object) "");
          }
          else
          {
            gvItem.SubItems.Add((object) "");
            gvItem.SubItems.Add((object) "");
          }
          gvItem.Tag = (object) key;
          this.gridViewState.Items.Add(gvItem);
        }
      }
      this.gridViewState.Sort(0, SortOrder.Ascending);
      this.gridViewState.EndUpdate();
      this.gridViewStatements_SelectedIndexChanged((object) this, (EventArgs) null);
      this.setDirtyFlag(false);
    }

    public override void Reset() => this.reset();

    public override void Save()
    {
      this.stateCharge = new Hashtable();
      string empty = string.Empty;
      for (int nItemIndex = 0; nItemIndex < this.gridViewState.Items.Count; ++nItemIndex)
      {
        double num1 = Utils.ParseDouble((object) this.gridViewState.Items[nItemIndex].SubItems[1].Text);
        double num2 = Utils.ParseDouble((object) this.gridViewState.Items[nItemIndex].SubItems[2].Text);
        if (num1 != 0.0 || num2 != 0.0)
        {
          if (num1 > num2)
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The minimum charge in " + this.gridViewState.Items[nItemIndex].Text + " cannot be greater than maximum charge.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.gridViewState.Items[nItemIndex].Selected = true;
            return;
          }
          string tag = (string) this.gridViewState.Items[nItemIndex].Tag;
          double[] numArray = new double[2]{ num1, num2 };
          if (this.stateCharge.ContainsKey((object) tag))
            this.stateCharge[(object) tag] = (object) numArray;
          else
            this.stateCharge.Add((object) tag, (object) numArray);
        }
      }
      this.session.ConfigurationManager.SetCompanySetting("SERVICING", "DaysToPrint", this.comboBoxDays.Text);
      this.session.ConfigurationManager.UpdateServicingMortgageStatements(this.formNames);
      this.session.ConfigurationManager.UpdateServicingLateCharge(this.stateCharge);
      this.setDirtyFlag(false);
    }

    private void comboBoxDays_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, TemplateSettingsType.CustomLetter, (FileSystemEntry) null, true))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
        if (this.formNames == null)
          this.formNames = CollectionsUtil.CreateCaseInsensitiveHashtable();
        string str = selectedItem.Name;
        if (str.ToLower().EndsWith(".doc"))
          str = str.Substring(0, str.Length - 4);
        else if (str.ToLower().EndsWith(".docx"))
          str = str.Substring(0, str.Length - 5);
        if (this.formNames.ContainsKey((object) str))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The " + selectedItem.Name + " Custom Form is already in the list.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.gridViewStatements.SelectedItems.Clear();
          this.formNames.Add((object) str, (object) selectedItem.ToString());
          this.gridViewStatements.Items.Add(new GVItem(str)
          {
            Tag = (object) new DictionaryEntry((object) str, (object) selectedItem.ToString()),
            Selected = true
          });
          this.setDirtyFlag(true);
        }
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (this.gridViewStatements.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a mortgage statement document first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.setDirtyFlag(true);
        int num2 = this.gridViewStatements.Items.Count - 1;
        int index = this.gridViewStatements.SelectedItems[0].Index;
        for (int nItemIndex = num2; nItemIndex >= 0; --nItemIndex)
        {
          if (this.gridViewStatements.Items[nItemIndex].Selected)
          {
            if (this.formNames.ContainsKey((object) this.gridViewStatements.Items[nItemIndex].Text))
              this.formNames.Remove((object) this.gridViewStatements.Items[nItemIndex].Text);
            this.gridViewStatements.Items.RemoveAt(nItemIndex);
          }
        }
        if (this.gridViewStatements.Items.Count == 0)
          return;
        if (index + 1 > this.gridViewStatements.Items.Count)
          this.gridViewStatements.Items[this.gridViewStatements.Items.Count - 1].Selected = true;
        else
          this.gridViewStatements.Items[index].Selected = true;
      }
    }

    private void gridViewStatements_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.stdIconBtnDelete.Enabled = this.gridViewStatements.SelectedItems.Count > 0;
    }

    private void gridViewState_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      if (this.isSyncMode)
      {
        e.Handled = true;
      }
      else
      {
        this.setDirtyFlag(true);
        Decimal num = Utils.ParseDecimal((object) e.EditorControl.Text);
        if (num == 0M)
          e.EditorControl.Text = "";
        else
          e.EditorControl.Text = num.ToString("N2");
      }
    }

    public string[] SelectedMortgageStatementPrintForm
    {
      get
      {
        return this.gridViewStatements.SelectedItems.Count == 0 ? (string[]) null : this.gridViewStatements.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => item.Text)).ToArray<string>();
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.gridViewStatements.Items.Count; ++nItemIndex)
          {
            if (this.gridViewStatements.Items[nItemIndex].Text == value[index])
            {
              this.gridViewStatements.Items[nItemIndex].Selected = true;
              break;
            }
          }
        }
      }
    }

    public string[] SelectedCustomPrintForm
    {
      get
      {
        return this.gridViewStatements.SelectedItems.Count == 0 ? (string[]) null : this.gridViewStatements.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => Convert.ToString(((DictionaryEntry) item.Tag).Value))).ToArray<string>();
      }
    }

    public string[] SelectedStateLateFeeCharges
    {
      get
      {
        return this.gridViewState.SelectedItems.Count == 0 ? (string[]) null : this.gridViewState.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (item => (string) item.Tag)).ToArray<string>();
      }
      set
      {
        for (int index = 0; index < value.Length; ++index)
        {
          for (int nItemIndex = 0; nItemIndex < this.gridViewState.Items.Count; ++nItemIndex)
          {
            if ((string) this.gridViewState.Items[nItemIndex].Tag == value[index])
            {
              this.gridViewState.Items[nItemIndex].Selected = true;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.label4 = new Label();
      this.comboBoxDays = new ComboBox();
      this.label1 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.stdIconBtnNew = new StandardIconButton();
      this.stdIconBtnDelete = new StandardIconButton();
      this.gcLateFee = new GroupContainer();
      this.gridViewState = new GridView();
      this.gradientPanel1 = new GradientPanel();
      this.label2 = new Label();
      this.txtMinimum = new TextBox();
      this.gcPrintForm = new GroupContainer();
      this.gridViewStatements = new GridView();
      this.borderPanel2 = new BorderPanel();
      this.gradientPanel2 = new GradientPanel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      ((ISupportInitialize) this.stdIconBtnNew).BeginInit();
      ((ISupportInitialize) this.stdIconBtnDelete).BeginInit();
      this.gcLateFee.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.gcPrintForm.SuspendLayout();
      this.borderPanel2.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.SuspendLayout();
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(7, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(163, 13);
      this.label4.TabIndex = 67;
      this.label4.Text = "Set the printing/mailing due date ";
      this.comboBoxDays.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxDays.FormattingEnabled = true;
      this.comboBoxDays.Location = new Point(175, 6);
      this.comboBoxDays.Name = "comboBoxDays";
      this.comboBoxDays.Size = new Size(80, 21);
      this.comboBoxDays.TabIndex = 68;
      this.comboBoxDays.SelectedIndexChanged += new EventHandler(this.comboBoxDays_SelectedIndexChanged);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(261, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(316, 13);
      this.label1.TabIndex = 69;
      this.label1.Text = "days prior to the Payment Due Date for the mortgage statement.   ";
      this.stdIconBtnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnNew.BackColor = Color.Transparent;
      this.stdIconBtnNew.Location = new Point(804, 5);
      this.stdIconBtnNew.Name = "stdIconBtnNew";
      this.stdIconBtnNew.Size = new Size(16, 16);
      this.stdIconBtnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.stdIconBtnNew.TabIndex = 84;
      this.stdIconBtnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnNew, "New");
      this.stdIconBtnNew.Click += new EventHandler(this.btnAdd_Click);
      this.stdIconBtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnDelete.BackColor = Color.Transparent;
      this.stdIconBtnDelete.Location = new Point(826, 5);
      this.stdIconBtnDelete.Name = "stdIconBtnDelete";
      this.stdIconBtnDelete.Size = new Size(16, 16);
      this.stdIconBtnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.stdIconBtnDelete.TabIndex = 83;
      this.stdIconBtnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnDelete, "Delete");
      this.stdIconBtnDelete.Click += new EventHandler(this.btnRemove_Click);
      this.gcLateFee.Borders = AnchorStyles.Top;
      this.gcLateFee.Controls.Add((Control) this.gridViewState);
      this.gcLateFee.Controls.Add((Control) this.gradientPanel1);
      this.gcLateFee.Controls.Add((Control) this.txtMinimum);
      this.gcLateFee.Dock = DockStyle.Bottom;
      this.gcLateFee.Location = new Point(1, 478);
      this.gcLateFee.Name = "gcLateFee";
      this.gcLateFee.Size = new Size(849, 187);
      this.gcLateFee.TabIndex = 90;
      this.gcLateFee.Text = "Late Fee";
      this.gridViewState.AllowMultiselect = false;
      this.gridViewState.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "State";
      gvColumn1.Width = 250;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.Numeric;
      gvColumn2.Text = "Minimum";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 100;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.TextBox;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Maximum";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 100;
      this.gridViewState.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewState.Dock = DockStyle.Fill;
      this.gridViewState.Location = new Point(0, 57);
      this.gridViewState.Name = "gridViewState";
      this.gridViewState.Size = new Size(849, 130);
      this.gridViewState.TabIndex = 83;
      this.gridViewState.EditorClosing += new GVSubItemEditingEventHandler(this.gridViewState_EditorClosing);
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.label2);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 26);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(849, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel1.TabIndex = 85;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(7, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(358, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Click the cells to enter state regulations for minimum and maximum late fees.";
      this.txtMinimum.Location = new Point(80, 3);
      this.txtMinimum.MaxLength = 7;
      this.txtMinimum.Name = "txtMinimum";
      this.txtMinimum.Size = new Size(100, 20);
      this.txtMinimum.TabIndex = 81;
      this.txtMinimum.TextAlign = HorizontalAlignment.Right;
      this.txtMinimum.Visible = false;
      this.gcPrintForm.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gcPrintForm.Controls.Add((Control) this.gridViewStatements);
      this.gcPrintForm.Controls.Add((Control) this.stdIconBtnNew);
      this.gcPrintForm.Controls.Add((Control) this.stdIconBtnDelete);
      this.gcPrintForm.Dock = DockStyle.Fill;
      this.gcPrintForm.Location = new Point(1, 31);
      this.gcPrintForm.Name = "gcPrintForm";
      this.gcPrintForm.Size = new Size(849, 440);
      this.gcPrintForm.TabIndex = 89;
      this.gcPrintForm.Text = "Mortgage Statement Print Form";
      this.gridViewStatements.BorderStyle = BorderStyle.None;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column1";
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Mortgage Statement Print Form";
      gvColumn4.Width = 849;
      this.gridViewStatements.Columns.AddRange(new GVColumn[1]
      {
        gvColumn4
      });
      this.gridViewStatements.Dock = DockStyle.Fill;
      this.gridViewStatements.Location = new Point(0, 26);
      this.gridViewStatements.Name = "gridViewStatements";
      this.gridViewStatements.Size = new Size(849, 413);
      this.gridViewStatements.TabIndex = 86;
      this.gridViewStatements.SelectedIndexChanged += new EventHandler(this.gridViewStatements_SelectedIndexChanged);
      this.borderPanel2.Controls.Add((Control) this.gcPrintForm);
      this.borderPanel2.Controls.Add((Control) this.gradientPanel2);
      this.borderPanel2.Controls.Add((Control) this.collapsibleSplitter1);
      this.borderPanel2.Controls.Add((Control) this.gcLateFee);
      this.borderPanel2.Dock = DockStyle.Fill;
      this.borderPanel2.Location = new Point(0, 0);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(851, 666);
      this.borderPanel2.TabIndex = 91;
      this.gradientPanel2.Borders = AnchorStyles.None;
      this.gradientPanel2.Controls.Add((Control) this.label1);
      this.gradientPanel2.Controls.Add((Control) this.label4);
      this.gradientPanel2.Controls.Add((Control) this.comboBoxDays);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(1, 1);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(849, 30);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel2.TabIndex = 91;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.gcLateFee;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(1, 471);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 90;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add((Control) this.borderPanel2);
      this.Name = nameof (ServicingLateChargeSetup);
      this.Size = new Size(851, 666);
      ((ISupportInitialize) this.stdIconBtnNew).EndInit();
      ((ISupportInitialize) this.stdIconBtnDelete).EndInit();
      this.gcLateFee.ResumeLayout(false);
      this.gcLateFee.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.gcPrintForm.ResumeLayout(false);
      this.borderPanel2.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
