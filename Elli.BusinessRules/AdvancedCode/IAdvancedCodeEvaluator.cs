// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.AdvancedCode.IAdvancedCodeEvaluator
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.AdvCode.Runtime;
using Elli.AdvCode.Runtime.Validation;

#nullable disable
namespace Elli.BusinessRules.AdvancedCode
{
  public interface IAdvancedCodeEvaluator
  {
    RuntimeContext GetAdvCodeContext();

    bool Evaluate(string code, bool throwUnknownException = false);

    bool Compile(string code);

    ValidationResult ValidateSyntax(string code);

    bool EvaluateAdvancedCode(string code, bool throwUnknownException = false);
  }
}
