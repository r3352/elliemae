// Decompiled with JetBrains decompiler
// Type: Elli.Common.ModelPaths.Parsing.IncompleteModelPathParseException
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common.ModelPaths.Parsing
{
  public class IncompleteModelPathParseException : Exception
  {
    public IncompleteModelPathParseException(string path, string message = null)
      : base(IncompleteModelPathParseException.GetMessage(path, message))
    {
      this.ModelPath = path;
    }

    public string ModelPath { get; }

    private static string GetMessage(string path, string message)
    {
      string message1 = "Incomplete model path: \"" + path + "\"";
      if (!string.IsNullOrWhiteSpace(message))
        message1 = message1 + ". " + message;
      return message1;
    }
  }
}
