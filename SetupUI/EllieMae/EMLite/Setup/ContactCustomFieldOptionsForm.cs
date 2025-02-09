// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ContactCustomFieldOptionsForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ContactCustomFieldOptionsForm : Form
  {
    private bool isDirty;
    private const int MAX_OPTION_LENGTH = 50;
    private Button btnCancel;
    private string[] _fieldOptions;
    private bool btnCloseClicked;
    private System.ComponentModel.Container components;
    private ListBox listBoxFieldOptions;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnClose;
    private Button btnUp;
    private Button btnDown;
    private Button btnNew;

    public string[] FieldOptions
    {
      get
      {
        this._fieldOptions = new string[this.listBoxFieldOptions.Items.Count];
        for (int index = 0; index < this.listBoxFieldOptions.Items.Count; ++index)
          this._fieldOptions[index] = (string) this.listBoxFieldOptions.Items[index];
        return this._fieldOptions;
      }
    }

    public bool IsDirty => this.isDirty;

    public ContactCustomFieldOptionsForm(string[] listItems)
    {
      this.InitializeComponent();
      this.listBoxFieldOptions.Items.Clear();
      if (listItems != null)
        this.listBoxFieldOptions.Items.AddRange((object[]) listItems);
      this.listBoxFieldOptions_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private bool containsDuplicate(string newPhase)
    {
      foreach (object obj in this.listBoxFieldOptions.Items)
      {
        if (string.Compare(newPhase, obj.ToString(), true) == 0)
          return true;
      }
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      NewPhraseForm newPhraseForm = new NewPhraseForm();
      newPhraseForm.WindowTitle = "New Custom Field Option";
      newPhraseForm.Description = "Please enter your new custom field option.";
      newPhraseForm.MaxPhaseLength = 50;
      newPhraseForm.IgnoreCase = true;
      newPhraseForm.TopMost = true;
      DialogResult dialogResult = DialogResult.Retry;
      while (true)
      {
        do
        {
          switch (dialogResult)
          {
            case DialogResult.Cancel:
              goto label_5;
            case DialogResult.Retry:
              dialogResult = newPhraseForm.ShowDialog();
              newPhraseForm.SelectText();
              continue;
            default:
              goto label_4;
          }
        }
        while (!this.containsDuplicate(newPhraseForm.NewPhrase) || dialogResult != DialogResult.OK);
        int num = (int) Utils.Dialog((IWin32Window) this, "The custom field option you just entered already exists. Please enter a different custom field option.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        dialogResult = DialogResult.Retry;
      }
label_5:
      return;
label_4:
      this.listBoxFieldOptions.Items.Add((object) newPhraseForm.NewPhrase.Trim());
      this.isDirty = true;
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      object selectedItem = this.listBoxFieldOptions.SelectedItem;
      if (selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an item in the list box to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (DialogResult.Cancel == MessageBox.Show("Are you sure that you want to delete the item " + selectedItem + "?", "Delete Item", MessageBoxButtons.OKCancel))
          return;
        this.listBoxFieldOptions.Items.Remove(selectedItem);
        this.isDirty = true;
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      object selectedItem = this.listBoxFieldOptions.SelectedItem;
      if (selectedItem == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select an item in the list box to edit.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        NewPhraseForm newPhraseForm = new NewPhraseForm(selectedItem.ToString());
        newPhraseForm.WindowTitle = "Edit Custom Field Option";
        newPhraseForm.Description = "Please enter your new custom field option.";
        newPhraseForm.MaxPhaseLength = 50;
        newPhraseForm.IgnoreCase = true;
        newPhraseForm.TopMost = true;
        DialogResult dialogResult = DialogResult.Retry;
        while (true)
        {
          do
          {
            switch (dialogResult)
            {
              case DialogResult.Cancel:
                goto label_7;
              case DialogResult.Retry:
                dialogResult = newPhraseForm.ShowDialog();
                newPhraseForm.SelectText();
                continue;
              default:
                goto label_6;
            }
          }
          while (!this.containsDuplicate(newPhraseForm.NewPhrase) || dialogResult != DialogResult.OK);
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The custom field option you just entered already exists. Please enter a different custom field option.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          dialogResult = DialogResult.Retry;
        }
label_7:
        return;
label_6:
        int index = this.listBoxFieldOptions.Items.IndexOf(selectedItem);
        this.listBoxFieldOptions.Items.RemoveAt(index);
        this.listBoxFieldOptions.Items.Insert(index, (object) newPhraseForm.NewPhrase.Trim());
        this.isDirty = true;
      }
    }

    private void btnUp_Click(object sender, EventArgs e)
    {
      object selectedItem = this.listBoxFieldOptions.SelectedItem;
      if (selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an item in the list box to move.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int index = this.listBoxFieldOptions.Items.IndexOf(selectedItem);
        if (index == 0)
          return;
        this.listBoxFieldOptions.Items.RemoveAt(index);
        this.listBoxFieldOptions.Items.Insert(index - 1, selectedItem);
        this.listBoxFieldOptions.SelectedIndex = index - 1;
        this.isDirty = true;
      }
    }

    private void btnDown_Click(object sender, EventArgs e)
    {
      object selectedItem = this.listBoxFieldOptions.SelectedItem;
      if (selectedItem == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an item in the list box to move.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int index = this.listBoxFieldOptions.Items.IndexOf(selectedItem);
        if (index == this.listBoxFieldOptions.Items.Count - 1)
          return;
        this.listBoxFieldOptions.Items.RemoveAt(index);
        this.listBoxFieldOptions.Items.Insert(index + 1, selectedItem);
        this.listBoxFieldOptions.SelectedIndex = index + 1;
        this.isDirty = true;
      }
    }

    private void btnClose_Click(object sender, EventArgs e) => this.btnCloseClicked = true;

    private void btnCancel_Click(object sender, EventArgs e) => this.isDirty = false;

    private void InitializeComponent()
    {
      this.listBoxFieldOptions = new ListBox();
      this.btnEdit = new Button();
      this.btnClose = new Button();
      this.btnDelete = new Button();
      this.btnNew = new Button();
      this.btnUp = new Button();
      this.btnDown = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.listBoxFieldOptions.Location = new Point(16, 12);
      this.listBoxFieldOptions.Name = "listBoxFieldOptions";
      this.listBoxFieldOptions.Size = new Size(310, 186);
      this.listBoxFieldOptions.TabIndex = 0;
      this.listBoxFieldOptions.SelectedIndexChanged += new EventHandler(this.listBoxFieldOptions_SelectedIndexChanged);
      this.btnEdit.Location = new Point(332, 68);
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(56, 23);
      this.btnEdit.TabIndex = 9;
      this.btnEdit.Text = "&Edit";
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(270, 208);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(56, 23);
      this.btnClose.TabIndex = 8;
      this.btnClose.Text = "&OK";
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnDelete.Location = new Point(332, 40);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(56, 23);
      this.btnDelete.TabIndex = 7;
      this.btnDelete.Text = "&Delete";
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnNew.Location = new Point(332, 12);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(56, 23);
      this.btnNew.TabIndex = 6;
      this.btnNew.Text = "&New";
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnUp.Location = new Point(332, 96);
      this.btnUp.Name = "btnUp";
      this.btnUp.Size = new Size(56, 23);
      this.btnUp.TabIndex = 10;
      this.btnUp.Text = "&Up";
      this.btnUp.Click += new EventHandler(this.btnUp_Click);
      this.btnDown.Location = new Point(332, 124);
      this.btnDown.Name = "btnDown";
      this.btnDown.Size = new Size(56, 23);
      this.btnDown.TabIndex = 11;
      this.btnDown.Text = "Do&wn";
      this.btnDown.Click += new EventHandler(this.btnDown_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(332, 208);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(56, 23);
      this.btnCancel.TabIndex = 12;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AcceptButton = (IButtonControl) this.btnClose;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(394, 237);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnDown);
      this.Controls.Add((Control) this.btnUp);
      this.Controls.Add((Control) this.btnEdit);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.btnDelete);
      this.Controls.Add((Control) this.btnNew);
      this.Controls.Add((Control) this.listBoxFieldOptions);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ContactCustomFieldOptionsForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Custom Field Options";
      this.FormClosing += new FormClosingEventHandler(this.ContactCustomFieldOptionsForm_FormClosing);
      this.ResumeLayout(false);
    }

    private void ContactCustomFieldOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.btnCloseClicked || !this.isDirty)
        return;
      switch (Utils.Dialog((IWin32Window) this, "Do you want to save the changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
      {
        case DialogResult.Cancel:
          e.Cancel = true;
          this.btnCloseClicked = false;
          break;
        case DialogResult.No:
          this.isDirty = false;
          this.DialogResult = DialogResult.Cancel;
          break;
        default:
          this.DialogResult = DialogResult.OK;
          break;
      }
    }

    private void listBoxFieldOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDelete.Enabled = this.btnEdit.Enabled = this.btnUp.Enabled = this.btnDown.Enabled = this.listBoxFieldOptions.SelectedItems.Count > 0;
      if (this.listBoxFieldOptions.SelectedIndex == 0)
        this.btnUp.Enabled = false;
      if (this.listBoxFieldOptions.SelectedIndex != this.listBoxFieldOptions.Items.Count - 1)
        return;
      this.btnDown.Enabled = false;
    }
  }
}
