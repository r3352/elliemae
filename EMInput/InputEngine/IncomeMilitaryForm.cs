// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.IncomeMilitaryForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class IncomeMilitaryForm : Form, IOnlineHelpTarget, IHelp
  {
    private const string className = "IncomeMilitaryForm";
    private Sessions.Session session;
    private IHtmlInput inputData;
    private LoanData loan;
    private GenericFormInputHandler incomeFormInputHandler;
    private IContainer components;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private Button btnCancel;
    private Button btnOK;
    private GroupContainer groupContainer1;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private TextBox txtOtherCobTotal;
    private TextBox txtOtherBorTotal;
    private TextBox txtOtherCob6;
    private TextBox txtOtherBor6;
    private TextBox txtOtherCob5;
    private TextBox txtOtherBor5;
    private TextBox txtOtherCob4;
    private TextBox txtOtherBor4;
    private TextBox txtOtherCob3;
    private TextBox txtOtherBor3;
    private TextBox txtOtherCob2;
    private TextBox txtOtherBor2;
    private TextBox txtOtherCob1;
    private Label label3;
    private Label label2;
    private TextBox txtOtherBor1;
    private Label label1;
    private ToolTip toolTipField;
    private EMHelpLink emHelpLink1;

    public IncomeMilitaryForm(IHtmlInput inputData, Sessions.Session session)
    {
      this.session = session;
      this.inputData = inputData;
      if (this.inputData is LoanData)
        this.loan = (LoanData) this.inputData;
      this.InitializeComponent();
      this.incomeFormInputHandler = new GenericFormInputHandler(this.inputData, this.Controls, this.session);
      this.initForm();
    }

    private void initForm()
    {
      this.incomeFormInputHandler.SetFieldValuesToForm();
      this.incomeFormInputHandler.SetBusinessRules(new ResourceManager(typeof (IncomeOtherForm)));
      this.incomeFormInputHandler.SetFieldTip(this.toolTipField);
      for (int index = 0; index < this.incomeFormInputHandler.FieldControls.Count; ++index)
        this.incomeFormInputHandler.SetFieldEvents(this.incomeFormInputHandler.FieldControls[index]);
      this.incomeFormInputHandler.OnKeyUp += new EventHandler(this.calculateFields);
    }

    private void calculateFields(object sender, EventArgs e)
    {
      Decimal num1 = 0M;
      Decimal num2 = 0M;
      for (int index = 0; index < this.incomeFormInputHandler.FieldControls.Count; ++index)
      {
        TextBox fieldControl = (TextBox) this.incomeFormInputHandler.FieldControls[index];
        if (!(fieldControl.Text.Trim() == string.Empty))
        {
          if (fieldControl.Name.StartsWith("txtOtherBor") && fieldControl.Name != "txtOtherBorTotal")
            num1 += Utils.ParseDecimal((object) fieldControl.Text.Trim());
          else if (fieldControl.Name.StartsWith("txtOtherCob") && fieldControl.Name != "txtOtherCobTotal")
            num2 += Utils.ParseDecimal((object) fieldControl.Text.Trim());
        }
      }
      this.txtOtherBorTotal.Text = num1 == 0M ? "" : num1.ToString("N2");
      this.txtOtherCobTotal.Text = num2 == 0M ? "" : num2.ToString("N2");
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.incomeFormInputHandler.SetFieldValuesToLoan(false);
      this.inputData.SetField("QM.X163", this.txtOtherBorTotal.Text.Trim());
      this.inputData.SetField("QM.X168", this.txtOtherCobTotal.Text.Trim());
      this.DialogResult = DialogResult.OK;
    }

    private void dialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    public string GetHelpTargetName() => nameof (IncomeMilitaryForm);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (IncomeMilitaryForm));
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.groupContainer1 = new GroupContainer();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.txtOtherCobTotal = new TextBox();
      this.txtOtherBorTotal = new TextBox();
      this.txtOtherCob6 = new TextBox();
      this.txtOtherBor6 = new TextBox();
      this.txtOtherCob5 = new TextBox();
      this.txtOtherBor5 = new TextBox();
      this.txtOtherCob4 = new TextBox();
      this.txtOtherBor4 = new TextBox();
      this.txtOtherCob3 = new TextBox();
      this.txtOtherBor3 = new TextBox();
      this.txtOtherCob2 = new TextBox();
      this.txtOtherBor2 = new TextBox();
      this.txtOtherCob1 = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.txtOtherBor1 = new TextBox();
      this.label1 = new Label();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.toolTipField = new ToolTip(this.components);
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer1.SuspendLayout();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.SuspendLayout();
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(317, 248);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 18;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Location = new Point(236, 248);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 17;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.txtOtherCobTotal);
      this.groupContainer1.Controls.Add((Control) this.txtOtherBorTotal);
      this.groupContainer1.Controls.Add((Control) this.txtOtherCob6);
      this.groupContainer1.Controls.Add((Control) this.txtOtherBor6);
      this.groupContainer1.Controls.Add((Control) this.txtOtherCob5);
      this.groupContainer1.Controls.Add((Control) this.txtOtherBor5);
      this.groupContainer1.Controls.Add((Control) this.txtOtherCob4);
      this.groupContainer1.Controls.Add((Control) this.txtOtherBor4);
      this.groupContainer1.Controls.Add((Control) this.txtOtherCob3);
      this.groupContainer1.Controls.Add((Control) this.txtOtherBor3);
      this.groupContainer1.Controls.Add((Control) this.txtOtherCob2);
      this.groupContainer1.Controls.Add((Control) this.txtOtherBor2);
      this.groupContainer1.Controls.Add((Control) this.txtOtherCob1);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.txtOtherBor1);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(12, 12);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(380, 230);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Military Income";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 195);
      this.label9.Name = "label9";
      this.label9.Size = new Size(104, 13);
      this.label9.TabIndex = 39;
      this.label9.Text = "Total Military Income";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 172);
      this.label8.Name = "label8";
      this.label8.Size = new Size(85, 13);
      this.label8.TabIndex = 38;
      this.label8.Text = "Military Prop Pay";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 149);
      this.label7.Name = "label7";
      this.label7.Size = new Size(108, 13);
      this.label7.TabIndex = 37;
      this.label7.Text = "Military Overseas Pay";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 126);
      this.label6.Name = "label6";
      this.label6.Size = new Size(97, 13);
      this.label6.TabIndex = 36;
      this.label6.Text = "Military Hazard Pay";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, 103);
      this.label5.Name = "label5";
      this.label5.Size = new Size(88, 13);
      this.label5.TabIndex = 35;
      this.label5.Text = "Military Flight Pay";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 80);
      this.label4.Name = "label4";
      this.label4.Size = new Size(99, 13);
      this.label4.TabIndex = 34;
      this.label4.Text = "Military Combat Pay";
      this.txtOtherCobTotal.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherCobTotal.Location = new Point(266, 192);
      this.txtOtherCobTotal.MaxLength = 10;
      this.txtOtherCobTotal.Name = "txtOtherCobTotal";
      this.txtOtherCobTotal.ReadOnly = true;
      this.txtOtherCobTotal.Size = new Size(100, 20);
      this.txtOtherCobTotal.TabIndex = 16;
      this.txtOtherCobTotal.TabStop = false;
      this.txtOtherCobTotal.Tag = (object) "QM.X294";
      this.txtOtherCobTotal.TextAlign = HorizontalAlignment.Right;
      this.txtOtherBorTotal.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherBorTotal.Location = new Point(160, 192);
      this.txtOtherBorTotal.MaxLength = 10;
      this.txtOtherBorTotal.Name = "txtOtherBorTotal";
      this.txtOtherBorTotal.ReadOnly = true;
      this.txtOtherBorTotal.Size = new Size(100, 20);
      this.txtOtherBorTotal.TabIndex = 7;
      this.txtOtherBorTotal.TabStop = false;
      this.txtOtherBorTotal.Tag = (object) "QM.X287";
      this.txtOtherBorTotal.TextAlign = HorizontalAlignment.Right;
      this.txtOtherCob6.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherCob6.Location = new Point(266, 169);
      this.txtOtherCob6.MaxLength = 10;
      this.txtOtherCob6.Name = "txtOtherCob6";
      this.txtOtherCob6.Size = new Size(100, 20);
      this.txtOtherCob6.TabIndex = 15;
      this.txtOtherCob6.Tag = (object) "QM.X293";
      this.txtOtherCob6.TextAlign = HorizontalAlignment.Right;
      this.txtOtherBor6.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherBor6.Location = new Point(160, 169);
      this.txtOtherBor6.MaxLength = 10;
      this.txtOtherBor6.Name = "txtOtherBor6";
      this.txtOtherBor6.Size = new Size(100, 20);
      this.txtOtherBor6.TabIndex = 6;
      this.txtOtherBor6.Tag = (object) "QM.X286";
      this.txtOtherBor6.TextAlign = HorizontalAlignment.Right;
      this.txtOtherCob5.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherCob5.Location = new Point(266, 146);
      this.txtOtherCob5.MaxLength = 10;
      this.txtOtherCob5.Name = "txtOtherCob5";
      this.txtOtherCob5.Size = new Size(100, 20);
      this.txtOtherCob5.TabIndex = 14;
      this.txtOtherCob5.Tag = (object) "QM.X292";
      this.txtOtherCob5.TextAlign = HorizontalAlignment.Right;
      this.txtOtherBor5.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherBor5.Location = new Point(160, 146);
      this.txtOtherBor5.MaxLength = 10;
      this.txtOtherBor5.Name = "txtOtherBor5";
      this.txtOtherBor5.Size = new Size(100, 20);
      this.txtOtherBor5.TabIndex = 5;
      this.txtOtherBor5.Tag = (object) "QM.X285";
      this.txtOtherBor5.TextAlign = HorizontalAlignment.Right;
      this.txtOtherCob4.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherCob4.Location = new Point(266, 123);
      this.txtOtherCob4.MaxLength = 10;
      this.txtOtherCob4.Name = "txtOtherCob4";
      this.txtOtherCob4.Size = new Size(100, 20);
      this.txtOtherCob4.TabIndex = 13;
      this.txtOtherCob4.Tag = (object) "QM.X291";
      this.txtOtherCob4.TextAlign = HorizontalAlignment.Right;
      this.txtOtherBor4.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherBor4.Location = new Point(160, 123);
      this.txtOtherBor4.MaxLength = 10;
      this.txtOtherBor4.Name = "txtOtherBor4";
      this.txtOtherBor4.Size = new Size(100, 20);
      this.txtOtherBor4.TabIndex = 4;
      this.txtOtherBor4.Tag = (object) "QM.X284";
      this.txtOtherBor4.TextAlign = HorizontalAlignment.Right;
      this.txtOtherCob3.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherCob3.Location = new Point(266, 100);
      this.txtOtherCob3.MaxLength = 10;
      this.txtOtherCob3.Name = "txtOtherCob3";
      this.txtOtherCob3.Size = new Size(100, 20);
      this.txtOtherCob3.TabIndex = 12;
      this.txtOtherCob3.Tag = (object) "QM.X290";
      this.txtOtherCob3.TextAlign = HorizontalAlignment.Right;
      this.txtOtherBor3.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherBor3.Location = new Point(160, 100);
      this.txtOtherBor3.MaxLength = 10;
      this.txtOtherBor3.Name = "txtOtherBor3";
      this.txtOtherBor3.Size = new Size(100, 20);
      this.txtOtherBor3.TabIndex = 3;
      this.txtOtherBor3.Tag = (object) "QM.X283";
      this.txtOtherBor3.TextAlign = HorizontalAlignment.Right;
      this.txtOtherCob2.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherCob2.Location = new Point(266, 77);
      this.txtOtherCob2.MaxLength = 10;
      this.txtOtherCob2.Name = "txtOtherCob2";
      this.txtOtherCob2.Size = new Size(100, 20);
      this.txtOtherCob2.TabIndex = 11;
      this.txtOtherCob2.Tag = (object) "QM.X289";
      this.txtOtherCob2.TextAlign = HorizontalAlignment.Right;
      this.txtOtherBor2.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherBor2.Location = new Point(160, 77);
      this.txtOtherBor2.MaxLength = 10;
      this.txtOtherBor2.Name = "txtOtherBor2";
      this.txtOtherBor2.Size = new Size(100, 20);
      this.txtOtherBor2.TabIndex = 2;
      this.txtOtherBor2.Tag = (object) "QM.X282";
      this.txtOtherBor2.TextAlign = HorizontalAlignment.Right;
      this.txtOtherCob1.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherCob1.Location = new Point(266, 54);
      this.txtOtherCob1.MaxLength = 10;
      this.txtOtherCob1.Name = "txtOtherCob1";
      this.txtOtherCob1.Size = new Size(100, 20);
      this.txtOtherCob1.TabIndex = 10;
      this.txtOtherCob1.Tag = (object) "QM.X288";
      this.txtOtherCob1.TextAlign = HorizontalAlignment.Right;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(263, 38);
      this.label3.Name = "label3";
      this.label3.Size = new Size(65, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Co-Borrower";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(157, 38);
      this.label2.Name = "label2";
      this.label2.Size = new Size(49, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Borrower";
      this.txtOtherBor1.BorderStyle = BorderStyle.FixedSingle;
      this.txtOtherBor1.Location = new Point(160, 54);
      this.txtOtherBor1.MaxLength = 10;
      this.txtOtherBor1.Name = "txtOtherBor1";
      this.txtOtherBor1.Size = new Size(100, 20);
      this.txtOtherBor1.TabIndex = 1;
      this.txtOtherBor1.Tag = (object) "QM.X281";
      this.txtOtherBor1.TextAlign = HorizontalAlignment.Right;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 57);
      this.label1.Name = "label1";
      this.label1.Size = new Size(90, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Military Base Pay ";
      this.pboxDownArrow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(194, (int) byte.MaxValue);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 76;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(159, 257);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 75;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = nameof (IncomeMilitaryForm);
      this.emHelpLink1.Location = new Point(21, 257);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 77;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(406, 281);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (IncomeMilitaryForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Calculate Military Income";
      this.KeyPress += new KeyPressEventHandler(this.dialog_KeyPress);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.ResumeLayout(false);
    }
  }
}
