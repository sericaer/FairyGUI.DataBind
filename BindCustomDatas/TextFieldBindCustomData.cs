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

            if (bind != null)
            {
                var textField = gObject as GTextField;

                if (bind.text != null)
                {
                    var bindkey = bind.text;
                    var property = view.GetType().GetProperty(bindkey);

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

                    rslt.Add((bindkey, handler));
                }
            }

            return rslt;
        }
    }
}
