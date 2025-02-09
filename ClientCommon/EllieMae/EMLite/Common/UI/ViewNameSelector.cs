// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ViewNameSelector
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ViewNameSelector : Form
  {
    private EllieMae.EMLite.ClientServer.TemplateSettingsType templateType;
    private string[] namesInUse;
    private string originalName;
    private IContainer components;
    private Label label1;
    private TextBox txtName;
    private DialogButtons dlgButtons;

    public ViewNameSelector(EllieMae.EMLite.ClientServer.TemplateSettingsType templateType, string[] namesInUse)
      : this(templateType, namesInUse, (string) null)
    {
    }

    public ViewNameSelector(
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateType,
      string[] namesInUse,
      string currentName)
    {
      this.templateType = templateType;
      this.namesInUse = namesInUse;
      this.originalName = currentName;
      this.InitializeComponent();
      FileSystemEntryFormatter systemEntryFormatter = new FileSystemEntryFormatter(this.txtName);
      if (!(this.originalName != ""))
        return;
      this.txtName.Text = this.originalName;
    }

    public string ViewName => this.txtName.Text;

    private void dlgButtons_OK(object sender, EventArgs e)
    {
      string str = this.txtName.Text.Trim();
      if (str == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must provide a name in the space provided.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this.templateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView && !FileSystem.IsValidName(str))
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The specified view name is invalid.  The name must be non-empty, cannot contain the backslash (\\), and cannot use special ASCII characters.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (str == this.originalName)
        this.DialogResult = DialogResult.Cancel;
      else if ((this.originalName == null || string.Compare(this.originalName, str, true) != 0) && this.templateExists(str))
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "A view with the name '" + str + "' already exists. You must provide a unique name for this view.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private bool templateExists(string name)
    {
      foreach (string strB in this.namesInUse)
      {
        if (string.Compare(name, strB, true) == 0)
          return true;
      }
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtName = new TextBox();
      this.dlgButtons = new DialogButtons();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(63, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "View Name";
      this.txtName.Location = new Point(78, 12);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(218, 20);
      this.txtName.TabIndex = 1;
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 41);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.Size = new Size(316, 44);
      this.dlgButtons.TabIndex = 2;
      this.dlgButtons.OK += new EventHandler(this.dlgButtons_OK);
      this.AcceptButton = (IButtonControl) this.dlgButtons;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(316, 85);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ViewNameSelector);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Duplicate/Rename View";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
