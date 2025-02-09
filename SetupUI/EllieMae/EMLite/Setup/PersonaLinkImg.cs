// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaLinkImg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonaLinkImg : ImageLink
  {
    private bool enabled;
    private bool linked;

    public PersonaLinkImg(string text, bool linked, bool enabled)
      : base((Element) new FormattedText(text, EncompassFonts.Normal1.ForeColor), (Image) Resources.link_disabled)
    {
      this.enabled = enabled;
      if (!linked)
      {
        if (this.enabled)
          this.NormalImage = (Image) Resources.link_broken;
        else
          this.NormalImage = (Image) Resources.link_broken_disabled;
      }
      else if (this.enabled)
      {
        this.NormalImage = (Image) Resources.link;
        this.linked = true;
      }
      else
        this.NormalImage = (Image) Resources.link_broken_disabled;
    }

    public bool IsLinked => this.linked;

    public void Link()
    {
      if (!this.enabled)
        return;
      this.NormalImage = (Image) Resources.link;
      this.linked = true;
    }

    public void Disconnect()
    {
      if (this.enabled)
        this.NormalImage = (Image) Resources.link_broken;
      else
        this.NormalImage = (Image) Resources.link_broken_disabled;
      this.linked = false;
    }
  }
}
