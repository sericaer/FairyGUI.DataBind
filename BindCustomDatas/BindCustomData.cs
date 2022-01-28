using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace FairyGUI.DataBind.BindCustomDatas
{
    public abstract class BindCustomData : IDisposable
    {
        public readonly static Dictionary<Type, Type> dict = new Dictionary<Type, Type>()
        {
            { typeof(GTextField), typeof(TextFieldBindCustomData) },
            { typeof(GRichTextField), typeof(RichTextFieldBindCustomData) },
            { typeof(GButton), typeof(ButtonBindCustomData)},
            { typeof(GTextInput), typeof(TextInputBindCustomData)},
            { typeof(GList), typeof(ListBindCustomData)},
            { typeof(GSlider), typeof(SliderBindCustomData)}
        };

        public class BindTemplate
        {
            public string enable;
            public string tips;
        }

        internal EventHandlerManager handerManager = new EventHandlerManager();


        public static BindCustomData Build(GObject gObject, INotifyPropertyChanged view)
        {
            try
            {
                var customStr = gObject.data as string;
                if (customStr == null)
                {
                    return null;
                }

                Type BindCustomDataType;
                if (!dict.TryGetValue(gObject.GetType(), out BindCustomDataType))
                {
                    throw new NotImplementedException();
                }

                var bindCustomData = JsonUtility.FromJson(customStr, BindCustomDataType) as BindCustomData;
                bindCustomData.Init(gObject, view);

                bindCustomData.handerManager.Initialize(view);

                return bindCustomData;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"messgae:{e.Message} type:{gObject.GetType()}, custom data:{gObject.data}");
                return null;
            }
        }

        internal abstract void Init(GObject leaf, INotifyPropertyChanged view);

        public void Dispose()
        {
            handerManager.Dispose();
        }

        internal void OnViewPropetyUpdate(object sender, PropertyChangedEventArgs e)
        {
            var handers = handerManager.GetHandlers(e.PropertyName);
            if (handers == null)
            {
                return;
            }

            foreach (var hander in handers)
            {
                hander.OnViewUpdate?.Invoke(sender);
            }
        }

        protected void BindEnable(string enableKey, GObject leaf, INotifyPropertyChanged view)
        {
            if(enableKey == null)
            {
                return;
            }

            var property = view.GetType().GetProperty(enableKey);

            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    leaf.enabled = (bool)property.GetValue(view);
                },
                OnViewUpdate = (view) =>
                {
                    leaf.enabled = (bool)property.GetValue(view);
                }
            };

            handerManager.Add(enableKey, handler);
        }

        protected void BindTips(string tipsKey, GObject gObject, INotifyPropertyChanged view)
        {
            if (tipsKey == null)
            {
                return;
            }

            var property = view.GetType().GetProperty(tipsKey);
            if (property == null)
            {
                return;
            }

            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    gObject.tooltips = (string)property.GetValue(view);
                },
                OnViewUpdate = (view) =>
                {
                    gObject.tooltips = (string)property.GetValue(view);
                }
            };

            handerManager.Add(tipsKey, handler);
        }
    }
}
