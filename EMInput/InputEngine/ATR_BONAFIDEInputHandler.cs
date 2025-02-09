// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ATR_BONAFIDEInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ATR_BONAFIDEInputHandler : InputHandlerBase
  {
    public ATR_BONAFIDEInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_BONAFIDEInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ATR_BONAFIDEInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ATR_BONAFIDEInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }

    internal override void CreateControls()
    {
      try
      {
        EllieMae.Encompass.Forms.Panel control1 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel_X1139Yes");
        EllieMae.Encompass.Forms.Panel control2 = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl("panel_X1139No");
        control2.Left = control1.Left;
        control2.Top = control1.Top;
        if (this.inputData.GetField("NEWHUD.X1139") == "Y")
        {
          control1.Visible = true;
          control2.Visible = false;
        }
        else
        {
          control1.Visible = false;
          control2.Visible = true;
        }
      }
      catch (Exception ex)
      {
      }
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(action))
      {
        case 719179447:
          if (!(action == "calculateovertimeincome"))
            break;
          this.SetFieldFocus("l_X138");
          break;
        case 1278929227:
          if (!(action == "baseincome"))
            break;
          this.SetFieldFocus("l_X137");
          break;
        case 1289470777:
          if (!(action == "calculatemilitaryallowenceincome"))
            break;
          this.SetFieldFocus("l_X180");
          break;
        case 1928564127:
          if (!(action == "calculatemilitaryincome"))
            break;
          this.SetFieldFocus("l_X163");
          break;
        case 2552332442:
          if (!(action == "vol"))
            break;
          this.SetFieldFocus("l_FL0102");
          break;
        case 2907831719:
          if (!(action == "calculatedividendincome"))
            break;
          this.SetFieldFocus("l_X141");
          break;
        case 3000050112:
          if (!(action == "calculateotherincome"))
            break;
          this.SetFieldFocus("l_X142");
          break;
      }
    }
  }
}
