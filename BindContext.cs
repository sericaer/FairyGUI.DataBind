using FairyGUI;
using FairyGUI.DataBind.BindCustomDatas;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FairyGUI.DataBind
{
    public class BindContext : IDisposable
    {
        public static Action<GObject, object> DoCmd;

        internal static List<BindContext> all = new List<BindContext>();

        public GObject gComponent { get; private set; }
        public INotifyPropertyChanged view { get; private set; }

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
            this.gComponent = gComponent;

            view.PropertyChanged += OnViewPropetyUpdate;

            handerManager.Initialize(view);

            all.Add(this);
        }

        public void Dispose()
        {
            view.PropertyChanged -= OnViewPropetyUpdate;
            handerManager.Exit();
            all.Remove(this);

            gComponent.Dispose();
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
