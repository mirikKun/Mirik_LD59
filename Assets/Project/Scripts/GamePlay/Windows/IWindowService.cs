using System;

namespace Project.Scripts.GamePlay.Windows
{
    public interface IWindowService
    {
        event Action<BaseWindow> WindowOpened;
        event Action<BaseWindow> WindowHidden;

        void Open(WindowId windowId);
        void Close(WindowId windowId);
        T Open<T>(WindowId windowId) where T : BaseWindow;
        void CloseAll();
        void Hide(WindowId windowId);

    }
}