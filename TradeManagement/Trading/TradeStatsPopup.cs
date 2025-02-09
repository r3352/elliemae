// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeStatsPopup
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeStatsPopup : Form
  {
    private IContainer components;
    private Label lblTitle;
    private Label lblCouponTitle;
    private Label lblMarginTitle;
    private Label lblCeilingTitle;
    private Label lblChangeTitle;
    private Label lblFloorTitle;
    private Label lblCoupon;
    private Label lblMargin;
    private Label lblCeiling;
    private Label lblChange;
    private Label lblFloor;

    public TradeStatsPopup(
      Decimal coupon,
      Decimal margin,
      Decimal ceiling,
      Decimal change,
      Decimal floor)
    {
      this.InitializeComponent();
      this.lblCoupon.Text = this.numToString(coupon);
      this.lblMargin.Text = this.numToString(margin);
      this.lblCeiling.Text = this.numToString(ceiling);
      this.lblFloor.Text = this.numToString(floor);
      this.lblChange.Text = this.numToString(change);
    }

    public TradeStatsPopup(string title, string key, Decimal value)
    {
      this.InitializeComponent();
      this.lblMarginTitle.Visible = false;
      this.lblMargin.Visible = false;
      this.lblCeilingTitle.Visible = false;
      this.lblCeiling.Visible = false;
      this.lblFloorTitle.Visible = false;
      this.lblFloor.Visible = false;
      this.lblChangeTitle.Visible = false;
      this.lblChange.Visible = false;
      this.lblTitle.Text = title;
      this.lblCouponTitle.Text = key;
      this.lblCoupon.Text = this.numToString(value, true);
    }

    private string numToString(Decimal value, bool roundToInteger = false)
    {
      if (value == Decimal.MinValue)
        return "N/A";
      return !roundToInteger ? (value * 100M).ToString("0.000") + "%" : Math.Round(value * 100M).ToString() + "%";
    }

    private void TradeStatsPopup_Paint(object sender, PaintEventArgs e)
    {
      ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblTitle = new Label();
      this.lblCouponTitle = new Label();
      this.lblMarginTitle = new Label();
      this.lblCeilingTitle = new Label();
      this.lblChangeTitle = new Label();
      this.lblFloorTitle = new Label();
      this.lblCoupon = new Label();
      this.lblMargin = new Label();
      this.lblCeiling = new Label();
      this.lblChange = new Label();
      this.lblFloor = new Label();
      this.SuspendLayout();
      this.lblTitle.AutoSize = true;
      this.lblTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTitle.Location = new Point(7, 9);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(122, 13);
      this.lblTitle.TabIndex = 0;
      this.lblTitle.Text = "Weighted Averages:";
      this.lblCouponTitle.AutoSize = true;
      this.lblCouponTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblCouponTitle.Location = new Point(7, 31);
      this.lblCouponTitle.Name = "lblCouponTitle";
      this.lblCouponTitle.Size = new Size(47, 13);
      this.lblCouponTitle.TabIndex = 1;
      this.lblCouponTitle.Text = "Coupon:";
      this.lblMarginTitle.AutoSize = true;
      this.lblMarginTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblMarginTitle.Location = new Point(7, 52);
      this.lblMarginTitle.Name = "lblMarginTitle";
      this.lblMarginTitle.Size = new Size(42, 13);
      this.lblMarginTitle.TabIndex = 2;
      this.lblMarginTitle.Text = "Margin:";
      this.lblCeilingTitle.AutoSize = true;
      this.lblCeilingTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblCeilingTitle.Location = new Point(7, 72);
      this.lblCeilingTitle.Name = "lblCeilingTitle";
      this.lblCeilingTitle.Size = new Size(41, 13);
      this.lblCeilingTitle.TabIndex = 3;
      this.lblCeilingTitle.Text = "Ceiling:";
      this.lblChangeTitle.AutoSize = true;
      this.lblChangeTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblChangeTitle.Location = new Point(7, 93);
      this.lblChangeTitle.Name = "lblChangeTitle";
      this.lblChangeTitle.Size = new Size(47, 13);
      this.lblChangeTitle.TabIndex = 4;
      this.lblChangeTitle.Text = "Change:";
      this.lblFloorTitle.AutoSize = true;
      this.lblFloorTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblFloorTitle.Location = new Point(7, 113);
      this.lblFloorTitle.Name = "lblFloorTitle";
      this.lblFloorTitle.Size = new Size(33, 13);
      this.lblFloorTitle.TabIndex = 5;
      this.lblFloorTitle.Text = "Floor:";
      this.lblCoupon.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCoupon.Location = new Point(78, 31);
      this.lblCoupon.Name = "lblCoupon";
      this.lblCoupon.Size = new Size(71, 14);
      this.lblCoupon.TabIndex = 6;
      this.lblCoupon.Text = "########";
      this.lblCoupon.TextAlign = ContentAlignment.TopRight;
      this.lblMargin.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblMargin.Location = new Point(78, 52);
      this.lblMargin.Name = "lblMargin";
      this.lblMargin.Size = new Size(71, 14);
      this.lblMargin.TabIndex = 7;
      this.lblMargin.Text = "########";
      this.lblMargin.TextAlign = ContentAlignment.TopRight;
      this.lblCeiling.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCeiling.Location = new Point(78, 71);
      this.lblCeiling.Name = "lblCeiling";
      this.lblCeiling.Size = new Size(71, 14);
      this.lblCeiling.TabIndex = 8;
      this.lblCeiling.Text = "########";
      this.lblCeiling.TextAlign = ContentAlignment.TopRight;
      this.lblChange.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblChange.Location = new Point(78, 93);
      this.lblChange.Name = "lblChange";
      this.lblChange.Size = new Size(71, 14);
      this.lblChange.TabIndex = 9;
      this.lblChange.Text = "########";
      this.lblChange.TextAlign = ContentAlignment.TopRight;
      this.lblFloor.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblFloor.Location = new Point(78, 113);
      this.lblFloor.Name = "lblFloor";
      this.lblFloor.Size = new Size(71, 14);
      this.lblFloor.TabIndex = 10;
      this.lblFloor.Text = "########";
      this.lblFloor.TextAlign = ContentAlignment.TopRight;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 192);
      this.ClientSize = new Size(154, 139);
      this.Controls.Add((Control) this.lblFloor);
      this.Controls.Add((Control) this.lblChange);
      this.Controls.Add((Control) this.lblCeiling);
      this.Controls.Add((Control) this.lblMargin);
      this.Controls.Add((Control) this.lblCoupon);
      this.Controls.Add((Control) this.lblFloorTitle);
      this.Controls.Add((Control) this.lblChangeTitle);
      this.Controls.Add((Control) this.lblCeilingTitle);
      this.Controls.Add((Control) this.lblMarginTitle);
      this.Controls.Add((Control) this.lblCouponTitle);
      this.Controls.Add((Control) this.lblTitle);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TradeStatsPopup);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = nameof (TradeStatsPopup);
      this.Paint += new PaintEventHandler(this.TradeStatsPopup_Paint);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
