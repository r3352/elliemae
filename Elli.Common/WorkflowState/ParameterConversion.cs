// Decompiled with JetBrains decompiler
// Type: Elli.Common.WorkflowState.ParameterConversion
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using System;

#nullable disable
namespace Elli.Common.WorkflowState
{
  internal static class ParameterConversion
  {
    public static object Unpack(object[] args, Type argType, int index)
    {
      Enforce.ArgumentNotNull<object[]>(args, nameof (args));
      if (args.Length <= index)
        throw new ArgumentException(string.Format("An argument of type {0} is required in position {1}.", (object) argType, (object) index));
      object obj = args[index];
      return obj == null || argType.IsAssignableFrom(obj.GetType()) ? obj : throw new ArgumentException(string.Format("The argument in position {0} is of type {1} but must be of type {2}.", (object) index, (object) obj.GetType(), (object) argType));
    }

    public static TArg Unpack<TArg>(object[] args, int index)
    {
      return (TArg) ParameterConversion.Unpack(args, typeof (TArg), index);
    }

    public static void Validate(object[] args, Type[] expected)
    {
      if (args.Length > expected.Length)
        throw new ArgumentException(string.Format("Too many parameters have been supplied. Expecting {0} but got {1}.", (object) expected.Length, (object) args.Length));
      for (int index = 0; index < expected.Length; ++index)
        ParameterConversion.Unpack(args, expected[index], index);
    }
  }
}
