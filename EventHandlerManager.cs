using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FairyGUI.DataBind
{

    class EventHandlerManager
    {

        private Dictionary<string, List<Action<object>>> dict;

        public EventHandlerManager()
        {
            dict = new Dictionary<string, List<Action<object>>>();
        }

        internal void Add(string key, Action<object> hander)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, new List<Action<object>>());
            }

            dict[key].Add(hander);
        }

        internal IEnumerable<Action<object>> GetHandlers(string propertyName)
        {
            List<Action<object>> rslt;
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
                handler.Invoke(view);
            }
        }

        internal void Exit()
        {
            dict.Clear();
        }
    }

}
