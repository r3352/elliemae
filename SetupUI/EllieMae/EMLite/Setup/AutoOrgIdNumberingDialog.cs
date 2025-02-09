// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AutoOrgIdNumberingDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AutoOrgIdNumberingDialog : Form
  {
    private Sessions.Session session;
    private bool isStartingNumberDirty;
    private IContainer components;
    private Label label1;
    private GradientPanel gradientPanel1;
    private TextBox txtStartingNumber;
    private Button btnCancel;
    private Label label3;
    private Label label2;
    private Button btnOk;
    private CheckBox chkEnableAutoCreate;
    private Panel panel1;

    public AutoOrgIdNumberingDialog(Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.txtStartingNumber.TextChanged -= new EventHandler(this.txtStartingNumber_TextChanged);
      this.chkEnableAutoCreate.Checked = this.txtStartingNumber.Enabled = string.Equals(this.session.ConfigurationManager.GetCompanySetting("POLICIES", "EnableAutoOrgIdNumbering"), "true", StringComparison.CurrentCultureIgnoreCase);
      this.txtStartingNumber.Text = this.session.ConfigurationManager.GetCompanySetting("POLICIES", "StartingOrgIdNumber");
      this.txtStartingNumber.TextChanged += new EventHandler(this.txtStartingNumber_TextChanged);
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (!this.validateData())
        return;
      Dictionary<string, object> settings = new Dictionary<string, object>();
      settings.Add("POLICIES.EnableAutoOrgIdNumbering", (object) this.chkEnableAutoCreate.Checked);
      if (this.isStartingNumberDirty)
      {
        settings.Add("POLICIES.StartingOrgIdNumber", (object) this.txtStartingNumber.Text.Trim());
        settings.Add("POLICIES.NextOrgIdNumber", (object) this.txtStartingNumber.Text.Trim());
      }
      Session.ServerManager.UpdateServerSettings((IDictionary) settings, true, false);
      this.DialogResult = DialogResult.OK;
    }

    private bool validateData()
    {
      if (this.chkEnableAutoCreate.Checked && this.txtStartingNumber.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Starting number for auto creating organization Id is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.txtStartingNumber.Enabled)
      {
        string s = this.txtStartingNumber.Text.Trim();
        if (string.IsNullOrEmpty(s))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Starting number for auto creating organization Id is required.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
        if (!double.TryParse(s, out double _) || s.Length > 15)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Starting number for auto creating organization Id is invalid. Starting number should be numeric and the maximum length is 15 digits", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      return true;
    }

    private void chkEnableAutoCreate_CheckedChanged(object sender, EventArgs e)
    {
      this.txtStartingNumber.Enabled = ((CheckBox) sender).Checked;
    }

    private void txtStartingNumber_TextChanged(object sender, EventArgs e)
    {
      this.isStartingNumberDirty = true;
    }

    private void txtStartingNumber_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsNumber(e.KeyChar))
        e.Handled = true;
      else
        e.Handled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AutoOrgIdNumberingDialog));
      this.label1 = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.txtStartingNumber = new TextBox();
      this.btnCancel = new Button();
      this.label3 = new Label();
      this.label2 = new Label();
      this.btnOk = new Button();
      this.chkEnableAutoCreate = new CheckBox();
      this.panel1 = new Panel();
      this.gradientPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      componentResourceManager.ApplyResources((object) this.label1, "label1");
      this.label1.BackColor = Color.Transparent;
      this.label1.Name = "label1";
      this.gradientPanel1.Controls.Add((Control) this.label1);
      componentResourceManager.ApplyResources((object) this.gradientPanel1, "gradientPanel1");
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Name = "gradientPanel1";
      componentResourceManager.ApplyResources((object) this.txtStartingNumber, "txtStartingNumber");
      this.txtStartingNumber.Name = "txtStartingNumber";
      this.txtStartingNumber.TextChanged += new EventHandler(this.txtStartingNumber_TextChanged);
      this.txtStartingNumber.KeyPress += new KeyPressEventHandler(this.txtStartingNumber_KeyPress);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      componentResourceManager.ApplyResources((object) this.btnCancel, "btnCancel");
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      componentResourceManager.ApplyResources((object) this.label3, "label3");
      this.label3.Name = "label3";
      componentResourceManager.ApplyResources((object) this.label2, "label2");
      this.label2.Name = "label2";
      componentResourceManager.ApplyResources((object) this.btnOk, "btnOk");
      this.btnOk.Name = "btnOk";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      componentResourceManager.ApplyResources((object) this.chkEnableAutoCreate, "chkEnableAutoCreate");
      this.chkEnableAutoCreate.Name = "chkEnableAutoCreate";
      this.chkEnableAutoCreate.UseVisualStyleBackColor = true;
      this.chkEnableAutoCreate.CheckedChanged += new EventHandler(this.chkEnableAutoCreate_CheckedChanged);
      this.panel1.Controls.Add((Control) this.chkEnableAutoCreate);
      this.panel1.Controls.Add((Control) this.btnOk);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.btnCancel);
      this.panel1.Controls.Add((Control) this.txtStartingNumber);
      componentResourceManager.ApplyResources((object) this.panel1, "panel1");
      this.panel1.Name = "panel1";
      this.AcceptButton = (IButtonControl) this.btnOk;
      componentResourceManager.ApplyResources((object) this, "$this");
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.Controls.Add((Control) this.gradientPanel1);
      this.Controls.Add((Control) this.panel1);
      this.FormBorderStyle = FormBorderStyle.Fixed3D;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AutoOrgIdNumberingDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
