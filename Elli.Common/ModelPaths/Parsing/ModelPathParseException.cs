// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.ModelPathParseException
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class ModelPathParseException : Exception
  {
    public ModelPathParseException(char c, int i, string path, string message = null)
      : base(ModelPathParseException.GetMessage(c, i, path, message))
    {
      this.ModelPath = path;
      this.InvalidChar = c;
      this.InvalidCharLocation = i;
    }

    public string ModelPath { get; }

    public char InvalidChar { get; }

    public int InvalidCharLocation { get; }

    private static string GetMessage(char c, int i, string path, string message)
    {
      string message1 = string.Format("Invalid character '{0}' at index {1} for model path \"{2}\"", (object) c, (object) i, (object) path);
      if (!string.IsNullOrWhiteSpace(message))
        message1 = message1 + ". " + message;
      return message1;
    }
  }
}
