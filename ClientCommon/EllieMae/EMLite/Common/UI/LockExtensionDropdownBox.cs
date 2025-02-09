// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LockExtensionDropdownBox
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LockExtensionDropdownBox : UserControl
  {
    private Sessions.Session session;
    public bool isTradeExtension;
    private IContainer components;
    private ComboBox cmbLockExtension;
    private TextBox txtLockExtension;

    public event EventHandler DDLTextChanged;

    public LockExtensionDropdownBox(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.initialDropDown();
    }

    private void initialDropDown()
    {
      this.cmbLockExtension.Items.Clear();
      this.cmbLockExtension.SelectedText = "";
      if (this.session == null)
        return;
      Decimal num = 0M;
      Dictionary<int, Decimal> dictionary = new Dictionary<int, Decimal>();
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Policies");
      if (!this.isTradeExtension)
      {
        if (!(bool) serverSettings[(object) "Policies.EnableLockExtension"])
        {
          this.cmbLockExtension.Enabled = false;
          this.txtLockExtension.Enabled = false;
        }
        else if ((int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"] == 1)
        {
          this.cmbLockExtension.Visible = true;
          this.txtLockExtension.Visible = false;
          if ((bool) serverSettings[(object) "Policies.LockExtensionAllowFixedExt"])
          {
            LockExtensionPriceAdjustment[] priceAdjustments = this.session.ConfigurationManager.GetLockExtensionPriceAdjustments();
            if (priceAdjustments != null && priceAdjustments.Length != 0)
            {
              foreach (LockExtensionPriceAdjustment extensionPriceAdjustment in priceAdjustments)
                this.cmbLockExtension.Items.Add((object) extensionPriceAdjustment.DaysToExtend);
              this.cmbLockExtension.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            if ((bool) serverSettings[(object) "Policies.LockExtensionAllowDailyAdj"])
            {
              num = (Decimal) serverSettings[(object) "Policies.LockExtensionDailyPriceAdj"];
              if (priceAdjustments == null || priceAdjustments.Length == 0)
              {
                this.txtLockExtension.Visible = true;
                this.cmbLockExtension.Visible = false;
              }
              else
                this.cmbLockExtension.DropDownStyle = ComboBoxStyle.DropDown;
            }
            else
            {
              if (priceAdjustments != null && priceAdjustments.Length != 0)
                return;
              this.cmbLockExtension.Enabled = false;
            }
          }
          else if ((bool) serverSettings[(object) "Policies.LockExtensionAllowDailyAdj"])
          {
            num = (Decimal) serverSettings[(object) "Policies.LockExtensionDailyPriceAdj"];
            this.cmbLockExtension.Visible = false;
            this.txtLockExtension.Visible = true;
          }
          else
          {
            this.cmbLockExtension.Enabled = false;
            this.txtLockExtension.Enabled = false;
          }
        }
        else
        {
          this.cmbLockExtension.Visible = false;
          this.txtLockExtension.Visible = true;
          if ((int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"] != 2)
            return;
          this.txtLockExtension.Enabled = false;
        }
      }
      else
      {
        if (!this.isTradeExtension)
          return;
        this.cmbLockExtension.Enabled = true;
        this.txtLockExtension.Enabled = true;
        if ((bool) serverSettings[(object) "Policies.EnableLockExtension"])
        {
          if ((bool) serverSettings[(object) "Policies.LockExtensionAllowFixedExt"])
          {
            LockExtensionPriceAdjustment[] priceAdjustments = this.session.ConfigurationManager.GetLockExtensionPriceAdjustments();
            if (priceAdjustments != null && priceAdjustments.Length != 0)
            {
              foreach (LockExtensionPriceAdjustment extensionPriceAdjustment in priceAdjustments)
                this.cmbLockExtension.Items.Add((object) extensionPriceAdjustment.DaysToExtend);
              this.cmbLockExtension.DropDownStyle = ComboBoxStyle.DropDown;
            }
          }
          if ((int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"] == 1)
          {
            this.cmbLockExtension.Visible = true;
            this.txtLockExtension.Visible = false;
          }
          else
          {
            this.cmbLockExtension.Visible = false;
            this.txtLockExtension.Visible = true;
          }
        }
        else
        {
          this.cmbLockExtension.Items.Clear();
          this.cmbLockExtension.SelectedText = "";
          this.cmbLockExtension.Visible = false;
          this.txtLockExtension.Visible = true;
        }
      }
    }

    public void ResetSession(Sessions.Session session, bool isTradeExtension = false)
    {
      this.isTradeExtension = isTradeExtension;
      this.session = session;
      this.initialDropDown();
    }

    public LockExtensionDropdownBox()
      : this(Session.DefaultInstance)
    {
    }

    private void cmbLockExtension_TextChanged(object sender, EventArgs e)
    {
      this.valueChanged(sender, e);
    }

    public override string Text
    {
      get
      {
        return this.cmbLockExtension.Visible ? this.cmbLockExtension.Text.Trim() : this.txtLockExtension.Text.Trim();
      }
      set
      {
        if (this.cmbLockExtension.Visible)
          this.cmbLockExtension.Text = value;
        else
          this.txtLockExtension.Text = value;
      }
    }

    public new object Tag
    {
      get => this.cmbLockExtension.Visible ? this.cmbLockExtension.Tag : this.txtLockExtension.Tag;
      set
      {
        if (this.cmbLockExtension.Visible)
          this.cmbLockExtension.Tag = value;
        else
          this.txtLockExtension.Tag = value;
      }
    }

    private void cmbLockExtension_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.valueChanged(sender, e);
    }

    private void valueChanged(object sender, EventArgs e)
    {
      if (this.Text == "")
        return;
      if (this.Text.Equals("0"))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Data out of range: 1 - 999", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Text = "";
      }
      else
      {
        if (!Utils.IsInt((object) this.Text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Invalid data for days to extend.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.Text = "";
        }
        if (this.DDLTextChanged == null)
          return;
        this.DDLTextChanged((object) this, e);
      }
    }

    private void txtLockExtension_TextChanged(object sender, EventArgs e)
    {
      this.valueChanged(sender, e);
      this.txtLockExtension.Select(this.txtLockExtension.Text.Trim().Length, 0);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cmbLockExtension = new ComboBox();
      this.txtLockExtension = new TextBox();
      this.SuspendLayout();
      this.cmbLockExtension.Dock = DockStyle.Top;
      this.cmbLockExtension.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbLockExtension.FormattingEnabled = true;
      this.cmbLockExtension.Location = new Point(0, 0);
      this.cmbLockExtension.Margin = new Padding(3, 3, 3, 10);
      this.cmbLockExtension.Name = "cmbLockExtension";
      this.cmbLockExtension.Size = new Size(125, 22);
      this.cmbLockExtension.TabIndex = 0;
      this.cmbLockExtension.SelectedIndexChanged += new EventHandler(this.cmbLockExtension_SelectedIndexChanged);
      this.cmbLockExtension.TextChanged += new EventHandler(this.cmbLockExtension_TextChanged);
      this.txtLockExtension.Dock = DockStyle.Top;
      this.txtLockExtension.Location = new Point(0, 22);
      this.txtLockExtension.Name = "txtLockExtension";
      this.txtLockExtension.Size = new Size(125, 20);
      this.txtLockExtension.TabIndex = 1;
      this.txtLockExtension.TextChanged += new EventHandler(this.txtLockExtension_TextChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.Transparent;
      this.Controls.Add((Control) this.txtLockExtension);
      this.Controls.Add((Control) this.cmbLockExtension);
      this.Name = nameof (LockExtensionDropdownBox);
      this.Size = new Size(125, 23);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
