// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.IPDFCreactiveX
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [TypeLibType(4160)]
  [DefaultMember("Document")]
  [Guid("45230D5E-5143-4514-9D2D-A844A8343F40")]
  [ComImport]
  public interface IPDFCreactiveX
  {
    [DispId(1)]
    short RulerSize { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ScrollWindow(int HorzScroll, int VertScroll);

    [DispId(3)]
    [ComAliasName("ACPDFCREACTIVEX.ReportViewConstants")]
    ReportViewConstants ReportView { [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.ReportViewConstants")] get; [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.ReportViewConstants"), In] set; }

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RedrawObject([MarshalAs(UnmanagedType.BStr)] string Object);

    [DispId(5)]
    [ComAliasName("ACPDFCREACTIVEX.ReportStateConstants")]
    ReportStateConstants ReportState { [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.ReportStateConstants")] get; [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.ReportStateConstants"), In] set; }

    [DispId(6)]
    int ZoomFactor { [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(7)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void InitTable(int RowCount, int ColCount);

    [DispId(8)]
    [ComAliasName("ACPDFCREACTIVEX.GridViewConstants")]
    GridViewConstants GridView { [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.GridViewConstants")] get; [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.GridViewConstants"), In] set; }

    [DispId(9)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Refresh();

    [DispId(10)]
    int AutoRefresh { [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(11)]
    int FitToParent { [DispId(11), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(11), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(12)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void InsertObject([ComAliasName("ACPDFCREACTIVEX.ObjectTypeConstants")] ObjectTypeConstants ObjectType);

    [DispId(13)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int UndoLevels();

    [DispId(14)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int RedoLevels();

    [DispId(15)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Undo();

    [DispId(16)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Redo();

    [DispId(17)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DeleteObject([MarshalAs(UnmanagedType.BStr)] string Object, int CanUndo);

    [DispId(18)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: ComAliasName("ACPDFCREACTIVEX.ObjectTypeConstants")]
    ObjectTypeConstants ActiveTool();

    [DispId(19)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DoCommandTool([ComAliasName("ACPDFCREACTIVEX.CommandToolConstants")] CommandToolConstants Id);

    [DispId(20)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: ComAliasName("ACPDFCREACTIVEX.CommandToolStatusConstants")]
    CommandToolStatusConstants UpdateCommandTool([ComAliasName("ACPDFCREACTIVEX.CommandToolConstants"), In] CommandToolConstants Id);

    [DispId(21)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RedrawRect(int left, int top, int right, int bottom);

    [DispId(22)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Save([MarshalAs(UnmanagedType.BStr)] string FileName, [ComAliasName("ACPDFCREACTIVEX.FileSaveOptionConstants")] FileSaveOptionConstants SaveOption);

    [DispId(23)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int Open([MarshalAs(UnmanagedType.BStr)] string FileName, [MarshalAs(UnmanagedType.BStr)] string Password);

    [DispId(24)]
    int MinimumGap { [DispId(24), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(24), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(25)]
    int HorzScrollBar { [DispId(25), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(25), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(26)]
    int VertScrollBar { [DispId(26), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(26), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(27)]
    int ShowMargins { [DispId(27), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(27), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(28)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void InitBlank();

    [DispId(29)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void InitReport();

    [DispId(30)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CreateSection([In] short Groups);

    [DispId(31)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Print([MarshalAs(UnmanagedType.BStr), In] string PrinterName, [In] int Prompt);

    [DispId(32)]
    string DataSource { [DispId(32), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(32), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(33)]
    object DataSourceEx { [DispId(33), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IUnknown)] get; [DispId(33), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.IUnknown), In] set; }

    [DispId(34)]
    acBookmark RootBookmark { [DispId(34), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

    [DispId(35)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ReachBookmark([MarshalAs(UnmanagedType.BStr)] string Bookmark);

    [DispId(37)]
    int Protected { [DispId(37), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(38)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int ReachText(
      [ComAliasName("ACPDFCREACTIVEX.ReachTextOptionConstants")] ReachTextOptionConstants Option,
      [MarshalAs(UnmanagedType.BStr)] string Text,
      [MarshalAs(UnmanagedType.BStr)] string FontName,
      short FontSize,
      int Bold,
      int Italic);

    [DispId(39)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CreateObject([ComAliasName("ACPDFCREACTIVEX.ObjectTypeConstants")] ObjectTypeConstants ObjectType, [MarshalAs(UnmanagedType.BStr)] string Reference);

    [DispId(40)]
    int PageCount { [DispId(40), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(41)]
    int CurrentPage { [DispId(41), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(41), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(42)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    object get_ObjectAttribute([MarshalAs(UnmanagedType.BStr)] string Object, [MarshalAs(UnmanagedType.BStr)] string Attribute);

    [DispId(42)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void set_ObjectAttribute([MarshalAs(UnmanagedType.BStr)] string Object, [MarshalAs(UnmanagedType.BStr)] string Attribute, [MarshalAs(UnmanagedType.Struct), In] object pVal);

    [DispId(43)]
    int Modified { [DispId(43), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(43), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(44)]
    string DataTableOrView { [DispId(44), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(44), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(45)]
    int Copies { [DispId(45), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(45), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(46)]
    short Duplex { [DispId(46), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(46), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(47)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddPage(int PageIndex);

    [DispId(48)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Encrypt([MarshalAs(UnmanagedType.BStr)] string OwnerPassword, [MarshalAs(UnmanagedType.BStr)] string UserPassword, int Options);

    [DispId(49)]
    int Linearized { [DispId(49), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(49), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(50)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ReceiveDoc(int lPort, int lTimeout);

    [DispId(51)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SendDoc([MarshalAs(UnmanagedType.BStr)] string szAddress, int lPort, [MarshalAs(UnmanagedType.BStr)] string szUsername);

    [DispId(52)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string ReachTextEx(
      [ComAliasName("ACPDFCREACTIVEX.ReachTextOptionConstants")] ReachTextOptionConstants Option,
      [MarshalAs(UnmanagedType.BStr)] string Text,
      [MarshalAs(UnmanagedType.BStr)] string FontName,
      short FontSize,
      int Bold,
      int Italic);

    [DispId(53)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ObjectAttributeStr([MarshalAs(UnmanagedType.BStr)] string Object, [MarshalAs(UnmanagedType.BStr)] string Attribute, [MarshalAs(UnmanagedType.BStr)] string newVal);

    [DispId(54)]
    int StatusBar { [DispId(54), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(54), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(55)]
    object SelectedObject { [DispId(55), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Struct)] get; }

    [DispId(56)]
    int PageWidth { [DispId(56), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(56), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(57)]
    int PageLength { [DispId(57), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(57), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(58)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DataReceived([In] ref byte data, [In] int length);

    [DispId(59)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void PageReceived();

    [DispId(60)]
    int VerticalNaviguationBar { [DispId(60), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(60), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(61)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartPrint([MarshalAs(UnmanagedType.BStr), In] string PrinterName, [In] int Prompt);

    [DispId(62)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void PrintPage([In] int PageNumber);

    [DispId(63)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndPrint();

    [DispId(64)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    acObject GetObjectXY([In] int X, [In] int Y);

    [DispId(65)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ExportToHTML([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.acHtmlExportOptions"), In] acHtmlExportOptions Options);

    [DispId(66)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ExportToRTF([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.acRtfExportOptions"), In] acRtfExportOptions Options, [In] int UseTabs);

    [DispId(67)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ExportToJPeg([MarshalAs(UnmanagedType.BStr), In] string FileName, [In] int Resolution, [In] int JPegLevel);

    [DispId(372)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ExportToXPS([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.acXPSExportOptions"), In] acXPSExportOptions Options);

    [DispId(68)]
    string PrintToFileName { [DispId(68), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(68), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(69)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void OptimizeDocument([In] int Level);

    [DispId(70)]
    object ParentContainer { [DispId(70), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IUnknown)] get; [DispId(70), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.IUnknown), In] set; }

    [DispId(71)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EmbedFont([MarshalAs(UnmanagedType.BStr), In] string BaseFont, [ComAliasName("ACPDFCREACTIVEX.acEmbedFontOptions"), In] acEmbedFontOptions Option);

    [DispId(72)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawCurrentPage(int hDC, int PrepareDC);

    [DispId(73)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ActivateObject([MarshalAs(UnmanagedType.BStr)] string Object);

    [DispId(74)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int OpenEx([MarshalAs(UnmanagedType.BStr)] string FileName, [MarshalAs(UnmanagedType.BStr)] string Password);

    [DispId(75)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetLicenseKey([MarshalAs(UnmanagedType.BStr)] string Company, [MarshalAs(UnmanagedType.BStr)] string LicKey);

    [DispId(76)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ExportToExcel([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.acExcelExportOptions"), In] acExcelExportOptions Options);

    [ComAliasName("ACPDFCREACTIVEX.acScaleConstants")]
    [DispId(77)]
    acScaleConstants ScaleToWindow { [DispId(77), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.acScaleConstants")] get; [DispId(77), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.acScaleConstants"), In] set; }

    [DispId(78)]
    [ComAliasName("ACPDFCREACTIVEX.acScaleConstants")]
    acScaleConstants ScaleToPrinter { [DispId(78), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.acScaleConstants")] get; [DispId(78), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.acScaleConstants"), In] set; }

    [DispId(0)]
    object Document { [DispId(0), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Struct)] get; }

    [DispId(79)]
    int SelectedObjectCount { [DispId(79), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(80)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    acObject GetObjectByName([MarshalAs(UnmanagedType.BStr), In] string Name);

    [DispId(81)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetAttributeForMultipleSelection([MarshalAs(UnmanagedType.BStr), In] string attribName, [MarshalAs(UnmanagedType.Struct), In] object attribVal);

    [DispId(82)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SelectAllObjects([In] int val);

    [DispId(83)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DuplicatePage([In] int PageIndex);

    [DispId(84)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DeletePage([In] int PageNumber, [In] int CanUndo);

    [DispId(85)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void LockAllObjects([In] int Lock);

    [DispId(86)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Append([MarshalAs(UnmanagedType.BStr), In] string FileName, [MarshalAs(UnmanagedType.BStr), In] string Password);

    [DispId(87)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Merge([MarshalAs(UnmanagedType.BStr), In] string FileName, [MarshalAs(UnmanagedType.BStr), In] string Password, [In] int Options);

    [DispId(88)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AppendEx([MarshalAs(UnmanagedType.Interface), In] PDFCreactiveX Document);

    [DispId(89)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void MergeEx([MarshalAs(UnmanagedType.Interface), In] PDFCreactiveX Document, [In] int Options);

    [DispId(90)]
    int ReadOnly { [DispId(90), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(91)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetPageNumbering(
      [ComAliasName("ACPDFCREACTIVEX.acPageNumbersPositions"), In] acPageNumbersPositions Position,
      [MarshalAs(UnmanagedType.BStr), In] string Font,
      [In] int ExtraMarginHorz,
      [In] int ExtraMarginVert,
      [In] int Color,
      [In] int StartWithPage,
      [MarshalAs(UnmanagedType.BStr), In] string Format);

    [DispId(92)]
    int TemplateMode { [DispId(92), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(92), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(93)]
    int TemplateVisible { [DispId(93), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(93), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(94)]
    int TemplatePrint { [DispId(94), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(94), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(95)]
    int TemplateRepeat { [DispId(95), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(95), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(96)]
    int WantTabs { [DispId(96), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(96), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(97)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartSave([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.FileSaveOptionConstants"), In] FileSaveOptionConstants SaveOption);

    [DispId(98)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SavePage([In] int PageNumber);

    [DispId(99)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndSave();

    [DispId(100)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ClearPage([In] int PageNumber);

    [DispId(101)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Encrypt128([MarshalAs(UnmanagedType.BStr)] string OwnerPassword, [MarshalAs(UnmanagedType.BStr)] string UserPassword, int Options);

    [DispId(102)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetWarningLevel();

    [DispId(103)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ProcessHyperlinksBookmarksFromIniFile([MarshalAs(UnmanagedType.BStr), In] string iniFileName);

    [DispId(104)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AutoHyperlinks([MarshalAs(UnmanagedType.BStr), In] string prefixes);

    [DispId(105)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AutoBookmarks([In] int levels, [MarshalAs(UnmanagedType.BStr), In] string fonts, [In] int startPage);

    [DispId(106)]
    int RulerBackColor { [DispId(106), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(106), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(107)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ExportToTiff([MarshalAs(UnmanagedType.BStr), In] string FileName, [In] int Resolution, [In] int TiffFormat);

    [DispId(108)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RotatePage([In] int PageNumber, [ComAliasName("ACPDFCREACTIVEX.RotatePageConstants"), In] RotatePageConstants Rotation);

    [DispId(109)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void MovePages([In] int PageNumber, [In] int PageCount, [In] int Destination);

    [DispId(200)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddExternalObjectTemplate([MarshalAs(UnmanagedType.Interface), In] IacExternalObject externalObject);

    [DispId(201)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void CreateObjectEx(
      [ComAliasName("ACPDFCREACTIVEX.ObjectTypeConstants"), In] ObjectTypeConstants ObjectType,
      [MarshalAs(UnmanagedType.BStr), In] string Reference,
      [In] int subType,
      [In] int PageNumber);

    [DispId(202)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    object GetObjectsInRectangle([In] int left, [In] int top, [In] int right, [In] int bottom, [In] int flags);

    [DispId(203)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetTargetDevmode([In] int hDevMode);

    [DispId(204)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void PrinterSetup();

    [DispId(205)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DigitalSignature(
      [MarshalAs(UnmanagedType.BStr), In] string SignerName,
      [MarshalAs(UnmanagedType.BStr), In] string Reason,
      [MarshalAs(UnmanagedType.BStr), In] string ImageFile,
      [MarshalAs(UnmanagedType.BStr), In] string Location,
      [In] int PageNumber,
      [In] int HorzPos,
      [In] int VertPos,
      [In] int width,
      [In] int height,
      [In] int flags);

    [DispId(206)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetFontAttributes([MarshalAs(UnmanagedType.BStr), In] string FontName, out int FontDescent, out int FontAscent);

    [DispId(300)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetAttributeForMultipleSelectionEx([MarshalAs(UnmanagedType.BStr), In] string attribName, [MarshalAs(UnmanagedType.Struct), In] object attribVal, [In] int CanUndo);

    [DispId(301)]
    [ComAliasName("ACPDFCREACTIVEX.acRefreshEventReasons")]
    acRefreshEventReasons RefreshEventReason { [DispId(301), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.acRefreshEventReasons")] get; }

    [DispId(302)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetRefreshEventReason([ComAliasName("ACPDFCREACTIVEX.acRefreshEventReasons"), In] acRefreshEventReasons newVal, [In] int bAddToExistingReason);

    [DispId(303)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetToolTipText([MarshalAs(UnmanagedType.BStr)] string tooltipText);

    [DispId(304)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    acPlugin NewPluginBaseObject();

    [DispId(306)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetPlugin([ComAliasName("ACPDFCREACTIVEX.CommandToolConstants"), In] CommandToolConstants PluginID, int bOnlyIfLoaded, [MarshalAs(UnmanagedType.IUnknown), In, Out] ref object ppUnknown);

    [DispId(350)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartSaveEx([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.FileSaveOptionConstants"), In] FileSaveOptionConstants SaveOption, out int lpHandle);

    [DispId(351)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SavePageEx([In] int Handle, [In] int PageNumber);

    [DispId(352)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndSaveEx([In] int Handle);

    [DispId(360)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DeleteAllHyperlinks();

    [DispId(361)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    object GetPageText([In] int PageNumber);

    [DispId(362)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string GetRawPageText([In] int PageNumber);

    [DispId(370)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ExportToHTMLEx(
      [MarshalAs(UnmanagedType.BStr), In] string FileName,
      [ComAliasName("ACPDFCREACTIVEX.acHtmlExportOptions"), In] acHtmlExportOptions Options,
      [MarshalAs(UnmanagedType.BStr), In] string imageNamingInfo,
      [In] int downsamplingResolution);

    [DispId(371)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AutoHyperlinksEx([MarshalAs(UnmanagedType.BStr), In] string prefixes, [In] int ClearExistingHyperlinks);

    [DispId(380)]
    string OptionalRegistryKeyForSavingSettings { [DispId(380), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(380), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(381)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetScreenCoordinates(
      [MarshalAs(UnmanagedType.BStr), In] string objectName,
      out int left,
      out int top,
      out int right,
      out int bottom);
  }
}
