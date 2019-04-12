using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class InteractionMediator : IInteractionMediator
    {
        public event EventHandler<RequestFileWindowArgs> OnRequestFileWindow;
        public void RequestFileWindow(object sender, RequestFileWindowArgs requestFileWindowArgs)
        {
            OnRequestFileWindow?.Invoke(sender, requestFileWindowArgs);
        }
    }

    public interface IInteractionMediator
    {
         event EventHandler<RequestFileWindowArgs> OnRequestFileWindow;
         void RequestFileWindow(object sender, RequestFileWindowArgs requestFileWindowArgs);
    }

    //public class ObjectEventArgs : EventArgs
    //{
    //    public ObjectEventArgs(object value)
    //    {
    //        Value = value;
    //    }

    //    public Object Value { get; }
    //}

}
