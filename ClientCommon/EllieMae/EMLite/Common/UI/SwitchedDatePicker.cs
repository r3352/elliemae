// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.SwitchedDatePicker
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design", typeof (IDesigner))]
  public class SwitchedDatePicker : SwitchedControl
  {
    private IContainer components;
    private TextBox txtDate;
    private CalendarButton btnCalendar;

    public SwitchedDatePicker() => this.InitializeComponent();

    public override string Text
    {
      get => this.txtDate.Text;
      set => this.txtDate.Text = value;
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTime Value
    {
      get => Utils.ParseDate((object) this.Text, DateTime.MaxValue);
      set => this.Text = value.ToString("MM/dd/yyyy");
    }

    protected override void OnResize(EventArgs e)
    {
      if (this.txtDate != null && this.Height != this.txtDate.Height)
        this.Height = this.txtDate.Height;
      if (this.txtDate != null)
      {
        this.txtDate.Location = this.ControlRectangle.Location;
        this.txtDate.Size = new Size(Math.Min(100, Math.Max(0, this.ControlRectangle.Size.Width - this.btnCalendar.Width - 5)), this.ControlRectangle.Size.Height);
        this.btnCalendar.Left = this.txtDate.Right + 5;
        this.btnCalendar.Top = (this.ControlRectangle.Height - this.btnCalendar.Height) / 2;
      }
      base.OnResize(e);
    }

    protected override void OnSwitchClick(EventArgs e)
    {
      base.OnSwitchClick(e);
      if (!this.txtDate.Enabled)
        return;
      this.txtDate.Focus();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SwitchedDatePicker));
      this.txtDate = new TextBox();
      this.btnCalendar = new CalendarButton();
      ((ISupportInitialize) this.btnCalendar).BeginInit();
      this.SuspendLayout();
      this.txtDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDate.Location = new Point(22, 0);
      this.txtDate.MaxLength = 10;
      this.txtDate.Name = "txtDate";
      this.txtDate.Size = new Size(98, 20);
      this.txtDate.TabIndex = 2;
      this.btnCalendar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCalendar.DateControl = (Control) this.txtDate;
      ((IconButton) this.btnCalendar).Image = (Image) componentResourceManager.GetObject("btnCalendar.Image");
      this.btnCalendar.Location = new Point(123, 1);
      this.btnCalendar.MouseDownImage = (Image) null;
      this.btnCalendar.Name = "btnCalendar";
      this.btnCalendar.Size = new Size(16, 16);
      this.btnCalendar.SizeMode = PictureBoxSizeMode.AutoSize;
      this.btnCalendar.TabIndex = 3;
      this.btnCalendar.TabStop = false;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.btnCalendar);
      this.Controls.Add((Control) this.txtDate);
      this.Name = nameof (SwitchedDatePicker);
      this.Size = new Size(139, 20);
      this.Controls.SetChildIndex((Control) this.txtDate, 0);
      this.Controls.SetChildIndex((Control) this.btnCalendar, 0);
      ((ISupportInitialize) this.btnCalendar).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
