// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TriggerAddFieldDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TriggerAddFieldDialog : Form
  {
    private FieldSettings fieldSettings;
    private TextBox[] textFields;
    private IContainer components;
    private TextBox textBoxID6;
    private Label label10;
    private TextBox textBoxID5;
    private Label label5;
    private TextBox textBoxID4;
    private Label label4;
    private TextBox textBoxID3;
    private Label label3;
    private TextBox textBoxID2;
    private Label label2;
    private Label label9;
    private TextBox textBoxID10;
    private Label label6;
    private TextBox textBoxID9;
    private Label label7;
    private Button moreBtn;
    private TextBox textBoxID8;
    private Label label8;
    private Button okBtn;
    private TextBox textBoxID7;
    private TextBox textBoxID1;
    private Button cancelBtn;
    private Label label1;
    private GroupBox groupBox1;

    public event EventHandler AddFields;

    public TriggerAddFieldDialog(FieldSettings fieldSettings)
    {
      this.InitializeComponent();
      this.fieldSettings = fieldSettings;
      this.textFields = new TextBox[10]
      {
        this.textBoxID1,
        this.textBoxID2,
        this.textBoxID3,
        this.textBoxID4,
        this.textBoxID5,
        this.textBoxID6,
        this.textBoxID7,
        this.textBoxID8,
        this.textBoxID9,
        this.textBoxID10
      };
    }

    public string[] GetSelectedValues()
    {
      List<string> stringList = new List<string>();
      for (int index = 0; index < this.textFields.Length; ++index)
      {
        string str = this.textFields[index].Text.Trim();
        if (str != "")
          stringList.Add(str);
      }
      return stringList.ToArray();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (!this.validateFields())
        return;
      this.onAddFields();
      this.DialogResult = DialogResult.OK;
    }

    private void moreBtn_Click(object sender, EventArgs e)
    {
      if (!this.validateFields())
        return;
      this.onAddFields();
      this.clearFields();
    }

    private void onAddFields()
    {
      if (this.AddFields == null)
        return;
      this.AddFields((object) this, EventArgs.Empty);
    }

    private void clearFields()
    {
      for (int index = 0; index < this.textFields.Length; ++index)
        this.textFields[index].Text = "";
    }

    private bool validateFields()
    {
      for (int index = 0; index < this.textFields.Length; ++index)
      {
        string fieldId = this.textFields[index].Text.Trim();
        if (fieldId != "")
        {
          FieldDefinition field = EncompassFields.GetField(fieldId, this.fieldSettings);
          if (field == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + fieldId + "' is not a valid Field ID.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.textFields[index].SelectAll();
            this.textFields[index].Focus();
            return false;
          }
          if (field is VirtualField)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The value '" + fieldId + "' is a virtual field and cannot be used to trigger an action.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.textFields[index].SelectAll();
            this.textFields[index].Focus();
            return false;
          }
        }
      }
      return true;
    }

    private void TriggerAddFieldDialog_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.cancelBtn.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.textBoxID6 = new TextBox();
      this.label10 = new Label();
      this.textBoxID5 = new TextBox();
      this.label5 = new Label();
      this.textBoxID4 = new TextBox();
      this.label4 = new Label();
      this.textBoxID3 = new TextBox();
      this.label3 = new Label();
      this.textBoxID2 = new TextBox();
      this.label2 = new Label();
      this.label9 = new Label();
      this.textBoxID10 = new TextBox();
      this.label6 = new Label();
      this.textBoxID9 = new TextBox();
      this.label7 = new Label();
      this.moreBtn = new Button();
      this.textBoxID8 = new TextBox();
      this.label8 = new Label();
      this.okBtn = new Button();
      this.textBoxID7 = new TextBox();
      this.textBoxID1 = new TextBox();
      this.cancelBtn = new Button();
      this.label1 = new Label();
      this.groupBox1 = new GroupBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      this.textBoxID6.Location = new Point(60, 156);
      this.textBoxID6.Name = "textBoxID6";
      this.textBoxID6.Size = new Size(204, 20);
      this.textBoxID6.TabIndex = 10;
      this.label10.AutoSize = true;
      this.label10.Location = new Point(8, 160);
      this.label10.Name = "label10";
      this.label10.Size = new Size(43, 13);
      this.label10.TabIndex = 8;
      this.label10.Text = "Field ID";
      this.textBoxID5.Location = new Point(60, 129);
      this.textBoxID5.Name = "textBoxID5";
      this.textBoxID5.Size = new Size(204, 20);
      this.textBoxID5.TabIndex = 5;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(8, 132);
      this.label5.Name = "label5";
      this.label5.Size = new Size(43, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Field ID";
      this.textBoxID4.Location = new Point(60, 102);
      this.textBoxID4.Name = "textBoxID4";
      this.textBoxID4.Size = new Size(204, 20);
      this.textBoxID4.TabIndex = 4;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(8, 105);
      this.label4.Name = "label4";
      this.label4.Size = new Size(43, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "Field ID";
      this.textBoxID3.Location = new Point(60, 75);
      this.textBoxID3.Name = "textBoxID3";
      this.textBoxID3.Size = new Size(204, 20);
      this.textBoxID3.TabIndex = 3;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(8, 77);
      this.label3.Name = "label3";
      this.label3.Size = new Size(43, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Field ID";
      this.textBoxID2.Location = new Point(60, 48);
      this.textBoxID2.Name = "textBoxID2";
      this.textBoxID2.Size = new Size(204, 20);
      this.textBoxID2.TabIndex = 2;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(8, 51);
      this.label2.Name = "label2";
      this.label2.Size = new Size(43, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Field ID";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(8, 188);
      this.label9.Name = "label9";
      this.label9.Size = new Size(43, 13);
      this.label9.TabIndex = 9;
      this.label9.Text = "Field ID";
      this.textBoxID10.Location = new Point(60, 264);
      this.textBoxID10.Name = "textBoxID10";
      this.textBoxID10.Size = new Size(204, 20);
      this.textBoxID10.TabIndex = 15;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(8, 268);
      this.label6.Name = "label6";
      this.label6.Size = new Size(43, 13);
      this.label6.TabIndex = 17;
      this.label6.Text = "Field ID";
      this.textBoxID9.Location = new Point(60, 237);
      this.textBoxID9.Name = "textBoxID9";
      this.textBoxID9.Size = new Size(204, 20);
      this.textBoxID9.TabIndex = 14;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(8, 240);
      this.label7.Name = "label7";
      this.label7.Size = new Size(43, 13);
      this.label7.TabIndex = 16;
      this.label7.Text = "Field ID";
      this.moreBtn.Location = new Point(129, 309);
      this.moreBtn.Name = "moreBtn";
      this.moreBtn.Size = new Size(75, 23);
      this.moreBtn.TabIndex = 12;
      this.moreBtn.Text = "Add &More";
      this.moreBtn.Click += new EventHandler(this.moreBtn_Click);
      this.textBoxID8.Location = new Point(60, 210);
      this.textBoxID8.Name = "textBoxID8";
      this.textBoxID8.Size = new Size(204, 20);
      this.textBoxID8.TabIndex = 13;
      this.label8.AutoSize = true;
      this.label8.Location = new Point(8, 212);
      this.label8.Name = "label8";
      this.label8.Size = new Size(43, 13);
      this.label8.TabIndex = 12;
      this.label8.Text = "Field ID";
      this.okBtn.Location = new Point(45, 309);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 10;
      this.okBtn.Text = "&Add";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.textBoxID7.Location = new Point(60, 183);
      this.textBoxID7.Name = "textBoxID7";
      this.textBoxID7.Size = new Size(204, 20);
      this.textBoxID7.TabIndex = 11;
      this.textBoxID1.Location = new Point(60, 21);
      this.textBoxID1.Name = "textBoxID1";
      this.textBoxID1.Size = new Size(204, 20);
      this.textBoxID1.TabIndex = 1;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(213, 309);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 11;
      this.cancelBtn.Text = "&Cancel";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 23);
      this.label1.Name = "label1";
      this.label1.Size = new Size(43, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Field ID";
      this.groupBox1.Controls.Add((Control) this.textBoxID10);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.textBoxID9);
      this.groupBox1.Controls.Add((Control) this.label7);
      this.groupBox1.Controls.Add((Control) this.textBoxID8);
      this.groupBox1.Controls.Add((Control) this.label8);
      this.groupBox1.Controls.Add((Control) this.textBoxID7);
      this.groupBox1.Controls.Add((Control) this.label9);
      this.groupBox1.Controls.Add((Control) this.textBoxID6);
      this.groupBox1.Controls.Add((Control) this.label10);
      this.groupBox1.Controls.Add((Control) this.textBoxID5);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.textBoxID4);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.textBoxID3);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.textBoxID2);
      this.groupBox1.Controls.Add((Control) this.label2);
      this.groupBox1.Controls.Add((Control) this.textBoxID1);
      this.groupBox1.Controls.Add((Control) this.label1);
      this.groupBox1.Location = new Point(9, 7);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(280, 296);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(299, 342);
      this.Controls.Add((Control) this.moreBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.groupBox1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (TriggerAddFieldDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Fields";
      this.KeyDown += new KeyEventHandler(this.TriggerAddFieldDialog_KeyDown);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
