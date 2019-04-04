using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class InteractionMediator : IInteractionMediator
    {
        public event EventHandler<ObjectEventArgs> OnRequestFileWindow;
        public void RequestFileWindow(object sender, object parameter)
        {
            OnRequestFileWindow?.Invoke(sender, new ObjectEventArgs(parameter));
        }
    }

    public interface IInteractionMediator
    {
         event EventHandler<ObjectEventArgs> OnRequestFileWindow;
         void RequestFileWindow(object sender, object parameter);
    }

    public class ObjectEventArgs : EventArgs
    {
        public ObjectEventArgs(object value)
        {
            Value = value;
        }

        public Object Value { get; }
    }

}
