// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TableGroupDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TableGroupDialog : Form, IHelp
  {
    private const string className = "TableGroupDialog";
    private static string sw = Tracing.SwInputEngine;
    private Label label1;
    private Button cancelBtn;
    private Button okBtn;
    private Label label2;
    private Label label3;
    private Label label4;
    private TextBox tableNameTxt;
    private TextBox offsetTxt;
    private TextBox nearestTxt;
    private RadioButton roundDownBtn;
    private RadioButton roundUpBtn;
    private System.ComponentModel.Container components;
    private ComboBox calcOnCombo;
    private TablePanel.TableID tableID;
    private string oldTableName;
    private EMHelpLink emHelpLink1;
    private GridView gridViewTable;
    private Label label5;
    private ComboBox cboPurpose;
    private Label labelFeeType;
    private ComboBox cboFeeType;
    private TableFeeListBase tablePur;
    private TableFeeListBase tableRefi;
    private Panel panelTop;
    private Panel panelType;
    private Panel panelBottom;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private StandardIconButton btnIconDelete;
    private StandardIconButton btnIconEdit;
    private StandardIconButton btnIconAdd;
    private TableFeeListBase.FeeTable currentFee;
    private Sessions.Session session;
    private string[] rateListArray;
    public bool UseThis;
    public string TableName;
    public string CalcBasedOn;
    public string Rounding;
    public string Nearest;
    public string Offset;
    public string RateList;
    public string FeeType;

    public TableGroupDialog(
      TablePanel.TableID tableID,
      TableFeeListBase.FeeTable currentFee,
      bool isForPurchase,
      TableFeeListBase tablePur,
      TableFeeListBase tableRefi,
      Sessions.Session session)
    {
      this.session = session;
      this.tableID = tableID;
      this.tablePur = tablePur;
      this.tableRefi = tableRefi;
      this.currentFee = currentFee;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      if (this.tableID == TablePanel.TableID.Escrow)
      {
        this.Text = "Escrow Fee Details";
        this.emHelpLink1.HelpTag = "Setup\\Escrow";
        this.labelFeeType.Visible = false;
        this.panelType.Visible = false;
        this.Height -= this.panelType.Height;
      }
      else if (this.tableID == TablePanel.TableID.Title)
      {
        this.Text = "Title Fee Details";
        this.emHelpLink1.HelpTag = "Setup\\Title";
      }
      this.UseThis = currentFee != null && currentFee.UseThis;
      this.tableNameTxt.Text = currentFee != null ? currentFee.TableName : "";
      this.oldTableName = currentFee != null ? currentFee.TableName : "";
      this.cboPurpose.Text = isForPurchase ? "Purchase" : "Refi";
      this.cboFeeType.Text = this.tableID != TablePanel.TableID.Title || currentFee != null && !(currentFee.FeeType == string.Empty) ? (this.tableID == TablePanel.TableID.Escrow ? "" : currentFee.FeeType) : "2009";
      if (currentFee != null)
      {
        for (int index = 0; index < this.calcOnCombo.Items.Count; ++index)
        {
          if (this.calcOnCombo.Items[index].ToString() == currentFee.CalcBasedOn)
          {
            this.calcOnCombo.SelectedIndex = index;
            break;
          }
        }
      }
      else
        this.calcOnCombo.Text = "Sales Price";
      if (currentFee != null && currentFee.Rounding != "Up")
      {
        this.roundUpBtn.Checked = false;
        this.roundDownBtn.Checked = true;
      }
      else
      {
        this.roundUpBtn.Checked = true;
        this.roundDownBtn.Checked = false;
      }
      this.nearestTxt.Text = currentFee != null ? currentFee.Nearest : "";
      this.offsetTxt.Text = currentFee != null ? currentFee.Offset : "";
      this.populateRateList(currentFee != null ? currentFee.RateList : "");
      this.gridViewTable.Sort(0, SortOrder.Ascending);
      this.gridViewTable_SelectedIndexChanged((object) null, (EventArgs) null);
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
      this.tableNameTxt = new TextBox();
      this.label1 = new Label();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.label4 = new Label();
      this.label3 = new Label();
      this.offsetTxt = new TextBox();
      this.nearestTxt = new TextBox();
      this.roundDownBtn = new RadioButton();
      this.roundUpBtn = new RadioButton();
      this.label2 = new Label();
      this.calcOnCombo = new ComboBox();
      this.emHelpLink1 = new EMHelpLink();
      this.gridViewTable = new GridView();
      this.label5 = new Label();
      this.cboPurpose = new ComboBox();
      this.labelFeeType = new Label();
      this.cboFeeType = new ComboBox();
      this.panelTop = new Panel();
      this.panelType = new Panel();
      this.panelBottom = new Panel();
      this.groupContainer2 = new GroupContainer();
      this.groupContainer1 = new GroupContainer();
      this.btnIconDelete = new StandardIconButton();
      this.btnIconEdit = new StandardIconButton();
      this.btnIconAdd = new StandardIconButton();
      this.panelTop.SuspendLayout();
      this.panelType.SuspendLayout();
      this.panelBottom.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.btnIconDelete).BeginInit();
      ((ISupportInitialize) this.btnIconEdit).BeginInit();
      ((ISupportInitialize) this.btnIconAdd).BeginInit();
      this.SuspendLayout();
      this.tableNameTxt.Location = new Point(84, 6);
      this.tableNameTxt.Name = "tableNameTxt";
      this.tableNameTxt.Size = new Size(313, 20);
      this.tableNameTxt.TabIndex = 1;
      this.tableNameTxt.Leave += new EventHandler(this.leave);
      this.tableNameTxt.KeyPress += new KeyPressEventHandler(this.keypress2);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(65, 13);
      this.label1.TabIndex = 40;
      this.label1.Text = "Table Name";
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(321, 334);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 15;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Location = new Point(237, 334);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 14;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(4, 108);
      this.label4.Name = "label4";
      this.label4.Size = new Size(69, 13);
      this.label4.TabIndex = 47;
      this.label4.Text = "With Offset $";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(4, 83);
      this.label3.Name = "label3";
      this.label3.Size = new Size(69, 13);
      this.label3.TabIndex = 46;
      this.label3.Text = "To Nearest $";
      this.offsetTxt.Location = new Point(99, 105);
      this.offsetTxt.Name = "offsetTxt";
      this.offsetTxt.Size = new Size(124, 20);
      this.offsetTxt.TabIndex = 13;
      this.offsetTxt.TextAlign = HorizontalAlignment.Right;
      this.offsetTxt.Leave += new EventHandler(this.leave);
      this.offsetTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.offsetTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.nearestTxt.Location = new Point(99, 80);
      this.nearestTxt.Name = "nearestTxt";
      this.nearestTxt.Size = new Size(124, 20);
      this.nearestTxt.TabIndex = 12;
      this.nearestTxt.TextAlign = HorizontalAlignment.Right;
      this.nearestTxt.Leave += new EventHandler(this.leave);
      this.nearestTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.nearestTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.roundDownBtn.Location = new Point(199, 60);
      this.roundDownBtn.Name = "roundDownBtn";
      this.roundDownBtn.Size = new Size(96, 16);
      this.roundDownBtn.TabIndex = 11;
      this.roundDownBtn.Text = "Round Down";
      this.roundUpBtn.Checked = true;
      this.roundUpBtn.Location = new Point(99, 60);
      this.roundUpBtn.Name = "roundUpBtn";
      this.roundUpBtn.Size = new Size(84, 16);
      this.roundUpBtn.TabIndex = 10;
      this.roundUpBtn.TabStop = true;
      this.roundUpBtn.Text = "Round Up";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(4, 36);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 13);
      this.label2.TabIndex = 41;
      this.label2.Text = "Based On";
      this.calcOnCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.calcOnCombo.Items.AddRange(new object[4]
      {
        (object) "Sales Price",
        (object) "Loan Amount",
        (object) "Appraisal Value",
        (object) "Base Loan Amount"
      });
      this.calcOnCombo.Location = new Point(100, 33);
      this.calcOnCombo.Name = "calcOnCombo";
      this.calcOnCombo.Size = new Size(124, 21);
      this.calcOnCombo.TabIndex = 9;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Location = new Point(10, 336);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 45;
      this.gridViewTable.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Up to";
      gvColumn1.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.Numeric;
      gvColumn2.Text = "Base";
      gvColumn2.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Factor";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 184;
      this.gridViewTable.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewTable.Dock = DockStyle.Fill;
      this.gridViewTable.Location = new Point(1, 26);
      this.gridViewTable.Name = "gridViewTable";
      this.gridViewTable.Size = new Size(384, 157);
      this.gridViewTable.SortOption = GVSortOption.None;
      this.gridViewTable.TabIndex = 7;
      this.gridViewTable.SelectedIndexChanged += new EventHandler(this.gridViewTable_SelectedIndexChanged);
      this.gridViewTable.DoubleClick += new EventHandler(this.gridViewTable_DoubleClick);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 33);
      this.label5.Name = "label5";
      this.label5.Size = new Size(46, 13);
      this.label5.TabIndex = 47;
      this.label5.Text = "Purpose";
      this.cboPurpose.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPurpose.Items.AddRange(new object[2]
      {
        (object) "Purchase",
        (object) "Refi"
      });
      this.cboPurpose.Location = new Point(84, 30);
      this.cboPurpose.Name = "cboPurpose";
      this.cboPurpose.Size = new Size(313, 21);
      this.cboPurpose.TabIndex = 2;
      this.labelFeeType.AutoSize = true;
      this.labelFeeType.Location = new Point(9, 3);
      this.labelFeeType.Name = "labelFeeType";
      this.labelFeeType.Size = new Size(31, 13);
      this.labelFeeType.TabIndex = 49;
      this.labelFeeType.Text = "Type";
      this.cboFeeType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFeeType.Items.AddRange(new object[3]
      {
        (object) "Owner",
        (object) "Lender",
        (object) "2009"
      });
      this.cboFeeType.Location = new Point(84, 0);
      this.cboFeeType.Name = "cboFeeType";
      this.cboFeeType.Size = new Size(313, 21);
      this.cboFeeType.TabIndex = 3;
      this.panelTop.Controls.Add((Control) this.tableNameTxt);
      this.panelTop.Controls.Add((Control) this.label1);
      this.panelTop.Controls.Add((Control) this.cboPurpose);
      this.panelTop.Controls.Add((Control) this.label5);
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(404, 55);
      this.panelTop.TabIndex = 50;
      this.panelType.Controls.Add((Control) this.cboFeeType);
      this.panelType.Controls.Add((Control) this.labelFeeType);
      this.panelType.Dock = DockStyle.Top;
      this.panelType.Location = new Point(0, 55);
      this.panelType.Name = "panelType";
      this.panelType.Size = new Size(404, 25);
      this.panelType.TabIndex = 51;
      this.panelBottom.Controls.Add((Control) this.groupContainer2);
      this.panelBottom.Controls.Add((Control) this.groupContainer1);
      this.panelBottom.Controls.Add((Control) this.okBtn);
      this.panelBottom.Controls.Add((Control) this.cancelBtn);
      this.panelBottom.Controls.Add((Control) this.emHelpLink1);
      this.panelBottom.Dock = DockStyle.Top;
      this.panelBottom.Location = new Point(0, 80);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(404, 366);
      this.panelBottom.TabIndex = 52;
      this.groupContainer2.Controls.Add((Control) this.label4);
      this.groupContainer2.Controls.Add((Control) this.label2);
      this.groupContainer2.Controls.Add((Control) this.label3);
      this.groupContainer2.Controls.Add((Control) this.calcOnCombo);
      this.groupContainer2.Controls.Add((Control) this.offsetTxt);
      this.groupContainer2.Controls.Add((Control) this.roundUpBtn);
      this.groupContainer2.Controls.Add((Control) this.nearestTxt);
      this.groupContainer2.Controls.Add((Control) this.roundDownBtn);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(10, 188);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(386, 140);
      this.groupContainer2.TabIndex = 47;
      this.groupContainer2.Text = "Calculation";
      this.groupContainer1.Controls.Add((Control) this.btnIconDelete);
      this.groupContainer1.Controls.Add((Control) this.btnIconEdit);
      this.groupContainer1.Controls.Add((Control) this.btnIconAdd);
      this.groupContainer1.Controls.Add((Control) this.gridViewTable);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(10, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(386, 184);
      this.groupContainer1.TabIndex = 46;
      this.groupContainer1.Text = "Fees";
      this.btnIconDelete.BackColor = Color.Transparent;
      this.btnIconDelete.Location = new Point(365, 5);
      this.btnIconDelete.Name = "btnIconDelete";
      this.btnIconDelete.Size = new Size(16, 16);
      this.btnIconDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnIconDelete.TabIndex = 10;
      this.btnIconDelete.TabStop = false;
      this.btnIconDelete.Click += new EventHandler(this.btnIconDelete_Click);
      this.btnIconEdit.BackColor = Color.Transparent;
      this.btnIconEdit.Location = new Point(341, 5);
      this.btnIconEdit.Name = "btnIconEdit";
      this.btnIconEdit.Size = new Size(16, 16);
      this.btnIconEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnIconEdit.TabIndex = 9;
      this.btnIconEdit.TabStop = false;
      this.btnIconEdit.Click += new EventHandler(this.btnIconEdit_Click);
      this.btnIconAdd.BackColor = Color.Transparent;
      this.btnIconAdd.Location = new Point(317, 5);
      this.btnIconAdd.Name = "btnIconAdd";
      this.btnIconAdd.Size = new Size(16, 16);
      this.btnIconAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnIconAdd.TabIndex = 8;
      this.btnIconAdd.TabStop = false;
      this.btnIconAdd.Click += new EventHandler(this.btnIconAdd_Click);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(404, 447);
      this.Controls.Add((Control) this.panelBottom);
      this.Controls.Add((Control) this.panelType);
      this.Controls.Add((Control) this.panelTop);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TableGroupDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Tables:";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.panelTop.ResumeLayout(false);
      this.panelTop.PerformLayout();
      this.panelType.ResumeLayout(false);
      this.panelType.PerformLayout();
      this.panelBottom.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      ((ISupportInitialize) this.btnIconDelete).EndInit();
      ((ISupportInitialize) this.btnIconEdit).EndInit();
      ((ISupportInitialize) this.btnIconAdd).EndInit();
      this.ResumeLayout(false);
    }

    private void populateRateList(string rateList)
    {
      if (rateList == string.Empty)
        return;
      this.rateListArray = rateList.Split('|');
      string empty = string.Empty;
      int length = this.rateListArray.Length;
      for (int index = 0; index < length; ++index)
      {
        string[] strArray = this.rateListArray[index].Split(':');
        if (strArray.Length > 1)
          this.gridViewTable.Items.Add(new GVItem(strArray[0])
          {
            SubItems = {
              (object) strArray[1],
              (object) strArray[2]
            }
          });
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.TableName = this.tableNameTxt.Text.Trim();
      if (this.TableName == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a fee description.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.tableNameTxt.Focus();
      }
      else
      {
        if (this.currentFee == null || this.oldTableName != this.TableName)
        {
          if (!this.IsForPurchase ? this.tableRefi.TableNameExists(this.TableName) : this.tablePur.TableNameExists(this.TableName))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The name that you entered for this table is already in use. Please try a different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.tableNameTxt.Focus();
            return;
          }
        }
        this.FeeType = this.tableID == TablePanel.TableID.Escrow || this.cboFeeType.Text == "2009" ? "" : this.cboFeeType.Text;
        this.CalcBasedOn = this.calcOnCombo.Text;
        this.Rounding = !this.roundUpBtn.Checked ? "Down" : "Up";
        this.Nearest = this.nearestTxt.Text;
        this.Offset = this.offsetTxt.Text;
        string empty = string.Empty;
        this.RateList = string.Empty;
        for (int nItemIndex = 0; nItemIndex < this.gridViewTable.Items.Count; ++nItemIndex)
        {
          string str = this.gridViewTable.Items[nItemIndex].Text + ":" + this.gridViewTable.Items[nItemIndex].SubItems[1].Text + ":" + this.gridViewTable.Items[nItemIndex].SubItems[2].Text;
          this.RateList = !(this.RateList == string.Empty) ? this.RateList + "|" + str : str;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender == null || !(sender is TextBox))
        return;
      TextBox textBox = (TextBox) sender;
      if (!(textBox.Name != "tableNameTxt"))
        return;
      double num = this.DoubleValue(textBox.Text);
      textBox.Text = num != 0.0 ? num.ToString("N2") : "";
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      TextBox textBox = (TextBox) sender;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-') || !(textBox.Name == "offsetTxt"))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void keypress2(object sender, KeyPressEventArgs e)
    {
      if (!e.KeyChar.Equals('|'))
        return;
      e.Handled = true;
    }

    private double DoubleValue(string strValue)
    {
      return strValue == string.Empty || strValue == null ? 0.0 : double.Parse(strValue.Replace(",", string.Empty));
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.tableID == TablePanel.TableID.Escrow ? "Setup\\Escrow" : "Setup\\Title");
    }

    private void gridViewTable_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnIconDelete.Enabled = this.gridViewTable.SelectedItems.Count > 0;
      this.btnIconEdit.Enabled = this.gridViewTable.SelectedItems.Count == 1;
    }

    public bool IsForPurchase => this.cboPurpose.Text == "Purchase";

    private void btnIconAdd_Click(object sender, EventArgs e)
    {
      this.gridViewTable.SelectedItems.Clear();
      using (TableDialog tableDialog = new TableDialog(this.calcOnCombo.Text, this.cboPurpose.Text == "Purchase"))
      {
        if (tableDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gridViewTable.Items.Add(new GVItem(tableDialog.UpToAmount)
        {
          SubItems = {
            (object) tableDialog.Base,
            (object) tableDialog.Factor
          },
          Selected = true
        });
        this.gridViewTable.SortOption = GVSortOption.Auto;
        this.gridViewTable.Sort(0, SortOrder.Ascending);
        this.gridViewTable.SortOption = GVSortOption.None;
      }
    }

    private void btnIconEdit_Click(object sender, EventArgs e)
    {
      if (this.gridViewTable.SelectedItems.Count == 0)
        return;
      using (TableDialog tableDialog = new TableDialog(this.gridViewTable.SelectedItems[0].Text.ToString(), this.gridViewTable.SelectedItems[0].SubItems[1].Text.ToString(), this.gridViewTable.SelectedItems[0].SubItems[2].Text.ToString(), this.calcOnCombo.Text, this.cboPurpose.Text == "Purchase"))
      {
        if (tableDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.gridViewTable.SelectedItems[0].Text = tableDialog.UpToAmount;
        this.gridViewTable.SelectedItems[0].SubItems[1].Text = tableDialog.Base;
        this.gridViewTable.SelectedItems[0].SubItems[2].Text = tableDialog.Factor;
        this.gridViewTable.Sort(0, SortOrder.Ascending);
      }
    }

    private void btnIconDelete_Click(object sender, EventArgs e)
    {
      int index = this.gridViewTable.SelectedItems[0].Index;
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem selectedItem in this.gridViewTable.SelectedItems)
        gvItemList.Add(selectedItem);
      foreach (GVItem gvItem in gvItemList)
        this.gridViewTable.Items.Remove(gvItem);
      if (this.gridViewTable.Items.Count == 0)
        return;
      if (index + 1 > this.gridViewTable.Items.Count)
        this.gridViewTable.Items[this.gridViewTable.Items.Count - 1].Selected = true;
      else
        this.gridViewTable.Items[index].Selected = true;
    }

    private void gridViewTable_DoubleClick(object sender, EventArgs e)
    {
      this.btnIconEdit_Click((object) null, (EventArgs) null);
    }
  }
}
