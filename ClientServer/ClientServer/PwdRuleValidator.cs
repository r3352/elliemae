// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PwdRuleValidator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PwdRuleValidator
  {
    private int minLength;
    private int minLower;
    private int minUpper;
    private int minDigits;
    private int minSpecial;

    public PwdRuleValidator(
      int minLength,
      int minLower,
      int minUpper,
      int minDigits,
      int minSpecial)
    {
      this.minLength = minLength;
      this.minLower = minLower;
      this.minUpper = minUpper;
      this.minDigits = minDigits;
      this.minSpecial = minSpecial;
    }

    public int MinimumLength => this.minLength;

    public int MinimumNumOfLowerCaseChars => this.minLower;

    public int MinimumNumOfUpperCaseChars => this.minUpper;

    public int MinimumNumOfDigits => this.minDigits;

    public int MinimumNumOfSpecialChars => this.minSpecial;

    public bool CheckMinLength(string password)
    {
      return password != null && password.Length >= this.minLength;
    }

    public bool CheckCompositionRule(string password)
    {
      if (password == null)
        return false;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      for (int index = 0; index < password.Length; ++index)
      {
        num1 += char.IsLower(password[index]) ? 1 : 0;
        num2 += char.IsUpper(password[index]) ? 1 : 0;
        num3 += char.IsDigit(password[index]) ? 1 : 0;
        num4 += char.IsPunctuation(password[index]) || char.IsSymbol(password[index]) ? 1 : 0;
      }
      return num1 >= this.minLower && num2 >= this.minUpper && num3 >= this.minDigits && num4 >= this.minSpecial;
    }

    public PwdRuleValidator.ViolationCode Check(string password)
    {
      if (!this.CheckMinLength(password))
        return PwdRuleValidator.ViolationCode.MinLengthRuleViolated;
      return !this.CheckCompositionRule(password) ? PwdRuleValidator.ViolationCode.CompositionRuleViolated : PwdRuleValidator.ViolationCode.NoViolation;
    }

    public string GetCompositionRuleDescription()
    {
      ArrayList arrayList = new ArrayList();
      if (this.MinimumNumOfUpperCaseChars > 0)
        arrayList.Add((object) ("* At least " + (object) this.MinimumNumOfUpperCaseChars + " upper-case letter(s)"));
      if (this.MinimumNumOfLowerCaseChars > 0)
        arrayList.Add((object) ("* At least " + (object) this.MinimumNumOfLowerCaseChars + " lower-case letter(s)"));
      if (this.MinimumNumOfDigits > 0)
        arrayList.Add((object) ("* At least " + (object) this.MinimumNumOfDigits + " digit(s)"));
      if (this.MinimumNumOfSpecialChars > 0)
        arrayList.Add((object) ("* At least " + (object) this.MinimumNumOfSpecialChars + " special character(s) (punctuation, symbols, etc.)"));
      return string.Join(Environment.NewLine, (string[]) arrayList.ToArray(typeof (string)));
    }

    public enum ViolationCode
    {
      NoViolation,
      MinLengthRuleViolated,
      CompositionRuleViolated,
    }
  }
}
