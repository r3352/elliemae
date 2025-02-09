// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SaveSearchDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class SaveSearchDialog : Form
  {
    private TradeFilter currentFilter;
    private FileSystemEntry selectedEntry;
    private IContainer components;
    private CheckBox chkDefault;
    private DialogButtons dlgButtons;
    private TextBox txtName;
    private RadioButton radNew;
    private RadioButton radUpdate;
    private Label label1;

    public SaveSearchDialog(TradeFilter filter, FileSystemEntry filterEntry, bool allowOverwrite)
    {
      this.InitializeComponent();
      this.currentFilter = filter;
      this.selectedEntry = filterEntry;
      if (allowOverwrite)
        return;
      this.radNew.Checked = true;
      this.radUpdate.Enabled = false;
    }

    public FileSystemEntry SelectedEntry => this.selectedEntry;

    private void radNew_CheckedChanged(object sender, EventArgs e)
    {
      this.txtName.Enabled = this.radNew.Checked;
    }

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      FileSystemEntry filterEntry = this.getFilterEntry();
      if (filterEntry == null)
        return;
      try
      {
        if (this.selectedEntry != null && string.Compare(filterEntry.Name, this.selectedEntry.Name, true) != 0 && Session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeFilter, filterEntry))
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "A view with the name '" + filterEntry.Name + "' already exists. You must provide a unique name for this view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else if (string.Compare(filterEntry.Name, LoanSearchScreen.StandardView.Name, true) == 0)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "A view with the name '" + filterEntry.Name + "' already exists. You must provide a unique name for this view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          TradeFilterTemplate data = new TradeFilterTemplate(filterEntry.Name, "", this.currentFilter);
          Session.ConfigurationManager.SaveTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeFilter, filterEntry, (BinaryObject) (BinaryConvertibleObject) data);
          this.selectedEntry = filterEntry;
          if (this.chkDefault.Checked)
            Session.WritePrivateProfileString("Trading", "LastSearch", filterEntry.ToString());
          this.DialogResult = DialogResult.OK;
        }
      }
      catch (Exception ex)
      {
        ErrorDialog.Display(ex);
      }
    }

    private FileSystemEntry getFilterEntry()
    {
      if (this.radUpdate.Checked)
        return new FileSystemEntry("\\" + this.selectedEntry.Name, FileSystemEntry.Types.File, (string) null);
      if (FileSystem.IsValidFilename(this.txtName.Text))
        return new FileSystemEntry("\\" + this.txtName.Text.Trim(), FileSystemEntry.Types.File, (string) null);
      int num = (int) Utils.Dialog((IWin32Window) this, "The specified filter name is invalid. The name must be non-empty and cannot contain the backslash (\\) character.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      return (FileSystemEntry) null;
    }

    private void txtName_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\\')
        return;
      e.Handled = true;
    }

    private void txtName_TextChanged(object sender, EventArgs e)
    {
      if (!this.txtName.Text.Contains("\\"))
        return;
      this.txtName.Text = this.txtName.Text.Replace("\\", "");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkDefault = new CheckBox();
      this.dlgButtons = new DialogButtons();
      this.txtName = new TextBox();
      this.radNew = new RadioButton();
      this.radUpdate = new RadioButton();
      this.label1 = new Label();
      this.SuspendLayout();
      this.chkDefault.AutoSize = true;
      this.chkDefault.Checked = true;
      this.chkDefault.CheckState = CheckState.Checked;
      this.chkDefault.Location = new Point(12, 97);
      this.chkDefault.Name = "chkDefault";
      this.chkDefault.Size = new Size(137, 18);
      this.chkDefault.TabIndex = 11;
      this.chkDefault.Text = "Set as my default view";
      this.chkDefault.UseVisualStyleBackColor = true;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 129);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.OKText = "&Save";
      this.dlgButtons.Size = new Size(301, 47);
      this.dlgButtons.TabIndex = 10;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.txtName.Enabled = false;
      this.txtName.Location = new Point(76, 62);
      this.txtName.MaxLength = 100;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(211, 20);
      this.txtName.TabIndex = 9;
      this.txtName.TextChanged += new EventHandler(this.txtName_TextChanged);
      this.txtName.KeyPress += new KeyPressEventHandler(this.txtName_KeyPress);
      this.radNew.AutoSize = true;
      this.radNew.Location = new Point(12, 64);
      this.radNew.Name = "radNew";
      this.radNew.Size = new Size(65, 18);
      this.radNew.TabIndex = 8;
      this.radNew.Text = "Save as";
      this.radNew.UseVisualStyleBackColor = true;
      this.radNew.CheckedChanged += new EventHandler(this.radNew_CheckedChanged);
      this.radUpdate.AutoSize = true;
      this.radUpdate.Checked = true;
      this.radUpdate.Location = new Point(12, 41);
      this.radUpdate.Name = "radUpdate";
      this.radUpdate.Size = new Size(142, 18);
      this.radUpdate.TabIndex = 7;
      this.radUpdate.TabStop = true;
      this.radUpdate.Text = "Update the current view";
      this.radUpdate.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(277, 14);
      this.label1.TabIndex = 6;
      this.label1.Text = "Search filters, columns and sorting are saved in a view.";
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(301, 176);
      this.Controls.Add((Control) this.chkDefault);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.radNew);
      this.Controls.Add((Control) this.radUpdate);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SaveSearchDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Save View";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
