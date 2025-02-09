// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CustomLetters.NewLetter
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.CustomLetters
{
  public class NewLetter : Form
  {
    private Label label1;
    private System.ComponentModel.Container components;
    private Button btnOK;
    private Button btnCancel;
    private Label originalLbl;
    private TextBox descTxt;
    private CustomLetterType letterType;
    private FileSystemEntry originalLetter;
    private FileSystemEntry newLetterEntry;
    private const string BADCHARS = "/:*?<>|.";

    public NewLetter(CustomLetterType letterType, FileSystemEntry originalLetter)
    {
      this.letterType = letterType;
      this.originalLetter = originalLetter;
      this.InitializeComponent();
      this.originalLbl.Text = "Original: " + (object) originalLetter;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.descTxt = new TextBox();
      this.btnOK = new Button();
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.originalLbl = new Label();
      this.SuspendLayout();
      this.descTxt.Location = new Point(8, 57);
      this.descTxt.MaxLength = 254;
      this.descTxt.Name = "descTxt";
      this.descTxt.Size = new Size(346, 20);
      this.descTxt.TabIndex = 0;
      this.descTxt.Text = "";
      this.descTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.btnOK.Location = new Point(105, 91);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(55, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "New Title:";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(185, 91);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.originalLbl.Location = new Point(8, 12);
      this.originalLbl.Name = "originalLbl";
      this.originalLbl.Size = new Size(344, 16);
      this.originalLbl.TabIndex = 7;
      this.originalLbl.Text = "Original:";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(364, 126);
      this.Controls.Add((Control) this.originalLbl);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NewLetter);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Import Custom Letter";
      this.ResumeLayout(false);
    }

    internal FileSystemEntry LetterEntry => this.newLetterEntry;

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (char.IsControl(e.KeyChar) || e.KeyChar != '\\')
        return;
      e.Handled = true;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      string str = this.descTxt.Text.Trim();
      if (str == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Form title cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string extension1 = Path.GetExtension(this.originalLetter.GetEncodedPath());
        string extension2 = Path.GetExtension(FileSystem.EncodeFilename(str, false));
        if (string.Compare(extension1, extension2, true) != 0)
          str += extension1;
        FileSystemEntry entry = new FileSystemEntry(this.originalLetter.ParentFolder.Path, str, FileSystemEntry.Types.File, this.originalLetter.Owner);
        if (Session.ConfigurationManager.CustomLetterObjectExistsOfAnyType(this.letterType, entry))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The custom letter title that you entered is already in use. Please try a different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.descTxt.Focus();
        }
        else
        {
          this.newLetterEntry = entry;
          this.DialogResult = DialogResult.OK;
        }
      }
    }
  }
}
