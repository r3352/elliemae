// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ConfirmReinsertDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactGroup;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ConfirmReinsertDialog : Form
  {
    private ContactGroupMember groupMember;
    private ConfirmReinsertDialog.SelectionTypes userSelection;
    private Panel pnlButtons;
    private Label lblButtonsSeparator;
    private Button btnYes;
    private Button btnYesAll;
    private Button btnNo;
    private Button btnNoAll;
    private Button btnView;
    private Label lblContactName;
    private Label lblInformation;
    private Panel pnlHeading;
    private Label lblHeadingSeparator;
    private Label lblContactsSelected;
    private Panel pnlMain;
    private ToolTip tipConfirmDialog;
    private IContainer components;

    public ConfirmReinsertDialog.SelectionTypes UserSelection => this.userSelection;

    public ConfirmReinsertDialog(ContactGroupMember groupMember)
    {
      this.groupMember = groupMember;
      this.InitializeComponent();
      if (groupMember == null)
        return;
      this.fitLabelText(this.lblContactName, groupMember.LastName + ", " + groupMember.FirstName);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnYes_Click(object sender, EventArgs e)
    {
      this.userSelection = ConfirmReinsertDialog.SelectionTypes.Yes;
      this.Close();
    }

    private void btnYesAll_Click(object sender, EventArgs e)
    {
      this.userSelection = ConfirmReinsertDialog.SelectionTypes.YesAll;
      this.Close();
    }

    private void btnNo_Click(object sender, EventArgs e)
    {
      this.userSelection = ConfirmReinsertDialog.SelectionTypes.No;
      this.Close();
    }

    private void btnNoAll_Click(object sender, EventArgs e)
    {
      this.userSelection = ConfirmReinsertDialog.SelectionTypes.NoAll;
      this.Close();
    }

    private void btnView_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      AttendeeInfo attendeeInfo = new AttendeeInfo(false);
      attendeeInfo.AssignInfo(this.groupMember.ContactId, this.groupMember.ContactType, false);
      Cursor.Current = Cursors.Default;
      int num = (int) attendeeInfo.ShowDialog();
    }

    private void fitLabelText(Label label, string text)
    {
      using (Graphics graphics = label.CreateGraphics())
      {
        if (Utils.FitLabelText(graphics, label, text))
          this.tipConfirmDialog.SetToolTip((Control) label, string.Empty);
        else
          this.tipConfirmDialog.SetToolTip((Control) label, Utils.FitToolTipText(graphics, label.Font, 400f, text));
      }
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.pnlButtons = new Panel();
      this.btnView = new Button();
      this.btnNoAll = new Button();
      this.btnNo = new Button();
      this.btnYesAll = new Button();
      this.btnYes = new Button();
      this.lblButtonsSeparator = new Label();
      this.lblContactName = new Label();
      this.lblInformation = new Label();
      this.pnlHeading = new Panel();
      this.lblHeadingSeparator = new Label();
      this.lblContactsSelected = new Label();
      this.pnlMain = new Panel();
      this.tipConfirmDialog = new ToolTip(this.components);
      this.pnlButtons.SuspendLayout();
      this.pnlHeading.SuspendLayout();
      this.pnlMain.SuspendLayout();
      this.SuspendLayout();
      this.pnlButtons.Controls.Add((Control) this.btnView);
      this.pnlButtons.Controls.Add((Control) this.btnNoAll);
      this.pnlButtons.Controls.Add((Control) this.btnNo);
      this.pnlButtons.Controls.Add((Control) this.btnYesAll);
      this.pnlButtons.Controls.Add((Control) this.btnYes);
      this.pnlButtons.Controls.Add((Control) this.lblButtonsSeparator);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 86);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(560, 32);
      this.pnlButtons.TabIndex = 5;
      this.btnView.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnView.Location = new Point(6, 4);
      this.btnView.Name = "btnView";
      this.btnView.TabIndex = 7;
      this.btnView.Text = "View";
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.btnNoAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNoAll.Location = new Point(479, 4);
      this.btnNoAll.Name = "btnNoAll";
      this.btnNoAll.TabIndex = 6;
      this.btnNoAll.Text = "No All";
      this.btnNoAll.Click += new EventHandler(this.btnNoAll_Click);
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.Location = new Point(400, 4);
      this.btnNo.Name = "btnNo";
      this.btnNo.TabIndex = 5;
      this.btnNo.Text = "No";
      this.btnNo.Click += new EventHandler(this.btnNo_Click);
      this.btnYesAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYesAll.Location = new Point(321, 4);
      this.btnYesAll.Name = "btnYesAll";
      this.btnYesAll.TabIndex = 4;
      this.btnYesAll.Text = "Yes All";
      this.btnYesAll.Click += new EventHandler(this.btnYesAll_Click);
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.Location = new Point(242, 4);
      this.btnYes.Name = "btnYes";
      this.btnYes.TabIndex = 3;
      this.btnYes.Text = "Yes";
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.lblButtonsSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblButtonsSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblButtonsSeparator.Location = new Point(6, 0);
      this.lblButtonsSeparator.Name = "lblButtonsSeparator";
      this.lblButtonsSeparator.Size = new Size(548, 1);
      this.lblButtonsSeparator.TabIndex = 2;
      this.lblContactName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblContactName.Location = new Point(6, 4);
      this.lblContactName.Name = "lblContactName";
      this.lblContactName.Size = new Size(548, 23);
      this.lblContactName.TabIndex = 6;
      this.lblContactName.Text = "Simpson, Homer";
      this.lblContactName.TextAlign = ContentAlignment.MiddleLeft;
      this.lblInformation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblInformation.Location = new Point(6, 24);
      this.lblInformation.Name = "lblInformation";
      this.lblInformation.Size = new Size(548, 23);
      this.lblInformation.TabIndex = 7;
      this.lblInformation.Text = "has previously participated in this campaign.  Do you want to add this contact again?";
      this.lblInformation.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlHeading.Controls.Add((Control) this.lblHeadingSeparator);
      this.pnlHeading.Controls.Add((Control) this.lblContactsSelected);
      this.pnlHeading.Dock = DockStyle.Top;
      this.pnlHeading.Location = new Point(0, 0);
      this.pnlHeading.Name = "pnlHeading";
      this.pnlHeading.Size = new Size(560, 32);
      this.pnlHeading.TabIndex = 8;
      this.lblHeadingSeparator.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblHeadingSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblHeadingSeparator.Location = new Point(6, 31);
      this.lblHeadingSeparator.Name = "lblHeadingSeparator";
      this.lblHeadingSeparator.Size = new Size(548, 1);
      this.lblHeadingSeparator.TabIndex = 5;
      this.lblContactsSelected.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblContactsSelected.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblContactsSelected.Location = new Point(6, 6);
      this.lblContactsSelected.Name = "lblContactsSelected";
      this.lblContactsSelected.Size = new Size(548, 23);
      this.lblContactsSelected.TabIndex = 4;
      this.lblContactsSelected.Text = "Confirm Campaign Participation:";
      this.lblContactsSelected.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlMain.Controls.Add((Control) this.lblContactName);
      this.pnlMain.Controls.Add((Control) this.lblInformation);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 32);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new Size(560, 54);
      this.pnlMain.TabIndex = 9;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(560, 118);
      this.Controls.Add((Control) this.pnlMain);
      this.Controls.Add((Control) this.pnlHeading);
      this.Controls.Add((Control) this.pnlButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ConfirmReinsertDialog);
      this.ShowInTaskbar = false;
      this.Text = "Encompass";
      this.pnlButtons.ResumeLayout(false);
      this.pnlHeading.ResumeLayout(false);
      this.pnlMain.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public enum SelectionTypes
    {
      Unknown,
      Yes,
      YesAll,
      No,
      NoAll,
    }
  }
}
