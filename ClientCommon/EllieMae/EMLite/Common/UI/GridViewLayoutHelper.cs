// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.GridViewLayoutHelper
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public static class GridViewLayoutHelper
  {
    public static int GetMaximumOptionWidth(FieldDefinition fieldDef, Graphics g, Font font)
    {
      if (fieldDef.Options.Count == 0)
        return 0;
      int maximumOptionWidth = 0;
      foreach (FieldOption option in fieldDef.Options)
      {
        int num = (int) g.MeasureString(option.Text, font).Width + 1;
        if (num > maximumOptionWidth)
          maximumOptionWidth = num;
      }
      return maximumOptionWidth;
    }

    public static int GetDefaultColumnWidth(FieldDefinition fieldDef, Graphics g, Font font)
    {
      int maximumOptionWidth = GridViewLayoutHelper.GetMaximumOptionWidth(fieldDef, g, font);
      return maximumOptionWidth > 0 ? Math.Min(maximumOptionWidth + 30, 200) : GridViewLayoutHelper.GetDefaultColumnWidth(fieldDef.Format);
    }

    public static int GetDefaultColumnWidth(FieldFormat format)
    {
      if (FieldFormatEnumUtil.IsNumeric(format))
        return 100;
      switch (format)
      {
        case FieldFormat.YN:
          return 40;
        case FieldFormat.X:
          return 40;
        case FieldFormat.DATE:
          return 80;
        case FieldFormat.MONTHDAY:
          return 60;
        case FieldFormat.DATETIME:
          return 80;
        default:
          return 180;
      }
    }
  }
}
