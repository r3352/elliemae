// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.IacDC
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace ACPDFCREACTIVEX
{
  [Guid("AAC07D86-4585-4214-9A6B-D55BD32011A4")]
  [TypeLibType(4160)]
  [ComImport]
  public interface IacDC
  {
    [DispId(1)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawLink(
      [In] int srcX1,
      [In] int srcY1,
      [In] int srcX2,
      [In] int srcY2,
      [In] int destX1,
      [In] int destY1,
      [In] int destX2,
      [In] int destY2,
      [In] int pointX,
      [In] int pointY,
      [In] int style,
      [In] int curvature);

    [DispId(2)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawLine([In] int left, [In] int top, [In] int right, [In] int bottom);

    [DispId(3)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawRectangle([In] int left, [In] int top, [In] int right, [In] int bottom);

    [DispId(4)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawCurve(
      [In] int boundLeft,
      [In] int boundTop,
      [In] int boundRight,
      [In] int boundBottom,
      [In] int left,
      [In] int top,
      [In] int right,
      [In] int bottom);

    [DispId(5)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawPolygon([MarshalAs(UnmanagedType.Struct), In] object points, [MarshalAs(UnmanagedType.Struct), In] object ops, [In] int fillMode);

    [DispId(6)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawOval([In] int left, [In] int top, [In] int right, [In] int bottom);

    [DispId(7)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawRoundRectangle([In] int left, [In] int top, [In] int right, [In] int bottom, [In] int roundingRadius);

    [DispId(8)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void DrawImage(
      [In] int left,
      [In] int top,
      [In] int right,
      [In] int bottom,
      [In] int imageWidth,
      [In] int imageHeight,
      [In] short bpc,
      [MarshalAs(UnmanagedType.Struct), In] object bits,
      [In] int size,
      [MarshalAs(UnmanagedType.Struct), In] object palette,
      [In] int mask);

    [DispId(9)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void TextOut(
      [In] int X,
      [In] int Y,
      [MarshalAs(UnmanagedType.BStr), In] string Text,
      [In] int length,
      [In] int angle,
      [In] int scale,
      [In] int charSpacing,
      [In] int wordSpacing);

    [DispId(10)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool IsPrinting();

    [DispId(11)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetPrinting([In] bool IsPrinting);

    [DispId(12)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    bool IsSkippingFormObjects();

    [DispId(13)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SkipFormObjects([In] bool bSkip);

    [DispId(14)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetBackgroundMode([In] int mode);

    [DispId(15)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetPen([In] int style, [In] int width, [In] int Color, [MarshalAs(UnmanagedType.Struct), In] object styles);

    [DispId(16)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetBrush([In] int style, [In] int Color);

    [DispId(17)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetFont([MarshalAs(UnmanagedType.Struct), In] object Font);

    [DispId(18)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetTextColor([In] int Color);

    [DispId(19)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetHyperlink([MarshalAs(UnmanagedType.BStr), In] string Hyperlink);

    [DispId(20)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetCurrentMode([In] short mode);

    [DispId(21)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetDrawingOrigin([In] int X, [In] int Y);

    [DispId(22)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetClippingPath([MarshalAs(UnmanagedType.Struct), In] object points, [MarshalAs(UnmanagedType.Struct), In] object vertices, [MarshalAs(UnmanagedType.Struct), In] object ops, [In] int clipmode);

    [DispId(23)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void InitGraphicsState([In] int left, [In] int top, [In] int right, [In] int bottom, [In] double rotate);

    [DispId(24)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void ClearGraphicsState();

    [DispId(25)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartDocument([In] int Color, [In] int width, [In] int height);

    [DispId(26)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndDocument();

    [DispId(27)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void startPage([In] int width, [In] int height);

    [DispId(28)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndPage();

    [DispId(29)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartTable([In] int borderWidth, [In] int positionTop, [In] int positionLeft);

    [DispId(30)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndTable();

    [DispId(31)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartRow([In] int rowHeight);

    [DispId(32)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndRow();

    [DispId(33)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartCell(
      [In] int width,
      [In] int height,
      [In] int hAlignment,
      [In] int vAlignment,
      [In] int hBorders,
      [In] int vBorders,
      [In] int backColor,
      [In] int borderColor,
      [In] int borderWidth,
      [MarshalAs(UnmanagedType.Struct), In] object cellSpan);

    [DispId(34)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndCell();

    [DispId(35)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartParagraph([In] int left, [In] int top, [In] int right, [In] int bottom, [MarshalAs(UnmanagedType.Struct)] object textInfo);

    [DispId(36)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndParagraph();

    [DispId(37)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void StartLine([MarshalAs(UnmanagedType.BStr), In] string Text, [In] int positionTop, [In] int positionLeft);

    [DispId(39)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EndLine();

    [DispId(40)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetImageCompression([In] int compression);

    [DispId(41)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int getImageCompression();

    [DispId(42)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void writeJPEGFile(
      [MarshalAs(UnmanagedType.Struct), In] object stream,
      [In] int bpc,
      [In] int width,
      [In] int height,
      [MarshalAs(UnmanagedType.Struct), In] object data,
      [In] int isHexData);

    [DispId(43)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int StoreCharDistances(
      [MarshalAs(UnmanagedType.Struct), In] object dx,
      [MarshalAs(UnmanagedType.BStr), In] string str,
      [In] int length,
      [MarshalAs(UnmanagedType.Struct), In] object Font,
      [In] int wordSpacing,
      [In] int charSpacing,
      [In] int scale);

    [DispId(44)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    double GetTextExtent([MarshalAs(UnmanagedType.Struct), In] object Font, [MarshalAs(UnmanagedType.BStr), In] string str, [In] int length, [In] int wordSpacing, [In] int charSpacing);

    [DispId(1610743851)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetTextExtentEx(
      [MarshalAs(UnmanagedType.Struct), In] object Font,
      [MarshalAs(UnmanagedType.BStr), In] string str,
      [In] int length,
      [In] double pointSize,
      [In] int wordSpacing,
      [In] int charSpacing);

    [DispId(45)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    double GetCharWidth([MarshalAs(UnmanagedType.Struct), In] object Font, [In] int character);

    [DispId(46)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetCharHeight([MarshalAs(UnmanagedType.Struct), In] object Font);

    [DispId(47)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetCharAscend([MarshalAs(UnmanagedType.Struct), In] object Font);

    [DispId(48)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetCharDescend([MarshalAs(UnmanagedType.Struct), In] object Font);

    [DispId(49)]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetCellHeight([MarshalAs(UnmanagedType.Struct), In] object Font);
  }
}
