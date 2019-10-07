namespace PollIt.Models
{
    /// <summary>
    ///     Enum representing themes
    /// </summary>
    public enum Theme
    {
        Light,
        Dark
    }

    /// <summary>
    /// Class to represent a theme selection
    /// </summary>
    class AppTheme
    {
        public Theme ThemeId { get; set; }
        public string Title { get; set; }
    }
}
