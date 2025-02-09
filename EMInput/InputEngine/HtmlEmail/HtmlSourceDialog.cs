// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.HtmlEmail.HtmlSourceDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.HtmlEmail
{
  public class HtmlSourceDialog : Form
  {
    private string html;
    private IContainer components;
    private TextBox txtSource;
    private Button btnCancel;
    private Button btnUpdate;
    private BorderPanel pnlBottom;

    public HtmlSourceDialog(string html, bool readOnly)
    {
      this.InitializeComponent();
      this.setWindowSize();
      this.txtSource.Text = html;
      this.txtSource.SelectionStart = 0;
      this.txtSource.SelectionLength = 0;
      if (!readOnly)
        return;
      this.txtSource.ReadOnly = readOnly;
      this.btnUpdate.Visible = false;
      this.btnCancel.Text = "Close";
    }

    private void setWindowSize()
    {
      if (Form.ActiveForm != null)
      {
        Form form = Form.ActiveForm;
        while (form.Owner != null)
          form = form.Owner;
        this.Width = Convert.ToInt32((double) form.Width * 0.95);
        this.Height = Convert.ToInt32((double) form.Height * 0.95);
      }
      else
      {
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Width = Convert.ToInt32((double) workingArea.Width * 0.95);
        workingArea = Screen.PrimaryScreen.WorkingArea;
        this.Height = Convert.ToInt32((double) workingArea.Height * 0.95);
      }
    }

    public string Html => this.html;

    private void btnUpdate_Click(object sender, EventArgs e)
    {
      string str = this.txtSource.Text.Trim();
      try
      {
        mshtml.IHTMLDocument2 htmlDocument2 = (mshtml.IHTMLDocument2) new HTMLDocumentClass();
        htmlDocument2.write((object) str);
        htmlDocument2.close();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The following error occurred when trying to load the html source code:\n\n" + ex.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return;
      }
      this.html = str;
      this.DialogResult = DialogResult.OK;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtSource = new TextBox();
      this.btnCancel = new Button();
      this.btnUpdate = new Button();
      this.pnlBottom = new BorderPanel();
      this.pnlBottom.SuspendLayout();
      this.SuspendLayout();
      this.txtSource.BorderStyle = BorderStyle.None;
      this.txtSource.Dock = DockStyle.Fill;
      this.txtSource.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtSource.HideSelection = false;
      this.txtSource.Location = new Point(0, 0);
      this.txtSource.MaxLength = 0;
      this.txtSource.Multiline = true;
      this.txtSource.Name = "txtSource";
      this.txtSource.ScrollBars = ScrollBars.Both;
      this.txtSource.Size = new Size(556, 316);
      this.txtSource.TabIndex = 0;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(472, 8);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnUpdate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnUpdate.Location = new Point(392, 8);
      this.btnUpdate.Name = "btnUpdate";
      this.btnUpdate.Size = new Size(75, 22);
      this.btnUpdate.TabIndex = 0;
      this.btnUpdate.Text = "Update";
      this.btnUpdate.UseVisualStyleBackColor = true;
      this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
      this.pnlBottom.Borders = AnchorStyles.Top;
      this.pnlBottom.Controls.Add((Control) this.btnCancel);
      this.pnlBottom.Controls.Add((Control) this.btnUpdate);
      this.pnlBottom.Dock = DockStyle.Bottom;
      this.pnlBottom.Location = new Point(0, 316);
      this.pnlBottom.Name = "pnlBottom";
      this.pnlBottom.Size = new Size(556, 38);
      this.pnlBottom.TabIndex = 2;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(556, 354);
      this.Controls.Add((Control) this.txtSource);
      this.Controls.Add((Control) this.pnlBottom);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Margin = new Padding(3, 4, 3, 4);
      this.MinimizeBox = false;
      this.Name = nameof (HtmlSourceDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "View Source";
      this.pnlBottom.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
