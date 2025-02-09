// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.SharedURLAUIHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.Encompass.Forms;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class SharedURLAUIHandler
  {
    public static List<RuntimeControl> GetControls(
      Form currentForm,
      string ctrlID2009,
      string ctrlID2020)
    {
      RuntimeControl runtimeControl1 = (RuntimeControl) null;
      RuntimeControl runtimeControl2 = (RuntimeControl) null;
      try
      {
        runtimeControl1 = (RuntimeControl) currentForm.FindControl(ctrlID2009);
        runtimeControl2 = (RuntimeControl) currentForm.FindControl(ctrlID2020);
      }
      catch (Exception ex)
      {
      }
      runtimeControl2.Top = runtimeControl1.Top;
      runtimeControl2.Left = runtimeControl1.Left;
      return new List<RuntimeControl>()
      {
        runtimeControl1,
        runtimeControl2
      };
    }

    public static void DisplayControls(bool use2020URLA, List<RuntimeControl> controls)
    {
      if (controls == null || controls.Count < 2)
        return;
      if (controls[0] != null)
        controls[0].Visible = !use2020URLA;
      if (controls[1] == null)
        return;
      controls[1].Visible = use2020URLA;
    }
  }
}
