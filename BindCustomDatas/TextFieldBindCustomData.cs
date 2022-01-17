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
        public class BindTemplate
        {
            public string text;
        }

        public BindTemplate bind;

        public override IEnumerable<(string key, BindHandler handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view)
        {
            var rslt = new List<(string key, BindHandler handler)>();
            if (bind == null || bind.text == null)
            {
                return rslt;
            }

            var property = view.GetType().GetProperty(bind.text);
            if (property == null)
            {
                return rslt;
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

            return rslt;
        }
    }
}
