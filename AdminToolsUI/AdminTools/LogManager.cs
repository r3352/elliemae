// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LogManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LogManager : Form
  {
    private Panel panel1;
    private Label label1;
    private ComboBox cboTraceLevel;
    private Button btnClear;
    private Button btnStart;
    private System.ComponentModel.Container components;
    private Button button1;
    private SaveFileDialog dlgSave;
    private RichTextBox txtLog;
    private bool captureEnabled;

    public LogManager()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
      this.txtLog.MaxLength = int.MaxValue;
      this.cboTraceLevel.Items.Clear();
      this.cboTraceLevel.Items.Add((object) TraceLevel.Off.ToString());
      this.cboTraceLevel.Items.Add((object) TraceLevel.Error.ToString());
      this.cboTraceLevel.Items.Add((object) TraceLevel.Warning.ToString());
      this.cboTraceLevel.Items.Add((object) TraceLevel.Info.ToString());
      this.cboTraceLevel.Items.Add((object) TraceLevel.Verbose.ToString());
      this.cboTraceLevel.SelectedIndex = 2;
      Session.Connection.ServerEvent += new ServerEventHandler(this.onServerEvent);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        Session.Connection.ServerEvent -= new ServerEventHandler(this.onServerEvent);
        if (this.components != null)
          this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LogManager));
      this.panel1 = new Panel();
      this.button1 = new Button();
      this.btnStart = new Button();
      this.btnClear = new Button();
      this.cboTraceLevel = new ComboBox();
      this.label1 = new Label();
      this.dlgSave = new SaveFileDialog();
      this.txtLog = new RichTextBox();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.button1);
      this.panel1.Controls.Add((Control) this.btnStart);
      this.panel1.Controls.Add((Control) this.btnClear);
      this.panel1.Controls.Add((Control) this.cboTraceLevel);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(512, 50);
      this.panel1.TabIndex = 2;
      this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.button1.Location = new Point(428, 14);
      this.button1.Name = "button1";
      this.button1.Size = new Size(75, 23);
      this.button1.TabIndex = 9;
      this.button1.Text = "Sa&ve";
      this.button1.Click += new EventHandler(this.button1_Click);
      this.btnStart.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnStart.Location = new Point(268, 14);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new Size(75, 23);
      this.btnStart.TabIndex = 8;
      this.btnStart.Text = "&Start";
      this.btnStart.Click += new EventHandler(this.btnStart_Click);
      this.btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClear.Location = new Point(348, 14);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(75, 23);
      this.btnClear.TabIndex = 7;
      this.btnClear.Text = "&Clear";
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.cboTraceLevel.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTraceLevel.Location = new Point(125, 14);
      this.cboTraceLevel.Name = "cboTraceLevel";
      this.cboTraceLevel.Size = new Size(121, 22);
      this.cboTraceLevel.TabIndex = 6;
      this.cboTraceLevel.SelectedIndexChanged += new EventHandler(this.cboTraceLevel_SelectedIndexChanged);
      this.label1.Location = new Point(11, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(110, 21);
      this.label1.TabIndex = 5;
      this.label1.Text = "Log filter level:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.dlgSave.DefaultExt = "rtf";
      this.dlgSave.FileName = "ServerLog.rtf";
      this.dlgSave.Filter = "Rich Text Files|*.rtf|Text Files|*.txt";
      this.txtLog.DetectUrls = false;
      this.txtLog.Dock = DockStyle.Fill;
      this.txtLog.Location = new Point(0, 50);
      this.txtLog.Name = "txtLog";
      this.txtLog.Size = new Size(512, 304);
      this.txtLog.TabIndex = 4;
      this.txtLog.Text = "";
      this.txtLog.KeyPress += new KeyPressEventHandler(this.txtLog_KeyPress);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(512, 354);
      this.Controls.Add((Control) this.txtLog);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (LogManager);
      this.Text = "Log Viewer";
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void cboTraceLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.captureEnabled)
        return;
      Session.Connection.Session.RegisterForTracing((TraceLevel) Enum.Parse(typeof (TraceLevel), this.cboTraceLevel.SelectedItem.ToString(), true));
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (!this.captureEnabled)
      {
        Session.Connection.Session.RegisterForTracing((TraceLevel) Enum.Parse(typeof (TraceLevel), this.cboTraceLevel.SelectedItem.ToString(), true));
        this.captureEnabled = true;
        this.btnStart.Text = "&Stop";
        this.appendToLog("Logging started at " + (object) Session.Connection.Session.ServerTime, Color.Black);
      }
      else
      {
        Session.Connection.Session.UnregisterForTracing();
        this.captureEnabled = false;
        this.btnStart.Text = "&Start";
        this.appendToLog("Logging stopped at " + (object) Session.Connection.Session.ServerTime, Color.Black);
      }
    }

    private void onServerEvent(IConnection conn, ServerEvent e)
    {
      if (!(e is TraceEvent))
        return;
      this.appendToLog(e.ToString(), this.traceLevelToColor(((TraceEvent) e).TraceLevel));
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Clearing the log will cause all capture data to be lost.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
        return;
      lock (this)
        this.txtLog.Clear();
    }

    private void appendToLog(string text, Color color)
    {
      lock (this)
      {
        this.txtLog.SelectionStart = this.txtLog.TextLength;
        this.txtLog.SelectionColor = color;
        this.txtLog.AppendText(text + "\r\n");
        this.txtLog.ScrollToCaret();
      }
    }

    private void txtLog_KeyPress(object sender, KeyPressEventArgs e) => e.Handled = true;

    private void button1_Click(object sender, EventArgs e)
    {
      if (this.dlgSave.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      string fileName = this.dlgSave.FileName;
      try
      {
        FileStream data = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read);
        if (Path.GetExtension(fileName).ToLower() == "rtf")
          this.txtLog.SaveFile((Stream) data, RichTextBoxStreamType.RichNoOleObjs);
        else
          this.txtLog.SaveFile((Stream) data, RichTextBoxStreamType.PlainText);
        data.Close();
        int num = (int) Utils.Dialog((IWin32Window) this, "Log written successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error writing to log file: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    private Color traceLevelToColor(TraceLevel level)
    {
      switch (level)
      {
        case TraceLevel.Error:
          return Color.Red;
        case TraceLevel.Warning:
          return Color.Orange;
        case TraceLevel.Info:
          return Color.Black;
        default:
          return Color.FromArgb(170, 170, 170);
      }
    }
  }
}
