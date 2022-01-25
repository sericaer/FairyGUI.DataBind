using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyGUI.DataBind.BindCustomDatas
{
    [System.Serializable]
    class SliderBindCustomData : BindCustomData
    {
        [System.Serializable]
        public new class BindTemplate : BindCustomData.BindTemplate
        {
            public string value;
        }

        public BindTemplate bind;

        public override IEnumerable<(string key, BindHandler handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view)
        {
            var rslt = new List<(string key, BindHandler handler)>();
            if (bind == null)
            {
                return rslt;
            }

            BindValue(gObject, view, rslt);

            return rslt;
        }

        private void BindValue(GObject gObject, INotifyPropertyChanged view, List<(string key, BindHandler handler)> rslt)
        {
            var property = view.GetType().GetProperty(bind.value);
            if (property == null)
            {
                return;
            }

            var slider = gObject as GSlider;

            EventCallback1 uiUpdate = (context) =>
            {
                property.SetValue(view, (int)slider.value);
            };

            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    slider.value = (int)property.GetValue(view);
                    slider.onChanged.Add(uiUpdate);
                },
                Exit = () =>
                {
                    slider.onChanged.Remove(uiUpdate);
                },
                OnViewUpdate = (view) =>
                {
                    slider.value = (int)property.GetValue(view);
                }
            };

            rslt.Add((bind.value, handler));
        }
    }
}
