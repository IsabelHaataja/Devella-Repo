
namespace DevellaLib.Helpers;

public static class InitialsHelper
{
    public static string GetInitials(string firstName, string surname)
    {
        var fullName = $"{firstName} {surname}".Trim();
        if (string.IsNullOrWhiteSpace(fullName))
            return string.Empty;

        var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return string.Concat(nameParts.Select(n => char.ToUpper(n[0])));
    }
}
