using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FairyGUI.DataBind
{
    public partial class BindContext
    {
        class EventHandlerManager
        {

            private Dictionary<string, Action<object>> dict;

            public EventHandlerManager()
            {
                dict = new Dictionary<string, Action<object>>();
            }

            internal void Add(GTextField textField, string customData, Action<object> hander)
            {
                dict.Add(customData, hander);
            }

            internal void Clean()
            {
                dict.Clear();
            }

            internal Action<object> GetHandler(string propertyName)
            {
                return dict[propertyName];
            }

            internal void RefreshInitValue(INotifyPropertyChanged view)
            {
                foreach (var handler in dict.Values)
                {
                    handler.Invoke(view);
                }
            }
        }
    }
}
