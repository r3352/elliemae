// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DatePickerCustom
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DatePickerCustom : DatePicker
  {
    private Color bgColor;

    [Browsable(true)]
    [Category("Appearance")]
    public override Color BackColor
    {
      get => this.bgColor;
      set => this.bgColor = value;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
      if (!this.Enabled)
        this.bgColor = SystemColors.Control;
      using (Brush brush = (Brush) new SolidBrush(this.bgColor))
        pevent.Graphics.FillRectangle(brush, this.ClientRectangle);
      this.txtDate.BackColor = this.bgColor;
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.BackColor = Color.FromArgb((int) byte.MaxValue, 224, 192);
      this.Name = nameof (DatePickerCustom);
      this.Size = new Size(145, 21);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    protected override void OnTextChanged(EventArgs e)
    {
      base.OnTextChanged(e);
      this.OnValueChanged();
    }
  }
}
