// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DocumentSetTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DocumentSetTemplateDialog : Form, IHelp
  {
    private const string className = "DocumentSetTemplateDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private Button removeBtn;
    private Button addBtn;
    private ComboBox stageCombo;
    private Label label1;
    private Label label4;
    private TextBox descTxt;
    private TextBox nameTxt;
    private Label label5;
    private Button cancelBtn;
    private Button saveBtn;
    private System.ComponentModel.Container components;
    private DocumentSetTemplate docTemplate;
    private DocumentTrackingSetup docList;
    private List<EllieMae.EMLite.Workflow.Milestone> msList;
    private EMHelpLink emHelpLink1;
    private GroupContainer groupContainer1;
    private GroupContainer groupContainer2;
    private GridView gridSelected;
    private PanelEx panelEx2;
    private GridView gridPredefined;
    private PanelEx panelEx1;
    private string[] milestoneNames;
    private bool readOnly;
    private Sessions.Session session;
    private LogList list;
    private const string BADCHARS = "/:*?<>|.";

    public DocumentSetTemplateDialog(
      DocumentSetTemplate docTemplate,
      bool readOnly,
      Sessions.Session session)
      : this(docTemplate, session)
    {
      this.readOnly = readOnly;
      if (!this.readOnly)
        return;
      this.nameTxt.ReadOnly = true;
      this.descTxt.ReadOnly = true;
      this.addBtn.Enabled = false;
      this.removeBtn.Enabled = false;
      this.saveBtn.Enabled = false;
      this.cancelBtn.Text = "Close";
      this.AcceptButton = (IButtonControl) this.cancelBtn;
      if (session.LoanData == null)
        return;
      this.list = session.LoanData.GetLogList();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridSelected.Items)
      {
        if (!(gvItem.SubItems[1].Text == ""))
        {
          EllieMae.EMLite.Workflow.Milestone milestoneByName = ((MilestoneTemplatesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.Milestones)).GetMilestoneByName(gvItem.SubItems[1].Text);
          if (milestoneByName != null && this.list.GetMilestoneByID(milestoneByName.MilestoneID) == null)
            gvItem.ForeColor = Color.Gray;
        }
      }
    }

    public DocumentSetTemplateDialog(DocumentSetTemplate docTemplate, Sessions.Session session)
    {
      this.session = session;
      this.docList = this.session.ConfigurationManager.GetDocumentTrackingSetup();
      this.msList = this.session.StartupInfo.Milestones;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.docTemplate = docTemplate;
      this.milestoneNames = this.getAllMilestoneNames();
      this.stageCombo.Items.AddRange((object[]) this.milestoneNames);
      this.showTrackedDocList();
      this.stageCombo.SelectedIndexChanged += new EventHandler(this.stageCombo_SelectedIndexChanged);
      this.stageCombo.SelectedItem = (object) this.milestoneNames[1];
      this.nameTxt.Text = this.docTemplate.TemplateName;
      this.descTxt.Text = this.docTemplate.Description;
      this.gridPredefined_SelectedIndexChanged((object) null, (EventArgs) null);
      this.gridSelected_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private string[] getAllMilestoneNames()
    {
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) string.Empty);
      for (int index = 0; index < this.msList.Count; ++index)
      {
        if (!this.msList[index].Archived && !(this.msList[index].Name == "Started") && !(this.msList[index].Name == "Completion"))
          arrayList.Add((object) this.msList[index].Name);
      }
      return (string[]) arrayList.ToArray(typeof (string));
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
      this.removeBtn = new Button();
      this.addBtn = new Button();
      this.stageCombo = new ComboBox();
      this.label1 = new Label();
      this.label4 = new Label();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label5 = new Label();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer1 = new GroupContainer();
      this.panelEx2 = new PanelEx();
      this.gridPredefined = new GridView();
      this.panelEx1 = new PanelEx();
      this.groupContainer2 = new GroupContainer();
      this.gridSelected = new GridView();
      this.groupContainer1.SuspendLayout();
      this.panelEx2.SuspendLayout();
      this.panelEx1.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.SuspendLayout();
      this.removeBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.removeBtn.Location = new Point(262, 271);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(72, 23);
      this.removeBtn.TabIndex = 5;
      this.removeBtn.Text = "< Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.addBtn.Location = new Point(261, 242);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(72, 23);
      this.addBtn.TabIndex = 4;
      this.addBtn.Text = "Add >";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.stageCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.stageCombo.Location = new Point(57, 4);
      this.stageCombo.Name = "stageCombo";
      this.stageCombo.Size = new Size(184, 21);
      this.stageCombo.TabIndex = 2;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(3, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(52, 13);
      this.label1.TabIndex = 18;
      this.label1.Text = "Milestone";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(4, 38);
      this.label4.Name = "label4";
      this.label4.Size = new Size(60, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "Description";
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(93, 38);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(494, 66);
      this.descTxt.TabIndex = 0;
      this.nameTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTxt.Location = new Point(93, 12);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.ReadOnly = true;
      this.nameTxt.Size = new Size(493, 20);
      this.nameTxt.TabIndex = 1;
      this.nameTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(5, 15);
      this.label5.Name = "label5";
      this.label5.Size = new Size(82, 13);
      this.label5.TabIndex = 23;
      this.label5.Text = "Template Name";
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(504, 512);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 8;
      this.cancelBtn.Text = "&Cancel";
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveBtn.Location = new Point(424, 512);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 24);
      this.saveBtn.TabIndex = 7;
      this.saveBtn.Text = "&Save";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Document Sets";
      this.emHelpLink1.Location = new Point(12, 516);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 25;
      this.groupContainer1.Controls.Add((Control) this.panelEx2);
      this.groupContainer1.Controls.Add((Control) this.panelEx1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(8, 110);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(246, 384);
      this.groupContainer1.TabIndex = 26;
      this.groupContainer1.Text = "Predefined Documents";
      this.panelEx2.Controls.Add((Control) this.gridPredefined);
      this.panelEx2.Dock = DockStyle.Fill;
      this.panelEx2.Location = new Point(1, 54);
      this.panelEx2.Name = "panelEx2";
      this.panelEx2.Size = new Size(244, 329);
      this.panelEx2.TabIndex = 1;
      this.gridPredefined.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SpringToFit = true;
      gvColumn1.Text = "Name";
      gvColumn1.Width = 244;
      this.gridPredefined.Columns.AddRange(new GVColumn[1]
      {
        gvColumn1
      });
      this.gridPredefined.Dock = DockStyle.Fill;
      this.gridPredefined.Location = new Point(0, 0);
      this.gridPredefined.Name = "gridPredefined";
      this.gridPredefined.Size = new Size(244, 329);
      this.gridPredefined.TabIndex = 0;
      this.gridPredefined.SelectedIndexChanged += new EventHandler(this.gridPredefined_SelectedIndexChanged);
      this.panelEx1.Controls.Add((Control) this.label1);
      this.panelEx1.Controls.Add((Control) this.stageCombo);
      this.panelEx1.Dock = DockStyle.Top;
      this.panelEx1.Location = new Point(1, 26);
      this.panelEx1.Name = "panelEx1";
      this.panelEx1.Size = new Size(244, 28);
      this.panelEx1.TabIndex = 0;
      this.groupContainer2.Controls.Add((Control) this.gridSelected);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(341, 110);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(246, 384);
      this.groupContainer2.TabIndex = 26;
      this.groupContainer2.Text = "Tracked Documents";
      this.gridSelected.BorderStyle = BorderStyle.None;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column1";
      gvColumn2.Text = "Document";
      gvColumn2.Width = 150;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.Text = "Milestone";
      gvColumn3.Width = 100;
      this.gridSelected.Columns.AddRange(new GVColumn[2]
      {
        gvColumn2,
        gvColumn3
      });
      this.gridSelected.Dock = DockStyle.Fill;
      this.gridSelected.Location = new Point(1, 26);
      this.gridSelected.Name = "gridSelected";
      this.gridSelected.Size = new Size(244, 357);
      this.gridSelected.TabIndex = 1;
      this.gridSelected.SelectedIndexChanged += new EventHandler(this.gridSelected_SelectedIndexChanged);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(596, 549);
      this.Controls.Add((Control) this.groupContainer2);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.removeBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.addBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.nameTxt);
      this.Controls.Add((Control) this.label5);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DocumentSetTemplateDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Document Set Template Details";
      this.KeyPress += new KeyPressEventHandler(this.DocumentSetTemplateDialog_KeyPress);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupContainer1.ResumeLayout(false);
      this.panelEx2.ResumeLayout(false);
      this.panelEx1.ResumeLayout(false);
      this.panelEx1.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      ArrayList documentsByMilestone = this.docTemplate.GetDocumentsByMilestone((string) this.stageCombo.SelectedItem);
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gridPredefined.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        this.gridPredefined.Items.Remove(gvItem);
        documentsByMilestone.Add((object) gvItem.Text);
      }
      this.showTrackedDocList();
      this.refreshSaveButton();
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gridSelected.SelectedItems)
        arrayList.Add((object) selectedItem);
      foreach (GVItem gvItem in arrayList)
      {
        string text = gvItem.SubItems[0].Text;
        this.docTemplate.GetDocumentsByMilestone(gvItem.SubItems[1].Text).Remove((object) text);
        this.gridSelected.Items.Remove(gvItem);
      }
      this.stageCombo_SelectedIndexChanged((object) null, (EventArgs) null);
      this.refreshSaveButton();
    }

    private void refreshSaveButton()
    {
      this.saveBtn.Enabled = !this.readOnly && this.gridSelected.Items.Count > 0;
    }

    private void stageCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      ArrayList documentsByMilestone = this.docTemplate.GetDocumentsByMilestone((string) this.stageCombo.SelectedItem);
      this.gridPredefined.Items.Clear();
      foreach (DocumentTemplate documentTemplate in this.docList.dictDocTrackByName.Values)
      {
        string name = documentTemplate.Name;
        if (!documentsByMilestone.Contains((object) name))
          this.gridPredefined.Items.Add(new GVItem(name));
      }
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      this.docTemplate.Description = this.descTxt.Text.Trim();
      this.DialogResult = DialogResult.OK;
    }

    private void showTrackedDocList()
    {
      this.gridSelected.Items.Clear();
      foreach (string str in this.docTemplate.GetDocumentsByMilestone(string.Empty))
        this.gridSelected.Items.Add(new GVItem(new string[2]
        {
          str,
          string.Empty
        }));
      foreach (string str in this.docTemplate.GetDocumentsByMilestone("Started"))
        this.gridSelected.Items.Add(new GVItem(new string[2]
        {
          str,
          "Started"
        }));
      for (int index = 1; index < this.msList.Count - 1; ++index)
      {
        string name = this.msList[index].Name;
        foreach (string text in this.docTemplate.GetDocumentsByMilestone(name))
          this.gridSelected.Items.Add(new GVItem(text)
          {
            SubItems = {
              (object) name
            }
          });
      }
      foreach (string text in this.docTemplate.GetDocumentsByMilestone("Completion"))
        this.gridSelected.Items.Add(new GVItem(text)
        {
          SubItems = {
            (object) "Completion"
          }
        });
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if ("/:*?<>|.".IndexOf(e.KeyChar) == -1)
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('\\'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('"'))
          {
            e.Handled = false;
            return;
          }
        }
        e.Handled = true;
      }
      else
        e.Handled = true;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, nameof (DocumentSetTemplateDialog));
    }

    private void DocumentSetTemplateDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.Close();
    }

    private void gridPredefined_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.addBtn.Enabled = !this.readOnly && this.gridPredefined.SelectedItems.Count > 0;
    }

    private void gridSelected_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.removeBtn.Enabled = !this.readOnly && this.gridSelected.SelectedItems.Count > 0;
    }
  }
}
