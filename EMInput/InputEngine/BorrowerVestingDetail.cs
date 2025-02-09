// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BorrowerVestingDetail
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class BorrowerVestingDetail : System.Windows.Forms.Form
  {
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Button cancelBtn;
    private System.Windows.Forms.Button okBtn;
    private System.Windows.Forms.TextBox txtBorPOA;
    private ComboBox cboBorType;
    private System.Windows.Forms.TextBox txtBorName;
    private System.Windows.Forms.TextBox txtBorAKA;
    private System.Windows.Forms.TextBox txtBorSSN;
    private ToolTip fieldToolTip;
    private ComboBox cboVesting;
    private System.Windows.Forms.Label label4;
    private IContainer components;
    private System.Windows.Forms.CheckBox chkSign;
    private EMHelpLink emHelpLink1;
    private ComboBox cboBorPair;
    private System.Windows.Forms.Label label7;
    private ComboBox cboTrusteeOf;
    private System.Windows.Forms.Label label8;
    private LoanData loan;
    private VestingPartyFields vestingPartyFields;
    private WinFormInputHandler inputHandler;
    private bool isNew;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.TextBox txtBorPOASigText;
    private ComboBox cboOccupancy;
    private DatePicker dpTILAppDateDOB;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label label12;
    private ComboBox cboIntent;
    private bool suspendEvents = true;

    public BorrowerVestingDetail(LoanData loan, VestingPartyFields vestingPartyFields)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.initVesting();
      this.initBorrowerPairs();
      this.initializeFields(vestingPartyFields);
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
      this.label5 = new System.Windows.Forms.Label();
      this.txtBorPOA = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.cboBorType = new ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtBorName = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txtBorAKA = new System.Windows.Forms.TextBox();
      this.txtBorSSN = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.cancelBtn = new System.Windows.Forms.Button();
      this.okBtn = new System.Windows.Forms.Button();
      this.fieldToolTip = new ToolTip(this.components);
      this.cboVesting = new ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.emHelpLink1 = new EMHelpLink();
      this.chkSign = new System.Windows.Forms.CheckBox();
      this.cboBorPair = new ComboBox();
      this.label7 = new System.Windows.Forms.Label();
      this.cboTrusteeOf = new ComboBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.txtBorPOASigText = new System.Windows.Forms.TextBox();
      this.cboOccupancy = new ComboBox();
      this.dpTILAppDateDOB = new DatePicker();
      this.label10 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.cboIntent = new ComboBox();
      this.SuspendLayout();
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 222);
      this.label5.Name = "label5";
      this.label5.Size = new Size(78, 14);
      this.label5.TabIndex = 21;
      this.label5.Text = "POA Borrower";
      this.txtBorPOA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBorPOA.Location = new Point(155, 219);
      this.txtBorPOA.Name = "txtBorPOA";
      this.txtBorPOA.Size = new Size(326, 20);
      this.txtBorPOA.TabIndex = 9;
      this.txtBorPOA.Tag = (object) "";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 78);
      this.label3.Name = "label3";
      this.label3.Size = new Size(69, 14);
      this.label3.TabIndex = 19;
      this.label3.Text = "Vesting Type";
      this.cboBorType.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboBorType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorType.Location = new Point(155, 75);
      this.cboBorType.MaxDropDownItems = 50;
      this.cboBorType.Name = "cboBorType";
      this.cboBorType.Size = new Size(152, 22);
      this.cboBorType.TabIndex = 3;
      this.cboBorType.Tag = (object) "";
      this.cboBorType.SelectedIndexChanged += new EventHandler(this.cboBorType_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 13);
      this.label2.Name = "label2";
      this.label2.Size = new Size(34, 14);
      this.label2.TabIndex = 18;
      this.label2.Text = "Name";
      this.txtBorName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBorName.Location = new Point(155, 10);
      this.txtBorName.Name = "txtBorName";
      this.txtBorName.Size = new Size(326, 20);
      this.txtBorName.TabIndex = 0;
      this.txtBorName.Tag = (object) "";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 34);
      this.label1.Name = "label1";
      this.label1.Size = new Size(83, 14);
      this.label1.TabIndex = 22;
      this.label1.Text = "Also Known As";
      this.txtBorAKA.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBorAKA.Location = new Point(155, 31);
      this.txtBorAKA.Name = "txtBorAKA";
      this.txtBorAKA.Size = new Size(326, 20);
      this.txtBorAKA.TabIndex = 1;
      this.txtBorAKA.Tag = (object) "";
      this.txtBorSSN.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBorSSN.Location = new Point(155, 52);
      this.txtBorSSN.Name = "txtBorSSN";
      this.txtBorSSN.Size = new Size(152, 20);
      this.txtBorSSN.TabIndex = 2;
      this.txtBorSSN.Tag = (object) "";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 55);
      this.label6.Name = "label6";
      this.label6.Size = new Size(98, 14);
      this.label6.TabIndex = 25;
      this.label6.Text = "Social Security No.";
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(406, 321);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 14;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(328, 321);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 13;
      this.okBtn.Text = "&OK";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cboVesting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboVesting.Location = new Point(155, 262);
      this.cboVesting.MaxDropDownItems = 10;
      this.cboVesting.Name = "cboVesting";
      this.cboVesting.Size = new Size(326, 22);
      this.cboVesting.TabIndex = 11;
      this.cboVesting.Tag = (object) "";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 266);
      this.label4.Name = "label4";
      this.label4.Size = new Size(43, 14);
      this.label4.TabIndex = 27;
      this.label4.Text = "Vesting";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Borrower Information - Vesting";
      this.emHelpLink1.Location = new Point(6, 327);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 15;
      this.emHelpLink1.TabStop = false;
      this.chkSign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.chkSign.AutoSize = true;
      this.chkSign.Location = new Point(155, 290);
      this.chkSign.Name = "chkSign";
      this.chkSign.Size = new Size(239, 18);
      this.chkSign.TabIndex = 12;
      this.chkSign.Tag = (object) "";
      this.chkSign.Text = "Authorized to Sign for Non-Intervivos Trusts";
      this.chkSign.UseVisualStyleBackColor = true;
      this.cboBorPair.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboBorPair.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorPair.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "Individual",
        (object) "Co-signer",
        (object) "Officer",
        (object) "Title only"
      });
      this.cboBorPair.Location = new Point(155, 171);
      this.cboBorPair.MaxDropDownItems = 10;
      this.cboBorPair.Name = "cboBorPair";
      this.cboBorPair.Size = new Size(326, 22);
      this.cboBorPair.TabIndex = 7;
      this.cboBorPair.Tag = (object) "";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 175);
      this.label7.Name = "label7";
      this.label7.Size = new Size(142, 14);
      this.label7.TabIndex = 72;
      this.label7.Text = "Connected to Borrower Pair";
      this.cboTrusteeOf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboTrusteeOf.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboTrusteeOf.Location = new Point(155, 195);
      this.cboTrusteeOf.MaxDropDownItems = 10;
      this.cboTrusteeOf.Name = "cboTrusteeOf";
      this.cboTrusteeOf.Size = new Size(326, 22);
      this.cboTrusteeOf.TabIndex = 8;
      this.cboTrusteeOf.Tag = (object) "";
      this.cboTrusteeOf.SelectedIndexChanged += new EventHandler(this.cboTrusteeOf_SelectedIndexChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 199);
      this.label8.Name = "label8";
      this.label8.Size = new Size(59, 14);
      this.label8.TabIndex = 74;
      this.label8.Text = "Trustee Of";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 244);
      this.label9.Name = "label9";
      this.label9.Size = new Size(100, 14);
      this.label9.TabIndex = 76;
      this.label9.Text = "POA Signature Text";
      this.txtBorPOASigText.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.txtBorPOASigText.Location = new Point(155, 240);
      this.txtBorPOASigText.Name = "txtBorPOASigText";
      this.txtBorPOASigText.Size = new Size(326, 20);
      this.txtBorPOASigText.TabIndex = 10;
      this.txtBorPOASigText.Tag = (object) "";
      this.cboOccupancy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboOccupancy.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboOccupancy.Location = new Point(155, 123);
      this.cboOccupancy.MaxDropDownItems = 50;
      this.cboOccupancy.Name = "cboOccupancy";
      this.cboOccupancy.Size = new Size(152, 22);
      this.cboOccupancy.TabIndex = 5;
      this.cboOccupancy.Tag = (object) "";
      this.dpTILAppDateDOB.BackColor = SystemColors.Window;
      this.dpTILAppDateDOB.Location = new Point(155, 99);
      this.dpTILAppDateDOB.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpTILAppDateDOB.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpTILAppDateDOB.Name = "dpTILAppDateDOB";
      this.dpTILAppDateDOB.Size = new Size(152, 22);
      this.dpTILAppDateDOB.TabIndex = 4;
      this.dpTILAppDateDOB.Tag = (object) "3292";
      this.dpTILAppDateDOB.ToolTip = "";
      this.dpTILAppDateDOB.Value = new DateTime(0L);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(6, 102);
      this.label10.Name = "label10";
      this.label10.Size = new Size(32, 14);
      this.label10.TabIndex = 79;
      this.label10.Text = "DOB ";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(6, 126);
      this.label11.Name = "label11";
      this.label11.Size = new Size(97, 14);
      this.label11.TabIndex = 80;
      this.label11.Text = "Occupancy Status";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(6, 150);
      this.label12.Name = "label12";
      this.label12.Size = new Size(92, 14);
      this.label12.TabIndex = 82;
      this.label12.Text = "Occupancy Intent";
      this.cboIntent.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.cboIntent.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboIntent.Location = new Point(155, 147);
      this.cboIntent.MaxDropDownItems = 50;
      this.cboIntent.Name = "cboIntent";
      this.cboIntent.Size = new Size(152, 22);
      this.cboIntent.TabIndex = 6;
      this.cboIntent.Tag = (object) "";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(494, 355);
      this.Controls.Add((System.Windows.Forms.Control) this.label12);
      this.Controls.Add((System.Windows.Forms.Control) this.cboIntent);
      this.Controls.Add((System.Windows.Forms.Control) this.label11);
      this.Controls.Add((System.Windows.Forms.Control) this.label10);
      this.Controls.Add((System.Windows.Forms.Control) this.dpTILAppDateDOB);
      this.Controls.Add((System.Windows.Forms.Control) this.cboOccupancy);
      this.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.Controls.Add((System.Windows.Forms.Control) this.txtBorPOASigText);
      this.Controls.Add((System.Windows.Forms.Control) this.cboTrusteeOf);
      this.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.Controls.Add((System.Windows.Forms.Control) this.cboBorPair);
      this.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.Controls.Add((System.Windows.Forms.Control) this.emHelpLink1);
      this.Controls.Add((System.Windows.Forms.Control) this.chkSign);
      this.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.Controls.Add((System.Windows.Forms.Control) this.cboVesting);
      this.Controls.Add((System.Windows.Forms.Control) this.txtBorAKA);
      this.Controls.Add((System.Windows.Forms.Control) this.txtBorSSN);
      this.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.Controls.Add((System.Windows.Forms.Control) this.txtBorName);
      this.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.Controls.Add((System.Windows.Forms.Control) this.cancelBtn);
      this.Controls.Add((System.Windows.Forms.Control) this.cboBorType);
      this.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.Controls.Add((System.Windows.Forms.Control) this.txtBorPOA);
      this.Controls.Add((System.Windows.Forms.Control) this.okBtn);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (BorrowerVestingDetail);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Borrower Vesting";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void initVesting()
    {
      this.cboVesting.Items.Clear();
      this.cboVesting.Items.AddRange((object[]) BorrowerVestingForm.VestingList);
      this.cboOccupancy.Items.Clear();
      this.cboOccupancy.Items.AddRange((object[]) new string[3]
      {
        "Primary",
        "Secondary",
        "Investment"
      });
      this.cboIntent.Items.Clear();
      this.cboIntent.Items.AddRange((object[]) new string[3]
      {
        "Will Occupy",
        "Will Not Occupy",
        "Currently Occupy"
      });
    }

    private void initBorrowerPairs()
    {
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      this.cboBorPair.Items.Clear();
      this.cboBorPair.Items.Add((object) "");
      foreach (object obj in borrowerPairs)
        this.cboBorPair.Items.Add(obj);
    }

    private void initializeFields(VestingPartyFields fields)
    {
      this.vestingPartyFields = fields;
      this.inputHandler = WinFormInputHandler.Create(this.loan);
      this.inputHandler.AutoCommit = false;
      this.inputHandler.DataBind += new DataBindEventHandler(this.performCustomDataBind);
      this.inputHandler.DataCommit += new DataCommitEventHandler(this.performCustomDataCommit);
      if (this.vestingPartyFields.Type == VestingPartyType.Other && this.vestingPartyFields.VestingPartyIndex > this.loan.GetNumberOfAdditionalVestingParties())
        this.isNew = true;
      this.txtBorName.Tag = (object) fields.NameField;
      this.txtBorAKA.Tag = (object) fields.AliasField;
      this.txtBorSSN.Tag = (object) fields.SSNField;
      this.cboBorType.Tag = (object) fields.TypeField;
      if (fields.BorrowerPairField != null)
        this.cboBorPair.Tag = (object) fields.BorrowerPairField;
      this.dpTILAppDateDOB.Tag = (object) fields.BorrowerDOB;
      this.cboOccupancy.Tag = (object) fields.OccupancyStatus;
      this.cboIntent.Tag = (object) fields.OccupancyIntent;
      this.cboTrusteeOf.Tag = (object) fields.TrusteeOfField;
      this.txtBorPOA.Tag = (object) fields.POAField;
      this.txtBorPOASigText.Tag = (object) fields.POASignatureTextField;
      this.cboVesting.Tag = (object) fields.VestingField;
      this.chkSign.Tag = (object) fields.AuthToSignField;
      if (this.vestingPartyFields.BorrowerPair != null)
      {
        this.cboBorPair.SelectedItem = (object) this.vestingPartyFields.BorrowerPair;
        this.cboBorPair.Enabled = false;
      }
      this.inputHandler.Attach((System.Windows.Forms.Control) this, this.fieldToolTip);
      if (this.vestingPartyFields.VestingPartyIndex > 0 && this.loan.GetField("TR" + this.vestingPartyFields.VestingPartyIndex.ToString("00") + "99") != "")
      {
        for (int index = this.cboBorType.Items.Count - 1; index >= 0; --index)
        {
          if (!this.cboBorType.Items[index].ToString().ToLower().StartsWith("title only") && !string.Equals(this.cboBorType.Items[index].ToString().ToLower(), "non title spouse"))
            this.cboBorType.Items.RemoveAt(index);
        }
      }
      this.suspendEvents = false;
    }

    private void performCustomDataCommit(object sender, DataCommitEventArgs e)
    {
      string fieldId = this.inputHandler.GetFieldID((System.Windows.Forms.Control) sender);
      if (this.loan.IsFieldReadOnly(fieldId))
      {
        e.Cancel = true;
      }
      else
      {
        if (fieldId == this.vestingPartyFields.BorrowerPairField)
          this.loan.SetCurrentField(fieldId, this.getSelectedPairID());
        else
          this.loan.SetCurrentField(fieldId, e.Value, this.vestingPartyFields.BorrowerPair, false);
        e.Cancel = true;
      }
    }

    private void performCustomDataBind(object sender, DataBindEventArgs e)
    {
      string fieldId = this.inputHandler.GetFieldID((System.Windows.Forms.Control) sender);
      if (fieldId == this.vestingPartyFields.BorrowerPairField)
      {
        this.selectBorrowerPair(e.Value);
        e.Cancel = true;
      }
      else
        e.Value = this.loan.GetField(fieldId, this.vestingPartyFields.BorrowerPair);
    }

    private void selectBorrowerPair(string pairId)
    {
      foreach (object obj in this.cboBorPair.Items)
      {
        if (obj is BorrowerPair && ((BorrowerPair) obj).Id == pairId)
        {
          this.cboBorPair.SelectedItem = obj;
          return;
        }
      }
      this.cboBorPair.SelectedIndex = 0;
    }

    private string getSelectedPairID()
    {
      return !(this.cboBorPair.SelectedItem is BorrowerPair selectedItem) ? "" : selectedItem.Id;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (!this.validateData())
        return;
      if (this.isNew)
        this.loan.NewAdditionalVestingParty();
      this.inputHandler.CommitValues();
      this.DialogResult = DialogResult.OK;
    }

    private bool validateData()
    {
      if (this.txtBorName.Text.Trim() == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A name must be supplied for this individual.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.cboBorType.SelectedIndex >= 0 && !(this.cboBorType.Text == ""))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "A Vesting Type must be selected for this individual.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void cboTrusteeOf_SelectedIndexChanged(object sender, EventArgs e)
    {
      string text = this.cboTrusteeOf.Text;
      string id = "";
      switch (text)
      {
        case "Trust 1":
          id = "1861";
          break;
        case "Trust 2":
          id = "Vesting.Trst2Type";
          break;
      }
      string lower = this.loan.GetField(id).ToLower();
      if (lower.EndsWith("trust") && lower != "an inter vivos trust")
      {
        this.chkSign.Enabled = true;
        this.chkSign.Checked = this.loan.GetField(this.vestingPartyFields.AuthToSignField) == "Y";
        this.chkSign.Tag = (object) this.vestingPartyFields.AuthToSignField;
      }
      else
      {
        this.chkSign.Enabled = false;
        this.chkSign.Checked = false;
        this.chkSign.Tag = (object) null;
      }
    }

    private void cboBorType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents || !(this.cboBorType.SelectedItem is FieldOption selectedItem) || !VestingPartyFields.IsTrusteeType(selectedItem.Value) || this.cboTrusteeOf.SelectedIndex >= 1)
        return;
      this.cboTrusteeOf.SelectedIndex = 1;
    }
  }
}
