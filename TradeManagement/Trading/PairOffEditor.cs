// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.PairOffEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class PairOffEditor : UserControl
  {
    private PairOff pairOff;
    private bool modified;
    private bool locked;
    private bool readOnly;
    private IContainer components;
    private Label label1;
    private Label label2;
    private Label label3;
    private DatePicker dtDate;
    private TextBox txtUndeliveredAmt;
    private TextBox txtFee;
    private FieldLockButton fLock;

    public event EventHandler DataChange;

    public event CalculateFeeEventHandler CalculateFee;

    public PairOffEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtUndeliveredAmt, TextBoxContentRule.NonNegativeDecimal, "#,##0.00;;\"\"");
      TextBoxFormatter.Attach(this.txtFee, TextBoxContentRule.NonNegativeDecimal, "#,##0.00;;\"\"");
    }

    [Browsable(false)]
    public PairOff PairOff
    {
      get => this.pairOff;
      set
      {
        this.pairOff = value;
        if (value == null)
          return;
        this.loadPairOffData();
      }
    }

    public bool DataModified
    {
      get => this.modified && !this.readOnly;
      set => this.modified = value;
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        if (this.readOnly == value)
          return;
        this.readOnly = value;
        this.dtDate.ReadOnly = this.readOnly;
        this.txtUndeliveredAmt.ReadOnly = this.readOnly;
        this.txtFee.ReadOnly = !this.pairOff.Locked || this.readOnly;
      }
    }

    public void CommitChanges()
    {
      this.pairOff.Date = this.dtDate.Value;
      this.pairOff.UndeliveredAmount = Utils.ParseDecimal((object) this.txtUndeliveredAmt.Text);
      this.pairOff.Locked = this.locked;
      this.pairOff.Fee = !this.locked ? 0M : Utils.ParseDecimal((object) this.txtFee.Text);
      this.modified = false;
    }

    private void loadPairOffData()
    {
      this.dtDate.Value = this.pairOff.Date;
      this.txtUndeliveredAmt.Text = "";
      this.txtFee.Text = "";
      this.txtUndeliveredAmt.Text = this.pairOff.UndeliveredAmount.ToString("#,##0.00;;\"\"");
      if (this.pairOff.Locked)
        this.txtFee.Text = this.pairOff.Fee.ToString("#,##0.00;;\"\"");
      else
        this.calculateFee();
      this.locked = this.pairOff.Locked;
      this.fLock.Locked = this.pairOff.Locked;
      this.txtFee.ReadOnly = !this.pairOff.Locked || this.readOnly;
      this.modified = false;
    }

    private void onFieldValueChanged(object sender, EventArgs e) => this.onDataChange();

    private void fLock_Click(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      this.fLock.Locked = !this.fLock.Locked;
      this.txtFee.ReadOnly = !this.fLock.Locked;
      this.locked = this.fLock.Locked;
      if (!this.txtFee.ReadOnly)
        return;
      this.calculateFee();
    }

    private void onDataChange()
    {
      this.modified = true;
      if (this.DataChange == null)
        return;
      this.DataChange((object) this, EventArgs.Empty);
    }

    private void calculateFee()
    {
      CalculateFeeEventArgs e = new CalculateFeeEventArgs(Utils.ParseDecimal((object) this.txtUndeliveredAmt.Text));
      if (this.CalculateFee != null)
        this.CalculateFee((object) this, e);
      this.txtFee.Text = e.Fee.ToString("#,##0.00;;\"\"");
    }

    private void txtUndeliveredAmt_Validated(object sender, EventArgs e)
    {
      if (!this.txtFee.ReadOnly)
        return;
      this.calculateFee();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtUndeliveredAmt = new TextBox();
      this.txtFee = new TextBox();
      this.fLock = new FieldLockButton();
      this.dtDate = new DatePicker();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(0, 4);
      this.label1.Name = "label1";
      this.label1.Size = new Size(69, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Pair Off Date";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(0, 28);
      this.label2.Name = "label2";
      this.label2.Size = new Size(104, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Undelivered Amount";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(0, 51);
      this.label3.Name = "label3";
      this.label3.Size = new Size(66, 14);
      this.label3.TabIndex = 2;
      this.label3.Text = "Pair Off Amt";
      this.txtUndeliveredAmt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtUndeliveredAmt.Location = new Point(110, 25);
      this.txtUndeliveredAmt.MaxLength = 12;
      this.txtUndeliveredAmt.Name = "txtUndeliveredAmt";
      this.txtUndeliveredAmt.Size = new Size(94, 20);
      this.txtUndeliveredAmt.TabIndex = 4;
      this.txtUndeliveredAmt.TextAlign = HorizontalAlignment.Right;
      this.txtUndeliveredAmt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtUndeliveredAmt.Validated += new EventHandler(this.txtUndeliveredAmt_Validated);
      this.txtFee.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtFee.Location = new Point(110, 48);
      this.txtFee.MaxLength = 12;
      this.txtFee.Name = "txtFee";
      this.txtFee.Size = new Size(94, 20);
      this.txtFee.TabIndex = 5;
      this.txtFee.TextAlign = HorizontalAlignment.Right;
      this.txtFee.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.fLock.Location = new Point(90, 50);
      this.fLock.MaximumSize = new Size(16, 16);
      this.fLock.MinimumSize = new Size(16, 16);
      this.fLock.Name = "fLock";
      this.fLock.Size = new Size(16, 16);
      this.fLock.TabIndex = 9;
      this.fLock.Click += new EventHandler(this.fLock_Click);
      this.dtDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dtDate.BackColor = SystemColors.Window;
      this.dtDate.Location = new Point(110, 0);
      this.dtDate.Name = "dtDate";
      this.dtDate.Size = new Size(94, 22);
      this.dtDate.TabIndex = 3;
      this.dtDate.Value = new DateTime(0L);
      this.dtDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.fLock);
      this.Controls.Add((Control) this.txtFee);
      this.Controls.Add((Control) this.txtUndeliveredAmt);
      this.Controls.Add((Control) this.dtDate);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (PairOffEditor);
      this.Size = new Size(204, 83);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
