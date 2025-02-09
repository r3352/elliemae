// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecondaryMarketing.AddEPPSLoanPrograms
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.SecondaryMarketing
{
  public class AddEPPSLoanPrograms : Form
  {
    private EPPSLoanProgram[] selectedProgramIDs;
    private IContainer components;
    private GroupContainer groupContainer1;
    private TextBox textBox1;
    private TextBox textBoxID1;
    private TextBox textBoxID2;
    private TextBox textBoxID3;
    private TextBox textBoxID7;
    private TextBox textBoxID4;
    private TextBox textBoxID6;
    private TextBox textBoxID5;
    private TextBox textBox7;
    private TextBox textBox6;
    private TextBox textBox5;
    private TextBox textBox4;
    private TextBox textBox3;
    private TextBox textBox2;
    private Button okBtn;
    private Button cancelBtn;
    private Button moreBtn;

    public event EventHandler OnAddMoreButtonClick;

    public AddEPPSLoanPrograms()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.textBoxID1, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.textBoxID2, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.textBoxID3, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.textBoxID4, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.textBoxID5, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.textBoxID6, TextBoxContentRule.NonNegativeInteger);
      TextBoxFormatter.Attach(this.textBoxID7, TextBoxContentRule.NonNegativeInteger);
    }

    public EPPSLoanProgram[] SelectedProgramIDs => this.selectedProgramIDs;

    private void okBtn_Click(object sender, EventArgs e)
    {
      List<EPPSLoanProgram> eppsLoanProgramList = this.collectEntries();
      if (eppsLoanProgramList == null)
        return;
      this.selectedProgramIDs = new EPPSLoanProgram[eppsLoanProgramList.Count];
      eppsLoanProgramList.CopyTo(this.selectedProgramIDs);
      this.DialogResult = DialogResult.OK;
    }

    private List<EPPSLoanProgram> collectEntries()
    {
      EPPSLoanProgram[] eppsLoanProgramArray = new EPPSLoanProgram[7]
      {
        new EPPSLoanProgram(this.textBoxID1.Text.Trim(), this.textBox1.Text.Trim()),
        new EPPSLoanProgram(this.textBoxID2.Text.Trim(), this.textBox2.Text.Trim()),
        new EPPSLoanProgram(this.textBoxID3.Text.Trim(), this.textBox3.Text.Trim()),
        new EPPSLoanProgram(this.textBoxID4.Text.Trim(), this.textBox4.Text.Trim()),
        new EPPSLoanProgram(this.textBoxID5.Text.Trim(), this.textBox5.Text.Trim()),
        new EPPSLoanProgram(this.textBoxID6.Text.Trim(), this.textBox6.Text.Trim()),
        new EPPSLoanProgram(this.textBoxID7.Text.Trim(), this.textBox7.Text.Trim())
      };
      List<EPPSLoanProgram> eppsLoanProgramList1 = new List<EPPSLoanProgram>();
      ArrayList arrayList = new ArrayList();
      for (int index = 0; index < 7; ++index)
      {
        if (!(eppsLoanProgramArray[index].ProgramID == string.Empty) || !(eppsLoanProgramArray[index].ProgramName == string.Empty))
        {
          string text = this.validateProgram(eppsLoanProgramArray[index]);
          if (!string.IsNullOrEmpty(text))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            switch (index)
            {
              case 0:
                this.textBoxID1.Focus();
                break;
              case 1:
                this.textBoxID2.Focus();
                break;
              case 2:
                this.textBoxID3.Focus();
                break;
              case 3:
                this.textBoxID4.Focus();
                break;
              case 4:
                this.textBoxID5.Focus();
                break;
              case 5:
                this.textBoxID6.Focus();
                break;
              case 6:
                this.textBoxID7.Focus();
                break;
            }
            List<EPPSLoanProgram> eppsLoanProgramList2 = new List<EPPSLoanProgram>();
            return (List<EPPSLoanProgram>) null;
          }
          if (!arrayList.Contains((object) eppsLoanProgramArray[index].ProgramID))
          {
            arrayList.Add((object) eppsLoanProgramArray[index].ProgramID);
            eppsLoanProgramList1.Add(eppsLoanProgramArray[index]);
          }
        }
      }
      if (eppsLoanProgramList1.Count != 0)
        return eppsLoanProgramList1;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a new Entry.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return (List<EPPSLoanProgram>) null;
    }

    private void textBoxID_TextChanged(object sender, EventArgs e)
    {
      if (!(((Control) sender).Text.Trim() != "") || this.okBtn.Enabled)
        return;
      this.okBtn.Enabled = true;
      this.moreBtn.Enabled = true;
    }

    private string validateProgram(EPPSLoanProgram program)
    {
      return string.IsNullOrEmpty(program.ProgramID) || string.IsNullOrEmpty(program.ProgramName) ? "Both Program ID and Program Name are required fields. Please correct and try again." : "";
    }

    private void moreBtn_Click(object sender, EventArgs e)
    {
      List<EPPSLoanProgram> eppsLoanProgramList = this.collectEntries();
      this.textBoxID1.Text = this.textBox1.Text = this.textBoxID2.Text = this.textBox2.Text = this.textBoxID3.Text = this.textBox3.Text = this.textBoxID4.Text = this.textBox4.Text = "";
      this.textBoxID5.Text = this.textBox5.Text = this.textBoxID6.Text = this.textBox6.Text = this.textBoxID7.Text = this.textBox7.Text = "";
      if (eppsLoanProgramList == null)
        return;
      this.selectedProgramIDs = new EPPSLoanProgram[eppsLoanProgramList.Count];
      eppsLoanProgramList.CopyTo(this.selectedProgramIDs);
      if (this.OnAddMoreButtonClick == null)
        return;
      this.OnAddMoreButtonClick((object) this, e);
      this.okBtn.Enabled = false;
      this.moreBtn.Enabled = false;
    }

    private void cancelBtn_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.textBox7 = new TextBox();
      this.textBox6 = new TextBox();
      this.textBox5 = new TextBox();
      this.textBox4 = new TextBox();
      this.textBox3 = new TextBox();
      this.textBox2 = new TextBox();
      this.textBox1 = new TextBox();
      this.textBoxID1 = new TextBox();
      this.textBoxID2 = new TextBox();
      this.textBoxID3 = new TextBox();
      this.textBoxID7 = new TextBox();
      this.textBoxID4 = new TextBox();
      this.textBoxID6 = new TextBox();
      this.textBoxID5 = new TextBox();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.moreBtn = new Button();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.textBox7);
      this.groupContainer1.Controls.Add((Control) this.textBox6);
      this.groupContainer1.Controls.Add((Control) this.textBox5);
      this.groupContainer1.Controls.Add((Control) this.textBox4);
      this.groupContainer1.Controls.Add((Control) this.textBox3);
      this.groupContainer1.Controls.Add((Control) this.textBox2);
      this.groupContainer1.Controls.Add((Control) this.textBox1);
      this.groupContainer1.Controls.Add((Control) this.textBoxID1);
      this.groupContainer1.Controls.Add((Control) this.textBoxID2);
      this.groupContainer1.Controls.Add((Control) this.textBoxID3);
      this.groupContainer1.Controls.Add((Control) this.textBoxID7);
      this.groupContainer1.Controls.Add((Control) this.textBoxID4);
      this.groupContainer1.Controls.Add((Control) this.textBoxID6);
      this.groupContainer1.Controls.Add((Control) this.textBoxID5);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(27, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(435, 214);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Text = "Program ID                                       Program Name";
      this.textBox7.Location = new Point(103, 181);
      this.textBox7.MaxLength = 50;
      this.textBox7.Name = "textBox7";
      this.textBox7.Size = new Size(315, 20);
      this.textBox7.TabIndex = 18;
      this.textBox7.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBox6.Location = new Point(103, 155);
      this.textBox6.MaxLength = 50;
      this.textBox6.Name = "textBox6";
      this.textBox6.Size = new Size(315, 20);
      this.textBox6.TabIndex = 17;
      this.textBox6.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBox5.Location = new Point(103, 132);
      this.textBox5.MaxLength = 50;
      this.textBox5.Name = "textBox5";
      this.textBox5.Size = new Size(315, 20);
      this.textBox5.TabIndex = 16;
      this.textBox5.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBox4.Location = new Point(103, 108);
      this.textBox4.MaxLength = 50;
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new Size(315, 20);
      this.textBox4.TabIndex = 15;
      this.textBox4.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBox3.Location = new Point(103, 85);
      this.textBox3.MaxLength = 50;
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new Size(315, 20);
      this.textBox3.TabIndex = 14;
      this.textBox3.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBox2.Location = new Point(103, 60);
      this.textBox2.MaxLength = 50;
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(315, 20);
      this.textBox2.TabIndex = 13;
      this.textBox2.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBox1.Location = new Point(103, 36);
      this.textBox1.MaxLength = 50;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(315, 20);
      this.textBox1.TabIndex = 12;
      this.textBox1.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBoxID1.Location = new Point(14, 36);
      this.textBoxID1.MaxLength = 10;
      this.textBoxID1.Name = "textBoxID1";
      this.textBoxID1.Size = new Size(83, 20);
      this.textBoxID1.TabIndex = 1;
      this.textBoxID1.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBoxID2.Location = new Point(14, 60);
      this.textBoxID2.MaxLength = 10;
      this.textBoxID2.Name = "textBoxID2";
      this.textBoxID2.Size = new Size(83, 20);
      this.textBoxID2.TabIndex = 2;
      this.textBoxID2.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBoxID3.Location = new Point(14, 84);
      this.textBoxID3.MaxLength = 10;
      this.textBoxID3.Name = "textBoxID3";
      this.textBoxID3.Size = new Size(83, 20);
      this.textBoxID3.TabIndex = 3;
      this.textBoxID3.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBoxID7.Location = new Point(14, 180);
      this.textBoxID7.MaxLength = 10;
      this.textBoxID7.Name = "textBoxID7";
      this.textBoxID7.Size = new Size(83, 20);
      this.textBoxID7.TabIndex = 11;
      this.textBoxID7.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBoxID4.Location = new Point(14, 108);
      this.textBoxID4.MaxLength = 10;
      this.textBoxID4.Name = "textBoxID4";
      this.textBoxID4.Size = new Size(83, 20);
      this.textBoxID4.TabIndex = 4;
      this.textBoxID4.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBoxID6.Location = new Point(14, 156);
      this.textBoxID6.MaxLength = 10;
      this.textBoxID6.Name = "textBoxID6";
      this.textBoxID6.Size = new Size(83, 20);
      this.textBoxID6.TabIndex = 10;
      this.textBoxID6.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.textBoxID5.Location = new Point(14, 132);
      this.textBoxID5.MaxLength = 10;
      this.textBoxID5.Name = "textBoxID5";
      this.textBoxID5.Size = new Size(83, 20);
      this.textBoxID5.TabIndex = 5;
      this.textBoxID5.TextChanged += new EventHandler(this.textBoxID_TextChanged);
      this.okBtn.Enabled = false;
      this.okBtn.Location = new Point(130, 242);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(64, 22);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&Add";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(273, 242);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(64, 22);
      this.cancelBtn.TabIndex = 7;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.moreBtn.Enabled = false;
      this.moreBtn.Location = new Point(201, 242);
      this.moreBtn.Name = "moreBtn";
      this.moreBtn.Size = new Size(65, 22);
      this.moreBtn.TabIndex = 6;
      this.moreBtn.Text = "Add &More";
      this.moreBtn.Click += new EventHandler(this.moreBtn_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(523, 287);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.moreBtn);
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (AddEPPSLoanPrograms);
      this.Text = nameof (AddEPPSLoanPrograms);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
