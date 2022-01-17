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

            BindEnable(bind.enable, view, button, rslt);
            BindOnClick(view, button, rslt);
            
            return rslt;
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
