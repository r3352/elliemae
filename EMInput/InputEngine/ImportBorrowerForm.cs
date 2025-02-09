// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.ImportBorrowerForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class ImportBorrowerForm : Form
  {
    private LoanData loan;
    private int targetBorrowerPairNo = -1;
    private bool targetCoBorrower;
    private IContainer components;
    private Label label1;
    private Button btnNext;
    private Button btnCancel;
    private GroupContainer groupContainer1;
    private GridView gridViewPairs;
    private ComboBox cboSource;
    private Label label2;
    private ComboBox cboBorrower;

    public ImportBorrowerForm(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.initForm();
      this.cboSource_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void initForm()
    {
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      this.gridViewPairs.BeginUpdate();
      if (borrowerPairs != null && borrowerPairs.Length != 0)
      {
        string empty = string.Empty;
        for (int index = 0; index < borrowerPairs.Length; ++index)
        {
          GVItem gvItem = new GVItem(string.Concat((object) (index + 1)));
          string borrowerName1 = borrowerPairs[index].Borrower.FirstName + " " + borrowerPairs[index].Borrower.LastName;
          gvItem.SubItems.Add((object) borrowerName1);
          if (borrowerName1.Trim() == string.Empty)
          {
            BorrowerInRadioControl borrowerInRadioControl = new BorrowerInRadioControl("");
            borrowerInRadioControl.RadioButtonChecked += new EventHandler(this.radioButtonForBorrower_Checked);
            gvItem.SubItems[1].Value = (object) borrowerInRadioControl;
          }
          else
            gvItem.SubItems[1].Value = (object) new BorrowerInRadioControl(borrowerName1, true);
          string borrowerName2 = borrowerPairs[index].CoBorrower.FirstName + " " + borrowerPairs[index].CoBorrower.LastName;
          gvItem.SubItems.Add((object) borrowerName2);
          if (borrowerName2.Trim() == string.Empty)
          {
            BorrowerInRadioControl borrowerInRadioControl = new BorrowerInRadioControl("");
            borrowerInRadioControl.RadioButtonChecked += new EventHandler(this.radioButtonForBorrower_Checked);
            gvItem.SubItems[2].Value = (object) borrowerInRadioControl;
          }
          else
            gvItem.SubItems[2].Value = (object) new BorrowerInRadioControl(borrowerName2, true);
          gvItem.Tag = (object) borrowerPairs[index];
          this.gridViewPairs.Items.Add(gvItem);
        }
      }
      if (this.gridViewPairs.Items.Count < 6)
      {
        for (int index = this.gridViewPairs.Items.Count + 1; index <= 6; ++index)
        {
          GVItem gvItem = new GVItem(string.Concat((object) index));
          gvItem.SubItems.Add((object) "");
          BorrowerInRadioControl borrowerInRadioControl = new BorrowerInRadioControl("");
          borrowerInRadioControl.RadioButtonChecked += new EventHandler(this.radioButtonForBorrower_Checked);
          gvItem.SubItems[1].Value = (object) borrowerInRadioControl;
          gvItem.SubItems.Add((object) "");
          gvItem.Tag = (object) null;
          this.gridViewPairs.Items.Add(gvItem);
        }
      }
      this.gridViewPairs.EndUpdate();
      this.btnNext.Enabled = this.borrowerSelected() && this.cboSource.Text != string.Empty;
      switch (Session.LoanDataMgr.GetFieldAccessRights("Button_FMBorImport"))
      {
        case BizRule.FieldAccessRight.Hide:
        case BizRule.FieldAccessRight.ViewOnly:
          this.cboSource.Items.Remove((object) "ULAD / iLAD (MISMO 3.4) file");
          this.cboSource.Items.Remove((object) "FNMA 3.2 file");
          break;
      }
      Hashtable hashtable = ((FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features)).CheckPermissions(FeatureSets.LoanOtherFeatures, Session.UserInfo);
      if (!(bool) hashtable[(object) AclFeature.LoanTab_Other_ImportBorrowers])
      {
        this.cboSource.Enabled = false;
      }
      else
      {
        if (!(bool) hashtable[(object) AclFeature.LoanTab_Other_ImportBorrowersfromContacts])
          this.cboSource.Items.Remove((object) "Contacts");
        if (!(bool) hashtable[(object) AclFeature.LoanTab_Other_ImportBorrowersfromLoan])
          this.cboSource.Items.Remove((object) "Another loan file");
        if (!(bool) hashtable[(object) AclFeature.LoanTab_Other_ImportBorrowersfromENMA])
          this.cboSource.Items.Remove((object) "FNMA 3.2 file");
        if (!(bool) hashtable[(object) AclFeature.LoanTab_Other_ImportBorrowersfromMISMO])
          this.cboSource.Items.Remove((object) "ULAD / iLAD (MISMO 3.4) file");
      }
      LoanFolderAclInfo[] accessibleLoanFolders = ((LoanFoldersAclManager) Session.ACL.GetAclManager(AclCategory.LoanFolderMove)).GetAccessibleLoanFolders(AclFeature.LoanMgmt_Import, Session.UserID, Session.OrganizationManager.GetUser(Session.UserID).UserPersonas);
      for (int index = 0; index < accessibleLoanFolders.Length; ++index)
      {
        if (accessibleLoanFolders[index].FolderName == "Fannie Mae 3.x" && accessibleLoanFolders[index].MoveFromAccess == 0)
          this.cboSource.Items.Remove((object) "FNMA 3.2 file");
        else if (accessibleLoanFolders[index].FolderName == "ULAD" && accessibleLoanFolders[index].MoveFromAccess == 0)
          this.cboSource.Items.Remove((object) "ULAD / iLAD (MISMO 3.4) file");
      }
    }

    private void radioButtonForBorrower_Checked(object sender, EventArgs e)
    {
      BorrowerInRadioControl borrowerInRadioControl1 = (BorrowerInRadioControl) null;
      if (sender != null)
        borrowerInRadioControl1 = (BorrowerInRadioControl) sender;
      for (int nItemIndex1 = 0; nItemIndex1 < this.gridViewPairs.Items.Count; ++nItemIndex1)
      {
        for (int nItemIndex2 = 1; nItemIndex2 <= 2; ++nItemIndex2)
        {
          if (this.gridViewPairs.Items[nItemIndex1].SubItems[nItemIndex2].Value is BorrowerInRadioControl)
          {
            BorrowerInRadioControl borrowerInRadioControl2 = (BorrowerInRadioControl) this.gridViewPairs.Items[nItemIndex1].SubItems[nItemIndex2].Value;
            if (borrowerInRadioControl1 != borrowerInRadioControl2)
              borrowerInRadioControl2.UncheckRadio(true);
          }
        }
      }
      this.btnNext.Enabled = this.borrowerSelected() && this.cboSource.Text != string.Empty;
    }

    private bool borrowerSelected()
    {
      for (int nItemIndex1 = 0; nItemIndex1 < this.gridViewPairs.Items.Count; ++nItemIndex1)
      {
        for (int nItemIndex2 = 1; nItemIndex2 <= 2; ++nItemIndex2)
        {
          if (this.gridViewPairs.Items[nItemIndex1].SubItems[nItemIndex2].Value is BorrowerInRadioControl && ((BorrowerInRadioControl) this.gridViewPairs.Items[nItemIndex1].SubItems[nItemIndex2].Value).BorrowerIsChecked)
            return true;
        }
      }
      return false;
    }

    private void cboSource_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboSource.SelectedItem != null && (string.Compare(this.cboSource.SelectedItem.ToString(), "Another loan file", true) == 0 || string.Compare(this.cboSource.SelectedItem.ToString(), "FNMA 3.2 file", true) == 0 || string.Compare(this.cboSource.SelectedItem.ToString(), "ULAD / iLAD (MISMO 3.4) file", true) == 0))
      {
        this.cboBorrower.Visible = true;
        if (this.cboBorrower.SelectedIndex == -1)
          this.cboBorrower.SelectedIndex = 0;
      }
      else
        this.cboBorrower.Visible = false;
      this.btnNext.Enabled = this.borrowerSelected() && this.cboSource.Text != string.Empty;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.cboSource.Text == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select import source.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        for (int nItemIndex1 = 0; nItemIndex1 < this.gridViewPairs.Items.Count; ++nItemIndex1)
        {
          for (int nItemIndex2 = 1; nItemIndex2 <= 2; ++nItemIndex2)
          {
            if (this.gridViewPairs.Items[nItemIndex1].SubItems[nItemIndex2].Value is BorrowerInRadioControl && ((BorrowerInRadioControl) this.gridViewPairs.Items[nItemIndex1].SubItems[nItemIndex2].Value).BorrowerIsChecked)
            {
              this.targetBorrowerPairNo = nItemIndex1 + 1;
              this.targetCoBorrower = nItemIndex2 == 2;
              break;
            }
          }
        }
        if (this.targetBorrowerPairNo == -1)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "A borrower pair must be selected prior to this operation.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
          this.DialogResult = DialogResult.OK;
      }
    }

    public string SelectedSource => this.cboSource.SelectedItem.ToString();

    public bool ImportedFromCoborrower
    {
      get
      {
        return this.cboBorrower.SelectedItem != null && string.Compare(this.cboBorrower.SelectedItem.ToString(), "co-borrower", true) == 0;
      }
    }

    public int TargetBorrowerPairNo => this.targetBorrowerPairNo;

    public bool TargetCoBorrower => this.targetCoBorrower;

    private void gridViewPairs_DoubleClick(object sender, EventArgs e)
    {
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
      this.label1 = new Label();
      this.btnNext = new Button();
      this.btnCancel = new Button();
      this.groupContainer1 = new GroupContainer();
      this.gridViewPairs = new GridView();
      this.cboSource = new ComboBox();
      this.label2 = new Label();
      this.cboBorrower = new ComboBox();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 40);
      this.label1.Name = "label1";
      this.label1.Size = new Size(265, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Select an open borrower position for the imported data:";
      this.btnNext.Location = new Point(497, 335);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new Size(75, 23);
      this.btnNext.TabIndex = 7;
      this.btnNext.Text = "&Next";
      this.btnNext.UseVisualStyleBackColor = true;
      this.btnNext.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(578, 335);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.groupContainer1.Controls.Add((Control) this.gridViewPairs);
      this.groupContainer1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(7, 59);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(647, 264);
      this.groupContainer1.TabIndex = 5;
      this.groupContainer1.Text = "Borrower Pairs";
      this.gridViewPairs.AllowMultiselect = false;
      this.gridViewPairs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Pair";
      gvColumn1.Width = 30;
      gvColumn2.ActivatedEditorType = GVActivatedEditorType.UserType;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Borrower";
      gvColumn2.Width = 300;
      gvColumn3.ActivatedEditorType = GVActivatedEditorType.UserType;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Co-Borrower";
      gvColumn3.Width = 315;
      this.gridViewPairs.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewPairs.Dock = DockStyle.Fill;
      this.gridViewPairs.FullRowSelect = false;
      this.gridViewPairs.Location = new Point(1, 26);
      this.gridViewPairs.Name = "gridViewPairs";
      this.gridViewPairs.Selectable = false;
      this.gridViewPairs.Size = new Size(645, 237);
      this.gridViewPairs.SortOption = GVSortOption.None;
      this.gridViewPairs.TabIndex = 11;
      this.gridViewPairs.DoubleClick += new EventHandler(this.gridViewPairs_DoubleClick);
      this.cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSource.FormattingEnabled = true;
      this.cboSource.Items.AddRange(new object[4]
      {
        (object) "ULAD / iLAD (MISMO 3.4) file",
        (object) "FNMA 3.2 file",
        (object) "Another loan file",
        (object) "Contacts"
      });
      this.cboSource.Location = new Point(55, 10);
      this.cboSource.Name = "cboSource";
      this.cboSource.Size = new Size(160, 21);
      this.cboSource.TabIndex = 9;
      this.cboSource.SelectedIndexChanged += new EventHandler(this.cboSource_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 13);
      this.label2.Name = "label2";
      this.label2.Size = new Size(41, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "Source";
      this.cboBorrower.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrower.FormattingEnabled = true;
      this.cboBorrower.Items.AddRange(new object[2]
      {
        (object) "Borrower",
        (object) "Co-Borrower"
      });
      this.cboBorrower.Location = new Point(221, 10);
      this.cboBorrower.Name = "cboBorrower";
      this.cboBorrower.Size = new Size(160, 21);
      this.cboBorrower.TabIndex = 11;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(661, 366);
      this.Controls.Add((Control) this.cboBorrower);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.cboSource);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnNext);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ImportBorrowerForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Import Borrower";
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
