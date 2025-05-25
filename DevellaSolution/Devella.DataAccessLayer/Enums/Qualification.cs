
using System.ComponentModel.DataAnnotations;

namespace Devella.DataAccessLayer.Enums;

public enum Qualification
{
    [Display(Name = ".NET")]
    DotNet,
    [Display(Name = "React.js")]
    ReactJS,
    [Display(Name = "React Native")]
    ReactNative,
    Angular,
    [Display(Name = "Node.js")]
    NodeJS,
    [Display(Name = "Vue.js")]
    VueJS,
    Flutter,
    Ruby,
    PHP,
    DevOps
}
