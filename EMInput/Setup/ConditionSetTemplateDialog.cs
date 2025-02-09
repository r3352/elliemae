// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ConditionSetTemplateDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class ConditionSetTemplateDialog : Form, IHelp
  {
    private const string className = "ConditionSetTemplateDialog";
    private static string sw = Tracing.SwOutsideLoan;
    private GroupBox groupBox1;
    private Label label3;
    private Button removeBtn;
    private Button addBtn;
    private ListBox lstTemplates;
    private Label label2;
    private Label label4;
    private TextBox descTxt;
    private TextBox nameTxt;
    private Label label5;
    private Button cancelBtn;
    private Button saveBtn;
    private System.ComponentModel.Container components;
    private ConditionSetTemplate currentSet;
    private ListBox lstSelected;
    private ConditionTrackingSetup condSetup;
    private EMHelpLink emHelpLink1;
    private TemplateSettingsType templateType;
    private Panel pnlAddRemove;
    private Panel pnlAllCond;
    private Panel panel3;
    private Sessions.Session session;

    public ConditionSetTemplateDialog(
      ConditionSetTemplate currentSet,
      bool readOnly,
      Sessions.Session session)
      : this(currentSet, session)
    {
      if (!readOnly)
        return;
      this.nameTxt.ReadOnly = true;
      this.descTxt.ReadOnly = true;
      this.addBtn.Enabled = false;
      this.removeBtn.Enabled = false;
      this.saveBtn.Enabled = false;
      this.cancelBtn.Text = "Close";
      this.AcceptButton = (IButtonControl) this.cancelBtn;
      this.Text = "Condition Set Template";
    }

    public ConditionSetTemplateDialog(ConditionSetTemplate currentSet, Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.emHelpLink1.AssignSession(session);
      this.currentSet = currentSet;
      this.condSetup = this.session.ConfigurationManager.GetConditionTrackingSetup(currentSet.ConditionType);
      this.templateType = TemplateSettingsTypeConverter.FromConditionType(currentSet.ConditionType);
      this.nameTxt.Text = this.currentSet.TemplateName;
      this.descTxt.Text = this.currentSet.Description;
      this.loadAllConditionsList();
      this.loadSelectedItemList();
      if (currentSet.ConditionType != ConditionType.PostClosing)
        return;
      this.Text = this.Text.Replace("Underwriting", "Post-Closing");
      this.emHelpLink1.HelpTag = "PostClosingConditionSetTemplateDialog";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupBox1 = new GroupBox();
      this.lstSelected = new ListBox();
      this.label3 = new Label();
      this.removeBtn = new Button();
      this.addBtn = new Button();
      this.lstTemplates = new ListBox();
      this.label2 = new Label();
      this.label4 = new Label();
      this.descTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.label5 = new Label();
      this.cancelBtn = new Button();
      this.saveBtn = new Button();
      this.emHelpLink1 = new EMHelpLink();
      this.pnlAllCond = new Panel();
      this.pnlAddRemove = new Panel();
      this.panel3 = new Panel();
      this.groupBox1.SuspendLayout();
      this.pnlAllCond.SuspendLayout();
      this.pnlAddRemove.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBox1.Controls.Add((Control) this.panel3);
      this.groupBox1.Controls.Add((Control) this.pnlAddRemove);
      this.groupBox1.Controls.Add((Control) this.pnlAllCond);
      this.groupBox1.Location = new Point(12, 112);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(568, 388);
      this.groupBox1.TabIndex = 20;
      this.groupBox1.TabStop = false;
      this.groupBox1.Resize += new EventHandler(this.groupBox1_Resize);
      this.lstSelected.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lstSelected.HorizontalScrollbar = true;
      this.lstSelected.IntegralHeight = false;
      this.lstSelected.Location = new Point(6, 34);
      this.lstSelected.Name = "lstSelected";
      this.lstSelected.SelectionMode = SelectionMode.MultiSimple;
      this.lstSelected.Size = new Size(220, 326);
      this.lstSelected.Sorted = true;
      this.lstSelected.TabIndex = 26;
      this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(3, 10);
      this.label3.Name = "label3";
      this.label3.Size = new Size(212, 23);
      this.label3.TabIndex = 25;
      this.label3.Text = "Selected Conditions";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.removeBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.removeBtn.Location = new Point(5, 189);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(72, 23);
      this.removeBtn.TabIndex = 5;
      this.removeBtn.Text = "<- Remove";
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.addBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.addBtn.Location = new Point(5, 160);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(72, 23);
      this.addBtn.TabIndex = 4;
      this.addBtn.Text = "Add ->";
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.lstTemplates.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lstTemplates.HorizontalScrollbar = true;
      this.lstTemplates.IntegralHeight = false;
      this.lstTemplates.Location = new Point(15, 34);
      this.lstTemplates.Name = "lstTemplates";
      this.lstTemplates.SelectionMode = SelectionMode.MultiSimple;
      this.lstTemplates.Size = new Size(220, 326);
      this.lstTemplates.Sorted = true;
      this.lstTemplates.TabIndex = 3;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(19, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(204, 23);
      this.label2.TabIndex = 20;
      this.label2.Text = "All Conditions";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 38);
      this.label4.Name = "label4";
      this.label4.Size = new Size(60, 13);
      this.label4.TabIndex = 24;
      this.label4.Text = "Description";
      this.descTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.descTxt.Location = new Point(100, 38);
      this.descTxt.Multiline = true;
      this.descTxt.Name = "descTxt";
      this.descTxt.ScrollBars = ScrollBars.Both;
      this.descTxt.Size = new Size(480, 74);
      this.descTxt.TabIndex = 2;
      this.nameTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.nameTxt.Location = new Point(100, 12);
      this.nameTxt.MaxLength = 256;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.Size = new Size(480, 20);
      this.nameTxt.TabIndex = 1;
      this.nameTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 15);
      this.label5.Name = "label5";
      this.label5.Size = new Size(82, 13);
      this.label5.TabIndex = 23;
      this.label5.Text = "Template Name";
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(504, 512);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 4;
      this.cancelBtn.Text = "&Cancel";
      this.saveBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.saveBtn.Location = new Point(424, 512);
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(75, 24);
      this.saveBtn.TabIndex = 3;
      this.saveBtn.Text = "&Save";
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "UnderwritingConditionSetTemplateDialog";
      this.emHelpLink1.Location = new Point(12, 516);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 25;
      this.pnlAllCond.Controls.Add((Control) this.label2);
      this.pnlAllCond.Controls.Add((Control) this.lstTemplates);
      this.pnlAllCond.Dock = DockStyle.Left;
      this.pnlAllCond.Location = new Point(3, 16);
      this.pnlAllCond.Name = "pnlAllCond";
      this.pnlAllCond.Size = new Size(241, 369);
      this.pnlAllCond.TabIndex = 27;
      this.pnlAddRemove.Controls.Add((Control) this.addBtn);
      this.pnlAddRemove.Controls.Add((Control) this.removeBtn);
      this.pnlAddRemove.Dock = DockStyle.Left;
      this.pnlAddRemove.Location = new Point(244, 16);
      this.pnlAddRemove.Name = "pnlAddRemove";
      this.pnlAddRemove.Size = new Size(82, 369);
      this.pnlAddRemove.TabIndex = 28;
      this.panel3.Controls.Add((Control) this.label3);
      this.panel3.Controls.Add((Control) this.lstSelected);
      this.panel3.Dock = DockStyle.Fill;
      this.panel3.Location = new Point(326, 16);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(239, 369);
      this.panel3.TabIndex = 29;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(596, 549);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.saveBtn);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.descTxt);
      this.Controls.Add((Control) this.nameTxt);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.groupBox1);
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (ConditionSetTemplateDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Underwriting Condition Set Details";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.groupBox1.ResumeLayout(false);
      this.pnlAllCond.ResumeLayout(false);
      this.pnlAddRemove.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void loadAllConditionsList()
    {
      foreach (object obj in (CollectionBase) this.condSetup)
        this.lstTemplates.Items.Add(obj);
    }

    private void loadSelectedItemList()
    {
      foreach (string condition in this.currentSet.Conditions)
      {
        object byId = (object) this.condSetup.GetByID(condition);
        if (byId != null)
        {
          this.lstTemplates.Items.Remove(byId);
          this.lstSelected.Items.Add(byId);
        }
      }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      foreach (object obj in new ArrayList((ICollection) this.lstTemplates.SelectedItems))
      {
        this.lstSelected.Items.Add(obj);
        this.lstTemplates.Items.Remove(obj);
      }
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      foreach (object obj in new ArrayList((ICollection) this.lstSelected.SelectedItems))
      {
        this.lstSelected.Items.Remove(obj);
        this.lstTemplates.Items.Add(obj);
      }
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      string str1 = this.nameTxt.Text.Trim();
      if (str1 == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a name for this condition set.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.nameTxt.Focus();
      }
      else
      {
        FileSystemEntry fileSystemEntry = FileSystemEntry.Parse("public:\\" + str1);
        string str2 = (string) null;
        if (str1.ToUpper() != this.currentSet.TemplateName.ToUpper())
        {
          str2 = this.currentSet.TemplateName;
          if (this.session.ConfigurationManager.TemplateSettingsObjectExists(this.templateType, fileSystemEntry))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A condition set already exists with the name '" + str1 + "'. Please use a different name.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.nameTxt.Focus();
            return;
          }
        }
        this.currentSet.TemplateName = str1;
        this.currentSet.Description = this.descTxt.Text.Trim();
        this.currentSet.Conditions.Clear();
        foreach (IIdentifiable identifiable in this.lstSelected.Items)
          this.currentSet.Conditions.Add((object) identifiable.Guid);
        this.session.ConfigurationManager.SaveTemplateSettings(this.templateType, fileSystemEntry, (BinaryObject) (BinaryConvertibleObject) this.currentSet);
        if ((str2 ?? "") != "")
          this.session.ConfigurationManager.DeleteTemplateSettingsObject(this.templateType, FileSystemEntry.Parse("public:\\" + str2));
        this.DialogResult = DialogResult.OK;
      }
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      char keyChar = e.KeyChar;
      if (!keyChar.Equals('\\'))
      {
        keyChar = e.KeyChar;
        if (!keyChar.Equals('"'))
          return;
      }
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
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.templateType.ToString() + "TemplateDialog");
    }

    private void groupBox1_Resize(object sender, EventArgs e)
    {
      this.pnlAllCond.Size = new Size((this.groupBox1.Width - this.pnlAddRemove.Width) / 2, this.groupBox1.Height);
    }
  }
}
