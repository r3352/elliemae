// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.UI.eFolderUIHelper
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.UI
{
  public class eFolderUIHelper
  {
    [DllImport("user32.dll")]
    public static extern IntPtr CreateIconIndirect(ref eFolderUIHelper.IconInfo icon);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetIconInfo(IntPtr hIcon, ref eFolderUIHelper.IconInfo pIconInfo);

    public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
    {
      eFolderUIHelper.IconInfo iconInfo = new eFolderUIHelper.IconInfo();
      eFolderUIHelper.GetIconInfo(bmp.GetHicon(), ref iconInfo);
      iconInfo.xHotspot = xHotSpot;
      iconInfo.yHotspot = yHotSpot;
      iconInfo.fIcon = false;
      return new Cursor(eFolderUIHelper.CreateIconIndirect(ref iconInfo));
    }

    public static void SetPageThumbnailCursor(PageImage[] pageList)
    {
      if (pageList.Length > 1)
        Cursor.Current = new Cursor(Resources.MultiDocCursor.Handle);
      else
        Cursor.Current = new Cursor(Resources.DocCursor.Handle);
    }

    public static bool IsAnnotationVisible(PageAnnotation annotation)
    {
      return new eFolderAccessRights(Session.LoanDataMgr).CanViewAllAnnotations || annotation.Visibility != PageAnnotationVisibilityType.Personal || annotation.AddedBy == Session.UserInfo.FullName;
    }

    public struct IconInfo
    {
      public bool fIcon;
      public int xHotspot;
      public int yHotspot;
      public IntPtr hbmMask;
      public IntPtr hbmColor;
    }
  }
}
