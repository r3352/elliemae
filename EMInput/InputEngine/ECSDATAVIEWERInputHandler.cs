// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ECSDATAVIEWERInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ECSDATAVIEWERInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.TextBox boxSYSX2;
    private EllieMae.Encompass.Forms.TextBox box3152;
    private EllieMae.Encompass.Forms.TextBox box3153;
    private EllieMae.Encompass.Forms.TextBox box3154;
    private EllieMae.Encompass.Forms.TextBox box3155;

    public ECSDATAVIEWERInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public ECSDATAVIEWERInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public ECSDATAVIEWERInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public ECSDATAVIEWERInputHandler(
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
        if (this.boxSYSX2 == null)
        {
          this.boxSYSX2 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("boxSYSX2");
          this.boxSYSX2.Alignment = TextAlignment.Right;
        }
        bool flag = Utils.CheckIf2015RespaTila(this.inputData.GetField("3969"));
        this.box3152 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_3152");
        if (this.box3152 != null & flag)
          this.box3152.HelpKey = "3152-1";
        this.box3153 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_3153");
        if (this.box3153 != null & flag)
          this.box3153.HelpKey = "3153-1";
        this.box3154 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_3154");
        if (this.box3154 != null & flag)
          this.box3154.HelpKey = "3154-1";
        this.box3155 = (EllieMae.Encompass.Forms.TextBox) this.currentForm.FindControl("l_3155");
        if (!(this.box3155 != null & flag))
          return;
        this.box3155.HelpKey = "3155-1";
      }
      catch (Exception ex)
      {
      }
    }

    internal override ControlState GetControlState(RuntimeControl ctrl, string id)
    {
      ControlState controlState = this.getControlState(ctrl, id, ControlState.Enabled);
      switch (id)
      {
        case "hmda":
        case "feedetails":
        case "mipusda":
          return controlState;
        case "3626":
          controlState = ControlState.Default;
          goto case "hmda";
        default:
          controlState = ControlState.Disabled;
          goto case "hmda";
      }
    }
  }
}
