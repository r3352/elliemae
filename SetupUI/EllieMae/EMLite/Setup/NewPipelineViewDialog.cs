// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.NewPipelineViewDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class NewPipelineViewDialog : Form
  {
    private Sessions.Session session;
    private int personaID;
    private PipelineViewAclManager viewAclMgr;
    private IContainer components;
    private RadioButton radNew;
    private Label label1;
    private ComboBox cboPersona;
    private RadioButton radDuplicate;
    private Label label2;
    private Label label3;
    private ComboBox cboView;
    private DialogButtons dlgButtons;

    public NewPipelineViewDialog(Sessions.Session session, int personaID)
    {
      this.session = session;
      this.personaID = personaID;
      this.InitializeComponent();
      this.viewAclMgr = (PipelineViewAclManager) session.ACL.GetAclManager(AclCategory.PersonaPipelineView);
      this.loadPersonas();
    }

    public PersonaPipelineView GetPipelineView()
    {
      return this.radNew.Checked ? new PersonaPipelineView(this.personaID, "New View") : ((PersonaPipelineView) this.cboView.SelectedItem).Duplicate(this.personaID);
    }

    private void loadPersonas()
    {
      Persona[] allPersonas = this.session.PersonaManager.GetAllPersonas();
      Persona persona1 = (Persona) null;
      foreach (Persona persona2 in allPersonas)
      {
        this.cboPersona.Items.Add((object) persona2);
        if (persona2.ID == this.personaID)
          persona1 = persona2;
      }
      if (!(persona1 != (Persona) null))
        return;
      this.cboPersona.SelectedItem = (object) persona1;
    }

    private void cboPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
      PersonaPipelineView[] personaPipelineViews = this.viewAclMgr.GetPersonaPipelineViews(((Persona) this.cboPersona.SelectedItem).ID);
      this.cboView.Items.Clear();
      foreach (object obj in personaPipelineViews)
        this.cboView.Items.Add(obj);
      if (this.cboView.Items.Count <= 0)
        return;
      this.cboView.SelectedIndex = 0;
    }

    private void radDuplicate_CheckedChanged(object sender, EventArgs e)
    {
      this.cboPersona.Enabled = this.radDuplicate.Checked;
      this.cboView.Enabled = this.radDuplicate.Checked;
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      if (this.radDuplicate.Checked && this.cboView.SelectedIndex < 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select the Persona and View to use as the starting point for the new Pipeline View.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.radNew = new RadioButton();
      this.label1 = new Label();
      this.cboPersona = new ComboBox();
      this.radDuplicate = new RadioButton();
      this.label2 = new Label();
      this.label3 = new Label();
      this.cboView = new ComboBox();
      this.dlgButtons = new DialogButtons();
      this.SuspendLayout();
      this.radNew.AutoSize = true;
      this.radNew.Checked = true;
      this.radNew.Location = new Point(10, 52);
      this.radNew.Name = "radNew";
      this.radNew.Size = new Size(194, 18);
      this.radNew.TabIndex = 0;
      this.radNew.TabStop = true;
      this.radNew.Text = "Create a new, empty Pipeline View";
      this.radNew.UseVisualStyleBackColor = true;
      this.label1.Location = new Point(8, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(264, 32);
      this.label1.TabIndex = 1;
      this.label1.Text = "Select whether you want to create a new, empty view or copy an existing pipeline view:";
      this.cboPersona.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPersona.Enabled = false;
      this.cboPersona.FormattingEnabled = true;
      this.cboPersona.Location = new Point(79, 102);
      this.cboPersona.Name = "cboPersona";
      this.cboPersona.Size = new Size(188, 22);
      this.cboPersona.TabIndex = 2;
      this.cboPersona.SelectedIndexChanged += new EventHandler(this.cboPersona_SelectedIndexChanged);
      this.radDuplicate.AutoSize = true;
      this.radDuplicate.Location = new Point(10, 78);
      this.radDuplicate.Name = "radDuplicate";
      this.radDuplicate.Size = new Size(212, 18);
      this.radDuplicate.TabIndex = 3;
      this.radDuplicate.Text = "Create using an existing Pipeline View:";
      this.radDuplicate.UseVisualStyleBackColor = true;
      this.radDuplicate.CheckedChanged += new EventHandler(this.radDuplicate_CheckedChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(27, 107);
      this.label2.Name = "label2";
      this.label2.Size = new Size(47, 14);
      this.label2.TabIndex = 4;
      this.label2.Text = "Persona";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(27, 131);
      this.label3.Name = "label3";
      this.label3.Size = new Size(33, 14);
      this.label3.TabIndex = 6;
      this.label3.Text = "View";
      this.cboView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboView.Enabled = false;
      this.cboView.FormattingEnabled = true;
      this.cboView.Location = new Point(79, 126);
      this.cboView.Name = "cboView";
      this.cboView.Size = new Size(188, 22);
      this.cboView.TabIndex = 5;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 162);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(284, 44);
      this.dlgButtons.TabIndex = 7;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(284, 206);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.cboView);
      this.Controls.Add((Control) this.cboPersona);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.radDuplicate);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.radNew);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NewPipelineViewDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "New Pipeline View";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
