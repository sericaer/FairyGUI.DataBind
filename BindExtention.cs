using FairyGUI.DataBind.BindCustomDatas;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FairyGUI.DataBind
{
    public static class BindExtention
    {
        public static BindContext BindDataSource(this GComponent component, INotifyPropertyChanged view)
        {
            return new BindContext(component, view);
        }


        public static void EnumerateLeafObj(this GComponent component, Action<GObject> action)
        {
            foreach (var child in component.GetChildren())
            {
                if (child is GButton || child is GTextField)
                {
                    action?.Invoke(child);
                }
                else if (child is GComponent subCom)
                {
                    component.EnumerateLeafObj(action);
                }
                else
                {
                    throw new NotImplementedException();
                }
                
            }
        }
    }
}
