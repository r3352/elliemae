// Decompiled with JetBrains decompiler
// Type: EncompassServerConnectionException
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;

#nullable disable
public sealed class EncompassServerConnectionException : Exception
{
  public EncompassServerConnectionException(string message)
    : base(message)
  {
  }

  public EncompassServerConnectionException(string message, Exception inner)
    : base(message, inner)
  {
  }
}
