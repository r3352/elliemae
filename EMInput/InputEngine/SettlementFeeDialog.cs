// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SettlementFeeDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SettlementFeeDialog : Form
  {
    private IHtmlInput iData;
    private string escrowTableName = string.Empty;
    private PopupBusinessRules popupRules;
    private LoanData loan;
    private bool readOnly;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Label label1;
    private TextBox txtEscrowFee;
    private StandardIconButton btnIconEscrow;
    private Label label2;
    private Label label3;
    private TextBox txtFee1;
    private TextBox txtDesc1;
    private TextBox txtDesc2;
    private TextBox txtFee2;
    private TextBox txtDesc3;
    private TextBox txtFee3;
    private TextBox txtDesc4;
    private TextBox txtFee4;
    private TextBox txtDesc5;
    private TextBox txtFee5;
    private Label label4;
    private Label labelTotal;
    private ToolTip fieldToolTip;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private Label label5;
    private TextBox txtTableName;

    public SettlementFeeDialog(IHtmlInput iData, bool readOnly)
    {
      this.readOnly = readOnly;
      this.iData = iData;
      this.InitializeComponent();
      this.initForm();
      this.refreshTotal();
    }

    private void initForm()
    {
      if (this.iData is LoanData)
      {
        this.loan = (LoanData) this.iData;
        ResourceManager resources = new ResourceManager(typeof (SettlementFeeDialog));
        this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
        this.popupRules.DropdownSelected += new EventHandler(this.popupRules_DropdownSelected);
        if (this.getField("1172") == "VA")
        {
          this.txtDesc1.ReadOnly = this.txtDesc2.ReadOnly = this.txtDesc3.ReadOnly = this.txtDesc4.ReadOnly = this.txtDesc5.ReadOnly = true;
          this.txtFee1.ReadOnly = this.txtFee2.ReadOnly = this.txtFee3.ReadOnly = this.txtFee4.ReadOnly = this.txtFee5.ReadOnly = true;
        }
      }
      this.loadFieldValues(this.Controls);
      this.escrowTableName = this.getField("ESCROW_TABLE");
    }

    private void numericField_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = Utils.ParseDouble((object) textBox.Text.Trim());
      if (num != 0.0)
        textBox.Text = num.ToString("N2");
      else
        textBox.Text = string.Empty;
      this.refreshTotal();
    }

    private void refreshTotal()
    {
      this.labelTotal.Text = "$ " + (Utils.ParseDouble((object) this.txtFee1.Text) + Utils.ParseDouble((object) this.txtFee2.Text) + Utils.ParseDouble((object) this.txtFee3.Text) + Utils.ParseDouble((object) this.txtFee4.Text) + Utils.ParseDouble((object) this.txtFee5.Text) + Utils.ParseDouble((object) this.txtEscrowFee.Text)).ToString("N2");
    }

    private void popupRules_DropdownSelected(object sender, EventArgs e)
    {
      switch (sender)
      {
        case ComboBox _:
          ComboBox comboBox = (ComboBox) sender;
          this.setField(comboBox.Tag.ToString(), comboBox.Text);
          break;
        case TextBox _:
          TextBox textBox = (TextBox) sender;
          this.setField(textBox.Tag.ToString(), textBox.Text);
          break;
      }
    }

    private void Num2Field_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatField(FieldFormat.DECIMAL_2, (TextBox) sender);
    }

    private void formatField(FieldFormat format, TextBox box)
    {
      bool needsUpdate = false;
      string str = Utils.FormatInput(box.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      box.Text = str;
      box.SelectionStart = str.Length;
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (!this.readOnly)
      {
        this.saveFieldValues(this.Controls);
        if (this.iData is LoanData)
          this.setField("ESCROW_TABLE", this.escrowTableName);
        double num = Utils.ParseDouble((object) this.labelTotal.Text);
        this.setField("NEWHUD.X645", num != 0.0 ? num.ToString() : "");
        if (this.getField("14").ToUpper() == "CA")
        {
          if (this.getField("RE882.X56") == string.Empty && this.getField("NEWHUD.X782") != string.Empty)
            num += Utils.ParseDouble((object) this.getField("NEWHUD.X782"));
          this.setField("RE882.X57", num != 0.0 ? num.ToString() : "");
        }
      }
      this.DialogResult = DialogResult.OK;
    }

    private string getField(string id) => this.iData.GetField(id);

    private void setField(string id, string val) => this.iData.SetCurrentField(id, val);

    private void SettlementFeeDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnIconEscrow_Click(object sender, EventArgs e)
    {
      TableHandler tableHandler = new TableHandler(this.iData is LoanData ? (LoanData) this.iData : (LoanData) null, "2010_Escrow", Session.DefaultInstance);
      if (!tableHandler.LookUpTable("Escrow"))
        return;
      if (this.iData is LoanData)
        this.txtEscrowFee.Text = tableHandler.EscrowFee.ToString("N2");
      this.txtTableName.Text = tableHandler.EscrowTableName;
      this.escrowTableName = tableHandler.EscrowTableName;
      this.refreshTotal();
      this.txtEscrowFee.Focus();
    }

    private void saveFieldValues(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
          case DatePicker _:
            if (c.Tag != null)
            {
              string id = c.Tag.ToString();
              if (!(id == string.Empty))
              {
                this.setField(id, c.Text);
                continue;
              }
              continue;
            }
            continue;
          default:
            this.saveFieldValues(c.Controls);
            continue;
        }
      }
    }

    private void loadFieldValues(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
          case DatePicker _:
            if (c.Tag != null)
            {
              string str = c.Tag.ToString();
              if (!(str == string.Empty))
              {
                c.Text = string.Empty;
                if (this.readOnly && c is TextBox)
                {
                  ((TextBoxBase) c).ReadOnly = true;
                  c.BackColor = Color.WhiteSmoke;
                }
                this.fieldToolTip.SetToolTip(c, str);
                string field = this.getField(str);
                switch (c)
                {
                  case TextBox _:
                    TextBox ctrl1 = (TextBox) c;
                    ctrl1.Text = field;
                    if (!this.readOnly && this.iData is LoanData)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl1, str);
                      continue;
                    }
                    continue;
                  case ComboBox _:
                    ComboBox ctrl2 = (ComboBox) c;
                    ctrl2.Text = field;
                    if (this.readOnly && this.iData is LoanData)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl2, str);
                      continue;
                    }
                    continue;
                  case DatePicker _:
                    DatePicker ctrl3 = (DatePicker) c;
                    ctrl3.Text = field;
                    if (!this.readOnly && this.iData is LoanData)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl3, str);
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
              else
                continue;
            }
            else
              continue;
          default:
            this.loadFieldValues(c.Controls);
            continue;
        }
      }
    }

    private void txtTableName_Leave(object sender, EventArgs e)
    {
      this.escrowTableName = this.txtTableName.Text.Trim();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SettlementFeeDialog));
      this.dialogButtons1 = new DialogButtons();
      this.label1 = new Label();
      this.txtEscrowFee = new TextBox();
      this.btnIconEscrow = new StandardIconButton();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtFee1 = new TextBox();
      this.txtDesc1 = new TextBox();
      this.txtDesc2 = new TextBox();
      this.txtFee2 = new TextBox();
      this.txtDesc3 = new TextBox();
      this.txtFee3 = new TextBox();
      this.txtDesc4 = new TextBox();
      this.txtFee4 = new TextBox();
      this.txtDesc5 = new TextBox();
      this.txtFee5 = new TextBox();
      this.label4 = new Label();
      this.labelTotal = new Label();
      this.fieldToolTip = new ToolTip(this.components);
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.label5 = new Label();
      this.txtTableName = new TextBox();
      ((ISupportInitialize) this.btnIconEscrow).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 241);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(366, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(9, 45);
      this.label1.Name = "label1";
      this.label1.Size = new Size(71, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Description";
      this.txtEscrowFee.Location = new Point(238, 66);
      this.txtEscrowFee.Name = "txtEscrowFee";
      this.txtEscrowFee.Size = new Size(114, 20);
      this.txtEscrowFee.TabIndex = 1;
      this.txtEscrowFee.Tag = (object) "NEWHUD.X808";
      this.txtEscrowFee.TextAlign = HorizontalAlignment.Right;
      this.txtEscrowFee.Leave += new EventHandler(this.numericField_Leave);
      this.txtEscrowFee.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.btnIconEscrow.BackColor = Color.Transparent;
      this.btnIconEscrow.Location = new Point(214, 68);
      this.btnIconEscrow.MouseDownImage = (Image) null;
      this.btnIconEscrow.Name = "btnIconEscrow";
      this.btnIconEscrow.Size = new Size(16, 16);
      this.btnIconEscrow.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.btnIconEscrow.TabIndex = 3;
      this.btnIconEscrow.TabStop = false;
      this.btnIconEscrow.Click += new EventHandler(this.btnIconEscrow_Click);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(208, 45);
      this.label2.Name = "label2";
      this.label2.Size = new Size(28, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Fee";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(9, 69);
      this.label3.Name = "label3";
      this.label3.Size = new Size(63, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Escrow Fee";
      this.txtFee1.Location = new Point(238, 92);
      this.txtFee1.Name = "txtFee1";
      this.txtFee1.Size = new Size(114, 20);
      this.txtFee1.TabIndex = 3;
      this.txtFee1.Tag = (object) "NEWHUD.X810";
      this.txtFee1.TextAlign = HorizontalAlignment.Right;
      this.txtFee1.Leave += new EventHandler(this.numericField_Leave);
      this.txtFee1.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.txtDesc1.Location = new Point(12, 92);
      this.txtDesc1.Name = "txtDesc1";
      this.txtDesc1.Size = new Size(220, 20);
      this.txtDesc1.TabIndex = 2;
      this.txtDesc1.Tag = (object) "NEWHUD.X809";
      this.txtDesc2.Location = new Point(12, 118);
      this.txtDesc2.Name = "txtDesc2";
      this.txtDesc2.Size = new Size(220, 20);
      this.txtDesc2.TabIndex = 4;
      this.txtDesc2.Tag = (object) "NEWHUD.X811";
      this.txtFee2.Location = new Point(238, 118);
      this.txtFee2.Name = "txtFee2";
      this.txtFee2.Size = new Size(114, 20);
      this.txtFee2.TabIndex = 5;
      this.txtFee2.Tag = (object) "NEWHUD.X812";
      this.txtFee2.TextAlign = HorizontalAlignment.Right;
      this.txtFee2.Leave += new EventHandler(this.numericField_Leave);
      this.txtFee2.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.txtDesc3.Location = new Point(12, 144);
      this.txtDesc3.Name = "txtDesc3";
      this.txtDesc3.Size = new Size(220, 20);
      this.txtDesc3.TabIndex = 6;
      this.txtDesc3.Tag = (object) "NEWHUD.X813";
      this.txtFee3.Location = new Point(238, 144);
      this.txtFee3.Name = "txtFee3";
      this.txtFee3.Size = new Size(114, 20);
      this.txtFee3.TabIndex = 7;
      this.txtFee3.Tag = (object) "NEWHUD.X814";
      this.txtFee3.TextAlign = HorizontalAlignment.Right;
      this.txtFee3.Leave += new EventHandler(this.numericField_Leave);
      this.txtFee3.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.txtDesc4.Location = new Point(12, 170);
      this.txtDesc4.Name = "txtDesc4";
      this.txtDesc4.Size = new Size(220, 20);
      this.txtDesc4.TabIndex = 8;
      this.txtDesc4.Tag = (object) "NEWHUD.X815";
      this.txtFee4.Location = new Point(238, 170);
      this.txtFee4.Name = "txtFee4";
      this.txtFee4.Size = new Size(114, 20);
      this.txtFee4.TabIndex = 9;
      this.txtFee4.Tag = (object) "NEWHUD.X816";
      this.txtFee4.TextAlign = HorizontalAlignment.Right;
      this.txtFee4.Leave += new EventHandler(this.numericField_Leave);
      this.txtFee4.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.txtDesc5.Location = new Point(12, 196);
      this.txtDesc5.Name = "txtDesc5";
      this.txtDesc5.Size = new Size(220, 20);
      this.txtDesc5.TabIndex = 10;
      this.txtDesc5.Tag = (object) "NEWHUD.X817";
      this.txtFee5.Location = new Point(238, 196);
      this.txtFee5.Name = "txtFee5";
      this.txtFee5.Size = new Size(114, 20);
      this.txtFee5.TabIndex = 11;
      this.txtFee5.Tag = (object) "NEWHUD.X818";
      this.txtFee5.TextAlign = HorizontalAlignment.Right;
      this.txtFee5.Leave += new EventHandler(this.numericField_Leave);
      this.txtFee5.KeyUp += new KeyEventHandler(this.Num2Field_KeyUp);
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(9, 228);
      this.label4.Name = "label4";
      this.label4.Size = new Size(36, 13);
      this.label4.TabIndex = 16;
      this.label4.Text = "Total";
      this.labelTotal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.labelTotal.Location = new Point(103, 228);
      this.labelTotal.Name = "labelTotal";
      this.labelTotal.Size = new Size(250, 13);
      this.labelTotal.TabIndex = 17;
      this.labelTotal.Text = "(Total)";
      this.labelTotal.TextAlign = ContentAlignment.MiddleRight;
      this.pboxDownArrow.BackColor = Color.Transparent;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(59, 224);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 69;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.BackColor = Color.Transparent;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(82, 229);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 68;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.label5.Location = new Point(9, 9);
      this.label5.Name = "label5";
      this.label5.Size = new Size(291, 29);
      this.label5.TabIndex = 71;
      this.label5.Text = "If this is a VA loan or you are running compliance testing, enter a fee amount in the Escrow Fee field only.";
      this.txtTableName.Location = new Point(78, 66);
      this.txtTableName.Name = "txtTableName";
      this.txtTableName.Size = new Size(132, 20);
      this.txtTableName.TabIndex = 0;
      this.txtTableName.Tag = (object) "ESCROW_TABLE";
      this.txtTableName.Leave += new EventHandler(this.txtTableName_Leave);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(366, 285);
      this.Controls.Add((Control) this.txtTableName);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.labelTotal);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtDesc5);
      this.Controls.Add((Control) this.txtFee5);
      this.Controls.Add((Control) this.txtDesc4);
      this.Controls.Add((Control) this.txtFee4);
      this.Controls.Add((Control) this.txtDesc3);
      this.Controls.Add((Control) this.txtFee3);
      this.Controls.Add((Control) this.txtDesc2);
      this.Controls.Add((Control) this.txtFee2);
      this.Controls.Add((Control) this.txtDesc1);
      this.Controls.Add((Control) this.txtFee1);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnIconEscrow);
      this.Controls.Add((Control) this.txtEscrowFee);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SettlementFeeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Settlement or Closing Fees";
      this.KeyDown += new KeyEventHandler(this.SettlementFeeDialog_KeyDown);
      ((ISupportInitialize) this.btnIconEscrow).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
