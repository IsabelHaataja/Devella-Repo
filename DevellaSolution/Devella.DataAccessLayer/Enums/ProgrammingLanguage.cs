
using System.ComponentModel.DataAnnotations;

namespace Devella.DataAccessLayer.Enums;

public enum ProgrammingLanguage
{
    [Display(Name = "C#")]
    CSharp,
    Java,
    Python,
    JavaScript,
    TypeScript,
    [Display(Name = "C++")]
    CPlusPlus,
    C
}
