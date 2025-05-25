
using System.ComponentModel.DataAnnotations;

namespace Devella.DataAccessLayer.Enums;

public enum CompetenceArea
{
    [Display(Name = "Software Development")]
    SoftwareDevelopment,
    Qualityassurance,
    Design,
    AI,
    [Display(Name = "Cloud Infrastructure")]
    CloudInfrastructure,
    [Display(Name = "Data engeneering")]
    DataEngeneering,
    [Display(Name = "Data Science")]
    DataScience,
    [Display(Name = "Mobile Development")]
    MobileDevelopment,
    [Display(Name = "Back-end")]
    BackEnd,
    [Display(Name = "Front-end")]
    FrontEnd
}
