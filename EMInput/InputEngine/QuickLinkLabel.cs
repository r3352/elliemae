// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.QuickLinkLabel
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class QuickLinkLabel : Label
  {
    private QuickLink quickLink;
    private bool current;

    public QuickLinkLabel()
    {
      this.Font = EncompassFonts.Normal3.Font;
      this.AutoSize = true;
      this.Margin = new Padding(3, 3, 3, 0);
      this.setCurrent(this.current);
    }

    public QuickLinkLabel(QuickLink link)
      : this()
    {
      this.quickLink = link;
      this.Text = link.Text;
    }

    public QuickLink QuickLink => this.quickLink;

    public bool Current
    {
      get => this.current;
      set
      {
        this.current = value;
        this.setCurrent(value);
      }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
      base.OnMouseEnter(e);
      if (this.current)
        return;
      this.Font = new Font(this.Font, FontStyle.Underline);
    }

    protected override void OnMouseLeave(EventArgs e)
    {
      base.OnMouseLeave(e);
      if (this.current)
        return;
      this.Font = new Font(this.Font, FontStyle.Regular);
    }

    private void setCurrent(bool value)
    {
      if (value)
      {
        this.ForeColor = SystemColors.ControlText;
        this.Cursor = Cursors.Default;
      }
      else
      {
        this.ForeColor = EncompassFonts.Normal3.ForeColor;
        this.Cursor = Cursors.Hand;
      }
      this.Font = new Font(this.Font, FontStyle.Regular);
    }
  }
}
