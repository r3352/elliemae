// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MedianFamilyIncomeLookup
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
  public class MedianFamilyIncomeLookup : Form
  {
    private LoanData loan;
    private IContainer components;
    private GridView gridViewMFI;
    private Button OkBtn;
    private Button cancelBtn;

    public MedianFamilyIncomeLookup(LoanData loan)
    {
      this.InitializeComponent();
      this.loadMFIList(loan);
      this.loan = loan;
    }

    private void loadMFIList(LoanData loan)
    {
      MFILimit[] mfiLimitRecords = (MFILimit[]) loan.Calculator.GetMFILimitRecords(true);
      this.gridViewMFI.Items.Clear();
      foreach (MFILimit mfiLimit in mfiLimitRecords)
      {
        GVItem gvItem = new GVItem();
        gvItem.SubItems[0].Text = mfiLimit.MSAMDCode;
        gvItem.SubItems[1].Text = mfiLimit.MSAMDName;
        GVSubItem subItem1 = gvItem.SubItems[2];
        int num = mfiLimit.ActualMFIYear;
        string str1 = num.ToString();
        subItem1.Text = str1;
        gvItem.SubItems[3].Text = mfiLimit.ActualMFIAmount;
        GVSubItem subItem2 = gvItem.SubItems[4];
        num = mfiLimit.EstimatedMFIYear;
        string str2 = num.ToString();
        subItem2.Text = str2;
        gvItem.SubItems[5].Text = mfiLimit.EstimatedMFIAmount;
        gvItem.Tag = (object) mfiLimit.ID;
        this.gridViewMFI.Items.Add(gvItem);
      }
    }

    private void OkBtn_Click(object sender, EventArgs e)
    {
      if (this.gridViewMFI.Items.Count == 0)
        this.DialogResult = DialogResult.Cancel;
      else if (this.gridViewMFI.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select an Median Family Income Year from the list provided.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.loan.SetField("5018", this.gridViewMFI.SelectedItems[0].SubItems[2].Text);
        this.loan.SetField("5019", this.gridViewMFI.SelectedItems[0].SubItems[4].Text);
        this.loan.SetField("5020", this.gridViewMFI.SelectedItems[0].SubItems[3].Text);
        this.loan.SetField("5021", this.gridViewMFI.SelectedItems[0].SubItems[5].Text);
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
      this.gridViewMFI = new GridView();
      this.OkBtn = new Button();
      this.cancelBtn = new Button();
      this.SuspendLayout();
      this.gridViewMFI.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "MSA/MD Code Number";
      gvColumn1.Width = 130;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "MSA/MD Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Actual MFI Year";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Actual Median Family Income";
      gvColumn4.Width = 160;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Estimated MFI Year";
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Estimated Median Family Income";
      gvColumn6.Width = 180;
      this.gridViewMFI.Columns.AddRange(new GVColumn[6]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6
      });
      this.gridViewMFI.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewMFI.Location = new Point(0, 0);
      this.gridViewMFI.Margin = new Padding(4, 3, 4, 3);
      this.gridViewMFI.Name = "gridViewMFI";
      this.gridViewMFI.Size = new Size(794, 194);
      this.gridViewMFI.TabIndex = 0;
      this.OkBtn.Location = new Point(594, 211);
      this.OkBtn.Margin = new Padding(4, 3, 4, 3);
      this.OkBtn.Name = "OkBtn";
      this.OkBtn.Size = new Size(90, 21);
      this.OkBtn.TabIndex = 1;
      this.OkBtn.Text = "Select";
      this.OkBtn.UseVisualStyleBackColor = true;
      this.OkBtn.Click += new EventHandler(this.OkBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(692, 211);
      this.cancelBtn.Margin = new Padding(4, 3, 4, 3);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(90, 21);
      this.cancelBtn.TabIndex = 2;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(9f, 19f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(792, 241);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.OkBtn);
      this.Controls.Add((Control) this.gridViewMFI);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.Margin = new Padding(2, 3, 2, 3);
      this.MinimizeBox = false;
      this.Name = nameof (MedianFamilyIncomeLookup);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Median Family Income Lookup";
      this.ResumeLayout(false);
    }
  }
}
