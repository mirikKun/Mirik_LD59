using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Project.Scripts.GamePlay.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IWindowFactory _windowFactory;

        private readonly Dictionary<WindowId, BaseWindow> _openedWindows = new();
        public event Action<BaseWindow> WindowOpened;
        public event Action<BaseWindow> WindowHidden;


        public WindowService(IWindowFactory windowFactory) =>
            _windowFactory = windowFactory;

        public void Open(WindowId windowId)
        {
            if (_openedWindows.ContainsKey(windowId))
            {
                var existing = _openedWindows[windowId];
                existing.gameObject.SetActive(true);
                WindowOpened?.Invoke(existing);
                return;
            }

            var baseWindow = _windowFactory.CreateWindow(windowId);
            _openedWindows.Add(windowId, baseWindow);
            WindowOpened?.Invoke(baseWindow);
        }

        public T Open<T>(WindowId windowId) where T : BaseWindow
        {
            if (_openedWindows.ContainsKey(windowId))
            {
                var existing = _openedWindows[windowId];
                existing.gameObject.SetActive(true);
                WindowOpened?.Invoke(existing);
                return existing as T;
            }

            var window = _windowFactory.CreateWindow<T>(windowId);
            _openedWindows.Add(windowId, window);
            WindowOpened?.Invoke(window);
            return window;
        }

        public void Hide(WindowId windowId)
        {
            if (!_openedWindows.TryGetValue(windowId, out var window))
                return;

            window.gameObject.SetActive(false);
            WindowHidden?.Invoke(window);
        }

        public void Close(WindowId windowId)
        {
            if (!_openedWindows.TryGetValue(windowId, out var window))
                return;

            _openedWindows.Remove(windowId);

            WindowHidden?.Invoke(window);
            Object.Destroy(window.gameObject);
        }

        public void CloseAll()
        {
            foreach (var window in _openedWindows.Values)
            {
                if (window)
                {
                    WindowHidden?.Invoke(window);
                    Object.Destroy(window.gameObject);
                }
            }

            _openedWindows.Clear();
        }
    }
}