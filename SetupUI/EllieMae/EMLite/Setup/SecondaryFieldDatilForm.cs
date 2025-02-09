// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecondaryFieldDatilForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SecondaryFieldDatilForm : Form
  {
    private Label label1;
    private Button btnOK;
    private Button buttonCancel;
    private TextBox boxOption;
    private System.ComponentModel.Container components;
    private ArrayList currentList;
    private SecondaryFieldTypes type;
    private string newOption = string.Empty;

    public SecondaryFieldDatilForm(SecondaryFieldTypes type, string val, ArrayList currentList)
    {
      this.type = type;
      this.currentList = currentList;
      this.InitializeComponent();
      if (this.type == SecondaryFieldTypes.BasePrice)
      {
        this.Text = "Base Price Options";
      }
      else
      {
        switch (type)
        {
          case SecondaryFieldTypes.BaseRate:
            this.Text = "Base Rate Options";
            break;
          case SecondaryFieldTypes.BaseMargin:
            this.Text = "Base ARM Margin Options";
            break;
          case SecondaryFieldTypes.ProfitabilityOption:
            this.Text = "Profitability Options";
            break;
          case SecondaryFieldTypes.CommitmentTypeOption:
            this.Text = "Commitment Type Options";
            break;
          case SecondaryFieldTypes.TradeDescriptionOption:
            this.Text = "Trade Description Options";
            break;
          case SecondaryFieldTypes.LockTypeOption:
            this.Text = "Lock Type Options";
            break;
          case SecondaryFieldTypes.SecurityTerm:
            this.Text = "Security Term Options";
            break;
          case SecondaryFieldTypes.PreferredCarrier:
            this.Text = "Preferred Carrier Options";
            break;
          default:
            this.Text = "Payouts Options";
            break;
        }
      }
      if (!(val != string.Empty))
        return;
      this.boxOption.Text = val;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.boxOption = new TextBox();
      this.label1 = new Label();
      this.btnOK = new Button();
      this.buttonCancel = new Button();
      this.SuspendLayout();
      this.boxOption.Location = new Point(112, 20);
      this.boxOption.MaxLength = 256;
      this.boxOption.Name = "boxOption";
      this.boxOption.Size = new Size(316, 20);
      this.boxOption.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(16, 24);
      this.label1.Name = "label1";
      this.label1.Size = new Size(88, 13);
      this.label1.TabIndex = 57;
      this.label1.Text = "Dropdown value:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.btnOK.Location = new Point(268, 52);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 24);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(352, 52);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 24);
      this.buttonCancel.TabIndex = 2;
      this.buttonCancel.Text = "Cancel";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(444, 85);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.boxOption);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SecondaryFieldDatilForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Dropdown List";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public string NewOption => this.newOption;

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.newOption = this.boxOption.Text.Trim();
      if (this.newOption.Trim() == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Dropdown option cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        for (int index = 0; index < this.currentList.Count; ++index)
        {
          if (string.Compare(this.newOption, this.currentList[index].ToString(), true) == 0)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, (this.type != SecondaryFieldTypes.BasePrice ? (this.type != SecondaryFieldTypes.BaseRate ? (this.type != SecondaryFieldTypes.BaseMargin ? (this.type != SecondaryFieldTypes.ProfitabilityOption ? (this.type != SecondaryFieldTypes.CommitmentTypeOption ? (this.type != SecondaryFieldTypes.TradeDescriptionOption ? (this.type != SecondaryFieldTypes.LockTypeOption ? (this.type != SecondaryFieldTypes.SecurityTerm ? (this.type != SecondaryFieldTypes.PreferredCarrier ? "Payouts Options" : "Preferred Carrier Options") : "Security Term Options") : "Lock Type Options") : "Trade Description Options") : "Commitment Type Options") : "Profitability Options") : "Base Margin Options") : "Base Rate Options") : "Base Price Options") + " already contains a value '" + this.newOption + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        this.DialogResult = DialogResult.OK;
      }
    }
  }
}
