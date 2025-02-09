// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MessageInfoDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MessageInfoDialog : Form
  {
    private ToolTip toolTip;
    private IContainer components;
    private Button closeBtn;
    private Label message1Lbl;
    private Label message2Lbl;
    private TextBox borMessage1Txt;
    private TextBox borMessage2Txt;

    public MessageInfoDialog(RepAndWarrantTracker.ItemMessage itemMessage)
    {
      this.InitializeComponent();
      this.toolTip = new ToolTip()
      {
        ShowAlways = true,
        InitialDelay = 0,
        ReshowDelay = 0,
        AutoPopDelay = 0
      };
      this.Text = itemMessage.Title;
      this.borMessage1Txt.Text = itemMessage.Messages[0].Message ?? string.Empty;
      this.toolTip.SetToolTip((Control) this.borMessage1Txt, itemMessage.Messages[0].Tooltip ?? string.Empty);
      this.borMessage2Txt.Text = itemMessage.Messages.Count > 1 ? itemMessage.Messages[1].Message ?? string.Empty : string.Empty;
      this.toolTip.SetToolTip((Control) this.borMessage2Txt, itemMessage.Messages.Count > 1 ? itemMessage.Messages[1].Tooltip ?? string.Empty : string.Empty);
      if (itemMessage.Messages.Count != 1)
        return;
      this.borMessage1Txt.Size = new Size(401, 174);
      this.message1Lbl.Text = "Message";
    }

    private void closeBtn_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MessageInfoDialog));
      this.closeBtn = new Button();
      this.message1Lbl = new Label();
      this.message2Lbl = new Label();
      this.borMessage1Txt = new TextBox();
      this.borMessage2Txt = new TextBox();
      this.SuspendLayout();
      this.closeBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.closeBtn.DialogResult = DialogResult.OK;
      this.closeBtn.Location = new Point(341, 227);
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(75, 23);
      this.closeBtn.TabIndex = 0;
      this.closeBtn.Text = "&Close";
      this.closeBtn.UseVisualStyleBackColor = true;
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.message1Lbl.AutoSize = true;
      this.message1Lbl.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.message1Lbl.Location = new Point(12, 19);
      this.message1Lbl.Name = "message1Lbl";
      this.message1Lbl.Size = new Size(111, 13);
      this.message1Lbl.TabIndex = 1;
      this.message1Lbl.Text = "Borrower Message";
      this.message2Lbl.AutoSize = true;
      this.message2Lbl.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.message2Lbl.Location = new Point(12, 122);
      this.message2Lbl.Name = "message2Lbl";
      this.message2Lbl.Size = new Size(125, 13);
      this.message2Lbl.TabIndex = 2;
      this.message2Lbl.Text = "Coborrower Message";
      this.borMessage1Txt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.borMessage1Txt.Location = new Point(15, 38);
      this.borMessage1Txt.Multiline = true;
      this.borMessage1Txt.Name = "borMessage1Txt";
      this.borMessage1Txt.ReadOnly = true;
      this.borMessage1Txt.ScrollBars = ScrollBars.Vertical;
      this.borMessage1Txt.Size = new Size(401, 71);
      this.borMessage1Txt.TabIndex = 3;
      this.borMessage2Txt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.borMessage2Txt.Location = new Point(15, 141);
      this.borMessage2Txt.Multiline = true;
      this.borMessage2Txt.Name = "borMessage2Txt";
      this.borMessage2Txt.ReadOnly = true;
      this.borMessage2Txt.ScrollBars = ScrollBars.Vertical;
      this.borMessage2Txt.Size = new Size(401, 71);
      this.borMessage2Txt.TabIndex = 4;
      this.AcceptButton = (IButtonControl) this.closeBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.closeBtn;
      this.ClientSize = new Size(428, 259);
      this.Controls.Add((Control) this.borMessage1Txt);
      this.Controls.Add((Control) this.message1Lbl);
      this.Controls.Add((Control) this.closeBtn);
      this.Controls.Add((Control) this.borMessage2Txt);
      this.Controls.Add((Control) this.message2Lbl);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MessageInfoDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Fannie Mae - PIW Offered";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
