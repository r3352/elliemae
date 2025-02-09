// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Forms.ProgressReportDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Forms
{
  public class ProgressReportDialog : 
    Form,
    IProgressReportFeedback,
    ISynchronizeInvoke,
    IWin32Window,
    IServerProgressFeedback
  {
    private string Status_Processing = "Processing";
    private string Status_Success = "Sucsess";
    private string Status_Failed = "Fail";
    private string Status_Warning = "Warning";
    private AsynchronousProcessReport innerProcess;
    private object innerState;
    private bool cancel;
    private bool cancelEnabled = true;
    private DialogResult innerResult;
    private string[] listVWSourceData;
    private string[] detailList;
    private ListViewSortManager sortListViewMgr;
    private ListView listView1;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private Label lblCurrentRecord;
    private Button btnCancel;
    private ProgressBar statusProgressBar;
    private Font defaultFont;
    private Font processedFont;
    private Font imageSortingFont;
    private Label lblStatus;
    private Button btnClose;

    public ProgressReportDialog(
      string title,
      AsynchronousProcessReport process,
      object state,
      string[] displayList)
      : this(title, process, state, false, displayList)
    {
    }

    public ProgressReportDialog(
      string title,
      AsynchronousProcessReport process,
      object state,
      bool cancelEnabled,
      string[] displayList)
    {
      this.InitializeComponent();
      this.innerProcess = process;
      this.innerState = state;
      this.Text = title;
      this.cancelEnabled = cancelEnabled;
      this.listVWSourceData = displayList;
      if (!cancelEnabled)
        this.btnCancel.Visible = false;
      this.lblCurrentRecord.Text = "Initializing...";
      this.InitialListView();
    }

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void InitializeComponent()
    {
      this.statusProgressBar = new ProgressBar();
      this.lblStatus = new Label();
      this.listView1 = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.lblCurrentRecord = new Label();
      this.btnCancel = new Button();
      this.btnClose = new Button();
      this.SuspendLayout();
      this.statusProgressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.statusProgressBar.Location = new Point(4, 352);
      this.statusProgressBar.Name = "statusProgressBar";
      this.statusProgressBar.Size = new Size(600, 23);
      this.statusProgressBar.TabIndex = 0;
      this.lblStatus.Location = new Point(4, 332);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(144, 16);
      this.lblStatus.TabIndex = 1;
      this.lblStatus.Text = "Current processing record :";
      this.listView1.AllowColumnReorder = true;
      this.listView1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.listView1.Columns.AddRange(new ColumnHeader[2]
      {
        this.columnHeader1,
        this.columnHeader2
      });
      this.listView1.GridLines = true;
      this.listView1.Location = new Point(4, 4);
      this.listView1.Name = "listView1";
      this.listView1.Size = new Size(600, 324);
      this.listView1.TabIndex = 2;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = View.Details;
      this.columnHeader1.Text = "Record";
      this.columnHeader1.Width = 139;
      this.columnHeader2.Text = "Description";
      this.columnHeader2.Width = 545;
      this.lblCurrentRecord.Location = new Point(152, 332);
      this.lblCurrentRecord.Name = "lblCurrentRecord";
      this.lblCurrentRecord.Size = new Size(452, 16);
      this.lblCurrentRecord.TabIndex = 4;
      this.btnCancel.Location = new Point(528, 380);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClose.Location = new Point(528, 380);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 6;
      this.btnClose.Text = "Close";
      this.btnClose.Visible = false;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(608, 408);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblCurrentRecord);
      this.Controls.Add((Control) this.listView1);
      this.Controls.Add((Control) this.lblStatus);
      this.Controls.Add((Control) this.statusProgressBar);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ProgressReportDialog);
      this.ShowInTaskbar = false;
      this.Text = "(Title)";
      this.ResumeLayout(false);
    }

    private void MakeSuccessProcessed(ListViewItem item)
    {
      item.SubItems[0].Font = this.processedFont;
      item.SubItems[1].Font = this.processedFont;
      item.SubItems[1].ForeColor = Color.Green;
    }

    private void MakeFailProcessed(ListViewItem item)
    {
      item.SubItems[0].Font = this.processedFont;
      item.SubItems[1].Font = this.processedFont;
      item.SubItems[1].ForeColor = Color.Red;
    }

    private void MakeCurrentProcessing(ListViewItem item)
    {
      item.SubItems[0].Font = this.processedFont;
      item.SubItems[1].Font = this.processedFont;
      this.lblCurrentRecord.Text = item.SubItems[0].Text;
    }

    private void MakeToBeProcessed(ListViewItem item)
    {
      item.SubItems[0].ForeColor = Color.Black;
      item.SubItems[1].ForeColor = Color.Black;
    }

    private void MakeWarning(ListViewItem item)
    {
      item.SubItems[1].ForeColor = Color.Red;
      item.SubItems[0].Font = this.processedFont;
      item.SubItems[1].Font = this.processedFont;
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
        this.innerResult = this.innerProcess(this.innerState, (IProgressReportFeedback) this);
        if (this.innerResult != DialogResult.None)
          return;
        this.innerResult = DialogResult.Abort;
      }
      catch
      {
        this.innerResult = DialogResult.Abort;
      }
    }

    Form IProgressReportFeedback.ParentForm => (Form) this;

    public int MaxValue
    {
      get
      {
        lock (this)
          return this.statusProgressBar.Maximum;
      }
      set
      {
        if (this.InvokeRequired)
          this.Invoke((Delegate) new ProgressReportDialog.PropertySetter(this.Finish), (object) value);
        else
          this.Finish((object) value);
      }
    }

    public int Value
    {
      get
      {
        lock (this)
          return this.statusProgressBar.Value;
      }
      set
      {
        if (this.InvokeRequired)
          this.Invoke((Delegate) new ProgressReportDialog.PropertySetter(this.setValue), (object) value);
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

    public void UpdateStatus(string data)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressReportDialog.PropertySetter(this.UpdateListView), (object) data);
      else
        this.UpdateListView((object) data);
    }

    public string Status
    {
      get => "";
      set => this.UpdateStatus(value);
    }

    public string Details
    {
      get => "";
      set => this.detailList = value.Split('_');
    }

    public bool Increment(int count)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressReportDialog.PropertySetter(this.incrementProgress), (object) count);
      else
        this.incrementProgress((object) count);
      lock (this)
        return !this.cancel;
    }

    public bool ResetCounter(int maxValue)
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new ProgressReportDialog.PropertySetter(this.resetProgress), (object) maxValue);
      else
        this.resetProgress((object) maxValue);
      lock (this)
        return !this.cancel;
    }

    DialogResult IProgressReportFeedback.ShowDialog(System.Type formType, params object[] args)
    {
      if (this.InvokeRequired)
        return (DialogResult) this.Invoke((Delegate) new ProgressReportDialog.DialogInvoker(((IProgressReportFeedback) this).ShowDialog), (object) formType, (object) args);
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

    private void InitialListView()
    {
      if (this.listVWSourceData == null || this.listVWSourceData.Length == 0)
        return;
      this.sortListViewMgr = new ListViewSortManager(this.listView1, new System.Type[2]
      {
        typeof (ListViewTextCaseInsensitiveSort),
        typeof (ListViewTextCaseInsensitiveSort)
      });
      this.sortListViewMgr.Disable();
      this.listView1.BeginUpdate();
      foreach (string text in this.listVWSourceData)
      {
        ListViewItem listViewItem = new ListViewItem(text);
        listViewItem.UseItemStyleForSubItems = false;
        listViewItem.SubItems.Add("Waiting");
        this.MakeToBeProcessed(listViewItem);
        this.listView1.Items.Add(listViewItem);
        if (this.defaultFont == null)
        {
          this.defaultFont = listViewItem.Font;
          this.processedFont = new Font(this.defaultFont.FontFamily, this.defaultFont.Size, FontStyle.Bold);
          this.imageSortingFont = new Font(this.defaultFont.FontFamily, 0.1f);
        }
      }
      this.sortListViewMgr.Enable();
      this.listView1.EndUpdate();
    }

    private void setDetails(object value)
    {
      lock (this)
      {
        int num = this.cancel ? 1 : 0;
      }
    }

    private void setMaxValue(object value)
    {
      lock (this)
      {
        int num = (int) value;
        if (this.statusProgressBar.Value > num)
          this.statusProgressBar.Value = num;
        this.statusProgressBar.Maximum = num;
      }
    }

    private void setValue(object value)
    {
      lock (this)
      {
        int num = (int) value;
        if (num > this.statusProgressBar.Maximum)
          return;
        this.statusProgressBar.Value = num;
      }
    }

    private void Finish(object value)
    {
      lock (this)
      {
        this.btnClose.Visible = true;
        this.btnCancel.Visible = false;
        this.lblCurrentRecord.Text = "";
        this.lblStatus.Text = "Done";
        this.lblStatus.Font = this.processedFont;
        this.lblStatus.ForeColor = Color.Red;
      }
    }

    private void incrementProgress(object value)
    {
      lock (this)
      {
        this.statusProgressBar.Value = Math.Min(this.statusProgressBar.Value + (int) value, this.statusProgressBar.Maximum);
        if (this.statusProgressBar.Value != this.statusProgressBar.Maximum)
          return;
        this.btnCancel.Enabled = false;
        this.btnCancel.Visible = false;
        this.btnClose.Visible = true;
        this.btnClose.Enabled = true;
      }
    }

    private void resetProgress(object maxValue)
    {
      lock (this)
      {
        this.statusProgressBar.Value = 0;
        this.statusProgressBar.Maximum = (int) maxValue;
        if (this.statusProgressBar.Value >= this.statusProgressBar.Maximum || !this.cancelEnabled)
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
      }
      this.DialogResult = DialogResult.Cancel;
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

    private void UpdateListView(object statusObj)
    {
      string str = (string) statusObj;
      int index = int.Parse(this.detailList[0]);
      if (index >= this.listView1.Items.Count)
        return;
      ListViewItem listViewItem = this.listView1.Items[index];
      listViewItem.SubItems[1].Text = this.detailList[1];
      if (str == this.Status_Processing)
        this.MakeCurrentProcessing(listViewItem);
      else if (str == this.Status_Success)
        this.MakeSuccessProcessed(listViewItem);
      else if (str == this.Status_Failed)
      {
        this.MakeFailProcessed(listViewItem);
      }
      else
      {
        if (!(str == this.Status_Warning))
          return;
        this.MakeWarning(listViewItem);
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.BeginInvoke((Delegate) new MethodInvoker(this.setDialogResult));
    }

    private delegate void PropertySetter(object data);

    private delegate DialogResult DialogInvoker(System.Type formType, params object[] args);
  }
}
