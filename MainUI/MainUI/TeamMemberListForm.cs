// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.TeamMemberListForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class TeamMemberListForm : Form
  {
    private System.ComponentModel.Container components;
    private Panel panelTop;
    private Label labelFieldID;
    private PictureBox pictureBox1;
    private ListView listViewMembers;
    private ColumnHeader columnHeader1;
    private ColumnHeader columnHeader2;
    private Panel panelMain;
    private Panel panel1;
    private Label label1;
    private Panel panelShadow;
    private ListView listViewMember;
    private ColumnHeader headerAbbr;
    private ColumnHeader headerName;
    private PictureBox pictureBoxCancel;
    private LoanData loan;
    private string memberAbbr;
    private LoanAssociateLog selectedLoanAssociate;

    public TeamMemberListForm(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.initForm();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string MemberAbbr => this.memberAbbr;

    public LoanAssociateLog SelectedLoanAssociate => this.selectedLoanAssociate;

    private void InitializeComponent()
    {
      ResourceManager resourceManager = new ResourceManager(typeof (TeamMemberListForm));
      this.panelTop = new Panel();
      this.labelFieldID = new Label();
      this.pictureBox1 = new PictureBox();
      this.listViewMembers = new ListView();
      this.columnHeader1 = new ColumnHeader();
      this.columnHeader2 = new ColumnHeader();
      this.panelMain = new Panel();
      this.listViewMember = new ListView();
      this.headerAbbr = new ColumnHeader();
      this.headerName = new ColumnHeader();
      this.panel1 = new Panel();
      this.label1 = new Label();
      this.pictureBoxCancel = new PictureBox();
      this.panelShadow = new Panel();
      this.panelMain.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.TabIndex = 0;
      this.labelFieldID.Location = new Point(0, 0);
      this.labelFieldID.Name = "labelFieldID";
      this.labelFieldID.TabIndex = 0;
      this.pictureBox1.Location = new Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.listViewMembers.Location = new Point(0, 0);
      this.listViewMembers.Name = "listViewMembers";
      this.listViewMembers.TabIndex = 0;
      this.panelMain.BackColor = Color.White;
      this.panelMain.BorderStyle = BorderStyle.FixedSingle;
      this.panelMain.Controls.Add((Control) this.listViewMember);
      this.panelMain.Controls.Add((Control) this.panel1);
      this.panelMain.Location = new Point(4, 4);
      this.panelMain.Name = "panelMain";
      this.panelMain.Size = new Size(156, 195);
      this.panelMain.TabIndex = 6;
      this.listViewMember.BorderStyle = BorderStyle.None;
      this.listViewMember.Columns.AddRange(new ColumnHeader[2]
      {
        this.headerAbbr,
        this.headerName
      });
      this.listViewMember.Dock = DockStyle.Fill;
      this.listViewMember.FullRowSelect = true;
      this.listViewMember.Location = new Point(0, 20);
      this.listViewMember.MultiSelect = false;
      this.listViewMember.Name = "listViewMember";
      this.listViewMember.Size = new Size(154, 173);
      this.listViewMember.TabIndex = 2;
      this.listViewMember.View = View.Details;
      this.listViewMember.KeyPress += new KeyPressEventHandler(this.listViewMember_KeyPress);
      this.listViewMember.MouseUp += new MouseEventHandler(this.listViewMember_MouseUp);
      this.headerAbbr.Text = "Abbr";
      this.headerAbbr.Width = 40;
      this.headerName.Text = "Name";
      this.headerName.Width = 113;
      this.panel1.BackgroundImage = (Image) resourceManager.GetObject("panel1.BackgroundImage");
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.pictureBoxCancel);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(154, 20);
      this.panel1.TabIndex = 1;
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(2, 2);
      this.label1.Name = "label1";
      this.label1.Size = new Size(134, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Loan Team Members";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.pictureBoxCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pictureBoxCancel.BorderStyle = BorderStyle.FixedSingle;
      this.pictureBoxCancel.Image = (Image) resourceManager.GetObject("pictureBoxCancel.Image");
      this.pictureBoxCancel.Location = new Point(139, 3);
      this.pictureBoxCancel.Name = "pictureBoxCancel";
      this.pictureBoxCancel.Size = new Size(13, 13);
      this.pictureBoxCancel.TabIndex = 0;
      this.pictureBoxCancel.TabStop = false;
      this.pictureBoxCancel.Click += new EventHandler(this.pictureBoxCancel_Click);
      this.panelShadow.BackColor = Color.DarkGray;
      this.panelShadow.Location = new Point(8, 8);
      this.panelShadow.Name = "panelShadow";
      this.panelShadow.Size = new Size(156, 195);
      this.panelShadow.TabIndex = 5;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.Gainsboro;
      this.ClientSize = new Size(172, 208);
      this.Controls.Add((Control) this.panelMain);
      this.Controls.Add((Control) this.panelShadow);
      this.FormBorderStyle = FormBorderStyle.None;
      this.KeyPreview = true;
      this.Name = nameof (TeamMemberListForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.Manual;
      this.Text = nameof (TeamMemberListForm);
      this.TransparencyKey = Color.Gainsboro;
      this.panelMain.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      this.listViewMember.Items.Clear();
      ArrayList arrayList = new ArrayList();
      arrayList.AddRange((ICollection) this.loan.GetLogList().GetAllMilestones());
      arrayList.AddRange((ICollection) this.loan.GetLogList().GetAllMilestoneFreeRoles());
      LoanAssociateLog[] array = (LoanAssociateLog[]) arrayList.ToArray(typeof (LoanAssociateLog));
      if (array.Length == 0)
        return;
      RoleInfo[] allRoles = Session.LoanDataMgr.SystemConfiguration.AllRoles;
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (RoleInfo roleInfo in allRoles)
        insensitiveHashtable[(object) roleInfo.Name] = (object) roleInfo.RoleAbbr;
      bool flag = false;
      string str1 = string.Empty;
      this.listViewMember.BeginUpdate();
      foreach (LoanAssociateLog loanAssociateLog in array)
      {
        string text = loanAssociateLog.RoleID != RoleInfo.FileStarter.RoleID ? string.Concat(insensitiveHashtable[(object) loanAssociateLog.RoleName]) : RoleInfo.FileStarter.RoleAbbr;
        if ((loanAssociateLog.LoanAssociateID ?? "") != "" && text != "")
        {
          string str2 = loanAssociateLog.LoanAssociateName + " (" + loanAssociateLog.LoanAssociateID + ")";
          if (loanAssociateLog is MilestoneLog && !((MilestoneLog) loanAssociateLog).Done && !flag)
          {
            flag = true;
            str1 = str2 + " *";
          }
          this.listViewMember.Items.Add(new ListViewItem(text)
          {
            SubItems = {
              loanAssociateLog.LoanAssociateName
            },
            Tag = (object) loanAssociateLog
          });
        }
      }
      this.listViewMember.EndUpdate();
    }

    private void pictureBoxCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void listViewMember_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void listViewMember_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.listViewMember.SelectedItems.Count == 0)
      {
        this.DialogResult = DialogResult.Cancel;
      }
      else
      {
        this.selectedLoanAssociate = (LoanAssociateLog) this.listViewMember.SelectedItems[0].Tag;
        this.memberAbbr = this.listViewMember.SelectedItems[0].Text;
        this.DialogResult = DialogResult.OK;
      }
    }
  }
}
