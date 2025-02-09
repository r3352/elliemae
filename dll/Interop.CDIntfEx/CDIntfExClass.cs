// Decompiled with JetBrains decompiler
// Type: CDIntfEx.CDIntfExClass
// Assembly: Interop.CDIntfEx, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E0D8D59F-38F8-4E65-9D3A-50B747C0491E
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.CDIntfEx.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace CDIntfEx
{
  [TypeLibType(34)]
  [ClassInterface(0)]
  [ComSourceInterfaces("CDIntfEx._DCDIntfEvents\0\0")]
  [Guid("68B34268-7559-11D3-BBE5-D53DCBD65107")]
  [ComImport]
  public class CDIntfExClass : ICDIntfEx, CDIntfEx.CDIntfEx, _DCDIntfEvents_Event
  {
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public extern CDIntfExClass();

    [DispId(1)]
    public virtual extern bool FontEmbedding { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(2)]
    public virtual extern bool PageContentCompression { [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(3)]
    public virtual extern bool JPEGCompression { [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(4)]
    public virtual extern short PaperSize { [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(5)]
    public virtual extern int PaperWidth { [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(6)]
    public virtual extern int PaperLength { [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(7)]
    public virtual extern short Orientation { [DispId(7), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(7), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(8)]
    public virtual extern int Resolution { [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(9)]
    public virtual extern string DefaultDirectory { [DispId(9), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(9), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(10)]
    public virtual extern string DefaultFileName { [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(11)]
    public virtual extern short FileNameOptions { [DispId(11), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(11), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(12)]
    public virtual extern short HorizontalMargin { [DispId(12), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(12), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(13)]
    public virtual extern short VerticalMargin { [DispId(13), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(13), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(31)]
    public virtual extern int Attributes { [DispId(31), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(31), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(41)]
    public virtual extern short JPegLevel { [DispId(41), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(41), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(45)]
    public virtual extern string ServerAddress { [DispId(45), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(45), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(46)]
    public virtual extern int ServerPort { [DispId(46), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(46), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(47)]
    public virtual extern string ServerUsername { [DispId(47), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(47), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(48)]
    public virtual extern string EmailFieldTo { [DispId(48), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(48), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(49)]
    public virtual extern string EmailFieldCC { [DispId(49), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(49), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(50)]
    public virtual extern string EmailFieldBCC { [DispId(50), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(51)]
    public virtual extern string EmailSubject { [DispId(51), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(51), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(52)]
    public virtual extern string EmailMessage { [DispId(52), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(52), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(53)]
    public virtual extern bool EmailPrompt { [DispId(53), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(53), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(60)]
    public virtual extern int FileNameOptionsEx { [DispId(60), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(60), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(61)]
    public virtual extern int DevmodeFlags { [DispId(61), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(61), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(62)]
    public virtual extern string EmailFieldFrom { [DispId(62), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(62), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(63)]
    public virtual extern string SmtpServer { [DispId(63), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(63), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(64)]
    public virtual extern int SmtpPort { [DispId(64), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(64), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(65)]
    public virtual extern string TargetPrinterName { [DispId(65), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(65), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(66)]
    public virtual extern string PageProcessor { [DispId(66), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(66), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(69)]
    public virtual extern int EmailOptions { [DispId(69), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(69), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(72)]
    public virtual extern int PrinterLanguage { [DispId(72), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(72), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(73)]
    public virtual extern string PrinterParamStr { [DispId(73), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(73), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(74)]
    public virtual extern int PrinterParamInt { [DispId(74), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(74), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(76)]
    public virtual extern string OwnerPassword { [DispId(76), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(76), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(77)]
    public virtual extern string UserPassword { [DispId(77), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(77), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(78)]
    public virtual extern int Permissions { [DispId(78), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(78), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(79)]
    public virtual extern int ImageOptions { [DispId(79), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(79), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(81)]
    public virtual extern bool SimPostscript { [DispId(81), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(81), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(16)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: ComAliasName("stdole.OLE_HANDLE")]
    public virtual extern int CreateDC();

    [DispId(17)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern bool SetDefaultConfig();

    [DispId(18)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern bool SetDefaultPrinter();

    [DispId(19)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern bool StartSpooler();

    [DispId(20)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern bool StopSpooler();

    [DispId(21)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int PDFDriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(22)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int HTMLDriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(23)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int EMFDriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(24)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void DriverEnd();

    [DispId(25)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string GetLastErrorMsg();

    [DispId(26)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern bool RestoreDefaultPrinter();

    [DispId(27)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int DriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(28)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string GetDocumentTitle(int JobID);

    [DispId(29)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SetBookmark(int hDC, int lParent, [MarshalAs(UnmanagedType.BStr)] string Title);

    [DispId(30)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int CaptureEvents(int bCapture);

    [DispId(32)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SetWatermark(
      [MarshalAs(UnmanagedType.BStr)] string Watermark,
      [MarshalAs(UnmanagedType.BStr)] string FontName,
      short FontSize,
      short Orientation,
      int Color,
      int HorzPos,
      int VertPos,
      int Foreground);

    [DispId(33)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SetHyperLink(int hDC, [MarshalAs(UnmanagedType.BStr)] string URL);

    [DispId(34)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern bool SetDefaultConfigEx();

    [DispId(35)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int RTFDriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(39)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SendMessagesTo([MarshalAs(UnmanagedType.BStr)] string WndClass);

    [DispId(40)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int BatchConvert([MarshalAs(UnmanagedType.BStr)] string FileName);

    [DispId(42)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int Lock([MarshalAs(UnmanagedType.BStr)] string szLockName);

    [DispId(43)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int Unlock([MarshalAs(UnmanagedType.BStr)] string szLockName, int dwTimeout);

    [DispId(44)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SendMail(
      [MarshalAs(UnmanagedType.BStr)] string szTo,
      [MarshalAs(UnmanagedType.BStr)] string szCC,
      [MarshalAs(UnmanagedType.BStr)] string szBCC,
      [MarshalAs(UnmanagedType.BStr)] string szSubject,
      [MarshalAs(UnmanagedType.BStr)] string szMessage,
      [MarshalAs(UnmanagedType.BStr)] string szFilenames,
      int lOptions);

    [DispId(54)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int TestLock([MarshalAs(UnmanagedType.BStr)] string szLockName);

    [DispId(55)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int GLock();

    [DispId(56)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int GUnlock();

    [DispId(57)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SetDocEmailProps(
      [MarshalAs(UnmanagedType.BStr)] string szDocTitle,
      [MarshalAs(UnmanagedType.BStr)] string szTo,
      [MarshalAs(UnmanagedType.BStr)] string szCC,
      [MarshalAs(UnmanagedType.BStr)] string szBCC,
      [MarshalAs(UnmanagedType.BStr)] string szSubject,
      [MarshalAs(UnmanagedType.BStr)] string szMessage,
      int lPrompt);

    [DispId(58)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SetDocServerProps(
      [MarshalAs(UnmanagedType.BStr)] string szDocTitle,
      [MarshalAs(UnmanagedType.BStr)] string szHostname,
      [MarshalAs(UnmanagedType.BStr)] string szUsername);

    [DispId(59)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SetDocFileProps(
      [MarshalAs(UnmanagedType.BStr)] string szDocTitle,
      int lOptions,
      [MarshalAs(UnmanagedType.BStr)] string szFileDir,
      [MarshalAs(UnmanagedType.BStr)] string szFileName);

    [DispId(67)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SendSmtpMail(
      [MarshalAs(UnmanagedType.BStr)] string szHostname,
      int lPort,
      [MarshalAs(UnmanagedType.BStr)] string szFrom,
      [MarshalAs(UnmanagedType.BStr)] string szTo,
      [MarshalAs(UnmanagedType.BStr)] string szCC,
      [MarshalAs(UnmanagedType.BStr)] string szBCC,
      [MarshalAs(UnmanagedType.BStr)] string szSubject,
      [MarshalAs(UnmanagedType.BStr)] string szMessage,
      [MarshalAs(UnmanagedType.BStr)] string szFilenames);

    [DispId(68)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int SetDocEmailPropsEx(
      [MarshalAs(UnmanagedType.BStr)] string szDocTitle,
      [MarshalAs(UnmanagedType.BStr)] string szSmtpServer,
      int lSmtpPort,
      [MarshalAs(UnmanagedType.BStr)] string szFrom,
      [MarshalAs(UnmanagedType.BStr)] string szTo,
      [MarshalAs(UnmanagedType.BStr)] string szCC,
      [MarshalAs(UnmanagedType.BStr)] string szBCC,
      [MarshalAs(UnmanagedType.BStr)] string szSubject,
      [MarshalAs(UnmanagedType.BStr)] string szMessage,
      int lFlags);

    [DispId(70)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern int EnablePrinter([MarshalAs(UnmanagedType.BStr)] string Company, [MarshalAs(UnmanagedType.BStr)] string Code);

    [DispId(71)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern bool KeepPreProcessed(int fKeep);

    [DispId(75)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    public virtual extern string GetGeneratedFilename();

    [DispId(80)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    public virtual extern object GetData();

    public virtual extern event _DCDIntfEvents_StartDocPreEventHandler StartDocPre;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_StartDocPre([In] _DCDIntfEvents_StartDocPreEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_StartDocPre([In] _DCDIntfEvents_StartDocPreEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_StartDocPost([In] _DCDIntfEvents_StartDocPostEventHandler obj0);

    public virtual extern event _DCDIntfEvents_StartDocPostEventHandler StartDocPost;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_StartDocPost([In] _DCDIntfEvents_StartDocPostEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_EndDocPre([In] _DCDIntfEvents_EndDocPreEventHandler obj0);

    public virtual extern event _DCDIntfEvents_EndDocPreEventHandler EndDocPre;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_EndDocPre([In] _DCDIntfEvents_EndDocPreEventHandler obj0);

    public virtual extern event _DCDIntfEvents_EndDocPostEventHandler EndDocPost;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_EndDocPost([In] _DCDIntfEvents_EndDocPostEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_EndDocPost([In] _DCDIntfEvents_EndDocPostEventHandler obj0);

    public virtual extern event _DCDIntfEvents_StartPageEventHandler StartPage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_StartPage([In] _DCDIntfEvents_StartPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_StartPage([In] _DCDIntfEvents_StartPageEventHandler obj0);

    public virtual extern event _DCDIntfEvents_EndPageEventHandler EndPage;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_EndPage([In] _DCDIntfEvents_EndPageEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_EndPage([In] _DCDIntfEvents_EndPageEventHandler obj0);

    public virtual extern event _DCDIntfEvents_EnabledPreEventHandler EnabledPre;

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void add_EnabledPre([In] _DCDIntfEvents_EnabledPreEventHandler obj0);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    public virtual extern void remove_EnabledPre([In] _DCDIntfEvents_EnabledPreEventHandler obj0);
  }
}
