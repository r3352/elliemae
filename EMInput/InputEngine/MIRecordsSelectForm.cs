// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MIRecordsSelectForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MIRecordsSelectForm : Form
  {
    private MITableControl tableControl;
    private IContainer components;
    private Panel panelList;
    private Button tbnSelect;
    private Button btnCancel;
    private Label label1;
    private Button btnView;

    public MIRecordsSelectForm(LoanTypeEnum loanType, MIRecord[] records, Sessions.Session session)
    {
      this.InitializeComponent();
      this.tableControl = new MITableControl(loanType, records, session);
      this.tableControl.RecordDoubleClick += new GVItemEventHandler(this.tableControl_RecordDoubleClick);
      this.panelList.Controls.Add((Control) this.tableControl);
    }

    private void tableControl_RecordDoubleClick(object sender, GVItemEventArgs e)
    {
      this.tbnSelect_Click((object) null, (EventArgs) null);
    }

    private void tbnSelect_Click(object sender, EventArgs e)
    {
      if (this.tableControl.CurrentSelectMI == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a record first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public MIRecord SelectedMI => this.tableControl.CurrentSelectMI;

    private void btnView_Click(object sender, EventArgs e) => this.tableControl.ViewRecord();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelList = new Panel();
      this.tbnSelect = new Button();
      this.btnCancel = new Button();
      this.label1 = new Label();
      this.btnView = new Button();
      this.SuspendLayout();
      this.panelList.Location = new Point(12, 41);
      this.panelList.Name = "panelList";
      this.panelList.Size = new Size(546, 313);
      this.panelList.TabIndex = 0;
      this.tbnSelect.Location = new Point(564, 41);
      this.tbnSelect.Name = "tbnSelect";
      this.tbnSelect.Size = new Size(75, 23);
      this.tbnSelect.TabIndex = 1;
      this.tbnSelect.Text = "&Select";
      this.tbnSelect.UseVisualStyleBackColor = true;
      this.tbnSelect.Click += new EventHandler(this.tbnSelect_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(564, 101);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(535, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "You have multiple MI templates that match your current loan information. Please select one template to populate:";
      this.btnView.Location = new Point(564, 72);
      this.btnView.Name = "btnView";
      this.btnView.Size = new Size(75, 23);
      this.btnView.TabIndex = 4;
      this.btnView.Text = "&View";
      this.btnView.UseVisualStyleBackColor = true;
      this.btnView.Click += new EventHandler(this.btnView_Click);
      this.AcceptButton = (IButtonControl) this.tbnSelect;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(650, 371);
      this.Controls.Add((Control) this.btnView);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.tbnSelect);
      this.Controls.Add((Control) this.panelList);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MIRecordsSelectForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select MI";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
