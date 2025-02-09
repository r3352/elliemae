// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ContactNotesForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public abstract class ContactNotesForm : Form, IBindingForm
  {
    private TextBox txtBoxDetails;
    private Label label1;
    private Label label2;
    private TextBox txtBoxSubject;
    private DateTimePicker dateTimeTimeOfNote;
    private Button btnPhrases;
    private int currentContactId = -1;
    private ContactNote currentNote;
    private bool changed;
    private GroupContainer gcNotesList;
    private CollapsibleSplitter collapsibleSplitter1;
    private GroupContainer gcNoteDetail;
    private FlowLayoutPanel flowLayoutPanel1;
    private VerticalSeparator verticalSeparator3;
    private StandardIconButton btnDelete;
    private StandardIconButton btnNew;
    private GridView lvwNotes;
    private ToolTip toolTip1;
    private IContainer components;
    private object currentContact;
    public bool IsReadOnly;

    public event ContactDeletedEventHandler ContactDeleted;

    public ContactNotesForm()
    {
      this.InitializeComponent();
      this.clearNote();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.txtBoxDetails = new TextBox();
      this.label1 = new Label();
      this.txtBoxSubject = new TextBox();
      this.dateTimeTimeOfNote = new DateTimePicker();
      this.label2 = new Label();
      this.btnPhrases = new Button();
      this.gcNotesList = new GroupContainer();
      this.lvwNotes = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.verticalSeparator3 = new VerticalSeparator();
      this.btnDelete = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.gcNoteDetail = new GroupContainer();
      this.toolTip1 = new ToolTip(this.components);
      this.gcNotesList.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.gcNoteDetail.SuspendLayout();
      this.SuspendLayout();
      this.txtBoxDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxDetails.Location = new Point(7, 68);
      this.txtBoxDetails.MaxLength = 3700;
      this.txtBoxDetails.Multiline = true;
      this.txtBoxDetails.Name = "txtBoxDetails";
      this.txtBoxDetails.ScrollBars = ScrollBars.Both;
      this.txtBoxDetails.Size = new Size(421, 188);
      this.txtBoxDetails.TabIndex = 7;
      this.txtBoxDetails.TextChanged += new EventHandler(this.controlChanged);
      this.txtBoxDetails.MouseLeave += new EventHandler(this.details_MouseLeave);
      this.label1.Location = new Point(4, 36);
      this.label1.Name = "label1";
      this.label1.Size = new Size(44, 16);
      this.label1.TabIndex = 3;
      this.label1.Text = "Subject";
      this.txtBoxSubject.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxSubject.Location = new Point(54, 33);
      this.txtBoxSubject.MaxLength = 50;
      this.txtBoxSubject.Name = "txtBoxSubject";
      this.txtBoxSubject.Size = new Size(167, 20);
      this.txtBoxSubject.TabIndex = 4;
      this.txtBoxSubject.TextChanged += new EventHandler(this.summaryFieldChanged);
      this.txtBoxSubject.MouseLeave += new EventHandler(this.details_MouseLeave);
      this.dateTimeTimeOfNote.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.dateTimeTimeOfNote.CalendarMonthBackground = Color.WhiteSmoke;
      this.dateTimeTimeOfNote.CalendarTitleBackColor = Color.FromArgb(29, 110, 174);
      this.dateTimeTimeOfNote.CustomFormat = "M'/'d'/'yyyy' 'h':'mm' 'tt";
      this.dateTimeTimeOfNote.Format = DateTimePickerFormat.Custom;
      this.dateTimeTimeOfNote.Location = new Point(289, 32);
      this.dateTimeTimeOfNote.Name = "dateTimeTimeOfNote";
      this.dateTimeTimeOfNote.Size = new Size(136, 20);
      this.dateTimeTimeOfNote.TabIndex = 6;
      this.dateTimeTimeOfNote.MouseLeave += new EventHandler(this.details_MouseLeave);
      this.dateTimeTimeOfNote.ValueChanged += new EventHandler(this.summaryFieldChanged);
      this.label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.label2.Location = new Point(227, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(56, 16);
      this.label2.TabIndex = 5;
      this.label2.Text = "Date/Time";
      this.btnPhrases.BackColor = SystemColors.Control;
      this.btnPhrases.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnPhrases.Location = new Point(61, 0);
      this.btnPhrases.Margin = new Padding(0);
      this.btnPhrases.Name = "btnPhrases";
      this.btnPhrases.Padding = new Padding(2, 0, 0, 0);
      this.btnPhrases.RightToLeft = RightToLeft.Yes;
      this.btnPhrases.Size = new Size(75, 22);
      this.btnPhrases.TabIndex = 2;
      this.btnPhrases.Text = "Phrases";
      this.btnPhrases.UseVisualStyleBackColor = true;
      this.btnPhrases.Click += new EventHandler(this.btnPhrases_Click);
      this.gcNotesList.Controls.Add((Control) this.lvwNotes);
      this.gcNotesList.Controls.Add((Control) this.flowLayoutPanel1);
      this.gcNotesList.Dock = DockStyle.Left;
      this.gcNotesList.Location = new Point(0, 0);
      this.gcNotesList.Name = "gcNotesList";
      this.gcNotesList.Size = new Size(260, 268);
      this.gcNotesList.TabIndex = 11;
      this.gcNotesList.Text = "Notes";
      this.lvwNotes.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Date/Time";
      gvColumn1.SortMethod = GVSortMethod.DateTime;
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Subject";
      gvColumn2.Width = 160;
      this.lvwNotes.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.lvwNotes.Dock = DockStyle.Fill;
      this.lvwNotes.Location = new Point(1, 26);
      this.lvwNotes.Name = "lvwNotes";
      this.lvwNotes.Size = new Size(258, 241);
      this.lvwNotes.TabIndex = 12;
      this.lvwNotes.SelectedIndexChanged += new EventHandler(this.lvwNotes_SelectedIndexChanged);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnPhrases);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator3);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.flowLayoutPanel1.Location = new Point(119, 2);
      this.flowLayoutPanel1.Margin = new Padding(0);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.RightToLeft = RightToLeft.Yes;
      this.flowLayoutPanel1.Size = new Size(136, 22);
      this.flowLayoutPanel1.TabIndex = 11;
      this.verticalSeparator3.Location = new Point(56, 3);
      this.verticalSeparator3.MaximumSize = new Size(2, 16);
      this.verticalSeparator3.MinimumSize = new Size(2, 16);
      this.verticalSeparator3.Name = "verticalSeparator3";
      this.verticalSeparator3.Size = new Size(2, 16);
      this.verticalSeparator3.TabIndex = 29;
      this.verticalSeparator3.Text = "verticalSeparator3";
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(35, 3);
      this.btnDelete.Margin = new Padding(2, 3, 3, 3);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 30;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Note");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(14, 3);
      this.btnNew.Margin = new Padding(2, 3, 3, 3);
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 31;
      this.btnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNew, "New Note");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.gcNotesList;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(260, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 12;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.gcNoteDetail.Controls.Add((Control) this.label1);
      this.gcNoteDetail.Controls.Add((Control) this.txtBoxSubject);
      this.gcNoteDetail.Controls.Add((Control) this.label2);
      this.gcNoteDetail.Controls.Add((Control) this.txtBoxDetails);
      this.gcNoteDetail.Controls.Add((Control) this.dateTimeTimeOfNote);
      this.gcNoteDetail.Dock = DockStyle.Fill;
      this.gcNoteDetail.Location = new Point(267, 0);
      this.gcNoteDetail.Name = "gcNoteDetail";
      this.gcNoteDetail.Size = new Size(440, 268);
      this.gcNoteDetail.TabIndex = 13;
      this.gcNoteDetail.Text = "Note Details";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(707, 268);
      this.Controls.Add((Control) this.gcNoteDetail);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.gcNotesList);
      this.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (ContactNotesForm);
      this.Text = nameof (ContactNotesForm);
      this.Closing += new CancelEventHandler(this.ContactNotesForm_Closing);
      this.gcNotesList.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.gcNoteDetail.ResumeLayout(false);
      this.gcNoteDetail.PerformLayout();
      this.ResumeLayout(false);
    }

    public bool isDirty() => this.changed;

    public int CurrentContactID
    {
      get => this.currentContactId;
      set
      {
        this.currentContactId = -1;
        this.lvwNotes.Items.Clear();
        this.gcNotesList.Text = "Notes (0)";
        this.clearNote();
        if (value < 0)
          return;
        foreach (ContactNote fetchNote in this.fetchNotes(value))
          this.addNoteToListview(fetchNote);
        this.currentContactId = value;
        if (this.lvwNotes.Items.Count > 0)
          this.lvwNotes.Items[0].Selected = true;
        this.btnNew.Enabled = true;
      }
    }

    public object CurrentContact
    {
      get => this.currentContact;
      set
      {
        this.currentContactId = -1;
        this.currentContact = (object) null;
        this.lvwNotes.Items.Clear();
        this.gcNotesList.Text = "Notes (0)";
        this.clearNote();
        if (value == null)
          return;
        this.currentContact = value;
        this.currentContactId = value is BorrowerInfo ? ((BorrowerInfo) value).ContactID : ((BizPartnerInfo) value).ContactID;
        foreach (ContactNote fetchNote in this.fetchNotes(this.currentContactId))
          this.addNoteToListview(fetchNote);
        if (this.lvwNotes.Items.Count > 0)
          this.lvwNotes.Items[0].Selected = true;
        this.btnNew.Enabled = true;
      }
    }

    private GVItem addNoteToListview(ContactNote note)
    {
      string[] items = new string[2];
      DateTime timestamp = note.Timestamp;
      string shortDateString = timestamp.ToShortDateString();
      timestamp = note.Timestamp;
      string shortTimeString = timestamp.ToShortTimeString();
      items[0] = shortDateString + " " + shortTimeString;
      items[1] = note.Subject;
      GVItem listview = new GVItem(items);
      listview.Tag = (object) note;
      this.lvwNotes.Items.Add(listview);
      this.gcNotesList.Text = "Notes (" + (object) this.lvwNotes.Items.Count + ")";
      return listview;
    }

    public void removeNoteFromListView(int noteId)
    {
      if (noteId == -1)
      {
        if (this.ContactDeleted != null)
          this.ContactDeleted(this.currentContactId);
        this.CurrentContactID = -1;
      }
      else
      {
        for (int nItemIndex = 0; nItemIndex < this.lvwNotes.Items.Count; ++nItemIndex)
        {
          if (((ContactNote) this.lvwNotes.Items[nItemIndex].Tag).NoteID == noteId)
          {
            if (this.lvwNotes.Items[nItemIndex].Selected)
              this.lvwNotes.Items[nItemIndex].Selected = false;
            this.lvwNotes.Items.RemoveAt(nItemIndex);
            return;
          }
        }
        this.gcNotesList.Text = "Notes (" + (object) this.lvwNotes.Items.Count + ")";
      }
    }

    private void removeNoteSafe(int noteId)
    {
      this.Invoke((Delegate) new ContactNotesForm.NoteMethod(this.removeNoteFromListView), (object) noteId);
    }

    private void removeNoteAsynch(int noteId)
    {
      new ContactNotesForm.NoteMethod(this.removeNoteSafe).BeginInvoke(noteId, (AsyncCallback) null, (object) null);
    }

    public bool SaveChanges() => this.SaveChanges(true);

    public bool SaveChanges(bool silent)
    {
      if (!this.changed || this.currentContactId == -1)
        return false;
      if (this.currentNote == null)
        return false;
      try
      {
        this.currentNote.Details = this.txtBoxDetails.Text;
        this.currentNote.Subject = this.txtBoxSubject.Text;
        this.currentNote.Timestamp = this.dateTimeTimeOfNote.Value;
        this.updateNote(this.currentContactId, this.currentNote);
        this.changed = false;
      }
      catch (ObjectNotFoundException ex)
      {
        if (ex.ObjectType == ObjectType.ContactNote)
        {
          if (!silent)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "An attempt to save the current note failed because the note has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          this.removeNoteAsynch(this.currentNote.NoteID);
          this.currentNote = (ContactNote) null;
        }
        else if (ex.ObjectType == ObjectType.Contact)
        {
          if (!silent)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "An attempt to save the current note failed because the contact has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.removeNoteAsynch(-1);
          }
          this.currentContactId = -1;
          this.currentNote = (ContactNote) null;
          if (silent)
            throw;
        }
      }
      catch (Exception ex)
      {
        if (!silent)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to save this note: " + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      return false;
    }

    protected abstract ContactNote[] fetchNotes(int contactId);

    protected abstract ContactNote fetchNote(int contactId, int noteId);

    protected abstract void updateNote(int contactId, ContactNote note);

    protected abstract int addNote(int contactId, ContactNote note);

    protected abstract void deleteNote(int contactId, int noteId);

    private void lvwNotes_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SaveChanges(false);
      try
      {
        if (this.lvwNotes.SelectedItems.Count == 1)
        {
          this.showNote((ContactNote) this.lvwNotes.SelectedItems[0].Tag);
        }
        else
        {
          this.currentNote = (ContactNote) null;
          this.clearNote();
          if (this.lvwNotes.SelectedItems.Count <= 0)
            return;
          this.btnDelete.Enabled = true;
        }
      }
      catch
      {
      }
    }

    public void disableChildFields()
    {
      this.txtBoxDetails.Text = string.Empty;
      this.txtBoxDetails.ReadOnly = true;
      this.txtBoxSubject.Text = string.Empty;
      this.txtBoxSubject.ReadOnly = true;
      this.dateTimeTimeOfNote.Text = string.Empty;
      this.dateTimeTimeOfNote.Enabled = false;
      this.btnDelete.Enabled = false;
      this.btnPhrases.Enabled = false;
      this.btnNew.Enabled = false;
    }

    public void enableChildFields()
    {
      this.txtBoxSubject.ReadOnly = false;
      this.txtBoxDetails.ReadOnly = false;
      this.dateTimeTimeOfNote.Enabled = true;
      this.btnNew.Enabled = true;
      this.btnDelete.Enabled = true;
      this.btnPhrases.Enabled = true;
    }

    protected void showNote(ContactNote note)
    {
      if (!this.IsReadOnly)
        this.enableChildFields();
      this.txtBoxSubject.Text = note.Subject;
      this.dateTimeTimeOfNote.Value = note.Timestamp;
      this.txtBoxDetails.Text = note.Details;
      this.currentNote = note;
      this.changed = false;
    }

    private void clearNote()
    {
      this.disableChildFields();
      if (this.currentContactId >= 0)
        this.btnNew.Enabled = true;
      this.currentNote = (ContactNote) null;
      this.changed = false;
    }

    public int CreateNewNote()
    {
      this.SaveChanges(false);
      if (this.currentContactId == -1)
        return -1;
      try
      {
        int noteId = this.addNote(this.currentContactId, new ContactNote());
        this.currentNote = this.fetchNote(this.currentContactId, noteId);
        GVItem listview = this.addNoteToListview(this.currentNote);
        if (this.lvwNotes.SelectedItems.Count > 0)
        {
          foreach (GVItem selectedItem in this.lvwNotes.SelectedItems)
            selectedItem.Selected = false;
        }
        listview.Selected = true;
        this.txtBoxSubject.Focus();
        return noteId;
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to add new note. This contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.removeNoteAsynch(-1);
        return -1;
      }
    }

    private void btnNew_Click(object sender, EventArgs e) => this.CreateNewNote();

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.lvwNotes.SelectedItems.Count == 0)
        return;
      if (Utils.Dialog((IWin32Window) this, "Delete the selected note(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      try
      {
        foreach (GVItem selectedItem in this.lvwNotes.SelectedItems)
        {
          this.deleteNote(this.currentContactId, ((ContactNote) this.lvwNotes.SelectedItems[0].Tag).NoteID);
          this.currentNote = (ContactNote) null;
          this.changed = false;
          this.lvwNotes.Items.Remove(selectedItem);
        }
      }
      catch (ObjectNotFoundException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error deleting note. This contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.removeNoteAsynch(-1);
      }
    }

    private void btnPhrases_Click(object sender, EventArgs e)
    {
      NotePhrasesForm instance = NotePhrasesForm.Instance;
      instance.TopMost = true;
      instance.PhraseSelected += new NotePhrasesForm.PhraseSelectedEventHandler(this.OnPhraseSelected);
      instance.Show();
    }

    public void OnPhraseSelected(PhraseSelectedEventArg e)
    {
      this.txtBoxDetails.Text += e.SelectedPhrase;
    }

    private void controlChanged(object sender, EventArgs e) => this.changed = true;

    private void ContactNotesForm_Closing(object sender, CancelEventArgs e)
    {
      try
      {
        this.SaveChanges();
      }
      catch
      {
      }
    }

    public void summaryFieldChanged(object source, EventArgs e)
    {
      if (this.currentNote != null && this.lvwNotes.SelectedItems.Count > 0)
      {
        GVItem selectedItem = this.lvwNotes.SelectedItems[0];
        DateTime dateTime = this.dateTimeTimeOfNote.Value;
        selectedItem.SubItems[0].Text = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
        selectedItem.SubItems[1].Text = this.txtBoxSubject.Text;
      }
      this.controlChanged(source, e);
    }

    private void details_MouseLeave(object sender, EventArgs e) => this.SaveChanges(false);

    private delegate void NoteMethod(int noteId);
  }
}
