// Decompiled with JetBrains decompiler
// Type: CDIntfEx.ICDIntfEx
// Assembly: Interop.CDIntfEx, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E0D8D59F-38F8-4E65-9D3A-50B747C0491E
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.CDIntfEx.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace CDIntfEx
{
  [Guid("68B34269-7559-11D3-BBE5-D53DCBD65107")]
  [TypeLibType(4096)]
  [InterfaceType(2)]
  [ComImport]
  public interface ICDIntfEx
  {
    [DispId(1)]
    bool FontEmbedding { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(2)]
    bool PageContentCompression { [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(3)]
    bool JPEGCompression { [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(4)]
    short PaperSize { [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(4), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(5)]
    int PaperWidth { [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(5), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(6)]
    int PaperLength { [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(6), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(7)]
    short Orientation { [DispId(7), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(7), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(8)]
    int Resolution { [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(8), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(9)]
    string DefaultDirectory { [DispId(9), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(9), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(10)]
    string DefaultFileName { [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(11)]
    short FileNameOptions { [DispId(11), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(11), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(12)]
    short HorizontalMargin { [DispId(12), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(12), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(13)]
    short VerticalMargin { [DispId(13), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(13), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(31)]
    int Attributes { [DispId(31), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(31), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(41)]
    short JPegLevel { [DispId(41), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(41), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(45)]
    string ServerAddress { [DispId(45), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(45), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(46)]
    int ServerPort { [DispId(46), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(46), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(47)]
    string ServerUsername { [DispId(47), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(47), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(48)]
    string EmailFieldTo { [DispId(48), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(48), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(49)]
    string EmailFieldCC { [DispId(49), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(49), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(50)]
    string EmailFieldBCC { [DispId(50), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(50), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(51)]
    string EmailSubject { [DispId(51), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(51), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(52)]
    string EmailMessage { [DispId(52), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(52), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(53)]
    bool EmailPrompt { [DispId(53), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(53), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(60)]
    int FileNameOptionsEx { [DispId(60), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(60), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(61)]
    int DevmodeFlags { [DispId(61), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(61), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(62)]
    string EmailFieldFrom { [DispId(62), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(62), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(63)]
    string SmtpServer { [DispId(63), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(63), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(64)]
    int SmtpPort { [DispId(64), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(64), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(65)]
    string TargetPrinterName { [DispId(65), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(65), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(66)]
    string PageProcessor { [DispId(66), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(66), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(69)]
    int EmailOptions { [DispId(69), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(69), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(72)]
    int PrinterLanguage { [DispId(72), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(72), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(73)]
    string PrinterParamStr { [DispId(73), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(73), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(74)]
    int PrinterParamInt { [DispId(74), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(74), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(76)]
    string OwnerPassword { [DispId(76), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(76), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(77)]
    string UserPassword { [DispId(77), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(77), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(78)]
    int Permissions { [DispId(78), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(78), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(79)]
    int ImageOptions { [DispId(79), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(79), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(81)]
    bool SimPostscript { [DispId(81), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(81), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

    [DispId(16)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: ComAliasName("stdole.OLE_HANDLE")]
    int CreateDC();

    [DispId(17)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool SetDefaultConfig();

    [DispId(18)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool SetDefaultPrinter();

    [DispId(19)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool StartSpooler();

    [DispId(20)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool StopSpooler();

    [DispId(21)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int PDFDriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(22)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int HTMLDriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(23)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int EMFDriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(24)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DriverEnd();

    [DispId(25)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string GetLastErrorMsg();

    [DispId(26)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool RestoreDefaultPrinter();

    [DispId(27)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int DriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(28)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string GetDocumentTitle(int JobID);

    [DispId(29)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int SetBookmark(int hDC, int lParent, [MarshalAs(UnmanagedType.BStr)] string Title);

    [DispId(30)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int CaptureEvents(int bCapture);

    [DispId(32)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int SetWatermark(
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
    int SetHyperLink(int hDC, [MarshalAs(UnmanagedType.BStr)] string URL);

    [DispId(34)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool SetDefaultConfigEx();

    [DispId(35)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int RTFDriverInit([MarshalAs(UnmanagedType.BStr)] string PrinterName);

    [DispId(39)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int SendMessagesTo([MarshalAs(UnmanagedType.BStr)] string WndClass);

    [DispId(40)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int BatchConvert([MarshalAs(UnmanagedType.BStr)] string FileName);

    [DispId(42)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int Lock([MarshalAs(UnmanagedType.BStr)] string szLockName);

    [DispId(43)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int Unlock([MarshalAs(UnmanagedType.BStr)] string szLockName, int dwTimeout);

    [DispId(44)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int SendMail(
      [MarshalAs(UnmanagedType.BStr)] string szTo,
      [MarshalAs(UnmanagedType.BStr)] string szCC,
      [MarshalAs(UnmanagedType.BStr)] string szBCC,
      [MarshalAs(UnmanagedType.BStr)] string szSubject,
      [MarshalAs(UnmanagedType.BStr)] string szMessage,
      [MarshalAs(UnmanagedType.BStr)] string szFilenames,
      int lOptions);

    [DispId(54)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int TestLock([MarshalAs(UnmanagedType.BStr)] string szLockName);

    [DispId(55)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GLock();

    [DispId(56)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GUnlock();

    [DispId(57)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int SetDocEmailProps(
      [MarshalAs(UnmanagedType.BStr)] string szDocTitle,
      [MarshalAs(UnmanagedType.BStr)] string szTo,
      [MarshalAs(UnmanagedType.BStr)] string szCC,
      [MarshalAs(UnmanagedType.BStr)] string szBCC,
      [MarshalAs(UnmanagedType.BStr)] string szSubject,
      [MarshalAs(UnmanagedType.BStr)] string szMessage,
      int lPrompt);

    [DispId(58)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int SetDocServerProps([MarshalAs(UnmanagedType.BStr)] string szDocTitle, [MarshalAs(UnmanagedType.BStr)] string szHostname, [MarshalAs(UnmanagedType.BStr)] string szUsername);

    [DispId(59)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int SetDocFileProps([MarshalAs(UnmanagedType.BStr)] string szDocTitle, int lOptions, [MarshalAs(UnmanagedType.BStr)] string szFileDir, [MarshalAs(UnmanagedType.BStr)] string szFileName);

    [DispId(67)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int SendSmtpMail(
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
    int SetDocEmailPropsEx(
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
    int EnablePrinter([MarshalAs(UnmanagedType.BStr)] string Company, [MarshalAs(UnmanagedType.BStr)] string Code);

    [DispId(71)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool KeepPreProcessed(int fKeep);

    [DispId(75)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.BStr)]
    string GetGeneratedFilename();

    [DispId(80)]
    [MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    [return: MarshalAs(UnmanagedType.Struct)]
    object GetData();
  }
}
