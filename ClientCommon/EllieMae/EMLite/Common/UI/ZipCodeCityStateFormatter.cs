// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ZipCodeCityStateFormatter
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ZipCodeCityStateFormatter : ZipCodeFormatter
  {
    private TextBox cityField;
    private Control stateField;

    public ZipCodeCityStateFormatter(TextBox textBox, TextBox cityField, Control stateField)
      : base(textBox)
    {
      this.cityField = cityField;
      this.stateField = stateField;
    }

    protected override void HandleValidated()
    {
      if (this.cityField == null && this.stateField == null)
        return;
      try
      {
        ZipCodeInfo infoWithUserDefined = ZipcodeSelector.GetZipCodeInfoWithUserDefined(this.CurrentText.Substring(0, 5));
        if (infoWithUserDefined == null)
          return;
        if (this.cityField != null)
          this.cityField.Text = infoWithUserDefined.City;
        if (this.stateField is TextBox)
        {
          this.stateField.Text = infoWithUserDefined.State;
        }
        else
        {
          if (!(this.stateField is ComboBox))
            return;
          ComboBox stateField = (ComboBox) this.stateField;
          for (int index = 0; index < stateField.Items.Count; ++index)
          {
            if (stateField.Items[index].ToString() == infoWithUserDefined.State)
            {
              stateField.SelectedIndex = index;
              break;
            }
          }
        }
      }
      catch
      {
      }
    }
  }
}
