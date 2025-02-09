// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditCompanyNoteControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditCompanyNoteControl : UserControl
  {
    private Sessions.Session session;
    private int orgID = -1;
    private IContainer components;
    private StandardIconButton btnView;
    private StandardIconButton btnDelete;
    private StandardIconButton btnExport;
    private StandardIconButton btnAdd;
    private GridView gridViewNotes;
    private GroupContainer groupContainer2;
    private Panel panelHeader;
    private Label label33;
    private GroupContainer groupContainer3;

    public EditCompanyNoteControl(Sessions.Session session, int orgID)
    {
      this.session = session;
      this.orgID = orgID;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.initForm();
      bool hierarchyAccess;
      Session.TpoHierarchyAccessCache.TryGetValue(orgID, out hierarchyAccess);
      TPOClientUtils.DisableControls((UserControl) this, hierarchyAccess);
      if (this.gridViewNotes.Items.Count <= 0)
        return;
      this.btnExport.Enabled = true;
    }

    private void initForm()
    {
      this.gridViewNotes.BeginUpdate();
      this.gridViewNotes.Items.Clear();
      ExternalOrgNotes organizationNotes = this.session.ConfigurationManager.GetExternalOrganizationNotes(this.orgID);
      if (organizationNotes != null && organizationNotes.Count > 0)
      {
        for (int i = 0; i < organizationNotes.Count; ++i)
          this.gridViewNotes.Items.Add(this.buildNotesGVItem(organizationNotes.GetNotesAt(i)));
      }
      this.gridViewNotes.EndUpdate();
      this.btnExport.Enabled = this.gridViewNotes.Items.Count > 0;
    }

    private GVItem buildNotesGVItem(ExternalOrgNote note)
    {
      GVItem gvItem = new GVItem(note.AddedDateTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
      gvItem.SubItems.Add((object) note.WhoAdded);
      if (note.NotesDetails.Length > this.gridViewNotes.Columns[2].Width / 5 - 10)
        gvItem.SubItems.Add((object) (note.NotesDetails.Substring(0, this.gridViewNotes.Columns[2].Width / 5 - 10) + "..."));
      else
        gvItem.SubItems.Add((object) note.NotesDetails);
      gvItem.Tag = (object) note;
      return gvItem;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      using (AddNotesForm addNotesForm = new AddNotesForm())
      {
        if (addNotesForm.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          ExternalOrgNote externalOrgNote = new ExternalOrgNote(this.session.UserID, addNotesForm.NotesDetails);
          externalOrgNote.ExternalCompanyID = this.orgID;
          externalOrgNote.AddedDateTime = this.session.ServerTime;
          int num = this.session.ConfigurationManager.AddExternalOrganizationNotes(externalOrgNote);
          WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
          if (num > -1)
          {
            externalOrgNote.NoteID = num;
            this.gridViewNotes.Items.Add(this.buildNotesGVItem(externalOrgNote));
          }
        }
      }
      this.btnExport.Enabled = this.gridViewNotes.Items.Count > 0;
    }

    private void gridViewNotes_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnDelete.Enabled = this.gridViewNotes.SelectedItems.Count > 0;
      this.btnView.Enabled = this.gridViewNotes.SelectedItems.Count == 1;
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      if (this.gridViewNotes.SelectedItems.Count == 0)
        return;
      using (AddNotesForm addNotesForm = new AddNotesForm(((ExternalOrgNote) this.gridViewNotes.SelectedItems[0].Tag).NotesDetails))
      {
        int num = (int) addNotesForm.ShowDialog((IWin32Window) this);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gridViewNotes.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select note(s) to delete.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the selected note(s)?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          return;
        ExternalOrgNotes deleteNotes = new ExternalOrgNotes();
        this.gridViewNotes.SelectedItems.ToList<GVItem>().ForEach((Action<GVItem>) (x => deleteNotes.AddNotes((ExternalOrgNote) x.Tag)));
        this.session.ConfigurationManager.DeleteExternalOrganizationNotes(this.orgID, deleteNotes);
        WebhookApiHelper.PublishExternalOrgWebhookEvent(this.session.UserID, this.Parent.Text, this.orgID);
        this.initForm();
      }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      ExcelHandler excelExport = new ExcelHandler();
      excelExport.Headers = new string[3]
      {
        "Date/Time",
        "User",
        "Details"
      };
      this.gridViewNotes.Items.ToList<GVItem>().ForEach((Action<GVItem>) (x =>
      {
        ExternalOrgNote tag = (ExternalOrgNote) x.Tag;
        string str = tag.NotesDetails.Replace("\r", "\n");
        excelExport.AddDataRow(new string[3]
        {
          tag.AddedDateTime.ToString("MM/dd/yyyy hh:mm:ss tt"),
          tag.WhoAdded,
          str
        });
      }));
      excelExport.CreateExcel();
    }

    public void DisableControls()
    {
      this.btnAdd.Visible = this.btnDelete.Visible = this.btnExport.Visible = false;
      this.btnView.Left = this.btnDelete.Left;
      this.disableControl(this.Controls);
      if (this.gridViewNotes.Items.Count > 0)
      {
        this.gridViewNotes.SelectedIndexChanged -= new EventHandler(this.gridViewNotes_SelectedIndexChanged);
        this.gridViewNotes.Items[0].Selected = true;
        this.btnView.Enabled = true;
        this.gridViewNotes.SelectedIndexChanged += new EventHandler(this.gridViewNotes_SelectedIndexChanged);
      }
      else
        this.btnView.Visible = false;
    }

    private void disableControl(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        switch (control)
        {
          case TextBox _:
          case CheckBox _:
          case ComboBox _:
          case DatePicker _:
            control.Enabled = false;
            break;
        }
        if (control.Controls != null && control.Controls.Count > 0)
          this.disableControl(control.Controls);
      }
    }

    private void btnView_Click(object source, GVItemEventArgs e)
    {
      this.btnView_Click(source, new EventArgs());
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      this.groupContainer2 = new GroupContainer();
      this.groupContainer3 = new GroupContainer();
      this.btnView = new StandardIconButton();
      this.gridViewNotes = new GridView();
      this.btnDelete = new StandardIconButton();
      this.btnExport = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.panelHeader = new Panel();
      this.label33 = new Label();
      this.groupContainer2.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      ((ISupportInitialize) this.btnView).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnExport).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.panelHeader.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainer2.Controls.Add((Control) this.groupContainer3);
      this.groupContainer2.Controls.Add((Control) this.panelHeader);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(5, 5);
      this.groupContainer2.Margin = new Padding(0, 0, 0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(862, 610);
      this.groupContainer2.TabIndex = 11;
      this.groupContainer2.Text = "Notes";
      this.groupContainer3.Borders = AnchorStyles.Top;
      this.groupContainer3.Controls.Add((Control) this.btnView);
      this.groupContainer3.Controls.Add((Control) this.gridViewNotes);
      this.groupContainer3.Controls.Add((Control) this.btnDelete);
      this.groupContainer3.Controls.Add((Control) this.btnExport);
      this.groupContainer3.Controls.Add((Control) this.btnAdd);
      this.groupContainer3.Dock = DockStyle.Fill;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(1, 52);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(860, 557);
      this.groupContainer3.TabIndex = 12;
      this.groupContainer3.Text = "Notes";
      this.btnView.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnView.BackColor = Color.Transparent;
      this.btnView.Enabled = false;
      this.btnView.Location = new Point(795, 6);
      this.btnView.MouseDownImage = (Image) null;
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(16, 16);
      this.btnView.StandardButtonType = StandardIconButton.ButtonType.ZoomInButton;
      this.btnView.TabIndex = 12;
      this.btnView.TabStop = false;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.gridViewNotes.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnDate";
      gvColumn1.SortMethod = GVSortMethod.DateTime;
      gvColumn1.Text = "Date/Time";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 136;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnUser";
      gvColumn2.Text = "User";
      gvColumn2.Width = 120;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnDetails";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Details";
      gvColumn3.Width = 604;
      this.gridViewNotes.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewNotes.Dock = DockStyle.Fill;
      this.gridViewNotes.Location = new Point(0, 26);
      this.gridViewNotes.Name = "gridViewNotes";
      this.gridViewNotes.Size = new Size(860, 531);
      this.gridViewNotes.TabIndex = 8;
      this.gridViewNotes.SelectedIndexChanged += new EventHandler(this.gridViewNotes_SelectedIndexChanged);
      this.gridViewNotes.ItemDoubleClick += new GVItemEventHandler(this.btnView_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(839, 6);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 11;
      this.btnDelete.TabStop = false;
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Enabled = false;
      this.btnExport.Location = new Point(817, 6);
      this.btnExport.MouseDownImage = (Image) null;
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 10;
      this.btnExport.TabStop = false;
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(773, 6);
      this.btnAdd.MouseDownImage = (Image) null;
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 9;
      this.btnAdd.TabStop = false;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.panelHeader.Controls.Add((Control) this.label33);
      this.panelHeader.Dock = DockStyle.Top;
      this.panelHeader.Location = new Point(1, 26);
      this.panelHeader.Name = "panelHeader";
      this.panelHeader.Size = new Size(860, 26);
      this.panelHeader.TabIndex = 2;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(6, 6);
      this.label33.Name = "label33";
      this.label33.Size = new Size(589, 13);
      this.label33.TabIndex = 36;
      this.label33.Text = "Use this screen to add notes, comments, and any additional information about the Third Party Originator company or branch.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer2);
      this.Margin = new Padding(0, 0, 0, 0);
      this.Name = nameof (EditCompanyNoteControl);
      this.Padding = new Padding(5);
      this.Size = new Size(872, 620);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      ((ISupportInitialize) this.btnView).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnExport).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.panelHeader.ResumeLayout(false);
      this.panelHeader.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
