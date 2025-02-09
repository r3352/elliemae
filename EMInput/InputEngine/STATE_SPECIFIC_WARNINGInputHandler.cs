// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.STATE_SPECIFIC_WARNINGInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using mshtml;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class STATE_SPECIFIC_WARNINGInputHandler : InputHandlerBase
  {
    private EllieMae.Encompass.Forms.Panel panelNoState;
    private EllieMae.Encompass.Forms.Panel panelNoNeed;
    private EllieMae.Encompass.Forms.Label labelState;

    public STATE_SPECIFIC_WARNINGInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public STATE_SPECIFIC_WARNINGInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
      try
      {
        if (this.panelNoState == null)
          this.panelNoState = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl(nameof (panelNoState));
        if (this.panelNoNeed == null)
          this.panelNoNeed = (EllieMae.Encompass.Forms.Panel) this.currentForm.FindControl(nameof (panelNoNeed));
        if (this.labelState == null)
          this.labelState = (EllieMae.Encompass.Forms.Label) this.currentForm.FindControl(nameof (labelState));
        this.panelNoNeed.Top = this.panelNoState.Top;
        this.panelNoNeed.Left = this.panelNoState.Left;
        this.panelNoNeed.BorderColor = ColorTranslator.FromHtml("#FF7B7B");
        this.panelNoState.BorderColor = ColorTranslator.FromHtml("#FF7B7B");
        this.panelNoNeed.BackColor = ColorTranslator.FromHtml("#FFE2E2");
        this.panelNoState.BackColor = ColorTranslator.FromHtml("#FFE2E2");
      }
      catch (Exception ex)
      {
      }
      string field = this.loan.GetField("14");
      if (field == string.Empty)
      {
        this.panelNoNeed.Visible = false;
        this.panelNoState.Visible = true;
      }
      else
      {
        this.panelNoNeed.Visible = true;
        this.panelNoState.Visible = false;
        this.labelState.Text = Utils.GetFullStateName(field) == string.Empty ? "State " + field + "!" : Utils.GetFullStateName(field);
      }
    }

    public STATE_SPECIFIC_WARNINGInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public STATE_SPECIFIC_WARNINGInputHandler(
      Sessions.Session session,
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, dataTemplate, htmldoc, form, property)
    {
    }
  }
}
