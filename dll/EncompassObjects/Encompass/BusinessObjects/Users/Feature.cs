// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Feature
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("9103AF76-2875-4f44-9896-0E461E4DF604")]
  public enum Feature
  {
    EditorShowAllFormsCheckBox = 10, // 0x0000000A
    TabPipeline = 100, // 0x00000064
    TabContacts = 101, // 0x00000065
    TabNews = 102, // 0x00000066
    TabDashboard = 103, // 0x00000067
    TabTrades = 105, // 0x00000069
    TabReportLoan = 111, // 0x0000006F
    TabReportBorrowerContact = 112, // 0x00000070
    TabReportBusinessContact = 114, // 0x00000072
    LoanCreateBlank = 200, // 0x000000C8
    LoanCreateFromTemplate = 201, // 0x000000C9
    LoanDuplicate = 210, // 0x000000D2
    LoanMove = 211, // 0x000000D3
    LoanDelete = 212, // 0x000000D4
    LoanUpload = 213, // 0x000000D5
    LoanDownload = 214, // 0x000000D6
    LoanImport = 215, // 0x000000D7
    LoanDuplicateForSecond = 216, // 0x000000D8
    LoanTransfer = 217, // 0x000000D9
    LoanSearchLoansByRateLockReq = 218, // 0x000000DA
    ToolsTabAccess = 300, // 0x0000012C
    ToolsFileContacts = 301, // 0x0000012D
    ToolsBizContacts = 302, // 0x0000012E
    ToolsConversationLog = 303, // 0x0000012F
    ToolsDocTracking = 304, // 0x00000130
    DocumentTrackingNewEditImport = 305, // 0x00000131
    DocumentTrackingDelete = 306, // 0x00000132
    DocumentTrackingDeleteProtected = 307, // 0x00000133
    DocumentTrackingManageAccess = 308, // 0x00000134
    DocumentTrackingRemoveAccessProtected = 309, // 0x00000135
    ConditionTracking = 310, // 0x00000136
    ConditionTrackingNewEditImport = 311, // 0x00000137
    ConditionViewer = 312, // 0x00000138
    ToolsPrequal = 313, // 0x00000139
    ToolsDebtConsolidation = 314, // 0x0000013A
    ToolsLoanComparison = 315, // 0x0000013B
    ToolsCashToClose = 316, // 0x0000013C
    ToolsRentOwn = 317, // 0x0000013D
    ToolsProfitMngt = 318, // 0x0000013E
    ToolsTrustAccount = 319, // 0x0000013F
    ToolsSecureFormTransfer = 320, // 0x00000140
    ToolsGrantWriteAccess = 321, // 0x00000141
    ToolsFundingWS = 323, // 0x00000143
    ToolsBrokerCheckCal = 324, // 0x00000144
    ToolsFundingBalWS = 325, // 0x00000145
    ToolsPostClosingConditionTracking = 326, // 0x00000146
    ToolsSecondaryRegistration = 327, // 0x00000147
    ToolsPurchaseAdviceForm = 328, // 0x00000148
    ToolsShippingDetail = 329, // 0x00000149
    ToolsLockRequestForm = 330, // 0x0000014A
    ToolsUnderwriterSummary = 331, // 0x0000014B
    ToolsPostClosingNewEditImport = 332, // 0x0000014C
    eFolderAccess = 400, // 0x00000190
    eFolderAttach = 401, // 0x00000191
    eFolderDelete = 402, // 0x00000192
    eFolderRequest = 403, // 0x00000193
    eFolderDownloadFaxes = 404, // 0x00000194
    eFolderSend = 406, // 0x00000196
    eFolderArchive = 407, // 0x00000197
    eFolderAssignAs = 409, // 0x00000199
    InstantMessenger = 500, // 0x000001F4
    BorrowerContactAccess = 600, // 0x00000258
    BorrowerContactCreateNew = 601, // 0x00000259
    BorrowerContactCopy = 602, // 0x0000025A
    BorrowerContactDelete = 603, // 0x0000025B
    BorrowerContactReassign = 604, // 0x0000025C
    BorrowerContactOriginateLoan = 605, // 0x0000025D
    BorrowerContactMailMerge = 606, // 0x0000025E
    BorrowerContactImport = 607, // 0x0000025F
    BorrowerContactExport = 608, // 0x00000260
    BizContactAccess = 700, // 0x000002BC
    BizContactCreateNew = 701, // 0x000002BD
    BizContactCopy = 702, // 0x000002BE
    BizContactDelete = 703, // 0x000002BF
    BizContactMailMerge = 706, // 0x000002C2
    BizContactImport = 707, // 0x000002C3
    BizContactExport = 708, // 0x000002C4
    CampaignManagement = 709, // 0x000002C5
    LoanPrintAccess = 800, // 0x00000320
    LoanPrintStandardForms = 801, // 0x00000321
    LoanPrintCustomForms = 802, // 0x00000322
    LoanPrintPreview = 803, // 0x00000323
    SettingsLoanCustomFields = 932, // 0x000003A4
    SettingsCustomInputFormEditor = 933, // 0x000003A5
    SettingsPlanCode = 942, // 0x000003AE
    SettingsAltLender = 943, // 0x000003AF
    SettingsConditionSetup = 944, // 0x000003B0
    SettingsUserGroups = 950, // 0x000003B6
    SettingsBusinessRules = 960, // 0x000003C0
    SettingsLoanReassignment = 971, // 0x000003CB
    PersonalLoanPrograms = 1011, // 0x000003F3
    PersonalClosingCosts = 1012, // 0x000003F4
    PersonalDocumentSets = 1013, // 0x000003F5
    PersonalInputFormSets = 1014, // 0x000003F6
    PersonalMiscDataTemplates = 1015, // 0x000003F7
    PersonalLoanTemplateSets = 1016, // 0x000003F8
    PersonalCustomPrintForms = 1021, // 0x000003FD
    PersonalPrintGroups = 1022, // 0x000003FE
    PersonalePASS = 1023, // 0x000003FF
    PersonalCustomLetters = 1024, // 0x00000400
    PersonalReports = 1025, // 0x00000401
    LoansAssignRights = 1031, // 0x00000407
    LoansUnlock = 1032, // 0x00000408
  }
}
