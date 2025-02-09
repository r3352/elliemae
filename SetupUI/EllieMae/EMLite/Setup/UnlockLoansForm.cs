// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.UnlockLoansForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class UnlockLoansForm : Form
  {
    private ComboBox folderCombo;
    private Label folderLabel;
    private IContainer components;
    private Button unlockButton;
    private GridView lvwLoan;
    private ContextMenu contextMenu1;
    private MenuItem showDetailMenuItem;
    private MenuItem showAllMenuItem;
    private Panel panel1;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton stdIconBtnRefresh;
    private ToolTip toolTip1;
    private GroupContainer gcUnlockLoan;
    private GradientPanel gradientPanel1;
    private SetUpContainer setupContainer;

    public UnlockLoansForm(SetUpContainer setupContainer)
    {
      this.setupContainer = setupContainer;
      this.InitializeComponent();
      this.initFolderCombo();
      this.lvwLoan.Sort(0, SortOrder.Ascending);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      this.contextMenu1 = new ContextMenu();
      this.showDetailMenuItem = new MenuItem();
      this.showAllMenuItem = new MenuItem();
      this.panel1 = new Panel();
      this.toolTip1 = new ToolTip(this.components);
      this.gcUnlockLoan = new GroupContainer();
      this.verticalSeparator1 = new VerticalSeparator();
      this.stdIconBtnRefresh = new StandardIconButton();
      this.lvwLoan = new GridView();
      this.unlockButton = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.folderLabel = new Label();
      this.folderCombo = new ComboBox();
      this.panel1.SuspendLayout();
      this.gcUnlockLoan.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnRefresh).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.SuspendLayout();
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.showDetailMenuItem,
        this.showAllMenuItem
      });
      this.showDetailMenuItem.Index = 0;
      this.showDetailMenuItem.Text = "Show details of selected loans";
      this.showDetailMenuItem.Click += new EventHandler(this.showDetailMenuItem_Click);
      this.showAllMenuItem.Index = 1;
      this.showAllMenuItem.Text = "Show details of all loans";
      this.showAllMenuItem.Click += new EventHandler(this.showAllMenuItem_Click);
      this.panel1.Controls.Add((Control) this.gradientPanel1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(681, 36);
      this.panel1.TabIndex = 7;
      this.gcUnlockLoan.Controls.Add((Control) this.verticalSeparator1);
      this.gcUnlockLoan.Controls.Add((Control) this.stdIconBtnRefresh);
      this.gcUnlockLoan.Controls.Add((Control) this.lvwLoan);
      this.gcUnlockLoan.Controls.Add((Control) this.unlockButton);
      this.gcUnlockLoan.Dock = DockStyle.Fill;
      this.gcUnlockLoan.HeaderForeColor = SystemColors.ControlText;
      this.gcUnlockLoan.Location = new Point(0, 36);
      this.gcUnlockLoan.Name = "gcUnlockLoan";
      this.gcUnlockLoan.Size = new Size(681, 390);
      this.gcUnlockLoan.TabIndex = 8;
      this.gcUnlockLoan.Text = "Locked Loan Files (#)";
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(610, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 8;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.stdIconBtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnRefresh.BackColor = Color.Transparent;
      this.stdIconBtnRefresh.Location = new Point(588, 5);
      this.stdIconBtnRefresh.MouseDownImage = (Image) null;
      this.stdIconBtnRefresh.Name = "stdIconBtnRefresh";
      this.stdIconBtnRefresh.Size = new Size(16, 17);
      this.stdIconBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.stdIconBtnRefresh.TabIndex = 7;
      this.stdIconBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnRefresh, "Refresh");
      this.stdIconBtnRefresh.Click += new EventHandler(this.stdIconBtnRefresh_Click);
      this.lvwLoan.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Borrower Name";
      gvColumn1.Width = 119;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Loan Number";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column6";
      gvColumn3.Text = "Lock Type";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column3";
      gvColumn4.Text = "Locked By (SID)";
      gvColumn4.Width = 117;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Login Info";
      gvColumn5.Width = 206;
      this.lvwLoan.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.lvwLoan.ContextMenu = this.contextMenu1;
      this.lvwLoan.Dock = DockStyle.Fill;
      this.lvwLoan.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lvwLoan.Location = new Point(1, 26);
      this.lvwLoan.Name = "lvwLoan";
      this.lvwLoan.Size = new Size(679, 363);
      this.lvwLoan.TabIndex = 4;
      this.lvwLoan.SelectedIndexChanged += new EventHandler(this.lvwLoan_SelectedIndexChanged);
      this.unlockButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.unlockButton.BackColor = SystemColors.Control;
      this.unlockButton.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.unlockButton.Location = new Point(615, 2);
      this.unlockButton.Name = "unlockButton";
      this.unlockButton.Size = new Size(62, 22);
      this.unlockButton.TabIndex = 6;
      this.unlockButton.Text = "&Unlock";
      this.unlockButton.UseVisualStyleBackColor = true;
      this.unlockButton.Click += new EventHandler(this.unlockButton_Click);
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.folderLabel);
      this.gradientPanel1.Controls.Add((Control) this.folderCombo);
      this.gradientPanel1.Dock = DockStyle.Fill;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(681, 36);
      this.gradientPanel1.TabIndex = 4;
      this.folderLabel.BackColor = Color.Transparent;
      this.folderLabel.Location = new Point(9, 6);
      this.folderLabel.Name = "folderLabel";
      this.folderLabel.Size = new Size(64, 22);
      this.folderLabel.TabIndex = 3;
      this.folderLabel.Text = "Loan Folder";
      this.folderLabel.TextAlign = ContentAlignment.MiddleLeft;
      this.folderCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.folderCombo.Location = new Point(79, 6);
      this.folderCombo.Name = "folderCombo";
      this.folderCombo.Size = new Size(276, 22);
      this.folderCombo.TabIndex = 2;
      this.folderCombo.SelectedIndexChanged += new EventHandler(this.folderCombo_SelectedIndexChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(681, 426);
      this.Controls.Add((Control) this.gcUnlockLoan);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (UnlockLoansForm);
      this.Text = nameof (UnlockLoansForm);
      this.VisibleChanged += new EventHandler(this.UnlockLoansForm_VisibleChanged);
      this.panel1.ResumeLayout(false);
      this.gcUnlockLoan.ResumeLayout(false);
      ((ISupportInitialize) this.stdIconBtnRefresh).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void initFolderCombo()
    {
      this.folderCombo.Items.Clear();
      this.folderCombo.Items.Add((object) new LoanFolderInfo(SystemSettings.AllFolders));
      LoanFolderInfo[] allLoanFolderInfos = Session.LoanManager.GetAllLoanFolderInfos(false);
      this.folderCombo.Items.AddRange((object[]) allLoanFolderInfos);
      this.folderCombo.SelectedIndex = -1;
      for (int index = 1; index < this.folderCombo.Items.Count; ++index)
      {
        if (string.Compare(((LoanFolderInfo) this.folderCombo.Items[index]).Name, Session.UserInfo.WorkingFolder ?? "", StringComparison.OrdinalIgnoreCase) == 0)
        {
          this.folderCombo.SelectedIndex = index;
          break;
        }
      }
      if (this.folderCombo.SelectedIndex >= 0 || allLoanFolderInfos.Length == 0)
        return;
      this.folderCombo.SelectedIndex = 0;
      for (int index = 1; index < this.folderCombo.Items.Count; ++index)
      {
        if (((LoanFolderInfo) this.folderCombo.Items[index]).Type == LoanFolderInfo.LoanFolderType.Regular)
        {
          this.folderCombo.SelectedIndex = index;
          break;
        }
      }
    }

    private void loadLoans()
    {
      string[] fields = new string[13]
      {
        "LoanNumber",
        "BorrowerFirstName",
        "BorrowerLastName",
        "LoanOfficerName",
        "LoanProcessorName",
        "LoanCloserName",
        "Address1",
        "LoanAmount",
        "LoanType",
        "LoanPurpose",
        "CoBorrowerFirstName",
        "CoBorrowerLastName",
        "LoanName"
      };
      LockInfo[] lockInfoArray = !(((LoanFolderInfo) this.folderCombo.SelectedItem).Name == SystemSettings.AllFolders) ? Session.LoanManager.GetCurrentLocks(true, Session.UserInfo.WorkingFolder, false) : Session.LoanManager.GetCurrentLocks(true, false);
      string[] guids = new string[lockInfoArray.Length];
      for (int index = 0; index < lockInfoArray.Length; ++index)
        guids[index] = lockInfoArray[index].GUID;
      PipelineInfo[] pipelineInfoArray = (PipelineInfo[]) null;
      if (guids.Length != 0)
        pipelineInfoArray = Session.LoanManager.GetPipeline(guids, fields, PipelineData.Lock, false);
      this.lvwLoan.BeginUpdate();
      this.lvwLoan.Items.Clear();
      for (int index = 0; index < guids.Length; ++index)
      {
        PipelineInfo pInfo = pipelineInfoArray[index];
        if (pInfo != null)
        {
          string str1 = lockInfoArray[index].LogonServer ?? "";
          string text = pInfo.LastName + ", " + pInfo.FirstName;
          string loanNumber = pInfo.LoanNumber;
          string str2 = lockInfoArray[index].LockedBy + " (" + lockInfoArray[index].LoginSessionID + ")";
          string empty = string.Empty;
          string str3 = lockInfoArray[index].Exclusive == LockInfo.ExclusiveLock.Exclusive || lockInfoArray[index].Exclusive == LockInfo.ExclusiveLock.ExclusiveA ? "Exclusive" : (lockInfoArray[index].Exclusive != LockInfo.ExclusiveLock.Nonexclusive ? (lockInfoArray[index].Exclusive != LockInfo.ExclusiveLock.NGSharedLock ? (lockInfoArray[index].Exclusive != LockInfo.ExclusiveLock.Both ? "Release" : "Both") : "Shared (Next Generation)") : "Shared (Multi-User Editing)");
          string str4 = "Unknown";
          if (lockInfoArray[index].CurrentlyLoggedOn == LockInfo.LockOwnerLoggedOn.True)
            str4 = lockInfoArray[index].LockedBy + " is currently logged on " + str1;
          else if (lockInfoArray[index].IsSessionLess && lockInfoArray[index].LockedFor == LoanInfo.LockReason.OpenForWork && lockInfoArray[index].Exclusive == LockInfo.ExclusiveLock.NGSharedLock)
            str4 = " Next generation app. No session required ";
          else if (lockInfoArray[index].CurrentlyLoggedOn == LockInfo.LockOwnerLoggedOn.False)
            str4 = "This might be from a crashed Encompass session";
          this.lvwLoan.Items.Add(new GVItem(text)
          {
            Tag = (object) new UnlockLoansForm.PipelineInfoLockInfo(pInfo, lockInfoArray[index]),
            SubItems = {
              (object) loanNumber,
              (object) str3,
              (object) str2,
              (object) str4
            }
          });
        }
      }
      this.lvwLoan.EndUpdate();
      this.gcUnlockLoan.Text = "Locked Loan Files (" + (object) this.lvwLoan.Items.Count + ")";
      this.lvwLoan_SelectedIndexChanged((object) this, (EventArgs) null);
    }

    private void folderCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      string name = ((LoanFolderInfo) this.folderCombo.SelectedItem).Name;
      if (name != SystemSettings.AllFolders)
        Session.WorkingFolder = name;
      this.loadLoans();
    }

    private void unlockButton_Click(object sender, EventArgs e)
    {
      if (this.lvwLoan.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select at least one loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        bool applicationRight = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_UnlockLoanFiles);
        for (int index = 0; index < this.lvwLoan.SelectedItems.Count; ++index)
        {
          UnlockLoansForm.PipelineInfoLockInfo tag = (UnlockLoansForm.PipelineInfoLockInfo) this.lvwLoan.SelectedItems[index].Tag;
          PipelineInfo pinfo = tag.PInfo;
          LockInfo linfo = tag.LInfo;
          ILoan loan = Session.LoanManager.OpenLoan(pinfo.GUID);
          LoanInfo.Right rights = loan.GetRights(Session.UserID);
          DialogResult dialogResult = DialogResult.Yes;
          if (linfo.CurrentlyLoggedOn != LockInfo.LockOwnerLoggedOn.False)
            dialogResult = Utils.Dialog((IWin32Window) this, string.Format("Loan '{0} {1}' is locked by '{2}' ({3}). If you remove the user's lock on the loan, the user won't be able to save any changes. Do you still want to continue?", (object) pinfo.FirstName, (object) pinfo.LastName, (object) linfo.LockedBy, (object) linfo.LoginSessionID), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
          if (dialogResult == DialogResult.Yes)
          {
            if (Session.UserInfo.IsSuperAdministrator())
              loan.Unlock(true, linfo.LoginSessionID, true);
            else if (Session.UserInfo.IsAdministrator() || Session.UserID == linfo.LockedBy && (LoanInfo.Right.FullRight == rights || LoanInfo.Right.Access == rights))
              loan.Unlock(true, linfo.LoginSessionID);
            else if (applicationRight)
            {
              loan.Unlock(true, linfo.LoginSessionID);
            }
            else
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "The loan is locked by user '" + linfo.LockedBy + "' and you do not have the right to unlock this loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
          }
          loan.Close();
        }
        this.loadLoans();
      }
    }

    private void showDetailMenuItem_Click(object sender, EventArgs e)
    {
      if (this.lvwLoan.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select at least one loan.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        PipelineInfo[] pipelineInfos = new PipelineInfo[this.lvwLoan.SelectedItems.Count];
        for (int index = 0; index < this.lvwLoan.SelectedItems.Count; ++index)
          pipelineInfos[index] = ((UnlockLoansForm.PipelineInfoLockInfo) this.lvwLoan.SelectedItems[index].Tag).PInfo;
        new LoanDetails(pipelineInfos).Show();
      }
    }

    private void showAllMenuItem_Click(object sender, EventArgs e)
    {
      PipelineInfo[] pipelineInfos = new PipelineInfo[this.lvwLoan.Items.Count];
      for (int nItemIndex = 0; nItemIndex < this.lvwLoan.Items.Count; ++nItemIndex)
        pipelineInfos[nItemIndex] = ((UnlockLoansForm.PipelineInfoLockInfo) this.lvwLoan.Items[nItemIndex].Tag).PInfo;
      new LoanDetails(pipelineInfos).Show();
    }

    private void UnlockLoansForm_VisibleChanged(object sender, EventArgs e)
    {
      this.initFolderCombo();
    }

    private void stdIconBtnRefresh_Click(object sender, EventArgs e)
    {
      this.folderCombo_SelectedIndexChanged((object) this, (EventArgs) null);
      SaveConfirmScreen.Show((IWin32Window) this.setupContainer, "Data refreshed.");
    }

    private void lvwLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.unlockButton.Enabled = this.lvwLoan.SelectedItems.Count > 0;
    }

    private class PipelineInfoLockInfo
    {
      public readonly PipelineInfo PInfo;
      public readonly LockInfo LInfo;

      public PipelineInfoLockInfo(PipelineInfo pInfo, LockInfo lInfo)
      {
        this.PInfo = pInfo;
        this.LInfo = lInfo;
      }
    }
  }
}
