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

        private List<BindCustomData> bindDatas;

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
            bindDatas = new List<BindCustomData>();

            gComponent.EnumerateLeafObj((leaf) =>
            {
                var customData = BindCustomData.Build(leaf, view);
                if (customData == null)
                {
                    return;
                }

                bindDatas.Add(customData);
            });

            this.view = view;
            this.gComponent = gComponent;

            view.PropertyChanged += OnViewPropetyUpdate;
        }

        public void Dispose()
        {
            Debug.Log($"[JLOG] Dispose({this.GetHashCode()}");

            view.PropertyChanged -= OnViewPropetyUpdate;
            
            foreach (var bindData in bindDatas)
            {
                bindData.Dispose();
            }
        }

        private void OnViewPropetyUpdate(object sender, PropertyChangedEventArgs e)
        {
            foreach (var bindData in bindDatas)
            {
                bindData.OnViewPropetyUpdate(sender, e);
            }
        }
    }
}
