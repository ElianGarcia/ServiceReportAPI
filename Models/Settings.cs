namespace ServiceReportAPI.Models
{
    public class Settings
    {
        public int SettingId { get; set; }
        public string Language { get; set; }
        public decimal IncrementValue { get; set; }
        public int UserId { get; set; }

        public Settings() { 
        
        }

        public Settings(int settingId, string language, decimal incrementValue, int userId)
        {
            SettingId = settingId;
            Language = language ?? throw new ArgumentNullException(nameof(language));
            IncrementValue = incrementValue;
            UserId = userId;
        }
    }
}
