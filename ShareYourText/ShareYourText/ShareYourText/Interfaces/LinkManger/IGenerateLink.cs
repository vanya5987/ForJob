namespace ShareYourText.Interfaces.LinkManger
{
    public interface IGenerateLink
    {
        string GenerateSHA256Hash();
        string GenerateBaseLink();
    }
}

