// Decompiled with JetBrains decompiler
// Type: ACPDFCREACTIVEX.__MIDL_IacPDFCreactiveXConst_0051
// Assembly: Interop.ACPDFCREACTIVEX, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4DA5DA38-3850-4E97-8521-1A0336DA34F6
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\Interop.ACPDFCREACTIVEX.dll

#nullable disable
namespace ACPDFCREACTIVEX
{
  public enum __MIDL_IacPDFCreactiveXConst_0051
  {
    acRefreshReasonUnknown = 0,
    acRefreshReportStateChange = 1,
    acRefreshZoomFactorChange = 2,
    acRefreshObjectCreated = 4,
    acRefreshSelectedObjectChange = 8,
    acRefreshObjectActiveStateChange = 16, // 0x00000010
    acRefreshObjectMoved = 16, // 0x00000010
    acRefreshCurrentPageChanged = 32, // 0x00000020
    acRefreshPageScrolled = 64, // 0x00000040
    acRefreshPageDeleted = 128, // 0x00000080
    acRefreshPageAdded = 256, // 0x00000100
    acRefreshPageScrolledIntoView = 512, // 0x00000200
    acRefreshObjectSetAttributeValue = 1024, // 0x00000400
    acRefreshObjectPasted = 2048, // 0x00000800
    acRefreshSelectedObjectsDeleted = 4096, // 0x00001000
    acRefreshDocumentOpened = 8192, // 0x00002000
    acRefreshPageRotated = 16384, // 0x00004000
    acRefreshBookmarkChange = 32768, // 0x00008000
    acRefreshReportViewChange = 131072, // 0x00020000
    acRefreshPagesMoved = 262144, // 0x00040000
  }
}
