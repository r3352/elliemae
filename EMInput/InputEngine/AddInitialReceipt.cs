// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddInitialReceipt
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AddInitialReceipt : Form
  {
    private DocTrackingType docTrackingType;
    private Sessions.Session session;
    private LoanData loanData;
    private Hashtable settings;
    private Hashtable docTypeClearReceivedDt;
    private const int DotFormHight = 487;
    private const int FtpFormHight = 377;
    private const int DotFormWithShippingHight = 729;
    private const int FtpFormWithShippingHight = 627;
    private bool isSchedShipping;
    private string[] carrierList = new string[0];
    private IContainer components;
    private TextBox txtReceivedBy;
    private Label label7;
    private Label label4;
    private Label label1;
    private DatePickerCustom dpReceivedDt;
    private Panel pnlDocType;
    private CheckBox ckFTP;
    private CheckBox ckDot;
    private Panel pnlComments;
    private RichTextBox rtComments;
    private Label label3;
    private Button btnCancel;
    private Button btnOk;
    private Panel pnlShipping;
    private Label label14;
    private DatePickerCustom dpShippingDt;
    private StandardIconButton btnOrg;
    private ComboBox cboxCarrier;
    private TextBox txtPhone;
    private TextBox txtEmail;
    private TextBox txtAddr;
    private TextBox txtContact;
    private TextBox txtOrg;
    private TextBox txtTrackNum;
    private Label label9;
    private Label label8;
    private Label label2;
    private Label label6;
    private Label label5;
    private Label label10;
    private Label label11;
    private Label label13;
    private Label lblDocumentNum;
    private TextBox txtDocumentNumber;
    private Label lblBookNum;
    private TextBox txtBookNumber;
    private Label lblPageNum;
    private TextBox txtPageNumber;
    private Label lblRidersRec;
    private RadioButton rbYes;
    private RadioButton rbNa;
    private RadioButton rbNo;
    private Panel pnlDocNumber;

    public AddInitialReceipt(DocTrackingType docTrackingType)
    {
      this.InitializeComponent();
      this.docTrackingType = docTrackingType;
      this.session = DocTrackingUtils.Session;
      this.loanData = this.session.LoanData;
      this.settings = DocTrackingUtils.DocTrackingSettings;
      this.isSchedShipping = Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) "EnableScheduledShipment"]);
      this.InitCarrierList();
      this.InitData();
    }

    public AddInitialReceipt(DocumentTrackingLog dtl)
    {
      this.InitializeComponent();
      this.btnOk.Visible = false;
      this.btnCancel.Text = "Close";
      this.InitDataFromLog(dtl);
      this.EnableDisableAddInitialReceipt(false);
    }

    private void InitData()
    {
      this.docTypeClearReceivedDt = new Hashtable();
      foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
      {
        string key = control.Tag.ToString();
        if (key != this.docTrackingType.ToString())
        {
          this.docTypeClearReceivedDt[(object) key] = (object) false;
          control.Checked = false;
          control.Enabled = Utils.ParseBoolean(DocTrackingUtils.DocTrackingSettings[(object) ("Enable" + key)]);
        }
        else
        {
          control.Enabled = false;
          control.Checked = true;
        }
      }
      this.dpReceivedDt.Value = DateTime.Now;
      this.txtReceivedBy.Text = this.session.UserInfo.FullName;
      this.ToggleShippingInfo();
      this.ClearShippingInfo();
    }

    private void AutoPopulateShippingInfo()
    {
      if (!(this.dpShippingDt.Value != DateTime.MinValue))
        return;
      if (((IEnumerable<string>) this.carrierList).Count<string>() > 0)
        this.cboxCarrier.SelectedText = this.carrierList[0];
      string prefix = DocTrackingUtils.GetPrefix(this.docTrackingType, DocTrackingRequestType.ShippingStatus);
      if (!string.IsNullOrEmpty(this.loanData.GetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtOrg))))
      {
        this.txtOrg.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtOrg));
        this.txtContact.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtContact));
        this.txtAddr.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtAddr));
        this.txtEmail.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtEmail));
        this.txtPhone.Text = this.loanData.GetField(DocTrackingUtils.GetFieldId(prefix, (Control) this.txtPhone));
      }
      else if (!string.IsNullOrEmpty(this.loanData.GetField("VEND.X387")))
      {
        this.txtOrg.Text = this.loanData.GetField("VEND.X387");
        this.txtContact.Text = this.loanData.GetField("VEND.X392");
        this.txtAddr.Text = DocTrackingUtils.BuildAddress(new string[4]
        {
          this.loanData.GetField("VEND.X388"),
          this.loanData.GetField("VEND.X389"),
          this.loanData.GetField("VEND.X390"),
          this.loanData.GetField("VEND.X391")
        });
        this.txtEmail.Text = this.loanData.GetField("VEND.X394");
        this.txtPhone.Text = this.loanData.GetField("VEND.X393");
      }
      else if (!string.IsNullOrEmpty(this.loanData.GetField("VEND.X263")))
      {
        this.txtOrg.Text = this.loanData.GetField("VEND.X263");
        this.txtContact.Text = this.loanData.GetField("VEND.X271");
        this.txtAddr.Text = DocTrackingUtils.BuildAddress(new string[4]
        {
          this.loanData.GetField("VEND.X264"),
          this.loanData.GetField("VEND.X265"),
          this.loanData.GetField("VEND.X266"),
          this.loanData.GetField("VEND.X267")
        });
        this.txtEmail.Text = this.loanData.GetField("VEND.X273");
        this.txtPhone.Text = this.loanData.GetField("VEND.X272");
      }
      else
      {
        this.txtOrg.Text = "";
        this.txtContact.Text = "";
        this.txtAddr.Text = "";
        this.txtEmail.Text = "";
        this.txtPhone.Text = "";
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (!this.validateData())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, DocTrackingUtils.Validate_Error_message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string val = "";
        foreach (CheckBox control in (ArrangedElementCollection) this.pnlDocType.Controls)
        {
          if (control.Checked)
          {
            string str = control.Tag.ToString();
            DocTrackingType docTrackingType = (DocTrackingType) Enum.Parse(typeof (DocTrackingType), str, true);
            string prefix1 = DocTrackingUtils.GetPrefix(docTrackingType, DocTrackingRequestType.InitialRequest);
            this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix1, (Control) this.dpReceivedDt), this.dpReceivedDt.Text);
            this.loanData.SetField(string.Format(DocTrackingUtils.FieldPrefix + "{0}.InitialRequest.NextFollowUpDate", (object) str), "");
            if (str == DocTrackingType.DOT.ToString())
            {
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix1, (Control) this.txtDocumentNumber), this.txtDocumentNumber.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix1, (Control) this.txtBookNumber), this.txtBookNumber.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix1, (Control) this.txtPageNumber), this.txtPageNumber.Text);
              if (this.rbYes.Checked)
                val = this.rbYes.Text;
              if (this.rbNo.Checked)
                val = this.rbNo.Text;
              if (this.rbNa.Checked)
                val = this.rbNa.Text;
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix1, (Control) this.rbYes), val);
            }
            if (this.isSchedShipping)
            {
              string prefix2 = DocTrackingUtils.GetPrefix(docTrackingType, DocTrackingRequestType.ShippingStatus);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.dpShippingDt), this.dpShippingDt.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.txtReceivedBy), this.txtReceivedBy.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.cboxCarrier), this.cboxCarrier.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.txtTrackNum), this.txtTrackNum.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.txtOrg), this.txtOrg.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.txtContact), this.txtContact.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.txtAddr), this.txtAddr.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.txtEmail), this.txtEmail.Text);
              this.loanData.SetField(DocTrackingUtils.GetFieldId(prefix2, (Control) this.txtPhone), this.txtPhone.Text);
            }
          }
        }
        DocumentTrackingLog rec1 = new DocumentTrackingLog(DateTime.Now)
        {
          ActionCd = this.isSchedShipping ? DocTrackingActionCd.ActionInitialReceivedShipment : DocTrackingActionCd.ActionInitialReceived,
          Action = this.isSchedShipping ? DocTrackingUtils.Action_Initial_ReceivedShipment : DocTrackingUtils.Action_Initial_Received,
          LogDate = this.dpReceivedDt.Text,
          LogBy = this.txtReceivedBy.Text,
          Dot = this.ckDot.Checked,
          Ftp = this.ckFTP.Checked
        };
        rec1.DocTrackingSnapshot = new Hashtable()
        {
          {
            (object) "ReceivedDate",
            (object) this.dpReceivedDt.Text
          },
          {
            (object) "DocumentNumber",
            (object) this.txtDocumentNumber.Text
          },
          {
            (object) "BookNumber",
            (object) this.txtBookNumber.Text
          },
          {
            (object) "PageNumber",
            (object) this.txtPageNumber.Text
          },
          {
            (object) "RidersRecvd",
            (object) val
          },
          {
            (object) "ShippingDate",
            (object) this.dpShippingDt.Text
          },
          {
            (object) "Carrier",
            (object) this.cboxCarrier.Text
          },
          {
            (object) "TrackingNumber",
            (object) this.txtTrackNum.Text
          },
          {
            (object) "Address",
            (object) this.txtAddr.Text
          },
          {
            (object) "Organization",
            (object) this.txtOrg.Text
          },
          {
            (object) "Contact",
            (object) this.txtContact.Text
          },
          {
            (object) "Email",
            (object) this.txtEmail.Text
          },
          {
            (object) "Phone",
            (object) this.txtPhone.Text
          },
          {
            (object) "Comments",
            (object) this.rtComments.Text
          }
        };
        this.loanData.GetLogList().AddRecord((LogRecordBase) rec1);
        if (this.dpShippingDt.Value != DateTime.MinValue)
        {
          DocumentTrackingLog rec2 = new DocumentTrackingLog(DateTime.Now)
          {
            ActionCd = DocTrackingActionCd.ActionSchedShipment,
            Action = DocTrackingUtils.Action_Sched_Shipment,
            LogDate = this.dpShippingDt.Text,
            LogBy = this.txtReceivedBy.Text,
            Dot = this.ckDot.Checked,
            Ftp = this.ckFTP.Checked,
            Organization = this.txtOrg.Text,
            Contact = this.txtContact.Text
          };
          rec2.DocTrackingSnapshot = new Hashtable()
          {
            {
              (object) "ReceivedDate",
              (object) this.dpReceivedDt.Text
            },
            {
              (object) "DocumentNumber",
              (object) this.txtDocumentNumber.Text
            },
            {
              (object) "BookNumber",
              (object) this.txtBookNumber.Text
            },
            {
              (object) "PageNumber",
              (object) this.txtPageNumber.Text
            },
            {
              (object) "RidersRecvd",
              (object) val
            },
            {
              (object) "ShippingDate",
              (object) this.dpShippingDt.Text
            },
            {
              (object) "Carrier",
              (object) this.cboxCarrier.Text
            },
            {
              (object) "TrackingNumber",
              (object) this.txtTrackNum.Text
            },
            {
              (object) "Address",
              (object) this.txtAddr.Text
            },
            {
              (object) "Organization",
              (object) this.txtOrg.Text
            },
            {
              (object) "Contact",
              (object) this.txtContact.Text
            },
            {
              (object) "Email",
              (object) this.txtEmail.Text
            },
            {
              (object) "Phone",
              (object) this.txtPhone.Text
            },
            {
              (object) "Comments",
              (object) this.rtComments.Text
            }
          };
          this.loanData.GetLogList().AddRecord((LogRecordBase) rec2);
        }
        if (!string.IsNullOrEmpty(this.rtComments.Text))
          DocTrackingUtils.SaveComments(new DocumentTrackingComment()
          {
            UserName = this.session.UserInfo.FullName,
            LogDate = DateTime.Now,
            CommentText = this.rtComments.Text
          });
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool validateData()
    {
      bool flag = true;
      if (!this.ckDot.Checked && !this.ckFTP.Checked)
      {
        DocTrackingUtils.HilightFields((Control) this.ckDot, true);
        DocTrackingUtils.HilightFields((Control) this.ckFTP, true);
        flag = false;
      }
      else
      {
        DocTrackingUtils.HilightFields((Control) this.ckDot, false);
        DocTrackingUtils.HilightFields((Control) this.ckFTP, false);
      }
      DocTrackingUtils.HilightFields((Control) this.dpReceivedDt, false);
      if (string.IsNullOrEmpty(this.dpReceivedDt.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.dpReceivedDt, true);
        flag = false;
      }
      this.dpReceivedDt.Refresh();
      DocTrackingUtils.HilightFields((Control) this.txtReceivedBy, false);
      if (string.IsNullOrEmpty(this.txtReceivedBy.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtReceivedBy, true);
        flag = false;
      }
      DocTrackingUtils.HilightFields((Control) this.dpShippingDt, false);
      if (this.dpShippingDt.Value != DateTime.MinValue && string.IsNullOrEmpty(this.dpShippingDt.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.dpShippingDt, true);
        flag = false;
      }
      this.dpShippingDt.Refresh();
      DocTrackingUtils.HilightFields((Control) this.cboxCarrier, false);
      if (this.dpShippingDt.Value != DateTime.MinValue && string.IsNullOrEmpty(this.cboxCarrier.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.cboxCarrier, true);
        flag = false;
      }
      DocTrackingUtils.HilightFields((Control) this.txtTrackNum, false);
      if (this.dpShippingDt.Value != DateTime.MinValue && string.IsNullOrEmpty(this.txtTrackNum.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtTrackNum, true);
        flag = false;
      }
      DocTrackingUtils.HilightFields((Control) this.txtOrg, false);
      if (this.dpShippingDt.Value != DateTime.MinValue && string.IsNullOrEmpty(this.txtOrg.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtOrg, true);
        flag = false;
      }
      DocTrackingUtils.HilightFields((Control) this.txtContact, false);
      if (this.dpShippingDt.Value != DateTime.MinValue && string.IsNullOrEmpty(this.txtContact.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtContact, true);
        flag = false;
      }
      DocTrackingUtils.HilightFields((Control) this.txtAddr, false);
      if (this.dpShippingDt.Value != DateTime.MinValue && string.IsNullOrEmpty(this.txtAddr.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtAddr, true);
        flag = false;
      }
      DocTrackingUtils.HilightFields((Control) this.txtEmail, false);
      if (this.dpShippingDt.Value != DateTime.MinValue && !string.IsNullOrEmpty(this.txtEmail.Text.Trim()) && !Utils.ValidateEmail(this.txtEmail.Text.Trim()))
      {
        DocTrackingUtils.HilightFields((Control) this.txtEmail, true);
        flag = false;
      }
      return flag;
    }

    private void ToggleShippingInfo()
    {
      if (this.isSchedShipping)
      {
        this.pnlShipping.Visible = true;
        if (this.ckDot.Checked)
        {
          this.pnlDocNumber.Visible = true;
          this.Height = 729;
          this.pnlShipping.Location = new Point(24, 264);
          this.pnlComments.Location = new Point(24, 512);
        }
        else
        {
          this.pnlDocNumber.Visible = false;
          this.Height = 627;
          this.pnlShipping.Location = new Point(24, 155);
          this.pnlComments.Location = new Point(24, 402);
        }
      }
      else
      {
        this.pnlShipping.Visible = false;
        if (this.ckDot.Checked)
        {
          this.pnlDocNumber.Visible = true;
          this.Height = 487;
          this.pnlComments.Location = new Point(24, 264);
        }
        else
        {
          this.pnlDocNumber.Visible = false;
          this.Height = 377;
          this.pnlComments.Location = new Point(24, 155);
        }
      }
    }

    private void EnableDisableAddInitialReceipt(bool isEnable)
    {
      this.ckDot.Enabled = isEnable;
      this.ckFTP.Enabled = isEnable;
      this.dpReceivedDt.Enabled = isEnable;
      this.txtReceivedBy.Enabled = isEnable;
      this.txtDocumentNumber.Enabled = isEnable;
      this.txtBookNumber.Enabled = isEnable;
      this.txtPageNumber.Enabled = isEnable;
      this.rbYes.Enabled = isEnable;
      this.rbNo.Enabled = isEnable;
      this.rbNa.Enabled = isEnable;
      this.rtComments.Enabled = isEnable;
      if (!this.isSchedShipping)
        return;
      this.dpShippingDt.Enabled = isEnable;
      this.cboxCarrier.Enabled = isEnable;
      this.txtTrackNum.Enabled = isEnable;
      this.txtOrg.Enabled = isEnable;
      this.txtContact.Enabled = isEnable;
      this.txtAddr.Enabled = isEnable;
      this.txtEmail.Enabled = isEnable;
      this.txtPhone.Enabled = isEnable;
      this.btnOrg.Enabled = isEnable;
    }

    private void InitDataFromLog(DocumentTrackingLog dtl)
    {
      this.isSchedShipping = dtl.ActionCd == DocTrackingActionCd.ActionSchedShipment || dtl.ActionCd == DocTrackingActionCd.ActionInitialReceivedShipment;
      this.ckDot.Checked = dtl.Dot;
      this.ckFTP.Checked = dtl.Ftp;
      this.ToggleShippingInfo();
      this.dpReceivedDt.Value = Utils.ParseDate((object) dtl.LogDate);
      this.txtReceivedBy.Text = dtl.LogBy;
      Hashtable trackingSnapshot = dtl.DocTrackingSnapshot;
      if (trackingSnapshot == null)
        return;
      this.dpReceivedDt.Text = (string) trackingSnapshot[(object) "ReceivedDate"];
      this.txtDocumentNumber.Text = (string) trackingSnapshot[(object) "DocumentNumber"];
      this.txtBookNumber.Text = (string) trackingSnapshot[(object) "BookNumber"];
      this.txtPageNumber.Text = (string) trackingSnapshot[(object) "PageNumber"];
      switch ((string) trackingSnapshot[(object) "RidersRecvd"])
      {
        case "Yes":
          this.rbYes.Checked = true;
          break;
        case "No":
          this.rbNo.Checked = true;
          break;
        case "N/A":
          this.rbNa.Checked = true;
          break;
      }
      this.dpShippingDt.Text = (string) trackingSnapshot[(object) "ShippingDate"];
      this.cboxCarrier.Text = (string) trackingSnapshot[(object) "Carrier"];
      this.txtTrackNum.Text = (string) trackingSnapshot[(object) "TrackingNumber"];
      this.txtOrg.Text = (string) trackingSnapshot[(object) "Organization"];
      this.txtContact.Text = (string) trackingSnapshot[(object) "Contact"];
      this.txtAddr.Text = (string) trackingSnapshot[(object) "Address"];
      this.txtEmail.Text = (string) trackingSnapshot[(object) "Email"];
      this.txtPhone.Text = (string) trackingSnapshot[(object) "Phone"];
      this.rtComments.Text = (string) trackingSnapshot[(object) "Comments"];
    }

    private void ckDot_Click(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox) sender;
      string key = checkBox.Tag.ToString();
      if (key != this.docTrackingType.ToString() && checkBox.Checked && !Utils.ParseBoolean(this.docTypeClearReceivedDt[(object) key]) && Utils.ParseDate((object) this.loanData.GetField(string.Format(DocTrackingUtils.FieldPrefix + "{0}.InitialRequest.Received", (object) key.ToString()))) != DateTime.MinValue)
      {
        if (Utils.Dialog((IWin32Window) this, DocTrackingUtils.InitialRequest_AddReceipt, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
          this.docTypeClearReceivedDt[(object) key] = (object) true;
        else
          checkBox.Checked = false;
      }
      this.ToggleShippingInfo();
    }

    private void InitCarrierList()
    {
      this.carrierList = (string[]) Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.PreferredCarrier).ToArray(typeof (string));
      this.cboxCarrier.Items.AddRange((object[]) this.carrierList);
    }

    private void dpShippingDt_ValueChanged(object sender, EventArgs e)
    {
      DatePickerCustom datePickerCustom = sender as DatePickerCustom;
      if ((!(datePickerCustom.PriorValue == DateTime.MinValue) || !(datePickerCustom.Value != DateTime.MinValue)) && (!(datePickerCustom.PriorValue != DateTime.MinValue) || !(datePickerCustom.Value == DateTime.MinValue)))
        return;
      this.ClearShippingInfo();
      this.AutoPopulateShippingInfo();
    }

    private void ClearShippingInfo()
    {
      if (this.dpShippingDt.Value == DateTime.MinValue)
      {
        this.cboxCarrier.Text = string.Empty;
        this.txtTrackNum.Text = string.Empty;
        this.txtOrg.Text = string.Empty;
        this.txtContact.Text = string.Empty;
        this.txtAddr.Text = string.Empty;
        this.txtEmail.Text = string.Empty;
        this.txtPhone.Text = string.Empty;
        DocTrackingUtils.HilightFields((Control) this.cboxCarrier, false);
        DocTrackingUtils.HilightFields((Control) this.txtTrackNum, false);
        DocTrackingUtils.HilightFields((Control) this.txtOrg, false);
        DocTrackingUtils.HilightFields((Control) this.txtContact, false);
        DocTrackingUtils.HilightFields((Control) this.txtAddr, false);
        DocTrackingUtils.HilightFields((Control) this.txtEmail, false);
        DocTrackingUtils.HilightFields((Control) this.txtPhone, false);
        this.EnableDisableShipping(false);
      }
      else
        this.EnableDisableShipping(true);
    }

    private void EnableDisableShipping(bool isEnable)
    {
      this.cboxCarrier.Enabled = isEnable;
      this.txtTrackNum.Enabled = isEnable;
      this.txtOrg.Enabled = isEnable;
      this.txtContact.Enabled = isEnable;
      this.txtAddr.Enabled = isEnable;
      this.txtEmail.Enabled = isEnable;
      this.txtPhone.Enabled = isEnable;
      this.btnOrg.Enabled = isEnable;
    }

    private void btnOrg_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      rxContact.CompanyName = this.txtOrg.Text.Trim();
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("", rxContact.CompanyName, "", rxContact, true, true, CRMRoleType.NotFound, true, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
        {
          this.session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        }
        else
        {
          RxContactInfo rxContactRecord = rxBusinessContact.RxContactRecord;
          this.txtOrg.Text = rxContactRecord[RolodexField.Company];
          this.txtContact.Text = rxContactRecord[RolodexField.Name];
          this.txtAddr.Text = rxContactRecord.BizAddress1 + (string.IsNullOrEmpty(rxContactRecord.BizAddress2) ? "" : ", " + rxContactRecord.BizAddress2) + (string.IsNullOrEmpty(rxContactRecord.BizCity) ? "" : ", " + rxContactRecord.BizCity) + (string.IsNullOrEmpty(rxContactRecord.BizState) ? "" : ", " + rxContactRecord.BizState) + (string.IsNullOrEmpty(rxContactRecord.BizZip) ? "" : ", " + rxContactRecord.BizZip);
          this.txtPhone.Text = rxContactRecord[RolodexField.Phone];
          this.txtEmail.Text = rxContactRecord[RolodexField.Email];
        }
      }
    }

    private void txtPhone_TextChanged(object sender, EventArgs e)
    {
      DocTrackingUtils.ValidatePhone((TextBox) sender, (Control) this);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtReceivedBy = new TextBox();
      this.label7 = new Label();
      this.label4 = new Label();
      this.label1 = new Label();
      this.pnlDocType = new Panel();
      this.ckFTP = new CheckBox();
      this.ckDot = new CheckBox();
      this.pnlComments = new Panel();
      this.rtComments = new RichTextBox();
      this.label3 = new Label();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.pnlShipping = new Panel();
      this.label14 = new Label();
      this.dpShippingDt = new DatePickerCustom();
      this.btnOrg = new StandardIconButton();
      this.cboxCarrier = new ComboBox();
      this.txtPhone = new TextBox();
      this.txtEmail = new TextBox();
      this.txtAddr = new TextBox();
      this.txtContact = new TextBox();
      this.txtOrg = new TextBox();
      this.txtTrackNum = new TextBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label2 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.label13 = new Label();
      this.lblDocumentNum = new Label();
      this.txtDocumentNumber = new TextBox();
      this.lblBookNum = new Label();
      this.txtBookNumber = new TextBox();
      this.lblPageNum = new Label();
      this.txtPageNumber = new TextBox();
      this.lblRidersRec = new Label();
      this.rbYes = new RadioButton();
      this.rbNa = new RadioButton();
      this.rbNo = new RadioButton();
      this.pnlDocNumber = new Panel();
      this.dpReceivedDt = new DatePickerCustom();
      this.pnlDocType.SuspendLayout();
      this.pnlComments.SuspendLayout();
      this.pnlShipping.SuspendLayout();
      ((ISupportInitialize) this.btnOrg).BeginInit();
      this.pnlDocNumber.SuspendLayout();
      this.SuspendLayout();
      this.txtReceivedBy.Location = new Point(120, 125);
      this.txtReceivedBy.Name = "txtReceivedBy";
      this.txtReceivedBy.Size = new Size(253, 20);
      this.txtReceivedBy.TabIndex = 4;
      this.txtReceivedBy.Tag = (object) "ShippedBy";
      this.label7.AutoSize = true;
      this.label7.BackColor = Color.Transparent;
      this.label7.Location = new Point(29, (int) sbyte.MaxValue);
      this.label7.Name = "label7";
      this.label7.Size = new Size(67, 13);
      this.label7.TabIndex = 40;
      this.label7.Text = "Received by";
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(29, 101);
      this.label4.Name = "label4";
      this.label4.Size = new Size(79, 13);
      this.label4.TabIndex = 38;
      this.label4.Text = "Received Date";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(29, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(146, 13);
      this.label1.TabIndex = 34;
      this.label1.Text = "Select the items you received";
      this.pnlDocType.Controls.Add((Control) this.ckFTP);
      this.pnlDocType.Controls.Add((Control) this.ckDot);
      this.pnlDocType.Location = new Point(32, 37);
      this.pnlDocType.Name = "pnlDocType";
      this.pnlDocType.Size = new Size(200, 56);
      this.pnlDocType.TabIndex = 55;
      this.ckFTP.AutoSize = true;
      this.ckFTP.Location = new Point(3, 26);
      this.ckFTP.Name = "ckFTP";
      this.ckFTP.Size = new Size(102, 17);
      this.ckFTP.TabIndex = 2;
      this.ckFTP.Tag = (object) "FTP";
      this.ckFTP.Text = "Final Title Policy";
      this.ckFTP.UseVisualStyleBackColor = true;
      this.ckFTP.Click += new EventHandler(this.ckDot_Click);
      this.ckDot.AutoSize = true;
      this.ckDot.Location = new Point(3, 3);
      this.ckDot.Name = "ckDot";
      this.ckDot.Size = new Size(99, 17);
      this.ckDot.TabIndex = 1;
      this.ckDot.Tag = (object) "DOT";
      this.ckDot.Text = "DOT/Mortgage";
      this.ckDot.UseVisualStyleBackColor = true;
      this.ckDot.Click += new EventHandler(this.ckDot_Click);
      this.pnlComments.Controls.Add((Control) this.rtComments);
      this.pnlComments.Controls.Add((Control) this.label3);
      this.pnlComments.Controls.Add((Control) this.btnCancel);
      this.pnlComments.Controls.Add((Control) this.btnOk);
      this.pnlComments.Location = new Point(24, 512);
      this.pnlComments.Name = "pnlComments";
      this.pnlComments.Size = new Size(380, 168);
      this.pnlComments.TabIndex = 57;
      this.rtComments.Location = new Point(8, 25);
      this.rtComments.MaxLength = 500;
      this.rtComments.Name = "rtComments";
      this.rtComments.Size = new Size(341, 95);
      this.rtComments.TabIndex = 19;
      this.rtComments.Text = "";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(56, 13);
      this.label3.TabIndex = 56;
      this.label3.Text = "Comments";
      this.btnCancel.Location = new Point(288, 138);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 21;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOk.Location = new Point(207, 138);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 20;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.pnlShipping.Controls.Add((Control) this.label14);
      this.pnlShipping.Controls.Add((Control) this.dpShippingDt);
      this.pnlShipping.Controls.Add((Control) this.btnOrg);
      this.pnlShipping.Controls.Add((Control) this.cboxCarrier);
      this.pnlShipping.Controls.Add((Control) this.txtPhone);
      this.pnlShipping.Controls.Add((Control) this.txtEmail);
      this.pnlShipping.Controls.Add((Control) this.txtAddr);
      this.pnlShipping.Controls.Add((Control) this.txtContact);
      this.pnlShipping.Controls.Add((Control) this.txtOrg);
      this.pnlShipping.Controls.Add((Control) this.txtTrackNum);
      this.pnlShipping.Controls.Add((Control) this.label9);
      this.pnlShipping.Controls.Add((Control) this.label8);
      this.pnlShipping.Controls.Add((Control) this.label2);
      this.pnlShipping.Controls.Add((Control) this.label6);
      this.pnlShipping.Controls.Add((Control) this.label5);
      this.pnlShipping.Controls.Add((Control) this.label10);
      this.pnlShipping.Controls.Add((Control) this.label11);
      this.pnlShipping.Controls.Add((Control) this.label13);
      this.pnlShipping.Location = new Point(24, 264);
      this.pnlShipping.Name = "pnlShipping";
      this.pnlShipping.Size = new Size(380, 242);
      this.pnlShipping.TabIndex = 58;
      this.label14.AutoSize = true;
      this.label14.Location = new Point(5, 11);
      this.label14.Name = "label14";
      this.label14.Size = new Size(182, 13);
      this.label14.TabIndex = 71;
      this.label14.Text = "Scheduled Shipment Setup (optional)";
      this.dpShippingDt.Location = new Point(96, 38);
      this.dpShippingDt.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpShippingDt.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpShippingDt.Name = "dpShippingDt";
      this.dpShippingDt.Size = new Size(135, 21);
      this.dpShippingDt.TabIndex = 11;
      this.dpShippingDt.Tag = (object) "ShippingDate";
      this.dpShippingDt.ToolTip = "";
      this.dpShippingDt.Value = new DateTime(0L);
      this.dpShippingDt.ValueChanged += new EventHandler(this.dpShippingDt_ValueChanged);
      this.btnOrg.BackColor = Color.Transparent;
      this.btnOrg.Location = new Point(356, 117);
      this.btnOrg.MouseDownImage = (Image) null;
      this.btnOrg.Name = "btnOrg";
      this.btnOrg.Size = new Size(16, 16);
      this.btnOrg.StandardButtonType = StandardIconButton.ButtonType.RolodexButton;
      this.btnOrg.TabIndex = 69;
      this.btnOrg.TabStop = false;
      this.btnOrg.Click += new EventHandler(this.btnOrg_Click);
      this.cboxCarrier.BackColor = Color.White;
      this.cboxCarrier.FormattingEnabled = true;
      this.cboxCarrier.Location = new Point(96, 64);
      this.cboxCarrier.Name = "cboxCarrier";
      this.cboxCarrier.Size = new Size(135, 21);
      this.cboxCarrier.TabIndex = 12;
      this.cboxCarrier.Tag = (object) "Carrier";
      this.txtPhone.Location = new Point(96, 214);
      this.txtPhone.Name = "txtPhone";
      this.txtPhone.Size = new Size(253, 20);
      this.txtPhone.TabIndex = 18;
      this.txtPhone.Tag = (object) "Phone";
      this.txtPhone.TextChanged += new EventHandler(this.txtPhone_TextChanged);
      this.txtEmail.Location = new Point(96, 189);
      this.txtEmail.Name = "txtEmail";
      this.txtEmail.Size = new Size(253, 20);
      this.txtEmail.TabIndex = 17;
      this.txtEmail.Tag = (object) "Email";
      this.txtAddr.Location = new Point(96, 164);
      this.txtAddr.Name = "txtAddr";
      this.txtAddr.Size = new Size(253, 20);
      this.txtAddr.TabIndex = 16;
      this.txtAddr.Tag = (object) "Address";
      this.txtContact.Location = new Point(96, 139);
      this.txtContact.Name = "txtContact";
      this.txtContact.Size = new Size(253, 20);
      this.txtContact.TabIndex = 15;
      this.txtContact.Tag = (object) "Contact";
      this.txtOrg.Location = new Point(96, 114);
      this.txtOrg.Name = "txtOrg";
      this.txtOrg.Size = new Size(253, 20);
      this.txtOrg.TabIndex = 14;
      this.txtOrg.Tag = (object) "Organization";
      this.txtTrackNum.Location = new Point(96, 89);
      this.txtTrackNum.Name = "txtTrackNum";
      this.txtTrackNum.Size = new Size(253, 20);
      this.txtTrackNum.TabIndex = 13;
      this.txtTrackNum.Tag = (object) "TrackingNumber";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(5, 217);
      this.label9.Name = "label9";
      this.label9.Size = new Size(38, 13);
      this.label9.TabIndex = 60;
      this.label9.Text = "Phone";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(5, 192);
      this.label8.Name = "label8";
      this.label8.Size = new Size(32, 13);
      this.label8.TabIndex = 59;
      this.label8.Text = "Email";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(5, 167);
      this.label2.Name = "label2";
      this.label2.Size = new Size(45, 13);
      this.label2.TabIndex = 58;
      this.label2.Text = "Address";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(5, 142);
      this.label6.Name = "label6";
      this.label6.Size = new Size(44, 13);
      this.label6.TabIndex = 57;
      this.label6.Text = "Contact";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(5, 117);
      this.label5.Name = "label5";
      this.label5.Size = new Size(66, 13);
      this.label5.TabIndex = 56;
      this.label5.Text = "Organization";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(5, 92);
      this.label10.Name = "label10";
      this.label10.Size = new Size(59, 13);
      this.label10.TabIndex = 55;
      this.label10.Text = "Tracking #";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(5, 67);
      this.label11.Name = "label11";
      this.label11.Size = new Size(37, 13);
      this.label11.TabIndex = 54;
      this.label11.Text = "Carrier";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(5, 41);
      this.label13.Name = "label13";
      this.label13.Size = new Size(74, 13);
      this.label13.TabIndex = 52;
      this.label13.Text = "Shipping Date";
      this.lblDocumentNum.AutoSize = true;
      this.lblDocumentNum.BackColor = Color.Transparent;
      this.lblDocumentNum.ForeColor = SystemColors.ControlText;
      this.lblDocumentNum.Location = new Point(5, 9);
      this.lblDocumentNum.Name = "lblDocumentNum";
      this.lblDocumentNum.Size = new Size(66, 13);
      this.lblDocumentNum.TabIndex = 66;
      this.lblDocumentNum.Text = "Document #";
      this.txtDocumentNumber.Location = new Point(96, 6);
      this.txtDocumentNumber.Name = "txtDocumentNumber";
      this.txtDocumentNumber.Size = new Size(253, 20);
      this.txtDocumentNumber.TabIndex = 5;
      this.txtDocumentNumber.Tag = (object) "DocumentNumber";
      this.lblBookNum.AutoSize = true;
      this.lblBookNum.BackColor = Color.Transparent;
      this.lblBookNum.ForeColor = SystemColors.ControlText;
      this.lblBookNum.Location = new Point(5, 35);
      this.lblBookNum.Name = "lblBookNum";
      this.lblBookNum.Size = new Size(42, 13);
      this.lblBookNum.TabIndex = 68;
      this.lblBookNum.Text = "Book #";
      this.txtBookNumber.Location = new Point(96, 32);
      this.txtBookNumber.Name = "txtBookNumber";
      this.txtBookNumber.Size = new Size(253, 20);
      this.txtBookNumber.TabIndex = 6;
      this.txtBookNumber.Tag = (object) "BookNumber";
      this.lblPageNum.AutoSize = true;
      this.lblPageNum.BackColor = Color.Transparent;
      this.lblPageNum.ForeColor = SystemColors.ControlText;
      this.lblPageNum.Location = new Point(5, 61);
      this.lblPageNum.Name = "lblPageNum";
      this.lblPageNum.Size = new Size(42, 13);
      this.lblPageNum.TabIndex = 70;
      this.lblPageNum.Text = "Page #";
      this.txtPageNumber.Location = new Point(96, 58);
      this.txtPageNumber.Name = "txtPageNumber";
      this.txtPageNumber.Size = new Size(253, 20);
      this.txtPageNumber.TabIndex = 7;
      this.txtPageNumber.Tag = (object) "PageNumber";
      this.lblRidersRec.AutoSize = true;
      this.lblRidersRec.BackColor = Color.Transparent;
      this.lblRidersRec.ForeColor = SystemColors.ControlText;
      this.lblRidersRec.Location = new Point(5, 88);
      this.lblRidersRec.Name = "lblRidersRec";
      this.lblRidersRec.Size = new Size(74, 13);
      this.lblRidersRec.TabIndex = 72;
      this.lblRidersRec.Text = "Riders Rec'vd";
      this.rbYes.AutoSize = true;
      this.rbYes.BackColor = Color.Transparent;
      this.rbYes.ForeColor = SystemColors.ControlText;
      this.rbYes.Location = new Point(96, 86);
      this.rbYes.Name = "rbYes";
      this.rbYes.Size = new Size(43, 17);
      this.rbYes.TabIndex = 8;
      this.rbYes.TabStop = true;
      this.rbYes.Tag = (object) "RiderReceived";
      this.rbYes.Text = "Yes";
      this.rbYes.UseVisualStyleBackColor = false;
      this.rbNa.AutoSize = true;
      this.rbNa.BackColor = Color.Transparent;
      this.rbNa.ForeColor = SystemColors.ControlText;
      this.rbNa.Location = new Point(186, 87);
      this.rbNa.Name = "rbNa";
      this.rbNa.Size = new Size(45, 17);
      this.rbNa.TabIndex = 10;
      this.rbNa.TabStop = true;
      this.rbNa.Tag = (object) "RiderReceived";
      this.rbNa.Text = "N/A";
      this.rbNa.UseVisualStyleBackColor = false;
      this.rbNo.AutoSize = true;
      this.rbNo.BackColor = Color.Transparent;
      this.rbNo.ForeColor = SystemColors.ControlText;
      this.rbNo.Location = new Point(143, 87);
      this.rbNo.Name = "rbNo";
      this.rbNo.Size = new Size(39, 17);
      this.rbNo.TabIndex = 9;
      this.rbNo.TabStop = true;
      this.rbNo.Tag = (object) "RiderReceived";
      this.rbNo.Text = "No";
      this.rbNo.UseVisualStyleBackColor = false;
      this.pnlDocNumber.Controls.Add((Control) this.rbNo);
      this.pnlDocNumber.Controls.Add((Control) this.rbNa);
      this.pnlDocNumber.Controls.Add((Control) this.rbYes);
      this.pnlDocNumber.Controls.Add((Control) this.lblRidersRec);
      this.pnlDocNumber.Controls.Add((Control) this.txtPageNumber);
      this.pnlDocNumber.Controls.Add((Control) this.lblPageNum);
      this.pnlDocNumber.Controls.Add((Control) this.txtBookNumber);
      this.pnlDocNumber.Controls.Add((Control) this.lblBookNum);
      this.pnlDocNumber.Controls.Add((Control) this.txtDocumentNumber);
      this.pnlDocNumber.Controls.Add((Control) this.lblDocumentNum);
      this.pnlDocNumber.Location = new Point(24, 143);
      this.pnlDocNumber.Name = "pnlDocNumber";
      this.pnlDocNumber.Size = new Size(380, 114);
      this.pnlDocNumber.TabIndex = 56;
      this.dpReceivedDt.Location = new Point(120, 99);
      this.dpReceivedDt.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpReceivedDt.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpReceivedDt.Name = "dpReceivedDt";
      this.dpReceivedDt.Size = new Size(135, 21);
      this.dpReceivedDt.TabIndex = 3;
      this.dpReceivedDt.Tag = (object) "Received";
      this.dpReceivedDt.ToolTip = "";
      this.dpReceivedDt.Value = new DateTime(0L);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(408, 691);
      this.Controls.Add((Control) this.pnlShipping);
      this.Controls.Add((Control) this.pnlComments);
      this.Controls.Add((Control) this.pnlDocNumber);
      this.Controls.Add((Control) this.pnlDocType);
      this.Controls.Add((Control) this.dpReceivedDt);
      this.Controls.Add((Control) this.txtReceivedBy);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddInitialReceipt);
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Initial Receipt";
      this.pnlDocType.ResumeLayout(false);
      this.pnlDocType.PerformLayout();
      this.pnlComments.ResumeLayout(false);
      this.pnlComments.PerformLayout();
      this.pnlShipping.ResumeLayout(false);
      this.pnlShipping.PerformLayout();
      ((ISupportInitialize) this.btnOrg).EndInit();
      this.pnlDocNumber.ResumeLayout(false);
      this.pnlDocNumber.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
