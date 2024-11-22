namespace ShareYourText.Templates
{
    public abstract class LinkDbForm
    {
        public DateTime ExpirationDate { get; private set; }

        public LinkDbForm()
        {
            ExpirationDate = DateTime.UtcNow.AddDays(7);
        }
    }
}

