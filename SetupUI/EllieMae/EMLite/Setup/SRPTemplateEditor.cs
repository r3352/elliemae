// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SRPTemplateEditor
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SRPTemplateEditor : Form
  {
    private SRPTableTemplate srpTemplate;
    private bool modified;
    private Sessions.Session session;
    private IContainer components;
    private Panel pnlTemplate;
    private TextBox txtName;
    private Label label1;
    private GroupBox groupBox1;
    private Panel panel1;
    private Button btnCancel;
    private Button btnSave;
    private Panel panel2;
    private SRPTableEditor ctlEditor;
    private TextBox txtDescription;
    private Label label2;
    protected EMHelpLink emHelpLink1;

    public SRPTemplateEditor(SRPTableTemplate srpTemplate, Sessions.Session session)
    {
      this.srpTemplate = srpTemplate;
      this.session = session;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.loadSRPTemplateData();
    }

    public SRPTemplateEditor()
      : this(new SRPTableTemplate(), Session.DefaultInstance)
    {
    }

    private void loadSRPTemplateData()
    {
      this.txtName.Text = this.srpTemplate.TemplateName;
      this.txtDescription.Text = this.srpTemplate.Description;
      this.ctlEditor.SRPTable = this.srpTemplate.SRPTable;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.srpTemplate != null && !this.performTemplateValidation() || !this.ctlEditor.ValidateChanges())
        return;
      this.ctlEditor.CommitChanges();
      this.srpTemplate.Description = this.txtDescription.Text;
      if (!this.saveTemplate())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private bool saveTemplate()
    {
      string templateName = this.srpTemplate.TemplateName;
      string entryName = this.txtName.Text.Trim();
      if (templateName != "" && templateName != entryName)
        this.session.ConfigurationManager.DeleteTemplateSettingsObject(TemplateSettingsType.SRPTable, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, templateName, FileSystemEntry.Types.File, (string) null));
      this.srpTemplate.TemplateName = entryName;
      this.session.ConfigurationManager.SaveTemplateSettings(TemplateSettingsType.SRPTable, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, entryName, FileSystemEntry.Types.File, (string) null), (BinaryObject) (BinaryConvertibleObject) this.srpTemplate);
      return true;
    }

    private bool performTemplateValidation()
    {
      string str = this.txtName.Text.Trim();
      if (str == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A name must be provided for this template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      FileSystemEntry fileEntry = new FileSystemEntry(FileSystemEntry.PublicRoot.Path, str, FileSystemEntry.Types.File, (string) null);
      if (string.Compare(str, this.srpTemplate.TemplateName, true) == 0 || !this.session.ConfigurationManager.TemplateSettingsObjectExists(TemplateSettingsType.SRPTable, fileEntry))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The name '" + str + "' is already in use. Select a new name and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void SRPTableEditor_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.Cancel || !this.modified || Utils.Dialog((IWin32Window) this, "Any changes made to this screen will be lost.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        return;
      e.Cancel = true;
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlTemplate = new Panel();
      this.txtDescription = new TextBox();
      this.label2 = new Label();
      this.txtName = new TextBox();
      this.label1 = new Label();
      this.groupBox1 = new GroupBox();
      this.ctlEditor = new SRPTableEditor();
      this.panel1 = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.panel2 = new Panel();
      this.pnlTemplate.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.pnlTemplate.Controls.Add((Control) this.txtDescription);
      this.pnlTemplate.Controls.Add((Control) this.label2);
      this.pnlTemplate.Controls.Add((Control) this.txtName);
      this.pnlTemplate.Controls.Add((Control) this.label1);
      this.pnlTemplate.Dock = DockStyle.Top;
      this.pnlTemplate.Location = new Point(0, 0);
      this.pnlTemplate.Name = "pnlTemplate";
      this.pnlTemplate.Size = new Size(439, 101);
      this.pnlTemplate.TabIndex = 1;
      this.txtDescription.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDescription.Location = new Point(81, 37);
      this.txtDescription.MaxLength = 5000;
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(345, 56);
      this.txtDescription.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 40);
      this.label2.Name = "label2";
      this.label2.Size = new Size(64, 14);
      this.label2.TabIndex = 2;
      this.label2.Text = "Description:";
      this.txtName.Location = new Point(81, 12);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(260, 20);
      this.txtName.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(66, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Table Name:";
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.ctlEditor);
      this.groupBox1.Location = new Point(10, 5);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(418, 425);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.ctlEditor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlEditor.Location = new Point(6, 17);
      this.ctlEditor.Name = "ctlEditor";
      this.ctlEditor.ReadOnly = false;
      this.ctlEditor.Size = new Size(406, 404);
      this.ctlEditor.SRPTable = (SRPTable) null;
      this.ctlEditor.TabIndex = 0;
      this.panel1.Controls.Add((Control) this.emHelpLink1);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.btnSave);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 541);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(439, 46);
      this.panel1.TabIndex = 3;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Setup\\SRP Templates";
      this.emHelpLink1.Location = new Point(10, 14);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 17);
      this.emHelpLink1.TabIndex = 113;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(354, 15);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(274, 15);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 0;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panel2.Controls.Add((Control) this.groupBox1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 101);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(439, 440);
      this.panel2.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(439, 587);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pnlTemplate);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (SRPTemplateEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Create/Edit SRP Table";
      this.FormClosing += new FormClosingEventHandler(this.SRPTableEditor_FormClosing);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.pnlTemplate.ResumeLayout(false);
      this.pnlTemplate.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
