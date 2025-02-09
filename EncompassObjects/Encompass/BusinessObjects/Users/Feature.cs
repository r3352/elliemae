// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Feature
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Enumeration of the different access items to which users/personas can be grantred access
  /// </summary>
  [Guid("9103AF76-2875-4f44-9896-0E461E4DF604")]
  public enum Feature
  {
    /// <summary>Indicates access to all forms</summary>
    EditorShowAllFormsCheckBox = 10, // 0x0000000A
    /// <summary>Indicates access to pipeline tab</summary>
    TabPipeline = 100, // 0x00000064
    /// <summary>Indicates access to contacts tab</summary>
    TabContacts = 101, // 0x00000065
    /// <summary>Indicates access to news tab</summary>
    TabNews = 102, // 0x00000066
    /// <summary>Indicates access to dashboard tab</summary>
    TabDashboard = 103, // 0x00000067
    /// <summary>Indicates access to trades tab</summary>
    TabTrades = 105, // 0x00000069
    /// <summary>Indicates access to Report Loan</summary>
    TabReportLoan = 111, // 0x0000006F
    /// <summary>Indicates access to Borrower Contact Report</summary>
    TabReportBorrowerContact = 112, // 0x00000070
    /// <summary>Indicates access to Business Contact Report</summary>
    TabReportBusinessContact = 114, // 0x00000072
    /// <summary>Indicates access to create blank loan</summary>
    LoanCreateBlank = 200, // 0x000000C8
    /// <summary>Indicates access to create loan from template</summary>
    LoanCreateFromTemplate = 201, // 0x000000C9
    /// <summary>Indicates access to Loan Duplicate Option</summary>
    LoanDuplicate = 210, // 0x000000D2
    /// <summary>Indicates access to Loan Move Option</summary>
    LoanMove = 211, // 0x000000D3
    /// <summary>Indicates access to Loan Delete Option</summary>
    LoanDelete = 212, // 0x000000D4
    /// <summary>Indicates access to Loan Upload</summary>
    LoanUpload = 213, // 0x000000D5
    /// <summary>Indicates access to Loan Download</summary>
    LoanDownload = 214, // 0x000000D6
    /// <summary>Indicates access to Loan Import</summary>
    LoanImport = 215, // 0x000000D7
    /// <summary>Indicates access to Loan Duplicate for Second</summary>
    LoanDuplicateForSecond = 216, // 0x000000D8
    /// <summary>Indicates access to Loan Transfer</summary>
    LoanTransfer = 217, // 0x000000D9
    /// <summary>Indicates access to Loan Search By Rate Lock</summary>
    LoanSearchLoansByRateLockReq = 218, // 0x000000DA
    /// <summary>Indicates access to Tools tab</summary>
    ToolsTabAccess = 300, // 0x0000012C
    /// <summary>Indicates access to File Contacts</summary>
    ToolsFileContacts = 301, // 0x0000012D
    /// <summary>Indicates access to Biz Contacts</summary>
    ToolsBizContacts = 302, // 0x0000012E
    /// <summary>Indicates access to Conversation Log</summary>
    ToolsConversationLog = 303, // 0x0000012F
    /// <summary>Indicates access to Doc Tracking</summary>
    ToolsDocTracking = 304, // 0x00000130
    /// <summary>Indicates access to DocTracking Create/Edit Import</summary>
    DocumentTrackingNewEditImport = 305, // 0x00000131
    /// <summary>Indicates access to DocTracking Delete Option</summary>
    DocumentTrackingDelete = 306, // 0x00000132
    /// <summary>Indicates  DocTracking Delete Protected</summary>
    DocumentTrackingDeleteProtected = 307, // 0x00000133
    /// <summary>Indicates access to DocTracking Management</summary>
    DocumentTrackingManageAccess = 308, // 0x00000134
    /// <summary>
    /// Indicates access to DocumentTrackingRemoveAccessProtected
    /// </summary>
    DocumentTrackingRemoveAccessProtected = 309, // 0x00000135
    /// <summary>Removes Doctracking Access Protected</summary>
    ConditionTracking = 310, // 0x00000136
    /// <summary>Indicates access to Condition Tracking</summary>
    ConditionTrackingNewEditImport = 311, // 0x00000137
    /// <summary>
    /// Indicates access to ConditionTracking Create Edit Import
    /// </summary>
    ConditionViewer = 312, // 0x00000138
    /// <summary>Indicates access to ToolsPrequal</summary>
    ToolsPrequal = 313, // 0x00000139
    /// <summary>Indicates access to ToolsDebtConsolidation</summary>
    ToolsDebtConsolidation = 314, // 0x0000013A
    /// <summary>Indicates access to ToolsLoanComparison</summary>
    ToolsLoanComparison = 315, // 0x0000013B
    /// <summary>Indicates access to ToolsCashToClose</summary>
    ToolsCashToClose = 316, // 0x0000013C
    /// <summary>Indicates access to ToolsRentOwn</summary>
    ToolsRentOwn = 317, // 0x0000013D
    /// <summary>Indicates access to ToolsProfitMngt</summary>
    ToolsProfitMngt = 318, // 0x0000013E
    /// <summary>Indicates access to ToolsTrustAccount</summary>
    ToolsTrustAccount = 319, // 0x0000013F
    /// <summary>Indicates access to ToolsSecureFormTransfer</summary>
    ToolsSecureFormTransfer = 320, // 0x00000140
    /// <summary>Indicates access to ToolsGrantWriteAccess</summary>
    ToolsGrantWriteAccess = 321, // 0x00000141
    /// <summary>Indicates access to ToolsFundingWS</summary>
    ToolsFundingWS = 323, // 0x00000143
    /// <summary>Indicates access to ToolsBrokerCheckCal</summary>
    ToolsBrokerCheckCal = 324, // 0x00000144
    /// <summary>Indicates access to ToolsFundingBalWS</summary>
    ToolsFundingBalWS = 325, // 0x00000145
    /// <summary>Indicates access to ToolsPostClosingConditionTracking</summary>
    ToolsPostClosingConditionTracking = 326, // 0x00000146
    /// <summary>Indicates access to ToolsSecondaryRegistration</summary>
    ToolsSecondaryRegistration = 327, // 0x00000147
    /// <summary>Indicates access to ToolsPurchaseAdviceForm</summary>
    ToolsPurchaseAdviceForm = 328, // 0x00000148
    /// <summary>Indicates access to ToolsShippingDetail</summary>
    ToolsShippingDetail = 329, // 0x00000149
    /// <summary>Indicates access to ToolsLockRequestForm</summary>
    ToolsLockRequestForm = 330, // 0x0000014A
    /// <summary>Indicates access to ToolsUnderwriterSummary</summary>
    ToolsUnderwriterSummary = 331, // 0x0000014B
    /// <summary>Indicates access to ToolsPostClosingNewEditImport</summary>
    ToolsPostClosingNewEditImport = 332, // 0x0000014C
    /// <summary>Indicates access to eFolderAccess</summary>
    eFolderAccess = 400, // 0x00000190
    /// <summary>Indicates access to eFolderAttach</summary>
    eFolderAttach = 401, // 0x00000191
    /// <summary>Indicates access to eFolderDelete</summary>
    eFolderDelete = 402, // 0x00000192
    /// <summary>Indicates access to eFolderRequest</summary>
    eFolderRequest = 403, // 0x00000193
    /// <summary>Indicates access to eFolderDownloadFaxes</summary>
    eFolderDownloadFaxes = 404, // 0x00000194
    /// <summary>Indicates access to eFolderSend</summary>
    eFolderSend = 406, // 0x00000196
    /// <summary>Indicates access to eFolderArchive</summary>
    eFolderArchive = 407, // 0x00000197
    /// <summary>Indicates access to eFolderAssignAs</summary>
    eFolderAssignAs = 409, // 0x00000199
    /// <summary>Indicates access to InstantMessenger</summary>
    InstantMessenger = 500, // 0x000001F4
    /// <summary>Indicates access to BorrowerContactAccess</summary>
    BorrowerContactAccess = 600, // 0x00000258
    /// <summary>Indicates access to BorrowerContactCreateNew</summary>
    BorrowerContactCreateNew = 601, // 0x00000259
    /// <summary>Indicates access to BorrowerContactCopy</summary>
    BorrowerContactCopy = 602, // 0x0000025A
    /// <summary>Indicates access to BorrowerContactDelete</summary>
    BorrowerContactDelete = 603, // 0x0000025B
    /// <summary>Indicates access to BorrowerContactReassign</summary>
    BorrowerContactReassign = 604, // 0x0000025C
    /// <summary>Indicates access to BorrowerContactOriginateLoan</summary>
    BorrowerContactOriginateLoan = 605, // 0x0000025D
    /// <summary>Indicates access to BorrowerContactMailMerge</summary>
    BorrowerContactMailMerge = 606, // 0x0000025E
    /// <summary>Indicates access to BorrowerContactImport</summary>
    BorrowerContactImport = 607, // 0x0000025F
    /// <summary>Indicates access to BorrowerContactExport</summary>
    BorrowerContactExport = 608, // 0x00000260
    /// <summary>Indicates access to BizContactAccess</summary>
    BizContactAccess = 700, // 0x000002BC
    /// <summary>Indicates access to BizContactCreateNew</summary>
    BizContactCreateNew = 701, // 0x000002BD
    /// <summary>Indicates access to BizContactCopy</summary>
    BizContactCopy = 702, // 0x000002BE
    /// <summary>Indicates access to BizContactDelete</summary>
    BizContactDelete = 703, // 0x000002BF
    /// <summary>Indicates access to BizContactMailMerge</summary>
    BizContactMailMerge = 706, // 0x000002C2
    /// <summary>Indicates access to BizContactImport</summary>
    BizContactImport = 707, // 0x000002C3
    /// <summary>Indicates access to BizContactExport</summary>
    BizContactExport = 708, // 0x000002C4
    /// <summary>Indicates access to CampaignManagement</summary>
    CampaignManagement = 709, // 0x000002C5
    /// <summary>Indicates access to LoanPrintAccess</summary>
    LoanPrintAccess = 800, // 0x00000320
    /// <summary>Indicates access to LoanPrintStandardForms</summary>
    LoanPrintStandardForms = 801, // 0x00000321
    /// <summary>Indicates access to LoanPrintCustomForms</summary>
    LoanPrintCustomForms = 802, // 0x00000322
    /// <summary>Indicates access to LoanPrintPreview</summary>
    LoanPrintPreview = 803, // 0x00000323
    /// <summary>Indicates access to SettingsLoanCustomFields</summary>
    SettingsLoanCustomFields = 932, // 0x000003A4
    /// <summary>Indicates access to SettingsCustomInputFormEditor</summary>
    SettingsCustomInputFormEditor = 933, // 0x000003A5
    /// <summary>Indicates access to SettingsPlanCode</summary>
    SettingsPlanCode = 942, // 0x000003AE
    /// <summary>Indicates access to SettingsAltLender</summary>
    SettingsAltLender = 943, // 0x000003AF
    /// <summary>Indicates access to SettingsConditionSetup</summary>
    SettingsConditionSetup = 944, // 0x000003B0
    /// <summary>Indicates access to SettingsUserGroups</summary>
    SettingsUserGroups = 950, // 0x000003B6
    /// <summary>Indicates access to SettingsBusinessRules</summary>
    SettingsBusinessRules = 960, // 0x000003C0
    /// <summary>Indicates access to SettingsLoanReassignment</summary>
    SettingsLoanReassignment = 971, // 0x000003CB
    /// <summary>Indicates access to PersonalLoanPrograms</summary>
    PersonalLoanPrograms = 1011, // 0x000003F3
    /// <summary>Indicates access to PersonalClosingCosts</summary>
    PersonalClosingCosts = 1012, // 0x000003F4
    /// <summary>Indicates access to PersonalDocumentSets</summary>
    PersonalDocumentSets = 1013, // 0x000003F5
    /// <summary>Indicates access to PersonalInputFormSets</summary>
    PersonalInputFormSets = 1014, // 0x000003F6
    /// <summary>Indicates access to PersonalMiscDataTemplates</summary>
    PersonalMiscDataTemplates = 1015, // 0x000003F7
    /// <summary>Indicates access to PersonalLoanTemplateSets</summary>
    PersonalLoanTemplateSets = 1016, // 0x000003F8
    /// <summary>Indicates access to PersonalCustomPrintForms</summary>
    PersonalCustomPrintForms = 1021, // 0x000003FD
    /// <summary>Indicates access to PersonalPrintGroups</summary>
    PersonalPrintGroups = 1022, // 0x000003FE
    /// <summary>Indicates access to PersonalePASS</summary>
    PersonalePASS = 1023, // 0x000003FF
    /// <summary>Indicates access to PersonalCustomLetters</summary>
    PersonalCustomLetters = 1024, // 0x00000400
    /// <summary>Indicates access to PersonalReports</summary>
    PersonalReports = 1025, // 0x00000401
    /// <summary>Indicates access to LoansAssignRights</summary>
    LoansAssignRights = 1031, // 0x00000407
    /// <summary>Indicates access to LoansUnlock</summary>
    LoansUnlock = 1032, // 0x00000408
  }
}
