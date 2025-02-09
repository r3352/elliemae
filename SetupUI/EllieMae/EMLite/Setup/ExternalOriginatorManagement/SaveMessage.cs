// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.SaveMessage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement
{
  public class SaveMessage : Form
  {
    private string component;
    private IContainer components;
    private Label lblMsg;
    private Button btnSave;
    private Button btnCancel;
    private PictureBox pictureBox1;
    private Panel panel1;

    public SaveMessage(string component)
    {
      this.component = component;
      this.InitializeComponent();
      this.populateLabel();
    }

    public SaveMessage()
    {
      this.component = "this screen";
      this.InitializeComponent();
      this.populateLabel();
    }

    public SaveMessage(string component, string label)
    {
      this.component = component;
      this.InitializeComponent();
      this.populateLabel(label);
    }

    private void populateLabel() => this.lblMsg.Text = "Do you want to save changes?";

    private void populateLabel(string label) => this.lblMsg.Text = label;

    private void btnSave_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SaveMessage));
      this.lblMsg = new Label();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.pictureBox1 = new PictureBox();
      this.panel1 = new Panel();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.lblMsg.Location = new Point(5, 6);
      this.lblMsg.Name = "lblMsg";
      this.lblMsg.Size = new Size(275, 34);
      this.lblMsg.TabIndex = 0;
      this.lblMsg.Text = "The component information has been modified. Please save it before changing other settings.";
      this.btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSave.Location = new Point(182, 79);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 1;
      this.btnSave.Text = "Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(263, 79);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.pictureBox1.Image = (Image) Resources.Warning;
      this.pictureBox1.Location = new Point(11, 24);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(32, 40);
      this.pictureBox1.TabIndex = 3;
      this.pictureBox1.TabStop = false;
      this.panel1.Controls.Add((Control) this.lblMsg);
      this.panel1.Location = new Point(50, 24);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(283, 45);
      this.panel1.TabIndex = 4;
      this.AcceptButton = (IButtonControl) this.btnSave;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.BackColor = SystemColors.Window;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(349, 109);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SaveMessage);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
