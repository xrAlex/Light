using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Light.Services
{
    public interface IDialog
    {
        object DataContext { get; set; }
        bool? DialogResult { get; set; }
        Window Owner { get; set; }
        void Close();
        bool? ShowDialog();
    }

    public interface IDialogService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose where TView : IDialog;
        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;
        Window GetOwner();

    }

    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }

    public class DialogCloseRequestedEventArgs : EventArgs
    {
        public DialogCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }
        public bool? DialogResult { get; }
    }

    public class DialogService : IDialogService
    {
        private readonly Window owner;
        private readonly List<DialogForm> dialogs;

        public DialogService(Window owner)
        {
            this.owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }

        public IDictionary<Type, Type> Mappings { get; set; }

        public void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
                                                  where TView : IDialog
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
            }
            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];

            IDialog dialog = (IDialog)Activator.CreateInstance(viewType);

            void handler(object sender, DialogCloseRequestedEventArgs e)
            {
                viewModel.CloseRequested -= handler;

                if (e.DialogResult.HasValue)
                {
                    dialog.DialogResult = e.DialogResult;
                }
                else
                {
                    dialog.Close();
                }
            }

            viewModel.CloseRequested += handler;
            dialog.DataContext = viewModel;
            dialog.Owner = owner;

            return dialog.ShowDialog();
        }

        public Window GetOwner()
        {
            return dialogs[0].DialogM.Owner;
        }
    }

    public class DialogForm
    {
        public string DialogName { get; set; }
        public IDialog DialogM { get; set; }
    }
}
