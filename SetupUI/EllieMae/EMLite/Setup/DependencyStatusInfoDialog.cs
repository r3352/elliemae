// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DependencyStatusInfoDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DependencyStatusInfoDialog : Form
  {
    private IContainer components;
    private PanelEx panelExTop;
    private Label lblInfoHeader;
    private Label lblDependencyDetails;
    private PanelEx panelExBottom;
    private Button btnCancel;
    private RichTextBox rtbStatusInfo;

    public DependencyStatusInfoDialog(string dependencyDetails, string info)
    {
      this.InitializeComponent();
      this.lblDependencyDetails.Text = dependencyDetails;
      this.rtbStatusInfo.Text = info;
    }

    public void setRuleImportUI() => this.lblInfoHeader.Visible = false;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panelExTop = new PanelEx();
      this.rtbStatusInfo = new RichTextBox();
      this.lblInfoHeader = new Label();
      this.lblDependencyDetails = new Label();
      this.panelExBottom = new PanelEx();
      this.btnCancel = new Button();
      this.panelExTop.SuspendLayout();
      this.panelExBottom.SuspendLayout();
      this.SuspendLayout();
      this.panelExTop.Controls.Add((Control) this.rtbStatusInfo);
      this.panelExTop.Controls.Add((Control) this.lblInfoHeader);
      this.panelExTop.Controls.Add((Control) this.lblDependencyDetails);
      this.panelExTop.Dock = DockStyle.Fill;
      this.panelExTop.Location = new Point(0, 0);
      this.panelExTop.Name = "panelExTop";
      this.panelExTop.Size = new Size(636, 318);
      this.panelExTop.TabIndex = 2;
      this.rtbStatusInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.rtbStatusInfo.BackColor = Color.White;
      this.rtbStatusInfo.Location = new Point(15, 67);
      this.rtbStatusInfo.Name = "rtbStatusInfo";
      this.rtbStatusInfo.ReadOnly = true;
      this.rtbStatusInfo.Size = new Size(609, 245);
      this.rtbStatusInfo.TabIndex = 1;
      this.rtbStatusInfo.Text = "";
      this.lblInfoHeader.AutoSize = true;
      this.lblInfoHeader.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblInfoHeader.Location = new Point(12, 15);
      this.lblInfoHeader.Name = "lblInfoHeader";
      this.lblInfoHeader.Size = new Size(386, 13);
      this.lblInfoHeader.TabIndex = 0;
      this.lblInfoHeader.Text = "The following dependency could not be found in the target system:";
      this.lblDependencyDetails.AutoSize = true;
      this.lblDependencyDetails.Location = new Point(12, 39);
      this.lblDependencyDetails.Name = "lblDependencyDetails";
      this.lblDependencyDetails.Size = new Size(21, 13);
      this.lblDependencyDetails.TabIndex = 0;
      this.lblDependencyDetails.Text = "{0}";
      this.panelExBottom.Controls.Add((Control) this.btnCancel);
      this.panelExBottom.Dock = DockStyle.Bottom;
      this.panelExBottom.Location = new Point(0, 318);
      this.panelExBottom.Name = "panelExBottom";
      this.panelExBottom.Size = new Size(636, 46);
      this.panelExBottom.TabIndex = 3;
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(549, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(636, 364);
      this.Controls.Add((Control) this.panelExTop);
      this.Controls.Add((Control) this.panelExBottom);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DependencyStatusInfoDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Source Info for Dependency";
      this.panelExTop.ResumeLayout(false);
      this.panelExTop.PerformLayout();
      this.panelExBottom.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
