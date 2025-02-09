// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FieldIDElement
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.UI;
using System;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FieldIDElement : TextElement, IComparable
  {
    private string sortValue;

    public FieldIDElement(string fieldId)
      : base(fieldId)
    {
      this.sortValue = FieldIDElement.GetSortValueForFieldID(fieldId);
    }

    public int CompareTo(object obj)
    {
      return !(obj is FieldIDElement fieldIdElement) ? 1 : string.Compare(this.sortValue, fieldIdElement.sortValue, true);
    }

    public override string ToString() => this.Text;

    public override bool Equals(object obj)
    {
      return obj is FieldIDElement fieldIdElement && string.Compare(fieldIdElement.ToString(), this.ToString(), true) == 0;
    }

    public override int GetHashCode() => this.ToString().ToUpper().GetHashCode();

    public static string GetSortValueForFieldID(string fieldId)
    {
      try
      {
        int.Parse(fieldId);
        return new string('0', 10 - fieldId.Length) + fieldId;
      }
      catch
      {
        return fieldId;
      }
    }
  }
}
