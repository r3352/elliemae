// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.DisclosedHUDGFEDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using AxSHDocVw;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine.Forms;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class DisclosedHUDGFEDialog : Form, IOnlineHelpTarget, IHelp
  {
    private const string className = "DisclosedHUDGFEDialog";
    private static string sw = Tracing.SwInputEngine;
    private string formName = "2010 GFE";
    private DisclosureTrackingLog log;
    private System.Runtime.InteropServices.ComTypes.IConnectionPoint conPt;
    private int cookie;
    private IHtmlInput input;
    private static object nobj = (object) Missing.Value;
    private IContainer components;
    private AxWebBrowser axWebBrowser;
    private Panel panel1;
    private Button btnCancel;

    public DisclosedHUDGFEDialog(DisclosureTrackingLog log)
    {
      this.InitializeComponent();
      this.log = log;
      this.initialPage();
    }

    private void initialPage()
    {
      InputFormInfo formByName = new InputFormList(Session.SessionObjects).GetFormByName(this.formName);
      this.log.GetDisclosedFields(DisclosureTrackingLog.HUDGFEFIELDS);
      this.input = (IHtmlInput) new DisclosedHUDGFEHandler(this.log, LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.AllLoanDataFields));
      this.hookUpBrowserHandler();
      this.axWebBrowser.Navigate(FormStore.GetFormHTMLPath(Session.DefaultInstance, formByName), ref DisclosedHUDGFEDialog.nobj, ref DisclosedHUDGFEDialog.nobj, ref DisclosedHUDGFEDialog.nobj, ref DisclosedHUDGFEDialog.nobj);
    }

    protected void hookUpBrowserHandler()
    {
      System.Runtime.InteropServices.ComTypes.IConnectionPointContainer ocx = (System.Runtime.InteropServices.ComTypes.IConnectionPointContainer) this.axWebBrowser.GetOcx();
      Guid guid = typeof (SHDocVw.DWebBrowserEvents2).GUID;
      ocx.FindConnectionPoint(ref guid, out this.conPt);
      FrmBrowserHandler pUnkSink = new FrmBrowserHandler(Session.DefaultInstance, (IWin32Window) this, this.input);
      this.conPt.Advise((object) pUnkSink, out this.cookie);
      pUnkSink.SetHelpTarget((IOnlineHelpTarget) this);
    }

    public string GetHelpTargetName() => "";

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DisclosedHUDGFEDialog));
      this.axWebBrowser = new AxWebBrowser();
      this.panel1 = new Panel();
      this.btnCancel = new Button();
      this.axWebBrowser.BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.axWebBrowser.Dock = DockStyle.Fill;
      this.axWebBrowser.Enabled = true;
      this.axWebBrowser.Location = new Point(0, 0);
      this.axWebBrowser.OcxState = (AxHost.State) componentResourceManager.GetObject("axWebBrowser.OcxState");
      this.axWebBrowser.Size = new Size(705, 433);
      this.axWebBrowser.TabIndex = 5;
      this.panel1.Controls.Add((Control) this.axWebBrowser);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(705, 433);
      this.panel1.TabIndex = 6;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(618, 442);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(705, 474);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DisclosedHUDGFEDialog);
      this.Text = "Disclosed Snapshot";
      this.axWebBrowser.EndInit();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
