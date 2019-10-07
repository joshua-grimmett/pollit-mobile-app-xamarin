namespace PollIt.Models
{
    /// <summary>
    ///     Enum representing Menu Items
    /// </summary>
    public enum MenuItemType
    {
        Polls,
        CreatePoll,
        JoinPoll,
        Settings
    }

    /// <summary>
    ///     Class to represent a Menu Item
    /// </summary>
    class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}