// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Log.LockForm
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Log
{
  public class LockForm : UserControl
  {
    private LoanDataMgr loanMgr;
    private bool readOnly = true;
    private string[] rateOptions = new string[0];
    private string[] priceOptions = new string[0];
    private string[] marginOptions = new string[0];
    private PopupBusinessRules popupRules;
    private bool isFirstLoading = true;
    private bool hideControls;
    private bool hideBR;
    private bool hideBP;
    private bool hideEX;
    private bool hideRL;
    private bool hideCPA;
    private bool hideBM;
    private ResourceManager UIresources;
    private LoanLockTool loanLockTool;
    private LockRequestCalculator lockCalculator;
    private bool use10Digits;
    private bool hidePR;
    private bool isDirty;
    private string[] profitabilityOptions = new string[0];
    private Hashtable companySettings;
    private LockExtensionUtils lockExtensionUtils;
    private string[] locktypeOptions = new string[0];
    private bool isSummary;
    private bool hideCurrentExpDate;
    private int additionalHeight = 88;
    private bool isUpdateSell;
    private bool hideCPC;
    private bool hideBPC;
    private Hashtable dataTables;
    private LockRequestLog lrl;
    private IContainer components;
    private GroupBox groupBox3;
    private TextBox txtInvestCommitment;
    private Label label23;
    private GroupBox groupBox7;
    private Label label24;
    private Label label25;
    private Label label26;
    private Label label27;
    private Label label28;
    private Label label29;
    private GroupBox groupBox8;
    private Label label31;
    private Label label32;
    private Label label35;
    private GroupBox groupBox9;
    private Label label36;
    private Label label37;
    private Label label38;
    private Label label39;
    private Label label40;
    private Label label41;
    private TextBox textBox43;
    private Label label42;
    private TextBox textBox44;
    private Label label43;
    private Label label44;
    private Label label45;
    private TextBox textBox46;
    private Label label46;
    private TextBox textBox47;
    private Label label47;
    private TextBox txtInvestLoan;
    private Label label48;
    private Label label49;
    private TextBox textBox50;
    private Label label50;
    private Button editBtn;
    private CheckBox checkBoxHedging;
    private ComboBox comboBoxPrice22;
    private ComboBox comboBoxPrice23;
    private ComboBox comboBoxPrice17;
    private ComboBox comboBoxPrice18;
    private ComboBox comboBoxPrice19;
    private ComboBox comboBoxPrice20;
    private ComboBox comboBoxPrice21;
    private ComboBox comboBoxPrice10;
    private ComboBox comboBoxPrice11;
    private ComboBox comboBoxPrice31;
    private ComboBox comboBoxPrice32;
    private ComboBox comboBoxPrice33;
    private ComboBox comboBoxPrice34;
    private ComboBox comboBoxPrice35;
    private ComboBox comboBoxPrice36;
    private ComboBox comboBoxPrice37;
    private ComboBox comboBoxPrice38;
    private ComboBox comboBoxPrice39;
    private ComboBox comboBoxPrice40;
    private ComboBox comboBoxPrice41;
    private ComboBox comboBoxPrice42;
    private ComboBox comboBoxPrice43;
    private ComboBox comboBoxPrice44;
    private ComboBox comboBoxPrice45;
    private ComboBox comboBoxPrice46;
    private ComboBox comboBoxPrice47;
    private ComboBox comboBoxPrice48;
    private ComboBox comboBoxPrice49;
    private ComboBox comboBoxPrice50;
    private ComboBox comboBoxRate23;
    private ComboBox comboBoxRate22;
    private ComboBox comboBoxRate21;
    private ToolTip toolTipField;
    private TextBox textBoxPrice46;
    private TextBox textBoxPrice45;
    private TextBox textBoxPrice44;
    private TextBox textBoxPrice43;
    private TextBox textBoxPrice42;
    private TextBox textBoxPrice41;
    private TextBox textBoxPrice40;
    private TextBox textBoxPrice39;
    private TextBox textBoxPrice38;
    private TextBox textBoxPrice37;
    private TextBox textBoxPrice36;
    private TextBox textBoxPrice35;
    private TextBox textBoxPrice34;
    private TextBox textBoxPrice33;
    private TextBox textBoxPrice32;
    private TextBox textBoxPrice31;
    private TextBox textBoxPrice30;
    private TextBox textBoxPrice29;
    private TextBox textBoxPrice28;
    private TextBox textBoxPrice27;
    private TextBox textBoxPrice50;
    private TextBox textBoxPrice26;
    private TextBox textBoxRate46;
    private TextBox textBoxRate45;
    private TextBox textBoxRate27;
    private TextBox textBoxRate26;
    private TextBox textBoxRate25;
    private TextBox textBoxPrice21;
    private TextBox textBoxPrice20;
    private TextBox textBoxPrice19;
    private TextBox textBoxPrice18;
    private TextBox textBoxPrice17;
    private TextBox textBoxPrice16;
    private TextBox textBoxPrice15;
    private TextBox textBoxPrice14;
    private TextBox textBoxPrice13;
    private TextBox textBoxPrice12;
    private TextBox textBoxRate24;
    private Button btnCopyRequest;
    private Button btnCopyBuy;
    private TextBox txtSellLockDays;
    private TextBox txtSellSideSRP;
    private Label label61;
    private TextBox textBoxZip;
    private TextBox textBoxInvestorContact;
    private TextBox textBoxInvestorName;
    private TextBox textBoxWebUrl;
    private TextBox textBoxState;
    private TextBox textBoxCity;
    private TextBox textBoxAddress;
    private TextBox textBoxPhone;
    private Label label12;
    private TextBox textBoxRate40;
    private ComboBox comboBoxRate36;
    private TextBox textBoxRate39;
    private ComboBox comboBoxRate35;
    private TextBox textBoxRate38;
    private ComboBox comboBoxRate34;
    private TextBox textBoxRate37;
    private ComboBox comboBoxRate33;
    private TextBox textBoxRate36;
    private ComboBox comboBoxRate32;
    private TextBox textBoxRate35;
    private ComboBox comboBoxRate31;
    private TextBox textBoxRate34;
    private ComboBox comboBoxRate30;
    private TextBox textBoxRate33;
    private ComboBox comboBoxRate29;
    private TextBox textBoxRate32;
    private ComboBox comboBoxRate28;
    private TextBox textBoxRate31;
    private ComboBox comboBoxRate27;
    private TextBox textBoxRate30;
    private ComboBox comboBoxRate26;
    private TextBox textBoxRate29;
    private ComboBox comboBoxRate25;
    private TextBox textBoxRate28;
    private ComboBox comboBoxRate24;
    private TextBox textBoxRate44;
    private ComboBox comboBoxRate40;
    private TextBox textBoxRate43;
    private ComboBox comboBoxRate39;
    private TextBox textBoxRate42;
    private ComboBox comboBoxRate38;
    private TextBox textBoxRate41;
    private ComboBox comboBoxRate37;
    private PictureBox pboxAsterisk;
    private PictureBox pboxDownArrow;
    private TextBox txtFulfilled;
    private Label labelFulfilled;
    private Panel panelTop;
    private Panel panelBody;
    private Panel panelBodyBottom;
    private Panel pnlBuySideInfo;
    private Panel panelLeft80;
    private Panel panel1BodyTop;
    private Panel panelBodyRight;
    private Panel panelBodyLeft;
    private Panel panelBodyLeftInside;
    private Panel panelBodyRightInside;
    private Panel panelRight50;
    private Panel panelRight70;
    private Panel panelRight60;
    private Panel panelRight80;
    private Panel panelRight110;
    private ComboBox comboBoxMargin21;
    private TextBox textBoxMargin31;
    private ComboBox comboBoxMargin22;
    private TextBox textBoxMargin32;
    private ComboBox comboBoxMargin23;
    private TextBox textBoxMargin33;
    private ComboBox comboBoxMargin24;
    private TextBox textBoxMargin34;
    private Panel panelRight100;
    private Label label17;
    private TextBox textBoxMargin30;
    private GroupBox groupBox2;
    private Panel panelRight130;
    private Label label19;
    private TextBox textBoxMargin66;
    private GroupBox groupBox6;
    private Label label20;
    private TextBox textBoxMargin67;
    private Panel panelRight120;
    private ComboBox comboBoxMargin25;
    private TextBox textBoxMargin35;
    private TextBox textBoxMargin36;
    private ComboBox comboBoxMargin26;
    private ComboBox comboBoxMargin27;
    private TextBox textBoxMargin37;
    private ComboBox comboBoxMargin28;
    private TextBox textBoxMargin38;
    private TextBox textBoxMargin39;
    private ComboBox comboBoxMargin29;
    private ComboBox comboBoxMargin30;
    private TextBox textBoxMargin40;
    private TextBox textBoxMargin41;
    private ComboBox comboBoxMargin31;
    private ComboBox comboBoxMargin32;
    private TextBox textBoxMargin42;
    private TextBox textBoxMargin51;
    private ComboBox comboBoxMargin33;
    private ComboBox comboBoxMargin34;
    private TextBox textBoxMargin55;
    private TextBox textBoxMargin59;
    private ComboBox comboBoxMargin35;
    private ComboBox comboBoxMargin36;
    private TextBox textBoxMargin60;
    private TextBox textBoxMargin61;
    private ComboBox comboBoxMargin37;
    private ComboBox comboBoxMargin38;
    private TextBox textBoxMargin62;
    private TextBox textBoxMargin63;
    private ComboBox comboBoxMargin39;
    private ComboBox comboBoxMargin40;
    private TextBox textBoxMargin64;
    private Panel panelRight140;
    private Panel panelRight150;
    private Panel panelRight160;
    private Label label18;
    private Panel panelLeftInvestor;
    private Button btnTradeSummary;
    private TextBox txtInvestorStatus;
    private Button btnInvestor;
    private Button btnClearSellSide;
    private Button btnClearBuyside;
    private Label label21;
    private TextBox textBoxEmail;
    private TextBox textBoxLoanProgram;
    private Label labelLoanProgram;
    private GroupContainer groupContainerRight;
    private Panel panelRightInvestor;
    private Panel panelRight30;
    private Panel panelRight20;
    private Panel pnlSellSideInfo;
    private Panel panelRight40;
    private GroupContainer groupContainer2;
    private GroupBox groupBox10;
    private StandardIconButton iconBtnInvestor;
    private GradientPanel gradientPanel1;
    private DatePicker txtSellLockDate;
    private DatePicker txtSellExpireDate;
    private Label label30;
    private DatePicker dp2297;
    private DatePicker dp2206;
    private DatePicker dp2291;
    private TextBox textBox3;
    private Label label63;
    private DatePicker datePicker2;
    private Label label65;
    private Label label66;
    private Panel pnlBuySideLockExtension;
    private Label label67;
    private TextBox textBox5;
    private TextBox txtSellSideRequestedType;
    private Label label72;
    private Panel panelSellSideProfitSpacerA;
    private Panel panelSellSideProfitSpacerB;
    private Panel panelSellSideExtensionA;
    private TextBox textBoxExtension13Desc;
    private TextBox textBoxExtension13;
    private TextBox textBoxExtension12Desc;
    private TextBox textBoxExtension12;
    private TextBox textBoxExtension11Desc;
    private TextBox textBoxExtension11;
    private Label label78;
    private Panel panelSellSideExtensionB;
    private Label label91;
    private Label label90;
    private Label label89;
    private Label label88;
    private TextBox textBoxExtension20Desc;
    private TextBox textBoxExtension20;
    private Label label98;
    private TextBox textBoxExtension19Desc;
    private TextBox textBoxExtension19;
    private Label label99;
    private TextBox textBoxExtension18Desc;
    private TextBox textBoxExtension18;
    private Label label100;
    private TextBox textBoxExtension17Desc;
    private TextBox textBoxExtension17;
    private Label label101;
    private TextBox textBoxExtension16Desc;
    private TextBox textBoxExtension16;
    private Label label102;
    private TextBox textBoxExtension15Desc;
    private TextBox textBoxExtension15;
    private Label label103;
    private TextBox textBoxExtension14Desc;
    private TextBox textBoxExtension14;
    private Panel panelRight155;
    private Label label105;
    private TextBox textBoxServicer;
    private Label label104;
    private StandardIconButton iconButtonServicer;
    private ComboBox comboBoxServicingType;
    private GroupBox groupBox11;
    private ComboBox comboBoxLockRate;
    private Panel panelBodyComp;
    private Panel panel2;
    private YellowGroupContainer GroupContainerComp;
    private Panel panelComp150;
    private Button btnInvestorComp;
    private Label label142;
    private StandardIconButton iconBtnCompInvestor;
    private Label label143;
    private TextBox textBoxEmail2;
    private Label label144;
    private TextBox textBoxInvestorName2;
    private Label label145;
    private TextBox textBoxInvestorContact2;
    private Label label146;
    private TextBox textBoxPhone2;
    private Label label147;
    private TextBox textBoxAddress2;
    private TextBox textBoxCity2;
    private Label label148;
    private GroupBox groupBox23;
    private TextBox textBoxState2;
    private TextBox textBoxWebUrl2;
    private Label label149;
    private Label label150;
    private TextBox textBoxZip2;
    private Panel panelComp140;
    private GroupBox groupBox22;
    private TextBox textBoxPrice74;
    private Label label140;
    private TextBox txtCompSRP;
    private Label label141;
    private Panel panelComp130;
    private Label label132;
    private TextBox textBoxMargin74;
    private GroupBox groupBox19;
    private GroupBox groupBox20;
    private Label label134;
    private TextBox textBoxMargin75;
    private Panel panelComp120;
    private ComboBox comboBoxMargin45;
    private TextBox textBoxMargin49;
    private TextBox textBoxMargin73;
    private ComboBox comboBoxMargin46;
    private ComboBox comboBoxMargin47;
    private TextBox textBoxMargin48;
    private ComboBox comboBoxMargin60;
    private TextBox textBoxMargin50;
    private TextBox textBoxMargin72;
    private ComboBox comboBoxMargin48;
    private ComboBox comboBoxMargin59;
    private TextBox textBoxMargin52;
    private TextBox textBoxMargin71;
    private ComboBox comboBoxMargin49;
    private ComboBox comboBoxMargin58;
    private TextBox textBoxMargin53;
    private TextBox textBoxMargin70;
    private ComboBox comboBoxMargin50;
    private ComboBox comboBoxMargin57;
    private TextBox textBoxMargin54;
    private TextBox textBoxMargin69;
    private ComboBox comboBoxMargin51;
    private ComboBox comboBoxMargin56;
    private TextBox textBoxMargin56;
    private TextBox textBoxMargin68;
    private ComboBox comboBoxMargin52;
    private ComboBox comboBoxMargin55;
    private TextBox textBoxMargin57;
    private TextBox textBoxMargin65;
    private ComboBox comboBoxMargin53;
    private ComboBox comboBoxMargin54;
    private TextBox textBoxMargin58;
    private Panel panelComp110;
    private ComboBox comboBoxMargin41;
    private TextBox textBoxMargin44;
    private ComboBox comboBoxMargin42;
    private TextBox textBoxMargin45;
    private ComboBox comboBoxMargin43;
    private TextBox textBoxMargin46;
    private ComboBox comboBoxMargin44;
    private TextBox textBoxMargin47;
    private Panel panelComp100;
    private Label label139;
    private TextBox textBoxMargin43;
    private GroupBox groupBox17;
    private GroupBox groupBox18;
    private Panel panelCompExtensionB;
    private Label label119;
    private TextBox textBoxExtension30Desc;
    private TextBox textBoxExtension30;
    private Label label120;
    private TextBox textBoxExtension29Desc;
    private TextBox textBoxExtension29;
    private Label label121;
    private TextBox textBoxExtension28Desc;
    private TextBox textBoxExtension28;
    private Label label122;
    private TextBox textBoxExtension27Desc;
    private TextBox textBoxExtension27;
    private Label label123;
    private TextBox textBoxExtension26Desc;
    private TextBox textBoxExtension26;
    private Label label124;
    private TextBox textBoxExtension25Desc;
    private TextBox textBoxExtension25;
    private Label label125;
    private TextBox textBoxExtension24Desc;
    private TextBox textBoxExtension24;
    private Panel panelCompExtensionA;
    private Label label126;
    private Label label127;
    private Label label128;
    private TextBox textBoxExtension23Dec;
    private TextBox textBoxExtension23;
    private TextBox textBoxExtension22Desc;
    private TextBox textBoxExtension22;
    private TextBox textBoxExtension21Desc;
    private TextBox textBoxExtension21;
    private Label label129;
    private Panel panelComp80;
    private TextBox textBoxPrice71;
    private ComboBox comboBoxPrice61;
    private ComboBox comboBoxPrice70;
    private TextBox textBoxPrice62;
    private TextBox textBoxPrice70;
    private ComboBox comboBoxPrice62;
    private ComboBox comboBoxPrice69;
    private TextBox textBoxPrice63;
    private TextBox textBoxPrice69;
    private ComboBox comboBoxPrice63;
    private ComboBox comboBoxPrice68;
    private TextBox textBoxPrice64;
    private TextBox textBoxPrice68;
    private ComboBox comboBoxPrice64;
    private ComboBox comboBoxPrice67;
    private TextBox textBoxPrice65;
    private TextBox textBoxPrice67;
    private ComboBox comboBoxPrice65;
    private ComboBox comboBoxPrice66;
    private TextBox textBoxPrice66;
    private Panel panelComp70;
    private ComboBox comboBoxPrice51;
    private TextBox textBoxPrice52;
    private ComboBox comboBoxPrice52;
    private TextBox textBoxPrice53;
    private ComboBox comboBoxPrice53;
    private TextBox textBoxPrice54;
    private ComboBox comboBoxPrice54;
    private TextBox textBoxPrice55;
    private ComboBox comboBoxPrice55;
    private TextBox textBoxPrice56;
    private ComboBox comboBoxPrice56;
    private TextBox textBoxPrice57;
    private ComboBox comboBoxPrice57;
    private TextBox textBoxPrice58;
    private ComboBox comboBoxPrice58;
    private TextBox textBoxPrice59;
    private ComboBox comboBoxPrice59;
    private TextBox textBoxPrice60;
    private ComboBox comboBoxPrice60;
    private TextBox textBoxPrice61;
    private Panel panelComp60;
    private Label label130;
    private TextBox textBoxPrice51;
    private Label label131;
    private Panel panelCompProfitSpacerB;
    private Panel panelCompProfitSpacerA;
    private GroupBox groupBox16;
    private Panel panelComp50;
    private Label label114;
    private TextBox textBoxRate68;
    private GroupBox groupBox13;
    private Label label115;
    private TextBox textBoxRate69;
    private Panel panelComp40;
    private TextBox textBoxRate67;
    private ComboBox comboBoxRate47;
    private TextBox textBoxRate60;
    private ComboBox comboBoxRate60;
    private ComboBox comboBoxRate54;
    private TextBox textBoxRate54;
    private ComboBox comboBoxRate53;
    private TextBox textBoxRate66;
    private TextBox textBoxRate61;
    private ComboBox comboBoxRate48;
    private TextBox textBoxRate59;
    private ComboBox comboBoxRate59;
    private ComboBox comboBoxRate55;
    private TextBox textBoxRate55;
    private ComboBox comboBoxRate52;
    private TextBox textBoxRate65;
    private TextBox textBoxRate62;
    private ComboBox comboBoxRate49;
    private TextBox textBoxRate58;
    private ComboBox comboBoxRate58;
    private ComboBox comboBoxRate56;
    private TextBox textBoxRate56;
    private ComboBox comboBoxRate51;
    private TextBox textBoxRate64;
    private TextBox textBoxRate63;
    private ComboBox comboBoxRate50;
    private TextBox textBoxRate57;
    private ComboBox comboBoxRate57;
    private Panel panelComp30;
    private ComboBox comboBoxRate41;
    private TextBox textBoxRate48;
    private ComboBox comboBoxRate44;
    private TextBox textBoxRate53;
    private TextBox textBoxRate51;
    private ComboBox comboBoxRate42;
    private TextBox textBoxRate50;
    private ComboBox comboBoxRate46;
    private ComboBox comboBoxRate45;
    private TextBox textBoxRate49;
    private ComboBox comboBoxRate43;
    private TextBox textBoxRate52;
    private Panel panelComp20;
    private TextBox textBoxRate47;
    private Label label116;
    private Panel pnlCompInfo;
    private TextBox txtCompRequestedType;
    private DatePicker dp3663;
    private Label label108;
    private Label label109;
    private DatePicker txtCompLockDate;
    private Label label110;
    private TextBox textBox10;
    private Label label111;
    private GroupBox groupBox12;
    private TextBox txt3662;
    private Label label112;
    private TextBox txtCompLockDays;
    private Label label113;
    private Panel panelCompInvestor;
    private TextBox textBoxCompLoanProgram;
    private Label labelCompLoanProgram;
    private GroupBox groupBox14;
    private Panel panelComp160;
    private ComboBox comboBoxLockRate2;
    private Label label155;
    private TextBox textBox134;
    private Label label160;
    private GroupBox groupBox25;
    private Panel panel28;
    private GroupBox groupBox24;
    private ComboBox comboBoxServicingType2;
    private Label label152;
    private GroupBox groupBox15;
    private Button btnClearComp;
    private Button btnCopyBuytoComp;
    private GroupContainer groupContainer1;
    private Panel panelBottomBody;
    private Panel panelBottomRight;
    private BorderPanel borderPanel3;
    private Label label133;
    private Label label135;
    private TextBox txtCompSRP2;
    private TextBox textBox15;
    private Label label136;
    private TextBox textBox16;
    private BorderPanel borderPanel2;
    private Label label54;
    private Label label56;
    private TextBox txtSellSRP;
    private TextBox textBox54;
    private Label label55;
    private TextBox textBox56;
    private Panel panelBottomLeft;
    private BorderPanel borderPanel1;
    private Label label52;
    private TextBox textBox53;
    private Label label51;
    private Label label53;
    private TextBox textBox52;
    private TextBox txtBuySRP;
    private Panel panelRight170;
    private Button btnSellComment;
    private TextBox txtSellComments;
    private Label label60;
    private Panel panelSumSell1;
    private Label label157;
    private TextBox textBox9;
    private Label label158;
    private TextBox textBox11;
    private GroupBox groupBox27;
    private TextBox textBox12;
    private Label label159;
    private Label label161;
    private TextBox textBox13;
    private Panel panelComp170;
    private Button btnCompComment;
    private TextBox txtCompComments;
    private Label label153;
    private Panel panelSumComp2;
    private ComboBox comboBoxLockRate4;
    private Button btnInvestorComp2;
    private Label label162;
    private StandardIconButton standardIconButton3;
    private Label label163;
    private TextBox textBoxCompInvestorName2;
    private Panel panelSumComp1;
    private TextBox textBox14;
    private Label label164;
    private TextBox textBox18;
    private Label label165;
    private TextBox textBoxMargin95;
    private Label label166;
    private Label label167;
    private TextBox textBox20;
    private Panel panelBottomBottom;
    private BorderPanel borderPanel5;
    private Label label57;
    private TextBox textBox21;
    private Label label58;
    private TextBox textBox24;
    private BorderPanel borderPanel4;
    private Label label137;
    private TextBox textBox17;
    private Label label138;
    private TextBox textBox19;
    private BorderPanel borderPanel6;
    private GroupBox groupBox29;
    private GroupBox groupBox28;
    private DatePicker txtCompExpireDate;
    private Label label74;
    private Label label168;
    private TextBox textBox23;
    private Label label107;
    private TextBox textBox8;
    private Label label106;
    private TextBox textBox7;
    private Panel panel3;
    private Label label176;
    private TextBox textBox25;
    private Label label177;
    private TextBox textBox26;
    private CheckBox checkBoxONRPLock;
    private GroupContainer grpCorrespondent;
    private TextBox txtDeliveryExpireDate;
    private Label label175;
    private TextBox txtExpireDate;
    private Label label174;
    private TextBox txtDeliveryType;
    private Label label171;
    private TextBox txtCommitmentType;
    private Label label170;
    private TextBox txtMasterNo;
    private TextBox txtCommitmentDate;
    private Label label169;
    private TextBox txtCommitmentNo;
    private Label label172;
    private Label label173;
    private Panel panelLeft150;
    private Button btnBuyComment;
    private Label label59;
    private TextBox txtBuyComments;
    private Panel panelLeft140;
    private TextBox textBox22;
    private Label label22;
    private Panel panelLeft130;
    private Label label13;
    private TextBox txtBuySideSRP;
    private Label label15;
    private TextBox textBoxMargin28;
    private GroupBox groupBox1;
    private Label label16;
    private TextBox textBoxMargin29;
    private Panel panelLeft120;
    private ComboBox comboBoxMargin100;
    private ComboBox comboBoxMargin5;
    private TextBox textBoxMargin11;
    private TextBox textBoxMargin12;
    private ComboBox comboBoxMargin6;
    private ComboBox comboBoxMargin7;
    private TextBox textBoxMargin10;
    private TextBox textBoxMargin13;
    private TextBox textBoxMargin14;
    private ComboBox comboBoxMargin9;
    private ComboBox comboBoxMargin10;
    private TextBox textBoxMargin15;
    private TextBox textBoxMargin16;
    private ComboBox comboBoxMargin11;
    private ComboBox comboBoxMargin12;
    private TextBox textBoxMargin17;
    private TextBox textBoxMargin18;
    private ComboBox comboBoxMargin13;
    private ComboBox comboBoxMargin14;
    private TextBox textBoxMargin19;
    private TextBox textBoxMargin20;
    private ComboBox comboBoxMargin15;
    private ComboBox comboBoxMargin16;
    private TextBox textBoxMargin21;
    private TextBox textBoxMargin24;
    private ComboBox comboBoxMargin17;
    private ComboBox comboBoxMargin18;
    private TextBox textBoxMargin25;
    private TextBox textBoxMargin26;
    private ComboBox comboBoxMargin19;
    private ComboBox comboBoxMargin20;
    private TextBox textBoxMargin27;
    private Panel panelLeft110;
    private ComboBox comboBoxMargin1;
    private TextBox textBoxMargin6;
    private ComboBox comboBoxMargin2;
    private TextBox textBoxMargin7;
    private ComboBox comboBoxMargin3;
    private TextBox textBoxMargin8;
    private ComboBox comboBoxMargin4;
    private TextBox textBoxMargin9;
    private Panel panelLeft100;
    private PictureBox picBMZoomOut;
    private PictureBox picBMZoomIn;
    private Label label14;
    private TextBox textBoxMargin5;
    private GroupBox groupBox5;
    private Panel panelBuySideExtensionB;
    private Label label97;
    private TextBox textBoxExtension10Desc;
    private TextBox textBoxExtension10;
    private Label label95;
    private TextBox textBoxExtension9Desc;
    private TextBox textBoxExtension9;
    private Label label96;
    private TextBox textBoxExtension8Desc;
    private TextBox textBoxExtension8;
    private Label label93;
    private TextBox textBoxExtension7Desc;
    private TextBox textBoxExtension7;
    private Label label94;
    private TextBox textBoxExtension6Desc;
    private TextBox textBoxExtension6;
    private Label label92;
    private TextBox textBoxExtension5Desc;
    private TextBox textBoxExtension5;
    private Label label87;
    private TextBox textBoxExtension4Desc;
    private TextBox textBoxExtension4;
    private Panel panelBuySideExtensionA;
    private Label label86;
    private Label label85;
    private Label label84;
    private TextBox textBoxExtension3Desc;
    private TextBox textBoxExtension3;
    private TextBox textBoxExtension2Desc;
    private TextBox textBoxExtension2;
    private TextBox textBoxExtension1Desc;
    private TextBox textBoxExtension1;
    private PictureBox picEXZoomOut;
    private PictureBox picEXZoomIn;
    private Label label82;
    private ComboBox comboBoxPrice16;
    private Panel panelLeft70;
    private ComboBox comboBoxPrice6;
    private TextBox textBoxPrice2;
    private ComboBox comboBoxPrice5;
    private TextBox textBoxPrice3;
    private ComboBox comboBoxPrice4;
    private TextBox textBoxPrice4;
    private ComboBox comboBoxPrice9;
    private TextBox textBoxPrice5;
    private ComboBox comboBoxPrice8;
    private TextBox textBoxPrice6;
    private ComboBox comboBoxPrice7;
    private TextBox textBoxPrice7;
    private ComboBox comboBoxPrice15;
    private TextBox textBoxPrice8;
    private ComboBox comboBoxPrice14;
    private TextBox textBoxPrice9;
    private ComboBox comboBoxPrice13;
    private TextBox textBoxPrice10;
    private ComboBox comboBoxPrice12;
    private TextBox textBoxPrice11;
    private Panel panelLeft60;
    private TextBox textBoxAdjBuyPrice;
    private Label label79;
    private Label label62;
    private PictureBox picBPZoomOut;
    private PictureBox picBPZoomIn;
    private Panel panelBuySideProfitB;
    private ComboBox comboBoxProfitMargin13;
    private TextBox textBoxProfitMargin13;
    private ComboBox comboBoxProfitMargin14;
    private TextBox textBoxProfitMargin14;
    private ComboBox comboBoxProfitMargin9;
    private TextBox textBoxProfitMargin9;
    private ComboBox comboBoxProfitMargin10;
    private TextBox textBoxProfitMargin10;
    private ComboBox comboBoxProfitMargin11;
    private TextBox textBoxProfitMargin11;
    private ComboBox comboBoxProfitMargin12;
    private TextBox textBoxProfitMargin12;
    private ComboBox comboBoxProfitMargin5;
    private TextBox textBoxProfitMargin5;
    private ComboBox comboBoxProfitMargin6;
    private TextBox textBoxProfitMargin6;
    private ComboBox comboBoxProfitMargin7;
    private TextBox textBoxProfitMargin7;
    private ComboBox comboBoxProfitMargin8;
    private TextBox textBoxProfitMargin8;
    private Panel panelBuySideProfitA;
    private ComboBox comboBoxProfitMargin1;
    private TextBox textBoxProfitMargin1;
    private ComboBox comboBoxProfitMargin2;
    private Label label11;
    private TextBox textBoxPrice1;
    private TextBox textBoxProfitMargin2;
    private ComboBox comboBoxProfitMargin3;
    private TextBox textBoxProfitMargin3;
    private ComboBox comboBoxProfitMargin4;
    private TextBox textBoxProfitMargin4;
    private PictureBox picPRZoomOut;
    private PictureBox picPRZoomIn;
    private Label label83;
    private Panel panelLeft50;
    private Label label77;
    private TextBox textBox6;
    private Label label75;
    private TextBox textBox2;
    private Label label76;
    private TextBox textBox4;
    private Label label7;
    private TextBox textBoxRate22;
    private GroupBox groupBox4;
    private Label label6;
    private TextBox textBoxRate23;
    private Panel panelLeft40;
    private TextBox textBoxRate21;
    private ComboBox comboBoxRate7;
    private ComboBox comboBoxRate20;
    private TextBox textBoxRate8;
    private TextBox textBoxRate20;
    private ComboBox comboBoxRate8;
    private ComboBox comboBoxRate19;
    private TextBox textBoxRate9;
    private TextBox textBoxRate19;
    private ComboBox comboBoxRate9;
    private ComboBox comboBoxRate18;
    private TextBox textBoxRate10;
    private TextBox textBoxRate18;
    private ComboBox comboBoxRate10;
    private ComboBox comboBoxRate17;
    private TextBox textBoxRate11;
    private TextBox textBoxRate17;
    private ComboBox comboBoxRate11;
    private ComboBox comboBoxRate16;
    private TextBox textBoxRate12;
    private TextBox textBoxRate16;
    private ComboBox comboBoxRate12;
    private ComboBox comboBoxRate15;
    private TextBox textBoxRate13;
    private TextBox textBoxRate15;
    private ComboBox comboBoxRate13;
    private ComboBox comboBoxRate14;
    private TextBox textBoxRate14;
    private Panel panelLeft30;
    private ComboBox comboBoxRate1;
    private TextBox textBoxRate2;
    private ComboBox comboBoxRate2;
    private TextBox textBoxRate3;
    private ComboBox comboBoxRate3;
    private TextBox textBoxRate4;
    private ComboBox comboBoxRate4;
    private TextBox textBoxRate5;
    private ComboBox comboBoxRate5;
    private TextBox textBoxRate6;
    private ComboBox comboBoxRate6;
    private TextBox textBoxRate7;
    private Panel panelLeft20;
    private PictureBox picZoomOutOver;
    private PictureBox picZoomInOver;
    private PictureBox picBRZoomOut;
    private PictureBox picBRZoomIn;
    private Label label8;
    private TextBox textBoxRate1;
    private Panel pnlBuySideLockExpDate;
    private DatePicker txtBuyExpireDate;
    private Label label5;
    private Panel panel1;
    private Label label68;
    private Label label71;
    private Label label70;
    private DatePicker txtBuySideExtendedLockExpires;
    private TextBox txtBuySideDaysToExtend;
    private TextBox txtBuySideLockExtendPriceAdj;
    private Panel pnlBuySideLockPricingField;
    private Panel pnlBuySideRequest;
    private TextBox txtBuySideRequestedType;
    private TextBox txt2148;
    private Label label666;
    private Label label2;
    private TextBox textBox1;
    private Label label1;
    private Panel pnlBuyCurrent;
    private Label label69;
    private TextBox textBox136;
    private Label label183;
    private Label label185;
    private TextBox txtProductName;
    private TextBox txtMSRValue;
    private Label label184;
    private TextBox txtCommitmentContract;
    private DatePicker dtDeliveryExpirationDate;
    private TextBox txtDeliveryTypeEditable;
    private ComboBox cmbCommitmentType;
    private Panel panelSumSell2;
    private ComboBox comboBoxLockRate3;
    private Button btnInvestor2;
    private Label label73;
    private StandardIconButton standardIconButton2;
    private TextBox textBoxCompInvestorName;
    private Label label151;
    private GroupBox groupBox26;
    private TextBox txtInvestCommitment2;
    private Label label154;
    private Label label156;
    private TextBox txtInvestLoan1;
    private Label label186;
    private TextBox textBox28;
    private Panel pnlBuysideLock;
    private DatePicker dp3256;
    private Label label182;
    private Label label64;
    private CheckBox ckbOnrpEligible;
    private Label label181;
    private Label label178;
    private DatePicker txtBuyLockDate;
    private Label label179;
    private TextBox txtLockTime;
    private TextBox textBox27;
    private Label label4;
    private Label label3;
    private TextBox txtBuyLockDays;
    private Label label180;
    private Panel panelCompRelockA;
    private Label label245;
    private Label label246;
    private Label label247;
    private TextBox tbRelock23Desc;
    private TextBox tbRelock23;
    private TextBox tbRelock22Desc;
    private TextBox tbRelock22;
    private TextBox tbRelock21Desc;
    private TextBox tbRelock21;
    private Label label248;
    private Panel panelSellSideCPA_B;
    private Label label209;
    private Label label210;
    private Label label211;
    private Label label212;
    private Label label213;
    private Label label214;
    private Label label215;
    private TextBox tbCPA20Desc;
    private TextBox tbCPA20;
    private TextBox tbCPA19Desc;
    private TextBox tbCPA19;
    private TextBox tbCPA18Desc;
    private TextBox tbCPA18;
    private TextBox tbCPA17Desc;
    private TextBox tbCPA17;
    private TextBox tbCPA16Desc;
    private TextBox tbCPA16;
    private TextBox tbCPA15Desc;
    private TextBox tbCPA15;
    private TextBox tbCPA14Desc;
    private TextBox tbCPA14;
    private Panel panelSellSideRelockB;
    private Label label216;
    private Label label217;
    private Label label218;
    private Label label219;
    private Label label220;
    private Label label221;
    private Label label222;
    private TextBox tbRelock20Desc;
    private TextBox tbRelock20;
    private TextBox tbRelock19Desc;
    private TextBox tbRelock19;
    private TextBox tbRelock18Desc;
    private TextBox tbRelock18;
    private TextBox tbRelock17Desc;
    private TextBox tbRelock17;
    private TextBox tbRelock16Desc;
    private TextBox tbRelock16;
    private TextBox tbRelock15Desc;
    private TextBox tbRelock15;
    private TextBox tbRelock14Desc;
    private TextBox tbRelock14;
    private Panel panelSellSideRelockA;
    private Panel panelCompCPA_B;
    private Label label231;
    private Label label232;
    private Label label233;
    private Label label234;
    private Label label235;
    private Label label236;
    private Label label237;
    private TextBox tbCPA30Desc;
    private TextBox tbCPA30;
    private TextBox tbCPA29Desc;
    private TextBox tbCPA29;
    private TextBox tbCPA28Desc;
    private TextBox tbCPA28;
    private TextBox tbCPA27Desc;
    private TextBox tbCPA27;
    private TextBox tbCPA26Desc;
    private TextBox tbCPA26;
    private TextBox tbCPA25Desc;
    private TextBox tbCPA25;
    private TextBox tbCPA24Desc;
    private TextBox tbCPA24;
    private Label label223;
    private Panel panelCompRelockB;
    private Label label238;
    private Label label239;
    private Label label240;
    private Label label241;
    private Label label242;
    private Label label243;
    private Label label244;
    private TextBox tbRelock30Desc;
    private TextBox tbRelock30;
    private TextBox tbRelock29Desc;
    private TextBox tbRelock29;
    private TextBox tbRelock28Desc;
    private TextBox tbRelock28;
    private TextBox tbRelock27Desc;
    private TextBox tbRelock27;
    private TextBox tbRelock26Desc;
    private TextBox tbRelock26;
    private TextBox tbRelock25Desc;
    private TextBox tbRelock25;
    private TextBox tbRelock24Desc;
    private TextBox tbRelock24;
    private Label label224;
    private Label label225;
    private TextBox tbRelock13Desc;
    private TextBox tbRelock13;
    private TextBox tbRelock12Desc;
    private TextBox tbRelock12;
    private TextBox tbRelock11Desc;
    private TextBox tbRelock11;
    private Label label226;
    private Panel panelSellSideCPA_A;
    private Label label227;
    private Label label228;
    private Label label229;
    private TextBox tbCPA13Desc;
    private TextBox tbCPA13;
    private TextBox tbCPA12Desc;
    private TextBox tbCPA12;
    private TextBox tbCPA11Desc;
    private TextBox tbCPA11;
    private Label label230;
    private Panel panelBuySideCPA_B;
    private Label label187;
    private Label label188;
    private Label label189;
    private Label label190;
    private Label label191;
    private Label label192;
    private Label label193;
    private TextBox tbCPA10Desc;
    private TextBox tbCPA10;
    private TextBox tbCPA9Desc;
    private TextBox tbCPA9;
    private TextBox tbCPA8Desc;
    private TextBox tbCPA8;
    private TextBox tbCPA7Desc;
    private TextBox tbCPA7;
    private TextBox tbCPA6Desc;
    private TextBox tbCPA6;
    private TextBox tbCPA1Desc5;
    private TextBox tbCPA5;
    private TextBox tbCPA4Desc;
    private TextBox tbCPA4;
    private Panel panelBuySideRelockB;
    private Label label194;
    private Label label195;
    private Label label196;
    private Label label197;
    private Label label198;
    private Label label199;
    private Label label200;
    private TextBox tbRelock10Desc;
    private TextBox tbRelock10;
    private TextBox tbRelock9Desc;
    private TextBox tbRelock9;
    private TextBox tbRelock8Desc;
    private TextBox tbRelock8;
    private TextBox tbRelock7Desc;
    private TextBox tbRelock7;
    private TextBox tbRelock6Desc;
    private TextBox tbRelock6;
    private TextBox tbRelock5Desc;
    private TextBox tbRelock5;
    private TextBox tbRelock4Desc;
    private TextBox tbRelock4;
    private Panel panelBuySideRelockA;
    private Label label201;
    private Label label202;
    private Label label203;
    private TextBox tbRelock3Desc;
    private TextBox tbRelock3;
    private TextBox tbRelock2Desc;
    private TextBox tbRelock2;
    private TextBox tbRelock1Desc;
    private TextBox tbRelock1;
    private Label label204;
    private Panel panelBuySideCPA_A;
    private Label label205;
    private Label label206;
    private Label label207;
    private TextBox tbCPA3Desc;
    private TextBox tbCPA3;
    private TextBox tbCPA2Desc;
    private TextBox tbCPA2;
    private TextBox tbCPA1Desc;
    private TextBox tbCPA1;
    private Label label208;
    private PictureBox picCPAZoomOut;
    private PictureBox picCPAZoomIn;
    private PictureBox picRLZoomOut;
    private PictureBox picRLZoomIn;
    private TextBox txtboxBuySideInvestorName;
    private StandardIconButton stdIconBtnBuySideInvestor;
    private Button btnBuySideInvestor;
    private Panel panelCompCPA_A;
    private Label label249;
    private Label label250;
    private Label label251;
    private TextBox tbCPA23Desc;
    private TextBox tbCPA23;
    private TextBox tbCPA22Desc;
    private TextBox tbCPA22;
    private TextBox tbCPA21Desc;
    private TextBox tbCPA21;
    private Label label252;
    private Panel panelCompTPC;
    private Panel panelCompBPC_B;
    private Panel panelCompBPC_A;
    private Panel panelCompCPC_B;
    private Panel panelCompCPC_A;
    private Panel panelBuySideTPC;
    private Panel panelBuySideBPC_B;
    private Panel panelBuySideBPC_A;
    private Panel panelBuySideCPC_B;
    private Panel panelBuySideCPC_A;
    private Label label10;
    private TextBox textBoxPrice22;
    private Label label9;
    private TextBox textBoxPrice23;
    private Label label262;
    private Label label260;
    private Label label261;
    private Label label259;
    private StandardIconButton iconBtnEditBranchConcession5;
    private TextBox textBoxBranchConcession5;
    private StandardIconButton iconBtnEditBranchConcession4;
    private TextBox textBoxBranchConcession4;
    private StandardIconButton iconBtnEditBranchConcession3;
    private TextBox textBoxBranchConcession3;
    private StandardIconButton iconBtnEditBranchConcession2;
    private TextBox textBoxBranchConcession2;
    private PictureBox picBPCZoomOut;
    private PictureBox picBPCZoomIn;
    private Label label258;
    private StandardIconButton iconBtnEditBranchConcession;
    private Label label81;
    private TextBox textBoxBranchConcession;
    private Label label257;
    private Label label255;
    private Label label256;
    private Label label254;
    private StandardIconButton iconBtnEditCorpConcession5;
    private TextBox textBoxCorpConcession5;
    private StandardIconButton iconBtnEditCorpConcession4;
    private TextBox textBoxCorpConcession4;
    private StandardIconButton iconBtnEditCorpConcession3;
    private TextBox textBoxCorpConcession3;
    private StandardIconButton iconBtnEditCorpConcession2;
    private TextBox textBoxCorpConcession2;
    private Label label253;
    private PictureBox picCPCZoomOut;
    private PictureBox picCPCZoomIn;
    private StandardIconButton iconBtnEditCorpConcession;
    private Label label80;
    private TextBox textBoxCorpConcession;
    private Panel panelSellSideTPC;
    private Panel panelSellSideBPC_B;
    private Panel panelSellSideBPC_A;
    private Panel panelSellSideCPC_B;
    private Panel panelSellSideCPC_A;
    private Label label34;
    private TextBox textBoxPrice47;
    private Label label33;
    private TextBox textBoxPrice48;
    private Label label117;
    private TextBox textBoxPrice72;
    private Label label118;
    private TextBox textBoxPrice73;
    private Panel panelBranchTotal;
    private Label label265;
    private Label label266;
    private TextBox textBox30;
    private Panel panelCorpTotal;
    private Label label263;
    private Label label264;
    private TextBox textBox29;

    public event EventHandler ZoomButtonClicked;

    public bool IsSummary
    {
      get => this.isSummary;
      set => this.isSummary = value;
    }

    public bool IsDirty
    {
      get => this.isDirty;
      set => this.isDirty = value;
    }

    public LockForm(
      LoanDataMgr loanMgr,
      bool readOnly,
      bool hideControls,
      LoanLockTool loanLockTool,
      bool hideCurrentLockExpDateField,
      bool isSummary,
      bool isUpdateSell = false)
    {
      this.lockCalculator = new LockRequestCalculator(Session.SessionObjects, loanMgr.LoanData);
      this.use10Digits = Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas;
      this.hideControls = hideControls;
      this.readOnly = readOnly;
      this.isUpdateSell = isUpdateSell;
      this.loanMgr = loanMgr;
      this.loanLockTool = loanLockTool;
      this.hideCurrentExpDate = hideCurrentLockExpDateField;
      this.InitializeComponent();
      this.companySettings = Session.SessionObjects.GetCompanySettingsFromCache("POLICIES");
      this.lockExtensionUtils = new LockExtensionUtils(Session.SessionObjects, this.loanMgr.LoanData);
      this.UIresources = new ResourceManager(typeof (LockForm));
      this.picBMZoomOut.Left = this.picBMZoomIn.Left;
      this.picBMZoomOut.Top = this.picBMZoomIn.Top;
      this.picBPZoomOut.Left = this.picBPZoomIn.Left;
      this.picBPZoomOut.Top = this.picBPZoomIn.Top;
      this.picBRZoomOut.Left = this.picBRZoomIn.Left;
      this.picBRZoomOut.Top = this.picBRZoomIn.Top;
      this.picPRZoomOut.Left = this.picPRZoomIn.Left;
      this.picPRZoomOut.Top = this.picPRZoomIn.Top;
      this.picEXZoomOut.Left = this.picEXZoomIn.Left;
      this.picEXZoomOut.Top = this.picEXZoomIn.Top;
      this.picRLZoomOut.Left = this.picRLZoomIn.Left;
      this.picRLZoomOut.Top = this.picRLZoomIn.Top;
      this.picCPAZoomOut.Left = this.picCPAZoomIn.Left;
      this.picCPAZoomOut.Top = this.picCPAZoomIn.Top;
      this.picCPCZoomOut.Left = this.picCPCZoomIn.Left;
      this.picCPCZoomOut.Top = this.picCPCZoomIn.Top;
      this.picBPCZoomOut.Left = this.picBPCZoomIn.Left;
      this.picBPCZoomOut.Top = this.picBPCZoomIn.Top;
      ResourceManager resources = new ResourceManager(typeof (LockForm));
      this.popupRules = new PopupBusinessRules(this.loanMgr.LoanData, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), Session.DefaultInstance);
      this.popupRules.DropdownSelected += new EventHandler(this.popupRules_DropdownSelected);
      this.panelSumComp1.Visible = false;
      this.panelSumComp2.Visible = false;
      this.panelSumSell1.Visible = false;
      this.panelSumSell2.Visible = false;
      this.groupBox28.Visible = false;
      if (this.loanMgr.LoanData != null && (this.loanMgr.LoanData.GetField("2626") == "Banked - Retail" && (bool) Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"] || this.loanMgr.LoanData.GetField("2626") == "Banked - Wholesale" && (bool) Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]))
      {
        this.label57.Text = "Gain Loss $ (b + a) - 100";
        this.label58.Text = "Gain Loss % (b + a) - 100";
        this.label62.Text = "(Par pricing is 0.00)\r\n";
      }
      if (this.hideControls)
      {
        this.textBoxLoanProgram.Visible = true;
        this.labelLoanProgram.Visible = true;
        this.textBoxCompLoanProgram.Visible = true;
        this.labelCompLoanProgram.Visible = true;
        this.btnClearBuyside.Visible = false;
        this.btnClearSellSide.Visible = false;
        this.btnCopyBuy.Visible = false;
        this.btnCopyRequest.Visible = false;
        this.btnClearComp.Visible = false;
        this.btnCopyBuytoComp.Visible = false;
        this.panelLeft30.Visible = false;
        this.panelLeft40.Visible = false;
        this.panelRight30.Visible = false;
        this.panelRight40.Visible = false;
        this.panelComp30.Visible = false;
        this.panelComp40.Visible = false;
        this.panelBuySideProfitA.Visible = false;
        this.panelSellSideProfitSpacerA.Visible = false;
        this.panelBuySideProfitB.Visible = false;
        this.panelSellSideProfitSpacerB.Visible = false;
        this.panelCompProfitSpacerA.Visible = false;
        this.panelCompProfitSpacerB.Visible = false;
        this.panelLeft70.Visible = false;
        this.panelLeft80.Visible = false;
        this.panelBuySideExtensionA.Visible = false;
        this.panelBuySideExtensionB.Visible = false;
        this.panelRight70.Visible = false;
        this.panelRight80.Visible = false;
        this.panelSellSideExtensionA.Visible = false;
        this.panelSellSideExtensionB.Visible = false;
        this.panelComp70.Visible = false;
        this.panelComp80.Visible = false;
        this.panelCompExtensionA.Visible = false;
        this.panelCompExtensionB.Visible = false;
        this.panelBuySideRelockA.Visible = false;
        this.panelBuySideRelockB.Visible = false;
        this.panelSellSideRelockA.Visible = false;
        this.panelSellSideRelockB.Visible = false;
        this.panelCompRelockA.Visible = false;
        this.panelCompRelockB.Visible = false;
        this.panelBuySideCPA_A.Visible = false;
        this.panelBuySideCPA_B.Visible = false;
        this.panelSellSideCPA_A.Visible = false;
        this.panelSellSideCPA_B.Visible = false;
        this.panelCompCPA_A.Visible = false;
        this.panelCompCPA_B.Visible = false;
        this.panelBuySideCPC_B.Visible = false;
        this.panelSellSideCPC_B.Visible = false;
        this.panelCompCPC_B.Visible = false;
        this.panelCorpTotal.Visible = true;
        this.panelBuySideBPC_B.Visible = false;
        this.panelSellSideBPC_B.Visible = false;
        this.panelCompBPC_B.Visible = false;
        this.panelBranchTotal.Visible = true;
        this.panelLeft110.Visible = false;
        this.panelLeft120.Visible = false;
        this.panelRight110.Visible = false;
        this.panelRight120.Visible = false;
        this.panelComp110.Visible = false;
        this.panelComp120.Visible = false;
        this.panelLeft150.Visible = false;
        this.panelRight170.Visible = false;
        this.panelComp170.Visible = false;
        this.picBRZoomIn.Visible = false;
        this.picBPZoomIn.Visible = false;
        this.picBMZoomIn.Visible = false;
        this.picPRZoomIn.Visible = false;
        this.picEXZoomIn.Visible = false;
        this.picRLZoomIn.Visible = false;
        this.picCPAZoomIn.Visible = false;
        this.picCPCZoomIn.Visible = false;
        this.picBPCZoomIn.Visible = false;
        this.picBRZoomOut.Visible = false;
        this.picBPZoomOut.Visible = false;
        this.picBMZoomOut.Visible = false;
        this.picPRZoomOut.Visible = false;
        this.picRLZoomOut.Visible = false;
        this.picCPAZoomOut.Visible = false;
        this.picCPCZoomOut.Visible = false;
        this.picBPCZoomOut.Visible = false;
        this.groupBox16.Visible = false;
        this.LockForm_SizeChanged((object) null, (EventArgs) null);
      }
      else
      {
        this.textBoxLoanProgram.Visible = false;
        this.labelLoanProgram.Visible = false;
        object[] allSecondaryFields = Session.ConfigurationManager.GetAllSecondaryFields();
        if (allSecondaryFields != null && allSecondaryFields.Length >= 3)
        {
          if (allSecondaryFields[0] != null)
            this.rateOptions = (string[]) allSecondaryFields[0];
          if (allSecondaryFields[1] != null)
            this.priceOptions = (string[]) allSecondaryFields[1];
          if (allSecondaryFields[2] != null)
            this.marginOptions = (string[]) allSecondaryFields[2];
          if (allSecondaryFields[4] != null)
            this.profitabilityOptions = (string[]) allSecondaryFields[4];
          if (allSecondaryFields[5] != null)
            this.locktypeOptions = (string[]) allSecondaryFields[5];
        }
        this.panelTop.Visible = false;
        this.panelLeftInvestor.Visible = false;
        this.panelRightInvestor.Visible = false;
        this.panelCompInvestor.Visible = false;
        this.panelCorpTotal.Visible = false;
        this.panelBranchTotal.Visible = false;
        this.populateDropdownOptions(this.Controls);
      }
      this.popupRules.SetButtonAccessMode(this.btnTradeSummary, "Button_ViewTradeSummary");
      if (this.loanMgr.LoanData.GetField("2032") == "")
      {
        string guid = this.loanMgr.LoanData.GUID;
        ITradeHistoryItem[] tradeHistoryForLoan = (ITradeHistoryItem[]) Session.LoanTradeManager.GetTradeHistoryForLoan(guid);
        if (tradeHistoryForLoan == null || tradeHistoryForLoan.Length == 0)
          tradeHistoryForLoan = (ITradeHistoryItem[]) Session.MbsPoolManager.GetTradeHistoryForLoan(guid);
        if (tradeHistoryForLoan == null || tradeHistoryForLoan.Length == 0)
        {
          this.btnTradeSummary.Enabled = false;
          this.txtInvestorStatus.Text = "";
        }
        else
          this.txtInvestorStatus.Text = this.getLoanFieldValue("2031");
      }
      else
        this.txtInvestorStatus.Text = this.getLoanFieldValue("2031");
      this.populateFieldValues(this.Controls);
      this.checkBoxHedging.Checked = this.loanMgr.LoanData.GetField("2401") == "Y";
      if (this.loanMgr.LoanData.GetField("2626") == "Correspondent")
        this.grpCorrespondent.Visible = true;
      else
        this.grpCorrespondent.Visible = false;
      this.populateToolTips(this.Controls);
      if (this.readOnly)
      {
        this.btnCopyRequest.Enabled = false;
        this.btnCopyBuy.Enabled = false;
        this.btnCopyBuytoComp.Enabled = false;
        this.btnInvestor.Enabled = false;
        this.btnInvestorComp.Enabled = false;
        this.btnInvestor2.Enabled = false;
        this.btnInvestorComp2.Enabled = false;
        this.btnClearBuyside.Enabled = false;
        this.btnClearSellSide.Enabled = false;
        this.btnClearComp.Enabled = false;
        this.iconBtnInvestor.Enabled = false;
        this.iconButtonServicer.Enabled = false;
        this.iconBtnCompInvestor.Enabled = false;
      }
      else
        this.iconButtonServicer.Enabled = true;
      if (isUpdateSell)
      {
        this.btnCopyBuy.Enabled = true;
        this.btnCopyBuytoComp.Enabled = true;
        this.btnCopyRequest.Enabled = false;
        this.btnClearBuyside.Enabled = true;
        this.btnBuyComment.Enabled = false;
        this.btnInvestor.Enabled = true;
        this.btnInvestorComp.Enabled = true;
        this.btnInvestor2.Enabled = true;
        this.btnInvestorComp2.Enabled = true;
        this.btnClearBuyside.Enabled = false;
        this.btnClearSellSide.Enabled = true;
        this.btnClearComp.Enabled = true;
        this.iconBtnInvestor.Enabled = true;
        this.iconButtonServicer.Enabled = true;
        this.iconBtnCompInvestor.Enabled = true;
      }
      this.popupRules.SetButtonAccessMode(this.btnInvestor, "Button_Investoronsecondary");
      this.iconBtnInvestor.Enabled = this.btnInvestor.Enabled;
      this.iconBtnInvestor.Visible = this.btnInvestor.Visible;
      this.popupRules.SetButtonAccessMode(this.btnCopyRequest, "Button_copyfromrequest");
      this.popupRules.SetButtonAccessMode(this.btnCopyBuy, "Button_copyfrombuyside");
      this.popupRules.SetButtonAccessMode(this.btnCopyBuytoComp, "Button_copyfrombuytocomp");
      this.popupRules.SetButtonAccessMode(this.btnClearBuyside, "Button_clearbuyside");
      this.popupRules.SetButtonAccessMode(this.btnClearSellSide, "Button_clearsellside");
      this.popupRules.SetButtonAccessMode(this.btnClearComp, "Button_clearcompside");
      this.popupRules.SetButtonAccessMode(this.btnBuyComment, "Button_addbuysidecomments");
      this.popupRules.SetButtonAccessMode(this.btnSellComment, "Button_addsellsidecomments");
      this.popupRules.SetButtonAccessMode(this.btnCompComment, "Button_addcompsidecomments");
      this.toolTipField.SetToolTip((Control) this.checkBoxHedging, "2401");
      this.isFirstLoading = false;
      this.hideBR = !(this.getField("2454") != "") && this.getNumField("2455") == 0.0 && !(this.getField("2488") != "") && this.getNumField("2489") == 0.0;
      this.hideBP = !(this.getField("2182") != "") && this.getNumField("2183") == 0.0 && !(this.getField("2253") != "") && this.getNumField("2254") == 0.0;
      this.hideEX = !(this.getField("3480") != "") && this.getNumField("3481") == 0.0;
      this.hideRL = !(this.getField("4282") != "") && this.getNumField("4283") == 0.0;
      this.hideCPA = !(this.getField("4362") != "") && this.getNumField("4363") == 0.0;
      this.hideCPC = !(this.getField("4753") != "");
      this.hideBPC = !(this.getField("4769") != "");
      this.hideBM = !(this.getField("2742") != "") && this.getNumField("2743") == 0.0 && !(this.getField("2450") != "") && this.getNumField("2451") == 0.0;
      this.hidePR = !(this.getField("3388") != "") && this.getNumField("3389") == 0.0;
      if (hideCurrentLockExpDateField)
      {
        this.pnlBuyCurrent.Visible = false;
        this.pnlBuySideLockExtension.Size = new Size(this.pnlBuySideLockExtension.Width, this.pnlBuySideLockExtension.Height - this.pnlBuySideLockExpDate.Height);
        this.pnlBuysideLock.Top = this.pnlBuySideRequest.Bottom;
        this.pnlBuySideLockExtension.Top = this.pnlBuysideLock.Bottom;
        this.pnlBuySideInfo.Size = new Size(this.pnlBuySideInfo.Width, this.pnlBuySideLockExtension.Height + this.pnlBuySideRequest.Height + this.pnlBuysideLock.Height);
        this.pnlSellSideInfo.Size = new Size(this.pnlSellSideInfo.Width, this.pnlBuySideInfo.Height);
        this.pnlCompInfo.Size = new Size(this.pnlCompInfo.Width, this.pnlBuySideInfo.Height);
      }
      else
      {
        this.pnlBuySideInfo.Size = new Size(this.pnlBuySideInfo.Width, this.pnlBuySideInfo.Height + this.additionalHeight);
        this.pnlSellSideInfo.Size = new Size(this.pnlSellSideInfo.Width, this.pnlBuySideInfo.Height + this.additionalHeight);
        this.pnlCompInfo.Size = new Size(this.pnlCompInfo.Width, this.pnlBuySideInfo.Height + this.additionalHeight);
      }
      if (this.lrl == null)
      {
        this.editBtn.Enabled = false;
        this.pnlBuySideLockExpDate.Visible = true;
        this.pnlBuySideLockExtension.Visible = false;
      }
      this.zoomInAndOut();
      this.isDirty = false;
    }

    private void popupRules_DropdownSelected(object sender, EventArgs e)
    {
      switch (sender)
      {
        case ComboBox _:
          ComboBox comboBox = (ComboBox) sender;
          this.setField(comboBox.Tag.ToString(), comboBox.Text);
          break;
        case TextBox _:
          TextBox textBox = (TextBox) sender;
          this.setField(textBox.Tag.ToString(), textBox.Text);
          break;
      }
    }

    internal void DisableBuySideLockandPricingFields()
    {
      this.txtBuyLockDate.Enabled = false;
      this.txtBuyLockDays.Enabled = false;
      this.pnlBuySideLockExpDate.Enabled = false;
      this.panel1.Enabled = false;
      this.panelLeft20.Enabled = this.panelLeft30.Enabled = this.panelLeft40.Enabled = this.panelLeft50.Enabled = false;
      this.panelBuySideProfitA.Enabled = this.panelBuySideProfitB.Enabled = false;
      this.panelLeft60.Enabled = this.panelLeft70.Enabled = this.panelLeft80.Enabled = false;
      this.panelLeft100.Enabled = this.panelLeft110.Enabled = this.panelLeft120.Enabled = this.panelLeft130.Enabled = false;
    }

    internal void RefreshScreen(Hashtable dataTables, LockRequestLog lrl)
    {
      this.lrl = lrl;
      this.dataTables = dataTables;
      if (this.dataTables != null && (!this.readOnly || this.isUpdateSell))
      {
        if (dataTables[(object) "3902"] != null && dataTables[(object) "3902"].ToString() != "" && dataTables[(object) "3911"] != null && !dataTables[(object) "3911"].ToString().Contains("Individual") && this.loanMgr.LoanData.GetField("3915") != "" && !this.loanMgr.LoanData.GetField("3911").Contains("Individual"))
          this.lockCalculator.PerformSnapshotCalculations(dataTables, true, false, false, false);
        else
          this.lockCalculator.PerformSnapshotCalculations(dataTables);
      }
      this.txtCommitmentType.Tag = (object) "3910";
      this.cmbCommitmentType.Tag = (object) "3910";
      this.txtDeliveryExpireDate.Tag = (object) "3913";
      this.dtDeliveryExpirationDate.Tag = (object) "3913";
      this.txtDeliveryType.Tag = (object) "3911";
      this.txtDeliveryTypeEditable.Tag = (object) "3911";
      this.populateFieldValues(this.Controls);
      this.txtInvestorStatus.Text = this.getLoanFieldValue("2031");
      if (this.readOnly)
      {
        this.toolTipField.RemoveAll();
        this.populateToolTips(this.Controls);
      }
      if (this.lrl == null)
      {
        this.editBtn.Enabled = false;
        this.pnlBuySideLockExpDate.Visible = true;
        this.pnlBuySideLockExtension.Visible = false;
        this.pnlBuySideInfo.Size = new Size(this.pnlBuySideInfo.Width, 143 + this.pnlBuySideLockExpDate.Height + this.additionalHeight);
        this.pnlSellSideInfo.Size = new Size(this.pnlSellSideInfo.Width, this.pnlBuySideLockExtension.Height);
      }
      else
      {
        this.editBtn.Enabled = true;
        string str = dataTables == null || !dataTables.ContainsKey((object) "4209") ? "" : dataTables[(object) "4209"].ToString();
        bool isCancelOrExpired = str == "Cancelled Lock" || str == "Expired Lock";
        if (this.lrl.IsLockExtension || this.lrl.IsLockCancellation && this.dataTables != null && this.dataTables[(object) "3363"] != null && !string.IsNullOrEmpty(this.dataTables[(object) "3363"].ToString()))
        {
          if (this.lrl.RateLockAction == RateLockAction.TradeExtension || this.lrl.ReviseAction == RateLockAction.TradeExtension)
            this.txtBuySideRequestedType.Text = this.txtSellSideRequestedType.Text = this.txtCompRequestedType.Text = "Trade Extension";
          else
            this.txtBuySideRequestedType.Text = this.txtSellSideRequestedType.Text = this.txtCompRequestedType.Text = "Extension";
          this.pnlBuySideLockExpDate.Visible = false;
          this.pnlBuySideLockExtension.Visible = true;
          this.pnlBuySideInfo.Size = new Size(this.pnlBuySideInfo.Width, 143 + this.pnlBuySideLockExtension.Height + this.additionalHeight);
          this.txt2148.ReadOnly = this.dp3256.ReadOnly = this.txtBuyLockDate.ReadOnly = this.txtBuyLockDays.ReadOnly = true;
          this.txt2148.Enabled = this.dp3256.Enabled = this.txtBuyLockDate.Enabled = this.txtBuyLockDays.Enabled = false;
        }
        else
        {
          this.txtBuySideRequestedType.Text = this.txtSellSideRequestedType.Text = this.txtCompRequestedType.Text = "Lock";
          this.pnlBuySideLockExpDate.Visible = true;
          this.pnlBuySideLockExtension.Visible = false;
          this.pnlBuySideInfo.Size = new Size(this.pnlBuySideInfo.Width, 143 + this.pnlBuySideLockExpDate.Height + this.additionalHeight);
          this.pnlSellSideInfo.Size = new Size(this.pnlSellSideInfo.Width, this.pnlBuySideLockExtension.Height);
          this.txt2148.ReadOnly = this.dp3256.ReadOnly = this.txtBuyLockDate.ReadOnly = this.txtBuyLockDays.ReadOnly = false;
          this.txt2148.Enabled = this.dp3256.Enabled = this.txtBuyLockDate.Enabled = this.txtBuyLockDays.Enabled = true;
        }
        if (this.lrl.RateLockAction != RateLockAction.TradeExtension && this.lrl.IsRelock | isCancelOrExpired)
          this.txtBuySideRequestedType.Text = this.txtSellSideRequestedType.Text = this.txtCompRequestedType.Text = LockUtils.GetRelockRequestType(lrl.IsRelock, isCancelOrExpired);
        if (this.lrl.IsLockCancellation)
          this.txtBuySideRequestedType.Text = this.txtSellSideRequestedType.Text = this.txtCompRequestedType.Text = "Cancellation";
        this.pnlCompInfo.Size = new Size(this.pnlCompInfo.Width, this.pnlBuySideInfo.Height);
        this.pnlSellSideInfo.Size = new Size(this.pnlSellSideInfo.Width, this.pnlBuySideInfo.Height);
      }
      if (this.readOnly)
      {
        this.txtBuyComments.ReadOnly = false;
        this.txtBuyComments.BackColor = Color.WhiteSmoke;
        if (!this.isUpdateSell)
        {
          this.txtSellComments.ReadOnly = false;
          this.txtSellComments.BackColor = Color.WhiteSmoke;
          this.txtCompComments.ReadOnly = false;
          this.txtCompComments.BackColor = Color.WhiteSmoke;
        }
      }
      if (!this.readOnly)
      {
        bool flag = !string.IsNullOrEmpty(this.txtCommitmentNo.Text.ToString());
        this.cmbCommitmentType.Visible = !flag;
        this.txtCommitmentType.Visible = flag;
        this.dtDeliveryExpirationDate.Visible = !flag;
        this.txtDeliveryExpireDate.Visible = flag;
        this.txtDeliveryTypeEditable.Visible = !flag;
        this.txtDeliveryType.Visible = flag;
        if (!flag)
        {
          this.txtCommitmentType.Tag = (object) "";
          this.txtDeliveryType.Tag = (object) "";
          this.txtDeliveryExpireDate.Tag = (object) "";
          this.cmbCommitmentType.Enabled = true;
          this.txtDeliveryTypeEditable.Enabled = true;
          this.dtDeliveryExpirationDate.Enabled = true;
        }
        else
        {
          this.cmbCommitmentType.Tag = (object) "";
          this.txtDeliveryTypeEditable.Tag = (object) "";
          this.dtDeliveryExpirationDate.Tag = (object) "";
        }
      }
      if (!this.hideControls)
      {
        this.panelLeftInvestor.Visible = this.panelRightInvestor.Visible = this.panelCompInvestor.Visible = false;
        this.LockFormView();
        this.LockForm_SizeChanged((object) null, (EventArgs) null);
      }
      this.ckbOnrpEligible.Checked = dataTables != null && dataTables[(object) "4059"] != null && dataTables[(object) "4059"].ToString() == "Y";
      this.checkBoxONRPLock.Checked = this.getLoanFieldValue("4057") == "Y";
      this.isDirty = false;
    }

    private void populateFieldValues(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
          case DatePicker _:
            if (c.Tag != null)
            {
              string str1 = c.Tag.ToString();
              if (!(str1 == string.Empty))
              {
                c.Text = this.defaultValue(str1);
                if (c is TextBox && (this.readOnly && !this.isUpdateSell || this.isUpdateSell && (LockRequestLog.BuySideFields.Contains(str1) || str1 == "3363" || str1 == "3365")))
                {
                  ((TextBoxBase) c).ReadOnly = true;
                  c.BackColor = Color.WhiteSmoke;
                }
                string str2 = string.Empty;
                if (this.dataTables != null && this.dataTables.ContainsKey((object) str1))
                {
                  if (LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(str1))
                  {
                    if (this.dataTables[(object) str1].ToString().Trim() == string.Empty)
                    {
                      str2 = "";
                    }
                    else
                    {
                      Decimal num = Utils.ParseDecimal((object) this.dataTables[(object) str1].ToString(), false, 10);
                      str2 = !Session.SessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas ? num.ToString("N3") : num.ToString("N10");
                    }
                  }
                  else
                    str2 = this.dataTables[(object) str1].ToString();
                }
                switch (c)
                {
                  case TextBox _:
                    TextBox ctrl1 = (TextBox) c;
                    ctrl1.Text = str2;
                    if (this.isFirstLoading)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl1, str1);
                      continue;
                    }
                    continue;
                  case ComboBox _:
                    ComboBox ctrl2 = (ComboBox) c;
                    ctrl2.Text = str2;
                    ctrl2.MouseWheel += new MouseEventHandler(this.Block_MouseWheel);
                    if (this.isFirstLoading)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl2, str1);
                      continue;
                    }
                    continue;
                  case DatePicker _:
                    DatePicker ctrl3 = (DatePicker) c;
                    ctrl3.Text = str2;
                    if (this.isFirstLoading)
                    {
                      this.popupRules.SetBusinessRules((object) ctrl3, str1);
                      continue;
                    }
                    continue;
                  default:
                    continue;
                }
              }
              else
                continue;
            }
            else
              continue;
          default:
            this.populateFieldValues(c.Controls);
            continue;
        }
      }
    }

    private void Block_MouseWheel(object sender, MouseEventArgs e)
    {
      ((HandledMouseEventArgs) e).Handled = true;
    }

    private string defaultValue(string id)
    {
      return id == "2287" || id == "3832" || id == "2288" || id == "2286" || id == "2232" || id == "3714" ? this.getField(id) : string.Empty;
    }

    private string getLoanFieldValue(string id)
    {
      return Session.LoanData.GetFieldDefinition(id).ToDisplayValue(Session.LoanData.GetField(id));
    }

    private void populateDropdownOptions(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        if (c is ComboBox)
        {
          ComboBox comboBox = (ComboBox) c;
          if (comboBox != null)
          {
            string name = comboBox.Name;
            if (name.StartsWith("comboBoxRate") && this.rateOptions != null)
              comboBox.Items.AddRange((object[]) this.rateOptions);
            else if (name.StartsWith("comboBoxMargin") && this.marginOptions != null)
              comboBox.Items.AddRange((object[]) this.marginOptions);
            else if (name.StartsWith("comboBoxProfitMargin"))
              comboBox.Items.AddRange((object[]) this.profitabilityOptions);
            else if (name.StartsWith("comboBoxServicingType"))
              comboBox.Items.AddRange((object[]) new string[3]
              {
                "",
                "Service Retained",
                "Service Released"
              });
            else if (name.StartsWith("comboBoxLockRate"))
              comboBox.Items.AddRange((object[]) this.locktypeOptions);
            else if (name.StartsWith("cmbCommitmentType"))
              comboBox.Items.AddRange((object[]) new string[3]
              {
                "",
                "Mandatory",
                "Best Efforts"
              });
            else if (this.priceOptions != null)
              comboBox.Items.AddRange((object[]) this.priceOptions);
          }
        }
        else
          this.populateDropdownOptions(c.Controls);
      }
    }

    private void editBtn_Click(object sender, EventArgs e)
    {
      if (BuySellForm.IsFormDisplayed)
      {
        if (BuySellForm.Instance.WindowState == FormWindowState.Minimized)
          BuySellForm.Instance.WindowState = FormWindowState.Normal;
        int num = (int) Utils.Dialog((IWin32Window) this, "To view Current Lock, please close Secondary Loan Lock Tool Window first.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
        this.loanLockTool.popUpCurrentLock();
    }

    private void displayFieldId(string fieldId)
    {
      Session.Application.GetService<IStatusDisplay>().DisplayFieldID(fieldId);
    }

    private void textField_Enter(object sender, EventArgs e)
    {
      Control control = (Control) sender;
      if (control == null)
        return;
      if (control.Tag == null)
        this.displayFieldId(string.Empty);
      else
        this.displayFieldId(control.Tag.ToString());
    }

    private void comboBoxField_Enter(object sender, EventArgs e)
    {
      ComboBox comboBox = (ComboBox) sender;
      if (comboBox == null || comboBox.Tag == null)
        return;
      this.displayFieldId(comboBox.Tag.ToString());
    }

    public Hashtable ReadFieldValues()
    {
      Hashtable newData = new Hashtable();
      this.readFieldValues(this.Controls, newData);
      this.setField("2029", Session.UserInfo.FullName);
      this.setField("2030", Session.UserInfo.FullName);
      return newData;
    }

    public Hashtable SetFieldValues(Hashtable newData)
    {
      if (newData == null)
        newData = new Hashtable();
      string dataTable1 = (string) this.dataTables[(object) "2866"];
      string dataTable2 = (string) this.dataTables[(object) "2853"];
      string dataTable3 = (string) this.dataTables[(object) "3873"];
      string dataTable4 = (string) this.dataTables[(object) "3875"];
      this.dataTables = newData;
      this.readFieldValues(this.Controls, this.dataTables);
      this.setField("2029", Session.UserInfo.FullName);
      this.setField("2030", Session.UserInfo.FullName);
      if (dataTable1 != null)
        this.setField("2866", dataTable1);
      if (dataTable2 != null)
        this.setField("VASUMM.X23", dataTable2);
      if (dataTable3 != null)
        this.setField("3873", dataTable3);
      if (dataTable4 != null)
        this.setField("3875", dataTable4);
      return this.dataTables;
    }

    private void readFieldValues(Control.ControlCollection cs, Hashtable newData)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox = (TextBox) c;
            if (textBox.Tag != null)
            {
              string key = textBox.Tag.ToString();
              if ((textBox.Visible || !(key == "2222") && !(key == "2151")) && !(key == string.Empty))
              {
                if (newData.ContainsKey((object) key))
                {
                  newData[(object) key] = (object) textBox.Text.Trim();
                  continue;
                }
                newData.Add((object) key, (object) textBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            if (comboBox.Tag != null)
            {
              string key = comboBox.Tag.ToString();
              if (!(key == string.Empty))
              {
                if (newData.ContainsKey((object) key))
                {
                  newData[(object) key] = (object) comboBox.Text.Trim();
                  continue;
                }
                newData.Add((object) key, (object) comboBox.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          case DatePicker _:
            DatePicker datePicker = (DatePicker) c;
            if (datePicker.Tag != null)
            {
              string key = datePicker.Tag.ToString();
              if (!(key == string.Empty))
              {
                if (newData.ContainsKey((object) key))
                {
                  newData[(object) key] = (object) datePicker.Text.Trim();
                  continue;
                }
                newData.Add((object) key, (object) datePicker.Text.Trim());
                continue;
              }
              continue;
            }
            continue;
          default:
            this.readFieldValues(c.Controls, newData);
            continue;
        }
      }
    }

    private void numField_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void zipcodeField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.ZIPCODE);
    }

    private void stateField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.STATE);
    }

    private void numField_KeyUp(object sender, KeyEventArgs e)
    {
      if (LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(string.Concat(((Control) sender).Tag)))
      {
        if (this.use10Digits)
          this.formatFieldValue(sender, FieldFormat.DECIMAL_10);
        else
          this.formatFieldValue(sender, FieldFormat.DECIMAL_3);
      }
      else
        this.formatFieldValue(sender, FieldFormat.DECIMAL_3);
    }

    private void numFieldDecimal4_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.DECIMAL_4);
    }

    private void phoneField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.PHONE);
    }

    private void formatFieldValue(object sender, FieldFormat format)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void populateToolTips(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
            TextBox textBox1 = (TextBox) c;
            if (textBox1 != null && textBox1.Tag != null)
            {
              string caption = textBox1.Tag.ToString();
              if (!(caption == string.Empty))
              {
                if (textBox1.Text.Trim().ToString() != string.Empty && textBox1.Name.StartsWith("COMBOFIELD"))
                  caption = caption + "\r\n" + textBox1.Text.ToString();
                this.toolTipField.SetToolTip((Control) textBox1, caption);
                continue;
              }
              continue;
            }
            continue;
          case ComboBox _:
            ComboBox comboBox = (ComboBox) c;
            if (comboBox != null && comboBox.Tag != null)
            {
              string caption = comboBox.Tag.ToString();
              if (!(caption == string.Empty))
              {
                if (this.readOnly && !this.isUpdateSell || this.isUpdateSell && LockRequestLog.BuySideFields.Contains(caption))
                {
                  TextBox textBox2 = new TextBox();
                  textBox2.Name = "COMBOFIELD" + comboBox.Tag.ToString();
                  textBox2.Top = comboBox.Top;
                  textBox2.Left = comboBox.Left;
                  textBox2.Width = comboBox.Width;
                  textBox2.Height = comboBox.Height;
                  textBox2.Text = comboBox.Text.ToString();
                  textBox2.Tag = comboBox.Tag;
                  textBox2.ReadOnly = true;
                  textBox2.BackColor = Color.WhiteSmoke;
                  textBox2.TabStop = false;
                  textBox2.BringToFront();
                  textBox2.TabIndex = comboBox.TabIndex;
                  textBox2.Enter += new EventHandler(this.textField_Enter);
                  textBox2.Leave += new EventHandler(this.strField_Leave);
                  comboBox.Parent.Controls.Add((Control) textBox2);
                  this.toolTipField.SetToolTip((Control) textBox2, caption);
                  comboBox.Visible = false;
                  continue;
                }
                this.toolTipField.SetToolTip((Control) comboBox, caption);
                continue;
              }
              continue;
            }
            continue;
          case DatePicker _:
            DatePicker datePicker = (DatePicker) c;
            if (datePicker != null && datePicker.Tag != null)
            {
              string caption = datePicker.Tag.ToString();
              if (!(caption == string.Empty))
              {
                if (this.readOnly && !this.isUpdateSell || this.isUpdateSell && LockRequestLog.BuySideFields.Contains(caption))
                {
                  TextBox textBox3 = new TextBox();
                  textBox3.Name = "COMBOFIELD" + datePicker.Tag.ToString();
                  textBox3.Top = datePicker.Top;
                  textBox3.Left = datePicker.Left;
                  textBox3.Width = datePicker.Width;
                  textBox3.Height = datePicker.Height;
                  textBox3.Text = datePicker.Text.ToString();
                  textBox3.Tag = datePicker.Tag;
                  textBox3.ReadOnly = true;
                  textBox3.BackColor = Color.WhiteSmoke;
                  textBox3.TabStop = false;
                  textBox3.BringToFront();
                  textBox3.TabIndex = datePicker.TabIndex;
                  textBox3.Enter += new EventHandler(this.textField_Enter);
                  textBox3.Leave += new EventHandler(this.strField_Leave);
                  datePicker.Parent.Controls.Add((Control) textBox3);
                  this.toolTipField.SetToolTip((Control) textBox3, caption);
                  datePicker.Visible = false;
                  continue;
                }
                this.toolTipField.SetToolTip((Control) datePicker, caption);
                continue;
              }
              continue;
            }
            continue;
          default:
            this.populateToolTips(c.Controls);
            continue;
        }
      }
    }

    private void checkBoxHedging_CheckedChanged(object sender, EventArgs e)
    {
      if (this.checkBoxHedging.Checked)
        this.loanMgr.LoanData.SetField("2401", "Y");
      else
        this.loanMgr.LoanData.SetField("2401", "N");
    }

    private void numField_Leave(object sender, EventArgs e)
    {
      TextBox ctrl = (TextBox) sender;
      if (ctrl.Tag.ToString() + string.Empty == string.Empty)
        return;
      string str = ctrl.Tag.ToString();
      if (this.readOnly && !this.isUpdateSell || this.isUpdateSell && LockRequestLog.BuySideFields.Contains(str))
        return;
      this.popupRules.RuleValidate((object) ctrl, str);
      if (ctrl.Name.StartsWith("textBoxRate") || ctrl.Name.StartsWith("textBoxPrice") || ctrl.Name.StartsWith("textBoxMargin") || ctrl.Name.StartsWith("textBoxExtension") && !ctrl.Name.Contains("Desc") || ctrl.Name.StartsWith("tbRelock") && !ctrl.Name.Contains("Desc") || ctrl.Name.StartsWith("tbCPA") && !ctrl.Name.Contains("Desc") || ctrl.Name.StartsWith("textBoxCorpConcession") || ctrl.Name.StartsWith("textBoxBranchConcession") || str == "2205" || str == "2276" || str == "2277")
      {
        double num = Utils.ParseDouble((object) ctrl.Text.Trim());
        if (ctrl.Text.Trim() != string.Empty)
        {
          if (LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(str))
          {
            if (this.use10Digits)
              ctrl.Text = num.ToString("N10");
            else
              ctrl.Text = num.ToString("N3");
          }
          else
            ctrl.Text = num.ToString("N3");
        }
      }
      this.setField(str, ctrl.Text.Trim());
      this.RefreshScreen(this.dataTables, this.lrl);
    }

    private void calculateLockExtension(object sender)
    {
      string field = this.getField("2151");
      if (sender == this.txtBuySideDaysToExtend)
      {
        if (Utils.IsInt((object) this.getField("3363")))
        {
          int daysToExtend = Utils.ParseInt((object) this.getField("3363"));
          this.setField("3364", this.getExpirationDate(field, daysToExtend).ToString("MM/dd/yyyy"));
          Decimal priceAdjustment = this.getPriceAdjustment(daysToExtend, (Decimal) Utils.ToDouble(this.getField("3365")), this.getField("2151"));
          this.setField("3365", priceAdjustment == 0M ? "" : priceAdjustment.ToString("N3"));
        }
        else
        {
          this.setField("3364", "");
          this.setField("3365", "");
        }
      }
      else if (sender == this.txtBuySideExtendedLockExpires)
      {
        int days = (Utils.ToDate(this.getField("3364")) - Utils.ToDate(field)).Days;
        this.setField("3363", days.ToString());
        this.setField("3364", this.getExpirationDate(field, days).ToString("MM/dd/yyyy"));
        Decimal priceAdjustment = this.getPriceAdjustment(days, (Decimal) Utils.ToDouble(this.getField("3365")), this.getField("2151"));
        this.setField("3365", priceAdjustment == 0M ? "" : priceAdjustment.ToString("N3"));
      }
      else
      {
        TextBox lockExtendPriceAdj = this.txtBuySideLockExtendPriceAdj;
      }
    }

    private double getNumField(string id)
    {
      return this.dataTables == null || !this.dataTables.ContainsKey((object) id) ? 0.0 : Utils.ParseDouble(this.dataTables[(object) id]);
    }

    private void setField(string id, string val)
    {
      if (this.dataTables == null)
        return;
      if (!this.dataTables.ContainsKey((object) id))
        this.dataTables.Add((object) id, (object) val);
      else
        this.dataTables[(object) id] = (object) val;
      this.isDirty = false;
    }

    public string getField(string id)
    {
      return this.dataTables == null || !this.dataTables.ContainsKey((object) id) ? "" : this.dataTables[(object) id].ToString();
    }

    private void strField_Leave(object sender, EventArgs e)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (this.readOnly && !this.isUpdateSell || this.isUpdateSell && LockRequestLog.BuySideFields.Contains(empty1))
        return;
      string tag;
      string val;
      switch (sender)
      {
        case TextBox _:
          TextBox ctrl1 = (TextBox) sender;
          tag = ctrl1.Tag.ToString();
          this.popupRules.RuleValidate((object) ctrl1, tag);
          val = ctrl1.Text.Trim();
          if (ctrl1.Name == "txtBuyLockDate")
          {
            if (this.txtBuyLockDate.Text != this.getField("2149"))
              this.calculateLockDate("761", this.txtBuyExpireDate, this.txtBuyLockDate, this.txtBuyLockDays);
          }
          else if (ctrl1.Name == "txtBuyExpireDate")
          {
            if (this.txtBuyExpireDate.Text != this.getField("2151"))
            {
              this.calculateLockDate("762", this.txtBuyExpireDate, this.txtBuyLockDate, this.txtBuyLockDays);
              val = ctrl1.Text.Trim();
            }
          }
          else if (ctrl1.Name == "txtBuyLockDays")
          {
            if (this.txtBuyLockDays.Text != this.getField("2150"))
              this.calculateLockDate("432", this.txtBuyExpireDate, this.txtBuyLockDate, this.txtBuyLockDays);
          }
          else if (ctrl1.Name == "txtSellLockDate")
          {
            if (this.txtSellLockDate.Text != this.getField("2220"))
              this.calculateLockDate("761", this.txtSellExpireDate, this.txtSellLockDate, this.txtSellLockDays);
          }
          else if (ctrl1.Name == "txtSellExpireDate")
          {
            if (this.txtSellExpireDate.Text != this.getField("2222"))
            {
              this.calculateLockDate("762", this.txtSellExpireDate, this.txtSellLockDate, this.txtSellLockDays);
              val = ctrl1.Text.Trim();
            }
          }
          else if (ctrl1.Name == "txtSellLockDays")
          {
            if (this.txtSellLockDays.Text != this.getField("2221"))
              this.calculateLockDate("432", this.txtSellExpireDate, this.txtSellLockDate, this.txtSellLockDays);
          }
          else if (ctrl1.Name == "txtCompLockDate")
          {
            if (this.txtCompLockDate.Text != this.getField("3664"))
              this.calculateLockDate("761", this.txtCompExpireDate, this.txtCompLockDate, this.txtCompLockDays);
          }
          else if (ctrl1.Name == "txtCompExpireDate")
          {
            if (this.txtCompExpireDate.Text != this.getField("3666"))
            {
              this.calculateLockDate("762", this.txtCompExpireDate, this.txtCompLockDate, this.txtCompLockDays);
              val = ctrl1.Text.Trim();
            }
          }
          else if (ctrl1.Name == "txtCompLockDays" && this.txtCompLockDays.Text != this.getField("3665"))
            this.calculateLockDate("432", this.txtCompExpireDate, this.txtCompLockDate, this.txtCompLockDays);
          if (ctrl1.Name == "txtBuyLockDate" || ctrl1.Name == "txtBuyExpireDate" || ctrl1.Name == "txtBuyLockDays")
          {
            this.setField("2149", this.txtBuyLockDate.Text.Trim());
            this.setField("2150", this.txtBuyLockDays.Text.Trim());
            this.setField("2151", this.txtBuyExpireDate.Text.Trim());
            break;
          }
          if (ctrl1.Name == "txtSellLockDate" || ctrl1.Name == "txtSellExpireDate" || ctrl1.Name == "txtSellLockDays")
          {
            this.setField("2220", this.txtSellLockDate.Text.Trim());
            this.setField("2221", this.txtSellLockDays.Text.Trim());
            this.setField("2222", this.txtSellExpireDate.Text.Trim());
            break;
          }
          if (ctrl1.Name == "txtCompLockDate" || ctrl1.Name == "txtCompExpireDate" || ctrl1.Name == "txtCompLockDays")
          {
            this.setField("3664", this.txtCompLockDate.Text.Trim());
            this.setField("3665", this.txtCompLockDays.Text.Trim());
            this.setField("3666", this.txtCompExpireDate.Text.Trim());
            break;
          }
          if (ctrl1.Name == "textBoxZip" && ctrl1.Text.Trim().Length >= 5)
          {
            ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(ctrl1.Text.Trim().Substring(0, 5), ZipCodeUtils.GetMultipleZipInfoAt(ctrl1.Text.Trim().Substring(0, 5)));
            if (zipCodeInfo != null)
            {
              this.textBoxCity.Text = Utils.CapsConvert(zipCodeInfo.City, false);
              this.setField("2282", this.textBoxCity.Text);
              this.textBoxState.Text = zipCodeInfo.State;
              this.setField("2283", this.textBoxState.Text);
              break;
            }
            break;
          }
          break;
        case ComboBox _:
          ComboBox ctrl2 = (ComboBox) sender;
          tag = ctrl2.Tag.ToString();
          this.popupRules.RuleValidate((object) ctrl2, tag);
          val = ctrl2.Text.Trim();
          break;
        case DatePicker _:
          DatePicker ctrl3 = (DatePicker) sender;
          if (ctrl3 == null)
            return;
          tag = (string) ctrl3.Tag;
          if ((tag ?? "") == "")
            return;
          this.popupRules.RuleValidate((object) ctrl3, tag);
          val = ctrl3.Text.Trim();
          if (ctrl3.Name == "txtBuyLockDate")
          {
            if (this.txtBuyLockDate.Text != this.getField("2149"))
              this.calculateLockDate("761", this.txtBuyExpireDate, this.txtBuyLockDate, this.txtBuyLockDays);
          }
          else if (ctrl3.Name == "txtBuyExpireDate")
          {
            if (this.txtBuyExpireDate.Text != this.getField("2151"))
            {
              this.calculateLockDate("762", this.txtBuyExpireDate, this.txtBuyLockDate, this.txtBuyLockDays);
              val = ctrl3.Text.Trim();
            }
          }
          else if (ctrl3.Name == "txtSellLockDate")
          {
            if (this.txtSellLockDate.Text != this.getField("2220"))
              this.calculateLockDate("761", this.txtSellExpireDate, this.txtSellLockDate, this.txtSellLockDays);
          }
          else if (ctrl3.Name == "txtSellExpireDate")
          {
            if (this.txtSellExpireDate.Text != this.getField("2222"))
            {
              this.calculateLockDate("762", this.txtSellExpireDate, this.txtSellLockDate, this.txtSellLockDays);
              val = ctrl3.Text.Trim();
            }
          }
          else if (ctrl3.Name == "txtCompLockDate")
          {
            if (this.txtSellLockDate.Text != this.getField("3664"))
              this.calculateLockDate("761", this.txtCompExpireDate, this.txtCompLockDate, this.txtCompLockDays);
          }
          else if (ctrl3.Name == "txtCompExpireDate" && this.txtCompExpireDate.Text != this.getField("3666"))
          {
            this.calculateLockDate("762", this.txtCompExpireDate, this.txtCompLockDate, this.txtCompLockDays);
            val = ctrl3.Text.Trim();
          }
          if (ctrl3.Name == "txtBuyLockDate" || ctrl3.Name == "txtBuyExpireDate")
          {
            this.setField("2149", this.txtBuyLockDate.Text.Trim());
            this.setField("2150", this.txtBuyLockDays.Text.Trim());
            this.setField("2151", this.txtBuyExpireDate.Text.Trim());
            break;
          }
          if (ctrl3.Name == "txtSellLockDate" || ctrl3.Name == "txtSellExpireDate")
          {
            this.setField("2220", this.txtSellLockDate.Text.Trim());
            this.setField("2221", this.txtSellLockDays.Text.Trim());
            this.setField("2222", this.txtSellExpireDate.Text.Trim());
            break;
          }
          if (ctrl3.Name == "txtCompLockDate" || ctrl3.Name == "txtCompExpireDate")
          {
            this.setField("3664", this.txtCompLockDate.Text.Trim());
            this.setField("3665", this.txtCompLockDays.Text.Trim());
            this.setField("3666", this.txtCompExpireDate.Text.Trim());
            break;
          }
          break;
        default:
          return;
      }
      this.setField(tag, val);
    }

    private void DisableBuyLockDateAndDays()
    {
      ProductPricingSetting productPricingPartner = Session.StartupInfo.ProductPricingPartner;
      if (productPricingPartner == null || LockUtils.IfShipDark(Session.SessionObjects, "EPPS_EPC2_SHIP_DARK_SR") || productPricingPartner.VendorPlatform != VendorPlatform.EPC2 || !productPricingPartner.IsEPPS)
        return;
      this.txtBuyLockDate.Enabled = false;
      this.txtBuyLockDays.Enabled = false;
    }

    private void btnCopyRequest_Click(object sender, EventArgs e)
    {
      LockUtils.CopySnapshotRequestSideToBuySide(this.dataTables, Session.UserInfo.FullName, this.lrl.IsLockExtension);
      this.RefreshScreen(this.dataTables, this.lrl);
      this.DisableBuyLockDateAndDays();
    }

    private void btnCopyBuy_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        for (int index = 2148; index <= 2203; ++index)
        {
          string str = this.getField(index.ToString());
          int num = index + 71;
          if (this.lrl.IsLockExtension)
          {
            switch (num)
            {
              case 2221:
                str = this.setLockDate();
                break;
              case 2222:
                str = this.txtBuySideExtendedLockExpires.Text;
                this.txtSellExpireDate.Text = str;
                this.txtSellExpireDate.Value = DateTime.Parse(str);
                this.txtSellLockDate.Text = this.txtBuyLockDate.Text;
                this.txtSellLockDate.Value = this.txtBuyLockDate.Value;
                this.calculateLockDate("762", this.txtSellExpireDate, this.txtSellLockDate, this.txtSellLockDays);
                this.setField("2221", this.txtSellLockDays.Text);
                break;
            }
          }
          this.setField(num.ToString(), str);
        }
        for (int index = 3474; index <= 3493; ++index)
        {
          string field = this.getField(index.ToString());
          this.setField((index + 20).ToString(), field);
        }
        for (int index = 0; index < 20; ++index)
        {
          int num1 = 4276 + index;
          int num2 = 4296 + index;
          string val = this.getField(num1.ToString()) ?? "";
          this.setField(num2.ToString(), val);
        }
        for (int index = 0; index < 20; ++index)
        {
          int num3 = 4356 + index;
          int num4 = 4376 + index;
          string val = this.getField(num3.ToString()) ?? "";
          this.setField(num4.ToString(), val);
        }
        for (int index = 2448; index <= 2481; ++index)
        {
          string field = this.getField(index.ToString());
          this.setField((index + 34).ToString(), field);
        }
        for (int index = 2733; index <= 2775; ++index)
        {
          string field = this.getField(index.ToString());
          this.setField((index + 43).ToString(), field);
        }
        string field1 = this.getField("2215");
        if (!string.IsNullOrEmpty(field1) && !field1.Equals(this.getField("2286")))
          this.setField("2286", this.getField("2215"));
        this.setField("2030", Session.UserInfo.FullName);
        this.setField("3123", this.getField("2866"));
        this.setField("3257", this.getField("3256"));
        string field2 = this.getField("4751");
        if (!string.IsNullOrEmpty(field2) && !field2.Equals(this.getField("2278")))
          this.setField("2278", this.getField("4751"));
        this.RefreshScreen(this.dataTables, this.lrl);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void calculateLockDate(
      string id,
      DatePicker fieldExpire,
      DatePicker fieldLockDate,
      TextBox fieldDays)
    {
      LockExpDayCountSetting policySetting = (LockExpDayCountSetting) Session.StartupInfo.PolicySettings[(object) "Policies.LockExpDayCount"];
      DateTime lockDate = DateTime.MaxValue;
      DateTime dateTime = DateTime.MaxValue;
      string str1 = "762";
      string str2 = "432";
      string str3 = "761";
      string s1 = fieldExpire.Text.Trim();
      string s2 = fieldLockDate.Text.Trim();
      int lockDays1 = Utils.ParseInt((object) fieldDays.Text.Trim());
      if (s2 == "//")
        s2 = string.Empty;
      if (s1 == "//")
        s1 = string.Empty;
      if (s2 == string.Empty && id == str3)
      {
        fieldDays.Text = "";
        fieldExpire.Text = "";
      }
      else
      {
        try
        {
          if (s2 != string.Empty)
            lockDate = DateTime.Parse(s2).Date;
        }
        catch (Exception ex)
        {
          return;
        }
        try
        {
          if (s1 != string.Empty)
            dateTime = DateTime.Parse(s1).Date;
        }
        catch (Exception ex)
        {
          return;
        }
        bool ignoreSettings = this.dataTables[(object) "3902"] != null && Convert.ToString(this.dataTables[(object) "3902"]) != "" && this.dataTables[(object) "3911"] != null && !this.dataTables[(object) "3911"].ToString().Contains("Individual") && this.loanMgr.LoanData.GetField("3915") != "" && !this.loanMgr.LoanData.GetField("3911").Contains("Individual");
        if (id == str2 || id == str3 && lockDays1 != 0)
        {
          if (lockDays1 > 0)
          {
            if (s2 == string.Empty && s1 != string.Empty)
            {
              fieldLockDate.Text = this.lockCalculator.CalculateLockDate(dateTime, lockDays1).ToString("MM/dd/yyyy");
            }
            else
            {
              if (s2 == string.Empty && s1 == string.Empty)
                return;
              fieldExpire.Text = this.lockCalculator.CalculateLockExpirationDate(lockDate, lockDays1, ignoreSettings).ToString("MM/dd/yyyy");
            }
          }
          else
          {
            fieldExpire.Text = string.Empty;
            fieldDays.Text = string.Empty;
          }
        }
        else
        {
          if (!(id == str1) && (!(id == str3) || lockDays1 != 0))
            return;
          if (s1 != string.Empty)
          {
            if (!ignoreSettings)
              dateTime = this.lockCalculator.GetNextClosestLockExpirationDate(dateTime);
            fieldExpire.Text = dateTime.ToString("MM/dd/yyyy");
            if (s2 == string.Empty && lockDays1 != 0)
            {
              fieldLockDate.Text = this.lockCalculator.CalculateLockDate(dateTime, lockDays1, ignoreSettings).ToString("MM/dd/yyyy");
            }
            else
            {
              if (!(s2 != string.Empty))
                return;
              int lockDays2 = this.lockCalculator.CalculateLockDays(lockDate, dateTime, ignoreSettings);
              if (lockDays2 > 0)
                fieldDays.Text = lockDays2.ToString();
              else
                fieldDays.Text = "";
            }
          }
          else
            fieldDays.Text = "";
        }
      }
    }

    private void buySideSRP_TextChanged(object sender, EventArgs e)
    {
      this.txtBuySRP.Text = this.txtBuySideSRP.Text;
    }

    private void buySellSRP_TextChanged(object sender, EventArgs e)
    {
      this.txtSellSRP.Text = this.txtSellSideSRP.Text;
    }

    private void pictureBoxContact_Click(object sender, EventArgs e)
    {
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Investor", this.textBoxInvestorName.Text.Trim(), this.textBoxInvestorContact.Text.Trim(), new RxContactInfo()
      {
        [RolodexField.Company] = this.textBoxInvestorName.Text.Trim(),
        [RolodexField.Name] = this.textBoxInvestorContact.Text.Trim()
      }, true, true, CRMRoleType.NotFound, true, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
        {
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        }
        else
        {
          RxContactInfo rxContactRecord = rxBusinessContact.RxContactRecord;
          this.textBoxInvestorName.Text = rxContactRecord[RolodexField.Company];
          this.textBoxCompInvestorName.Text = rxContactRecord[RolodexField.Company];
          this.setField("2278", this.textBoxInvestorName.Text);
          this.textBoxInvestorContact.Text = rxContactRecord[RolodexField.Name];
          this.setField("2279", this.textBoxInvestorContact.Text);
          this.textBoxPhone.Text = rxContactRecord[RolodexField.Phone];
          this.setField("2280", this.textBoxPhone.Text);
          this.textBoxEmail.Text = rxContactRecord[RolodexField.Email];
          this.setField("3055", this.textBoxEmail.Text);
          this.textBoxAddress.Text = rxContactRecord[RolodexField.FullAddress];
          this.setField("2281", this.textBoxAddress.Text);
          this.textBoxCity.Text = rxContactRecord[RolodexField.City];
          this.setField("2282", this.textBoxCity.Text);
          this.textBoxState.Text = rxContactRecord[RolodexField.State];
          this.setField("2283", this.textBoxState.Text);
          this.textBoxZip.Text = rxContactRecord[RolodexField.ZipCode];
          this.setField("2284", this.textBoxZip.Text);
          this.textBoxWebUrl.Text = rxContactRecord[RolodexField.WebSite];
          this.setField("2285", this.textBoxWebUrl.Text);
        }
      }
    }

    private void timeField_KeyDown(object sender, KeyEventArgs e)
    {
      if (!e.Control || e.KeyCode != Keys.D)
        return;
      TextBox textBox = (TextBox) sender;
      textBox.Text = DateTime.Now.ToShortTimeString();
      this.setField(textBox.Tag.ToString(), textBox.Text);
    }

    private void textBoxInvestorName_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.pictureBoxContact_Click((object) null, (EventArgs) null);
    }

    private void dateField_Leave(object sender, EventArgs e)
    {
      DatePicker ctrl = (DatePicker) sender;
      if (ctrl == null)
        return;
      string tag = (string) ctrl.Tag;
      if ((tag ?? "") == "" || this.readOnly && !this.isUpdateSell || this.isUpdateSell && LockRequestLog.BuySideFields.Contains(tag))
        return;
      this.popupRules.RuleValidate((object) ctrl, tag);
      if (ctrl.Text == "//")
        this.setField(tag, "");
      else
        this.setField(tag, ctrl.Text);
    }

    public void ZoomPanels(
      bool hideBR,
      bool hideBP,
      bool hideBM,
      bool hidePR,
      bool hideEX,
      bool hideRL,
      bool hideCPA,
      bool hideCPC,
      bool hideBPC)
    {
      this.hideBR = hideBR;
      this.hideBP = hideBP;
      this.hideBM = hideBM;
      this.hidePR = hidePR;
      this.hideEX = hideEX;
      this.hideRL = hideRL;
      this.hideCPA = hideCPA;
      this.hideCPC = hideCPC;
      this.hideBPC = hideBPC;
      this.zoomInAndOut();
    }

    private void zoomInAndOut()
    {
      if (this.hideControls)
        return;
      if (this.hideBR)
      {
        this.panelLeft40.Visible = false;
        this.panelRight40.Visible = false;
        this.panelComp40.Visible = false;
        this.picBRZoomIn.Visible = true;
        this.picBRZoomOut.Visible = false;
      }
      else
      {
        this.panelLeft40.Visible = true;
        this.panelRight40.Visible = true;
        this.panelComp40.Visible = true;
        this.picBRZoomIn.Visible = false;
        this.picBRZoomOut.Visible = true;
      }
      if (this.hideBP)
      {
        this.panelLeft80.Visible = false;
        this.panelRight80.Visible = false;
        this.panelComp80.Visible = false;
        this.picBPZoomIn.Visible = true;
        this.picBPZoomOut.Visible = false;
      }
      else
      {
        this.panelLeft80.Visible = true;
        this.panelRight80.Visible = true;
        this.panelComp80.Visible = true;
        this.picBPZoomIn.Visible = false;
        this.picBPZoomOut.Visible = true;
      }
      if (this.hideBM)
      {
        this.panelLeft120.Visible = false;
        this.panelRight120.Visible = false;
        this.panelComp120.Visible = false;
        this.picBMZoomIn.Visible = true;
        this.picBMZoomOut.Visible = false;
      }
      else
      {
        this.panelLeft120.Visible = true;
        this.panelRight120.Visible = true;
        this.panelComp120.Visible = true;
        this.picBMZoomIn.Visible = false;
        this.picBMZoomOut.Visible = true;
      }
      if (this.hidePR)
      {
        this.panelBuySideProfitB.Visible = false;
        this.panelSellSideProfitSpacerB.Visible = false;
        this.panelCompProfitSpacerB.Visible = false;
        this.picPRZoomIn.Visible = true;
        this.picPRZoomOut.Visible = false;
        if (this.isSummary)
        {
          this.panelBuySideProfitB.Height = 70;
          this.panelBuySideProfitB.Visible = true;
          this.panelSumSell2.Height = this.panelSumComp2.Height = 95;
        }
      }
      else
      {
        this.panelBuySideProfitB.Visible = true;
        this.panelSellSideProfitSpacerB.Visible = this.panelCompProfitSpacerB.Visible = !this.isSummary;
        if (this.isSummary)
        {
          this.panelBuySideProfitB.Height = 242;
          this.panelSumSell2.Height = this.panelSumComp2.Height = this.panelBuySideProfitA.Height + this.panelBuySideProfitB.Height - this.panelSumComp1.Height;
        }
        this.picPRZoomIn.Visible = false;
        this.picPRZoomOut.Visible = true;
      }
      if (this.hideEX)
      {
        this.panelBuySideExtensionB.Visible = false;
        this.panelSellSideExtensionB.Visible = false;
        this.panelCompExtensionB.Visible = false;
        this.picEXZoomIn.Visible = true;
        this.picEXZoomOut.Visible = false;
      }
      else
      {
        this.panelBuySideExtensionB.Visible = true;
        this.panelSellSideExtensionB.Visible = true;
        this.panelCompExtensionB.Visible = true;
        this.picEXZoomIn.Visible = false;
        this.picEXZoomOut.Visible = true;
      }
      this.panelBuySideRelockB.Visible = !this.hideRL;
      this.panelSellSideRelockB.Visible = !this.hideRL;
      this.panelCompRelockB.Visible = !this.hideRL;
      this.picRLZoomIn.Visible = this.hideRL;
      this.picRLZoomOut.Visible = !this.hideRL;
      this.panelBuySideCPA_B.Visible = !this.hideCPA;
      this.panelSellSideCPA_B.Visible = !this.hideCPA;
      this.panelCompCPA_B.Visible = !this.hideCPA;
      this.picCPAZoomIn.Visible = this.hideCPA;
      this.picCPAZoomOut.Visible = !this.hideCPA;
      this.panelBuySideCPC_B.Visible = !this.hideCPC;
      this.panelSellSideCPC_B.Visible = !this.hideCPC;
      this.panelCompCPC_B.Visible = !this.hideCPC;
      this.picCPCZoomIn.Visible = this.hideCPC;
      this.picCPCZoomOut.Visible = !this.hideCPC;
      this.panelBuySideBPC_B.Visible = !this.hideBPC;
      this.panelSellSideBPC_B.Visible = !this.hideBPC;
      this.panelCompBPC_B.Visible = !this.hideBPC;
      this.picBPCZoomIn.Visible = this.hideBPC;
      this.picBPCZoomOut.Visible = !this.hideBPC;
      this.LockForm_SizeChanged((object) null, (EventArgs) null);
    }

    private void ZoomButton_Clicked(object sender, EventArgs e)
    {
      string name = ((Control) sender).Name;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(name))
      {
        case 42794915:
          if (!(name == "picCPAZoomIn"))
            return;
          this.hideCPA = false;
          break;
        case 387864817:
          if (!(name == "picBPZoomIn"))
            return;
          this.hideBP = false;
          break;
        case 518219510:
          if (!(name == "picEXZoomIn"))
            return;
          this.hideEX = false;
          break;
        case 686178818:
          if (!(name == "picBMZoomIn"))
            return;
          this.hideBM = false;
          break;
        case 1413198263:
          if (!(name == "picEXZoomOut"))
            return;
          this.hideEX = true;
          break;
        case 1580154386:
          if (!(name == "picBPZoomOut"))
            return;
          this.hideBP = true;
          break;
        case 1764123033:
          if (!(name == "picPRZoomIn"))
            return;
          this.hidePR = false;
          break;
        case 1920795938:
          if (!(name == "picCPCZoomOut"))
            return;
          this.hideCPC = true;
          break;
        case 1946482279:
          if (!(name == "picBPCZoomOut"))
            return;
          this.hideBPC = true;
          break;
        case 1947951515:
          if (!(name == "picBMZoomOut"))
            return;
          this.hideBM = true;
          break;
        case 2239828019:
          if (!(name == "picBRZoomIn"))
            return;
          this.hideBR = false;
          break;
        case 2413136416:
          if (!(name == "picBRZoomOut"))
            return;
          this.hideBR = true;
          break;
        case 2435625525:
          if (!(name == "picRLZoomIn"))
            return;
          this.hideRL = false;
          break;
        case 3084277534:
          if (!(name == "picRLZoomOut"))
            return;
          this.hideRL = true;
          break;
        case 3369489057:
          if (!(name == "picCPCZoomIn"))
            return;
          this.hideCPC = false;
          break;
        case 3908798778:
          if (!(name == "picPRZoomOut"))
            return;
          this.hidePR = true;
          break;
        case 4224344304:
          if (!(name == "picCPAZoomOut"))
            return;
          this.hideCPA = true;
          break;
        case 4282185734:
          if (!(name == "picBPCZoomIn"))
            return;
          this.hideBPC = false;
          break;
        default:
          return;
      }
      this.zoomInAndOut();
      if (this.ZoomButtonClicked == null)
        return;
      this.ZoomButtonClicked(sender, e);
    }

    public bool IsBaseRateDisplayedAll => !this.hideBR;

    public bool IsBasePriceDisplayedAll => !this.hideBP;

    public bool IsBaseMarginDisplayedAll => !this.hideBM;

    public bool IsProfitMarginDisplayedAll => !this.hidePR;

    public bool IsExtensionDisplayedAll => !this.hideEX;

    public bool IsRelockDisplayedAll => !this.hideRL;

    public bool IsCPADisplayedAll => !this.hideCPA;

    public bool IsCPCDisplayedAll => !this.hideCPC;

    public bool IsBPCDisplayedAll => !this.hideBPC;

    private void btnTradeSummary_Click(object sender, EventArgs e)
    {
      using (TradeSummaryDialog tradeSummaryDialog = new TradeSummaryDialog(Session.LoanData.GUID))
      {
        int num = (int) tradeSummaryDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void btnInvestor_Click(object sender, EventArgs e)
    {
      using (InvestorTemplateSelector templateSelector = new InvestorTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        InvestorTemplate selectedTemplate = templateSelector.SelectedTemplate;
        this.textBoxInvestorName.Text = selectedTemplate.CompanyInformation.ContactInformation.EntityName;
        this.textBoxCompInvestorName.Text = selectedTemplate.CompanyInformation.ContactInformation.EntityName;
        this.textBoxInvestorContact.Text = selectedTemplate.CompanyInformation.ContactInformation.ContactName;
        this.textBoxAddress.Text = selectedTemplate.CompanyInformation.ContactInformation.Address.Street1;
        this.textBoxCity.Text = selectedTemplate.CompanyInformation.ContactInformation.Address.City;
        this.textBoxState.Text = selectedTemplate.CompanyInformation.ContactInformation.Address.State;
        this.textBoxZip.Text = selectedTemplate.CompanyInformation.ContactInformation.Address.Zip;
        this.textBoxEmail.Text = selectedTemplate.CompanyInformation.ContactInformation.EmailAddress;
        this.textBoxPhone.Text = selectedTemplate.CompanyInformation.ContactInformation.PhoneNumber;
        this.textBoxWebUrl.Text = selectedTemplate.CompanyInformation.ContactInformation.WebSite;
        this.commitControlValueToField(this.textBoxInvestorName);
        this.commitControlValueToField(this.textBoxInvestorContact);
        this.commitControlValueToField(this.textBoxAddress);
        this.commitControlValueToField(this.textBoxCity);
        this.commitControlValueToField(this.textBoxState);
        this.commitControlValueToField(this.textBoxZip);
        this.commitControlValueToField(this.textBoxEmail);
        this.commitControlValueToField(this.textBoxPhone);
        this.commitControlValueToField(this.textBoxWebUrl);
        this.setField("3263", selectedTemplate.TemplateName);
        Session.LoanDataMgr.ApplyInvestorToLoan(selectedTemplate.CompanyInformation, (ContactInformation) null, false);
      }
    }

    private void commitControlValueToField(TextBox control)
    {
      string id = string.Concat(control.Tag);
      if (!(id != "") || !this.popupRules.RuleValidate((object) control, id))
        return;
      this.setField(id, control.Text.Trim());
    }

    private void zoomIn_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) this.UIresources.GetObject("picZoomInOver.Image");
    }

    private void zoomOut_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) this.UIresources.GetObject("picZoomOutOver.Image");
    }

    private void zoomOut_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) this.UIresources.GetObject("picBRZoomOut.Image");
    }

    private void zoomIn_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) this.UIresources.GetObject("picBRZoomIn.Image");
    }

    private void btnBuyComment_Click(object sender, EventArgs e) => this.addComments(0);

    private void btnSellComment_Click(object sender, EventArgs e) => this.addComments(1);

    private void addComments(int source)
    {
      using (AddCommentForm addCommentForm = new AddCommentForm(Session.UserInfo.FullName))
      {
        if (addCommentForm.ShowDialog((IWin32Window) this) != DialogResult.OK || addCommentForm.NewComments == string.Empty)
          return;
        string str1 = string.Empty;
        bool onBuySide = false;
        if (source == 0)
          onBuySide = true;
        switch (source)
        {
          case 0:
            str1 = this.txtBuyComments.Text.Trim();
            break;
          case 1:
            str1 = this.txtSellComments.Text.Trim();
            break;
          case 2:
            str1 = this.txtCompComments.Text.Trim();
            break;
        }
        if (str1 != string.Empty)
          str1 += "\r\n\r\n";
        string str2 = str1 + addCommentForm.NewComments;
        bool flag = false;
        if (this.readOnly)
          flag = this.loanMgr.LoanData.UpdateConfirmLockComments(this.lrl.Guid, str2, onBuySide);
        switch (source)
        {
          case 0:
            if (!(!this.readOnly | flag))
              break;
            this.setField("2204", str2);
            this.txtBuyComments.Text = str2;
            this.txtBuyComments.SelectionStart = str2.Length;
            this.txtBuyComments.ScrollToCaret();
            this.txtBuyComments.Focus();
            break;
          case 1:
            if (!(!this.readOnly | flag) && !this.isUpdateSell)
              break;
            this.setField("2275", str2);
            this.txtSellComments.Text = str2;
            this.txtSellComments.SelectionStart = str2.Length;
            this.txtSellComments.ScrollToCaret();
            this.txtSellComments.Focus();
            break;
          case 2:
            if (!(!this.readOnly | flag) && !this.isUpdateSell)
              break;
            this.setField("3834", str2);
            this.txtCompComments.Text = str2;
            this.txtCompComments.SelectionStart = str2.Length;
            this.txtCompComments.ScrollToCaret();
            this.txtCompComments.Focus();
            break;
        }
      }
    }

    private void txtBuyComments_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void txtSellComments_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void txtBuyComments_KeyDown(object sender, KeyEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void txtSellComments_KeyDown(object sender, KeyEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void txtComComments_KeyDown(object sender, KeyPressEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void btnClearBuyside_Click(object sender, EventArgs e)
    {
      bool visible = this.pnlBuySideLockExtension.Visible;
      for (int index = 2148; index <= 2203; ++index)
      {
        if (!(index == 2151 & visible))
          this.setField(index.ToString(), "");
      }
      for (int index = 2448; index <= 2481; ++index)
        this.setField(index.ToString(), "");
      for (int index = 2733; index <= 2775; ++index)
        this.setField(index.ToString(), "");
      for (int index = 3380; index <= 3420; ++index)
        this.setField(index.ToString(), "");
      for (int index = 3474; index <= 3493; ++index)
        this.setField(index.ToString(), "");
      for (int index = 0; index < 20; ++index)
        this.setField((4276 + index).ToString(), "");
      for (int index = 0; index < 20; ++index)
        this.setField((4356 + index).ToString(), "");
      this.setField("2205", "");
      this.setField("2215", "");
      this.setField("3256", "");
      this.setField("3363", "");
      this.setField("3364", "");
      this.setField("3365", "");
      this.setField("3848", "");
      this.setField("3873", "");
      this.setField("3875", "");
      this.setField("4751", "");
      this.RefreshScreen(this.dataTables, this.lrl);
      this.DisableBuyLockDateAndDays();
    }

    private void btnClearSellSide_Click(object sender, EventArgs e)
    {
      for (int index = 2219; index <= 2274; ++index)
        this.setField(index.ToString(), "");
      for (int index = 2482; index <= 2515; ++index)
        this.setField(index.ToString(), "");
      for (int index = 2776; index <= 2818; ++index)
        this.setField(index.ToString(), "");
      for (int index = 2276; index <= 2292; ++index)
        this.setField(index.ToString(), "");
      for (int index = 3494; index <= 3513; ++index)
        this.setField(index.ToString(), "");
      for (int index = 0; index < 20; ++index)
        this.setField((4296 + index).ToString(), "");
      for (int index = 0; index < 20; ++index)
        this.setField((4376 + index).ToString(), "");
      this.setField("2297", "");
      this.setField("2206", "");
      this.setField("2286", "");
      this.setField("3123", "");
      this.setField("3257", "");
      this.setField("3257", "");
      this.setField("3055", "");
      this.setField("3534", "");
      this.setField("3535", "");
      this.setField("3888", "");
      this.setField("3889", "");
      this.setField("3890", "");
      this.setField("3263", "");
      this.RefreshScreen(this.dataTables, this.lrl);
    }

    private void LockForm_SizeChanged(object sender, EventArgs e)
    {
      this.SizeChanged -= new EventHandler(this.LockForm_SizeChanged);
      if (this.isSummary)
        this.Height = this.pnlBuySideInfo.Bottom + this.panel28.Height + this.panelSumSell1.Height + this.panelSumSell2.Height + this.panelBodyBottom.Height + this.panel3.Height;
      else
        this.Height = (this.hideControls ? this.panelRight160.Top + this.panelRight160.Height : this.panelRight170.Top + this.panelRight170.Height) + (this.hideControls ? this.panelTop.Height : 0) + this.panelBodyBottom.Height;
      this.SizeChanged += new EventHandler(this.LockForm_SizeChanged);
    }

    private void iconBtnEditCorpConcession_Click(object sender, EventArgs e)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string field1;
      string field2;
      string field3;
      string field4;
      switch (((Control) sender).Name.Replace("iconBtnEditCorpConcession", string.Empty))
      {
        case "2":
          field1 = this.getField("4753");
          field2 = this.getField("4754");
          field3 = this.getField("4755");
          field4 = this.getField("4756");
          break;
        case "3":
          field1 = this.getField("4757");
          field2 = this.getField("4758");
          field3 = this.getField("4759");
          field4 = this.getField("4760");
          break;
        case "4":
          field1 = this.getField("4761");
          field2 = this.getField("4762");
          field3 = this.getField("4763");
          field4 = this.getField("4764");
          break;
        case "5":
          field1 = this.getField("4765");
          field2 = this.getField("4766");
          field3 = this.getField("4767");
          field4 = this.getField("4768");
          break;
        default:
          field1 = this.getField("3371");
          field2 = this.getField("3372");
          field3 = this.getField("3373");
          field4 = this.getField("3374");
          break;
      }
      using (EditConcessionDialog concessionDialog = new EditConcessionDialog(this.readOnly || this.isUpdateSell, "Corporate Price Concession", field1, field2, field3, field4, this.use10Digits))
      {
        if (concessionDialog.ShowDialog() != DialogResult.OK || this.readOnly || this.isUpdateSell)
          return;
        this.textBoxCorpConcession.Text = concessionDialog.concession;
        switch (((Control) sender).Name.Replace("iconBtnEditCorpConcession", string.Empty))
        {
          case "2":
            this.setField("4753", concessionDialog.concession);
            this.setField("4754", concessionDialog.approvalDate);
            this.setField("4755", concessionDialog.approvedBy);
            this.setField("4756", concessionDialog.reason);
            break;
          case "3":
            this.setField("4757", concessionDialog.concession);
            this.setField("4758", concessionDialog.approvalDate);
            this.setField("4759", concessionDialog.approvedBy);
            this.setField("4760", concessionDialog.reason);
            break;
          case "4":
            this.setField("4761", concessionDialog.concession);
            this.setField("4762", concessionDialog.approvalDate);
            this.setField("4763", concessionDialog.approvedBy);
            this.setField("4764", concessionDialog.reason);
            break;
          case "5":
            this.setField("4765", concessionDialog.concession);
            this.setField("4766", concessionDialog.approvalDate);
            this.setField("4767", concessionDialog.approvedBy);
            this.setField("4768", concessionDialog.reason);
            break;
          default:
            this.setField("3371", concessionDialog.concession);
            this.setField("3372", concessionDialog.approvalDate);
            this.setField("3373", concessionDialog.approvedBy);
            this.setField("3374", concessionDialog.reason);
            break;
        }
        this.RefreshScreen(this.dataTables, this.lrl);
      }
    }

    private void iconBtnEditBranchConcession_Click(object sender, EventArgs e)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string field1;
      string field2;
      string field3;
      string field4;
      switch (((Control) sender).Name.Replace("iconBtnEditBranchConcession", string.Empty))
      {
        case "2":
          field1 = this.getField("4769");
          field2 = this.getField("4770");
          field3 = this.getField("4771");
          field4 = this.getField("4772");
          break;
        case "3":
          field1 = this.getField("4773");
          field2 = this.getField("4774");
          field3 = this.getField("4775");
          field4 = this.getField("4776");
          break;
        case "4":
          field1 = this.getField("4777");
          field2 = this.getField("4778");
          field3 = this.getField("4779");
          field4 = this.getField("4780");
          break;
        case "5":
          field1 = this.getField("4781");
          field2 = this.getField("4782");
          field3 = this.getField("4783");
          field4 = this.getField("4784");
          break;
        default:
          field1 = this.getField("3375");
          field2 = this.getField("3376");
          field3 = this.getField("3377");
          field4 = this.getField("3378");
          break;
      }
      using (EditConcessionDialog concessionDialog = new EditConcessionDialog(this.readOnly, "Branch Price Concession", field1, field2, field3, field4, this.use10Digits))
      {
        if (concessionDialog.ShowDialog() != DialogResult.OK || this.readOnly)
          return;
        this.textBoxCorpConcession.Text = concessionDialog.concession;
        switch (((Control) sender).Name.Replace("iconBtnEditBranchConcession", string.Empty))
        {
          case "2":
            this.setField("4769", concessionDialog.concession);
            this.setField("4770", concessionDialog.approvalDate);
            this.setField("4771", concessionDialog.approvedBy);
            this.setField("4772", concessionDialog.reason);
            break;
          case "3":
            this.setField("4773", concessionDialog.concession);
            this.setField("4774", concessionDialog.approvalDate);
            this.setField("4775", concessionDialog.approvedBy);
            this.setField("4776", concessionDialog.reason);
            break;
          case "4":
            this.setField("4777", concessionDialog.concession);
            this.setField("4778", concessionDialog.approvalDate);
            this.setField("4779", concessionDialog.approvedBy);
            this.setField("4780", concessionDialog.reason);
            break;
          case "5":
            this.setField("4781", concessionDialog.concession);
            this.setField("4782", concessionDialog.approvalDate);
            this.setField("4783", concessionDialog.approvedBy);
            this.setField("4784", concessionDialog.reason);
            break;
          default:
            this.setField("3375", concessionDialog.concession);
            this.setField("3376", concessionDialog.approvalDate);
            this.setField("3377", concessionDialog.approvedBy);
            this.setField("3378", concessionDialog.reason);
            break;
        }
        this.RefreshScreen(this.dataTables, this.lrl);
      }
    }

    private void lockExtensionField_Leave(object sender, EventArgs e)
    {
      if (sender == this.txtBuySideDaysToExtend && this.txtBuySideDaysToExtend.Text != this.getField("3363"))
      {
        string id = this.txtBuySideDaysToExtend.Tag.ToString();
        string val = this.txtBuySideDaysToExtend.Text.Trim();
        this.popupRules.RuleValidate((object) this.txtBuySideDaysToExtend, id);
        this.setField(id, val);
        this.calculateLockExtension(sender);
        this.UpdateLockExtensionAdjustment();
      }
      else if (sender == this.txtBuySideExtendedLockExpires && this.txtBuySideExtendedLockExpires.Text != this.getField("3364"))
      {
        string id = this.txtBuySideExtendedLockExpires.Tag.ToString();
        string val = this.txtBuySideExtendedLockExpires.Text.Trim();
        this.popupRules.RuleValidate((object) this.txtBuySideExtendedLockExpires, id);
        if (Utils.ParseDate((object) this.txtBuySideExtendedLockExpires.Text, DateTime.MinValue) <= Utils.ToDate(this.getField("3358")))
          val = this.getField("3358");
        this.setField(id, val);
        this.calculateLockExtension(sender);
        this.UpdateLockExtensionAdjustment();
      }
      else if (sender == this.txtBuySideLockExtendPriceAdj && this.txtBuySideLockExtendPriceAdj.Text != this.getField("3365"))
      {
        string id = this.txtBuySideLockExtendPriceAdj.Tag.ToString();
        string val = this.txtBuySideLockExtendPriceAdj.Text.Trim();
        this.popupRules.RuleValidate((object) this.txtBuySideLockExtendPriceAdj, id);
        if (val != "")
          val = Utils.ParseDouble((object) val).ToString("N3");
        this.setField(id, val);
        this.calculateLockExtension(sender);
        this.UpdateLockExtensionAdjustment();
      }
      this.RefreshScreen(this.dataTables, this.lrl);
    }

    private DateTime getExpirationDate(string currentLockExpirationDateString, int daysToExtend)
    {
      DateTime rawExpireDate = Utils.ParseDate((object) currentLockExpirationDateString, DateTime.MinValue).AddDays((double) daysToExtend);
      return string.Concat(this.companySettings[(object) "LockExtensionCalendarOpt"]) != "True" ? rawExpireDate : new LockRequestCalculator(Session.DefaultInstance.SessionObjects, this.loanMgr.LoanData).GetNextClosestLockExpirationDate(rawExpireDate);
    }

    private Decimal getPriceAdjustment(
      int daysToExtend,
      Decimal originalPriceAdjustment,
      string originalExpirationDateString)
    {
      Decimal priceAdjustment1 = 0M;
      if (this.lockExtensionUtils.HasPriceAdjustment(daysToExtend))
        priceAdjustment1 = this.lockExtensionUtils.GetPriceAdjustment(daysToExtend);
      if (string.Concat(this.companySettings[(object) "LockExtensionCalendarOpt"]) == "False" || string.Concat(this.companySettings[(object) "LockExtCalOpt_ApplyPriceAdj"]) != "True")
        return priceAdjustment1;
      DateTime date = Utils.ParseDate((object) originalExpirationDateString, DateTime.MinValue);
      DateTime expirationDate = this.getExpirationDate(originalExpirationDateString, daysToExtend);
      Decimal priceAdjustment2 = 0M;
      int days = expirationDate.Subtract(date).Days;
      if (this.lockExtensionUtils.HasPriceAdjustment(days))
        priceAdjustment2 = this.lockExtensionUtils.GetPriceAdjustment(days);
      return priceAdjustment2;
    }

    private void UpdateLockExtensionAdjustment()
    {
      int num1 = Utils.ParseInt((object) this.getField("3433"), 1);
      if (num1 > 10)
        return;
      int num2 = 3474 + (num1 - 1) * 2;
      if (Utils.IsInt((object) this.getField("3363")) && Utils.IsDouble((object) this.getField("3365")))
      {
        int num3 = Utils.ParseInt((object) this.getField("3363"));
        string field = this.getField("3365");
        this.setField(num2.ToString(), "(" + (object) num3 + (num3 == 1 ? (object) " day" : (object) " days") + ")");
        this.setField((num2 + 1).ToString(), field);
      }
      else
      {
        this.setField(num2.ToString(), "");
        this.setField((num2 + 1).ToString(), "");
      }
    }

    private void iconButtonServicer_Click(object sender, EventArgs e)
    {
      RxContactInfo rxContact = new RxContactInfo();
      rxContact.CompanyName = this.textBoxServicer.Text;
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Servicing", rxContact.CompanyName, "", rxContact, CRMRoleType.NotFound, false, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
        {
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        }
        else
        {
          this.textBoxServicer.Text = rxBusinessContact.RxContactRecord.CompanyName;
          this.setField("3535", rxBusinessContact.RxContactRecord.CompanyName);
        }
      }
    }

    private void txtCompSRP_TextChanged(object sender, EventArgs e)
    {
      this.txtCompSRP.Text = this.txtCompSRP.Text;
    }

    private void txtCompComments_KeyDown(object sender, KeyEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void txtCompComments_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = true;
    }

    private void btnCopyBuytoComp_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        this.setField("3660", this.getField("2866"));
        this.setField("3661", Session.UserInfo.FullName);
        this.setField("3662", this.getField("2148"));
        this.setField("3663", this.getField("3256"));
        this.setFieldinRange(2149, 2151, 1515);
        this.setFieldinRange(2152, 2158, 1519);
        this.setFieldinRange(2159, 2203, 1553);
        this.setFieldinRange(2448, 2481, 1230);
        this.setFieldinRange(3474, 3493, 281);
        this.setFieldinRange(4276, 4295, 40);
        this.setFieldinRange(4356, 4375, 40);
        this.setFieldinRange(2733, 2775, 1044);
        this.setField("3822", this.getField("4751"));
        if (this.lrl.IsLockExtension)
        {
          this.setField("3665", this.setLockDate());
          string text = this.txtBuySideExtendedLockExpires.Text;
          this.setField("3666", text);
          this.txtCompExpireDate.Text = text;
          this.txtCompExpireDate.Value = DateTime.Parse(text);
          this.txtCompLockDate.Text = this.txtBuyLockDate.Text;
          this.txtCompLockDate.Value = this.txtBuyLockDate.Value;
          this.calculateLockDate("762", this.txtCompExpireDate, this.txtCompLockDate, this.txtCompLockDays);
          this.setField("3665", this.txtCompLockDays.Text);
        }
        this.RefreshScreen(this.dataTables, this.lrl);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    internal void setFieldinRange(int begin, int end, int diff)
    {
      for (int index = begin; index <= end; ++index)
        this.setField((index + diff).ToString(), this.getField(index.ToString()));
    }

    internal void setFieldEmptyinRange(int begin, int end)
    {
      for (int index = begin; index <= end; ++index)
        this.setField(index.ToString(), "");
    }

    private void btnClearComp_Click(object sender, EventArgs e)
    {
      this.setField("3660", "");
      this.setFieldEmptyinRange(3662, 3833);
      this.setFieldEmptyinRange(3835, 3837);
      this.setFieldEmptyinRange(4316, 4335);
      this.setFieldEmptyinRange(4396, 4415);
      this.RefreshScreen(this.dataTables, this.lrl);
    }

    private void btnInvestorComp_Click(object sender, EventArgs e)
    {
      using (InvestorTemplateSelector templateSelector = new InvestorTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        InvestorTemplate selectedTemplate = templateSelector.SelectedTemplate;
        this.textBoxInvestorName2.Text = selectedTemplate.CompanyInformation.ContactInformation.EntityName;
        this.textBoxCompInvestorName2.Text = selectedTemplate.CompanyInformation.ContactInformation.EntityName;
        this.textBoxInvestorContact2.Text = selectedTemplate.CompanyInformation.ContactInformation.ContactName;
        this.textBoxAddress2.Text = selectedTemplate.CompanyInformation.ContactInformation.Address.Street1;
        this.textBoxCity2.Text = selectedTemplate.CompanyInformation.ContactInformation.Address.City;
        this.textBoxState2.Text = selectedTemplate.CompanyInformation.ContactInformation.Address.State;
        this.textBoxZip2.Text = selectedTemplate.CompanyInformation.ContactInformation.Address.Zip;
        this.textBoxEmail2.Text = selectedTemplate.CompanyInformation.ContactInformation.EmailAddress;
        this.textBoxPhone2.Text = selectedTemplate.CompanyInformation.ContactInformation.PhoneNumber;
        this.textBoxWebUrl2.Text = selectedTemplate.CompanyInformation.ContactInformation.WebSite;
        this.commitControlValueToField(this.textBoxInvestorName2);
        this.commitControlValueToField(this.textBoxInvestorContact2);
        this.commitControlValueToField(this.textBoxAddress2);
        this.commitControlValueToField(this.textBoxCity2);
        this.commitControlValueToField(this.textBoxState2);
        this.commitControlValueToField(this.textBoxZip2);
        this.commitControlValueToField(this.textBoxEmail2);
        this.commitControlValueToField(this.textBoxPhone2);
        this.commitControlValueToField(this.textBoxWebUrl2);
        this.setField("3838", selectedTemplate.TemplateName);
      }
    }

    private void iconBtnCompInvestor_Click(object sender, EventArgs e)
    {
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Investor", this.textBoxInvestorName.Text.Trim(), this.textBoxInvestorContact.Text.Trim(), new RxContactInfo()
      {
        [RolodexField.Company] = this.textBoxInvestorName.Text.Trim(),
        [RolodexField.Name] = this.textBoxInvestorContact.Text.Trim()
      }, true, true, CRMRoleType.NotFound, true, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
        {
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        }
        else
        {
          RxContactInfo rxContactRecord = rxBusinessContact.RxContactRecord;
          this.textBoxInvestorName2.Text = rxContactRecord[RolodexField.Company];
          this.textBoxCompInvestorName2.Text = rxContactRecord[RolodexField.Company];
          this.setField("3822", this.textBoxInvestorName2.Text);
          this.textBoxInvestorContact2.Text = rxContactRecord[RolodexField.Name];
          this.setField("3823", this.textBoxInvestorContact2.Text);
          this.textBoxPhone2.Text = rxContactRecord[RolodexField.Phone];
          this.setField("3824", this.textBoxPhone2.Text);
          this.textBoxEmail2.Text = rxContactRecord[RolodexField.Email];
          this.setField("3825", this.textBoxEmail2.Text);
          this.textBoxAddress2.Text = rxContactRecord[RolodexField.FullAddress];
          this.setField("3826", this.textBoxAddress2.Text);
          this.textBoxCity2.Text = rxContactRecord[RolodexField.City];
          this.setField("3827", this.textBoxCity2.Text);
          this.textBoxState2.Text = rxContactRecord[RolodexField.State];
          this.setField("3828", this.textBoxState2.Text);
          this.textBoxZip2.Text = rxContactRecord[RolodexField.ZipCode];
          this.setField("3829", this.textBoxZip2.Text);
          this.textBoxWebUrl2.Text = rxContactRecord[RolodexField.WebSite];
          this.setField("3830", this.textBoxWebUrl2.Text);
        }
      }
    }

    public void LockFormView()
    {
      bool flag1 = false;
      bool flag2 = true;
      if (this.isSummary)
      {
        this.groupBox28.Visible = this.isSummary;
        this.panelLeft20.Visible = this.panelLeft30.Visible = this.panelLeft40.Visible = this.panelLeft50.Visible = this.panelLeft60.Visible = this.panelLeft70.Visible = flag1;
        this.panelLeft80.Visible = this.panelLeft100.Visible = this.panelLeft110.Visible = this.panelLeft120.Visible = flag1;
        this.panelBuySideCPC_A.Visible = this.panelBuySideCPC_B.Visible = this.panelBuySideBPC_A.Visible = this.panelBuySideBPC_B.Visible = this.panelBuySideTPC.Visible = flag1;
        this.panelBuySideExtensionA.Visible = this.panelBuySideExtensionB.Visible = this.panelBuySideProfitB.Visible = flag1;
        this.panelBuySideRelockA.Visible = this.panelBuySideRelockB.Visible = flag1;
        this.panelBuySideCPA_A.Visible = this.panelBuySideCPA_B.Visible = flag1;
        this.panelLeft130.Visible = this.panelLeft140.Visible = this.panelLeft150.Visible = flag1;
        this.panel28.Visible = false;
        this.panelRight155.Visible = false;
        this.panelRight20.Visible = this.panelRight30.Visible = this.panelRight40.Visible = this.panelRight50.Visible = this.panelRight60.Visible = this.panelRight70.Visible = flag1;
        this.panelRight80.Visible = this.panelRight100.Visible = this.panelRight110.Visible = this.panelRight120.Visible = flag1;
        this.panelSellSideCPC_A.Visible = this.panelSellSideCPC_B.Visible = this.panelSellSideBPC_A.Visible = this.panelSellSideBPC_B.Visible = this.panelSellSideTPC.Visible = flag1;
        this.panelRight130.Visible = this.panelRight140.Visible = this.panelRight150.Visible = this.panelRight160.Visible = this.panelRight170.Visible = flag1;
        this.panelSellSideExtensionA.Visible = this.panelSellSideExtensionB.Visible = this.panelSellSideProfitSpacerA.Visible = this.panelSellSideProfitSpacerB.Visible = flag1;
        this.panelSellSideRelockA.Visible = this.panelSellSideRelockB.Visible = flag1;
        this.panelSellSideCPA_A.Visible = this.panelSellSideCPA_B.Visible = flag1;
        this.panelComp20.Visible = this.panelComp30.Visible = this.panelComp40.Visible = this.panelComp50.Visible = this.panelComp60.Visible = this.panelComp70.Visible = flag1;
        this.panelComp80.Visible = this.panelComp100.Visible = this.panelComp110.Visible = this.panelComp120.Visible = flag1;
        this.panelCompCPC_A.Visible = this.panelCompCPC_B.Visible = this.panelCompBPC_A.Visible = this.panelCompBPC_B.Visible = this.panelCompTPC.Visible = flag1;
        this.panelComp130.Visible = this.panelComp140.Visible = this.panelComp150.Visible = this.panelComp160.Visible = this.panelComp170.Visible = flag1;
        this.panelCompExtensionA.Visible = this.panelCompExtensionB.Visible = this.panelCompProfitSpacerA.Visible = this.panelCompProfitSpacerB.Visible = flag1;
        this.panelCompRelockA.Visible = this.panelCompRelockB.Visible = flag1;
        this.panelCompCPA_A.Visible = this.panelCompCPA_B.Visible = flag1;
        this.panelSumComp1.Visible = this.panelSumComp2.Visible = this.panelSumSell1.Visible = this.panelSumSell2.Visible = flag2;
        this.groupBox28.Visible = this.groupBox26.Visible = false;
        this.panelBuySideProfitB.Visible = true;
        if (this.hidePR)
        {
          this.panelBuySideProfitB.Height = 70;
          this.panelSumSell2.Height = this.panelSumComp2.Height = 90;
        }
        else
        {
          this.panelBuySideProfitB.Height = 242;
          this.panelSumSell2.Height = this.panelSumComp2.Height = this.panelBuySideProfitA.Height + this.panelBuySideProfitB.Height - this.panelSumComp1.Height;
        }
      }
      else
      {
        if (this.hideControls)
          return;
        this.panel28.Visible = flag2;
        this.panelRight155.Visible = flag2;
        this.panelLeft20.Visible = this.panelLeft30.Visible = this.panelLeft50.Visible = this.panelLeft60.Visible = this.panelLeft70.Visible = this.panelLeft100.Visible = this.panelLeft110.Visible = this.panelLeft130.Visible = this.panelLeft140.Visible = this.panelLeft150.Visible = this.panel3.Visible = flag2;
        this.panelBuySideCPC_A.Visible = this.panelBuySideBPC_A.Visible = this.panelBuySideTPC.Visible = flag2;
        this.panelBuySideCPC_B.Visible = this.panelSellSideCPC_B.Visible = this.panelCompCPC_B.Visible = !this.hideCPC;
        this.panelRight20.Visible = this.panelRight30.Visible = this.panelRight50.Visible = this.panelRight60.Visible = this.panelRight70.Visible = this.panelRight100.Visible = this.panelRight110.Visible = this.panelRight130.Visible = this.panelRight140.Visible = this.panelRight150.Visible = this.panelRight170.Visible = this.panelRight160.Visible = this.panelComp160.Visible = flag2;
        this.panelSellSideCPC_A.Visible = this.panelSellSideBPC_A.Visible = this.panelSellSideTPC.Visible = flag2;
        this.panelBuySideBPC_B.Visible = this.panelSellSideBPC_B.Visible = this.panelCompBPC_B.Visible = !this.hideBPC;
        this.panelComp20.Visible = this.panelComp30.Visible = this.panelComp50.Visible = this.panelComp60.Visible = this.panelComp70.Visible = this.panelComp100.Visible = this.panelComp110.Visible = this.panelComp130.Visible = this.panelComp140.Visible = this.panelComp150.Visible = this.panelComp170.Visible = flag2;
        this.panelCompCPC_A.Visible = this.panelCompBPC_A.Visible = this.panelCompTPC.Visible = flag2;
        this.panelBuySideExtensionA.Visible = this.panelBuySideRelockA.Visible = this.panelBuySideCPA_A.Visible = this.panelBuySideProfitA.Visible = this.panelSellSideExtensionA.Visible = this.panelSellSideRelockA.Visible = this.panelSellSideCPA_A.Visible = this.panelSellSideProfitSpacerA.Visible = this.panelCompProfitSpacerA.Visible = this.panelCompExtensionA.Visible = this.panelCompRelockA.Visible = this.panelCompCPA_A.Visible = flag2;
        this.panelSumSell1.Visible = this.panelSumSell2.Visible = this.panelSumComp1.Visible = this.panelSumComp2.Visible = flag1;
        this.groupBox28.Visible = this.groupBox26.Visible = true;
        this.panelBuySideProfitB.Height = 242;
        this.panelBuySideProfitB.Visible = this.panelSellSideProfitSpacerB.Visible = this.panelCompProfitSpacerB.Visible = !this.hidePR;
      }
    }

    private string setLockDate()
    {
      LockRequestLog lockRequestLog = (LockRequestLog) null;
      Hashtable hashtable = (Hashtable) null;
      string str = "";
      if (this.lrl.ParentLockGuid != "")
      {
        lockRequestLog = this.loanMgr.LoanData.GetLogList().GetLockRequest(this.lrl.ParentLockGuid);
        hashtable = lockRequestLog.GetLockRequestSnapshot();
      }
      if (lockRequestLog != null && Utils.ParseInt(hashtable[(object) "3433"], 0) == Utils.ParseInt((object) this.getField("3433"), 0))
      {
        int num = Utils.ParseInt(hashtable[(object) "3363"], 0);
        str = string.Concat((object) (Utils.ParseInt((object) this.getField("3431"), 0) + (Utils.ParseInt((object) this.getField("3363"), 0) - num) + Utils.ParseInt((object) this.getField("2150"), 0)));
      }
      return str;
    }

    private void btnCompComment_Click(object sender, EventArgs e) => this.addComments(2);

    internal bool ValidateLockExpirationDays()
    {
      LockUtils lockUtils = new LockUtils(Session.SessionObjects);
      int oriLockDays = Utils.ParseInt((object) this.txtBuyLockDays.Text, 0);
      int extLockDays = Utils.ParseInt((object) this.txtBuySideDaysToExtend.Text, 0);
      LockRequestLog lockRequestLog = (LockRequestLog) null;
      Hashtable hashtable = (Hashtable) null;
      int original3363Value = 0;
      if (this.lrl.ParentLockGuid != "")
      {
        lockRequestLog = this.loanMgr.LoanData.GetLogList().GetLockRequest(this.lrl.ParentLockGuid);
        hashtable = lockRequestLog.GetLockRequestSnapshot();
      }
      if (lockRequestLog != null && Utils.ParseInt(hashtable[(object) "3433"], 0) == Utils.ParseInt((object) this.getField("3433"), 0))
        original3363Value = Utils.ParseInt(hashtable[(object) "3363"], 0);
      string extend = lockUtils.ValidateDaysToExtend(Session.LoanData, extLockDays, oriLockDays, this.dataTables, original3363Value);
      if (extend == string.Empty)
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, extend, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      return false;
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
    }

    private void btnBuySideInvestor_Click(object sender, EventArgs e)
    {
      using (InvestorTemplateSelector templateSelector = new InvestorTemplateSelector())
      {
        if (templateSelector.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.txtboxBuySideInvestorName.Text = templateSelector.SelectedTemplate.CompanyInformation.ContactInformation.EntityName;
        this.commitControlValueToField(this.txtboxBuySideInvestorName);
      }
    }

    private void stdIconBtnBuySideInvestor_Click(object sender, EventArgs e)
    {
      using (RxBusinessContact rxBusinessContact = new RxBusinessContact("Investor", this.textBoxInvestorName.Text.Trim(), this.textBoxInvestorContact.Text.Trim(), new RxContactInfo()
      {
        [RolodexField.Company] = this.textBoxInvestorName.Text.Trim(),
        [RolodexField.Name] = this.textBoxInvestorContact.Text.Trim()
      }, true, true, CRMRoleType.NotFound, true, ""))
      {
        if (rxBusinessContact.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (rxBusinessContact.GoToContact)
        {
          Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
        }
        else
        {
          this.txtboxBuySideInvestorName.Text = rxBusinessContact.RxContactRecord[RolodexField.Company];
          this.setField("4751", this.txtboxBuySideInvestorName.Text);
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      // ISSUE: The method is too long to display (67738 instructions)
    }
  }
}
