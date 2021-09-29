using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Light.Templates.Entities
{
    public class DialogStorage
    {
        public Window Instance { get; set; }
        public Window Owner { get; }
        public Type Type { get; }

        public void CloseInstance()
        {
            Instance?.Close();
            Instance = null;
        }

        public void ShowInstance() => Instance?.Show();
        public void HideInstance() => Instance?.Hide();

        public DialogStorage(Window instance = null, Window owner = null, Type type = null)
        {
            Instance = instance;
            Owner = owner;
            Type = type;
        }
    }

}
