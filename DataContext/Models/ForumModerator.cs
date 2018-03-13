namespace DataContext.Models
{
    public enum ModeratorLevel
    {
        Comoderator,    // Forum
        Moderator,      // Forum + subforums
        SuperModerator, // Forum + subforums (control over moderators)
        Admin           // Forum + subforums (admin level control)
    }
    
    public class ForumModerator : BaseEntity
    {
        public virtual Forum Forum { get; set; }
        
        public virtual ApplicationUser User { get; set; }

        public ModeratorLevel Level { get; set; } = ModeratorLevel.Moderator;
    }
}