// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.SaveViewDialog
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class SaveViewDialog : Form
  {
    private DocumentTrackingView currentDocumentView;
    private ConditionTrackingView currentConditionView;
    private FileSystemEntry selectedEntry;
    private EllieMae.EMLite.ClientServer.TemplateSettingsType type;
    private string[] namesInUse;
    private IContainer components;
    private CheckBox chkDefault;
    private DialogButtons dlgButtons;
    private TextBox txtName;
    private RadioButton radNew;
    private RadioButton radUpdate;
    private Label label1;

    public SaveViewDialog(
      EllieMae.EMLite.ClientServer.TemplateSettingsType type,
      BinaryConvertibleObject view,
      string[] namesInUse,
      bool allowOverwrite)
    {
      this.InitializeComponent();
      this.type = type;
      if (this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
        this.currentDocumentView = (DocumentTrackingView) view;
      else
        this.currentConditionView = (ConditionTrackingView) view;
      this.namesInUse = namesInUse;
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
      try
      {
        if (this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
        {
          this.currentDocumentView.IsDefault = this.chkDefault.Checked;
          if (this.radNew.Checked)
          {
            if (!FileSystem.IsValidName(this.txtName.Text))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The specified view name is invalid.  The name must be non-empty, cannot contain the backslash (\\), and cannot use special ASCII characters.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return;
            }
            if (this.isNameInUse(this.txtName.Text))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "A view with the name '" + this.txtName.Text + "' already exists. You must provide a unique name for this view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }
            this.currentDocumentView.Name = this.txtName.Text.Trim();
            Session.EfolderDocTrackViewManager.CreateView(this.currentDocumentView);
          }
          else
            Session.EfolderDocTrackViewManager.UpdateView(this.currentDocumentView);
        }
        else
        {
          FileSystemEntry viewEntry = this.getViewEntry();
          if (viewEntry == null)
            return;
          Session.ConfigurationManager.SaveTemplateSettings(this.type, viewEntry, (BinaryObject) (BinaryConvertibleObject) this.currentConditionView);
          this.selectedEntry = viewEntry;
        }
        if (this.chkDefault.Checked)
        {
          if (this.type != EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
            Session.WritePrivateProfileString(this.type.ToString(), "DefaultView", this.selectedEntry.ToString());
          if (this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.PreliminaryConditionTrackingView)
            Session.StartupInfo.DefaultPreliminaryConditionTrackingView = this.currentConditionView;
          else if (this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.PostClosingConditionTrackingView)
            Session.StartupInfo.DefaultPostClosingConditionTrackingView = this.currentConditionView;
          else if (this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionTrackingView)
            Session.StartupInfo.DefaultUnderwritingConditionTrackingView = this.currentConditionView;
          else if (this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.SellConditionTrackingView)
            Session.StartupInfo.DefaultSellConditionTrackingView = this.currentConditionView;
          else if (this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.EnhancedConditionTrackingView)
            Session.StartupInfo.DefaultEnhancedConditionTrackingView = this.currentConditionView;
        }
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        ErrorDialog.Display(ex);
      }
    }

    private FileSystemEntry getViewEntry()
    {
      if (this.radUpdate.Checked)
        return this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView ? new FileSystemEntry("\\" + this.currentDocumentView.Name, FileSystemEntry.Types.File, Session.UserID) : new FileSystemEntry("\\" + this.currentConditionView.Name, FileSystemEntry.Types.File, Session.UserID);
      if (!FileSystem.IsValidFilename(this.txtName.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The specified view name is invalid. The name must be non-empty and cannot contain the backslash (\\) character.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (FileSystemEntry) null;
      }
      if (this.isNameInUse(this.txtName.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A view with the name '" + this.txtName.Text + "' already exists. You must provide a unique name for this view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return (FileSystemEntry) null;
      }
      if (this.type == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView)
      {
        this.currentDocumentView.Name = this.txtName.Text.Trim();
        return new FileSystemEntry("\\" + this.currentDocumentView.Name, FileSystemEntry.Types.File, Session.UserID);
      }
      this.currentConditionView.Name = this.txtName.Text.Trim();
      return new FileSystemEntry("\\" + this.currentConditionView.Name, FileSystemEntry.Types.File, Session.UserID);
    }

    private bool isNameInUse(string name)
    {
      foreach (string strB in this.namesInUse)
      {
        if (string.Compare(name, strB, true) == 0)
          return true;
      }
      return false;
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
      this.chkDefault.Location = new Point(14, 90);
      this.chkDefault.Name = "chkDefault";
      this.chkDefault.Size = new Size(137, 18);
      this.chkDefault.TabIndex = 11;
      this.chkDefault.Text = "Set as my default view";
      this.chkDefault.UseVisualStyleBackColor = true;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 118);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.OKText = "&Save";
      this.dlgButtons.Size = new Size(306, 47);
      this.dlgButtons.TabIndex = 10;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.txtName.Enabled = false;
      this.txtName.Location = new Point(77, 56);
      this.txtName.MaxLength = 100;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(212, 20);
      this.txtName.TabIndex = 9;
      this.radNew.AutoSize = true;
      this.radNew.Location = new Point(13, 57);
      this.radNew.Name = "radNew";
      this.radNew.Size = new Size(65, 18);
      this.radNew.TabIndex = 8;
      this.radNew.Text = "Save as";
      this.radNew.UseVisualStyleBackColor = true;
      this.radNew.CheckedChanged += new EventHandler(this.radNew_CheckedChanged);
      this.radUpdate.AutoSize = true;
      this.radUpdate.Checked = true;
      this.radUpdate.Location = new Point(13, 36);
      this.radUpdate.Name = "radUpdate";
      this.radUpdate.Size = new Size(142, 18);
      this.radUpdate.TabIndex = 7;
      this.radUpdate.TabStop = true;
      this.radUpdate.Text = "Update the current view";
      this.radUpdate.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(11, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(277, 14);
      this.label1.TabIndex = 6;
      this.label1.Text = "Search filters, columns and sorting are saved in a view.";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(306, 165);
      this.Controls.Add((Control) this.chkDefault);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.radNew);
      this.Controls.Add((Control) this.radUpdate);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SaveViewDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Save View";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
