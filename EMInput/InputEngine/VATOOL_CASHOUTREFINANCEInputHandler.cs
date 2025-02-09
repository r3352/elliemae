// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VATOOL_CASHOUTREFINANCEInputHandler
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Forms;
using mshtml;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class VATOOL_CASHOUTREFINANCEInputHandler : InputHandlerBase
  {
    private QuickEntryPopupDialog quickVolPage;
    private EllieMae.Encompass.Forms.Button btnShowVols;
    private mshtml.IHTMLEventObj clickedpEvtObj;

    public VATOOL_CASHOUTREFINANCEInputHandler(
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, mainScreen, htmldoc, form, property)
    {
    }

    public VATOOL_CASHOUTREFINANCEInputHandler(
      Sessions.Session session,
      IMainScreen mainScreen,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : base(session, mainScreen, htmldoc, form, property)
    {
    }

    public VATOOL_CASHOUTREFINANCEInputHandler(
      IWin32Window owner,
      LoanData dataTemplate,
      HTMLDocument htmldoc,
      EllieMae.Encompass.Forms.Form form,
      object property)
      : this(Session.DefaultInstance, owner, dataTemplate, htmldoc, form, property)
    {
    }

    public VATOOL_CASHOUTREFINANCEInputHandler(
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
      base.CreateControls();
      try
      {
        this.btnShowVols = (EllieMae.Encompass.Forms.Button) this.currentForm.FindControl("btnShowVols");
      }
      catch (Exception ex)
      {
      }
    }

    public override void RefreshContents(bool skipButtonFieldLockRules)
    {
      base.RefreshContents(skipButtonFieldLockRules);
      this.btnShowVols.Enabled = this.quickVolPage == null;
    }

    public override bool onclick(mshtml.IHTMLEventObj pEvtObj)
    {
      this.clickedpEvtObj = pEvtObj;
      return base.onclick(pEvtObj);
    }

    public override void Dispose()
    {
      if (this.quickVolPage != null)
        this.quickVolPage.Close();
      base.Dispose();
    }

    public override void ExecAction(string action)
    {
      base.ExecAction(action);
      if (!(action == "volnonmodal"))
        return;
      this.showAllVOLDetails();
    }

    private void showAllVOLDetails()
    {
      if (this.quickVolPage != null)
      {
        this.quickVolPage.BringToFront();
        this.quickVolPage.Focus();
      }
      else
      {
        this.btnShowVols.Enabled = false;
        this.quickVolPage = new QuickEntryPopupDialog(this.inputData, "VOL", new InputFormInfo("VOLPanel", "VOLPanel"), 900, 512, FieldSource.CurrentLoan, "VOL", this.session);
        this.quickVolPage.Text = "Quick Entry - VOL";
        this.quickVolPage.FormBorderStyle = FormBorderStyle.Sizable;
        this.quickVolPage.ShowIcon = false;
        this.quickVolPage.MaximizeBox = false;
        this.quickVolPage.StartPosition = FormStartPosition.Manual;
        this.quickVolPage.Location = new Point(this.clickedpEvtObj.screenX, this.clickedpEvtObj.screenY + 18);
        this.quickVolPage.FormClosing += new FormClosingEventHandler(this.quickVolPage_FormClosing);
        this.quickVolPage.OkClicked += new EventHandler(this.quickVolPage_OkClicked);
        this.quickVolPage.OnFieldChanged += new EventHandler(this.quickVolPage_OnFieldChanged);
        this.quickVolPage.Disposed += new EventHandler(this.quickVolPage_Disposed);
        this.quickVolPage.ButtonClicked += new EventHandler(this.quickVolPage_OnButtonClicked);
        this.quickVolPage.Location = new Point(this.clickedpEvtObj.screenX + 10, (SystemInformation.VirtualScreen.Height - this.quickVolPage.Height) / 2);
        this.quickVolPage.Show((IWin32Window) this.session.MainForm);
        this.quickVolPage.BringToFront();
      }
    }

    private void quickVolPage_OkClicked(object sender, EventArgs e)
    {
      if (this.quickVolPage == null)
        return;
      this.quickVolPage.Close();
      if (this.quickVolPage != null)
        this.quickVolPage.Dispose();
      this.quickVolPage = (QuickEntryPopupDialog) null;
      this.RefreshContents();
    }

    private void quickVolPage_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.RefreshContents();
    }

    private void quickVolPage_Disposed(object sender, EventArgs e)
    {
      this.RefreshContents();
      this.quickVolPage = (QuickEntryPopupDialog) null;
      this.btnShowVols.Enabled = true;
    }

    private void quickVolPage_OnFieldChanged(object sender, EventArgs e)
    {
      this.RefreshContents(true);
    }

    private void quickVolPage_OnButtonClicked(object sender, EventArgs e) => this.RefreshContents();

    public void CloseAllPopupWindows()
    {
      this.quickVolPage_OkClicked((object) null, (EventArgs) null);
    }
  }
}
