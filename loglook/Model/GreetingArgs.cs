using System;

namespace Model
{
    public class GreetingArgs : EventArgs
    {
        private readonly string m_name;

        public GreetingArgs(string name)
        {
            m_name = name;
        }

        public string Greeting => $"Hello, {m_name}!";
    }
}