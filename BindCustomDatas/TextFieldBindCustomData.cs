using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyGUI.DataBind.BindCustomDatas
{
    [System.Serializable]
    class TextFieldBindCustomData : BindCustomData
    {
        [System.Serializable]
        public new class BindTemplate : BindCustomData.BindTemplate
        {
            public string text;
        }

        public BindTemplate bind;

        public override IEnumerable<(string key, BindHandler handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view)
        {
            var rslt = new List<(string key, BindHandler handler)>();
            if (bind == null)
            {
                return rslt;
            }

            BindEnable(bind.enable, view, gObject, rslt);

            BindText(gObject, view, rslt);

            return rslt;
        }

        private void BindText(GObject gObject, INotifyPropertyChanged view, List<(string key, BindHandler handler)> rslt)
        {
            var property = view.GetType().GetProperty(bind.text);
            if (property == null)
            {
                return;
            }

            var textField = gObject as GTextField;
            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    textField.text = property.GetValue(view).ToString();
                },
                OnViewUpdate = (view) =>
                {
                    textField.text = property.GetValue(view).ToString();
                }
            };

            rslt.Add((bind.text, handler));
        }
    }
}
