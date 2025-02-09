// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PersonaNameDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PersonaNameDlg : Form
  {
    private Sessions.Session session;
    private Label label1;
    private TextBox textBoxPersonaName;
    private Button btnOK;
    private Button button1;
    private Label label2;
    private RadioButton rbtnAccessToNone;
    private RadioButton rbtnAccessToAll;
    private Label label3;
    private Label label4;
    private Panel panelPersonaType;
    private CheckBox chkInternal;
    private CheckBox chkExternal;
    private bool canAccessPersonaType;
    private bool clonePersona;
    private System.ComponentModel.Container components;

    public string PersonaName => this.textBoxPersonaName.Text;

    public bool SettingsDefault => this.rbtnAccessToAll.Checked;

    public bool IsInternal => this.chkInternal.Checked;

    public bool IsExternal => this.chkExternal.Checked;

    public PersonaNameDlg(Sessions.Session session, bool clonePersona)
    {
      this.session = session;
      this.clonePersona = clonePersona;
      this.InitializeComponent();
      this.canAccessPersonaType = session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      this.panelPersonaType.Visible = this.canAccessPersonaType;
      if (!this.canAccessPersonaType)
        this.Height -= this.panelPersonaType.Height;
      if (clonePersona)
      {
        this.label2.Enabled = false;
        this.rbtnAccessToAll.Checked = false;
        this.rbtnAccessToAll.Enabled = false;
        this.rbtnAccessToNone.Enabled = false;
        this.rbtnAccessToNone.Checked = false;
        this.panelPersonaType.Enabled = false;
      }
      else
        this.btnOK.Enabled = false;
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
      this.textBoxPersonaName = new TextBox();
      this.btnOK = new Button();
      this.button1 = new Button();
      this.label2 = new Label();
      this.rbtnAccessToNone = new RadioButton();
      this.rbtnAccessToAll = new RadioButton();
      this.label3 = new Label();
      this.label4 = new Label();
      this.panelPersonaType = new Panel();
      this.chkExternal = new CheckBox();
      this.chkInternal = new CheckBox();
      this.panelPersonaType.SuspendLayout();
      this.SuspendLayout();
      this.label1.Location = new Point(12, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(80, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Persona Name";
      this.textBoxPersonaName.Location = new Point(96, 12);
      this.textBoxPersonaName.MaxLength = 64;
      this.textBoxPersonaName.Name = "textBoxPersonaName";
      this.textBoxPersonaName.Size = new Size(236, 20);
      this.textBoxPersonaName.TabIndex = 1;
      this.textBoxPersonaName.TextChanged += new EventHandler(this.textBoxPersonaName_TextChanged);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(204, 192);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(60, 24);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.button1.DialogResult = DialogResult.Cancel;
      this.button1.Location = new Point(272, 192);
      this.button1.Name = "button1";
      this.button1.Size = new Size(60, 24);
      this.button1.TabIndex = 3;
      this.button1.Text = "Cancel";
      this.label2.Location = new Point(12, 44);
      this.label2.Name = "label2";
      this.label2.Size = new Size(92, 16);
      this.label2.TabIndex = 5;
      this.label2.Text = "Start with:";
      this.rbtnAccessToNone.Checked = true;
      this.rbtnAccessToNone.Location = new Point(20, 60);
      this.rbtnAccessToNone.Name = "rbtnAccessToNone";
      this.rbtnAccessToNone.Size = new Size(192, 16);
      this.rbtnAccessToNone.TabIndex = 6;
      this.rbtnAccessToNone.TabStop = true;
      this.rbtnAccessToNone.Text = "Access to No Features";
      this.rbtnAccessToAll.Location = new Point(20, 76);
      this.rbtnAccessToAll.Name = "rbtnAccessToAll";
      this.rbtnAccessToAll.Size = new Size(192, 16);
      this.rbtnAccessToAll.TabIndex = 7;
      this.rbtnAccessToAll.Text = "Access to All Features";
      this.label3.Anchor = AnchorStyles.Bottom;
      this.label3.BorderStyle = BorderStyle.Fixed3D;
      this.label3.Location = new Point(12, 180);
      this.label3.Name = "label3";
      this.label3.Size = new Size(328, 2);
      this.label3.TabIndex = 8;
      this.label4.Location = new Point(9, 6);
      this.label4.Name = "label4";
      this.label4.Size = new Size(92, 16);
      this.label4.TabIndex = 10;
      this.label4.Text = "Persona Type:";
      this.panelPersonaType.Controls.Add((Control) this.chkExternal);
      this.panelPersonaType.Controls.Add((Control) this.chkInternal);
      this.panelPersonaType.Controls.Add((Control) this.label4);
      this.panelPersonaType.Location = new Point(4, 99);
      this.panelPersonaType.Name = "panelPersonaType";
      this.panelPersonaType.Size = new Size(328, 74);
      this.panelPersonaType.TabIndex = 10;
      this.chkExternal.AutoSize = true;
      this.chkExternal.Location = new Point(17, 50);
      this.chkExternal.Name = "chkExternal";
      this.chkExternal.Size = new Size(64, 17);
      this.chkExternal.TabIndex = 12;
      this.chkExternal.Text = "External";
      this.chkExternal.UseVisualStyleBackColor = true;
      this.chkInternal.AutoSize = true;
      this.chkInternal.Location = new Point(17, 29);
      this.chkInternal.Name = "chkInternal";
      this.chkInternal.Size = new Size(61, 17);
      this.chkInternal.TabIndex = 11;
      this.chkInternal.Text = "Internal";
      this.chkInternal.UseVisualStyleBackColor = true;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(346, 220);
      this.Controls.Add((Control) this.panelPersonaType);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.rbtnAccessToAll);
      this.Controls.Add((Control) this.rbtnAccessToNone);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.button1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.textBoxPersonaName);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PersonaNameDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Create a Persona";
      this.panelPersonaType.ResumeLayout(false);
      this.panelPersonaType.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.PersonaName.ToLower().Replace(" ", "") == "administrator" || this.PersonaName.ToLower().Replace(" ", "") == "superadministrator")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The selected persona name is a reserved key word. Please enter a different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.session.PersonaManager.PersonaNameExists(this.PersonaName))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The persona name already exists. Please enter a different name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.clonePersona && this.canAccessPersonaType && !this.chkInternal.Checked && !this.chkExternal.Checked)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Please select a persona type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!this.canAccessPersonaType && !this.clonePersona)
          this.chkInternal.Checked = true;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void textBoxPersonaName_TextChanged(object sender, EventArgs e)
    {
      if (this.textBoxPersonaName.Text.Length > 0)
        this.btnOK.Enabled = true;
      else if (this.textBoxPersonaName.Text.Length > 64)
        this.btnOK.Enabled = false;
      else
        this.btnOK.Enabled = false;
    }
  }
}
