// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.ExceptionExtensions
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using Encompass.Diagnostics.Logging.Schema;
using System;
using System.Collections.Generic;

#nullable disable
namespace Encompass.Diagnostics
{
  public static class ExceptionExtensions
  {
    public static string GetFullStackTrace(this Exception ex)
    {
      if (ex == null)
        return (string) null;
      return ex.InnerException.GetFullStackTrace() + "[" + ex.GetType().FullName + "] " + ex.Message + Environment.NewLine + (ex.StackTrace ?? string.Empty) + Environment.NewLine + Environment.NewLine;
    }

    public static string GetFullStackTrace(this LogErrorData error)
    {
      if (error == null)
        return (string) null;
      return error.Xcause.GetFullStackTrace() + "[" + error.Type + "] " + error.Message + Environment.NewLine + (error.StackTrace ?? string.Empty) + Environment.NewLine + Environment.NewLine;
    }

    public static T FindType<T>(this Exception exception) where T : Exception
    {
      Stack<Exception> exceptionStack = new Stack<Exception>();
      for (; exception != null; exception = exceptionStack.Count > 0 ? exceptionStack.Pop() : (Exception) null)
      {
        T cast;
        if (isType(exception, out cast) || isType(exception?.InnerException, out cast))
          return cast;
        if (exception.InnerException != null)
          exceptionStack.Push(exception.InnerException);
        if (exception is AggregateException aggregateException)
        {
          foreach (Exception innerException in aggregateException.InnerExceptions)
            exceptionStack.Push(innerException);
        }
      }
      return default (T);

      static bool isType(Exception ex, out T cast) => (object) (cast = ex as T) != null;
    }
  }
}
