namespace ShareYourText.Singleton
{
    public sealed class FormCreater
    {
        private static ShareText _form;

        private FormCreater() { }

        public static ShareText GetForm()
        {
            if (_form == null)
                _form = new ShareText();

            return _form;
        }
    }
}

