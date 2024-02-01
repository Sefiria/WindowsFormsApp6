using System;
using System.Linq;

namespace WindowsFormsApp24.Events
{
    internal class Command
    {
        internal object[] data;
        internal Action<object[]> Action;
        public Command(Action<object[]> action, params object[] args)
        {
            Action = action;
            data = args.Cast<object>().ToArray();
        }
        public Command(Action action)
        {
            Action = new Action<object[]>(_ => action());
        }
        public void Execute()
        {
            Action?.Invoke(data);
        }
    }
}
