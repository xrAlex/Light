using System.Windows.Input;
using Sparky.Commands;
using Sparky.Services.Interfaces;
using Sparky.ViewModels.Base;

namespace Sparky.ViewModels
{
    internal sealed class InformationViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;
        private readonly ILinksService _linksService;
        public ICommand CloseInformationWindow { get; }
        public ICommand OpenDonationLink { get; }
        public ICommand OpenGithubLink { get; }
        public ICommand OpenUserGuideLink { get; }


        private void OnCloseInformationWindowCommandExecute()
        {
            _dialogService.CloseDialog<InformationViewModel>();
        }
        private void OnOpenDonationLinkCommandExecute()
        {
            _linksService.OpenLink("https://buymeacoffee.com/xrAlex");
        }
        private void OnOpenGithubLinkCommandExecute()
        {
            _linksService.OpenLink("https://github.com/xrAlex/Light");
        }
        private void OnOpenUserGuideLinkCommandExecute()
        {
            // TODO: Change link
            _linksService.OpenLink("https://github.com/xrAlex/Light");
        }

        public InformationViewModel(IDialogService dialogService, ILinksService linksService)
        {
            CloseInformationWindow = new LambdaCommand(p => OnCloseInformationWindowCommandExecute());
            OpenDonationLink = new LambdaCommand(p => OnOpenDonationLinkCommandExecute());
            OpenGithubLink = new LambdaCommand(p => OnOpenGithubLinkCommandExecute());
            OpenUserGuideLink = new LambdaCommand(p => OnOpenUserGuideLinkCommandExecute());

            _dialogService = dialogService;
            _linksService = linksService;
        }

        public InformationViewModel(){}
    }
}
