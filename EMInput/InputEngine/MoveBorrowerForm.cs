// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MoveBorrowerForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MoveBorrowerForm : Form
  {
    private LoanData loan;
    private int selectedPairNo;
    private bool forCoborrower;
    private int targetBorrowerPairNo = -1;
    private bool targetCoBorrower;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnCancel;
    private Button btnMove;
    private Label label1;
    private GridView gridViewPairs;

    public MoveBorrowerForm(LoanData loan, int selectedPairNo, bool forCoborrower)
    {
      this.loan = loan;
      this.selectedPairNo = selectedPairNo;
      this.forCoborrower = forCoborrower;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      if (borrowerPairs == null || borrowerPairs.Length == 0)
        return;
      this.gridViewPairs.BeginUpdate();
      string empty = string.Empty;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        GVItem gvItem = new GVItem(string.Concat((object) (index + 1)));
        string borrowerName1 = borrowerPairs[index].Borrower.FirstName + " " + borrowerPairs[index].Borrower.LastName;
        gvItem.SubItems.Add((object) borrowerName1);
        if (this.selectedPairNo - 1 != index || this.forCoborrower)
        {
          BorrowerInRadioControl borrowerInRadioControl = new BorrowerInRadioControl(borrowerName1);
          borrowerInRadioControl.RadioButtonChecked += new EventHandler(this.radioButtonForBorrower_Checked);
          gvItem.SubItems[1].Value = (object) borrowerInRadioControl;
        }
        string borrowerName2 = borrowerPairs[index].CoBorrower.FirstName + " " + borrowerPairs[index].CoBorrower.LastName;
        gvItem.SubItems.Add((object) borrowerName2);
        if (this.selectedPairNo - 1 != index || !this.forCoborrower)
        {
          BorrowerInRadioControl borrowerInRadioControl = new BorrowerInRadioControl(borrowerName2);
          borrowerInRadioControl.RadioButtonChecked += new EventHandler(this.radioButtonForBorrower_Checked);
          gvItem.SubItems[2].Value = (object) borrowerInRadioControl;
        }
        if (this.selectedPairNo == index + 1)
        {
          if (this.forCoborrower)
            gvItem.SubItems[2].BackColor = Color.LightBlue;
          else
            gvItem.SubItems[1].BackColor = Color.LightBlue;
        }
        gvItem.Tag = (object) borrowerPairs[index];
        this.gridViewPairs.Items.Add(gvItem);
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
      this.radioButtonForBorrower_Checked((object) null, (EventArgs) null);
      this.gridViewPairs.EndUpdate();
    }

    private void radioButtonForBorrower_Checked(object sender, EventArgs e)
    {
      BorrowerInRadioControl borrowerInRadioControl1 = (BorrowerInRadioControl) null;
      if (sender != null)
        borrowerInRadioControl1 = (BorrowerInRadioControl) sender;
      bool flag = false;
      for (int nItemIndex1 = 0; nItemIndex1 < this.gridViewPairs.Items.Count; ++nItemIndex1)
      {
        for (int nItemIndex2 = 1; nItemIndex2 <= 2; ++nItemIndex2)
        {
          if (this.gridViewPairs.Items[nItemIndex1].SubItems[nItemIndex2].Value is BorrowerInRadioControl)
          {
            BorrowerInRadioControl borrowerInRadioControl2 = (BorrowerInRadioControl) this.gridViewPairs.Items[nItemIndex1].SubItems[nItemIndex2].Value;
            if (borrowerInRadioControl1 != borrowerInRadioControl2)
              borrowerInRadioControl2.UncheckRadio(true);
            if (borrowerInRadioControl2.BorrowerIsChecked)
              flag = true;
          }
        }
      }
      this.btnMove.Enabled = flag;
    }

    private void btnMove_Click(object sender, EventArgs e)
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
        int num = (int) Utils.Dialog((IWin32Window) this, "A borrower pair must be selected prior to this operation.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public int TargetBorrowerPairNo => this.targetBorrowerPairNo;

    public bool TargetCoBorrower => this.targetCoBorrower;

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
      this.btnCancel = new Button();
      this.btnMove = new Button();
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.gridViewPairs = new GridView();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(583, 318);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnMove.Location = new Point(502, 318);
      this.btnMove.Name = "btnMove";
      this.btnMove.Size = new Size(75, 23);
      this.btnMove.TabIndex = 3;
      this.btnMove.Text = "&Move";
      this.btnMove.UseVisualStyleBackColor = true;
      this.btnMove.Click += new EventHandler(this.btnMove_Click);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(305, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Choose a borrower position to swap with the selected borrower:";
      this.groupContainer1.Controls.Add((Control) this.gridViewPairs);
      this.groupContainer1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 33);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(647, 264);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Borrower Pairs";
      this.gridViewPairs.AllowMultiselect = false;
      this.gridViewPairs.AlternatingColors = false;
      this.gridViewPairs.BorderColor = SystemColors.Control;
      this.gridViewPairs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Pair";
      gvColumn1.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn1.Width = 40;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Borrower";
      gvColumn2.Width = 300;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Co-Borrower";
      gvColumn3.Width = 300;
      this.gridViewPairs.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gridViewPairs.Dock = DockStyle.Fill;
      this.gridViewPairs.FullRowSelect = false;
      this.gridViewPairs.HeaderHeight = 0;
      this.gridViewPairs.HeaderVisible = false;
      this.gridViewPairs.Location = new Point(1, 26);
      this.gridViewPairs.Name = "gridViewPairs";
      this.gridViewPairs.Selectable = false;
      this.gridViewPairs.Size = new Size(645, 237);
      this.gridViewPairs.TabIndex = 1;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(671, 353);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnMove);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MoveBorrowerForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Move Borrower";
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
