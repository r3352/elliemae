// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.LenderRepresentativeLookup
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class LenderRepresentativeLookup : Form
  {
    private LoanData loan;
    private string eSignerType;
    private IContainer components;
    private GroupContainer groupContainer1;
    private GridView gvUsers;
    private Button cancelBtn;
    private Button okBtn;

    public LenderRepresentativeLookup(string eSignerType, LoanData loan)
    {
      this.InitializeComponent();
      this.loadUserList(eSignerType, loan);
    }

    private void loadUserList(string eSignerType, LoanData loan)
    {
      this.loan = loan;
      string str = "";
      this.eSignerType = eSignerType;
      switch (this.eSignerType)
      {
        case "broker":
          str = loan.GetField("4843");
          break;
        case "commitment":
          str = loan.GetField("4847");
          break;
        case "fhaVa":
          str = loan.GetField("4845");
          break;
        case "fhaVaMortgagee":
          str = loan.GetField("4846");
          break;
        case "lender":
          str = loan.GetField("4842");
          break;
        case "lenderRep":
          str = loan.GetField("4675");
          break;
        case "loanOriginator":
          str = loan.GetField("4841");
          break;
        case "usda":
          str = loan.GetField("4844");
          break;
      }
      if (str == "")
        return;
      UserInfo[] scopedUsersWithRole = Session.OrganizationManager.GetScopedUsersWithRole(Convert.ToInt32(str));
      this.gvUsers.Items.Clear();
      foreach (UserInfo userInfo in scopedUsersWithRole)
        this.gvUsers.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = userInfo.Userid
            },
            [1] = {
              Text = userInfo.LastName
            },
            [2] = {
              Text = userInfo.FirstName
            }
          },
          Tag = (object) userInfo.Userid
        });
      this.gvUsers.Sort(0, SortOrder.Ascending);
    }

    private void LenderRepresentativeLookup_Load(object sender, EventArgs e)
    {
    }

    private void gridView1_Click(object sender, EventArgs e)
    {
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.gvUsers.Items.Count == 0)
        this.DialogResult = DialogResult.Cancel;
      else if (this.gvUsers.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select a loan associate from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string id1 = "";
        string id2 = "";
        switch (this.eSignerType)
        {
          case "broker":
            id1 = "4810";
            id2 = "4833";
            break;
          case "commitment":
            id1 = "4827";
            id2 = "4837";
            break;
          case "fhaVa":
            id1 = "4816";
            id2 = "4835";
            break;
          case "fhaVaMortgagee":
            id1 = "4821";
            id2 = "4836";
            break;
          case "lender":
            id1 = "4807";
            id2 = "4832";
            break;
          case "lenderRep":
            id1 = "4682";
            id2 = "4840";
            break;
          case "loanOriginator":
            id1 = "4804";
            id2 = "4831";
            break;
          case "usda":
            id1 = "4813";
            id2 = "4834";
            break;
        }
        if (id2 != "" && id1 != "" && this.loan.GetField(id1) != this.gvUsers.SelectedItems[0].Tag.ToString())
          this.loan.SetField(id2, "");
        if (id1 != "")
          this.loan.SetField(id1, this.gvUsers.SelectedItems[0].Tag.ToString());
        this.DialogResult = DialogResult.OK;
      }
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
      this.groupContainer1 = new GroupContainer();
      this.gvUsers = new GridView();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.gvUsers);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(351, 353);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Select a User";
      this.gvUsers.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "User ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Last Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "First Name";
      gvColumn3.Width = 100;
      this.gvUsers.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvUsers.Dock = DockStyle.Fill;
      this.gvUsers.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvUsers.Location = new Point(1, 26);
      this.gvUsers.Name = "gvUsers";
      this.gvUsers.Size = new Size(349, 326);
      this.gvUsers.TabIndex = 4;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(286, 388);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(198, 388);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 6;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(375, 422);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (LenderRepresentativeLookup);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Loan Team Member";
      this.Load += new EventHandler(this.LenderRepresentativeLookup_Load);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
