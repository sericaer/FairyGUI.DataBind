using FairyGUI.DataBind.BindCustomDatas;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FairyGUI.DataBind
{
    public static class BindExtention
    {
        public static void BindDataSource(this GComponent component, INotifyPropertyChanged view)
        {
            BindContext.Bind(component, view);
        }


        public static void EnumerateLeafObj(this GComponent component, Action<GObject> action)
        {
            foreach (var child in component.GetChildren())
            {
                if (BindCustomData.dict.ContainsKey(child.GetType()))
                {
                    action?.Invoke(child);
                }
                else if (child is GComponent subCom)
                {
                    subCom.EnumerateLeafObj(action);
                }
                //else
                //{
                //    throw new NotImplementedException();
                //}
                
            }
        }

        public static GObject Clone(this GObject gObject)
        {
            return gObject.packageItem.owner.CreateObject(gObject.packageItem.name);
        }
    }
}
