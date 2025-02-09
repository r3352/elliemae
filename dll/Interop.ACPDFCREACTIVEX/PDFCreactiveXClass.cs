// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.PDFCreactiveXClass
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [TypeLibType(2)]
  [DefaultMember("Document")]
  [Guid("525CA8D6-81EE-4AED-B95C-9C4DBAD5980B")]
  [ClassInterface(0)]
  [ComSourceInterfaces("ACPDFCREACTIVEX._IPDFCreactiveXEvents\0\0")]
  [ComImport]
  public class PDFCreactiveXClass : 
    IPDFCreactiveX,
    PDFCreactiveX,
    _IPDFCreactiveXEvents_Event,
    IPersistStreamInit
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern PDFCreactiveXClass();

    [DispId(1)]
    public virtual extern short RulerSize { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ScrollWindow(int HorzScroll, int VertScroll);

    [DispId(3)]
    [ComAliasName("ACPDFCREACTIVEX.ReportViewConstants")]
    public virtual extern ReportViewConstants ReportView { [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.ReportViewConstants")] get; [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.ReportViewConstants"), In] set; }

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void RedrawObject([MarshalAs(UnmanagedType.BStr)] string Object);

    [DispId(5)]
    [ComAliasName("ACPDFCREACTIVEX.ReportStateConstants")]
    public virtual extern ReportStateConstants ReportState { [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.ReportStateConstants")] get; [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.ReportStateConstants"), In] set; }

    [DispId(6)]
    public virtual extern int ZoomFactor { [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(7)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void InitTable(int RowCount, int ColCount);

    [DispId(8)]
    [ComAliasName("ACPDFCREACTIVEX.GridViewConstants")]
    public virtual extern GridViewConstants GridView { [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.GridViewConstants")] get; [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.GridViewConstants"), In] set; }

    [DispId(9)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Refresh();

    [DispId(10)]
    public virtual extern int AutoRefresh { [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(11)]
    public virtual extern int FitToParent { [DispId(11), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(11), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(12)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void InsertObject([ComAliasName("ACPDFCREACTIVEX.ObjectTypeConstants")] ObjectTypeConstants ObjectType);

    [DispId(13)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int UndoLevels();

    [DispId(14)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int RedoLevels();

    [DispId(15)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Undo();

    [DispId(16)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Redo();

    [DispId(17)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DeleteObject([MarshalAs(UnmanagedType.BStr)] string Object, int CanUndo);

    [DispId(18)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: ComAliasName("ACPDFCREACTIVEX.ObjectTypeConstants")]
    public virtual extern ObjectTypeConstants ActiveTool();

    [DispId(19)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DoCommandTool([ComAliasName("ACPDFCREACTIVEX.CommandToolConstants")] CommandToolConstants Id);

    [DispId(20)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: ComAliasName("ACPDFCREACTIVEX.CommandToolStatusConstants")]
    public virtual extern CommandToolStatusConstants UpdateCommandTool([ComAliasName("ACPDFCREACTIVEX.CommandToolConstants"), In] CommandToolConstants Id);

    [DispId(21)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void RedrawRect(int left, int top, int right, int bottom);

    [DispId(22)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Save([MarshalAs(UnmanagedType.BStr)] string FileName, [ComAliasName("ACPDFCREACTIVEX.FileSaveOptionConstants")] FileSaveOptionConstants SaveOption);

    [DispId(23)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int Open([MarshalAs(UnmanagedType.BStr)] string FileName, [MarshalAs(UnmanagedType.BStr)] string Password);

    [DispId(24)]
    public virtual extern int MinimumGap { [DispId(24), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(24), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(25)]
    public virtual extern int HorzScrollBar { [DispId(25), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(25), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(26)]
    public virtual extern int VertScrollBar { [DispId(26), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(26), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(27)]
    public virtual extern int ShowMargins { [DispId(27), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(27), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(28)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void InitBlank();

    [DispId(29)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void InitReport();

    [DispId(30)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void CreateSection([In] short Groups);

    [DispId(31)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Print([MarshalAs(UnmanagedType.BStr), In] string PrinterName, [In] int Prompt);

    [DispId(32)]
    public virtual extern string DataSource { [DispId(32), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(32), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(33)]
    public virtual extern object DataSourceEx { [DispId(33), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IUnknown)] get; [DispId(33), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.IUnknown), In] set; }

    [DispId(34)]
    public virtual extern acBookmark RootBookmark { [DispId(34), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }

    [DispId(35)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ReachBookmark([MarshalAs(UnmanagedType.BStr)] string Bookmark);

    [DispId(37)]
    public virtual extern int Protected { [DispId(37), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(38)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int ReachText(
      [ComAliasName("ACPDFCREACTIVEX.ReachTextOptionConstants")] ReachTextOptionConstants Option,
      [MarshalAs(UnmanagedType.BStr)] string Text,
      [MarshalAs(UnmanagedType.BStr)] string FontName,
      short FontSize,
      int Bold,
      int Italic);

    [DispId(39)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void CreateObject([ComAliasName("ACPDFCREACTIVEX.ObjectTypeConstants")] ObjectTypeConstants ObjectType, [MarshalAs(UnmanagedType.BStr)] string Reference);

    [DispId(40)]
    public virtual extern int PageCount { [DispId(40), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(41)]
    public virtual extern int CurrentPage { [DispId(41), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(41), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(42)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    public virtual extern object get_ObjectAttribute([MarshalAs(UnmanagedType.BStr)] string Object, [MarshalAs(UnmanagedType.BStr)] string Attribute);

    [DispId(42)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void set_ObjectAttribute([MarshalAs(UnmanagedType.BStr)] string Object, [MarshalAs(UnmanagedType.BStr)] string Attribute, [MarshalAs(UnmanagedType.Struct), In] object pVal);

    [DispId(43)]
    public virtual extern int Modified { [DispId(43), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(43), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(44)]
    public virtual extern string DataTableOrView { [DispId(44), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(44), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(45)]
    public virtual extern int Copies { [DispId(45), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(45), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(46)]
    public virtual extern short Duplex { [DispId(46), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(46), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(47)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AddPage(int PageIndex);

    [DispId(48)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Encrypt([MarshalAs(UnmanagedType.BStr)] string OwnerPassword, [MarshalAs(UnmanagedType.BStr)] string UserPassword, int Options);

    [DispId(49)]
    public virtual extern int Linearized { [DispId(49), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(49), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(50)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ReceiveDoc(int lPort, int lTimeout);

    [DispId(51)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SendDoc([MarshalAs(UnmanagedType.BStr)] string szAddress, int lPort, [MarshalAs(UnmanagedType.BStr)] string szUsername);

    [DispId(52)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string ReachTextEx(
      [ComAliasName("ACPDFCREACTIVEX.ReachTextOptionConstants")] ReachTextOptionConstants Option,
      [MarshalAs(UnmanagedType.BStr)] string Text,
      [MarshalAs(UnmanagedType.BStr)] string FontName,
      short FontSize,
      int Bold,
      int Italic);

    [DispId(53)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ObjectAttributeStr([MarshalAs(UnmanagedType.BStr)] string Object, [MarshalAs(UnmanagedType.BStr)] string Attribute, [MarshalAs(UnmanagedType.BStr)] string newVal);

    [DispId(54)]
    public virtual extern int StatusBar { [DispId(54), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(54), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(55)]
    public virtual extern object SelectedObject { [DispId(55), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Struct)] get; }

    [DispId(56)]
    public virtual extern int PageWidth { [DispId(56), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(56), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(57)]
    public virtual extern int PageLength { [DispId(57), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(57), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(58)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DataReceived([In] ref byte data, [In] int length);

    [DispId(59)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void PageReceived();

    [DispId(60)]
    public virtual extern int VerticalNaviguationBar { [DispId(60), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(60), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(61)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void StartPrint([MarshalAs(UnmanagedType.BStr), In] string PrinterName, [In] int Prompt);

    [DispId(62)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void PrintPage([In] int PageNumber);

    [DispId(63)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void EndPrint();

    [DispId(64)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern acObject GetObjectXY([In] int X, [In] int Y);

    [DispId(65)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ExportToHTML([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.acHtmlExportOptions"), In] acHtmlExportOptions Options);

    [DispId(66)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ExportToRTF(
      [MarshalAs(UnmanagedType.BStr), In] string FileName,
      [ComAliasName("ACPDFCREACTIVEX.acRtfExportOptions"), In] acRtfExportOptions Options,
      [In] int UseTabs);

    [DispId(67)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ExportToJPeg([MarshalAs(UnmanagedType.BStr), In] string FileName, [In] int Resolution, [In] int JPegLevel);

    [DispId(372)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ExportToXPS([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.acXPSExportOptions"), In] acXPSExportOptions Options);

    [DispId(68)]
    public virtual extern string PrintToFileName { [DispId(68), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(68), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(69)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void OptimizeDocument([In] int Level);

    [DispId(70)]
    public virtual extern object ParentContainer { [DispId(70), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.IUnknown)] get; [DispId(70), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.IUnknown), In] set; }

    [DispId(71)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void EmbedFont([MarshalAs(UnmanagedType.BStr), In] string BaseFont, [ComAliasName("ACPDFCREACTIVEX.acEmbedFontOptions"), In] acEmbedFontOptions Option);

    [DispId(72)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DrawCurrentPage(int hDC, int PrepareDC);

    [DispId(73)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ActivateObject([MarshalAs(UnmanagedType.BStr)] string Object);

    [DispId(74)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int OpenEx([MarshalAs(UnmanagedType.BStr)] string FileName, [MarshalAs(UnmanagedType.BStr)] string Password);

    [DispId(75)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetLicenseKey([MarshalAs(UnmanagedType.BStr)] string Company, [MarshalAs(UnmanagedType.BStr)] string LicKey);

    [DispId(76)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ExportToExcel([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.acExcelExportOptions"), In] acExcelExportOptions Options);

    [DispId(77)]
    [ComAliasName("ACPDFCREACTIVEX.acScaleConstants")]
    public virtual extern acScaleConstants ScaleToWindow { [DispId(77), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.acScaleConstants")] get; [DispId(77), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.acScaleConstants"), In] set; }

    [DispId(78)]
    [ComAliasName("ACPDFCREACTIVEX.acScaleConstants")]
    public virtual extern acScaleConstants ScaleToPrinter { [DispId(78), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.acScaleConstants")] get; [DispId(78), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: ComAliasName("ACPDFCREACTIVEX.acScaleConstants"), In] set; }

    [DispId(0)]
    public virtual extern object Document { [DispId(0), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Struct)] get; }

    [DispId(79)]
    public virtual extern int SelectedObjectCount { [DispId(79), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(80)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern acObject GetObjectByName([MarshalAs(UnmanagedType.BStr), In] string Name);

    [DispId(81)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetAttributeForMultipleSelection([MarshalAs(UnmanagedType.BStr), In] string attribName, [MarshalAs(UnmanagedType.Struct), In] object attribVal);

    [DispId(82)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SelectAllObjects([In] int val);

    [DispId(83)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DuplicatePage([In] int PageIndex);

    [DispId(84)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DeletePage([In] int PageNumber, [In] int CanUndo);

    [DispId(85)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void LockAllObjects([In] int Lock);

    [DispId(86)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Append([MarshalAs(UnmanagedType.BStr), In] string FileName, [MarshalAs(UnmanagedType.BStr), In] string Password);

    [DispId(87)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Merge([MarshalAs(UnmanagedType.BStr), In] string FileName, [MarshalAs(UnmanagedType.BStr), In] string Password, [In] int Options);

    [DispId(88)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AppendEx([MarshalAs(UnmanagedType.Interface), In] PDFCreactiveX Document);

    [DispId(89)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void MergeEx([MarshalAs(UnmanagedType.Interface), In] PDFCreactiveX Document, [In] int Options);

    [DispId(90)]
    public virtual extern int ReadOnly { [DispId(90), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; }

    [DispId(91)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetPageNumbering(
      [ComAliasName("ACPDFCREACTIVEX.acPageNumbersPositions"), In] acPageNumbersPositions Position,
      [MarshalAs(UnmanagedType.BStr), In] string Font,
      [In] int ExtraMarginHorz,
      [In] int ExtraMarginVert,
      [In] int Color,
      [In] int StartWithPage,
      [MarshalAs(UnmanagedType.BStr), In] string Format);

    [DispId(92)]
    public virtual extern int TemplateMode { [DispId(92), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(92), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(93)]
    public virtual extern int TemplateVisible { [DispId(93), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(93), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(94)]
    public virtual extern int TemplatePrint { [DispId(94), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(94), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(95)]
    public virtual extern int TemplateRepeat { [DispId(95), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(95), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(96)]
    public virtual extern int WantTabs { [DispId(96), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(96), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(97)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void StartSave([MarshalAs(UnmanagedType.BStr), In] string FileName, [ComAliasName("ACPDFCREACTIVEX.FileSaveOptionConstants"), In] FileSaveOptionConstants SaveOption);

    [DispId(98)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SavePage([In] int PageNumber);

    [DispId(99)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void EndSave();

    [DispId(100)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ClearPage([In] int PageNumber);

    [DispId(101)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Encrypt128([MarshalAs(UnmanagedType.BStr)] string OwnerPassword, [MarshalAs(UnmanagedType.BStr)] string UserPassword, int Options);

    [DispId(102)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int GetWarningLevel();

    [DispId(103)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ProcessHyperlinksBookmarksFromIniFile([MarshalAs(UnmanagedType.BStr), In] string iniFileName);

    [DispId(104)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AutoHyperlinks([MarshalAs(UnmanagedType.BStr), In] string prefixes);

    [DispId(105)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AutoBookmarks([In] int levels, [MarshalAs(UnmanagedType.BStr), In] string fonts, [In] int startPage);

    [DispId(106)]
    public virtual extern int RulerBackColor { [DispId(106), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(106), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(107)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ExportToTiff([MarshalAs(UnmanagedType.BStr), In] string FileName, [In] int Resolution, [In] int TiffFormat);

    [DispId(108)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void RotatePage([In] int PageNumber, [ComAliasName("ACPDFCREACTIVEX.RotatePageConstants"), In] RotatePageConstants Rotation);

    [DispId(109)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void MovePages([In] int PageNumber, [In] int PageCount, [In] int Destination);

    [DispId(200)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AddExternalObjectTemplate([MarshalAs(UnmanagedType.Interface), In] IacExternalObject externalObject);

    [DispId(201)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void CreateObjectEx(
      [ComAliasName("ACPDFCREACTIVEX.ObjectTypeConstants"), In] ObjectTypeConstants ObjectType,
      [MarshalAs(UnmanagedType.BStr), In] string Reference,
      [In] int subType,
      [In] int PageNumber);

    [DispId(202)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    public virtual extern object GetObjectsInRectangle(
      [In] int left,
      [In] int top,
      [In] int right,
      [In] int bottom,
      [In] int flags);

    [DispId(203)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetTargetDevmode([In] int hDevMode);

    [DispId(204)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void PrinterSetup();

    [DispId(205)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DigitalSignature(
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
    public virtual extern void GetFontAttributes(
      [MarshalAs(UnmanagedType.BStr), In] string FontName,
      out int FontDescent,
      out int FontAscent);

    [DispId(300)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetAttributeForMultipleSelectionEx(
      [MarshalAs(UnmanagedType.BStr), In] string attribName,
      [MarshalAs(UnmanagedType.Struct), In] object attribVal,
      [In] int CanUndo);

    [DispId(301)]
    [ComAliasName("ACPDFCREACTIVEX.acRefreshEventReasons")]
    public virtual extern acRefreshEventReasons RefreshEventReason { [DispId(301), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: ComAliasName("ACPDFCREACTIVEX.acRefreshEventReasons")] get; }

    [DispId(302)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetRefreshEventReason(
      [ComAliasName("ACPDFCREACTIVEX.acRefreshEventReasons"), In] acRefreshEventReasons newVal,
      [In] int bAddToExistingReason);

    [DispId(303)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SetToolTipText([MarshalAs(UnmanagedType.BStr)] string tooltipText);

    [DispId(304)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Interface)]
    public virtual extern acPlugin NewPluginBaseObject();

    [DispId(306)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetPlugin(
      [ComAliasName("ACPDFCREACTIVEX.CommandToolConstants"), In] CommandToolConstants PluginID,
      int bOnlyIfLoaded,
      [MarshalAs(UnmanagedType.IUnknown), In, Out] ref object ppUnknown);

    [DispId(350)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void StartSaveEx(
      [MarshalAs(UnmanagedType.BStr), In] string FileName,
      [ComAliasName("ACPDFCREACTIVEX.FileSaveOptionConstants"), In] FileSaveOptionConstants SaveOption,
      out int lpHandle);

    [DispId(351)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void SavePageEx([In] int Handle, [In] int PageNumber);

    [DispId(352)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void EndSaveEx([In] int Handle);

    [DispId(360)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DeleteAllHyperlinks();

    [DispId(361)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    public virtual extern object GetPageText([In] int PageNumber);

    [DispId(362)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string GetRawPageText([In] int PageNumber);

    [DispId(370)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void ExportToHTMLEx(
      [MarshalAs(UnmanagedType.BStr), In] string FileName,
      [ComAliasName("ACPDFCREACTIVEX.acHtmlExportOptions"), In] acHtmlExportOptions Options,
      [MarshalAs(UnmanagedType.BStr), In] string imageNamingInfo,
      [In] int downsamplingResolution);

    [DispId(371)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void AutoHyperlinksEx([MarshalAs(UnmanagedType.BStr), In] string prefixes, [In] int ClearExistingHyperlinks);

    [DispId(380)]
    public virtual extern string OptionalRegistryKeyForSavingSettings { [DispId(380), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.BStr)] get; [DispId(380), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(381)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetScreenCoordinates(
      [MarshalAs(UnmanagedType.BStr), In] string objectName,
      out int left,
      out int top,
      out int right,
      out int bottom);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetClassID(out Guid pClassID);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void IsDirty();

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Load([MarshalAs(UnmanagedType.Interface), In] IStream pstm);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void Save([MarshalAs(UnmanagedType.Interface), In] IStream pstm, [In] int fClearDirty);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void GetSizeMax(out _ULARGE_INTEGER pCbSize);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void InitNew();

    public virtual extern event _IPDFCreactiveXEvents_BeforeDeleteEventHandler BeforeDelete;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_BeforeDelete(
      [In] _IPDFCreactiveXEvents_BeforeDeleteEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_BeforeDelete(
      [In] _IPDFCreactiveXEvents_BeforeDeleteEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_PrintPageEventHandler _IPDFCreactiveXEvents_Event_PrintPage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_PrintPage([In] _IPDFCreactiveXEvents_PrintPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_PrintPage([In] _IPDFCreactiveXEvents_PrintPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_SavePage([In] _IPDFCreactiveXEvents_SavePageEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_SavePageEventHandler _IPDFCreactiveXEvents_Event_SavePage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_SavePage([In] _IPDFCreactiveXEvents_SavePageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_ClickHyperlink(
      [In] _IPDFCreactiveXEvents_ClickHyperlinkEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_ClickHyperlinkEventHandler ClickHyperlink;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_ClickHyperlink(
      [In] _IPDFCreactiveXEvents_ClickHyperlinkEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_RefreshEventHandler _IPDFCreactiveXEvents_Event_Refresh;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_Refresh([In] _IPDFCreactiveXEvents_RefreshEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_Refresh([In] _IPDFCreactiveXEvents_RefreshEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_SelectedObjectChange(
      [In] _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler SelectedObjectChange;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_SelectedObjectChange(
      [In] _IPDFCreactiveXEvents_SelectedObjectChangeEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_ObjectTextChange(
      [In] _IPDFCreactiveXEvents_ObjectTextChangeEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_ObjectTextChangeEventHandler ObjectTextChange;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_ObjectTextChange(
      [In] _IPDFCreactiveXEvents_ObjectTextChangeEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler ContextSensitiveMenu;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_ContextSensitiveMenu(
      [In] _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_ContextSensitiveMenu(
      [In] _IPDFCreactiveXEvents_ContextSensitiveMenuEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_MouseDown([In] _IPDFCreactiveXEvents_MouseDownEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_MouseDownEventHandler MouseDown;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_MouseDown([In] _IPDFCreactiveXEvents_MouseDownEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_MouseMoveEventHandler MouseMove;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_MouseMove([In] _IPDFCreactiveXEvents_MouseMoveEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_MouseMove([In] _IPDFCreactiveXEvents_MouseMoveEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_MouseUpEventHandler MouseUp;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_MouseUp([In] _IPDFCreactiveXEvents_MouseUpEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_MouseUp([In] _IPDFCreactiveXEvents_MouseUpEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_NewObjectEventHandler NewObject;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_NewObject([In] _IPDFCreactiveXEvents_NewObjectEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_NewObject([In] _IPDFCreactiveXEvents_NewObjectEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_ActivateObjectEventHandler _IPDFCreactiveXEvents_Event_ActivateObject;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_ActivateObject(
      [In] _IPDFCreactiveXEvents_ActivateObjectEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_ActivateObject(
      [In] _IPDFCreactiveXEvents_ActivateObjectEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_LoadPageEventHandler LoadPage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_LoadPage([In] _IPDFCreactiveXEvents_LoadPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_LoadPage([In] _IPDFCreactiveXEvents_LoadPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_EvaluateExpression(
      [In] _IPDFCreactiveXEvents_EvaluateExpressionEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_EvaluateExpressionEventHandler EvaluateExpression;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_EvaluateExpression(
      [In] _IPDFCreactiveXEvents_EvaluateExpressionEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_ProcessingProgress(
      [In] _IPDFCreactiveXEvents_ProcessingProgressEventHandler obj0);

    public virtual extern event _IPDFCreactiveXEvents_ProcessingProgressEventHandler ProcessingProgress;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_ProcessingProgress(
      [In] _IPDFCreactiveXEvents_ProcessingProgressEventHandler obj0);
  }
}
