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
        }

        public BindTemplate bind;

        public override IEnumerable<(string key, Action<object> handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view)
        {
            var rslt = new List<(string key, Action<object> handler)>();

            if (bind != null)
            {
                var button = gObject as GButton;

                if (bind.onClick != null)
                {
                    var bindkey = bind.onClick;
                    var method = view.GetType().GetMethod(bindkey);

                    Action<object> handler = (sender) =>
                    {
                        button.onClick.Add((context) => { method.Invoke(sender, null); });
                    };

                    rslt.Add((bindkey, handler));
                }
            }

            return rslt;
        }
    }
}
