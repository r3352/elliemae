// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.UI.SSFConsole
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.SSF.UI
{
  internal class SSFConsole : Form
  {
    private const string className = "SSFConsole";
    private static readonly string sw = Tracing.SwThinThick;
    private static List<SSFConsole> _consoleList = new List<SSFConsole>();
    private SSFControl owner;
    private IContainer components;
    private RichTextBox rtbConsole;
    private BorderPanel pnlConsole;
    private Button btnClose;

    public static void ShowConsole(SSFControl owner)
    {
      Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Checking Console Windows");
      SSFConsole ssfConsole1 = SSFConsole._consoleList.FirstOrDefault<SSFConsole>((Func<SSFConsole, bool>) (c => c.owner == owner));
      if (ssfConsole1 == null)
      {
        Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Creating New Console Window");
        SSFConsole ssfConsole2 = new SSFConsole(owner);
        Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Showing New Console Window");
        ssfConsole2.Show((IWin32Window) owner.ParentForm);
      }
      else
      {
        Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Checking Console WindowState");
        if (ssfConsole1.WindowState == FormWindowState.Minimized)
          ssfConsole1.WindowState = FormWindowState.Normal;
        Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Activating Console Window");
        ssfConsole1.Activate();
      }
    }

    private SSFConsole(SSFControl owner)
    {
      this.InitializeComponent();
      Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Storing Console Owner");
      this.owner = owner;
      Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Subscribing to 'ConsoleMessage' Event");
      this.owner.ConsoleMessage += new SSFControl.ConsoleMessageEventHandler(this.owner_ConsoleMessage);
      Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Registering Console Window");
      SSFConsole._consoleList.Add(this);
      foreach (ConsoleMessageEventArgs consoleMessage in this.owner.ConsoleMessages)
        this.AppendMessage(consoleMessage);
    }

    private void owner_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
    {
      this.AppendMessage(e);
      this.rtbConsole.ScrollToCaret();
    }

    private void AppendMessage(ConsoleMessageEventArgs message)
    {
      if (string.IsNullOrWhiteSpace(message.Message))
        return;
      string str = message.Message;
      if (str.StartsWith("%c"))
        str = str.Substring(2);
      if (str.Contains(" color: "))
        str = str.Substring(0, str.LastIndexOf(" color: "));
      int textLength = this.rtbConsole.TextLength;
      this.rtbConsole.AppendText(str + Environment.NewLine);
      if (message.Level != MessageLevel.Error && message.Level != MessageLevel.Warning)
        return;
      this.rtbConsole.SelectionStart = textLength;
      this.rtbConsole.SelectionLength = str.Length;
      switch (message.Level)
      {
        case MessageLevel.Warning:
          this.rtbConsole.SelectionColor = Color.DarkOrange;
          break;
        case MessageLevel.Error:
          this.rtbConsole.SelectionColor = Color.Red;
          break;
      }
      this.rtbConsole.SelectionStart = this.rtbConsole.TextLength;
    }

    private void SSFConsole_FormClosed(object sender, FormClosedEventArgs e)
    {
      Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Unregistering Console Window");
      SSFConsole._consoleList.Remove(this);
      Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Unsubscribing from 'ConsoleMessage' Event");
      this.owner.ConsoleMessage -= new SSFControl.ConsoleMessageEventHandler(this.owner_ConsoleMessage);
      Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Releasing Console Owner");
      this.owner = (SSFControl) null;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      Tracing.Log(SSFConsole.sw, TraceLevel.Verbose, nameof (SSFConsole), "Closing Console Window");
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rtbConsole = new RichTextBox();
      this.pnlConsole = new BorderPanel();
      this.btnClose = new Button();
      this.pnlConsole.SuspendLayout();
      this.SuspendLayout();
      this.rtbConsole.BackColor = SystemColors.Window;
      this.rtbConsole.BorderStyle = BorderStyle.None;
      this.rtbConsole.Dock = DockStyle.Fill;
      this.rtbConsole.Font = new Font("Courier New", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rtbConsole.Location = new Point(1, 1);
      this.rtbConsole.Name = "rtbConsole";
      this.rtbConsole.ReadOnly = true;
      this.rtbConsole.ScrollBars = RichTextBoxScrollBars.Vertical;
      this.rtbConsole.Size = new Size(774, 426);
      this.rtbConsole.TabIndex = 0;
      this.rtbConsole.Text = "";
      this.pnlConsole.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlConsole.Controls.Add((Control) this.rtbConsole);
      this.pnlConsole.Location = new Point(12, 12);
      this.pnlConsole.Name = "pnlConsole";
      this.pnlConsole.Size = new Size(776, 428);
      this.pnlConsole.TabIndex = 2;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.Location = new Point(714, 448);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 3;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(798, 480);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.pnlConsole);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SSFConsole);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Chromium Console";
      this.FormClosed += new FormClosedEventHandler(this.SSFConsole_FormClosed);
      this.pnlConsole.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
