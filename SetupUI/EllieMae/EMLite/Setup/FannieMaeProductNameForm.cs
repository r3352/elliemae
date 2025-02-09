// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FannieMaeProductNameForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FannieMaeProductNameForm : Form
  {
    private DataTable productNameTable;
    private int? editItemIndex;
    private IContainer components;
    private Panel pnlFannieMaeProductName;
    private TextBox txtProductDisplayTime;
    private Label lblProductDisplayName;
    private TextBox txtFannieMaeProductName;
    private Label lblFannieMaeProductName;
    private TextBox txtDescription;
    private Label lblDescription;
    private Button btnOK;
    private Button btnCancel;

    public FannieMaeProductNameForm(DataTable productNameTable, int? editItemIndex)
    {
      this.InitializeComponent();
      this.editItemIndex = editItemIndex;
      this.productNameTable = productNameTable;
      if (!editItemIndex.HasValue)
        return;
      DataRow row = productNameTable.Rows[editItemIndex.Value];
      this.txtFannieMaeProductName.Text = (string) row[nameof (ProductName)];
      this.txtProductDisplayTime.Text = (string) row[nameof (DisplayName)];
      this.txtDescription.Text = (string) row[nameof (Description)];
    }

    public new string ProductName => this.txtFannieMaeProductName.Text.Trim();

    public string DisplayName => this.txtProductDisplayTime.Text.Trim();

    public string Description => this.txtDescription.Text.Trim();

    private bool ValidateForm(out string validationMessage)
    {
      bool flag = false;
      validationMessage = "";
      if (this.txtFannieMaeProductName.Text.Trim() == "" || this.txtProductDisplayTime.Text.Trim() == "")
      {
        validationMessage = "Product Name and Product Display Name cannot be blank.";
        return false;
      }
      if (this.txtFannieMaeProductName.Text.Contains<char>(','))
      {
        this.txtFannieMaeProductName.Focus();
        validationMessage = "Product name is invalid. Product Names cannot contain any commas.";
        return false;
      }
      for (int index = 0; index < this.productNameTable.Rows.Count; ++index)
      {
        if ((string) this.productNameTable.Rows[index]["ProductName"] == this.txtFannieMaeProductName.Text.Trim())
        {
          if (this.editItemIndex.HasValue)
          {
            int num = index;
            int? editItemIndex = this.editItemIndex;
            int valueOrDefault = editItemIndex.GetValueOrDefault();
            if (num == valueOrDefault & editItemIndex.HasValue)
              continue;
          }
          flag = true;
        }
      }
      if (!flag)
        return true;
      validationMessage = "The Product Name is already in use.";
      return false;
    }

    private bool Save()
    {
      string validationMessage;
      if (this.ValidateForm(out validationMessage))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, validationMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.Save())
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pnlFannieMaeProductName = new Panel();
      this.txtDescription = new TextBox();
      this.lblDescription = new Label();
      this.txtProductDisplayTime = new TextBox();
      this.lblProductDisplayName = new Label();
      this.txtFannieMaeProductName = new TextBox();
      this.lblFannieMaeProductName = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.pnlFannieMaeProductName.SuspendLayout();
      this.SuspendLayout();
      this.pnlFannieMaeProductName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtDescription);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.lblDescription);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtProductDisplayTime);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.lblProductDisplayName);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.txtFannieMaeProductName);
      this.pnlFannieMaeProductName.Controls.Add((Control) this.lblFannieMaeProductName);
      this.pnlFannieMaeProductName.Location = new Point(12, 13);
      this.pnlFannieMaeProductName.Margin = new Padding(3, 10, 3, 3);
      this.pnlFannieMaeProductName.Name = "pnlFannieMaeProductName";
      this.pnlFannieMaeProductName.Size = new Size(448, 95);
      this.pnlFannieMaeProductName.TabIndex = 0;
      this.txtDescription.Location = new Point(145, 64);
      this.txtDescription.MaxLength = 50;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(262, 20);
      this.txtDescription.TabIndex = 5;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(5, 67);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(100, 13);
      this.lblDescription.TabIndex = 4;
      this.lblDescription.Text = "Product Description";
      this.txtProductDisplayTime.Location = new Point(145, 37);
      this.txtProductDisplayTime.MaxLength = 50;
      this.txtProductDisplayTime.Name = "txtProductDisplayTime";
      this.txtProductDisplayTime.Size = new Size(262, 20);
      this.txtProductDisplayTime.TabIndex = 3;
      this.lblProductDisplayName.AutoSize = true;
      this.lblProductDisplayName.Location = new Point(5, 40);
      this.lblProductDisplayName.Name = "lblProductDisplayName";
      this.lblProductDisplayName.Size = new Size(112, 13);
      this.lblProductDisplayName.TabIndex = 2;
      this.lblProductDisplayName.Text = "Product Display Name";
      this.txtFannieMaeProductName.Location = new Point(145, 10);
      this.txtFannieMaeProductName.MaxLength = 50;
      this.txtFannieMaeProductName.Name = "txtFannieMaeProductName";
      this.txtFannieMaeProductName.Size = new Size(262, 20);
      this.txtFannieMaeProductName.TabIndex = 1;
      this.lblFannieMaeProductName.AutoSize = true;
      this.lblFannieMaeProductName.Location = new Point(5, 13);
      this.lblFannieMaeProductName.Name = "lblFannieMaeProductName";
      this.lblFannieMaeProductName.Size = new Size(134, 13);
      this.lblFannieMaeProductName.TabIndex = 0;
      this.lblFannieMaeProductName.Text = "Fannie Mae Product Name";
      this.btnOK.Location = new Point(301, 115);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Location = new Point(382, 115);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(472, 150);
      this.ControlBox = false;
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.pnlFannieMaeProductName);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FannieMaeProductNameForm);
      this.ShowIcon = false;
      this.Text = "Create/Edit Fannie Mae Product Names";
      this.pnlFannieMaeProductName.ResumeLayout(false);
      this.pnlFannieMaeProductName.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
