// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddNewSetting
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddNewSetting : Form
  {
    private int settingTypeId = -1;
    private string settingTypeKey = string.Empty;
    private List<ExternalSettingValue> settingValues;
    private ExternalSettingValue externalSettingValue;
    private Sessions.Session session;
    private IContainer components;
    private TextBox txtSettingName;
    private Label lblSetting;
    private Button btnOK;
    private Button btnCancel;
    private TextBox txtCode;
    private Label lblCode;

    public AddNewSetting(
      Sessions.Session session,
      int settingTypeId,
      string settingTypeKey,
      List<ExternalSettingValue> settingValues,
      ExternalSettingValue externalSettingValue = null)
    {
      this.InitializeComponent();
      this.settingTypeId = settingTypeId;
      this.settingTypeKey = settingTypeKey;
      this.session = session;
      this.externalSettingValue = externalSettingValue;
      this.settingValues = settingValues;
      if (this.externalSettingValue != null)
        this.Text = "Edit Setting";
      if (settingTypeKey == "Group")
      {
        this.lblCode.Text = "Group Code";
        this.txtCode.Visible = true;
        this.lblSetting.Text = "Group Name";
      }
      else
      {
        this.lblCode.Visible = false;
        this.txtCode.Visible = false;
        if (settingTypeKey == "Document Category")
          this.lblSetting.Text = "Category";
        else if (settingTypeKey == "ICE PPE Rate Sheet")
          this.lblSetting.Text = "Rate Sheet";
        else
          this.lblSetting.Text = settingTypeKey;
      }
      this.initialPageValue();
    }

    public List<ExternalSettingValue> modifiedSettingValues => this.settingValues;

    private void AddorUpdateSetting()
    {
      if (this.externalSettingValue == null)
      {
        int sortId = 0;
        if (this.settingValues != null && this.settingValues.Count > 0)
          sortId = this.settingValues[this.settingValues.Count - 1].sortId + 1;
        this.externalSettingValue = new ExternalSettingValue(-1, this.settingTypeId, this.txtCode.Text == null ? string.Empty : this.txtCode.Text.Trim(), this.txtSettingName.Text.Trim(), sortId);
        this.externalSettingValue.settingId = this.session.ConfigurationManager.AddExternalOrgSettingValue(this.externalSettingValue);
        this.settingValues.Add(this.externalSettingValue);
      }
      else
      {
        this.externalSettingValue.settingValue = this.txtSettingName.Text.Trim();
        this.externalSettingValue.settingCode = this.txtCode.Text.Trim();
        this.session.ConfigurationManager.UpdateExternalOrgSettingValue(this.externalSettingValue);
        foreach (ExternalSettingValue settingValue in this.settingValues)
        {
          if (settingValue.settingId == this.externalSettingValue.settingId)
          {
            settingValue.settingValue = this.externalSettingValue.settingValue;
            break;
          }
        }
      }
    }

    private void initialPageValue()
    {
      if (this.externalSettingValue == null)
        return;
      this.txtSettingName.Text = this.externalSettingValue.settingValue;
      this.txtCode.Text = this.externalSettingValue.settingCode;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.txtCode.Visible && this.txtCode.Text.Trim() == string.Empty && this.txtSettingName.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Setting value should not be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.txtCode.Visible && this.txtCode.Text.Trim() == string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Group Code is a required field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.txtSettingName.Text.Trim() == string.Empty)
      {
        if (this.txtCode.Visible)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Group Name is a required field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Setting value should not be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else
      {
        bool flag = false;
        foreach (ExternalSettingValue settingValue in this.settingValues)
        {
          if (this.externalSettingValue != null)
          {
            if (this.txtCode.Visible)
            {
              if (settingValue.settingId != this.externalSettingValue.settingId && settingValue.settingCode == this.txtCode.Text.Trim())
              {
                flag = true;
                break;
              }
            }
            else if (settingValue.settingId != this.externalSettingValue.settingId && settingValue.settingValue == this.txtSettingName.Text.Trim())
            {
              flag = true;
              break;
            }
          }
          else if (this.txtCode.Visible)
          {
            if (this.txtCode.Visible && settingValue.settingCode == this.txtCode.Text.Trim())
            {
              flag = true;
              break;
            }
          }
          else if (settingValue.settingValue == this.txtSettingName.Text.Trim())
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          if (this.txtCode.Visible)
          {
            int num5 = (int) Utils.Dialog((IWin32Window) this, "Group Code already exists. Group Code should be unique.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
          else
          {
            int num6 = (int) Utils.Dialog((IWin32Window) this, "Setting value already exists. Setting value should be unique.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
        else
        {
          this.AddorUpdateSetting();
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AddNewSetting));
      this.txtSettingName = new TextBox();
      this.lblSetting = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.txtCode = new TextBox();
      this.lblCode = new Label();
      this.SuspendLayout();
      this.txtSettingName.Location = new Point(112, 39);
      this.txtSettingName.MaxLength = 250;
      this.txtSettingName.Name = "txtSettingName";
      this.txtSettingName.Size = new Size(295, 20);
      this.txtSettingName.TabIndex = 2;
      this.lblSetting.AutoSize = true;
      this.lblSetting.Location = new Point(23, 42);
      this.lblSetting.Name = "lblSetting";
      this.lblSetting.Size = new Size(37, 13);
      this.lblSetting.TabIndex = 2;
      this.lblSetting.Text = "Status";
      this.btnOK.Location = new Point(228, 79);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(326, 79);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.txtCode.Location = new Point(112, 13);
      this.txtCode.MaxLength = 50;
      this.txtCode.Name = "txtCode";
      this.txtCode.Size = new Size(295, 20);
      this.txtCode.TabIndex = 1;
      this.lblCode.AutoSize = true;
      this.lblCode.Location = new Point(23, 17);
      this.lblCode.Name = "lblCode";
      this.lblCode.RightToLeft = RightToLeft.No;
      this.lblCode.Size = new Size(35, 13);
      this.lblCode.TabIndex = 7;
      this.lblCode.Text = "label1";
      this.lblCode.TextAlign = ContentAlignment.TopRight;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(430, 114);
      this.Controls.Add((Control) this.lblCode);
      this.Controls.Add((Control) this.txtCode);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtSettingName);
      this.Controls.Add((Control) this.lblSetting);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddNewSetting);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add New Setting";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
