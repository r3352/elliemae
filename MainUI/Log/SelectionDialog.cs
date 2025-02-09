// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.SelectionDialog
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class SelectionDialog : Form
  {
    private ConversationDialog conversationDialog;
    private LoanData loanData;
    private DialogButtons dlgButtons;
    private Label labelContact;
    private ArrayList contacts;
    private RxContactInfo rxContact;
    private string contactType = string.Empty;
    private System.ComponentModel.Container components;
    private GridView gvContacts;

    public SelectionDialog(ConversationDialog conversationDialog, LoanData loanData)
    {
      this.conversationDialog = conversationDialog;
      this.loanData = loanData;
      this.InitializeComponent();
      this.loadContacts();
    }

    private void loadContacts()
    {
      this.contacts = new ArrayList();
      int nIndex = 0;
      string field1;
      if ((field1 = this.loanData.GetField("37")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          "(borrower)",
          this.loanData.GetField("36") + " " + field1
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "37");
        ++nIndex;
      }
      string field2;
      if ((field2 = this.loanData.GetField("69")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          "(co-borrower)",
          this.loanData.GetField("68") + " " + field2
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "69");
        ++nIndex;
      }
      LoanAssociateLog[] allLoanAssociates = this.loanData.GetLogList().GetAllLoanAssociates();
      for (int index = 0; index < allLoanAssociates.Length; ++index)
      {
        if (!(allLoanAssociates[index].LoanAssociateName == string.Empty))
        {
          this.gvContacts.Items.Insert(nIndex, new GVItem(new string[2]
          {
            "(" + allLoanAssociates[index].RoleName + ")",
            allLoanAssociates[index].LoanAssociateName
          })
          {
            Tag = (object) allLoanAssociates[index]
          });
          this.contacts.Add((object) "loan team member");
          ++nIndex;
        }
      }
      string field3;
      if ((field3 = this.loanData.GetField("1264")).Length != 0 || this.loanData.GetField("1256").Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field3,
          this.loanData.GetField("1256")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "1264");
        ++nIndex;
      }
      string field4;
      if ((field4 = this.loanData.GetField("617")).Length != 0 || this.loanData.GetField("618").Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field4,
          this.loanData.GetField("618")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "617");
        ++nIndex;
      }
      string field5;
      if ((field5 = this.loanData.GetField("610")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field5,
          this.loanData.GetField("611")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "610");
        ++nIndex;
      }
      string field6;
      if ((field6 = this.loanData.GetField("411")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field6,
          this.loanData.GetField("416")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "411");
        ++nIndex;
      }
      string field7;
      if ((field7 = this.loanData.GetField("56")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field7,
          this.loanData.GetField("VEND.X117")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "56");
        ++nIndex;
      }
      string field8;
      if ((field8 = this.loanData.GetField("VEND.X122")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field8,
          this.loanData.GetField("VEND.X128")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X122");
        ++nIndex;
      }
      string field9;
      if ((field9 = this.loanData.GetField("VEND.X133")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field9,
          this.loanData.GetField("VEND.X139")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X133");
        ++nIndex;
      }
      string field10;
      if ((field10 = this.loanData.GetField("VEND.X144")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field10,
          this.loanData.GetField("VEND.X150")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X144");
        ++nIndex;
      }
      string field11;
      if ((field11 = this.loanData.GetField("638")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          "(Seller 1)",
          field11
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "638");
        ++nIndex;
      }
      string field12;
      if ((field12 = this.loanData.GetField("VEND.X412")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          "(Seller 2)",
          field12
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X412");
        ++nIndex;
      }
      string field13;
      if ((field13 = this.loanData.GetField("VEND.X424")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field13,
          this.loanData.GetField("VEND.X429")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X424");
        ++nIndex;
      }
      string field14;
      if ((field14 = this.loanData.GetField("713")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field14,
          this.loanData.GetField("714")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "713");
        ++nIndex;
      }
      string field15;
      if ((field15 = this.loanData.GetField("L252")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field15,
          this.loanData.GetField("VEND.X162")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "L252");
        ++nIndex;
      }
      string field16;
      if ((field16 = this.loanData.GetField("L248")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field16,
          this.loanData.GetField("707")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "L248");
        ++nIndex;
      }
      string field17;
      if ((field17 = this.loanData.GetField("VEND.X34")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field17,
          this.loanData.GetField("VEND.X35")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X34");
        ++nIndex;
      }
      string field18;
      if ((field18 = this.loanData.GetField("1500")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field18,
          this.loanData.GetField("VEND.X13")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "1500");
        ++nIndex;
      }
      string field19;
      if ((field19 = this.loanData.GetField("624")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field19,
          this.loanData.GetField("625")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "624");
        ++nIndex;
      }
      string field20;
      if ((field20 = this.loanData.GetField("REGZGFE.X8")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field20,
          this.loanData.GetField("984")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "REGZGFE.X8");
        ++nIndex;
      }
      string field21;
      if ((field21 = this.loanData.GetField("VEND.X178")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field21,
          this.loanData.GetField("VEND.X184")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X178");
        ++nIndex;
      }
      string field22;
      if ((field22 = this.loanData.GetField("395")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field22,
          this.loanData.GetField("VEND.X195")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "395");
        ++nIndex;
      }
      string field23;
      if ((field23 = this.loanData.GetField("VEND.X200")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field23,
          this.loanData.GetField("VEND.X206")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X200");
        ++nIndex;
      }
      string field24;
      if ((field24 = this.loanData.GetField("VEND.X44")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field24,
          this.loanData.GetField("VEND.X45")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X44");
        ++nIndex;
      }
      string field25;
      if ((field25 = this.loanData.GetField("VEND.X263")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field25,
          this.loanData.GetField("VEND.X271")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X263");
        ++nIndex;
      }
      string field26;
      if ((field26 = this.loanData.GetField("VEND.X278")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field26,
          this.loanData.GetField("VEND.X286")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X278");
        ++nIndex;
      }
      string field27;
      if ((field27 = this.loanData.GetField("VEND.X293")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field27,
          this.loanData.GetField("VEND.X302")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X293");
        ++nIndex;
      }
      string field28;
      if ((field28 = this.loanData.GetField("VEND.X310")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field28,
          this.loanData.GetField("VEND.X317")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X310");
        ++nIndex;
      }
      string field29;
      if ((field29 = this.loanData.GetField("VEND.X84")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field29,
          this.loanData.GetField("VEND.X55")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X84");
        ++nIndex;
      }
      string field30;
      if ((field30 = this.loanData.GetField("VEND.X85")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field30,
          this.loanData.GetField("VEND.X65")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X85");
        ++nIndex;
      }
      string field31;
      if ((field31 = this.loanData.GetField("VEND.X86")).Length != 0)
      {
        GVItem gvItem = new GVItem(new string[2]
        {
          field31,
          this.loanData.GetField("VEND.X75")
        });
        this.gvContacts.Items.Insert(nIndex, gvItem);
        this.contacts.Add((object) "VEND.X86");
        ++nIndex;
      }
      string field32;
      if ((field32 = this.loanData.GetField("VEND.X11")).Length == 0)
        return;
      GVItem gvItem1 = new GVItem(new string[2]
      {
        field32,
        this.loanData.GetField("VEND.X2")
      });
      this.gvContacts.Items.Insert(nIndex, gvItem1);
      this.contacts.Add((object) "VEND.X11");
      int num = nIndex + 1;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.gvContacts.SelectedItems.Count == 0)
        return;
      string contact = (string) this.contacts[this.gvContacts.SelectedItems[0].Index];
      if (this.conversationDialog != null)
        this.populateConversationLog(contact);
      else
        this.populateContactInformation(contact);
      this.DialogResult = DialogResult.OK;
    }

    public RxContactInfo RxContact => this.rxContact;

    public string ContactType => this.contactType;

    private void populateContactInformation(string fieldId)
    {
      string empty = string.Empty;
      this.rxContact = new RxContactInfo();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(fieldId))
      {
        case 111074370:
          if (!(fieldId == "638"))
            break;
          string[] strArray1 = Utils.SplitName(this.loanData.GetField("638"));
          this.rxContact.FirstName = strArray1[0];
          this.rxContact.LastName = strArray1[1];
          this.rxContact.WorkPhone = (this.loanData.GetField("704") + "/" + this.loanData.GetField("VEND.X220")).Trim('/', ' ');
          this.rxContact.BizEmail = this.loanData.GetField("92");
          this.rxContact.BizAddress1 = this.loanData.GetField("701");
          this.rxContact.BizCity = this.loanData.GetField("702");
          this.rxContact.BizState = this.loanData.GetField("1249");
          this.rxContact.BizZip = this.loanData.GetField("703");
          this.contactType = "No Category";
          break;
        case 216585232:
          if (!(fieldId == "69"))
            break;
          this.rxContact.FirstName = this.loanData.GetField("4004") + " " + this.loanData.GetField("4005");
          this.rxContact.FirstName = this.rxContact.FirstName.Trim();
          this.rxContact.LastName = this.loanData.GetField("4006") + " " + this.loanData.GetField("4007");
          this.rxContact.LastName = this.rxContact.LastName.Trim();
          this.rxContact.WorkPhone = this.loanData.GetField("FE0217");
          this.rxContact.BizEmail = this.loanData.GetField("1179");
          this.rxContact.BizAddress1 = this.loanData.GetField("FR0204");
          this.rxContact.BizCity = this.loanData.GetField("FR0206");
          this.rxContact.BizState = this.loanData.GetField("FR0207");
          this.rxContact.BizZip = this.loanData.GetField("FR0208");
          this.contactType = "Borrower";
          break;
        case 595448055:
          if (!(fieldId == "VEND.X412"))
            break;
          string[] strArray2 = Utils.SplitName(this.loanData.GetField("VEND.X412"));
          this.rxContact.FirstName = strArray2[0];
          this.rxContact.LastName = strArray2[1];
          this.rxContact.WorkPhone = (this.loanData.GetField("VEND.X417") + "/" + this.loanData.GetField("VEND.X421")).Trim('/', ' ');
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X419");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X413");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X414");
          this.rxContact.BizState = this.loanData.GetField("VEND.X415");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X416");
          this.contactType = "No Category";
          break;
        case 908999587:
          if (!(fieldId == "REGZGFE.X8"))
            break;
          string[] strArray3 = Utils.SplitName(this.loanData.GetField("984"));
          this.rxContact.FirstName = strArray3[0];
          this.rxContact.LastName = strArray3[1];
          this.rxContact.WorkPhone = this.loanData.GetField("1410");
          this.rxContact.BizEmail = this.loanData.GetField("1411");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X171");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X172");
          this.rxContact.BizState = this.loanData.GetField("VEND.X173");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X174");
          this.contactType = "Underwriter";
          break;
        case 995269145:
          if (!(fieldId == "VEND.X278"))
            break;
          string[] strArray4 = Utils.SplitName(this.loanData.GetField("VEND.X286"));
          this.rxContact.FirstName = strArray4[0];
          this.rxContact.LastName = strArray4[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X287");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X288");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X279");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X280");
          this.rxContact.BizState = this.loanData.GetField("VEND.X281");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X282");
          this.contactType = "Organization";
          break;
        case 1079010145:
          if (!(fieldId == "VEND.X263"))
            break;
          string[] strArray5 = Utils.SplitName(this.loanData.GetField("VEND.X271"));
          this.rxContact.FirstName = strArray5[0];
          this.rxContact.LastName = strArray5[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X272");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X273");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X264");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X265");
          this.rxContact.BizState = this.loanData.GetField("VEND.X266");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X267");
          this.contactType = "Investor";
          break;
        case 1172536963:
          if (!(fieldId == "1500"))
            break;
          string[] strArray6 = Utils.SplitName(this.loanData.GetField("VEND.X13"));
          this.rxContact.FirstName = strArray6[0];
          this.rxContact.LastName = strArray6[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X19");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X21");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X14");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X15");
          this.rxContact.BizState = this.loanData.GetField("VEND.X16");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X17");
          this.contactType = "Flood Insurance";
          break;
        case 1422057211:
          if (!(fieldId == "VEND.X122"))
            break;
          string[] strArray7 = Utils.SplitName(this.loanData.GetField("VEND.X128"));
          this.rxContact.FirstName = strArray7[0];
          this.rxContact.LastName = strArray7[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X129");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X130");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X123");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X124");
          this.rxContact.BizState = this.loanData.GetField("VEND.X125");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X126");
          this.contactType = "Attorney";
          break;
        case 1561658572:
          if (!(fieldId == "VEND.X84"))
            break;
          string[] strArray8 = Utils.SplitName(this.loanData.GetField("VEND.X55"));
          this.rxContact.FirstName = strArray8[0];
          this.rxContact.LastName = strArray8[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X61");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X63");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X56");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X57");
          this.rxContact.BizState = this.loanData.GetField("VEND.X58");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X59");
          this.contactType = "No Category";
          break;
        case 1563630048:
          if (!(fieldId == "VEND.X44"))
            break;
          string[] strArray9 = Utils.SplitName(this.loanData.GetField("VEND.X45"));
          this.rxContact.FirstName = strArray9[0];
          this.rxContact.LastName = strArray9[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X51");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X53");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X46");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X47");
          this.rxContact.BizState = this.loanData.GetField("VEND.X48");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X49");
          this.contactType = "Organization";
          break;
        case 1578436191:
          if (!(fieldId == "VEND.X85"))
            break;
          string[] strArray10 = Utils.SplitName(this.loanData.GetField("VEND.X65"));
          this.rxContact.FirstName = strArray10[0];
          this.rxContact.LastName = strArray10[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X71");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X73");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X66");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X67");
          this.rxContact.BizState = this.loanData.GetField("VEND.X68");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X69");
          this.contactType = "No Category";
          break;
        case 1595213810:
          if (!(fieldId == "VEND.X86"))
            break;
          string[] strArray11 = Utils.SplitName(this.loanData.GetField("VEND.X75"));
          this.rxContact.FirstName = strArray11[0];
          this.rxContact.LastName = strArray11[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X81");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X83");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X76");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X77");
          this.rxContact.BizState = this.loanData.GetField("VEND.X78");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X79");
          this.contactType = "No Category";
          break;
        case 1597038191:
          if (!(fieldId == "VEND.X34"))
            break;
          string[] strArray12 = Utils.SplitName(this.loanData.GetField("VEND.X35"));
          this.rxContact.FirstName = strArray12[0];
          this.rxContact.LastName = strArray12[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X41");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X43");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X36");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X37");
          this.rxContact.BizState = this.loanData.GetField("VEND.X38");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X39");
          this.contactType = "Surveyor";
          break;
        case 1741391264:
          if (!(fieldId == "1264"))
            break;
          string[] strArray13 = Utils.SplitName(this.loanData.GetField("1256"));
          this.rxContact.FirstName = strArray13[0];
          this.rxContact.LastName = strArray13[1];
          this.rxContact.WorkPhone = this.loanData.GetField("1262");
          this.rxContact.BizEmail = this.loanData.GetField("95");
          this.rxContact.BizAddress1 = this.loanData.GetField("1257");
          this.rxContact.BizCity = this.loanData.GetField("1258");
          this.rxContact.BizState = this.loanData.GetField("1259");
          this.rxContact.BizZip = this.loanData.GetField("1260");
          this.rxContact.WebSite = this.loanData.GetField("VEND.X1034");
          this.contactType = "Lender";
          break;
        case 1835328466:
          if (!(fieldId == "loan team member"))
            break;
          LoanAssociateLog tag = (LoanAssociateLog) this.gvContacts.SelectedItems[0].Tag;
          string[] strArray14 = Utils.SplitName(tag.LoanAssociateName);
          this.rxContact.FirstName = strArray14[0];
          this.rxContact.LastName = strArray14[1];
          this.rxContact.WorkPhone = tag.LoanAssociatePhone;
          this.rxContact.BizEmail = tag.LoanAssociateEmail;
          this.rxContact.BizAddress1 = "";
          this.rxContact.BizCity = "";
          this.rxContact.BizState = "";
          this.rxContact.BizZip = "";
          this.contactType = "No Category";
          break;
        case 2191101768:
          if (!(fieldId == "610"))
            break;
          string[] strArray15 = Utils.SplitName(this.loanData.GetField("611"));
          this.rxContact.FirstName = strArray15[0];
          this.rxContact.LastName = strArray15[1];
          this.rxContact.WorkPhone = this.loanData.GetField("615");
          this.rxContact.BizEmail = this.loanData.GetField("87");
          this.rxContact.BizAddress1 = this.loanData.GetField("612");
          this.rxContact.BizCity = this.loanData.GetField("613");
          this.rxContact.BizState = this.loanData.GetField("1175");
          this.rxContact.BizZip = this.loanData.GetField("614");
          this.contactType = "Escrow Company";
          break;
        case 2308545101:
          if (!(fieldId == "617"))
            break;
          string[] strArray16 = Utils.SplitName(this.loanData.GetField("618"));
          this.rxContact.FirstName = strArray16[0];
          this.rxContact.LastName = strArray16[1];
          this.rxContact.WorkPhone = this.loanData.GetField("622");
          this.rxContact.BizEmail = this.loanData.GetField("89");
          this.rxContact.BizAddress1 = this.loanData.GetField("619");
          this.rxContact.BizCity = this.loanData.GetField("620");
          this.rxContact.BizState = this.loanData.GetField("1244");
          this.rxContact.BizZip = this.loanData.GetField("621");
          this.contactType = "Appraiser";
          break;
        case 2313243154:
          if (!(fieldId == "56"))
            break;
          string[] strArray17 = Utils.SplitName(this.loanData.GetField("VEND.X117"));
          this.rxContact.FirstName = strArray17[0];
          this.rxContact.LastName = strArray17[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X118");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X119");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X112");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X113");
          this.rxContact.BizState = this.loanData.GetField("VEND.X114");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X115");
          this.contactType = "Attorney";
          break;
        case 2331006511:
          if (!(fieldId == "37"))
            break;
          this.rxContact.FirstName = this.loanData.GetField("4000") + " " + this.loanData.GetField("4001");
          this.rxContact.FirstName = this.rxContact.FirstName.Trim();
          this.rxContact.LastName = this.loanData.GetField("4002") + " " + this.loanData.GetField("4003");
          this.rxContact.LastName = this.rxContact.LastName.Trim();
          this.rxContact.WorkPhone = this.loanData.GetField("FE0117");
          this.rxContact.BizEmail = this.loanData.GetField("1178");
          this.rxContact.BizAddress1 = this.loanData.GetField("FR0104");
          this.rxContact.BizCity = this.loanData.GetField("FR0106");
          this.rxContact.BizState = this.loanData.GetField("FR0107");
          this.rxContact.BizZip = this.loanData.GetField("FR0108");
          this.contactType = "Borrower";
          break;
        case 2407578002:
          if (!(fieldId == "VEND.X424"))
            break;
          string[] strArray18 = Utils.SplitName(this.loanData.GetField("VEND.X429"));
          this.rxContact.FirstName = strArray18[0];
          this.rxContact.LastName = strArray18[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X432");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X430");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X425");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X426");
          this.rxContact.BizState = this.loanData.GetField("VEND.X427");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X428");
          this.contactType = "No Category";
          break;
        case 2412400578:
          if (!(fieldId == "713"))
            break;
          string[] strArray19 = Utils.SplitName(this.loanData.GetField("714"));
          this.rxContact.FirstName = strArray19[0];
          this.rxContact.LastName = strArray19[1];
          this.rxContact.WorkPhone = this.loanData.GetField("718");
          this.rxContact.BizEmail = this.loanData.GetField("94");
          this.rxContact.BizAddress1 = this.loanData.GetField("715");
          this.rxContact.BizCity = this.loanData.GetField("716");
          this.rxContact.BizState = this.loanData.GetField("1253");
          this.rxContact.BizZip = this.loanData.GetField("717");
          this.contactType = "Builder";
          break;
        case 2598493664:
          if (!(fieldId == "VEND.X310"))
            break;
          string[] strArray20 = Utils.SplitName(this.loanData.GetField("VEND.X317"));
          this.rxContact.FirstName = strArray20[0];
          this.rxContact.LastName = strArray20[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X318");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X319");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X311");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X312");
          this.rxContact.BizState = this.loanData.GetField("VEND.X313");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X314");
          this.contactType = "Organization";
          break;
        case 2771239525:
          if (!(fieldId == "411"))
            break;
          string[] strArray21 = Utils.SplitName(this.loanData.GetField("416"));
          this.rxContact.FirstName = strArray21[0];
          this.rxContact.LastName = strArray21[1];
          this.rxContact.WorkPhone = this.loanData.GetField("417");
          this.rxContact.BizEmail = this.loanData.GetField("88");
          this.rxContact.BizAddress1 = this.loanData.GetField("412");
          this.rxContact.BizCity = this.loanData.GetField("413");
          this.rxContact.BizState = this.loanData.GetField("1174");
          this.rxContact.BizZip = this.loanData.GetField("414");
          this.contactType = "Title Insurance";
          break;
        case 3092074162:
          if (!(fieldId == "VEND.X293"))
            break;
          string[] strArray22 = Utils.SplitName(this.loanData.GetField("VEND.X302"));
          this.rxContact.FirstName = strArray22[0];
          this.rxContact.LastName = strArray22[1];
          this.rxContact.WorkPhone = (this.loanData.GetField("VEND.X303") + "/" + this.loanData.GetField("VEND.X304")).Trim('/', ' ');
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X305");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X294");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X295");
          this.rxContact.BizState = this.loanData.GetField("VEND.X296");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X297");
          this.rxContact.WebSite = this.loanData.GetField("VEND.X1036");
          this.contactType = "Organization";
          break;
        case 3194385916:
          if (!(fieldId == "395"))
            break;
          string[] strArray23 = Utils.SplitName(this.loanData.GetField("VEND.X195"));
          this.rxContact.FirstName = strArray23[0];
          this.rxContact.LastName = strArray23[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X196");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X197");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X190");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X191");
          this.rxContact.BizState = this.loanData.GetField("VEND.X192");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X193");
          this.contactType = "Doc Signing";
          break;
        case 3244205566:
          if (!(fieldId == "VEND.X200"))
            break;
          string[] strArray24 = Utils.SplitName(this.loanData.GetField("VEND.X206"));
          this.rxContact.FirstName = strArray24[0];
          this.rxContact.LastName = strArray24[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X207");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X208");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X201");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X202");
          this.rxContact.BizState = this.loanData.GetField("VEND.X203");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X204");
          this.contactType = "Warehouse Bank";
          break;
        case 3502923252:
          if (!(fieldId == "VEND.X178"))
            break;
          string[] strArray25 = Utils.SplitName(this.loanData.GetField("VEND.X184"));
          this.rxContact.FirstName = strArray25[0];
          this.rxContact.LastName = strArray25[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X185");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X186");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X179");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X180");
          this.rxContact.BizState = this.loanData.GetField("VEND.X181");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X182");
          this.contactType = "Servicing";
          break;
        case 3552564561:
          if (!(fieldId == "VEND.X133"))
            break;
          string[] strArray26 = Utils.SplitName(this.loanData.GetField("VEND.X139"));
          this.rxContact.FirstName = strArray26[0];
          this.rxContact.LastName = strArray26[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X140");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X141");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X134");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X135");
          this.rxContact.BizState = this.loanData.GetField("VEND.X136");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X137");
          this.contactType = "Real Estate Agent";
          break;
        case 3576531784:
          if (!(fieldId == "L252"))
            break;
          string[] strArray27 = Utils.SplitName(this.loanData.GetField("VEND.X162"));
          this.rxContact.FirstName = strArray27[0];
          this.rxContact.LastName = strArray27[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X163");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X164");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X157");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X158");
          this.rxContact.BizState = this.loanData.GetField("VEND.X159");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X160");
          this.contactType = "Hazard Insurance";
          break;
        case 3576678879:
          if (!(fieldId == "L248"))
            break;
          string[] strArray28 = Utils.SplitName(this.loanData.GetField("707"));
          this.rxContact.FirstName = strArray28[0];
          this.rxContact.LastName = strArray28[1];
          this.rxContact.WorkPhone = this.loanData.GetField("711");
          this.rxContact.BizEmail = this.loanData.GetField("93");
          this.rxContact.BizAddress1 = this.loanData.GetField("708");
          this.rxContact.BizCity = this.loanData.GetField("709");
          this.rxContact.BizState = this.loanData.GetField("1252");
          this.rxContact.BizZip = this.loanData.GetField("710");
          this.contactType = "Mortgage Insurance";
          break;
        case 3604030251:
          if (!(fieldId == "VEND.X144"))
            break;
          string[] strArray29 = Utils.SplitName(this.loanData.GetField("VEND.X150"));
          this.rxContact.FirstName = strArray29[0];
          this.rxContact.LastName = strArray29[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X151");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X152");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X145");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X146");
          this.rxContact.BizState = this.loanData.GetField("VEND.X147");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X148");
          this.rxContact.WebSite = this.loanData.GetField("VEND.X1038");
          this.contactType = "Real Estate Agent";
          break;
        case 3995840350:
          if (!(fieldId == "VEND.X11"))
            break;
          string[] strArray30 = Utils.SplitName(this.loanData.GetField("VEND.X2"));
          this.rxContact.FirstName = strArray30[0];
          this.rxContact.LastName = strArray30[1];
          this.rxContact.WorkPhone = this.loanData.GetField("VEND.X8");
          this.rxContact.BizEmail = this.loanData.GetField("VEND.X10");
          this.rxContact.BizAddress1 = this.loanData.GetField("VEND.X3");
          this.rxContact.BizCity = this.loanData.GetField("VEND.X4");
          this.rxContact.BizState = this.loanData.GetField("VEND.X5");
          this.rxContact.BizZip = this.loanData.GetField("VEND.X6");
          this.contactType = "No Category";
          break;
        case 4171302095:
          if (!(fieldId == "624"))
            break;
          string[] strArray31 = Utils.SplitName(this.loanData.GetField("625"));
          this.rxContact.FirstName = strArray31[0];
          this.rxContact.LastName = strArray31[1];
          this.rxContact.WorkPhone = this.loanData.GetField("629");
          this.rxContact.BizEmail = this.loanData.GetField("90");
          this.rxContact.BizAddress1 = this.loanData.GetField("626");
          this.rxContact.BizCity = this.loanData.GetField("627");
          this.rxContact.BizState = this.loanData.GetField("1245");
          this.rxContact.BizZip = this.loanData.GetField("628");
          this.contactType = "Credit Company";
          break;
      }
    }

    private void populateConversationLog(string fieldId)
    {
      string empty = string.Empty;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(fieldId))
      {
        case 111074370:
          if (!(fieldId == "638"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("638"));
          this.conversationDialog.SetCompany(string.Empty);
          this.conversationDialog.SetPhone((this.loanData.GetField("704") + "/" + this.loanData.GetField("VEND.X220")).Trim('/', ' '));
          this.conversationDialog.SetEmail(this.loanData.GetField("92"));
          break;
        case 216585232:
          if (!(fieldId == "69"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("68") + " " + this.loanData.GetField("69"));
          this.conversationDialog.SetCompany(string.Empty);
          this.conversationDialog.SetPhone((this.loanData.GetField("98") + "/" + this.loanData.GetField("FE0217")).Trim('/', ' '));
          this.conversationDialog.SetEmail(this.loanData.GetField("1268"));
          break;
        case 595448055:
          if (!(fieldId == "VEND.X412"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X412"));
          this.conversationDialog.SetCompany(string.Empty);
          this.conversationDialog.SetPhone((this.loanData.GetField("VEND.X417") + "/" + this.loanData.GetField("VEND.X421")).Trim('/', ' '));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X419"));
          break;
        case 908999587:
          if (!(fieldId == "REGZGFE.X8"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("984"));
          this.conversationDialog.SetCompany(this.loanData.GetField("REGZGFE.X8"));
          this.conversationDialog.SetPhone(this.loanData.GetField("1410"));
          this.conversationDialog.SetEmail(this.loanData.GetField("1411"));
          break;
        case 995269145:
          if (!(fieldId == "VEND.X278"))
            break;
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X278"));
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X286"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X288"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X287"));
          break;
        case 1079010145:
          if (!(fieldId == "VEND.X263"))
            break;
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X263"));
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X271"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X273"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X272"));
          break;
        case 1172536963:
          if (!(fieldId == "1500"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X13"));
          this.conversationDialog.SetCompany(this.loanData.GetField("1500"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X19"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X21"));
          break;
        case 1422057211:
          if (!(fieldId == "VEND.X122"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X128"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X122"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X129"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X130"));
          break;
        case 1561658572:
          if (!(fieldId == "VEND.X84"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X55"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X54"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X61"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X63"));
          break;
        case 1563630048:
          if (!(fieldId == "VEND.X44"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X45"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X44"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X51"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X53"));
          break;
        case 1578436191:
          if (!(fieldId == "VEND.X85"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X65"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X64"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X71"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X73"));
          break;
        case 1595213810:
          if (!(fieldId == "VEND.X86"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X75"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X74"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X81"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X83"));
          break;
        case 1597038191:
          if (!(fieldId == "VEND.X34"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X35"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X34"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X41"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X43"));
          break;
        case 1741391264:
          if (!(fieldId == "1264"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("1256"));
          this.conversationDialog.SetCompany(this.loanData.GetField("1264"));
          this.conversationDialog.SetPhone(this.loanData.GetField("1262"));
          this.conversationDialog.SetEmail(this.loanData.GetField("95"));
          break;
        case 1835328466:
          if (!(fieldId == "loan team member"))
            break;
          LoanAssociateLog tag = (LoanAssociateLog) this.gvContacts.SelectedItems[0].Tag;
          this.conversationDialog.SetName(tag.LoanAssociateName);
          this.conversationDialog.SetCompany(string.Empty);
          this.conversationDialog.SetPhone(tag.LoanAssociatePhone);
          this.conversationDialog.SetEmail(tag.LoanAssociateEmail);
          break;
        case 2191101768:
          if (!(fieldId == "610"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("611"));
          this.conversationDialog.SetCompany(this.loanData.GetField("610"));
          this.conversationDialog.SetPhone(this.loanData.GetField("615"));
          this.conversationDialog.SetEmail(this.loanData.GetField("87"));
          break;
        case 2308545101:
          if (!(fieldId == "617"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("618"));
          this.conversationDialog.SetCompany(this.loanData.GetField("617"));
          this.conversationDialog.SetPhone(this.loanData.GetField("622"));
          this.conversationDialog.SetEmail(this.loanData.GetField("89"));
          break;
        case 2313243154:
          if (!(fieldId == "56"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X117"));
          this.conversationDialog.SetCompany(this.loanData.GetField("56"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X118"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X119"));
          break;
        case 2331006511:
          if (!(fieldId == "37"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("36") + " " + this.loanData.GetField("37"));
          this.conversationDialog.SetCompany(string.Empty);
          this.conversationDialog.SetPhone((this.loanData.GetField("66") + "/" + this.loanData.GetField("FE0117")).Trim('/', ' '));
          this.conversationDialog.SetEmail(this.loanData.GetField("1240"));
          break;
        case 2407578002:
          if (!(fieldId == "VEND.X424"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X429"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X424"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X432"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X430"));
          break;
        case 2412400578:
          if (!(fieldId == "713"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("714"));
          this.conversationDialog.SetCompany(this.loanData.GetField("713"));
          this.conversationDialog.SetPhone(this.loanData.GetField("718"));
          this.conversationDialog.SetEmail(this.loanData.GetField("94"));
          break;
        case 2598493664:
          if (!(fieldId == "VEND.X310"))
            break;
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X310"));
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X317"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X319"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X318"));
          break;
        case 2771239525:
          if (!(fieldId == "411"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("416"));
          this.conversationDialog.SetCompany(this.loanData.GetField("411"));
          this.conversationDialog.SetPhone(this.loanData.GetField("417"));
          this.conversationDialog.SetEmail(this.loanData.GetField("88"));
          break;
        case 3092074162:
          if (!(fieldId == "VEND.X293"))
            break;
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X293"));
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X302"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X305"));
          this.conversationDialog.SetPhone((this.loanData.GetField("VEND.X303") + "/" + this.loanData.GetField("VEND.X304")).Trim('/', ' '));
          break;
        case 3194385916:
          if (!(fieldId == "395"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X195"));
          this.conversationDialog.SetCompany(this.loanData.GetField("395"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X196"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X197"));
          break;
        case 3244205566:
          if (!(fieldId == "VEND.X200"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X206"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X200"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X207"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X208"));
          break;
        case 3502923252:
          if (!(fieldId == "VEND.X178"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X184"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X178"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X185"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X186"));
          break;
        case 3552564561:
          if (!(fieldId == "VEND.X133"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X139"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X133"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X140"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X141"));
          break;
        case 3576531784:
          if (!(fieldId == "L252"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X162"));
          this.conversationDialog.SetCompany(this.loanData.GetField("L252"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X163"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X164"));
          break;
        case 3576678879:
          if (!(fieldId == "L248"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("707"));
          this.conversationDialog.SetCompany(this.loanData.GetField("L248"));
          this.conversationDialog.SetPhone(this.loanData.GetField("711"));
          this.conversationDialog.SetEmail(this.loanData.GetField("93"));
          break;
        case 3604030251:
          if (!(fieldId == "VEND.X144"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X150"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X144"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X151"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X152"));
          break;
        case 3995840350:
          if (!(fieldId == "VEND.X11"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("VEND.X2"));
          this.conversationDialog.SetCompany(this.loanData.GetField("VEND.X1"));
          this.conversationDialog.SetPhone(this.loanData.GetField("VEND.X8"));
          this.conversationDialog.SetEmail(this.loanData.GetField("VEND.X10"));
          break;
        case 4171302095:
          if (!(fieldId == "624"))
            break;
          this.conversationDialog.SetName(this.loanData.GetField("625"));
          this.conversationDialog.SetCompany(this.loanData.GetField("624"));
          this.conversationDialog.SetPhone(this.loanData.GetField("629"));
          this.conversationDialog.SetEmail(this.loanData.GetField("90"));
          break;
      }
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.gvContacts = new GridView();
      this.dlgButtons = new DialogButtons();
      this.labelContact = new Label();
      this.SuspendLayout();
      this.gvContacts.AllowMultiselect = false;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Company";
      gvColumn1.Width = 183;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Name";
      gvColumn2.Width = 191;
      this.gvContacts.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvContacts.Location = new Point(10, 10);
      this.gvContacts.Name = "gvContacts";
      this.gvContacts.Size = new Size(376, 304);
      this.gvContacts.TabIndex = 0;
      this.gvContacts.DoubleClick += new EventHandler(this.btnSelect_Click);
      this.dlgButtons.Dock = DockStyle.Bottom;
      this.dlgButtons.Location = new Point(0, 316);
      this.dlgButtons.Name = "dlgButtons";
      this.dlgButtons.OKText = "&Select";
      this.dlgButtons.Size = new Size(397, 44);
      this.dlgButtons.TabIndex = 1;
      this.dlgButtons.OK += new EventHandler(this.btnSelect_Click);
      this.labelContact.AutoSize = true;
      this.labelContact.Cursor = Cursors.Hand;
      this.labelContact.ForeColor = SystemColors.ActiveCaption;
      this.labelContact.Location = new Point(12, 330);
      this.labelContact.Name = "labelContact";
      this.labelContact.Size = new Size(158, 14);
      this.labelContact.TabIndex = 2;
      this.labelContact.Text = "Select From Business Contacts";
      this.labelContact.Click += new EventHandler(this.labelContact_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(397, 360);
      this.Controls.Add((Control) this.labelContact);
      this.Controls.Add((Control) this.dlgButtons);
      this.Controls.Add((Control) this.gvContacts);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SelectionDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Select a Contact";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void labelContact_Click(object sender, EventArgs e)
    {
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact(RxBusinessContact.ActionMode.SelectMode, false))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.rxContact = rxBusinessContact.RxContactRecord;
        if (this.conversationDialog != null)
        {
          this.conversationDialog.SetName(this.rxContact.FirstName + " " + this.rxContact.LastName);
          this.conversationDialog.SetCompany(this.rxContact.CompanyName);
          this.conversationDialog.SetPhone((this.rxContact.HomePhone + "/" + this.rxContact.WorkPhone).Trim('/', ' '), false);
          this.conversationDialog.SetEmail(this.rxContact.BizEmail);
        }
        this.DialogResult = DialogResult.OK;
      }
    }
  }
}
