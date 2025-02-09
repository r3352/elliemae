// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HelocTableContainer
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class HelocTableContainer : Form
  {
    private const string className = "HelocTableDialog";
    private Panel panelControl;
    private System.ComponentModel.Container components;
    private Button selectBtn;
    private TextBox textBoxName;
    private HelocDetailControl helocForm;
    private HelocRateTable helocTable;
    private string helocTableName = string.Empty;

    public HelocTableContainer(HelocRateTable helocTable, string tableName)
    {
      this.InitializeComponent();
      this.helocForm = new HelocDetailControl(helocTable, false);
      this.panelControl.Controls.Add((Control) this.helocForm);
      this.helocForm.HelocButtonClick += new EventHandler(this.helocForm_HelocButtonClick);
      this.textBoxName.Text = tableName;
      if (!helocTable.IsNewHELOC)
        return;
      this.Text = "Edit HELOC Historical Index Table";
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public HelocRateTable HelocTable => this.helocTable;

    public string HelocTableName => this.helocTableName;

    private void InitializeComponent()
    {
      this.panelControl = new Panel();
      this.selectBtn = new Button();
      this.textBoxName = new TextBox();
      this.SuspendLayout();
      this.panelControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelControl.Location = new Point(12, 39);
      this.panelControl.Name = "panelControl";
      this.panelControl.Size = new Size(536, 376);
      this.panelControl.TabIndex = 1;
      this.selectBtn.Font = new Font("Microsoft Sans Serif", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.selectBtn.Location = new Point(12, 12);
      this.selectBtn.Name = "selectBtn";
      this.selectBtn.Size = new Size(144, 24);
      this.selectBtn.TabIndex = 7;
      this.selectBtn.Text = "&Select From Template";
      this.selectBtn.Click += new EventHandler(this.selectBtn_Click);
      this.textBoxName.Location = new Point(164, 14);
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.ReadOnly = true;
      this.textBoxName.Size = new Size(280, 20);
      this.textBoxName.TabIndex = 8;
      this.textBoxName.TabStop = false;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(562, 420);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.selectBtn);
      this.Controls.Add((Control) this.panelControl);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (HelocTableContainer);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Edit HELOC Table";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void helocForm_HelocButtonClick(object sender, EventArgs e)
    {
      Button button = (Button) sender;
      if (button.Text == "&OK")
      {
        this.helocTable = this.helocForm.HelocTable;
        this.helocTableName = this.textBoxName.Text.Trim();
        this.DialogResult = DialogResult.OK;
      }
      else
      {
        if (!(button.Text == "&Cancel"))
          return;
        this.helocTable = (HelocRateTable) null;
        this.helocTableName = string.Empty;
        this.DialogResult = DialogResult.Cancel;
      }
    }

    private void selectBtn_Click(object sender, EventArgs e)
    {
      using (SelectHelocTableForm selectHelocTableForm = new SelectHelocTableForm(false))
      {
        if (selectHelocTableForm.ShowDialog((IWin32Window) this) != DialogResult.OK || this.helocForm.HelocCount > 0 && Utils.Dialog((IWin32Window) this, "Are you sure you want to overwrite current HELOC Draw/Repayment settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        HelocRateTable helocTable = (HelocRateTable) Session.ConfigurationManager.GetHelocTable(selectHelocTableForm.TableName, false);
        if (helocTable == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The HELOC table '" + selectHelocTableForm.TableName + "' can't be found.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this.textBoxName.Text = selectHelocTableForm.TableName;
          this.helocForm.ResetForm(helocTable);
        }
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "HelocTableDialog");
    }
  }
}
