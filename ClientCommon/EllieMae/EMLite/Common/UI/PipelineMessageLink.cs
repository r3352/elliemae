// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.PipelineMessageLink
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class PipelineMessageLink : AlertMessageLink
  {
    private Control parentControl;
    private PipelineInfo pinfo;

    public PipelineMessageLink(Control parentControl, PipelineElementData data)
      : base(AlertMessageLabel.AlertMessageStyle.Message, PipelineMessageLink.getDisplayValue(data))
    {
      this.parentControl = parentControl;
      this.pinfo = data.PipelineInfo;
      this.Click += new EventHandler(this.onLinkClick);
    }

    private void onLinkClick(object sender, EventArgs e)
    {
      using (EPassMessageDialog epassMessageDialog = new EPassMessageDialog(this.pinfo.GUID))
      {
        int num = (int) epassMessageDialog.ShowDialog((IWin32Window) this.parentControl);
      }
    }

    public override Rectangle Draw(ItemDrawArgs e)
    {
      return this.Text == "" ? Rectangle.Empty : base.Draw(e);
    }

    public override Size Measure(ItemDrawArgs drawArgs)
    {
      return this.Text == "" ? Size.Empty : Resources.alert.Size;
    }

    private static string getDisplayValue(PipelineElementData data)
    {
      int num = Utils.ParseInt(data.GetValue(), 0);
      return num == 0 ? "" : num.ToString();
    }
  }
}
