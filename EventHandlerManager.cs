using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FairyGUI.DataBind
{

    class EventHandlerManager : IDisposable
    {
        private Dictionary<string, List<BindHandler>> dict;

        public EventHandlerManager()
        {
            dict = new Dictionary<string, List<BindHandler>>();
        }

        internal void Add(string key, BindHandler hander)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new List<BindHandler>());
            }

            dict[key].Add(hander);
        }

        internal IEnumerable<BindHandler> GetHandlers(string propertyName)
        {
            List<BindHandler> rslt;
            if(dict.TryGetValue(propertyName, out rslt))
            {
                return rslt;
            }

            return null;
        }

        internal void Initialize(INotifyPropertyChanged view)
        {
            foreach (var handler in dict.Values.SelectMany(x => x))
            {
                handler.Init?.Invoke(view);
            }
        }

        public void Dispose()
        {
            foreach (var handler in dict.Values.SelectMany(x => x))
            {
                handler.Dispose();
            }

            dict.Clear();
        }
    }

    public class BindHandler : IDisposable
    {
        public Action<object> Init;
        public Action Exit;

        public Action<object> OnViewUpdate;
        public Action<object> OnUIUpdate;

        public void Dispose()
        {
            Exit?.Invoke() ;
        }
    }

}
