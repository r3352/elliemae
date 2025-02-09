// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ViewContactGroupDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ContactGroup;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ViewContactGroupDialog : Form
  {
    private EllieMae.EMLite.ContactGroup.ContactGroup contactGroup;
    private ContactQuery contactQuery;
    private Panel pnlButtons;
    private Label lblSeparator;
    private Button btnOK;
    private Panel pnlListControl;
    private Panel pnlSelection;
    private Label lblContactGroup;
    private ComboBox cboContactGroup;
    private System.ComponentModel.Container components;

    public ViewContactGroupDialog(EllieMae.EMLite.ContactGroup.ContactGroup contactGroup)
    {
      this.contactGroup = contactGroup;
      this.InitializeComponent();
      this.Text = "Contact Group";
      this.displayViewContactGroupControl();
    }

    public ViewContactGroupDialog(ContactQuery contactQuery)
    {
      this.contactQuery = contactQuery;
      this.InitializeComponent();
      this.Text = "Query Results";
      this.displayViewContactGroupControl();
    }

    private void displayViewContactGroupControl()
    {
      if (this.contactGroup == null && this.contactQuery == null)
        return;
      ViewContactGroupControl contactGroupControl = this.contactGroup != null ? new ViewContactGroupControl(this.contactGroup) : new ViewContactGroupControl(this.contactQuery);
      this.pnlSelection.Visible = false;
      contactGroupControl.Dock = DockStyle.Fill;
      this.pnlListControl.Controls.Add((Control) contactGroupControl);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnOK_Click(object sender, EventArgs e) => this.Close();

    private void InitializeComponent()
    {
      this.pnlButtons = new Panel();
      this.lblSeparator = new Label();
      this.btnOK = new Button();
      this.pnlListControl = new Panel();
      this.pnlSelection = new Panel();
      this.lblContactGroup = new Label();
      this.cboContactGroup = new ComboBox();
      this.pnlButtons.SuspendLayout();
      this.pnlListControl.SuspendLayout();
      this.pnlSelection.SuspendLayout();
      this.SuspendLayout();
      this.pnlButtons.Controls.Add((Control) this.lblSeparator);
      this.pnlButtons.Controls.Add((Control) this.btnOK);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 319);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(671, 32);
      this.pnlButtons.TabIndex = 1;
      this.lblSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblSeparator.Location = new Point(7, 0);
      this.lblSeparator.Name = "lblSeparator";
      this.lblSeparator.Size = new Size(657, 1);
      this.lblSeparator.TabIndex = 2;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(589, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.pnlListControl.Controls.Add((Control) this.pnlSelection);
      this.pnlListControl.Dock = DockStyle.Fill;
      this.pnlListControl.Location = new Point(0, 0);
      this.pnlListControl.Name = "pnlListControl";
      this.pnlListControl.Size = new Size(671, 319);
      this.pnlListControl.TabIndex = 2;
      this.pnlSelection.Controls.Add((Control) this.lblContactGroup);
      this.pnlSelection.Controls.Add((Control) this.cboContactGroup);
      this.pnlSelection.Dock = DockStyle.Top;
      this.pnlSelection.Location = new Point(0, 0);
      this.pnlSelection.Name = "pnlSelection";
      this.pnlSelection.Size = new Size(671, 32);
      this.pnlSelection.TabIndex = 0;
      this.lblContactGroup.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblContactGroup.Location = new Point(7, 4);
      this.lblContactGroup.Name = "lblContactGroup";
      this.lblContactGroup.Size = new Size(89, 23);
      this.lblContactGroup.TabIndex = 44;
      this.lblContactGroup.Text = "Contact Group:";
      this.lblContactGroup.TextAlign = ContentAlignment.MiddleLeft;
      this.cboContactGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboContactGroup.Location = new Point(96, 5);
      this.cboContactGroup.Name = "cboContactGroup";
      this.cboContactGroup.Size = new Size(288, 21);
      this.cboContactGroup.Sorted = true;
      this.cboContactGroup.TabIndex = 43;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(671, 351);
      this.Controls.Add((Control) this.pnlListControl);
      this.Controls.Add((Control) this.pnlButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ViewContactGroupDialog);
      this.ShowInTaskbar = false;
      this.Text = "Contact Group Membership";
      this.pnlButtons.ResumeLayout(false);
      this.pnlListControl.ResumeLayout(false);
      this.pnlSelection.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
