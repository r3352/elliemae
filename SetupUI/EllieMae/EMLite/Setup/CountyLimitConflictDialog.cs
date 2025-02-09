// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CountyLimitConflictDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class CountyLimitConflictDialog : Form
  {
    private Dictionary<CountyLimit, CountyLimit> conflicts;
    private IContainer components;
    private Panel pnlConflicts;
    private Panel pnlButton;
    private Button btnOK;
    private Label lblNumberofConflicts;
    private RadioButton rdoCustomized;
    private RadioButton rdoImported;
    private RadioButton rdoMix;

    public CountyLimitConflictDialog(Dictionary<CountyLimit, CountyLimit> conflicts)
    {
      this.InitializeComponent();
      this.conflicts = conflicts;
      this.loadPageData();
    }

    private void loadPageData()
    {
      int y = 0;
      IEnumerator<CountyLimit> enumerator = (IEnumerator<CountyLimit>) this.conflicts.Keys.GetEnumerator();
      while (enumerator.MoveNext())
      {
        CountyLimitConflict countyLimitConflict = new CountyLimitConflict(enumerator.Current, this.conflicts[enumerator.Current]);
        countyLimitConflict.Location = new Point(0, y);
        y += countyLimitConflict.Height - 1;
        this.pnlConflicts.Controls.Add((Control) countyLimitConflict);
      }
      this.lblNumberofConflicts.Text = "There are " + (object) this.pnlConflicts.Controls.Count + " conflicting county limit records.  What do you want to do?";
      this.rdoMix.Checked = true;
    }

    public CountyLimit[] SelectedCountyLimits
    {
      get
      {
        List<CountyLimit> countyLimitList = new List<CountyLimit>();
        foreach (Control control in (ArrangedElementCollection) this.pnlConflicts.Controls)
        {
          if (control is CountyLimitConflict)
            countyLimitList.Add(((CountyLimitConflict) control).SelectedCountyLimit);
        }
        return countyLimitList.ToArray();
      }
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void option_Changed(object sender, EventArgs e)
    {
      CountyLimitConflict.Type type = CountyLimitConflict.Type.Mixed;
      if (this.rdoCustomized.Checked)
        type = CountyLimitConflict.Type.Customized;
      else if (this.rdoImported.Checked)
        type = CountyLimitConflict.Type.Imported;
      foreach (Control control in (ArrangedElementCollection) this.pnlConflicts.Controls)
      {
        if (control is CountyLimitConflict)
          ((CountyLimitConflict) control).SetRecord(type);
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
      this.pnlConflicts = new Panel();
      this.pnlButton = new Panel();
      this.btnOK = new Button();
      this.lblNumberofConflicts = new Label();
      this.rdoCustomized = new RadioButton();
      this.rdoImported = new RadioButton();
      this.rdoMix = new RadioButton();
      this.pnlButton.SuspendLayout();
      this.SuspendLayout();
      this.pnlConflicts.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlConflicts.AutoScroll = true;
      this.pnlConflicts.Location = new Point(12, 95);
      this.pnlConflicts.Name = "pnlConflicts";
      this.pnlConflicts.Size = new Size(491, 237);
      this.pnlConflicts.TabIndex = 0;
      this.pnlButton.Controls.Add((Control) this.btnOK);
      this.pnlButton.Dock = DockStyle.Bottom;
      this.pnlButton.Location = new Point(0, 351);
      this.pnlButton.Name = "pnlButton";
      this.pnlButton.Size = new Size(515, 47);
      this.pnlButton.TabIndex = 1;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(428, 12);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblNumberofConflicts.AutoSize = true;
      this.lblNumberofConflicts.Location = new Point(9, 9);
      this.lblNumberofConflicts.Name = "lblNumberofConflicts";
      this.lblNumberofConflicts.Size = new Size(35, 13);
      this.lblNumberofConflicts.TabIndex = 2;
      this.lblNumberofConflicts.Text = "label1";
      this.rdoCustomized.AutoSize = true;
      this.rdoCustomized.Location = new Point(13, 26);
      this.rdoCustomized.Name = "rdoCustomized";
      this.rdoCustomized.Size = new Size(138, 17);
      this.rdoCustomized.TabIndex = 3;
      this.rdoCustomized.TabStop = true;
      this.rdoCustomized.Text = "Use All Current Records";
      this.rdoCustomized.UseVisualStyleBackColor = true;
      this.rdoCustomized.CheckedChanged += new EventHandler(this.option_Changed);
      this.rdoImported.AutoSize = true;
      this.rdoImported.Location = new Point(13, 49);
      this.rdoImported.Name = "rdoImported";
      this.rdoImported.Size = new Size(143, 17);
      this.rdoImported.TabIndex = 4;
      this.rdoImported.TabStop = true;
      this.rdoImported.Text = "Use All Website Records";
      this.rdoImported.UseVisualStyleBackColor = true;
      this.rdoImported.CheckedChanged += new EventHandler(this.option_Changed);
      this.rdoMix.AutoSize = true;
      this.rdoMix.Location = new Point(13, 72);
      this.rdoMix.Name = "rdoMix";
      this.rdoMix.Size = new Size(110, 17);
      this.rdoMix.TabIndex = 5;
      this.rdoMix.TabStop = true;
      this.rdoMix.Text = "Individually Select";
      this.rdoMix.UseVisualStyleBackColor = true;
      this.rdoMix.CheckedChanged += new EventHandler(this.option_Changed);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(515, 398);
      this.Controls.Add((Control) this.rdoMix);
      this.Controls.Add((Control) this.rdoImported);
      this.Controls.Add((Control) this.rdoCustomized);
      this.Controls.Add((Control) this.lblNumberofConflicts);
      this.Controls.Add((Control) this.pnlButton);
      this.Controls.Add((Control) this.pnlConflicts);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (CountyLimitConflictDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "County Limit Conflict Records";
      this.pnlButton.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
