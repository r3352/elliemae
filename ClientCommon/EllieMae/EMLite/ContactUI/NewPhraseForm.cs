// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.NewPhraseForm
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class NewPhraseForm : Form
  {
    private string newPhrase = string.Empty;
    private ArrayList existingNames;
    private bool ignoreCase;
    private Button btnCancel;
    private Button btnOK;
    private TextBox txtBoxPhrase;
    private Label lblDescription;
    private System.ComponentModel.Container components;

    public string WindowTitle
    {
      get => this.Text;
      set => this.Text = value;
    }

    public string Description
    {
      get => this.lblDescription.Text;
      set => this.lblDescription.Text = value;
    }

    public string NewPhrase
    {
      get => this.newPhrase;
      set => this.newPhrase = value;
    }

    public int MaxPhaseLength
    {
      get => this.txtBoxPhrase.MaxLength;
      set
      {
        if (0 > value)
          return;
        this.txtBoxPhrase.MaxLength = value;
      }
    }

    public bool IgnoreCase
    {
      get => this.ignoreCase;
      set => this.ignoreCase = value;
    }

    public NewPhraseForm()
      : this(string.Empty, (ArrayList) null)
    {
    }

    public NewPhraseForm(string phrase)
      : this(phrase, (ArrayList) null)
    {
    }

    public NewPhraseForm(string phrase, ArrayList existingNames)
    {
      this.InitializeComponent();
      this.txtBoxPhrase.Text = phrase;
      this.existingNames = existingNames;
    }

    public void SelectText()
    {
      this.txtBoxPhrase.Focus();
      this.txtBoxPhrase.SelectAll();
    }

    private bool containsDuplicate(string phrase)
    {
      if (this.existingNames != null)
      {
        foreach (object existingName in this.existingNames)
        {
          if (string.Compare(phrase.Trim(), existingName.ToString(), this.IgnoreCase) == 0)
            return true;
        }
      }
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (string.Empty == this.txtBoxPhrase.Text.Trim())
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The value cannot be blank. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.containsDuplicate(this.txtBoxPhrase.Text))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The name you entered already exists. Please type in another name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.txtBoxPhrase.Focus();
        this.txtBoxPhrase.SelectAll();
      }
      else
      {
        this.newPhrase = this.txtBoxPhrase.Text;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.txtBoxPhrase = new TextBox();
      this.lblDescription = new Label();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(260, 80);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.btnOK.Location = new Point(176, 80);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.txtBoxPhrase.Location = new Point(12, 52);
      this.txtBoxPhrase.Name = "txtBoxPhrase";
      this.txtBoxPhrase.Size = new Size(372, 20);
      this.txtBoxPhrase.TabIndex = 1;
      this.txtBoxPhrase.Text = "";
      this.lblDescription.Location = new Point(12, 8);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(376, 40);
      this.lblDescription.TabIndex = 0;
      this.lblDescription.Text = "Set Description Here";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(394, 111);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtBoxPhrase);
      this.Controls.Add((Control) this.lblDescription);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NewPhraseForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Set Title here";
      this.ResumeLayout(false);
    }
  }
}
