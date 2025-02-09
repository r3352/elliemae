// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.TabIndexSortComparer
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Used to sort FieldControl objects by their tab index</summary>
  internal class TabIndexSortComparer : IComparer
  {
    public int Compare(object x, object y)
    {
      return ((FieldControl) x).TabIndex - ((FieldControl) y).TabIndex;
    }
  }
}
