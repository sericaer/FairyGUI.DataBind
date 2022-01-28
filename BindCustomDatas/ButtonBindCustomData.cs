using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyGUI.DataBind.BindCustomDatas
{
    [System.Serializable]
    class ButtonBindCustomData : BindCustomData
    {
        [System.Serializable]
        public new  class BindTemplate : BindCustomData.BindTemplate
        {
            public string onClick;
            public string selected;
        }

        public BindTemplate bind;

        internal override void Init(GObject gObject, INotifyPropertyChanged view)
        {
            var button = gObject.asButton;

            BindEnable(bind.enable, button, view);

            BindOnClick(button, view);
            BindOnSelected(button, view);
        }

        private void BindOnSelected(GButton button, INotifyPropertyChanged view)
        {
            if(bind.selected == null)
            {
                return;
            }

            var property = view.GetType().GetProperty(bind.selected);

            EventCallback1 onSelected = (context) =>
            {
                property.SetValue(view, button.selected);
            };

            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    button.selected = (bool)property.GetValue(view);

                    button.onChanged.Add(onSelected);
                },

                Exit = () =>
                {
                    button.onChanged.Remove(onSelected);
                },

                OnViewUpdate = (view) =>
                {
                    button.selected = (bool)property.GetValue(view);
                }
            };

            handerManager.Add(bind.selected, handler);
        }

        private void BindOnClick(GButton button, INotifyPropertyChanged view)
        {
            if (bind.onClick == null)
            {
                return;
            }

            var method = view.GetType().GetMethod(bind.onClick);

            EventCallback1 onClick = (context) =>
            {
                var rslt = method.Invoke(view, null);

                BindContext.DoCmd?.Invoke(button, rslt);
            };

            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    button.onClick.Add(onClick);
                },
                Exit = () =>
                {
                    button.onClick.Remove(onClick);
                }
            };

            handerManager.Add(bind.onClick, handler);
        }
    }
}
