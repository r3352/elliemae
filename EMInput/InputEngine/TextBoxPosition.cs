// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.TextBoxPosition
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class TextBoxPosition
  {
    public int Row;
    public int Col;
    public string Val = string.Empty;

    internal TextBoxPosition(int row, int col)
      : this(row, col, "")
    {
    }

    internal TextBoxPosition(int row, int col, string val)
    {
      this.Row = row;
      this.Col = col;
      this.Val = val;
    }
  }
}
