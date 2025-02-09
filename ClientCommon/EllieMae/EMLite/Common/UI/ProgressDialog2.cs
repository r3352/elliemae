// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ProgressDialog2
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
  public class ProgressDialog2 : 
    Form,
    IProgressFeedback2,
    ISynchronizeInvoke,
    IWin32Window,
    IServerProgressFeedback2
  {
    private AsynchronousProcess2 innerProcess;
    private object innerState;
    private bool cancel;
    private bool cancelEnabled = true;
    private DialogResult innerResult;
    private ProgressBar[] pbProgressArray;
    private Label[] lblStatusArray;
    private Label[] lblDetailsArray;
    private int numPBs = 1;
    private IContainer components;
    private Button btnCancel;
    private ProgressBar pbProgress0;
    private Label lblStatus0;
    private Label lblDetails0;

    public int NumberOfThreads => this.numPBs;

    public ProgressDialog2(string title, AsynchronousProcess2 process, int numPBs = 1)
      : this(title, process, (object) null, numPBs)
    {
    }

    public ProgressDialog2(string title, AsynchronousProcess2 process, object state, int numPBs = 1)
      : this(title, process, state, false, numPBs)
    {
    }

    public ProgressDialog2(
      string title,
      AsynchronousProcess2 process,
      object state,
      bool cancelEnabled,
      int numPBs = 1)
    {
      if (this.numPBs < 1)
        throw new Exception("Invalid value for parameter numPBs of ProgressDialogs");
      this.InitializeComponent();
      this.pbProgress0.Visible = this.lblStatus0.Visible = this.lblDetails0.Visible = false;
      this.numPBs = numPBs;
      this.pbProgressArray = new ProgressBar[this.numPBs];
      this.lblStatusArray = new Label[this.numPBs];
      this.lblDetailsArray = new Label[this.numPBs];
      for (int index = this.numPBs - 1; index >= 0; --index)
      {
        this.pbProgressArray[index] = new ProgressBar();
        this.lblDetailsArray[index] = new Label();
        this.lblStatusArray[index] = new Label();
        this.lblDetailsArray[index].Location = new Point(26, 81 + 100 * index);
        this.lblDetailsArray[index].Name = "lblDetails" + (object) (index + 1);
        this.lblDetailsArray[index].Size = new Size(348, 18);
        this.lblDetailsArray[index].Text = "(Details)";
        this.pbProgressArray[index].Location = new Point(24, 59 + 100 * index);
        this.pbProgressArray[index].Name = "pbProgress" + (object) (index + 1);
        this.pbProgressArray[index].Size = new Size(350, 17);
        this.lblStatusArray[index].Location = new Point(26, 4 + 100 * index);
        this.lblStatusArray[index].Name = "lblStatus" + (object) (index + 1);
        this.lblStatusArray[index].Size = new Size(348, 48);
        this.lblStatusArray[index].Text = "(Status)";
        this.lblStatusArray[index].TextAlign = ContentAlignment.BottomLeft;
        this.Controls.Add((Control) this.lblDetailsArray[index]);
        this.Controls.Add((Control) this.pbProgressArray[index]);
        this.Controls.Add((Control) this.lblStatusArray[index]);
      }
      this.innerProcess = process;
      this.innerState = state;
      this.Text = title;
      this.cancelEnabled = cancelEnabled;
      if (!cancelEnabled)
        this.btnCancel.Visible = false;
      for (int pbIdx = 0; pbIdx < this.numPBs; ++pbIdx)
        this.SetStatus(pbIdx, "Initializing...");
      for (int pbIdx = 0; pbIdx < this.numPBs; ++pbIdx)
        this.SetDetails(pbIdx, "");
      this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + 100 * (this.numPBs - 1));
      this.btnCancel.Location = new Point(this.btnCancel.Location.X, this.btnCancel.Location.Y + 100 * (this.numPBs - 1));
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
        this.innerResult = this.innerProcess(this.innerState, (IProgressFeedback2) this);
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

    Form IProgressFeedback2.ParentForm => (Form) this;

    public int GetMaxValue(int pbIdx)
    {
      lock (this)
        return this.pbProgressArray[pbIdx].Maximum;
    }

    public void SetMaxValue(int pbIdx, int value)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressDialog2.PropertySetter(this.setMaxValue), (object) pbIdx, (object) value);
      else
        this.setMaxValue(pbIdx, (object) value);
    }

    public int GetValue(int pbIdx)
    {
      lock (this)
        return this.pbProgressArray[pbIdx].Value;
    }

    public void SetValue(int pbIdx, int value)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressDialog2.PropertySetter(this.setValue), (object) pbIdx, (object) value);
      else
        this.setValue(pbIdx, (object) value);
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

    public string GetStatus(int pbIdx)
    {
      lock (this)
        return this.lblStatusArray[pbIdx].Text;
    }

    public void SetStatus(int pbIdx, string value)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressDialog2.PropertySetter(this.setStatus), (object) pbIdx, (object) value);
      else
        this.setStatus(pbIdx, (object) value);
    }

    public string GetDetails(int pbIdx)
    {
      lock (this)
        return this.lblDetails0.Text;
    }

    public void SetDetails(int pbIdx, string value)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressDialog2.PropertySetter(this.setDetails), (object) pbIdx, (object) value);
      else
        this.setDetails(pbIdx, (object) value);
    }

    public bool Increment(int pbIdx, int count)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressDialog2.PropertySetter(this.incrementProgress), (object) pbIdx, (object) count);
      else
        this.incrementProgress(pbIdx, (object) count);
      lock (this)
        return !this.cancel;
    }

    public bool ResetCounter(int pbIdx, int maxValue)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressDialog2.PropertySetter(this.resetProgress), (object) pbIdx, (object) maxValue);
      else
        this.resetProgress(pbIdx, (object) maxValue);
      lock (this)
        return !this.cancel;
    }

    DialogResult IProgressFeedback2.ShowDialog(System.Type formType, params object[] args)
    {
      if (this.InvokeRequired)
        return (DialogResult) this.Invoke((Delegate) new ProgressDialog2.DialogInvoker(((IProgressFeedback) this).ShowDialog), (object) formType, (object) args);
      using (Form form = (Form) formType.InvokeMember("", BindingFlags.CreateInstance, (Binder) null, (object) null, args))
        return form.ShowDialog((IWin32Window) this);
    }

    public bool SetFeedback(int pbIdx, string status, string details, int value)
    {
      if (status != null)
        this.SetStatus(pbIdx, status);
      if (details != null)
        this.SetDetails(pbIdx, details);
      if (value >= 0)
        this.SetValue(pbIdx, value);
      lock (this)
        return !this.cancel;
    }

    public DialogResult MsgBox(string text, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
      if (!this.InvokeRequired)
        return Utils.Dialog((IWin32Window) this, text, buttons, icon);
      return (DialogResult) this.Invoke((Delegate) new ProgressDialog2.MsgBoxInvoker(this.MsgBox), (object) text, (object) buttons, (object) icon);
    }

    private void setStatus(int pbIdx, object value)
    {
      lock (this)
        this.lblStatusArray[pbIdx].Text = value.ToString();
    }

    private void setDetails(int pbIdx, object value)
    {
      lock (this)
      {
        if (this.cancel)
          return;
        this.lblDetailsArray[pbIdx].Text = this.fitStringToLabel(value.ToString(), this.lblDetailsArray[pbIdx]);
      }
    }

    private void setMaxValue(int pbIdx, object value)
    {
      lock (this)
      {
        int num = (int) value;
        if (this.pbProgressArray[pbIdx].Value > num)
          this.pbProgressArray[pbIdx].Value = num;
        this.pbProgressArray[pbIdx].Maximum = num;
      }
    }

    private void setValue(int pbIdx, object value)
    {
      lock (this)
      {
        int num = (int) value;
        if (num > this.pbProgressArray[pbIdx].Maximum)
          return;
        this.pbProgressArray[pbIdx].Value = num;
      }
    }

    private void incrementProgress(int pbIdx, object value)
    {
      lock (this)
      {
        int num = (int) value;
        this.pbProgressArray[pbIdx].Value = Math.Min(this.pbProgressArray[pbIdx].Value + num, this.pbProgressArray[pbIdx].Maximum);
        if (this.pbProgressArray[pbIdx].Value != this.pbProgressArray[pbIdx].Maximum)
          return;
        this.btnCancel.Enabled = false;
      }
    }

    private void resetProgress(int pbIdx, object maxValue)
    {
      lock (this)
      {
        this.pbProgressArray[pbIdx].Value = 0;
        this.pbProgressArray[pbIdx].Maximum = (int) maxValue;
        if (this.pbProgressArray[pbIdx].Value >= this.pbProgressArray[pbIdx].Maximum || !this.cancelEnabled)
          return;
        this.btnCancel.Enabled = true;
      }
    }

    private void setDialogResult() => this.DialogResult = this.innerResult;

    private void btnCancel_Click(object sender, EventArgs e)
    {
      lock (this)
      {
        for (int pbIdx = 0; pbIdx < this.numPBs; ++pbIdx)
        {
          this.SetStatus(pbIdx, "Cancelled. Please wait...");
          this.SetDetails(pbIdx, "");
        }
        this.cancel = true;
        this.btnCancel.Enabled = false;
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
      this.pbProgress0 = new ProgressBar();
      this.lblStatus0 = new Label();
      this.btnCancel = new Button();
      this.lblDetails0 = new Label();
      this.SuspendLayout();
      this.pbProgress0.Location = new Point(24, 59);
      this.pbProgress0.Name = "pbProgress0";
      this.pbProgress0.Size = new Size(350, 17);
      this.pbProgress0.TabIndex = 0;
      this.lblStatus0.Location = new Point(26, 4);
      this.lblStatus0.Name = "lblStatus0";
      this.lblStatus0.Size = new Size(348, 48);
      this.lblStatus0.TabIndex = 1;
      this.lblStatus0.Text = "(Status)";
      this.lblStatus0.TextAlign = ContentAlignment.BottomLeft;
      this.btnCancel.Location = new Point(300, 104);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblDetails0.Location = new Point(26, 81);
      this.lblDetails0.Name = "lblDetails0";
      this.lblDetails0.Size = new Size(348, 18);
      this.lblDetails0.TabIndex = 3;
      this.lblDetails0.Text = "(Details)";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(402, 134);
      this.ControlBox = false;
      this.Controls.Add((Control) this.lblDetails0);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblStatus0);
      this.Controls.Add((Control) this.pbProgress0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = nameof (ProgressDialog2);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "(Title)";
      this.ResumeLayout(false);
    }

    private delegate void PropertySetter(int pbIdx, object value);

    private delegate DialogResult DialogInvoker(System.Type formType, params object[] args);

    private delegate DialogResult MsgBoxInvoker(
      string text,
      MessageBoxButtons buttons,
      MessageBoxIcon icon);
  }
}
