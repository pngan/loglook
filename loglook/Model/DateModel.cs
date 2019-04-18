using System;

namespace Model
{
    public class DateModel
    {
        public DateModel(DateTime dateTime, int value = 0)
        {
            DateTime = dateTime;
            Value = value;
        }

        public void IncrementCount() => Value++;
        public DateTime DateTime { get; }
        public int Value { get; private set; }
    }
}