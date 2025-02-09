// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ProgressDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ProgressDialog : 
    Form,
    IProgressFeedback,
    ISynchronizeInvoke,
    IWin32Window,
    IServerProgressFeedback
  {
    private AsynchronousProcess innerProcess;
    private object innerState;
    private bool cancel;
    private bool cancelEnabled = true;
    private DialogResult innerResult;
    private ProgressBar pbProgress;
    private Label lblStatus;
    private Button btnCancel;
    private Label lblDetails;
    private System.ComponentModel.Container components;

    public ProgressDialog(string title, AsynchronousProcess process)
      : this(title, process, (object) null)
    {
    }

    public ProgressDialog(string title, AsynchronousProcess process, object state)
      : this(title, process, state, false)
    {
    }

    public ProgressDialog(
      string title,
      AsynchronousProcess process,
      object state,
      bool cancelEnabled)
    {
      this.InitializeComponent();
      this.innerProcess = process;
      this.innerState = state;
      this.Text = title;
      this.cancelEnabled = cancelEnabled;
      if (!cancelEnabled)
        this.btnCancel.Visible = false;
      this.Status = "Initializing...";
      this.Details = "";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        try
        {
          RemotingServices.Disconnect((MarshalByRefObject) this);
        }
        catch
        {
        }
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pbProgress = new ProgressBar();
      this.lblStatus = new Label();
      this.btnCancel = new Button();
      this.lblDetails = new Label();
      this.SuspendLayout();
      this.pbProgress.Location = new Point(24, 59);
      this.pbProgress.Name = "pbProgress";
      this.pbProgress.Size = new Size(350, 17);
      this.pbProgress.TabIndex = 0;
      this.lblStatus.Location = new Point(26, 4);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(348, 48);
      this.lblStatus.TabIndex = 1;
      this.lblStatus.Text = "(Status)";
      this.lblStatus.TextAlign = ContentAlignment.BottomLeft;
      this.btnCancel.Location = new Point(300, 104);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblDetails.Location = new Point(26, 81);
      this.lblDetails.Name = "lblDetails";
      this.lblDetails.Size = new Size(348, 18);
      this.lblDetails.TabIndex = 3;
      this.lblDetails.Text = "(Details)";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(402, 133);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblDetails);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblStatus);
      this.Controls.Add((Control) this.pbProgress);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (ProgressDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "(Title)";
      this.ResumeLayout(false);
    }

    public DialogResult RunWait(IWin32Window owner) => this.ShowDialog(owner);

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      Thread thread = new Thread(new ThreadStart(this.runProcess));
      thread.SetApartmentState(ApartmentState.STA);
      thread.Start();
    }

    private void runProcess()
    {
      try
      {
        this.innerResult = this.innerProcess(this.innerState, (IProgressFeedback) this);
        if (this.innerResult != DialogResult.None)
          return;
        this.innerResult = DialogResult.Abort;
      }
      catch
      {
        this.innerResult = DialogResult.Abort;
      }
      finally
      {
        this.BeginInvoke((Delegate) new MethodInvoker(this.setDialogResult));
      }
    }

    Form IProgressFeedback.ParentForm => (Form) this;

    public int MaxValue
    {
      get
      {
        lock (this)
          return this.pbProgress.Maximum;
      }
      set
      {
        if (this.InvokeRequired)
          this.Invoke((Delegate) new ProgressDialog.PropertySetter(this.setMaxValue), (object) value);
        else
          this.setMaxValue((object) value);
      }
    }

    public int Value
    {
      get
      {
        lock (this)
          return this.pbProgress.Value;
      }
      set
      {
        if (this.InvokeRequired)
          this.Invoke((Delegate) new ProgressDialog.PropertySetter(this.setValue), (object) value);
        else
          this.setValue((object) value);
      }
    }

    public bool Cancel
    {
      get
      {
        lock (this)
          return this.cancel;
      }
      set
      {
        lock (this)
          this.cancel = value;
      }
    }

    public string Status
    {
      get
      {
        lock (this)
          return this.lblStatus.Text;
      }
      set
      {
        if (this.InvokeRequired)
          this.Invoke((Delegate) new ProgressDialog.PropertySetter(this.setStatus), (object) value);
        else
          this.setStatus((object) value);
      }
    }

    public string Details
    {
      get
      {
        lock (this)
          return this.lblDetails.Text;
      }
      set
      {
        if (this.InvokeRequired)
          this.Invoke((Delegate) new ProgressDialog.PropertySetter(this.setDetails), (object) value);
        else
          this.setDetails((object) value);
      }
    }

    public bool Increment(int count)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressDialog.PropertySetter(this.incrementProgress), (object) count);
      else
        this.incrementProgress((object) count);
      lock (this)
        return !this.cancel;
    }

    public bool ResetCounter(int maxValue)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressDialog.PropertySetter(this.resetProgress), (object) maxValue);
      else
        this.resetProgress((object) maxValue);
      lock (this)
        return !this.cancel;
    }

    DialogResult IProgressFeedback.ShowDialog(System.Type formType, params object[] args)
    {
      if (this.InvokeRequired)
        return (DialogResult) this.Invoke((Delegate) new ProgressDialog.DialogInvoker(((IProgressFeedback) this).ShowDialog), (object) formType, (object) args);
      using (Form form = (Form) formType.InvokeMember("", BindingFlags.CreateInstance, (Binder) null, (object) null, args))
        return form.ShowDialog((IWin32Window) this);
    }

    public bool SetFeedback(string status, string details, int value)
    {
      if (status != null)
        this.Status = status;
      if (details != null)
        this.Details = details;
      if (value >= 0)
        this.Value = value;
      lock (this)
        return !this.cancel;
    }

    public DialogResult MsgBox(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
      if (!this.InvokeRequired)
        return Utils.Dialog((IWin32Window) this, text, buttons, icon);
      return (DialogResult) this.Invoke((Delegate) new ProgressDialog.MsgBoxInvoker(this.MsgBox), (object) text, (object) buttons, (object) icon);
    }

    private void setStatus(object value)
    {
      lock (this)
        this.lblStatus.Text = value.ToString();
    }

    private void setDetails(object value)
    {
      lock (this)
      {
        if (this.cancel)
          return;
        this.lblDetails.Text = this.fitStringToLabel(value.ToString(), this.lblDetails);
      }
    }

    private void setMaxValue(object value)
    {
      lock (this)
      {
        int num = (int) value;
        if (this.pbProgress.Value > num)
          this.pbProgress.Value = num;
        this.pbProgress.Maximum = num;
      }
    }

    private void setValue(object value)
    {
      lock (this)
      {
        int num = (int) value;
        if (num > this.pbProgress.Maximum)
          return;
        this.pbProgress.Value = num;
      }
    }

    private void incrementProgress(object value)
    {
      lock (this)
      {
        this.pbProgress.Value = Math.Min(this.pbProgress.Value + (int) value, this.pbProgress.Maximum);
        if (this.pbProgress.Value != this.pbProgress.Maximum)
          return;
        this.btnCancel.Enabled = false;
      }
    }

    private void resetProgress(object maxValue)
    {
      lock (this)
      {
        this.pbProgress.Value = 0;
        this.pbProgress.Maximum = (int) maxValue;
        if (this.pbProgress.Value >= this.pbProgress.Maximum || !this.cancelEnabled)
          return;
        this.btnCancel.Enabled = true;
      }
    }

    private void setDialogResult() => this.DialogResult = this.innerResult;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      lock (this)
      {
        this.Status = "Cancelled. Please wait...";
        this.Details = "";
        this.cancel = true;
        this.btnCancel.Enabled = false;
        this.Close();
      }
    }

    private string fitStringToLabel(string text, Label label)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        if ((double) graphics.MeasureString(text, label.Font).Width < (double) label.Width)
          return text;
        for (; text.Length > 0; text = text.Substring(1))
        {
          if ((double) graphics.MeasureString(text + "...", label.Font).Width <= (double) label.Width)
            break;
        }
      }
      return text + "...";
    }

    public override object InitializeLifetimeService() => (object) null;

    private delegate void PropertySetter(object value);

    private delegate DialogResult DialogInvoker(System.Type formType, params object[] args);

    private delegate DialogResult MsgBoxInvoker(
      string text,
      MessageBoxButtons buttons,
      MessageBoxIcon icon);
  }
}
