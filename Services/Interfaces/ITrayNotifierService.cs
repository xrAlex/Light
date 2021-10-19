namespace Sparky.Services.Interfaces
{
    interface ITrayNotifierService
    {
        void Dispose();
        void ShowTip(string localizationKey);
    }
}