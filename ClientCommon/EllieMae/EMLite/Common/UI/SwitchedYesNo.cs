// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.SwitchedYesNo
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design", typeof (IDesigner))]
  public class SwitchedYesNo : SwitchedRadioButtons
  {
    private RadioButton radYes;
    private RadioButton radNo;

    public SwitchedYesNo()
    {
      this.radYes = new RadioButton();
      this.radYes.Text = "Yes";
      this.RadioButtons.Add(this.radYes);
      this.radNo = new RadioButton();
      this.radNo.Text = "No";
      this.RadioButtons.Add(this.radNo);
    }

    public override string Text
    {
      get => !this.radYes.Checked ? "N" : "Y";
      set
      {
        this.radYes.Checked = value == "Y";
        this.radNo.Checked = !this.radYes.Checked;
      }
    }

    protected override void OnSelectedButtonChange(EventArgs e)
    {
      base.OnSelectedButtonChange(e);
      this.OnTextChanged(EventArgs.Empty);
    }
  }
}
