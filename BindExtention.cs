using System;
using System.ComponentModel;

namespace FairyGUI.DataBind
{
    public static class BindExtention
    {
        public static BindContext BindDataSource(this GObject gObject, INotifyPropertyChanged view)
        {
            return new BindContext(gObject, view);
        }

        public static void Bind(this GObject gObject, INotifyPropertyChanged view, BindContext bindContext)
        {
            switch (gObject)
            {
                case GComponent component:
                    component.Bind(view, bindContext);
                    break;
                case GTextField textField:
                    textField.Bind(view, bindContext);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void Bind(this GComponent component, INotifyPropertyChanged view, BindContext bindContext)
        {
            foreach (var child in component.GetChildren())
            {
                child.Bind(view, bindContext);
            }
        }

        private static void Bind(this GTextField textField, INotifyPropertyChanged view, BindContext bindContext)
        {
            var customData = textField.data as string;
            var property = view.GetType().GetProperty(customData);

            bindContext.Add(textField, customData, (sender) => textField.text = property.GetValue(sender).ToString());
        }
    }
}
