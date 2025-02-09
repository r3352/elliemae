// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.SwitchedStateZip
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
  public class SwitchedStateZip : SwitchedControl
  {
    private TextBox txtState;
    private TextBox txtZipCode;
    private SwitchedTextBox swtCity;

    public SwitchedStateZip() => this.InitializeComponent();

    private void InitializeComponent()
    {
      this.txtZipCode = new TextBox();
      this.txtState = new TextBox();
      this.SuspendLayout();
      this.txtZipCode.Location = new Point(70, 0);
      this.txtZipCode.MaxLength = 10;
      this.txtZipCode.Name = "txtZipCode";
      this.txtZipCode.Size = new Size(95, 20);
      this.txtZipCode.TabIndex = 2;
      this.txtZipCode.KeyPress += new KeyPressEventHandler(this.txtZipCode_KeyPress);
      this.txtZipCode.Validating += new CancelEventHandler(this.txtZipCode_Validating);
      this.txtState.Location = new Point(21, 0);
      this.txtState.MaxLength = 2;
      this.txtState.Name = "txtState";
      this.txtState.Size = new Size(45, 20);
      this.txtState.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.Controls.Add((Control) this.txtState);
      this.Controls.Add((Control) this.txtZipCode);
      this.Name = nameof (SwitchedStateZip);
      this.Size = new Size(165, 20);
      this.Controls.SetChildIndex((Control) this.txtZipCode, 0);
      this.Controls.SetChildIndex((Control) this.txtState, 0);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    [Category("Data")]
    [DefaultValue("")]
    public string ZipCodeFieldID
    {
      get => string.Concat(this.txtZipCode.Tag);
      set => this.txtZipCode.Tag = (object) value;
    }

    [Category("Data")]
    [DefaultValue("")]
    public string StateFieldID
    {
      get => string.Concat(this.txtState.Tag);
      set => this.txtState.Tag = (object) value;
    }

    [Category("Data")]
    [DefaultValue(null)]
    public SwitchedTextBox CityTextControl
    {
      get => this.swtCity;
      set => this.swtCity = value;
    }

    [Browsable(false)]
    public string StateValue
    {
      get => this.txtState.Text ?? "";
      set => this.txtState.Text = value;
    }

    [Browsable(false)]
    public string ZipCodeValue
    {
      get => this.txtZipCode.Text;
      set => this.txtZipCode.Text = value;
    }

    protected override void OnResize(EventArgs e)
    {
      if (this.txtState != null)
      {
        if (this.Height != this.txtState.Height)
          this.Height = this.txtState.Height;
        this.txtState.Location = this.ControlRectangle.Location;
        this.txtZipCode.Left = this.txtState.Right + 10;
        this.txtZipCode.Top = (this.Height - this.txtZipCode.Height) / 2;
        this.txtZipCode.Width = Math.Min(95, Math.Max(0, this.ControlRectangle.Width - this.txtState.Width - 10));
      }
      base.OnResize(e);
    }

    protected override void OnSwitchClick(EventArgs e)
    {
      base.OnSwitchClick(e);
      if (!this.txtState.Enabled)
        return;
      this.txtState.Focus();
    }

    private void txtZipCode_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || char.IsNumber(e.KeyChar) || e.KeyChar == '-')
        return;
      e.Handled = true;
    }

    private void txtZipCode_Validating(object sender, CancelEventArgs e)
    {
      if (this.txtZipCode.Text.Length < 5)
        return;
      if (this.swtCity == null)
      {
        ZipCodeInfo zipInfoAt = ZipCodeUtils.GetZipInfoAt(this.txtZipCode.Text.Substring(0, 5));
        if (zipInfoAt == null)
          return;
        this.txtState.Text = zipInfoAt.State;
      }
      else
      {
        ZipCodeInfo infoWithUserDefined = ZipcodeSelector.GetZipCodeInfoWithUserDefined(this.txtZipCode.Text.Substring(0, 5));
        if (infoWithUserDefined == null)
          return;
        this.txtState.Text = infoWithUserDefined.State;
        this.swtCity.SwitchState = SwitchButtonState.On;
        this.swtCity.Text = infoWithUserDefined.City;
      }
    }
  }
}
