// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.QMPaymentsForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class QMPaymentsForm : Form
  {
    private List<string[]> qmPayments;
    private IContainer components;
    private GridView lvwSchedule;
    private Button okBtn;

    public QMPaymentsForm(List<string[]> payments)
    {
      this.qmPayments = payments;
      this.InitializeComponent();
      this.load_schedule();
    }

    private void load_schedule()
    {
      this.lvwSchedule.Items.Clear();
      if (this.qmPayments == null)
        return;
      this.lvwSchedule.BeginUpdate();
      for (int index1 = 0; index1 < this.qmPayments.Count; ++index1)
      {
        GVItem gvItem = new GVItem(string.Concat((object) (index1 + 1)));
        for (int index2 = 0; index2 < this.qmPayments[index1].Length; ++index2)
          gvItem.SubItems.Add((object) this.qmPayments[index1][index2]);
        this.lvwSchedule.Items.Add(gvItem);
      }
      this.lvwSchedule.EndUpdate();
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
      GVColumn gvColumn8 = new GVColumn();
      this.lvwSchedule = new GridView();
      this.okBtn = new Button();
      this.SuspendLayout();
      this.lvwSchedule.AllowMultiselect = false;
      this.lvwSchedule.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColPayNo";
      gvColumn1.Text = "Pmt#";
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColPayDate";
      gvColumn2.SortMethod = GVSortMethod.Date;
      gvColumn2.Text = "Payment Date";
      gvColumn2.Width = 90;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColRate";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "Rate";
      gvColumn3.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn3.Width = 70;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColPayment";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Payment";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 90;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColPrincipal";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "Principal";
      gvColumn5.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn5.Width = 90;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColInterest";
      gvColumn6.SortMethod = GVSortMethod.Numeric;
      gvColumn6.Text = "Interest";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 90;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColMI";
      gvColumn7.SortMethod = GVSortMethod.Numeric;
      gvColumn7.Text = "MI";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 107;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColBalance";
      gvColumn8.SpringToFit = true;
      gvColumn8.Text = "Balance";
      gvColumn8.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn8.Width = 120;
      this.lvwSchedule.Columns.AddRange(new GVColumn[8]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.lvwSchedule.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.lvwSchedule.Location = new Point(12, 12);
      this.lvwSchedule.Name = "lvwSchedule";
      this.lvwSchedule.Size = new Size(709, 487);
      this.lvwSchedule.SortIconVisible = false;
      this.lvwSchedule.SortOption = GVSortOption.None;
      this.lvwSchedule.TabIndex = 52;
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(646, 505);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 47;
      this.okBtn.Text = "&Close";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(737, 541);
      this.Controls.Add((Control) this.lvwSchedule);
      this.Controls.Add((Control) this.okBtn);
      this.MinimizeBox = false;
      this.Name = nameof (QMPaymentsForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "QM Payment Schedule";
      this.ResumeLayout(false);
    }
  }
}
