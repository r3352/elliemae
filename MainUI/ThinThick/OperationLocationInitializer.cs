// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ThinThick.OperationLocationInitializer
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.Common.ThinThick.Operation.Location;
using System;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ThinThick
{
  public class OperationLocationInitializer
  {
    public static void RegisterAll()
    {
      foreach (Type operationType in Enumerable.Cast<Type>(typeof (OperationLocationInitializer).Assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => !type.IsAbstract && type.GetInterface("IOperation") != (Type) null)).Select<Type, Type>((Func<Type, Type>) (type => type)))
      {
        foreach (Type interfaceType in Enumerable.Cast<Type>(operationType.GetInterfaces()).Where<Type>((Func<Type, bool>) (typeInterface => typeInterface.GetInterface("IOperation") != (Type) null)).Select<Type, Type>((Func<Type, Type>) (typeInterface => typeInterface)))
          OperationLocationInitializer.Register(interfaceType, operationType);
      }
    }

    private static void Register(Type interfaceType, Type operationType)
    {
      OperationLocator.GetInstance().Register(interfaceType, operationType);
    }
  }
}
