// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.TabIndexSortComparer
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  internal class TabIndexSortComparer : IComparer
  {
    public int Compare(object x, object y)
    {
      return ((FieldControl) x).TabIndex - ((FieldControl) y).TabIndex;
    }
  }
}
