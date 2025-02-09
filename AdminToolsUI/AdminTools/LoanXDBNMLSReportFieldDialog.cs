// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanXDBNMLSReportFieldDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoanXDBNMLSReportFieldDialog : Form
  {
    private string reportName;
    private IContainer components;
    private Label lblHeader;
    private CheckBox chkHide;
    private Button btnNo;
    private GridView gvFields;
    private Label label2;
    private Button btnYes;

    public LoanXDBNMLSReportFieldDialog(List<object[]> missingFields, string reportName)
    {
      this.reportName = reportName;
      this.InitializeComponent();
      foreach (object[] missingField in missingFields)
      {
        StandardField standardField = (StandardField) missingField[0];
        FieldPairInfo fieldPairInfo = (FieldPairInfo) missingField[1];
        this.gvFields.Items.Add(new GVItem()
        {
          SubItems = {
            [0] = {
              Text = standardField.FieldID,
              SortValue = (object) this.toSortValue(standardField.FieldID)
            },
            [1] = {
              Text = standardField.Category == FieldCategory.Borrower || standardField.Category == FieldCategory.Coborrower ? LoanXDBManager.ToPairOrder(fieldPairInfo.PairIndex == 0 ? 1 : fieldPairInfo.PairIndex) : ""
            },
            [2] = {
              Text = standardField.Description
            }
          }
        });
      }
      this.gvFields.Sort(0, SortOrder.Ascending);
      this.lblHeader.Text = this.lblHeader.Text.Replace("%ReportName%", reportName);
    }

    public bool HideDialog => this.chkHide.Checked;

    private string toSortValue(string fieldId)
    {
      try
      {
        return int.Parse(fieldId).ToString("00000000");
      }
      catch
      {
        return fieldId;
      }
    }

    private void btnYes_Click(object sender, EventArgs e)
    {
      if (this.gvFields.Items.Count > 100)
      {
        if (Utils.Dialog((IWin32Window) this, "Due to the large number of fields required by the " + this.reportName + ", it is recommended that you add these fields to your Reporting Database only if you are required to run this report." + Environment.NewLine + Environment.NewLine + "Are you sure you want to add these fields to your Reporting Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
      }
      this.DialogResult = DialogResult.Yes;
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
      this.lblHeader = new Label();
      this.chkHide = new CheckBox();
      this.btnNo = new Button();
      this.label2 = new Label();
      this.btnYes = new Button();
      this.gvFields = new GridView();
      this.SuspendLayout();
      this.lblHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblHeader.Location = new Point(7, 7);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(467, 34);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "In order to generate the %ReportName%, the following fields must be added to your Reporting Database:";
      this.chkHide.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkHide.AutoSize = true;
      this.chkHide.Location = new Point(10, 287);
      this.chkHide.Name = "chkHide";
      this.chkHide.Size = new Size(106, 18);
      this.chkHide.TabIndex = 3;
      this.chkHide.Text = "Do not ask again";
      this.chkHide.UseVisualStyleBackColor = true;
      this.btnNo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNo.DialogResult = DialogResult.Cancel;
      this.btnNo.Location = new Point(399, 285);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(75, 23);
      this.btnNo.TabIndex = 2;
      this.btnNo.Text = "No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 261);
      this.label2.Name = "label2";
      this.label2.Size = new Size(327, 14);
      this.label2.TabIndex = 4;
      this.label2.Text = "Would you like to add these fields to the Reporting Database now?";
      this.btnYes.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnYes.Location = new Point(315, 285);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(75, 23);
      this.btnYes.TabIndex = 1;
      this.btnYes.Text = "&Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnYes.Click += new EventHandler(this.btnYes_Click);
      this.gvFields.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Custom;
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column3";
      gvColumn2.Text = "Pair";
      gvColumn2.Width = 50;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column2";
      gvColumn3.SpringToFit = true;
      gvColumn3.Text = "Description";
      gvColumn3.Width = 309;
      this.gvFields.Columns.AddRange(new GVColumn[3]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3
      });
      this.gvFields.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvFields.Location = new Point(10, 45);
      this.gvFields.Name = "gvFields";
      this.gvFields.Size = new Size(461, 210);
      this.gvFields.TabIndex = 3;
      this.AcceptButton = (IButtonControl) this.btnYes;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnNo;
      this.ClientSize = new Size(483, 317);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.gvFields);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.chkHide);
      this.Controls.Add((Control) this.lblHeader);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanXDBNMLSReportFieldDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Reporting Database";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
