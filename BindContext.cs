using FairyGUI;
using FairyGUI.DataBind.BindCustomDatas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace FairyGUI.DataBind
{
    public class BindContext : IDisposable
    {
        public static Action<GObject, object> DoCmd;

        internal static Dictionary<GComponent, BindContext> all = new Dictionary<GComponent, BindContext>();

        public GObject gComponent { get; private set; }
        public INotifyPropertyChanged view { get; private set; }

        private EventHandlerManager handerManager;

        public static void Bind(GComponent gComponent, INotifyPropertyChanged view)
        {
            if(all.ContainsKey(gComponent))
            {
                throw new Exception();
            }

            var context = new BindContext(gComponent, view);
            all.Add(gComponent, context);

            Debug.Log($"[JLOG] Bind({gComponent.GetHashCode()} {view.GetHashCode()}), context({context.GetHashCode()})");

            gComponent.onRemovedFromStage.Add(() =>
            {
                Debug.Log($"[JLOG] onRemovedFromStage({gComponent.GetHashCode()}");

                all[gComponent].Dispose();
                all.Remove(gComponent);
            });
        }

        private BindContext(GComponent gComponent, INotifyPropertyChanged view)
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
        }

        public void Dispose()
        {
            Debug.Log($"[JLOG] Dispose({this.GetHashCode()}");

            view.PropertyChanged -= OnViewPropetyUpdate;
            handerManager.Dispose();
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
