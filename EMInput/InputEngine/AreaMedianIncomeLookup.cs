// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AreaMedianIncomeLookup
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
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
  public class AreaMedianIncomeLookup : Form
  {
    private LoanData loan;
    private IContainer components;
    private GroupContainer groupContainer1;
    private GridView gvAMIs;
    private Button cancelBtn;
    private Button okBtn;

    public AreaMedianIncomeLookup(LoanData loan)
    {
      this.InitializeComponent();
      this.loadAMIList(loan);
    }

    private void loadAMIList(LoanData loan)
    {
      this.loan = loan;
      AMILimit[] amiLimitRecords = (AMILimit[]) loan.Calculator.GetAMILimitRecords(true);
      this.gvAMIs.Items.Clear();
      foreach (AMILimit amiLimit in amiLimitRecords)
        this.gvAMIs.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = amiLimit.LimitYear.ToString("0000")
            },
            [1] = {
              Text = amiLimit.FIPSCode
            },
            [2] = {
              Text = amiLimit.StateName
            },
            [3] = {
              Text = amiLimit.CountyName
            },
            [4] = {
              Text = amiLimit.AmiLimit100
            },
            [5] = {
              Text = amiLimit.AmiLimit80
            },
            [6] = {
              Text = amiLimit.AmiLimit50
            }
          },
          Tag = (object) amiLimit.ID
        });
    }

    private void AreaMedianIncomeLookup_Load(object sender, EventArgs e)
    {
    }

    private void gridView1_Click(object sender, EventArgs e)
    {
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.gvAMIs.Items.Count == 0)
        this.DialogResult = DialogResult.Cancel;
      else if (this.gvAMIs.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an Area Median Income Year from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.loan.SetField("MORNET.X30", this.gvAMIs.SelectedItems[0].SubItems[4].Text);
        this.loan.SetField("4971", this.gvAMIs.SelectedItems[0].SubItems[5].Text);
        this.loan.SetField("4972", this.gvAMIs.SelectedItems[0].SubItems[6].Text);
        this.loan.SetField("4970", this.gvAMIs.SelectedItems[0].Text);
        this.loan.Calculator.FormCalculation("MORNET.X30");
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
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      this.groupContainer1 = new GroupContainer();
      this.gvAMIs = new GridView();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.gvAMIs);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(701, 353);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Select an Area Median Income Year";
      this.gvAMIs.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Year";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "FIPS Code";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "State Name";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "County Name";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "100% AMI Limit";
      gvColumn5.Width = 100;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "80% AMI Limit";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "50% AMI Limit";
      gvColumn7.Width = 100;
      this.gvAMIs.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.gvAMIs.Dock = DockStyle.Fill;
      this.gvAMIs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvAMIs.Location = new Point(1, 26);
      this.gvAMIs.Name = "gvAMIs";
      this.gvAMIs.Size = new Size(699, 326);
      this.gvAMIs.TabIndex = 4;
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(638, 388);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "&Cancel";
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.Location = new Point(550, 388);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 22);
      this.okBtn.TabIndex = 6;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(727, 422);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.groupContainer1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (AreaMedianIncomeLookup);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Select Area Median Income";
      this.Load += new EventHandler(this.AreaMedianIncomeLookup_Load);
      this.groupContainer1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
