using FairyGUI;
using System;
using System.ComponentModel;

namespace FairyGUI.DataBind
{
    public partial class BindContext : IDisposable
    {
        private GObject gObject;
        private INotifyPropertyChanged view;

        EventHandlerManager handerManager;

        public BindContext(GObject gObject, INotifyPropertyChanged view)
        {
            handerManager = new EventHandlerManager();
            gObject.Bind(view, this);

            view.PropertyChanged += OnViewPropetyUpdate;

            handerManager.RefreshInitValue(view);
        }

        private void OnViewPropetyUpdate(object sender, PropertyChangedEventArgs e)
        {
            var hander = handerManager.GetHandler(e.PropertyName);
            hander.Invoke(sender);
        }

        public void Dispose()
        {
            handerManager.Clean();
        }

        internal void Add(GTextField textField, string customData, Action<object> hander)
        {
            handerManager.Add(textField, customData, hander);
        }
    }
}
