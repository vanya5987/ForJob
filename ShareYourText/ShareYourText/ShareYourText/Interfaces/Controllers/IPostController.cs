namespace ShareYourText.Interfaces.Controllers
{
    public interface IPostController
    {
        Task LikePostAsync();
        Task DislikePostAsync();
    }
}

