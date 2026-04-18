using System;

namespace Project.Scripts.Infrastructure.Saving
{
    public abstract class BaseSaveModel<T> where T : BaseSaveModel<T>
    {
        public event Action<T> DataChanged;

        protected void OnDataChanged()
        {
            DataChanged?.Invoke((T)this);
        }
    }
}

