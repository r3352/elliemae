// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.NotePhrasesForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class NotePhrasesForm : Form
  {
    private System.ComponentModel.Container components;
    private ListBox listBoxPhrases;
    private Button btnNew;
    private Button btnDelete;
    private Label label1;
    private Button btnCancel;
    private Button btnEdit;
    private Button button1;
    private static NotePhrasesForm instance;

    public static NotePhrasesForm Instance
    {
      get
      {
        if (NotePhrasesForm.instance == null)
          NotePhrasesForm.instance = new NotePhrasesForm();
        return NotePhrasesForm.instance;
      }
    }

    private NotePhrasesForm()
    {
      this.InitializeComponent();
      this.Init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnNew = new Button();
      this.listBoxPhrases = new ListBox();
      this.btnDelete = new Button();
      this.label1 = new Label();
      this.btnCancel = new Button();
      this.btnEdit = new Button();
      this.button1 = new Button();
      this.SuspendLayout();
      this.btnNew.Location = new Point(444, 88);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(52, 23);
      this.btnNew.TabIndex = 2;
      this.btnNew.Text = "&New";
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.listBoxPhrases.Location = new Point(4, 48);
      this.listBoxPhrases.Name = "listBoxPhrases";
      this.listBoxPhrases.Size = new Size(432, 160);
      this.listBoxPhrases.Sorted = true;
      this.listBoxPhrases.TabIndex = 1;
      this.listBoxPhrases.DoubleClick += new EventHandler(this.listBoxPhrases_DoubleClick);
      this.btnDelete.Location = new Point(444, 116);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(52, 23);
      this.btnDelete.TabIndex = 3;
      this.btnDelete.Text = "&Delete";
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.label1.Location = new Point(4, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(428, 36);
      this.label1.TabIndex = 0;
      this.label1.Text = "Double click a phrase to select it into your note. Or highlight a phrase and click Select to add it to your note.";
      this.btnCancel.Location = new Point(444, 172);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(52, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Close";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnEdit.Location = new Point(444, 144);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(52, 23);
      this.btnEdit.TabIndex = 5;
      this.btnEdit.Text = "&Edit";
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.button1.Location = new Point(444, 52);
      this.button1.Name = "button1";
      this.button1.Size = new Size(52, 23);
      this.button1.TabIndex = 6;
      this.button1.Text = "&Select";
      this.button1.Click += new EventHandler(this.button1_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(506, 219);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.btnEdit);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.listBoxPhrases);
      this.Controls.Add((Control) this.btnNew);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NotePhrasesForm);
      this.ShowInTaskbar = false;
      this.Text = "Phrases";
      this.Closed += new EventHandler(this.NotePhrasesForm_Closed);
      this.KeyUp += new KeyEventHandler(this.NotePhrasesForm_KeyUp);
      this.ResumeLayout(false);
    }

    private void NotePhrasesForm_Closed(object sender, EventArgs e)
    {
      NotePhrasesForm.instance = (NotePhrasesForm) null;
    }

    private void Init() => this.RefreshPhraseList();

    private void RefreshPhraseList()
    {
      this.listBoxPhrases.Items.Clear();
      foreach (object notePhrase in Session.ConfigurationManager.GetNotePhrases())
        this.listBoxPhrases.Items.Add(notePhrase);
    }

    private void DeletePhrase(string phrase)
    {
      Session.ConfigurationManager.RemoveNotePhrase(phrase);
      this.RefreshPhraseList();
    }

    private void AddNewPhrase(string newPhrase)
    {
      Session.ConfigurationManager.AddNotePhrase(newPhrase);
      this.RefreshPhraseList();
    }

    public event NotePhrasesForm.PhraseSelectedEventHandler PhraseSelected;

    protected virtual void OnPhraseSelected(PhraseSelectedEventArg e) => this.PhraseSelected(e);

    private void listBoxPhrases_DoubleClick(object sender, EventArgs e)
    {
      if (this.listBoxPhrases.SelectedItem == null)
        return;
      string str = this.listBoxPhrases.SelectedItem.ToString();
      str.TrimEnd();
      this.OnPhraseSelected(new PhraseSelectedEventArg(str + " "));
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      NewPhraseForm newPhraseForm = new NewPhraseForm();
      newPhraseForm.WindowTitle = "New Phrase";
      newPhraseForm.Description = "Please enter your new phrase.";
      newPhraseForm.TopMost = true;
      if (newPhraseForm.ShowDialog() == DialogResult.Cancel)
        return;
      this.AddNewPhrase(newPhraseForm.NewPhrase);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      object selectedItem = this.listBoxPhrases.SelectedItem;
      if (selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a phrase in the list box to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (DialogResult.Cancel == MessageBox.Show("Are you sure that you want to delete the phrase " + selectedItem + "?", "Delete Phrase", MessageBoxButtons.OKCancel))
          return;
        this.DeletePhrase(selectedItem.ToString());
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnEdit_Click(object sender, EventArgs e)
    {
      object selectedItem = this.listBoxPhrases.SelectedItem;
      if (selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a phrase in the list box to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        NewPhraseForm newPhraseForm = new NewPhraseForm(selectedItem.ToString());
        newPhraseForm.WindowTitle = "Edit Phrase";
        newPhraseForm.Description = "Please enter your new phrase.";
        newPhraseForm.TopMost = true;
        if (newPhraseForm.ShowDialog() == DialogResult.Cancel)
          return;
        this.DeletePhrase(selectedItem.ToString());
        this.AddNewPhrase(newPhraseForm.NewPhrase);
      }
    }

    private void button1_Click(object sender, EventArgs e)
    {
      this.listBoxPhrases_DoubleClick((object) null, (EventArgs) null);
    }

    private void NotePhrasesForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    public delegate void PhraseSelectedEventHandler(PhraseSelectedEventArg e);
  }
}
