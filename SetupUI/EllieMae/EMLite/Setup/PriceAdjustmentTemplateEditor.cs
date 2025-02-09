// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PriceAdjustmentTemplateEditor
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
  public class PriceAdjustmentTemplateEditor : Form
  {
    private PriceAdjustmentTemplate template;
    private bool modified;
    private Sessions.Session session;
    private IContainer components;
    private Label label1;
    private Label label2;
    private Button btnCancel;
    private Button btnSave;
    private PriceAdjustmentListEditor ctlEditor;
    private TextBox txtName;
    private TextBox txtDescription;
    protected EMHelpLink emHelpLink1;

    public PriceAdjustmentTemplateEditor(Sessions.Session session)
      : this(new PriceAdjustmentTemplate(), session)
    {
    }

    public PriceAdjustmentTemplateEditor()
      : this(Session.DefaultInstance)
    {
    }

    public PriceAdjustmentTemplateEditor(PriceAdjustmentTemplate template)
      : this(template, Session.DefaultInstance)
    {
    }

    public PriceAdjustmentTemplateEditor(PriceAdjustmentTemplate template, Sessions.Session session)
    {
      this.session = session;
      this.ctlEditor = new PriceAdjustmentListEditor(this.session);
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(this.session);
      this.template = template;
      this.loadTemplateData();
    }

    public PriceAdjustmentTemplate CurrentTemplate => this.template;

    private void loadTemplateData()
    {
      this.txtName.Text = this.template.TemplateName;
      this.txtDescription.Text = this.template.Description;
      this.ctlEditor.Adjustments = this.template.PriceAdjustments;
      this.modified = false;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (!this.validateTemplate())
        return;
      this.ctlEditor.CommitChanges();
      this.template.Description = this.txtDescription.Text;
      if (!this.saveTemplate())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private bool saveTemplate()
    {
      string templateName = this.template.TemplateName;
      string entryName = this.txtName.Text.Trim();
      if (templateName != "" && templateName != entryName)
        this.session.ConfigurationManager.DeleteTemplateSettingsObject(TemplateSettingsType.TradePriceAdjustment, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, templateName, FileSystemEntry.Types.File, (string) null));
      this.template.TemplateName = entryName;
      this.session.ConfigurationManager.SaveTemplateSettings(TemplateSettingsType.TradePriceAdjustment, new FileSystemEntry(FileSystemEntry.PublicRoot.Path, entryName, FileSystemEntry.Types.File, (string) null), (BinaryObject) (BinaryConvertibleObject) this.template);
      this.modified = false;
      return true;
    }

    private bool validateTemplate()
    {
      string str = this.txtName.Text.Trim();
      if (str == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A name must be provided for this template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      FileSystemEntry fileEntry = new FileSystemEntry(FileSystemEntry.PublicRoot.Path, str, FileSystemEntry.Types.File, (string) null);
      if (string.Compare(str, this.template.TemplateName, true) == 0 || !this.session.ConfigurationManager.TemplateSettingsObjectExists(TemplateSettingsType.TradePriceAdjustment, fileEntry))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The name '" + str + "' is already in use. Select a new name and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void txtName_TextChanged(object sender, EventArgs e) => this.modified = true;

    private void ctlEditor_DataModified(object sender, EventArgs e) => this.modified = true;

    private void PriceAdjustmentTemplateEditor_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.modified || this.DialogResult != DialogResult.Cancel || Utils.Dialog((IWin32Window) this, "There are unsaved changes to this template which will be lost.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
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
      this.label1 = new Label();
      this.label2 = new Label();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.txtName = new TextBox();
      this.txtDescription = new TextBox();
      this.emHelpLink1 = new EMHelpLink();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Name";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 37);
      this.label2.Name = "label2";
      this.label2.Size = new Size(61, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Description";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(445, 374);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Location = new Point(366, 374);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 4;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.txtName.Location = new Point(96, 11);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(282, 20);
      this.txtName.TabIndex = 1;
      this.txtName.TextChanged += new EventHandler(this.txtName_TextChanged);
      this.txtDescription.Location = new Point(96, 33);
      this.txtDescription.MaxLength = 5000;
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ScrollBars = ScrollBars.Vertical;
      this.txtDescription.Size = new Size(424, 54);
      this.txtDescription.TabIndex = 2;
      this.txtDescription.TextChanged += new EventHandler(this.txtName_TextChanged);
      this.ctlEditor.Adjustments = (TradePriceAdjustments) null;
      this.ctlEditor.Location = new Point(12, 92);
      this.ctlEditor.Name = "ctlEditor";
      this.ctlEditor.ReadOnly = false;
      this.ctlEditor.Size = new Size(508, 267);
      this.ctlEditor.TabIndex = 3;
      this.ctlEditor.DataChange += new EventHandler(this.ctlEditor_DataModified);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Setup\\Adjustment Templates";
      this.emHelpLink1.Location = new Point(12, 364);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 17);
      this.emHelpLink1.TabIndex = 112;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(533, 410);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.ctlEditor);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PriceAdjustmentTemplateEditor);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Create/Edit Template";
      this.FormClosing += new FormClosingEventHandler(this.PriceAdjustmentTemplateEditor_FormClosing);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
