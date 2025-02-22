﻿// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.CompanyInfoControl
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
  public class CompanyInfoControl : UserControl
  {
    private IContainer components;
    private GroupContainer gContainer;
    private Panel panelDBA;
    private Panel panelAddMore;
    private Button btnAddDBA;
    private Panel panelDBA2;
    private TextBox txtDBAName2;
    private Label label2;
    private Panel panelDBA3;
    private TextBox txtDBAName3;
    private Label label3;
    private Panel panelDBA4;
    private TextBox txtDBAName4;
    private Label label4;
    private Panel panelDBA1;
    private TextBox txtDBAName1;
    private Label label1;
    private TextBox addTxt;
    private Label nameLbl;
    private TextBox zipTxt;
    private TextBox nameTxt;
    private Label zipLbl;
    private Label addLbl;
    private Label phoneLbl;
    private TextBox stTxt;
    private Label cityLbl;
    private TextBox phoneTxt;
    private TextBox faxTxt;
    private Label stLbl;
    private TextBox cityTxt;
    private Label faxLbl;

    public CompanyInfoControl() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.gContainer = new GroupContainer();
      this.panelDBA = new Panel();
      this.panelAddMore = new Panel();
      this.btnAddDBA = new Button();
      this.panelDBA2 = new Panel();
      this.txtDBAName2 = new TextBox();
      this.label2 = new Label();
      this.panelDBA3 = new Panel();
      this.txtDBAName3 = new TextBox();
      this.label3 = new Label();
      this.panelDBA4 = new Panel();
      this.txtDBAName4 = new TextBox();
      this.label4 = new Label();
      this.panelDBA1 = new Panel();
      this.txtDBAName1 = new TextBox();
      this.label1 = new Label();
      this.addTxt = new TextBox();
      this.nameLbl = new Label();
      this.zipTxt = new TextBox();
      this.nameTxt = new TextBox();
      this.zipLbl = new Label();
      this.addLbl = new Label();
      this.phoneLbl = new Label();
      this.stTxt = new TextBox();
      this.cityLbl = new Label();
      this.phoneTxt = new TextBox();
      this.faxTxt = new TextBox();
      this.stLbl = new Label();
      this.cityTxt = new TextBox();
      this.faxLbl = new Label();
      this.gContainer.SuspendLayout();
      this.panelDBA.SuspendLayout();
      this.panelAddMore.SuspendLayout();
      this.panelDBA2.SuspendLayout();
      this.panelDBA3.SuspendLayout();
      this.panelDBA4.SuspendLayout();
      this.panelDBA1.SuspendLayout();
      this.SuspendLayout();
      this.gContainer.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gContainer.Controls.Add((Control) this.panelDBA);
      this.gContainer.Controls.Add((Control) this.addTxt);
      this.gContainer.Controls.Add((Control) this.nameLbl);
      this.gContainer.Controls.Add((Control) this.zipTxt);
      this.gContainer.Controls.Add((Control) this.nameTxt);
      this.gContainer.Controls.Add((Control) this.zipLbl);
      this.gContainer.Controls.Add((Control) this.addLbl);
      this.gContainer.Controls.Add((Control) this.phoneLbl);
      this.gContainer.Controls.Add((Control) this.stTxt);
      this.gContainer.Controls.Add((Control) this.cityLbl);
      this.gContainer.Controls.Add((Control) this.phoneTxt);
      this.gContainer.Controls.Add((Control) this.faxTxt);
      this.gContainer.Controls.Add((Control) this.stLbl);
      this.gContainer.Controls.Add((Control) this.cityTxt);
      this.gContainer.Controls.Add((Control) this.faxLbl);
      this.gContainer.Dock = DockStyle.Top;
      this.gContainer.HeaderForeColor = SystemColors.ControlText;
      this.gContainer.Location = new Point(0, 0);
      this.gContainer.Name = "gContainer";
      this.gContainer.Size = new Size(758, 216);
      this.gContainer.TabIndex = 16;
      this.gContainer.Text = "Company Information";
      this.panelDBA.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelDBA.Controls.Add((Control) this.panelAddMore);
      this.panelDBA.Controls.Add((Control) this.panelDBA2);
      this.panelDBA.Controls.Add((Control) this.panelDBA3);
      this.panelDBA.Controls.Add((Control) this.panelDBA4);
      this.panelDBA.Controls.Add((Control) this.panelDBA1);
      this.panelDBA.Location = new Point(388, 37);
      this.panelDBA.Name = "panelDBA";
      this.panelDBA.Size = new Size(367, 174);
      this.panelDBA.TabIndex = 16;
      this.panelAddMore.Controls.Add((Control) this.btnAddDBA);
      this.panelAddMore.Location = new Point(0, 112);
      this.panelAddMore.Name = "panelAddMore";
      this.panelAddMore.Size = new Size(355, 28);
      this.panelAddMore.TabIndex = 4;
      this.btnAddDBA.Location = new Point(87, 0);
      this.btnAddDBA.Name = "btnAddDBA";
      this.btnAddDBA.Size = new Size(75, 23);
      this.btnAddDBA.TabIndex = 0;
      this.btnAddDBA.Text = "Add More";
      this.btnAddDBA.UseVisualStyleBackColor = true;
      this.panelDBA2.Controls.Add((Control) this.txtDBAName2);
      this.panelDBA2.Controls.Add((Control) this.label2);
      this.panelDBA2.Location = new Point(0, 28);
      this.panelDBA2.Name = "panelDBA2";
      this.panelDBA2.Size = new Size(355, 28);
      this.panelDBA2.TabIndex = 1;
      this.panelDBA2.Visible = false;
      this.txtDBAName2.Location = new Point(87, 0);
      this.txtDBAName2.Name = "txtDBAName2";
      this.txtDBAName2.Size = new Size(264, 20);
      this.txtDBAName2.TabIndex = 3;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(3, 3);
      this.label2.Name = "label2";
      this.label2.Size = new Size(78, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "D.B.A. Name 2";
      this.panelDBA3.Controls.Add((Control) this.txtDBAName3);
      this.panelDBA3.Controls.Add((Control) this.label3);
      this.panelDBA3.Location = new Point(0, 56);
      this.panelDBA3.Name = "panelDBA3";
      this.panelDBA3.Size = new Size(355, 28);
      this.panelDBA3.TabIndex = 2;
      this.panelDBA3.Visible = false;
      this.txtDBAName3.Location = new Point(87, 0);
      this.txtDBAName3.Name = "txtDBAName3";
      this.txtDBAName3.Size = new Size(264, 20);
      this.txtDBAName3.TabIndex = 3;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3, 3);
      this.label3.Name = "label3";
      this.label3.Size = new Size(78, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "D.B.A. Name 3";
      this.panelDBA4.Controls.Add((Control) this.txtDBAName4);
      this.panelDBA4.Controls.Add((Control) this.label4);
      this.panelDBA4.Location = new Point(0, 84);
      this.panelDBA4.Name = "panelDBA4";
      this.panelDBA4.Size = new Size(355, 28);
      this.panelDBA4.TabIndex = 3;
      this.panelDBA4.Visible = false;
      this.txtDBAName4.Location = new Point(87, 0);
      this.txtDBAName4.Name = "txtDBAName4";
      this.txtDBAName4.Size = new Size(264, 20);
      this.txtDBAName4.TabIndex = 3;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(3, 3);
      this.label4.Name = "label4";
      this.label4.Size = new Size(78, 13);
      this.label4.TabIndex = 2;
      this.label4.Text = "D.B.A. Name 4";
      this.panelDBA1.Controls.Add((Control) this.txtDBAName1);
      this.panelDBA1.Controls.Add((Control) this.label1);
      this.panelDBA1.Location = new Point(0, 0);
      this.panelDBA1.Name = "panelDBA1";
      this.panelDBA1.Size = new Size(355, 28);
      this.panelDBA1.TabIndex = 0;
      this.txtDBAName1.Location = new Point(88, 0);
      this.txtDBAName1.Name = "txtDBAName1";
      this.txtDBAName1.Size = new Size(264, 20);
      this.txtDBAName1.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(69, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "D.B.A. Name";
      this.addTxt.Location = new Point(97, 65);
      this.addTxt.MaxLength = 250;
      this.addTxt.Name = "addTxt";
      this.addTxt.Size = new Size(284, 20);
      this.addTxt.TabIndex = 4;
      this.nameLbl.Location = new Point(9, 40);
      this.nameLbl.Name = "nameLbl";
      this.nameLbl.Size = new Size(80, 23);
      this.nameLbl.TabIndex = 1;
      this.nameLbl.Text = "Name";
      this.zipTxt.Location = new Point(197, 121);
      this.zipTxt.MaxLength = 10;
      this.zipTxt.Name = "zipTxt";
      this.zipTxt.Size = new Size(84, 20);
      this.zipTxt.TabIndex = 10;
      this.nameTxt.Location = new Point(97, 37);
      this.nameTxt.MaxLength = 250;
      this.nameTxt.Name = "nameTxt";
      this.nameTxt.Size = new Size(284, 20);
      this.nameTxt.TabIndex = 2;
      this.zipLbl.Location = new Point(160, 124);
      this.zipLbl.Name = "zipLbl";
      this.zipLbl.Size = new Size(31, 23);
      this.zipLbl.TabIndex = 9;
      this.zipLbl.Text = "Zip";
      this.addLbl.Location = new Point(9, 68);
      this.addLbl.Name = "addLbl";
      this.addLbl.Size = new Size(82, 23);
      this.addLbl.TabIndex = 3;
      this.addLbl.Text = "Address";
      this.phoneLbl.Location = new Point(9, 152);
      this.phoneLbl.Name = "phoneLbl";
      this.phoneLbl.Size = new Size(82, 23);
      this.phoneLbl.TabIndex = 11;
      this.phoneLbl.Text = "Phone Number";
      this.stTxt.Location = new Point(97, 121);
      this.stTxt.MaxLength = 2;
      this.stTxt.Name = "stTxt";
      this.stTxt.Size = new Size(44, 20);
      this.stTxt.TabIndex = 8;
      this.cityLbl.Location = new Point(9, 96);
      this.cityLbl.Name = "cityLbl";
      this.cityLbl.Size = new Size(82, 23);
      this.cityLbl.TabIndex = 5;
      this.cityLbl.Text = "City";
      this.phoneTxt.Location = new Point(97, 147);
      this.phoneTxt.MaxLength = 250;
      this.phoneTxt.Name = "phoneTxt";
      this.phoneTxt.Size = new Size(224, 20);
      this.phoneTxt.TabIndex = 12;
      this.faxTxt.Location = new Point(97, 177);
      this.faxTxt.MaxLength = 250;
      this.faxTxt.Name = "faxTxt";
      this.faxTxt.Size = new Size(224, 20);
      this.faxTxt.TabIndex = 14;
      this.stLbl.Location = new Point(9, 124);
      this.stLbl.Name = "stLbl";
      this.stLbl.Size = new Size(82, 23);
      this.stLbl.TabIndex = 7;
      this.stLbl.Text = "State";
      this.cityTxt.Location = new Point(97, 93);
      this.cityTxt.MaxLength = 250;
      this.cityTxt.Name = "cityTxt";
      this.cityTxt.Size = new Size(184, 20);
      this.cityTxt.TabIndex = 6;
      this.faxLbl.Location = new Point(9, 180);
      this.faxLbl.Name = "faxLbl";
      this.faxLbl.Size = new Size(82, 23);
      this.faxLbl.TabIndex = 13;
      this.faxLbl.Text = "Fax Number";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gContainer);
      this.Name = nameof (CompanyInfoControl);
      this.Size = new Size(758, 233);
      this.gContainer.ResumeLayout(false);
      this.gContainer.PerformLayout();
      this.panelDBA.ResumeLayout(false);
      this.panelAddMore.ResumeLayout(false);
      this.panelDBA2.ResumeLayout(false);
      this.panelDBA2.PerformLayout();
      this.panelDBA3.ResumeLayout(false);
      this.panelDBA3.PerformLayout();
      this.panelDBA4.ResumeLayout(false);
      this.panelDBA4.PerformLayout();
      this.panelDBA1.ResumeLayout(false);
      this.panelDBA1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
