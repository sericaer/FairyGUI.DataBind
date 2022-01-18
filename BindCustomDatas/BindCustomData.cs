using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace FairyGUI.DataBind.BindCustomDatas
{
    abstract class BindCustomData
    {
        public readonly static Dictionary<Type, Type> dict = new Dictionary<Type, Type>()
        {
            { typeof(GTextField), typeof(TextFieldBindCustomData) },
            { typeof(GButton), typeof(ButtonBindCustomData)},
            { typeof(GTextInput), typeof(TextInputBindCustomData)},
            { typeof(GList), typeof(ListBindCustomData)}
        };

        public class BindTemplate
        {
            public string enable;
        }

        public abstract IEnumerable<(string key, BindHandler handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view);

        public static BindCustomData Build(Type uiType, string customStr)
        {
            try
            {
                Type BindCustomDataType;
                if (!dict.TryGetValue(uiType, out BindCustomDataType))
                {
                    throw new NotImplementedException();
                }

                return JsonUtility.FromJson(customStr, BindCustomDataType) as BindCustomData;
            }
            catch(Exception e)
            {
                Debug.LogWarning($"messgae:{e.Message} type:{uiType}, custom data:{customStr}");
                return null;
            }
        }

        protected void BindEnable(string enableKey, INotifyPropertyChanged view, GObject button, List<(string key, BindHandler handler)> rslt)
        {
            if (enableKey == null)
            {
                return;
            }

            var property = view.GetType().GetProperty(enableKey);
            if (property == null)
            {
                return;
            }

            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    button.enabled = (bool)property.GetValue(view);
                },
                OnViewUpdate = (view) =>
                {
                    button.enabled = (bool)property.GetValue(view);
                }
            };

            rslt.Add((enableKey, handler));
        }
    }
}
