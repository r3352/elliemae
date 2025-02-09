// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.EPPSLoanProgramSelector
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class EPPSLoanProgramSelector : Form
  {
    public List<EPPSLoanProgram> selectedPrograms = new List<EPPSLoanProgram>();
    private IContainer components;
    private GroupContainer grpEPPSProgram;
    private GridView gvLP;
    private Button btnCancel;
    private Button btnSave;

    public EPPSLoanProgramSelector()
    {
      this.InitializeComponent();
      foreach (EPPSLoanProgram loanProgramsSetting in Session.ConfigurationManager.GetEPPSLoanProgramsSettings())
        this.gvLP.Items.Add(new GVItem()
        {
          SubItems = {
            (object) loanProgramsSetting.ProgramID,
            (object) loanProgramsSetting.ProgramName
          },
          Tag = (object) loanProgramsSetting
        });
      this.gvLP.Sort(1, SortOrder.Ascending);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      this.selectedPrograms = this.gvLP.SelectedItems.Select<GVItem, EPPSLoanProgram>((Func<GVItem, EPPSLoanProgram>) (t => (EPPSLoanProgram) t.Tag)).ToList<EPPSLoanProgram>();
      this.DialogResult = DialogResult.OK;
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
      this.grpEPPSProgram = new GroupContainer();
      this.gvLP = new GridView();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.grpEPPSProgram.SuspendLayout();
      this.SuspendLayout();
      this.grpEPPSProgram.Controls.Add((Control) this.gvLP);
      this.grpEPPSProgram.Dock = DockStyle.Top;
      this.grpEPPSProgram.HeaderForeColor = SystemColors.ControlText;
      this.grpEPPSProgram.Location = new Point(0, 0);
      this.grpEPPSProgram.Name = "grpEPPSProgram";
      this.grpEPPSProgram.Size = new Size(598, 543);
      this.grpEPPSProgram.TabIndex = 6;
      this.grpEPPSProgram.Text = "Add ICE PPE Loan Programs";
      this.gvLP.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Program ID";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Program Name";
      gvColumn2.Width = 300;
      this.gvLP.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvLP.Dock = DockStyle.Fill;
      this.gvLP.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLP.Location = new Point(1, 26);
      this.gvLP.Name = "gvLP";
      this.gvLP.Size = new Size(596, 516);
      this.gvLP.TabIndex = 104;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(299, 558);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 22;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(218, 558);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 22);
      this.btnSave.TabIndex = 23;
      this.btnSave.Text = "&Add";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(598, 595);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.grpEPPSProgram);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EPPSLoanProgramSelector);
      this.ShowIcon = false;
      this.Text = "ICE PPE Loan Programs";
      this.grpEPPSProgram.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
