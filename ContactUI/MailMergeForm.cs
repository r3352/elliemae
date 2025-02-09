// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.MailMergeForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.CustomLetters;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class MailMergeForm : Form
  {
    private System.ComponentModel.Container components;
    private static string className = nameof (MailMergeForm);
    private static string sw = Tracing.SwContact;
    private ContactLetterPanel cl;
    private FileSystemEntry _LetterFile;
    private string _Action = "";
    private bool _IsPrintPreview;

    private void LogInfo(string msg)
    {
      Tracing.Log(MailMergeForm.sw, TraceLevel.Info, MailMergeForm.className, msg);
    }

    public MailMergeForm(bool bEmailMerge, ContactType contactType, bool docMgmtOnly)
    {
      this.LogInfo("Enter MailMergeForm constructor.");
      this.InitializeComponent();
      this.LogInfo("UI componenets are initialized.");
      if (bEmailMerge)
      {
        this.LogInfo("Parameter bEmailMerge is true.");
        this.Text = "Email Merge";
      }
      else
        this.LogInfo("Parameter bEmailMerge is false.");
      this.cl = new ContactLetterPanel(Session.DefaultInstance, (IMainScreen) null, bEmailMerge, contactType, docMgmtOnly);
      this.cl.LetterSelected += new ContactLetterPanel.LetterSelectedEventHandler(this.OnLetterSelected);
      this.cl.Dock = DockStyle.Fill;
      this.Controls.Add((Control) this.cl);
      this.LogInfo("Leave MailMergeForm constructor.");
    }

    public void OnLetterSelected(LetterSelectedEventArg e)
    {
      this._LetterFile = e.LetterFile;
      this._IsPrintPreview = e.IsPrintPreview;
      this._Action = e.Action;
      this.Close();
    }

    public FileSystemEntry LetterFile
    {
      get => this._LetterFile;
      set => this._LetterFile = value;
    }

    public string Action
    {
      get => this._Action;
      set => this._Action = value;
    }

    public bool IsPrintPreview
    {
      get => this._IsPrintPreview;
      set => this._IsPrintPreview = value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(574, 539);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MailMergeForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Mail Merge";
      this.KeyUp += new KeyEventHandler(this.MailMergeForm_KeyUp);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
    }

    private void MailMergeForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.cl.PerformCancel();
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp("Mail Merge");
    }
  }
}
