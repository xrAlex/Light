namespace Light.Services.Interfaces
{
    internal interface ITrayNotifierService
    {
        void Dispose();
        void ShowTip(string tip);
    }
}