// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.HelocTableContainer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  internal class HelocTableContainer : Form
  {
    private Panel panelControl;
    private Label label1;
    private TextBox textBoxName;
    private System.ComponentModel.Container components;
    private HelocDetailControl helocForm;
    private Sessions.Session session;
    private bool useNewHELOCHistoricTable;
    private string tableName = "";
    private HelocRateTable helocTable;

    public HelocTableContainer(
      HelocRateTable helocTable,
      string tableName,
      bool readOnly,
      Sessions.Session session)
      : this(helocTable, tableName, readOnly, session, false)
    {
    }

    public HelocTableContainer(
      HelocRateTable helocTable,
      string tableName,
      bool readOnly,
      Sessions.Session session,
      bool useNewHELOCHistoricTable)
    {
      this.session = session;
      this.useNewHELOCHistoricTable = useNewHELOCHistoricTable;
      this.tableName = tableName;
      this.InitializeComponent();
      if (this.useNewHELOCHistoricTable)
        this.Text = "Edit HELOC Historical Index Table";
      this.helocForm = new HelocDetailControl(helocTable, readOnly, this.session, this.useNewHELOCHistoricTable);
      this.panelControl.Controls.Add((Control) this.helocForm);
      if (helocTable != null)
        this.textBoxName.Text = tableName;
      this.helocForm.HelocButtonClick += new EventHandler(this.helocForm_HelocButtonClick);
      if (!this.useNewHELOCHistoricTable)
        return;
      this.panelControl.Top -= 6;
    }

    public HelocTableContainer(HelocRateTable helocTable, string tableName, bool readOnly)
      : this(helocTable, tableName, readOnly, Session.DefaultInstance)
    {
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string TableName => this.tableName;

    public HelocRateTable HelocTable => this.helocTable;

    private void InitializeComponent()
    {
      this.panelControl = new Panel();
      this.textBoxName = new TextBox();
      this.label1 = new Label();
      this.SuspendLayout();
      this.panelControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelControl.Location = new Point(12, 38);
      this.panelControl.Name = "panelControl";
      this.panelControl.Size = new Size(538, 362);
      this.panelControl.TabIndex = 1;
      this.textBoxName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxName.Location = new Point(140, 14);
      this.textBoxName.MaxLength = 256;
      this.textBoxName.Name = "textBoxName";
      this.textBoxName.Size = new Size(330, 20);
      this.textBoxName.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(16, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(104, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "HELOC Table Name";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(562, 414);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.textBoxName);
      this.Controls.Add((Control) this.panelControl);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MinimizeBox = false;
      this.Name = nameof (HelocTableContainer);
      this.ShowIcon = false;
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
        if (this.textBoxName.Text.Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Table name can't be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.textBoxName.Focus();
        }
        else
        {
          string name = this.textBoxName.Text.Trim();
          if (this.session.ConfigurationManager.HelocTableObjectExists(name) && (this.tableName == "" || this.tableName.ToLower() != name.ToLower() && this.tableName != ""))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The HELOC Table already contains a table named '" + name + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.textBoxName.Focus();
          }
          else if (this.helocForm.HelocTable == null)
          {
            int num1 = (int) Utils.Dialog((IWin32Window) this, "Table can't be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          else
          {
            this.tableName = name;
            this.helocTable = this.helocForm.HelocTable;
            this.DialogResult = DialogResult.OK;
          }
        }
      }
      else
      {
        if (!(button.Text == "&Cancel"))
          return;
        this.helocTable = (HelocRateTable) null;
        this.tableName = "";
        this.DialogResult = DialogResult.Cancel;
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
