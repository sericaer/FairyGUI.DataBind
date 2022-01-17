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
        public class BindTemplate
        {
            public string onClick;
            public string enable;
        }

        public BindTemplate bind;

        public override IEnumerable<(string key, BindHandler handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view)
        {
            var rslt = new List<(string key, BindHandler handler)>();

            if (bind == null)
            {
                return rslt;
            }

            var button = gObject as GButton;

            BindOnClick(view, button, rslt);
            BindEnable(view, button, rslt);


            return rslt;
        }

        private void BindEnable(INotifyPropertyChanged view, GButton button, List<(string key, BindHandler handler)> rslt)
        {
            if (bind.enable == null)
            {
                return;
            }

            var property = view.GetType().GetProperty(bind.enable);
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

            rslt.Add((bind.enable, handler));
        }

        private void BindOnClick(INotifyPropertyChanged view, GButton button, List<(string key, BindHandler handler)> list)
        {
            if (bind.onClick == null)
            {
                return;
            }

            var method = view.GetType().GetMethod(bind.onClick);

            EventCallback1 onClick = (context) =>
            {
                method.Invoke(view, null);
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

            list.Add((bind.onClick, handler));
        }
    }
}
