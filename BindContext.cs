using FairyGUI;
using FairyGUI.DataBind.BindCustomDatas;
using System;
using System.ComponentModel;

namespace FairyGUI.DataBind
{
    public interface ICmd
    {
        void Exec();
    }

    public class BindContext : IDisposable
    {
        public static Action<GObject, object> DoCmd;

        private GObject gComponent;
        private INotifyPropertyChanged view;

        EventHandlerManager handerManager;

        public BindContext(GComponent gComponent, INotifyPropertyChanged view)
        {
            handerManager = new EventHandlerManager();

            gComponent.EnumerateLeafObj((leaf) =>
            {
                var customStr = leaf.data as string;
                if (customStr == null)
                {
                    return;
                }

                var customData = BindCustomData.Build(leaf.GetType(), customStr);
                if(customData == null)
                {
                    return;
                }

                var binds = customData.BindUI2View(leaf, view);
                foreach(var bind in binds)
                {
                    handerManager.Add(bind.key, bind.handler);
                } 
            });

            this.view = view;

            view.PropertyChanged += OnViewPropetyUpdate;

            handerManager.Initialize(view);
        }

        public void Dispose()
        {
            view.PropertyChanged -= OnViewPropetyUpdate;
            handerManager.Exit();
        }

        private void OnViewPropetyUpdate(object sender, PropertyChangedEventArgs e)
        {
            var handers = handerManager.GetHandlers(e.PropertyName);
            if(handers == null)
            {
                return;
            }

            foreach(var hander in handers)
            {
                hander.OnViewUpdate?.Invoke(sender);
            }
        }
    }
}
