using System;
using UnityToolkit;

namespace Game
{
    public abstract class ObservationObject<T> where T : ObservationObject<T>
    {
        private event Action<T> OnEvent;

        public ICommand Listen(Action<T> onEvent)
        {
            this.OnEvent += onEvent;
            return new CommonCommand(() => { UnListen(onEvent); });
        }

        public void UnListen(Action<T> onEvent)
        {
            OnEvent -= onEvent;
        }

        public void SetDirty()
        {
            OnEvent?.Invoke(this as T);
        }
    }
}