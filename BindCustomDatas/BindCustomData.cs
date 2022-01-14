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
        private static Dictionary<Type, Type> dict = new Dictionary<Type, Type>()
        {
            { typeof(GTextField), typeof(TextFieldBindCustomData) },
            { typeof(GButton), typeof(ButtonBindCustomData)}
        };

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

        public abstract IEnumerable<(string key, Action<object> handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view);
    }
}
