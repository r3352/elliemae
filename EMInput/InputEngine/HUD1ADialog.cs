// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HUD1ADialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HUD1ADialog : Form
  {
    private Button cancelBtn;
    private Button okBtn;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private Label label10;
    private Label label14;
    private TextBox txt1600;
    private Label label11;
    private IContainer components;
    private LoanData loan;
    private ToolTip fieldToolTip;
    private TextBox txt1601;
    private TextBox txt1602;
    private TextBox txt1603;
    private TextBox txt1604;
    private TextBox txtTotal;
    private PictureBox pboxAsterisk;
    private IMainScreen mainScreen;
    private PictureBox pboxDownArrow;
    private TableContainer tableContainer1;
    private GridView listViewLiabs;
    private GroupContainer groupContainer1;
    private PopupBusinessRules popupRules;

    public HUD1ADialog(LoanData loan, IMainScreen mainScreen)
    {
      this.loan = loan;
      this.mainScreen = mainScreen;
      this.InitializeComponent();
      ResourceManager resources = new ResourceManager(typeof (HUD1ADialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      this.popupRules.SetBusinessRules((object) this.txt1601, "HUD1A.X33");
      this.txt1601.Tag = (object) "HUD1A.X33";
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (HUD1ADialog));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label14 = new Label();
      this.txt1600 = new TextBox();
      this.txt1601 = new TextBox();
      this.txt1602 = new TextBox();
      this.txt1603 = new TextBox();
      this.txt1604 = new TextBox();
      this.txtTotal = new TextBox();
      this.label11 = new Label();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.tableContainer1 = new TableContainer();
      this.listViewLiabs = new GridView();
      this.groupContainer1 = new GroupContainer();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.tableContainer1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(437, 458);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 9;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.BackColor = SystemColors.Control;
      this.okBtn.Location = new Point(353, 458);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 8;
      this.okBtn.Text = "&OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(10, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 13);
      this.label1.TabIndex = 11;
      this.label1.Text = "1600.";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(46, 33);
      this.label2.Name = "label2";
      this.label2.Size = new Size(70, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "Loan Amount";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(46, 57);
      this.label3.Name = "label3";
      this.label3.Size = new Size(158, 13);
      this.label3.TabIndex = 14;
      this.label3.Text = "Plus Cash/Check from Borrower";
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(10, 57);
      this.label4.Name = "label4";
      this.label4.Size = new Size(34, 13);
      this.label4.TabIndex = 13;
      this.label4.Text = "1601.";
      this.label5.AutoSize = true;
      this.label5.BackColor = Color.Transparent;
      this.label5.Location = new Point(46, 81);
      this.label5.Name = "label5";
      this.label5.Size = new Size(209, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Minus Total Settlement Charges (line 1400)";
      this.label6.AutoSize = true;
      this.label6.BackColor = Color.Transparent;
      this.label6.Location = new Point(10, 81);
      this.label6.Name = "label6";
      this.label6.Size = new Size(34, 13);
      this.label6.TabIndex = 15;
      this.label6.Text = "1602.";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(46, 105);
      this.label7.Name = "label7";
      this.label7.Size = new Size(232, 13);
      this.label7.TabIndex = 18;
      this.label7.Text = "Minus Total Disbursements to Others (line 1520)";
      this.label8.AutoSize = true;
      this.label8.BackColor = Color.Transparent;
      this.label8.Location = new Point(10, 105);
      this.label8.Name = "label8";
      this.label8.Size = new Size(34, 13);
      this.label8.TabIndex = 17;
      this.label8.Text = "1603.";
      this.label9.AutoSize = true;
      this.label9.BackColor = Color.Transparent;
      this.label9.Location = new Point(46, 129);
      this.label9.Name = "label9";
      this.label9.Size = new Size(168, 13);
      this.label9.TabIndex = 20;
      this.label9.Text = "Equals Disbursements to Borrower";
      this.label10.AutoSize = true;
      this.label10.BackColor = Color.Transparent;
      this.label10.Location = new Point(10, 129);
      this.label10.Name = "label10";
      this.label10.Size = new Size(34, 13);
      this.label10.TabIndex = 19;
      this.label10.Text = "1604.";
      this.label14.AutoSize = true;
      this.label14.BackColor = Color.Transparent;
      this.label14.Location = new Point(11, 147);
      this.label14.Name = "label14";
      this.label14.Size = new Size(320, 13);
      this.label14.TabIndex = 23;
      this.label14.Text = "(after expiration of any applicable rescission period required by law)";
      this.txt1600.BackColor = Color.WhiteSmoke;
      this.txt1600.Location = new Point(389, 33);
      this.txt1600.Name = "txt1600";
      this.txt1600.ReadOnly = true;
      this.txt1600.Size = new Size(108, 20);
      this.txt1600.TabIndex = 3;
      this.txt1600.TabStop = false;
      this.txt1600.Tag = (object) "2";
      this.txt1600.TextAlign = HorizontalAlignment.Right;
      this.txt1601.Location = new Point(389, 57);
      this.txt1601.Name = "txt1601";
      this.txt1601.Size = new Size(108, 20);
      this.txt1601.TabIndex = 4;
      this.txt1601.Tag = (object) "HUD1A.X33";
      this.txt1601.TextAlign = HorizontalAlignment.Right;
      this.txt1601.TextChanged += new EventHandler(this.txt1601_TextChanged);
      this.txt1601.Leave += new EventHandler(this.textBox_Leave);
      this.txt1601.KeyUp += new KeyEventHandler(this.keyup);
      this.txt1601.Enter += new EventHandler(this.textBox_Enter);
      this.txt1602.BackColor = Color.WhiteSmoke;
      this.txt1602.Location = new Point(389, 81);
      this.txt1602.Name = "txt1602";
      this.txt1602.ReadOnly = true;
      this.txt1602.Size = new Size(108, 20);
      this.txt1602.TabIndex = 5;
      this.txt1602.TabStop = false;
      this.txt1602.Tag = (object) "L351";
      this.txt1602.TextAlign = HorizontalAlignment.Right;
      this.txt1603.BackColor = Color.WhiteSmoke;
      this.txt1603.Location = new Point(389, 105);
      this.txt1603.Name = "txt1603";
      this.txt1603.ReadOnly = true;
      this.txt1603.Size = new Size(108, 20);
      this.txt1603.TabIndex = 6;
      this.txt1603.TabStop = false;
      this.txt1603.Tag = (object) "HUD1A.X31";
      this.txt1603.TextAlign = HorizontalAlignment.Right;
      this.txt1604.BackColor = Color.WhiteSmoke;
      this.txt1604.Location = new Point(389, 129);
      this.txt1604.Name = "txt1604";
      this.txt1604.ReadOnly = true;
      this.txt1604.Size = new Size(108, 20);
      this.txt1604.TabIndex = 7;
      this.txt1604.TabStop = false;
      this.txt1604.Tag = (object) "HUD1A.X32";
      this.txt1604.TextAlign = HorizontalAlignment.Right;
      this.txtTotal.BackColor = Color.WhiteSmoke;
      this.txtTotal.Location = new Point(391, 247);
      this.txtTotal.Name = "txtTotal";
      this.txtTotal.ReadOnly = true;
      this.txtTotal.Size = new Size(108, 20);
      this.txtTotal.TabIndex = 2;
      this.txtTotal.TabStop = false;
      this.txtTotal.Tag = (object) "HUD1A.X31";
      this.txtTotal.TextAlign = HorizontalAlignment.Right;
      this.label11.AutoSize = true;
      this.label11.BackColor = Color.Transparent;
      this.label11.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label11.Location = new Point(160, 250);
      this.label11.Name = "label11";
      this.label11.Size = new Size(228, 14);
      this.label11.TabIndex = 31;
      this.label11.Text = "1520. TOTAL DISBURSED (enter on line 1603)";
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(307, 61);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 68;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.BackColor = Color.WhiteSmoke;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(307, 41);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 65;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.tableContainer1.Controls.Add((Control) this.listViewLiabs);
      this.tableContainer1.Controls.Add((Control) this.txtTotal);
      this.tableContainer1.Controls.Add((Control) this.label11);
      this.tableContainer1.Location = new Point(8, 8);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(504, 270);
      this.tableContainer1.TabIndex = 69;
      this.tableContainer1.Text = "Disbursement to Others";
      this.listViewLiabs.BorderStyle = BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Paid Off";
      gvColumn1.Width = 57;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Creditor Name";
      gvColumn2.Width = 253;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Balance";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 86;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Payoff Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 106;
      this.listViewLiabs.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listViewLiabs.Dock = DockStyle.Fill;
      this.listViewLiabs.Location = new Point(1, 26);
      this.listViewLiabs.Name = "listViewLiabs";
      this.listViewLiabs.Size = new Size(502, 218);
      this.listViewLiabs.SortOption = GVSortOption.None;
      this.listViewLiabs.TabIndex = 0;
      this.listViewLiabs.ItemDoubleClick += new GVItemEventHandler(this.listViewLiabs_ItemDoubleClick);
      this.groupContainer1.Controls.Add((Control) this.txt1600);
      this.groupContainer1.Controls.Add((Control) this.txt1601);
      this.groupContainer1.Controls.Add((Control) this.pboxDownArrow);
      this.groupContainer1.Controls.Add((Control) this.txt1602);
      this.groupContainer1.Controls.Add((Control) this.pboxAsterisk);
      this.groupContainer1.Controls.Add((Control) this.txt1603);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.txt1604);
      this.groupContainer1.Controls.Add((Control) this.label10);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(8, 277);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(504, 175);
      this.groupContainer1.TabIndex = 70;
      this.groupContainer1.Text = "Net Settlement";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(521, 491);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.tableContainer1);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (HUD1ADialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "HUD-1A";
      this.Load += new EventHandler(this.HUD1ADialog_Load);
      this.Activated += new EventHandler(this.HUD1ADialog_Activated);
      this.KeyPress += new KeyPressEventHandler(this.HUD1ADialog_KeyPress);
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.tableContainer1.ResumeLayout(false);
      this.tableContainer1.PerformLayout();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      this.listViewLiabs.Items.Clear();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        GVItem gvItem = new GVItem(index.ToString());
        gvItem.SubItems.Add((object) this.loan.GetSimpleField(str + "02"));
        gvItem.SubItems.Add((object) this.loan.GetField(str + "13"));
        gvItem.SubItems.Add((object) this.loan.GetField(str + "16"));
        if (this.loan.GetSimpleField(str + "18") == "Y")
          gvItem.SubItems[0].Checked = true;
        this.listViewLiabs.Items.Add(gvItem);
      }
      TextBox txtTotal = this.txtTotal;
      double num = Utils.ParseDouble((object) this.loan.GetSimpleField("HUD1A.X31"));
      string str1 = num.ToString("N2");
      txtTotal.Text = str1;
      TextBox txt1600 = this.txt1600;
      num = Utils.ParseDouble((object) this.loan.GetSimpleField("2"));
      string str2 = num.ToString("N2");
      txt1600.Text = str2;
      TextBox txt1602 = this.txt1602;
      num = Utils.ParseDouble((object) this.loan.GetSimpleField("L351"));
      string str3 = num.ToString("N2");
      txt1602.Text = str3;
      TextBox txt1603 = this.txt1603;
      num = Utils.ParseDouble((object) this.loan.GetSimpleField("HUD1A.X31"));
      string str4 = num.ToString("N2");
      txt1603.Text = str4;
      TextBox txt1604 = this.txt1604;
      num = Utils.ParseDouble((object) this.loan.GetSimpleField("HUD1A.X32"));
      string str5 = num.ToString("N2");
      txt1604.Text = str5;
      TextBox txt1601 = this.txt1601;
      num = Utils.ParseDouble((object) this.loan.GetSimpleField("HUD1A.X33"));
      string str6 = num.ToString("N2");
      txt1601.Text = str6;
    }

    private void textBox_Leave(object sender, EventArgs e)
    {
      TextBox ctrl = (TextBox) sender;
      if (this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag))
      {
        this.loan.SetCurrentField(ctrl.Tag.ToString(), ctrl.Text);
        if (ctrl.Name == "txt1601")
        {
          double num = Utils.ParseDouble((object) ctrl.Text);
          if (num != 0.0)
            ctrl.Text = num.ToString("N2");
          else
            ctrl.Text = string.Empty;
        }
      }
      ctrl.BackColor = Color.White;
    }

    private void textBox_Enter(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      textBox.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 204);
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(textBox.Tag.ToString());
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      for (int index = 1; index <= this.listViewLiabs.Items.Count; ++index)
      {
        string str = "FL" + index.ToString("00");
        if (this.listViewLiabs.Items[index - 1].SubItems[0].Checked)
        {
          this.loan.SetCurrentField(str + "18", "Y");
          this.loan.SetCurrentField(str + "16", this.listViewLiabs.Items[index - 1].SubItems[3].Text);
        }
        else
        {
          this.loan.SetCurrentField(str + "16", "");
          this.loan.SetCurrentField(str + "18", "");
        }
      }
      if (this.loan.GetField("19").IndexOf("Refinance") > -1 && Utils.Dialog((IWin32Window) this, "Do you want to copy line 1603 (Total Disbursements to Others) to line 104 on HUD-1 Page 1?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
      {
        this.loan.SetCurrentField("L84", "Payoff");
        this.loan.SetField("L85", this.txt1603.Text);
      }
      this.loan.SetField("HUD1A.X31", this.txtTotal.Text);
      this.loan.SetCurrentField("HUD1A.X32", this.txt1604.Text);
      Cursor.Current = Cursors.Default;
      this.DialogResult = DialogResult.OK;
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

    private void txt1601_TextChanged(object sender, EventArgs e)
    {
      this.txt1604.Text = (Utils.ParseDouble((object) this.txt1600.Text) + Utils.ParseDouble((object) this.txt1601.Text) - Utils.ParseDouble((object) this.txt1602.Text) - Utils.ParseDouble((object) this.txt1603.Text)).ToString("N2");
    }

    private void HUD1ADialog_Activated(object sender, EventArgs e)
    {
      if (this.listViewLiabs.Items.Count <= 0 || !this.listViewLiabs.Items[0].SubItems[0].Checked)
        return;
      this.listViewLiabs.Items[0].SubItems[3].Text = this.loan.GetField("FL0116");
    }

    private void HUD1ADialog_Load(object sender, EventArgs e)
    {
      this.initForm();
      this.fieldToolTip.SetToolTip((Control) this.txtTotal, "HUD1A.X31");
      this.fieldToolTip.SetToolTip((Control) this.txt1600, "2");
      this.fieldToolTip.SetToolTip((Control) this.txt1601, "HUD1A.X33");
      this.fieldToolTip.SetToolTip((Control) this.txt1602, "L351");
      this.fieldToolTip.SetToolTip((Control) this.txt1603, "HUD1A.X31");
      this.fieldToolTip.SetToolTip((Control) this.txt1604, "HUD1A.X32");
      this.listViewLiabs.SubItemCheck += new GVSubItemEventHandler(this.listViewLiabs_SubItemCheck);
    }

    private void HUD1ADialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void listViewLiabs_SubItemCheck(object source, GVSubItemEventArgs e)
    {
      double num = 0.0;
      e.SubItem.Item.SubItems[3].Text = !e.SubItem.Checked ? "" : e.SubItem.Item.SubItems[2].Text;
      string empty = string.Empty;
      for (int nItemIndex = 0; nItemIndex < this.listViewLiabs.Items.Count; ++nItemIndex)
      {
        if (this.listViewLiabs.Items[nItemIndex].Checked)
        {
          string str = this.listViewLiabs.Items[nItemIndex].SubItems[3].Text.Replace(",", "");
          num += Utils.ParseDouble((object) str);
        }
      }
      this.txtTotal.Text = num.ToString("N2");
      this.txt1603.Text = num.ToString("N2");
      this.txt1604.Text = (Utils.ParseDouble((object) this.txt1600.Text) + Utils.ParseDouble((object) this.txt1601.Text) - Utils.ParseDouble((object) this.txt1602.Text) - num).ToString("N2");
    }

    private void listViewLiabs_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      e.Item.Checked = !e.Item.Checked;
    }
  }
}
