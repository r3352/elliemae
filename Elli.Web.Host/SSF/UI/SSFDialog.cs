// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.UI.SSFDialog
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.SSF.Context;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.SSF.UI
{
  public class SSFDialog : Form
  {
    private const string className = "SSFDialog";
    private static readonly string sw = Tracing.SwThinThick;
    private SSFContext context;
    private IContainer components;
    private SSFControl ssfControl;

    public SSFDialog(SSFContext context)
    {
      this.InitializeComponent();
      this.context = context;
    }

    public void SetScaledWindowSize(Form parent, double widthFactor, double heightFactor)
    {
      if (parent != null)
      {
        this.Width = Convert.ToInt32((double) parent.Width * widthFactor);
        this.Height = Convert.ToInt32((double) parent.Height * heightFactor);
      }
      else
      {
        this.Width = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Width * widthFactor);
        this.Height = Convert.ToInt32((double) Screen.PrimaryScreen.WorkingArea.Height * heightFactor);
      }
    }

    public bool Show() => this.Show((IWin32Window) null);

    public bool Show(IWin32Window owner)
    {
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Calling SSFControl.LoadApp");
      bool flag = this.ssfControl.LoadApp(this.context);
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Checking SSFControl.LoadApp Result: " + flag.ToString());
      if (!flag)
      {
        int num = (int) Utils.Dialog(owner, "Unable to launch the workflow because the application could not be loaded.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Setting SSFContext.UnloadHandler");
      this.context.unloadHandler = new Action(this.unloadHandler);
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Checking LoanDataMgr");
      if (this.context.loanDataMgr != null)
      {
        Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Subscribing to LoanDataMgr.LoanClosing");
        this.context.loanDataMgr.LoanClosing += new EventHandler(this.LoanDataMgr_LoanClosing);
      }
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Showing Window");
      if (owner != null)
        base.Show(owner);
      else
        base.Show();
      return true;
    }

    public bool ShowDialog() => this.ShowDialog((IWin32Window) null);

    public bool ShowDialog(IWin32Window owner)
    {
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Checking LoanDataMgr.Dirty");
      LoanDataMgr loanDataMgr = this.context.loanDataMgr;
      if ((loanDataMgr != null ? (loanDataMgr.Dirty ? 1 : 0) : 0) != 0)
      {
        bool flag = false;
        Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Calling LoanDataMgr.Save");
        if (this.context.loanDataMgr.IsLoanLocked())
          flag = this.context.loanDataMgr.Save();
        Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Checking LoanDataMgr.Save Result: " + flag.ToString());
        if (!flag)
        {
          int num = (int) Utils.Dialog(owner, "Unable to launch the workflow because the loan could not be saved.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Calling SSFControl.LoadApp");
      bool flag1 = this.ssfControl.LoadApp(this.context);
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Checking SSFControl.LoadApp Result: " + flag1.ToString());
      if (!flag1)
      {
        int num = (int) Utils.Dialog(owner, "Unable to launch the workflow because the application could not be loaded.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Setting SSFContext.UnloadHandler");
      this.context.unloadHandler = new Action(this.unloadHandler);
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Showing Dialog");
      if (owner != null)
      {
        int num1 = (int) base.ShowDialog(owner);
      }
      else
      {
        int num2 = (int) base.ShowDialog();
      }
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Checking LoanDataMgr");
      if (this.context.loanDataMgr != null)
      {
        Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Calling LoanDataMgr.refreshLoanFromServer");
        this.context.loanDataMgr.refreshLoanFromServer();
      }
      return true;
    }

    private void unloadHandler()
    {
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Closing Dialog: UnloadHandler");
      if (this.InvokeRequired)
        this.Invoke((Delegate) (() => this.DialogResult = DialogResult.OK));
      else
        this.Close();
    }

    private void LoanDataMgr_LoanClosing(object sender, EventArgs e)
    {
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Closing Dialog: LoanClosing Event");
      this.Close();
    }

    private void SSFDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      bool flag = false;
      try
      {
        SSFEventArgs<bool[]> eventArgs = new SSFEventArgs<bool[]>("module", "unloading");
        Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Calling SSFControl.RaiseEvent: " + eventArgs.ToString());
        if (this.ssfControl.RaiseEvent<bool[]>(eventArgs))
          flag = Array.Exists<bool>(eventArgs.EventFeedback, (Predicate<bool>) (x => !x));
      }
      catch (Exception ex)
      {
        Tracing.Log(SSFDialog.sw, nameof (SSFDialog), TraceLevel.Error, "SSFControl.RaiseEvent Failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when raising the 'module.closing' event:\n\n" + ex.ToString());
      }
      if (flag)
      {
        e.Cancel = true;
      }
      else
      {
        if (this.OwnedForms.Length == 0)
          return;
        List<Form> formList = new List<Form>((IEnumerable<Form>) this.OwnedForms);
        foreach (Form form in formList)
        {
          Tracing.Log(SSFDialog.sw, nameof (SSFDialog), TraceLevel.Error, "Closing OwnedForm: " + form.Text);
          form.Close();
          Tracing.Log(SSFDialog.sw, nameof (SSFDialog), TraceLevel.Error, "Disposing OwnedForm");
          form.Dispose();
        }
        formList.Clear();
      }
    }

    private void SSFDialog_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.context.loanDataMgr == null)
        return;
      Tracing.Log(SSFDialog.sw, TraceLevel.Verbose, nameof (SSFDialog), "Unsubscribing to LoanDataMgr.LoanClosing");
      this.context.loanDataMgr.LoanClosing -= new EventHandler(this.LoanDataMgr_LoanClosing);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.ssfControl = new SSFControl();
      this.SuspendLayout();
      this.ssfControl.Dock = DockStyle.Fill;
      this.ssfControl.Location = new Point(0, 0);
      this.ssfControl.Margin = new Padding(6, 6, 6, 6);
      this.ssfControl.Name = "ssfControl";
      this.ssfControl.Size = new Size(537, 318);
      this.ssfControl.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(11f, 22f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(537, 318);
      this.Controls.Add((Control) this.ssfControl);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SSFDialog);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Scripting Framework";
      this.FormClosing += new FormClosingEventHandler(this.SSFDialog_FormClosing);
      this.ResumeLayout(false);
    }
  }
}
