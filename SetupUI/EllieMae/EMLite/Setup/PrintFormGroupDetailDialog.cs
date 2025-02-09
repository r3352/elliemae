// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PrintFormGroupDetailDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PrintFormGroupDetailDialog : Form
  {
    private Sessions.Session session;
    private AddPrintFormControl addPrintFormControl;
    private FileSystemEntry selectedEntry;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Label label1;
    private Label label2;
    private TextBox txtName;
    private TextBox txtDescription;
    private Panel panelFormSelector;

    public PrintFormGroupDetailDialog(Sessions.Session session, FileSystemEntry selectedEntry)
    {
      this.session = session;
      this.selectedEntry = selectedEntry;
      this.InitializeComponent();
      this.txtName.ReadOnly = true;
      this.initForm();
    }

    private void initForm()
    {
      this.addPrintFormControl = new AddPrintFormControl(this.session, this.selectedEntry == null || this.selectedEntry.IsPublic, false, (LoanData) null, false);
      this.panelFormSelector.Controls.Add((Control) this.addPrintFormControl);
      if (this.selectedEntry == null)
        return;
      this.txtName.Text = this.selectedEntry.Name;
      this.addPrintFormControl.LoadUserFormGroup(this.selectedEntry);
      if (this.selectedEntry.Properties.ContainsKey((object) "Description"))
        this.txtDescription.Text = this.selectedEntry.Properties[(object) "Description"].ToString();
      else
        this.txtDescription.Text = "";
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.GetSelectedForms().Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a form first.");
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public FormInfo[] GetSelectedForms() => this.addPrintFormControl.GetSelectedForms().ToArray();

    public string FormGroupDescription => this.txtDescription.Text;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dialogButtons1 = new DialogButtons();
      this.label1 = new Label();
      this.label2 = new Label();
      this.txtName = new TextBox();
      this.txtDescription = new TextBox();
      this.panelFormSelector = new Panel();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 521);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Save";
      this.dialogButtons1.Size = new Size(673, 44);
      this.dialogButtons1.TabIndex = 4;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(35, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Name";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 40);
      this.label2.Name = "label2";
      this.label2.Size = new Size(60, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Description";
      this.txtName.Location = new Point(98, 13);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(560, 20);
      this.txtName.TabIndex = 3;
      this.txtName.TabStop = false;
      this.txtDescription.Location = new Point(98, 37);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(560, 69);
      this.txtDescription.TabIndex = 0;
      this.panelFormSelector.Location = new Point(12, 118);
      this.panelFormSelector.Name = "panelFormSelector";
      this.panelFormSelector.Size = new Size(646, 397);
      this.panelFormSelector.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(673, 565);
      this.Controls.Add((Control) this.panelFormSelector);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (PrintFormGroupDetailDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Print Form Group Details";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
