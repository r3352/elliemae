// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CustomFieldsForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CustomFieldsForm : UserControl, IRefreshContents
  {
    private IContainer components;
    private ComboBox cboBox1;
    private ComboBox cboBox2;
    private ComboBox cboBox4;
    private ComboBox cboBox3;
    private ComboBox cboBox8;
    private ComboBox cboBox7;
    private ComboBox cboBox6;
    private ComboBox cboBox5;
    private ComboBox cboBox16;
    private ComboBox cboBox15;
    private ComboBox cboBox14;
    private ComboBox cboBox13;
    private ComboBox cboBox12;
    private ComboBox cboBox11;
    private ComboBox cboBox10;
    private ComboBox cboBox9;
    private ComboBox cboBox21;
    private ComboBox cboBox20;
    private ComboBox cboBox19;
    private ComboBox cboBox18;
    private ComboBox cboBox17;
    private ComboBox cboBox25;
    private ComboBox cboBox24;
    private ComboBox cboBox23;
    private ComboBox cboBox22;
    private LoanData loan;
    private int page;
    private System.Windows.Forms.TextBox[] labelFields = new System.Windows.Forms.TextBox[25];
    private System.Windows.Forms.Label[] labelNumber = new System.Windows.Forms.Label[25];
    private ComboBox[] comboFields = new ComboBox[25];
    private ToolTip fieldTips;
    private CustomFieldsInfo customFields;
    private System.Windows.Forms.Label label26;
    private System.Windows.Forms.Label label27;
    private System.Windows.Forms.Label label28;
    private System.Windows.Forms.Label label29;
    private System.Windows.Forms.Label label30;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.Label label33;
    private System.Windows.Forms.Label label34;
    private System.Windows.Forms.Label label35;
    private System.Windows.Forms.Label label36;
    private System.Windows.Forms.Label label37;
    private System.Windows.Forms.Label label38;
    private System.Windows.Forms.Label label39;
    private System.Windows.Forms.Label label40;
    private System.Windows.Forms.Label label41;
    private System.Windows.Forms.Label label42;
    private System.Windows.Forms.Label label43;
    private System.Windows.Forms.Label label44;
    private System.Windows.Forms.Label label45;
    private System.Windows.Forms.Label label46;
    private System.Windows.Forms.Label label47;
    private System.Windows.Forms.Label label48;
    private System.Windows.Forms.Label label49;
    private System.Windows.Forms.Label label50;
    private string loanID = string.Empty;
    private PictureBox pboxAsterisk;
    private PictureBox pboxDownArrow;
    private PopupBusinessRules popupRules;
    private bool isFirstLoading = true;
    private BorderPanel borderPanel1;
    private System.Windows.Forms.TextBox label1;
    private System.Windows.Forms.TextBox label3;
    private System.Windows.Forms.TextBox label2;
    private System.Windows.Forms.TextBox label4;
    private System.Windows.Forms.TextBox label6;
    private System.Windows.Forms.TextBox label5;
    private System.Windows.Forms.TextBox label7;
    private System.Windows.Forms.TextBox label8;
    private System.Windows.Forms.TextBox label9;
    private System.Windows.Forms.TextBox label25;
    private System.Windows.Forms.TextBox label19;
    private System.Windows.Forms.TextBox label24;
    private System.Windows.Forms.TextBox label23;
    private System.Windows.Forms.TextBox label22;
    private System.Windows.Forms.TextBox label21;
    private System.Windows.Forms.TextBox label20;
    private System.Windows.Forms.TextBox label18;
    private System.Windows.Forms.TextBox label12;
    private System.Windows.Forms.TextBox label17;
    private System.Windows.Forms.TextBox label10;
    private System.Windows.Forms.TextBox label16;
    private System.Windows.Forms.TextBox label11;
    private System.Windows.Forms.TextBox label15;
    private System.Windows.Forms.TextBox label14;
    private System.Windows.Forms.TextBox label13;
    private PiggybackSynchronization piggySyncTool;
    private StatusReport statusReport;

    public CustomFieldsForm(int page, LoanData loan)
    {
      this.page = page;
      this.loan = loan;
      if (!this.loan.IsInFindFieldForm && !this.loan.IsTemplate && Session.LoanDataMgr.LinkedLoan != null)
        this.loan.LinkedData = Session.LoanDataMgr.LinkedLoan.LoanData;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.InitializeControls();
      this.customFields = Session.ConfigurationManager.GetLoanCustomFields();
      if (!this.loan.IsInFindFieldForm)
      {
        ResourceManager resources = new ResourceManager(typeof (CustomFieldsForm));
        this.popupRules = new PopupBusinessRules(this.loan, resources, (System.Drawing.Image) resources.GetObject("pboxAsterisk.Image"), (System.Drawing.Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
        this.popupRules.DropdownSelected += new EventHandler(this.popupRules_DropdownSelected);
      }
      this.setDescription();
      this.setFieldValue();
      if (this.cboBox1.TabStop)
        this.cboBox1.Focus();
      this.piggySyncTool = new PiggybackSynchronization(this.loan);
      this.isFirstLoading = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CustomFieldsForm));
      this.cboBox1 = new ComboBox();
      this.cboBox2 = new ComboBox();
      this.cboBox4 = new ComboBox();
      this.cboBox3 = new ComboBox();
      this.cboBox8 = new ComboBox();
      this.cboBox7 = new ComboBox();
      this.cboBox6 = new ComboBox();
      this.cboBox5 = new ComboBox();
      this.cboBox16 = new ComboBox();
      this.cboBox15 = new ComboBox();
      this.cboBox14 = new ComboBox();
      this.cboBox13 = new ComboBox();
      this.cboBox12 = new ComboBox();
      this.cboBox11 = new ComboBox();
      this.cboBox10 = new ComboBox();
      this.cboBox9 = new ComboBox();
      this.cboBox21 = new ComboBox();
      this.cboBox20 = new ComboBox();
      this.cboBox19 = new ComboBox();
      this.cboBox18 = new ComboBox();
      this.cboBox17 = new ComboBox();
      this.cboBox25 = new ComboBox();
      this.cboBox24 = new ComboBox();
      this.cboBox23 = new ComboBox();
      this.cboBox22 = new ComboBox();
      this.fieldTips = new ToolTip(this.components);
      this.pboxAsterisk = new PictureBox();
      this.pboxDownArrow = new PictureBox();
      this.label50 = new System.Windows.Forms.Label();
      this.label49 = new System.Windows.Forms.Label();
      this.label48 = new System.Windows.Forms.Label();
      this.label47 = new System.Windows.Forms.Label();
      this.label46 = new System.Windows.Forms.Label();
      this.label45 = new System.Windows.Forms.Label();
      this.label44 = new System.Windows.Forms.Label();
      this.label43 = new System.Windows.Forms.Label();
      this.label42 = new System.Windows.Forms.Label();
      this.label41 = new System.Windows.Forms.Label();
      this.label40 = new System.Windows.Forms.Label();
      this.label39 = new System.Windows.Forms.Label();
      this.label38 = new System.Windows.Forms.Label();
      this.label37 = new System.Windows.Forms.Label();
      this.label36 = new System.Windows.Forms.Label();
      this.label35 = new System.Windows.Forms.Label();
      this.label34 = new System.Windows.Forms.Label();
      this.label33 = new System.Windows.Forms.Label();
      this.label32 = new System.Windows.Forms.Label();
      this.label31 = new System.Windows.Forms.Label();
      this.label30 = new System.Windows.Forms.Label();
      this.label28 = new System.Windows.Forms.Label();
      this.label29 = new System.Windows.Forms.Label();
      this.label27 = new System.Windows.Forms.Label();
      this.label26 = new System.Windows.Forms.Label();
      this.borderPanel1 = new BorderPanel();
      this.label25 = new System.Windows.Forms.TextBox();
      this.label19 = new System.Windows.Forms.TextBox();
      this.label24 = new System.Windows.Forms.TextBox();
      this.label23 = new System.Windows.Forms.TextBox();
      this.label22 = new System.Windows.Forms.TextBox();
      this.label21 = new System.Windows.Forms.TextBox();
      this.label20 = new System.Windows.Forms.TextBox();
      this.label18 = new System.Windows.Forms.TextBox();
      this.label12 = new System.Windows.Forms.TextBox();
      this.label17 = new System.Windows.Forms.TextBox();
      this.label10 = new System.Windows.Forms.TextBox();
      this.label16 = new System.Windows.Forms.TextBox();
      this.label11 = new System.Windows.Forms.TextBox();
      this.label15 = new System.Windows.Forms.TextBox();
      this.label14 = new System.Windows.Forms.TextBox();
      this.label13 = new System.Windows.Forms.TextBox();
      this.label9 = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.TextBox();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.cboBox1.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox1.IntegralHeight = false;
      this.cboBox1.Location = new Point(301, 10);
      this.cboBox1.MaxDropDownItems = 20;
      this.cboBox1.Name = "cboBox1";
      this.cboBox1.Size = new Size(260, 22);
      this.cboBox1.TabIndex = 0;
      this.cboBox1.Tag = (object) "1";
      this.cboBox1.Leave += new EventHandler(this.leaveField);
      this.cboBox1.Enter += new EventHandler(this.enterField);
      this.cboBox1.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox1.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox1.TextChanged += new EventHandler(this.formatInput);
      this.cboBox2.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox2.Location = new Point(301, 34);
      this.cboBox2.MaxDropDownItems = 20;
      this.cboBox2.Name = "cboBox2";
      this.cboBox2.Size = new Size(260, 22);
      this.cboBox2.TabIndex = 1;
      this.cboBox2.Tag = (object) "2";
      this.cboBox2.Leave += new EventHandler(this.leaveField);
      this.cboBox2.Enter += new EventHandler(this.enterField);
      this.cboBox2.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox2.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox2.TextChanged += new EventHandler(this.formatInput);
      this.cboBox4.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox4.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox4.Location = new Point(301, 82);
      this.cboBox4.MaxDropDownItems = 20;
      this.cboBox4.Name = "cboBox4";
      this.cboBox4.Size = new Size(260, 22);
      this.cboBox4.TabIndex = 3;
      this.cboBox4.Tag = (object) "4";
      this.cboBox4.Leave += new EventHandler(this.leaveField);
      this.cboBox4.Enter += new EventHandler(this.enterField);
      this.cboBox4.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox4.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox4.TextChanged += new EventHandler(this.formatInput);
      this.cboBox3.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox3.Location = new Point(301, 58);
      this.cboBox3.MaxDropDownItems = 20;
      this.cboBox3.Name = "cboBox3";
      this.cboBox3.Size = new Size(260, 22);
      this.cboBox3.TabIndex = 2;
      this.cboBox3.Tag = (object) "3";
      this.cboBox3.Leave += new EventHandler(this.leaveField);
      this.cboBox3.Enter += new EventHandler(this.enterField);
      this.cboBox3.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox3.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox3.TextChanged += new EventHandler(this.formatInput);
      this.cboBox8.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox8.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox8.Location = new Point(301, 178);
      this.cboBox8.MaxDropDownItems = 20;
      this.cboBox8.Name = "cboBox8";
      this.cboBox8.Size = new Size(260, 22);
      this.cboBox8.TabIndex = 7;
      this.cboBox8.Tag = (object) "8";
      this.cboBox8.Leave += new EventHandler(this.leaveField);
      this.cboBox8.Enter += new EventHandler(this.enterField);
      this.cboBox8.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox8.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox8.TextChanged += new EventHandler(this.formatInput);
      this.cboBox7.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox7.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox7.Location = new Point(301, 154);
      this.cboBox7.MaxDropDownItems = 20;
      this.cboBox7.Name = "cboBox7";
      this.cboBox7.Size = new Size(260, 22);
      this.cboBox7.TabIndex = 6;
      this.cboBox7.Tag = (object) "7";
      this.cboBox7.Leave += new EventHandler(this.leaveField);
      this.cboBox7.Enter += new EventHandler(this.enterField);
      this.cboBox7.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox7.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox7.TextChanged += new EventHandler(this.formatInput);
      this.cboBox6.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox6.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox6.Location = new Point(301, 130);
      this.cboBox6.MaxDropDownItems = 20;
      this.cboBox6.Name = "cboBox6";
      this.cboBox6.Size = new Size(260, 22);
      this.cboBox6.TabIndex = 5;
      this.cboBox6.Tag = (object) "6";
      this.cboBox6.Leave += new EventHandler(this.leaveField);
      this.cboBox6.Enter += new EventHandler(this.enterField);
      this.cboBox6.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox6.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox6.TextChanged += new EventHandler(this.formatInput);
      this.cboBox5.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox5.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox5.Location = new Point(301, 106);
      this.cboBox5.MaxDropDownItems = 20;
      this.cboBox5.Name = "cboBox5";
      this.cboBox5.Size = new Size(260, 22);
      this.cboBox5.TabIndex = 4;
      this.cboBox5.Tag = (object) "5";
      this.cboBox5.Leave += new EventHandler(this.leaveField);
      this.cboBox5.Enter += new EventHandler(this.enterField);
      this.cboBox5.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox5.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox5.TextChanged += new EventHandler(this.formatInput);
      this.cboBox16.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox16.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox16.Location = new Point(301, 370);
      this.cboBox16.MaxDropDownItems = 20;
      this.cboBox16.Name = "cboBox16";
      this.cboBox16.Size = new Size(260, 22);
      this.cboBox16.TabIndex = 15;
      this.cboBox16.Tag = (object) "16";
      this.cboBox16.Leave += new EventHandler(this.leaveField);
      this.cboBox16.Enter += new EventHandler(this.enterField);
      this.cboBox16.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox16.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox16.TextChanged += new EventHandler(this.formatInput);
      this.cboBox15.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox15.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox15.Location = new Point(301, 346);
      this.cboBox15.MaxDropDownItems = 20;
      this.cboBox15.Name = "cboBox15";
      this.cboBox15.Size = new Size(260, 22);
      this.cboBox15.TabIndex = 14;
      this.cboBox15.Tag = (object) "15";
      this.cboBox15.Leave += new EventHandler(this.leaveField);
      this.cboBox15.Enter += new EventHandler(this.enterField);
      this.cboBox15.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox15.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox15.TextChanged += new EventHandler(this.formatInput);
      this.cboBox14.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox14.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox14.Location = new Point(301, 322);
      this.cboBox14.MaxDropDownItems = 20;
      this.cboBox14.Name = "cboBox14";
      this.cboBox14.Size = new Size(260, 22);
      this.cboBox14.TabIndex = 13;
      this.cboBox14.Tag = (object) "14";
      this.cboBox14.Leave += new EventHandler(this.leaveField);
      this.cboBox14.Enter += new EventHandler(this.enterField);
      this.cboBox14.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox14.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox14.TextChanged += new EventHandler(this.formatInput);
      this.cboBox13.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox13.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox13.Location = new Point(301, 298);
      this.cboBox13.MaxDropDownItems = 20;
      this.cboBox13.Name = "cboBox13";
      this.cboBox13.Size = new Size(260, 22);
      this.cboBox13.TabIndex = 12;
      this.cboBox13.Tag = (object) "13";
      this.cboBox13.Leave += new EventHandler(this.leaveField);
      this.cboBox13.Enter += new EventHandler(this.enterField);
      this.cboBox13.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox13.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox13.TextChanged += new EventHandler(this.formatInput);
      this.cboBox12.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox12.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox12.Location = new Point(301, 274);
      this.cboBox12.MaxDropDownItems = 20;
      this.cboBox12.Name = "cboBox12";
      this.cboBox12.Size = new Size(260, 22);
      this.cboBox12.TabIndex = 11;
      this.cboBox12.Tag = (object) "12";
      this.cboBox12.Leave += new EventHandler(this.leaveField);
      this.cboBox12.Enter += new EventHandler(this.enterField);
      this.cboBox12.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox12.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox12.TextChanged += new EventHandler(this.formatInput);
      this.cboBox11.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox11.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox11.Location = new Point(301, 250);
      this.cboBox11.MaxDropDownItems = 20;
      this.cboBox11.Name = "cboBox11";
      this.cboBox11.Size = new Size(260, 22);
      this.cboBox11.TabIndex = 10;
      this.cboBox11.Tag = (object) "11";
      this.cboBox11.Leave += new EventHandler(this.leaveField);
      this.cboBox11.Enter += new EventHandler(this.enterField);
      this.cboBox11.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox11.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox11.TextChanged += new EventHandler(this.formatInput);
      this.cboBox10.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox10.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox10.Location = new Point(301, 226);
      this.cboBox10.MaxDropDownItems = 20;
      this.cboBox10.Name = "cboBox10";
      this.cboBox10.Size = new Size(260, 22);
      this.cboBox10.TabIndex = 9;
      this.cboBox10.Tag = (object) "10";
      this.cboBox10.Leave += new EventHandler(this.leaveField);
      this.cboBox10.Enter += new EventHandler(this.enterField);
      this.cboBox10.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox10.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox10.TextChanged += new EventHandler(this.formatInput);
      this.cboBox9.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox9.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox9.Location = new Point(301, 202);
      this.cboBox9.MaxDropDownItems = 20;
      this.cboBox9.Name = "cboBox9";
      this.cboBox9.Size = new Size(260, 22);
      this.cboBox9.TabIndex = 8;
      this.cboBox9.Tag = (object) "9";
      this.cboBox9.Leave += new EventHandler(this.leaveField);
      this.cboBox9.Enter += new EventHandler(this.enterField);
      this.cboBox9.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox9.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox9.TextChanged += new EventHandler(this.formatInput);
      this.cboBox21.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox21.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox21.Location = new Point(301, 490);
      this.cboBox21.MaxDropDownItems = 20;
      this.cboBox21.Name = "cboBox21";
      this.cboBox21.Size = new Size(260, 22);
      this.cboBox21.TabIndex = 20;
      this.cboBox21.Tag = (object) "21";
      this.cboBox21.Leave += new EventHandler(this.leaveField);
      this.cboBox21.Enter += new EventHandler(this.enterField);
      this.cboBox21.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox21.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox21.TextChanged += new EventHandler(this.formatInput);
      this.cboBox20.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox20.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox20.Location = new Point(301, 466);
      this.cboBox20.MaxDropDownItems = 20;
      this.cboBox20.Name = "cboBox20";
      this.cboBox20.Size = new Size(260, 22);
      this.cboBox20.TabIndex = 19;
      this.cboBox20.Tag = (object) "20";
      this.cboBox20.Leave += new EventHandler(this.leaveField);
      this.cboBox20.Enter += new EventHandler(this.enterField);
      this.cboBox20.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox20.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox20.TextChanged += new EventHandler(this.formatInput);
      this.cboBox19.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox19.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox19.Location = new Point(301, 442);
      this.cboBox19.MaxDropDownItems = 20;
      this.cboBox19.Name = "cboBox19";
      this.cboBox19.Size = new Size(260, 22);
      this.cboBox19.TabIndex = 18;
      this.cboBox19.Tag = (object) "19";
      this.cboBox19.Leave += new EventHandler(this.leaveField);
      this.cboBox19.Enter += new EventHandler(this.enterField);
      this.cboBox19.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox19.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox19.TextChanged += new EventHandler(this.formatInput);
      this.cboBox18.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox18.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox18.Location = new Point(301, 418);
      this.cboBox18.MaxDropDownItems = 20;
      this.cboBox18.Name = "cboBox18";
      this.cboBox18.Size = new Size(260, 22);
      this.cboBox18.TabIndex = 17;
      this.cboBox18.Tag = (object) "18";
      this.cboBox18.Leave += new EventHandler(this.leaveField);
      this.cboBox18.Enter += new EventHandler(this.enterField);
      this.cboBox18.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox18.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox18.TextChanged += new EventHandler(this.formatInput);
      this.cboBox17.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox17.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox17.Location = new Point(301, 394);
      this.cboBox17.MaxDropDownItems = 20;
      this.cboBox17.Name = "cboBox17";
      this.cboBox17.Size = new Size(260, 22);
      this.cboBox17.TabIndex = 16;
      this.cboBox17.Tag = (object) "17";
      this.cboBox17.Leave += new EventHandler(this.leaveField);
      this.cboBox17.Enter += new EventHandler(this.enterField);
      this.cboBox17.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox17.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox17.TextChanged += new EventHandler(this.formatInput);
      this.cboBox25.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox25.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox25.Location = new Point(301, 586);
      this.cboBox25.MaxDropDownItems = 20;
      this.cboBox25.Name = "cboBox25";
      this.cboBox25.Size = new Size(260, 22);
      this.cboBox25.TabIndex = 24;
      this.cboBox25.Tag = (object) "25";
      this.cboBox25.Leave += new EventHandler(this.leaveField);
      this.cboBox25.Enter += new EventHandler(this.enterField);
      this.cboBox25.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox25.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox25.TextChanged += new EventHandler(this.formatInput);
      this.cboBox24.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox24.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox24.Location = new Point(301, 562);
      this.cboBox24.MaxDropDownItems = 20;
      this.cboBox24.Name = "cboBox24";
      this.cboBox24.Size = new Size(260, 22);
      this.cboBox24.TabIndex = 23;
      this.cboBox24.Tag = (object) "24";
      this.cboBox24.Leave += new EventHandler(this.leaveField);
      this.cboBox24.Enter += new EventHandler(this.enterField);
      this.cboBox24.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox24.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox24.TextChanged += new EventHandler(this.formatInput);
      this.cboBox23.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox23.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox23.Location = new Point(301, 538);
      this.cboBox23.MaxDropDownItems = 20;
      this.cboBox23.Name = "cboBox23";
      this.cboBox23.Size = new Size(260, 22);
      this.cboBox23.TabIndex = 22;
      this.cboBox23.Tag = (object) "23";
      this.cboBox23.Leave += new EventHandler(this.leaveField);
      this.cboBox23.Enter += new EventHandler(this.enterField);
      this.cboBox23.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox23.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox23.TextChanged += new EventHandler(this.formatInput);
      this.cboBox22.DropDownStyle = ComboBoxStyle.Simple;
      this.cboBox22.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboBox22.Location = new Point(301, 514);
      this.cboBox22.MaxDropDownItems = 20;
      this.cboBox22.Name = "cboBox22";
      this.cboBox22.Size = new Size(260, 22);
      this.cboBox22.TabIndex = 21;
      this.cboBox22.Tag = (object) "22";
      this.cboBox22.Leave += new EventHandler(this.leaveField);
      this.cboBox22.Enter += new EventHandler(this.enterField);
      this.cboBox22.MouseDown += new MouseEventHandler(this.cboBox_MouseDown);
      this.cboBox22.KeyDown += new KeyEventHandler(this.box_KeyDown);
      this.cboBox22.TextChanged += new EventHandler(this.formatInput);
      this.pboxAsterisk.Image = (System.Drawing.Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(594, 277);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(4, 12);
      this.pboxAsterisk.TabIndex = 149;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.pboxDownArrow.Image = (System.Drawing.Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(587, 297);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 150;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.label50.AutoSize = true;
      this.label50.Location = new Point(4, 590);
      this.label50.Name = "label50";
      this.label50.Size = new Size(16, 13);
      this.label50.TabIndex = 148;
      this.label50.Text = "1.";
      this.label50.TextAlign = ContentAlignment.MiddleLeft;
      this.label49.AutoSize = true;
      this.label49.Location = new Point(4, 566);
      this.label49.Name = "label49";
      this.label49.Size = new Size(16, 13);
      this.label49.TabIndex = 147;
      this.label49.Text = "1.";
      this.label49.TextAlign = ContentAlignment.MiddleLeft;
      this.label48.AutoSize = true;
      this.label48.Location = new Point(4, 542);
      this.label48.Name = "label48";
      this.label48.Size = new Size(16, 13);
      this.label48.TabIndex = 146;
      this.label48.Text = "1.";
      this.label48.TextAlign = ContentAlignment.MiddleLeft;
      this.label47.AutoSize = true;
      this.label47.Location = new Point(4, 518);
      this.label47.Name = "label47";
      this.label47.Size = new Size(16, 13);
      this.label47.TabIndex = 145;
      this.label47.Text = "1.";
      this.label47.TextAlign = ContentAlignment.MiddleLeft;
      this.label46.AutoSize = true;
      this.label46.Location = new Point(4, 494);
      this.label46.Name = "label46";
      this.label46.Size = new Size(16, 13);
      this.label46.TabIndex = 144;
      this.label46.Text = "1.";
      this.label46.TextAlign = ContentAlignment.MiddleLeft;
      this.label45.AutoSize = true;
      this.label45.Location = new Point(4, 470);
      this.label45.Name = "label45";
      this.label45.Size = new Size(16, 13);
      this.label45.TabIndex = 143;
      this.label45.Text = "1.";
      this.label45.TextAlign = ContentAlignment.MiddleLeft;
      this.label44.AutoSize = true;
      this.label44.Location = new Point(4, 446);
      this.label44.Name = "label44";
      this.label44.Size = new Size(16, 13);
      this.label44.TabIndex = 142;
      this.label44.Text = "1.";
      this.label44.TextAlign = ContentAlignment.MiddleLeft;
      this.label43.AutoSize = true;
      this.label43.Location = new Point(4, 422);
      this.label43.Name = "label43";
      this.label43.Size = new Size(16, 13);
      this.label43.TabIndex = 141;
      this.label43.Text = "1.";
      this.label43.TextAlign = ContentAlignment.MiddleLeft;
      this.label42.AutoSize = true;
      this.label42.Location = new Point(4, 398);
      this.label42.Name = "label42";
      this.label42.Size = new Size(16, 13);
      this.label42.TabIndex = 140;
      this.label42.Text = "1.";
      this.label42.TextAlign = ContentAlignment.MiddleLeft;
      this.label41.AutoSize = true;
      this.label41.Location = new Point(4, 374);
      this.label41.Name = "label41";
      this.label41.Size = new Size(16, 13);
      this.label41.TabIndex = 139;
      this.label41.Text = "1.";
      this.label41.TextAlign = ContentAlignment.MiddleLeft;
      this.label40.AutoSize = true;
      this.label40.Location = new Point(4, 350);
      this.label40.Name = "label40";
      this.label40.Size = new Size(16, 13);
      this.label40.TabIndex = 138;
      this.label40.Text = "1.";
      this.label40.TextAlign = ContentAlignment.MiddleLeft;
      this.label39.AutoSize = true;
      this.label39.Location = new Point(4, 326);
      this.label39.Name = "label39";
      this.label39.Size = new Size(16, 13);
      this.label39.TabIndex = 137;
      this.label39.Text = "1.";
      this.label39.TextAlign = ContentAlignment.MiddleLeft;
      this.label38.AutoSize = true;
      this.label38.Location = new Point(4, 302);
      this.label38.Name = "label38";
      this.label38.Size = new Size(16, 13);
      this.label38.TabIndex = 136;
      this.label38.Text = "1.";
      this.label38.TextAlign = ContentAlignment.MiddleLeft;
      this.label37.AutoSize = true;
      this.label37.Location = new Point(4, 278);
      this.label37.Name = "label37";
      this.label37.Size = new Size(16, 13);
      this.label37.TabIndex = 135;
      this.label37.Text = "1.";
      this.label37.TextAlign = ContentAlignment.MiddleLeft;
      this.label36.AutoSize = true;
      this.label36.Location = new Point(4, 254);
      this.label36.Name = "label36";
      this.label36.Size = new Size(16, 13);
      this.label36.TabIndex = 134;
      this.label36.Text = "1.";
      this.label36.TextAlign = ContentAlignment.MiddleLeft;
      this.label35.AutoSize = true;
      this.label35.Location = new Point(4, 230);
      this.label35.Name = "label35";
      this.label35.Size = new Size(16, 13);
      this.label35.TabIndex = 133;
      this.label35.Text = "1.";
      this.label35.TextAlign = ContentAlignment.MiddleLeft;
      this.label34.AutoSize = true;
      this.label34.Location = new Point(4, 206);
      this.label34.Name = "label34";
      this.label34.Size = new Size(16, 13);
      this.label34.TabIndex = 132;
      this.label34.Text = "1.";
      this.label34.TextAlign = ContentAlignment.MiddleLeft;
      this.label33.AutoSize = true;
      this.label33.Location = new Point(4, 182);
      this.label33.Name = "label33";
      this.label33.Size = new Size(16, 13);
      this.label33.TabIndex = 131;
      this.label33.Text = "1.";
      this.label33.TextAlign = ContentAlignment.MiddleLeft;
      this.label32.AutoSize = true;
      this.label32.Location = new Point(4, 158);
      this.label32.Name = "label32";
      this.label32.Size = new Size(16, 13);
      this.label32.TabIndex = 130;
      this.label32.Text = "1.";
      this.label32.TextAlign = ContentAlignment.MiddleLeft;
      this.label31.AutoSize = true;
      this.label31.Location = new Point(4, 134);
      this.label31.Name = "label31";
      this.label31.Size = new Size(16, 13);
      this.label31.TabIndex = 129;
      this.label31.Text = "1.";
      this.label31.TextAlign = ContentAlignment.MiddleLeft;
      this.label30.AutoSize = true;
      this.label30.Location = new Point(4, 110);
      this.label30.Name = "label30";
      this.label30.Size = new Size(16, 13);
      this.label30.TabIndex = 128;
      this.label30.Text = "1.";
      this.label30.TextAlign = ContentAlignment.MiddleLeft;
      this.label28.AutoSize = true;
      this.label28.Location = new Point(4, 62);
      this.label28.Name = "label28";
      this.label28.Size = new Size(16, 13);
      this.label28.TabIndex = (int) sbyte.MaxValue;
      this.label28.Text = "1.";
      this.label28.TextAlign = ContentAlignment.MiddleLeft;
      this.label29.AutoSize = true;
      this.label29.Location = new Point(4, 86);
      this.label29.Name = "label29";
      this.label29.Size = new Size(16, 13);
      this.label29.TabIndex = 126;
      this.label29.Text = "1.";
      this.label29.TextAlign = ContentAlignment.MiddleLeft;
      this.label27.AutoSize = true;
      this.label27.Location = new Point(4, 38);
      this.label27.Name = "label27";
      this.label27.Size = new Size(16, 13);
      this.label27.TabIndex = 125;
      this.label27.Text = "1.";
      this.label27.TextAlign = ContentAlignment.MiddleLeft;
      this.label26.AutoSize = true;
      this.label26.Location = new Point(4, 14);
      this.label26.Name = "label26";
      this.label26.Size = new Size(16, 13);
      this.label26.TabIndex = 124;
      this.label26.Text = "1.";
      this.label26.TextAlign = ContentAlignment.MiddleLeft;
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label25);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label19);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label24);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label23);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label22);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label21);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label20);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label18);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label12);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label17);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label10);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label16);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label11);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label15);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label14);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label13);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label9);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label8);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label7);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label2);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label6);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label26);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label50);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox24);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label49);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label48);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox25);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label47);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox5);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label46);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox3);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label45);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox17);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label44);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label43);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox18);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label42);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox19);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label41);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox8);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label40);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox20);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label39);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox21);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label38);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label37);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox4);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label36);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label35);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label34);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label33);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox2);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label32);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label31);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label30);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label28);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label29);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.label27);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox7);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox23);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox1);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox6);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox12);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox9);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox22);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox13);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox16);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox10);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox15);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox11);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.cboBox14);
      this.borderPanel1.Location = new Point(4, 4);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(568, 618);
      this.borderPanel1.TabIndex = 125;
      this.label25.BackColor = Color.WhiteSmoke;
      this.label25.Location = new Point(39, 587);
      this.label25.Name = "label25";
      this.label25.ReadOnly = true;
      this.label25.Size = new Size(260, 20);
      this.label25.TabIndex = 173;
      this.label19.BackColor = Color.WhiteSmoke;
      this.label19.Location = new Point(39, 443);
      this.label19.Name = "label19";
      this.label19.ReadOnly = true;
      this.label19.Size = new Size(260, 20);
      this.label19.TabIndex = 167;
      this.label24.BackColor = Color.WhiteSmoke;
      this.label24.Location = new Point(39, 563);
      this.label24.Name = "label24";
      this.label24.ReadOnly = true;
      this.label24.Size = new Size(260, 20);
      this.label24.TabIndex = 172;
      this.label23.BackColor = Color.WhiteSmoke;
      this.label23.Location = new Point(39, 539);
      this.label23.Name = "label23";
      this.label23.ReadOnly = true;
      this.label23.Size = new Size(260, 20);
      this.label23.TabIndex = 171;
      this.label22.BackColor = Color.WhiteSmoke;
      this.label22.Location = new Point(39, 515);
      this.label22.Name = "label22";
      this.label22.ReadOnly = true;
      this.label22.Size = new Size(260, 20);
      this.label22.TabIndex = 170;
      this.label21.BackColor = Color.WhiteSmoke;
      this.label21.Location = new Point(39, 491);
      this.label21.Name = "label21";
      this.label21.ReadOnly = true;
      this.label21.Size = new Size(260, 20);
      this.label21.TabIndex = 169;
      this.label20.BackColor = Color.WhiteSmoke;
      this.label20.Location = new Point(39, 467);
      this.label20.Name = "label20";
      this.label20.ReadOnly = true;
      this.label20.Size = new Size(260, 20);
      this.label20.TabIndex = 168;
      this.label18.BackColor = Color.WhiteSmoke;
      this.label18.Location = new Point(39, 419);
      this.label18.Name = "label18";
      this.label18.ReadOnly = true;
      this.label18.Size = new Size(260, 20);
      this.label18.TabIndex = 166;
      this.label12.BackColor = Color.WhiteSmoke;
      this.label12.Location = new Point(39, 275);
      this.label12.Name = "label12";
      this.label12.ReadOnly = true;
      this.label12.Size = new Size(260, 20);
      this.label12.TabIndex = 160;
      this.label17.BackColor = Color.WhiteSmoke;
      this.label17.Location = new Point(39, 395);
      this.label17.Name = "label17";
      this.label17.ReadOnly = true;
      this.label17.Size = new Size(260, 20);
      this.label17.TabIndex = 165;
      this.label10.BackColor = Color.WhiteSmoke;
      this.label10.Location = new Point(39, 227);
      this.label10.Name = "label10";
      this.label10.ReadOnly = true;
      this.label10.Size = new Size(260, 20);
      this.label10.TabIndex = 158;
      this.label16.BackColor = Color.WhiteSmoke;
      this.label16.Location = new Point(39, 371);
      this.label16.Name = "label16";
      this.label16.ReadOnly = true;
      this.label16.Size = new Size(260, 20);
      this.label16.TabIndex = 164;
      this.label11.BackColor = Color.WhiteSmoke;
      this.label11.Location = new Point(39, 251);
      this.label11.Name = "label11";
      this.label11.ReadOnly = true;
      this.label11.Size = new Size(260, 20);
      this.label11.TabIndex = 159;
      this.label15.BackColor = Color.WhiteSmoke;
      this.label15.Location = new Point(39, 347);
      this.label15.Name = "label15";
      this.label15.ReadOnly = true;
      this.label15.Size = new Size(260, 20);
      this.label15.TabIndex = 163;
      this.label14.BackColor = Color.WhiteSmoke;
      this.label14.Location = new Point(39, 323);
      this.label14.Name = "label14";
      this.label14.ReadOnly = true;
      this.label14.Size = new Size(260, 20);
      this.label14.TabIndex = 162;
      this.label13.BackColor = Color.WhiteSmoke;
      this.label13.Location = new Point(39, 299);
      this.label13.Name = "label13";
      this.label13.ReadOnly = true;
      this.label13.Size = new Size(260, 20);
      this.label13.TabIndex = 161;
      this.label9.BackColor = Color.WhiteSmoke;
      this.label9.Location = new Point(39, 203);
      this.label9.Name = "label9";
      this.label9.ReadOnly = true;
      this.label9.Size = new Size(260, 20);
      this.label9.TabIndex = 157;
      this.label3.BackColor = Color.WhiteSmoke;
      this.label3.Location = new Point(39, 59);
      this.label3.Name = "label3";
      this.label3.ReadOnly = true;
      this.label3.Size = new Size(260, 20);
      this.label3.TabIndex = 152;
      this.label8.BackColor = Color.WhiteSmoke;
      this.label8.Location = new Point(39, 179);
      this.label8.Name = "label8";
      this.label8.ReadOnly = true;
      this.label8.Size = new Size(260, 20);
      this.label8.TabIndex = 156;
      this.label1.BackColor = Color.WhiteSmoke;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(39, 11);
      this.label1.Name = "label1";
      this.label1.ReadOnly = true;
      this.label1.Size = new Size(260, 20);
      this.label1.TabIndex = 149;
      this.label7.BackColor = Color.WhiteSmoke;
      this.label7.Location = new Point(39, 155);
      this.label7.Name = "label7";
      this.label7.ReadOnly = true;
      this.label7.Size = new Size(260, 20);
      this.label7.TabIndex = 155;
      this.label2.BackColor = Color.WhiteSmoke;
      this.label2.Location = new Point(39, 35);
      this.label2.Name = "label2";
      this.label2.ReadOnly = true;
      this.label2.Size = new Size(260, 20);
      this.label2.TabIndex = 151;
      this.label6.BackColor = Color.WhiteSmoke;
      this.label6.Location = new Point(39, 131);
      this.label6.Name = "label6";
      this.label6.ReadOnly = true;
      this.label6.Size = new Size(260, 20);
      this.label6.TabIndex = 154;
      this.label5.BackColor = Color.WhiteSmoke;
      this.label5.Location = new Point(39, 107);
      this.label5.Name = "label5";
      this.label5.ReadOnly = true;
      this.label5.Size = new Size(260, 20);
      this.label5.TabIndex = 154;
      this.label4.BackColor = Color.WhiteSmoke;
      this.label4.Location = new Point(39, 83);
      this.label4.Name = "label4";
      this.label4.ReadOnly = true;
      this.label4.Size = new Size(260, 20);
      this.label4.TabIndex = 153;
      this.AutoScroll = true;
      this.Controls.Add((System.Windows.Forms.Control) this.pboxAsterisk);
      this.Controls.Add((System.Windows.Forms.Control) this.borderPanel1);
      this.Controls.Add((System.Windows.Forms.Control) this.pboxDownArrow);
      this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (CustomFieldsForm);
      this.Size = new Size(604, 638);
      this.Paint += new PaintEventHandler(this.CustomFieldsForm_Paint);
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void setFieldValue()
    {
      this.fieldTips.RemoveAll();
      int num = (this.page - 1) * 25;
      for (int index = 1; index <= 25; ++index)
      {
        ++num;
        string str = "CUST" + num.ToString("00") + "FV";
        this.comboFields[index - 1].Tag = (object) str;
        this.fieldTips.SetToolTip((System.Windows.Forms.Control) this.labelFields[index - 1], "CUST" + num.ToString("00") + "DESC");
        this.fieldTips.SetToolTip((System.Windows.Forms.Control) this.comboFields[index - 1], str);
        CustomFieldInfo field1 = this.customFields.GetField(str);
        if (field1.Format == FieldFormat.STRING)
          this.comboFields[index - 1].MaxLength = field1.MaxLength;
        if (this.loan != null)
        {
          string field2 = this.loan.GetField(str);
          this.comboFields[index - 1].Text = field2;
          if (!this.loan.IsTemplate && !this.loan.IsInFindFieldForm && this.isFirstLoading)
            this.popupRules.SetBusinessRules((object) this.comboFields[index - 1], str);
          if (this.loan.IsInFindFieldForm)
          {
            switch (this.loan.SelectedFieldType(str))
            {
              case LoanData.FindFieldTypes.NewSelect:
                this.comboFields[index - 1].BackColor = InputHandlerBase.SelectedFieldColor;
                continue;
              case LoanData.FindFieldTypes.Existing:
                this.comboFields[index - 1].BackColor = InputHandlerBase.ExistingFieldColor;
                continue;
              default:
                this.comboFields[index - 1].BackColor = Color.White;
                continue;
            }
          }
        }
      }
    }

    private void formatInput(object sender, EventArgs e)
    {
      if (sender == null || !(sender is ComboBox))
        return;
      ComboBox comboBox = (ComboBox) sender;
      FieldFormat dataFormat = FieldFormat.NONE;
      CustomFieldInfo field = this.customFields.GetField(comboBox.Tag.ToString());
      if (field != null)
        dataFormat = field.Format;
      bool needsUpdate = false;
      string str = Utils.FormatInput(comboBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      comboBox.Text = str;
      comboBox.SelectionStart = str.Length;
    }

    private void InitializeControls()
    {
      this.labelFields[0] = this.label1;
      this.labelFields[1] = this.label2;
      this.labelFields[2] = this.label3;
      this.labelFields[3] = this.label4;
      this.labelFields[4] = this.label5;
      this.labelFields[5] = this.label6;
      this.labelFields[6] = this.label7;
      this.labelFields[7] = this.label8;
      this.labelFields[8] = this.label9;
      this.labelFields[9] = this.label10;
      this.labelFields[10] = this.label11;
      this.labelFields[11] = this.label12;
      this.labelFields[12] = this.label13;
      this.labelFields[13] = this.label14;
      this.labelFields[14] = this.label15;
      this.labelFields[15] = this.label16;
      this.labelFields[16] = this.label17;
      this.labelFields[17] = this.label18;
      this.labelFields[18] = this.label19;
      this.labelFields[19] = this.label20;
      this.labelFields[20] = this.label21;
      this.labelFields[21] = this.label22;
      this.labelFields[22] = this.label23;
      this.labelFields[23] = this.label24;
      this.labelFields[24] = this.label25;
      this.labelNumber[0] = this.label26;
      this.labelNumber[1] = this.label27;
      this.labelNumber[2] = this.label28;
      this.labelNumber[3] = this.label29;
      this.labelNumber[4] = this.label30;
      this.labelNumber[5] = this.label31;
      this.labelNumber[6] = this.label32;
      this.labelNumber[7] = this.label33;
      this.labelNumber[8] = this.label34;
      this.labelNumber[9] = this.label35;
      this.labelNumber[10] = this.label36;
      this.labelNumber[11] = this.label37;
      this.labelNumber[12] = this.label38;
      this.labelNumber[13] = this.label39;
      this.labelNumber[14] = this.label40;
      this.labelNumber[15] = this.label41;
      this.labelNumber[16] = this.label42;
      this.labelNumber[17] = this.label43;
      this.labelNumber[18] = this.label44;
      this.labelNumber[19] = this.label45;
      this.labelNumber[20] = this.label46;
      this.labelNumber[21] = this.label47;
      this.labelNumber[22] = this.label48;
      this.labelNumber[23] = this.label49;
      this.labelNumber[24] = this.label50;
      this.comboFields[0] = this.cboBox1;
      this.comboFields[1] = this.cboBox2;
      this.comboFields[2] = this.cboBox3;
      this.comboFields[3] = this.cboBox4;
      this.comboFields[4] = this.cboBox5;
      this.comboFields[5] = this.cboBox6;
      this.comboFields[6] = this.cboBox7;
      this.comboFields[7] = this.cboBox8;
      this.comboFields[8] = this.cboBox9;
      this.comboFields[9] = this.cboBox10;
      this.comboFields[10] = this.cboBox11;
      this.comboFields[11] = this.cboBox12;
      this.comboFields[12] = this.cboBox13;
      this.comboFields[13] = this.cboBox14;
      this.comboFields[14] = this.cboBox15;
      this.comboFields[15] = this.cboBox16;
      this.comboFields[16] = this.cboBox17;
      this.comboFields[17] = this.cboBox18;
      this.comboFields[18] = this.cboBox19;
      this.comboFields[19] = this.cboBox20;
      this.comboFields[20] = this.cboBox21;
      this.comboFields[21] = this.cboBox22;
      this.comboFields[22] = this.cboBox23;
      this.comboFields[23] = this.cboBox24;
      this.comboFields[24] = this.cboBox25;
    }

    private void leaveField(object sender, EventArgs e)
    {
      if (sender == null || !(sender is ComboBox))
        return;
      ComboBox ctrl = (ComboBox) sender;
      string id = ctrl.Tag.ToString();
      string simpleField = this.loan.GetSimpleField(id);
      if (!(simpleField != ctrl.Text) || !ctrl.Enabled)
        return;
      if (!this.popupRules.RuleValidate((object) ctrl, id))
        return;
      try
      {
        this.loan.SetField(id, ctrl.Text);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.loan.SetField(id, simpleField);
        ctrl.Text = simpleField;
      }
      this.piggySyncTool.SyncPiggyBackField(id, FieldSource.LinkedLoan, ctrl.Text);
      try
      {
        if (this.loan != null && !this.loan.IsTemplate)
        {
          TriggerImplDef def = Session.LoanDataMgr.ApplyLoanTemplateTrigger(id, ctrl.Text);
          if (def != null)
          {
            this.showApplyLoanTemplateProgress();
            Session.LoanDataMgr.ApplyLoanTemplate(def);
            this.closeProgress();
          }
        }
        Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
      }
    }

    private void threadStart(object message)
    {
      this.statusReport = new StatusReport(string.Concat(message));
      this.statusReport.Text = "Applying loan template";
      Application.Run((System.Windows.Forms.Form) this.statusReport);
    }

    private void closeProgress()
    {
      if (this.statusReport == null)
        return;
      if (this.statusReport.InvokeRequired)
      {
        this.statusReport.Invoke((Delegate) new MethodInvoker(this.closeProgress));
      }
      else
      {
        try
        {
          this.statusReport.Close();
        }
        catch
        {
        }
      }
    }

    private void showApplyLoanTemplateProgress()
    {
      new Thread(new ParameterizedThreadStart(this.threadStart))
      {
        IsBackground = true
      }.Start((object) "Please wait. Applying loan template is in progress.");
    }

    private int toInt(string val)
    {
      try
      {
        return Convert.ToInt32(val == string.Empty || val == null ? 0.0 : double.Parse(val));
      }
      catch
      {
        return 0;
      }
    }

    private void enterField(object sender, EventArgs e)
    {
      if (this.loan.IsTemplate || sender == null || !(sender is ComboBox))
        return;
      string id = ((System.Windows.Forms.Control) sender).Tag.ToString();
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(id);
    }

    private void CustomFieldsForm_Paint(object sender, PaintEventArgs e)
    {
      if (!this.Visible)
        return;
      this.customFields = Session.ConfigurationManager.GetLoanCustomFields();
      this.setDescription();
    }

    private void setDescription()
    {
      int num = (this.page - 1) * 25;
      for (int index1 = 1; index1 <= 25; ++index1)
      {
        ++num;
        string str = "CUST" + num.ToString("00") + "FV";
        CustomFieldInfo customFieldInfo = this.customFields.GetField(str) ?? new CustomFieldInfo(str);
        this.labelFields[index1 - 1].Text = customFieldInfo.Description;
        this.labelNumber[index1 - 1].Text = num.ToString() + ".";
        this.comboFields[index1 - 1].DropDownStyle = customFieldInfo.Format != FieldFormat.DROPDOWN ? (customFieldInfo.Format != FieldFormat.DROPDOWNLIST ? ComboBoxStyle.Simple : ComboBoxStyle.DropDownList) : ComboBoxStyle.DropDown;
        if (customFieldInfo.Format == FieldFormat.DROPDOWN || customFieldInfo.Format == FieldFormat.DROPDOWNLIST)
        {
          string text = this.comboFields[index1 - 1].Text;
          this.comboFields[index1 - 1].Items.Clear();
          string[] options = customFieldInfo.Options;
          if (options != null)
          {
            for (int index2 = 0; index2 < options.Length; ++index2)
            {
              this.comboFields[index1 - 1].Items.Add((object) options[index2]);
              if (customFieldInfo.Format == FieldFormat.DROPDOWNLIST && text == options[index2])
                this.comboFields[index1 - 1].SelectedIndex = index2;
            }
            if (customFieldInfo.Format == FieldFormat.DROPDOWN)
              this.comboFields[index1 - 1].Text = text;
          }
        }
        if (customFieldInfo.Format == FieldFormat.AUDIT || customFieldInfo.Calculation != "")
          this.comboFields[index1 - 1].Enabled = false;
        else
          this.comboFields[index1 - 1].Enabled = true;
      }
    }

    private void box_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.D)
        return;
      ComboBox comboBox1 = (ComboBox) sender;
      CustomFieldInfo field = this.customFields.GetField(comboBox1.Tag.ToString());
      if (field == null)
        return;
      switch (field.Format)
      {
        case FieldFormat.DATE:
          comboBox1.Text = DateTime.Today.ToString("MM/dd/yyyy");
          break;
        case FieldFormat.MONTHDAY:
          ComboBox comboBox2 = comboBox1;
          int num = DateTime.Today.Month;
          string str1 = num.ToString("00");
          num = DateTime.Today.Day;
          string str2 = num.ToString("00");
          string str3 = str1 + "/" + str2;
          comboBox2.Text = str3;
          break;
      }
    }

    private void cboBox_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.loan == null || !this.loan.IsInFindFieldForm)
        return;
      ComboBox comboBox = (ComboBox) sender;
      string id = comboBox.Tag.ToString();
      switch (this.loan.SelectedFieldType(id))
      {
        case LoanData.FindFieldTypes.None:
          this.loan.AddSelectedField(id);
          comboBox.BackColor = Color.LightYellow;
          break;
        case LoanData.FindFieldTypes.NewSelect:
          this.loan.RemoveSelectedField(id);
          comboBox.BackColor = Color.White;
          break;
        case LoanData.FindFieldTypes.Existing:
          int num = (int) Utils.Dialog((IWin32Window) null, "You can't remove existing selected field in current list. Please use 'Remove' button to remove existing fields.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          break;
      }
    }

    private void popupRules_DropdownSelected(object sender, EventArgs e)
    {
      if (!(sender is ComboBox))
        return;
      ComboBox comboBox = (ComboBox) sender;
      this.loan.SetCurrentField(comboBox.Tag.ToString(), comboBox.Text);
    }

    public void RefreshContents() => this.setFieldValue();

    public void RefreshLoanContents() => this.RefreshContents();
  }
}
