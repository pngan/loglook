using System;

namespace Model
{
    public class MainModel : IMainModel
    {
        public string LogContent { get; private set; }

        public void Start()
        {
            
        }



        public void SetName(string name)
        {
            OnGreetingChanged?.Invoke(this, new GreetingArgs(name));
        }

        private void SetLogContent(string logContent)
        {
            LogContent = logContent;
        }

        public event EventHandler<GreetingArgs> OnGreetingChanged;
    }
}