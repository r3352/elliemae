// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddPrintFormDialog
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddPrintFormDialog : Form
  {
    private Sessions.Session session;
    private CustomFormListControl customFormListControl;
    private OutputFormListControl standardFormListControl;
    private Hashtable existingForms;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private TabControlEx tabControlEx1;
    private TabPageEx tabPageStandard;
    private TabPageEx tabPageCustom;

    public AddPrintFormDialog(Sessions.Session session, Hashtable existingForms)
    {
      this.session = session;
      this.existingForms = existingForms;
      this.InitializeComponent();
      this.initForm();
    }

    private void initForm()
    {
      this.customFormListControl = new CustomFormListControl(this.session);
      this.tabPageCustom.Controls.Add((Control) this.customFormListControl);
      this.standardFormListControl = new OutputFormListControl(this.session, (LoanData) null, false, false, true);
      this.tabPageStandard.Controls.Add((Control) this.standardFormListControl);
    }

    internal FormItemInfo SelectedForm
    {
      get
      {
        return this.tabControlEx1.SelectedPage == this.tabPageCustom ? this.customFormListControl.GetSelectedLetters()[0] : this.standardFormListControl.GetSelectedForms()[0];
      }
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      if (this.tabControlEx1.SelectedPage == this.tabPageCustom && this.customFormListControl.SelectedFormCount == 0 || this.tabControlEx1.SelectedPage == this.tabPageStandard && this.standardFormListControl.SelectedFormCount == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a print form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
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
      this.dialogButtons1 = new DialogButtons();
      this.tabControlEx1 = new TabControlEx();
      this.tabPageStandard = new TabPageEx();
      this.tabPageCustom = new TabPageEx();
      this.tabControlEx1.SuspendLayout();
      this.SuspendLayout();
      this.dialogButtons1.DialogResult = DialogResult.Cancel;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 429);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.OKText = "&Select";
      this.dialogButtons1.Size = new Size(525, 44);
      this.dialogButtons1.TabIndex = 34;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.tabControlEx1.Location = new Point(12, 12);
      this.tabControlEx1.Name = "tabControlEx1";
      this.tabControlEx1.Size = new Size(501, 411);
      this.tabControlEx1.TabHeight = 20;
      this.tabControlEx1.TabIndex = 35;
      this.tabControlEx1.TabPages.Add(this.tabPageStandard);
      this.tabControlEx1.TabPages.Add(this.tabPageCustom);
      this.tabControlEx1.Text = "tabControlEx1";
      this.tabPageStandard.BackColor = Color.Transparent;
      this.tabPageStandard.Location = new Point(1, 23);
      this.tabPageStandard.Name = "tabPageStandard";
      this.tabPageStandard.TabIndex = 0;
      this.tabPageStandard.TabWidth = 100;
      this.tabPageStandard.Text = "Standard Forms";
      this.tabPageStandard.Value = (object) "Standard Forms";
      this.tabPageCustom.BackColor = Color.Transparent;
      this.tabPageCustom.Location = new Point(1, 23);
      this.tabPageCustom.Name = "tabPageCustom";
      this.tabPageCustom.TabIndex = 0;
      this.tabPageCustom.TabWidth = 100;
      this.tabPageCustom.Text = "Custom Forms";
      this.tabPageCustom.Value = (object) "Custom Forms";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.dialogButtons1;
      this.ClientSize = new Size(525, 473);
      this.Controls.Add((Control) this.tabControlEx1);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddPrintFormDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Print Form";
      this.tabControlEx1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
