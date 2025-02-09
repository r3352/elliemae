// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.MFAEnabledProducts
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class MFAEnabledProducts : Form
  {
    private const string MFA_Enabled_Products = "Password.mfaEnabledProducts";
    private EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts mfaProd;
    private EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts initialValue;
    private static readonly string sw = Tracing.SwOutsideLoan;
    private IContainer components;
    private GroupContainer groupContainer1;
    private Button btnClose;
    private Panel panel1;
    private Label label1;
    private CheckBox chkConnectProducts;
    private Button btnOK;
    private CheckBox chkEncompass;
    private CheckBox chkConnectAdminProduct;

    public MFAEnabledProducts()
    {
      this.InitializeComponent();
      this.mfaProd = this.initialValue = Session.ServerManager.GetServerSetting("Password.mfaEnabledProducts", false) != null ? (EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts) Convert.ToInt32(Session.ServerManager.GetServerSetting("Password.mfaEnabledProducts", false)) : EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.Disabled;
      this.chkConnectAdminProduct.Visible = true;
      this.chkEncompass.Visible = this.IsMFAEnabledInSC();
      this.updateCheckboxStatus();
    }

    private bool IsMFAEnabledInSC()
    {
      string str = "";
      try
      {
        str = SmartClientUtils.GetAttribute(Session.StartupInfo.ServerInstanceName, "EncompassBE", "AppLauncher.exe", "TokenLoginOnly");
      }
      catch (Exception ex)
      {
        Tracing.Log(MFAEnabledProducts.sw, TraceLevel.Error, this.Name, "Error while checking IsTokenLoginEnabled." + ex.StackTrace);
      }
      return !string.IsNullOrEmpty(str) && !(str.Trim() == "0");
    }

    private void updateCheckboxStatus()
    {
      this.chkConnectAdminProduct.Checked = false;
      this.chkConnectProducts.Checked = false;
      this.chkEncompass.Checked = false;
      if ((this.mfaProd & EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.Encompass) == EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.Encompass)
        this.chkEncompass.Checked = true;
      if ((this.mfaProd & EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.ConnectAdminProduct) == EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.ConnectAdminProduct)
        this.chkConnectAdminProduct.Checked = true;
      if ((this.mfaProd & EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.ConnectProduct) != EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.ConnectProduct)
        return;
      this.chkConnectProducts.Checked = true;
    }

    private void getmfaEnabledProductsStatus()
    {
      this.mfaProd = EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.Disabled;
      if (this.chkEncompass.Checked)
        this.mfaProd |= EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.Encompass;
      if (this.chkConnectProducts.Checked)
        this.mfaProd |= EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.ConnectProduct;
      if (!this.chkConnectAdminProduct.Checked)
        return;
      this.mfaProd |= EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts.ConnectAdminProduct;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.getmfaEnabledProductsStatus();
      Session.ServerManager.UpdateServerSetting("Password.mfaEnabledProducts", (object) (int) this.mfaProd, false);
      if (this.mfaProd == this.initialValue)
        return;
      Session.InsertSystemAuditRecord((SystemAuditRecord) new ServerSettingsAuditRecord(Session.UserID, Session.UserInfo.FullName, ActionType.ServerSettingsChanged, DateTime.Now, AuditObjectType.AdminTools_ServerSettings, ServerSettingsCategory.Password, AdminToolsServerSetting.Password_MFAEnabledProducts, Convert.ToString((int) this.initialValue), Convert.ToString((int) this.mfaProd)));
    }

    public EllieMae.EMLite.ClientServer.Configuration.MFAEnabledProducts ProductsEnabledMFA
    {
      get => this.mfaProd;
    }

    private void panel1_Paint(object sender, PaintEventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MFAEnabledProducts));
      this.groupContainer1 = new GroupContainer();
      this.btnOK = new Button();
      this.panel1 = new Panel();
      this.chkEncompass = new CheckBox();
      this.chkConnectAdminProduct = new CheckBox();
      this.chkConnectProducts = new CheckBox();
      this.label1 = new Label();
      this.btnClose = new Button();
      this.groupContainer1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.btnOK);
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Controls.Add((Control) this.btnClose);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(457, 381);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Enable MFA";
      this.btnOK.Location = new Point(289, 351);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 43;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.panel1.Controls.Add((Control) this.chkEncompass);
      this.panel1.Controls.Add((Control) this.chkConnectAdminProduct);
      this.panel1.Controls.Add((Control) this.chkConnectProducts);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(1, 26);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(455, 319);
      this.panel1.TabIndex = 42;
      this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);
      this.chkEncompass.AutoSize = true;
      this.chkEncompass.Location = new Point(46, 84);
      this.chkEncompass.Name = "chkEncompass";
      this.chkEncompass.Size = new Size(81, 17);
      this.chkEncompass.TabIndex = 1;
      this.chkEncompass.Text = "Encompass";
      this.chkEncompass.UseVisualStyleBackColor = true;
      this.chkConnectAdminProduct.AutoSize = true;
      this.chkConnectAdminProduct.Location = new Point(46, 117);
      this.chkConnectAdminProduct.Name = "chkConnectAdminProduct";
      this.chkConnectAdminProduct.Size = new Size(138, 17);
      this.chkConnectAdminProduct.TabIndex = 1;
      this.chkConnectAdminProduct.Text = "Connect Admin Portal";
      this.chkConnectAdminProduct.UseVisualStyleBackColor = true;
      this.chkConnectProducts.AutoSize = true;
      this.chkConnectProducts.Location = new Point(46, 49);
      this.chkConnectProducts.Name = "chkConnectProducts";
      this.chkConnectProducts.Size = new Size(177, 17);
      this.chkConnectProducts.TabIndex = 1;
      this.chkConnectProducts.Text = "Connect Products (Encompass Web)";
      this.chkConnectProducts.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(250, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select the products that should have MFA enabled.";
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.DialogResult = DialogResult.OK;
      this.btnClose.Location = new Point(370, 351);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 41;
      this.btnClose.Text = "Cancel";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(457, 381);
      this.Controls.Add((Control) this.groupContainer1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (MFAEnabledProducts);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Enable MFA";
      this.groupContainer1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
