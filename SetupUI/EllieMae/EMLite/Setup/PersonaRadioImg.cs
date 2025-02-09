// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaRadioImg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonaRadioImg : ImageLink
  {
    private string id = "";

    public PersonaRadioImg(bool enabled, bool checkedState, string ID, EventHandler eventHandler)
      : base((Element) new FormattedText("", EncompassFonts.Normal1.ForeColor), (Image) Resources.radio_button, eventHandler)
    {
      this.id = ID;
      if (enabled)
      {
        if (checkedState)
          this.NormalImage = (Image) Resources.radio_button;
        else
          this.NormalImage = (Image) Resources.radio_button_not_checked;
      }
      else if (checkedState)
        this.NormalImage = (Image) Resources.radio_button;
      else
        this.NormalImage = (Image) Resources.radio_button_not_checked;
    }

    public string UniqueID => this.id;
  }
}
