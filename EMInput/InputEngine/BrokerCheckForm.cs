// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.BrokerCheckForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class BrokerCheckForm : CustomUserControl, IRefreshContents
  {
    private ToolTip fieldToolTip;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label7;
    private IContainer components;
    private LoanData loan;
    private TextBox textBoxFaxNumber;
    private TextBox textBoxPhoneNumber;
    private TextBox textBoxContactName;
    private TextBox textBoxBrokerName;
    private TextBox textBoxTotal;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private StandardIconButton btnRolodex;
    private StandardIconButton btnPhone;
    private BorderPanel borderPanel1;
    private TableContainer tableContainer1;
    private GridView listViewFees;
    private StandardIconButton btnFax;
    private DatePicker dtDate;
    private PopupBusinessRules popupRules;

    public BrokerCheckForm(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.textBoxBrokerName.Tag = (object) "VEND.X293";
      this.textBoxContactName.Tag = (object) "VEND.X302";
      this.textBoxPhoneNumber.Tag = (object) "VEND.X303";
      this.textBoxFaxNumber.Tag = (object) "VEND.X306";
      this.dtDate.Tag = (object) "VEND.X368";
      this.textBoxTotal.Tag = (object) "1988";
      this.fieldToolTip.SetToolTip((Control) this.textBoxBrokerName, "VEND.X293: File Contacts Broker Name");
      this.fieldToolTip.SetToolTip((Control) this.textBoxContactName, "VEND.X302: File Contacts Broker Contact Name");
      this.fieldToolTip.SetToolTip((Control) this.textBoxPhoneNumber, "VEND.X303: File Contacts Broker Phone 1");
      this.fieldToolTip.SetToolTip((Control) this.textBoxFaxNumber, "VEND.X306: File Contacts Broker Fax");
      this.fieldToolTip.SetToolTip((Control) this.textBoxTotal, "1988: Trans Details Total Paid to Broker");
      this.fieldToolTip.SetToolTip((Control) this.dtDate, "VEND.X368: File Contacts Broker Check Confirmed Date");
      this.initForm();
      if (this.loan != null && this.loan.IsFieldReadOnly("VEND.X293"))
        this.textBoxBrokerName.ReadOnly = true;
      if (this.loan != null && this.loan.IsFieldReadOnly("VEND.X302"))
        this.textBoxContactName.ReadOnly = true;
      if (this.loan != null && this.loan.IsFieldReadOnly("VEND.X303"))
        this.textBoxPhoneNumber.ReadOnly = true;
      if (this.loan != null && this.loan.IsFieldReadOnly("VEND.X306"))
        this.textBoxFaxNumber.ReadOnly = true;
      if (this.loan != null && this.loan.IsFieldReadOnly("VEND.X368"))
        this.dtDate.ReadOnly = true;
      if (this.loan != null && this.loan.IsFieldReadOnly("1988"))
        this.textBoxTotal.ReadOnly = true;
      ResourceManager resources = new ResourceManager(typeof (BrokerCheckForm));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      this.popupRules.SetBusinessRules((object) this.textBoxBrokerName, "VEND.X293");
      this.popupRules.SetBusinessRules((object) this.textBoxContactName, "VEND.X302");
      this.popupRules.SetBusinessRules((object) this.textBoxPhoneNumber, "VEND.X303");
      this.popupRules.SetBusinessRules((object) this.textBoxFaxNumber, "VEND.X306");
      this.popupRules.SetBusinessRules((object) this.dtDate, "VEND.X368");
      this.popupRules.SetBusinessRules((object) this.textBoxTotal, "1988");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BrokerCheckForm));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      this.fieldToolTip = new ToolTip(this.components);
      this.btnPhone = new StandardIconButton();
      this.btnRolodex = new StandardIconButton();
      this.btnFax = new StandardIconButton();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.label7 = new Label();
      this.textBoxTotal = new TextBox();
      this.textBoxBrokerName = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.textBoxFaxNumber = new TextBox();
      this.textBoxPhoneNumber = new TextBox();
      this.textBoxContactName = new TextBox();
      this.label1 = new Label();
      this.borderPanel1 = new BorderPanel();
      this.dtDate = new DatePicker();
      this.tableContainer1 = new TableContainer();
      this.listViewFees = new GridView();
      ((ISupportInitialize) this.btnPhone).BeginInit();
      ((ISupportInitialize) this.btnRolodex).BeginInit();
      ((ISupportInitialize) this.btnFax).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.borderPanel1.SuspendLayout();
      this.tableContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnPhone.BackColor = Color.Transparent;
      this.btnPhone.Location = new Point(616, 32);
      this.btnPhone.Name = "btnPhone";
      this.btnPhone.Size = new Size(16, 16);
      this.btnPhone.StandardButtonType = StandardIconButton.ButtonType.PhoneButton;
      this.btnPhone.TabIndex = 13;
      this.btnPhone.TabStop = false;
      this.fieldToolTip.SetToolTip((Control) this.btnPhone, "Open the Conversation Log");
      this.btnPhone.Click += new EventHandler(this.pictureBoxPhone_Click);
      this.btnRolodex.BackColor = Color.Transparent;
      this.btnRolodex.Location = new Point(278, 10);
      this.btnRolodex.Name = "btnRolodex";
      this.btnRolodex.Size = new Size(16, 16);
      this.btnRolodex.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnRolodex.TabIndex = 12;
      this.btnRolodex.TabStop = false;
      this.fieldToolTip.SetToolTip((Control) this.btnRolodex, "Select a Business Contact");
      this.btnRolodex.Click += new EventHandler(this.pictureBoxContact_Click);
      this.btnFax.BackColor = Color.Transparent;
      this.btnFax.Location = new Point(616, 53);
      this.btnFax.Name = "btnFax";
      this.btnFax.Size = new Size(16, 16);
      this.btnFax.StandardButtonType = StandardIconButton.ButtonType.FaxPhoneButton;
      this.btnFax.TabIndex = 72;
      this.btnFax.TabStop = false;
      this.fieldToolTip.SetToolTip((Control) this.btnFax, "Open the Conversation Log");
      this.btnFax.Click += new EventHandler(this.btnFax_Click);
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(56, 57);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 70;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(20, 57);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 69;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(390, 575);
      this.label7.Name = "label7";
      this.label7.Size = new Size(118, 14);
      this.label7.TabIndex = 10;
      this.label7.Text = "Total Paid To Broker";
      this.textBoxTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.textBoxTotal.Location = new Point(513, 572);
      this.textBoxTotal.Name = "textBoxTotal";
      this.textBoxTotal.ReadOnly = true;
      this.textBoxTotal.Size = new Size(121, 20);
      this.textBoxTotal.TabIndex = 6;
      this.textBoxTotal.TabStop = false;
      this.textBoxTotal.TextAlign = HorizontalAlignment.Right;
      this.textBoxBrokerName.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxBrokerName.Location = new Point(100, 8);
      this.textBoxBrokerName.Name = "textBoxBrokerName";
      this.textBoxBrokerName.Size = new Size(176, 20);
      this.textBoxBrokerName.TabIndex = 1;
      this.textBoxBrokerName.Leave += new EventHandler(this.leaveField);
      this.textBoxBrokerName.MouseDown += new MouseEventHandler(this.textBoxBrokerName_MouseDown);
      this.textBoxBrokerName.Enter += new EventHandler(this.enterField);
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(346, 55);
      this.label5.Name = "label5";
      this.label5.Size = new Size(65, 14);
      this.label5.TabIndex = 9;
      this.label5.Text = "Fax Number";
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(346, 33);
      this.label4.Name = "label4";
      this.label4.Size = new Size(77, 14);
      this.label4.TabIndex = 8;
      this.label4.Text = "Phone Number";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(346, 11);
      this.label3.Name = "label3";
      this.label3.Size = new Size(74, 14);
      this.label3.TabIndex = 7;
      this.label3.Text = "Contact Name";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(5, 33);
      this.label2.Name = "label2";
      this.label2.Size = new Size(89, 14);
      this.label2.TabIndex = 6;
      this.label2.Text = "Check Confirmed";
      this.textBoxFaxNumber.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxFaxNumber.Location = new Point(428, 52);
      this.textBoxFaxNumber.Name = "textBoxFaxNumber";
      this.textBoxFaxNumber.Size = new Size(186, 20);
      this.textBoxFaxNumber.TabIndex = 5;
      this.textBoxFaxNumber.Leave += new EventHandler(this.leaveField);
      this.textBoxFaxNumber.KeyUp += new KeyEventHandler(this.phoneField_Keyup);
      this.textBoxFaxNumber.Enter += new EventHandler(this.enterField);
      this.textBoxPhoneNumber.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxPhoneNumber.Location = new Point(428, 30);
      this.textBoxPhoneNumber.Name = "textBoxPhoneNumber";
      this.textBoxPhoneNumber.Size = new Size(186, 20);
      this.textBoxPhoneNumber.TabIndex = 4;
      this.textBoxPhoneNumber.Leave += new EventHandler(this.leaveField);
      this.textBoxPhoneNumber.KeyUp += new KeyEventHandler(this.phoneField_Keyup);
      this.textBoxPhoneNumber.Enter += new EventHandler(this.enterField);
      this.textBoxContactName.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.textBoxContactName.Location = new Point(428, 8);
      this.textBoxContactName.Name = "textBoxContactName";
      this.textBoxContactName.Size = new Size(186, 20);
      this.textBoxContactName.TabIndex = 3;
      this.textBoxContactName.Leave += new EventHandler(this.leaveField);
      this.textBoxContactName.Enter += new EventHandler(this.enterField);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(5, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(69, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Broker Name";
      this.borderPanel1.Controls.Add((Control) this.dtDate);
      this.borderPanel1.Controls.Add((Control) this.btnFax);
      this.borderPanel1.Controls.Add((Control) this.tableContainer1);
      this.borderPanel1.Controls.Add((Control) this.pboxDownArrow);
      this.borderPanel1.Controls.Add((Control) this.pboxAsterisk);
      this.borderPanel1.Controls.Add((Control) this.btnPhone);
      this.borderPanel1.Controls.Add((Control) this.label1);
      this.borderPanel1.Controls.Add((Control) this.label5);
      this.borderPanel1.Controls.Add((Control) this.btnRolodex);
      this.borderPanel1.Controls.Add((Control) this.label4);
      this.borderPanel1.Controls.Add((Control) this.label3);
      this.borderPanel1.Controls.Add((Control) this.textBoxFaxNumber);
      this.borderPanel1.Controls.Add((Control) this.textBoxBrokerName);
      this.borderPanel1.Controls.Add((Control) this.textBoxPhoneNumber);
      this.borderPanel1.Controls.Add((Control) this.label2);
      this.borderPanel1.Controls.Add((Control) this.textBoxContactName);
      this.borderPanel1.Location = new Point(4, 3);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(641, 678);
      this.borderPanel1.TabIndex = 0;
      this.dtDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.dtDate.BackColor = SystemColors.Window;
      this.dtDate.Location = new Point(100, 30);
      this.dtDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtDate.Name = "dtDate";
      this.dtDate.Size = new Size(176, 21);
      this.dtDate.TabIndex = 2;
      this.dtDate.ToolTip = "";
      this.dtDate.Value = new DateTime(0L);
      this.dtDate.ValueChanged += new EventHandler(this.dtDate_ValueChanged);
      this.dtDate.Leave += new EventHandler(this.dateField_Leave);
      this.dtDate.Enter += new EventHandler(this.enterField);
      this.tableContainer1.Controls.Add((Control) this.label7);
      this.tableContainer1.Controls.Add((Control) this.listViewFees);
      this.tableContainer1.Controls.Add((Control) this.textBoxTotal);
      this.tableContainer1.Location = new Point(0, 83);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(641, 595);
      this.tableContainer1.TabIndex = 71;
      this.tableContainer1.Text = "The following fees are paid to broker as indicated on the Itemization";
      this.listViewFees.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.None;
      gvColumn1.Text = "";
      gvColumn1.Width = 50;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.None;
      gvColumn2.Text = "Fee Description";
      gvColumn2.Width = 362;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.None;
      gvColumn3.Text = "Paid By";
      gvColumn3.Width = 130;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.None;
      gvColumn4.SpringToFit = true;
      gvColumn4.Text = "Amount";
      gvColumn4.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn4.Width = 97;
      this.listViewFees.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listViewFees.Dock = DockStyle.Fill;
      this.listViewFees.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.listViewFees.Location = new Point(1, 26);
      this.listViewFees.Name = "listViewFees";
      this.listViewFees.Size = new Size(639, 543);
      this.listViewFees.SortOption = GVSortOption.None;
      this.listViewFees.TabIndex = 0;
      this.listViewFees.TabStop = false;
      this.Controls.Add((Control) this.borderPanel1);
      this.Name = nameof (BrokerCheckForm);
      this.Size = new Size(648, 684);
      ((ISupportInitialize) this.btnPhone).EndInit();
      ((ISupportInitialize) this.btnRolodex).EndInit();
      ((ISupportInitialize) this.btnFax).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.tableContainer1.ResumeLayout(false);
      this.tableContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void initForm()
    {
      this.textBoxFaxNumber.Text = this.loan.GetField((string) this.textBoxFaxNumber.Tag);
      this.textBoxPhoneNumber.Text = this.loan.GetField((string) this.textBoxPhoneNumber.Tag);
      this.textBoxContactName.Text = this.loan.GetField((string) this.textBoxContactName.Tag);
      this.dtDate.Text = this.loan.GetField((string) this.dtDate.Tag);
      this.textBoxBrokerName.Text = this.loan.GetField((string) this.textBoxBrokerName.Tag);
      this.textBoxTotal.Text = this.loan.GetField((string) this.textBoxTotal.Tag);
      this.listViewFees.Items.Clear();
      this.listViewFees.BeginUpdate();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      double num1 = 0.0;
      bool flag1 = this.loan.GetField("NEWHUD.X1139") == "Y";
      bool flag2 = this.loan.GetField("LE2.X28") == "Y";
      List<GFEItem> gfeItemList = this.loan.GetField("NEWHUD.X354") == "Y" || Utils.CheckIf2015RespaTila(this.loan.GetField("3969")) ? GFEItemCollection.GFEItems2010 : GFEItemCollection.GFEItems;
      for (int index1 = 0; index1 < gfeItemList.Count; ++index1)
      {
        GFEItem gfeItem = gfeItemList[index1];
        if (gfeItem.LineNumber < 2001 && (this.loan.Use2015RESPA || gfeItem.LineNumber >= 100) && (!this.loan.Use2015RESPA || !GFEItemCollection.Excluded2015GFEItem.Contains(gfeItem.LineNumber)) && (!(this.loan.Use2015RESPA & flag2) || gfeItem.LineNumber >= 520 || gfeItem.LineNumber >= 21 && gfeItem.LineNumber <= 45) && (!this.loan.Use2015RESPA || flag2 || gfeItem.LineNumber < 21 || gfeItem.LineNumber > 45) && (this.loan.Use2015RESPA || gfeItem.LineNumber < 1316 || gfeItem.LineNumber > 1320) && (!flag1 || gfeItem.LineNumber != 802 || !(gfeItem.ComponentID == string.Empty)) && (flag1 || gfeItem.LineNumber != 802 || !(gfeItem.ComponentID != string.Empty)))
        {
          if (gfeItem.LineNumber == 802 && (this.loan.Use2010RESPA || this.loan.Use2015RESPA))
          {
            if (!(this.loan.GetField("NEWHUD.X715") == "Include Origination Credit") && (!flag1 || gfeItem.LineNumber != 802 || !(gfeItem.ComponentID == "a") && !(gfeItem.ComponentID == "b") && !(gfeItem.ComponentID == "c") && !(gfeItem.ComponentID == "d")))
            {
              if (this.loan.GetField("NEWHUD.X713") == "Origination Charge")
              {
                if (flag1)
                {
                  gfeItem = gfeItem.Clone();
                  gfeItem.LineNumber = 801;
                  if (gfeItem.ComponentID == "e")
                    gfeItem.ComponentID = "p";
                  if (gfeItem.ComponentID == "f")
                    gfeItem.ComponentID = "q";
                  if (gfeItem.ComponentID == "g")
                    gfeItem.ComponentID = "r";
                  if (gfeItem.ComponentID == "h")
                    gfeItem.ComponentID = "s";
                }
                else
                  gfeItem = new GFEItem(801, "l", "", "NEWHUD.X15", "", "NEWHUD.X749", "", "NEWHUD.X627", "NEWHUD.X398", "", "Origination Points");
              }
            }
            else
              continue;
          }
          string str = !(gfeItem.PaidByFieldID != "") ? string.Empty : this.loan.GetField(gfeItem.PaidByFieldID);
          if (gfeItem.LineNumber >= 520 && (this.loan.Use2010RESPA || this.loan.Use2015RESPA || !(gfeItem.POCFieldID != string.Empty) || !(this.loan.GetField(gfeItem.POCFieldID) == "Y")) && (!(gfeItem.PTBFieldID == string.Empty) && (!(gfeItem.PTBFieldID != string.Empty) || !(this.loan.GetField(gfeItem.PTBFieldID) != "Broker")) || !this.loan.Use2010RESPA && !this.loan.Use2015RESPA && (this.loan.Use2010RESPA || this.loan.Use2015RESPA || gfeItem.LineNumber == 824 || gfeItem.LineNumber == 825)))
          {
            for (int index2 = 1; index2 <= 2; ++index2)
            {
              if ((this.loan.Use2010RESPA || this.loan.Use2015RESPA || gfeItem.LineNumber != 824 && gfeItem.LineNumber != 825 || index2 <= 1) && (gfeItem.LineNumber != 801 || !(gfeItem.ComponentID == "f") || index2 != 2))
              {
                GVItem gvItem = new GVItem(gfeItem.LineNumber.ToString() + gfeItem.ComponentID + ".");
                if (gfeItem.Description.Length <= 4 || gfeItem.Description.StartsWith("NEWHUD.X") || gfeItem.Description.StartsWith("NEWHUD2.X"))
                  gvItem.SubItems.Add((object) this.loan.GetField(gfeItem.Description));
                else
                  gvItem.SubItems.Add((object) FundingFee.GetFeeDescription2015(gfeItem.LineNumber.ToString() + gfeItem.ComponentID + ".", gfeItem.Description));
                double num2 = 0.0;
                if (index2 == 1)
                {
                  if (gfeItem.BorrowerFieldID != string.Empty)
                  {
                    num2 = Utils.ParseDouble((object) this.loan.GetField(gfeItem.BorrowerFieldID));
                    if (flag1 && gfeItem.LineNumber == 802 && (gfeItem.ComponentID == "a" || gfeItem.ComponentID == "b" || gfeItem.ComponentID == "c" || gfeItem.ComponentID == "d"))
                      num2 *= -1.0;
                    if (!flag1 && gfeItem.LineNumber == 801 && gfeItem.ComponentID == "l" && this.loan.GetField("NEWHUD.X713") == "Origination Charge")
                      num2 -= Utils.ParseDouble((object) this.loan.GetField("NEWHUD.X831"));
                    if (gfeItem.POCFieldID != string.Empty && this.loan.GetField(gfeItem.POCFieldID) == "Y")
                    {
                      if (HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) gfeItem.PaidByFieldID))
                      {
                        double num3 = Utils.ParseDouble((object) this.loan.GetField(((string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) gfeItem.PaidByFieldID])[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]));
                        if (num2 != num3)
                          num2 -= num3;
                        else
                          continue;
                      }
                      else
                        continue;
                    }
                  }
                }
                else if (gfeItem.SellerFieldID != string.Empty)
                  num2 = Utils.ParseDouble((object) this.loan.GetField(gfeItem.SellerFieldID));
                if (str != "")
                {
                  gvItem.SubItems.Add((object) str);
                  if (index2 == 1)
                  {
                    double num4 = Utils.ParseDouble((object) this.loan.GetField(gfeItem.SellerFieldID));
                    num2 += num4;
                    ++index2;
                  }
                }
                else if (index2 == 1)
                {
                  if (!this.loan.Use2010RESPA && !this.loan.Use2015RESPA && (gfeItem.LineNumber == 824 || gfeItem.LineNumber == 825))
                    gvItem.SubItems.Add((object) "Lender");
                  else
                    gvItem.SubItems.Add((object) "Borrower");
                }
                else
                  gvItem.SubItems.Add((object) "Seller");
                if (num2 != 0.0)
                {
                  gvItem.SubItems.Add((object) num2.ToString("N2"));
                  gvItem.Tag = (object) gfeItem;
                  num1 += num2;
                  this.listViewFees.Items.Add(gvItem);
                }
              }
            }
          }
        }
      }
      double num5 = Utils.ParseDouble((object) this.loan.GetField("2005"));
      if (this.loan.GetField("2833") == "Y" && num5 != 0.0)
      {
        double num6 = num5 * -1.0;
        GVItem gvItem = new GVItem("");
        gvItem.SubItems.Add((object) "Deduction from Yield Spread Premium");
        gvItem.SubItems.Add((object) "Broker");
        gvItem.SubItems.Add((object) num6.ToString("N2"));
        double num7 = num1 + num6;
        this.listViewFees.Items.Add(gvItem);
      }
      this.listViewFees.EndUpdate();
    }

    private void enterField(object sender, EventArgs e)
    {
      string id = string.Empty;
      switch (sender)
      {
        case TextBox _:
          id = (string) ((Control) sender).Tag;
          break;
        case DatePicker _:
          id = (string) ((Control) sender).Tag;
          break;
      }
      Session.Application.GetService<IStatusDisplay>()?.DisplayFieldID(id);
    }

    private void pictureBoxPhone_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().StartConversation(new ConversationLog(DateTime.Now, Session.UserInfo.FullName)
      {
        Company = this.textBoxBrokerName.Text,
        Phone = this.textBoxPhoneNumber.Text,
        Email = this.loan.GetField("VEND.X305"),
        Name = this.textBoxContactName.Text,
        IsEmail = false
      });
    }

    private void textBoxBrokerName_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.pictureBoxContact_Click((object) null, (EventArgs) null);
    }

    private void pictureBoxContact_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      string[] firstLastName = ContactUtil.GetFirstLastName(this.textBoxContactName.Text);
      rxContact.FirstName = firstLastName[0];
      rxContact.LastName = firstLastName[1];
      CRMRoleType crmRoleType = ContactUtil.GetCRMRoleType("VEND.X293");
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("", "", rxContact.LastName, rxContact, false, crmRoleType, true, string.Concat((object) (int) crmRoleType)))
      {
        if (rxBusinessContact.ShowDialog() == DialogResult.OK)
        {
          if (rxBusinessContact.GoToContact)
          {
            Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
            return;
          }
          if (rxBusinessContact.RxContactRecord != (RxContactInfo) null)
          {
            this.textBoxBrokerName.Text = rxBusinessContact.RxContactRecord.CompanyName;
            this.textBoxContactName.Text = rxBusinessContact.RxContactRecord.FirstName + " " + rxBusinessContact.RxContactRecord.LastName;
            this.textBoxPhoneNumber.Text = rxBusinessContact.RxContactRecord.WorkPhone;
            this.textBoxFaxNumber.Text = rxBusinessContact.RxContactRecord.FaxNumber;
            this.loan.SetField("VEND.X293", rxBusinessContact.RxContactRecord.CompanyName);
            this.loan.SetField("VEND.X294", rxBusinessContact.RxContactRecord.BizAddress1);
            this.loan.SetField("VEND.X295", rxBusinessContact.RxContactRecord.BizCity);
            this.loan.SetField("VEND.X296", rxBusinessContact.RxContactRecord.BizState);
            this.loan.SetField("VEND.X297", rxBusinessContact.RxContactRecord.BizZip);
            this.loan.SetField("VEND.X302", (rxBusinessContact.RxContactRecord.FirstName + " " + rxBusinessContact.RxContactRecord.LastName).Trim());
            this.loan.SetField("VEND.X303", rxBusinessContact.RxContactRecord.WorkPhone);
            this.loan.SetField("VEND.X304", "");
            this.loan.SetField("VEND.X305", rxBusinessContact.RxContactRecord.BizEmail);
            this.loan.SetField("VEND.X306", rxBusinessContact.RxContactRecord.FaxNumber);
            this.loan.SetField("VEND.X1036", rxBusinessContact.RxContactRecord.WebSite);
            this.loan.SetField("VEND.X1037", rxBusinessContact.RxContactRecord.WebSite);
          }
        }
      }
      this.updateBusinessRule();
      this.textBoxBrokerName.Focus();
    }

    private void phoneField_Keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.PHONE;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void leaveField(object sender, EventArgs e)
    {
      TextBox ctrl = (TextBox) sender;
      if (ctrl == null)
        return;
      ctrl.BackColor = Color.White;
      string tag = (string) ctrl.Tag;
      if ((tag ?? "") == "" || this.loan.IsFieldReadOnly(tag))
        return;
      if (this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag))
        this.loan.SetField(tag, ctrl.Text);
      this.updateBusinessRule();
    }

    private void dateField_Leave(object sender, EventArgs e)
    {
      DatePicker datePicker = (DatePicker) sender;
      if (datePicker == null)
        return;
      string tag = (string) datePicker.Tag;
      if ((tag ?? "") == "")
        return;
      if (datePicker.Text == "//" || datePicker.Text == "")
        this.loan.SetField(tag, "");
      else
        this.loan.SetField(tag, datePicker.Text);
      this.updateBusinessRule();
    }

    private void btnFax_Click(object sender, EventArgs e)
    {
      Session.Application.GetService<ILoanEditor>().StartConversation(new ConversationLog(DateTime.Now, Session.UserInfo.FullName)
      {
        Company = this.textBoxBrokerName.Text,
        Phone = this.textBoxFaxNumber.Text,
        Email = this.loan.GetField("VEND.X305"),
        Name = this.textBoxContactName.Text,
        IsEmail = false
      });
    }

    public void RefreshContents() => this.initForm();

    public void RefreshLoanContents() => this.RefreshContents();

    private void dtDate_ValueChanged(object sender, EventArgs e) => this.dateField_Leave(sender, e);

    private void updateBusinessRule()
    {
      try
      {
        if (this.loan == null || this.loan.IsTemplate)
          return;
        Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
      }
    }
  }
}
