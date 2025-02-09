// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DynamicDataManagement.DDMFieldDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.DynamicDataManagement
{
  public class DDMFieldDialog : Form
  {
    private IContainer components;
    private ComboBox cboComortgagor;
    private Label label1;
    private Label label6;
    private TextBox textBoxID;
    private TextBox textBoxDescription;
    private Label label3;
    private Button btnOK;
    private Button btnCancel;

    public DDMField DdmField { get; set; }

    public string Description { get; set; }

    public int PairIndex { get; set; }

    public DDMFieldDialog(DDMField dbField)
    {
      this.InitializeComponent();
      if (dbField != null)
      {
        this.DdmField = new DDMField()
        {
          ComortgagorPair = dbField.ComortgagorPair,
          Description = dbField.Description,
          FieldId = dbField.FieldId
        };
        this.textBoxID.Text = dbField.FieldId;
        this.textBoxDescription.Text = dbField.Description;
        if (dbField.ComortgagorPair > 0)
          this.cboComortgagor.SelectedIndex = dbField.ComortgagorPair - 1;
      }
      this.textBoxDescription.TextChanged += new EventHandler(this.ddmField_Changed);
      this.cboComortgagor.SelectedIndexChanged += new EventHandler(this.ddmField_Changed);
    }

    private void ddmField_Changed(object sender, EventArgs e) => this.DdmField.IsDirty = true;

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.Description = this.textBoxDescription.Text;
      this.PairIndex = this.cboComortgagor.SelectedIndex;
      this.DdmField.ComortgagorPair = this.PairIndex + 1;
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
      this.cboComortgagor = new ComboBox();
      this.label1 = new Label();
      this.label6 = new Label();
      this.textBoxID = new TextBox();
      this.textBoxDescription = new TextBox();
      this.label3 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.cboComortgagor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboComortgagor.FormattingEnabled = true;
      this.cboComortgagor.Items.AddRange(new object[6]
      {
        (object) "1st",
        (object) "2nd",
        (object) "3rd",
        (object) "4th",
        (object) "5th",
        (object) "6th"
      });
      this.cboComortgagor.Location = new Point(85, 51);
      this.cboComortgagor.Name = "cboComortgagor";
      this.cboComortgagor.Size = new Size(92, 21);
      this.cboComortgagor.TabIndex = 22;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 10);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 13);
      this.label1.TabIndex = 24;
      this.label1.Text = "Field ID";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 55);
      this.label6.Name = "label6";
      this.label6.Size = new Size(70, 13);
      this.label6.TabIndex = 26;
      this.label6.Text = "Borrower Pair";
      this.textBoxID.Location = new Point(85, 7);
      this.textBoxID.Name = "textBoxID";
      this.textBoxID.ReadOnly = true;
      this.textBoxID.Size = new Size(240, 20);
      this.textBoxID.TabIndex = 23;
      this.textBoxID.TabStop = false;
      this.textBoxDescription.Location = new Point(85, 29);
      this.textBoxDescription.MaxLength = 100;
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.Size = new Size(240, 20);
      this.textBoxDescription.TabIndex = 21;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3, 32);
      this.label3.Name = "label3";
      this.label3.Size = new Size(60, 13);
      this.label3.TabIndex = 25;
      this.label3.Text = "Description";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(170, 81);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 27;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(250, 81);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 28;
      this.btnCancel.Text = "&Cancel";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(341, 110);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.cboComortgagor);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.textBoxID);
      this.Controls.Add((Control) this.textBoxDescription);
      this.Controls.Add((Control) this.label3);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DDMFieldDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Select Borrower Pair";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
