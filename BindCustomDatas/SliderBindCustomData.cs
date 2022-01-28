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

        internal override void Init(GObject gObject, INotifyPropertyChanged view)
        {
            var slider = gObject.asSlider;

            BindValue(slider, view);
        }

        private void BindValue(GSlider slider, INotifyPropertyChanged view)
        {
            if(bind.value == null)
            {
                return;
            }

            var property = view.GetType().GetProperty(bind.value);

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

            handerManager.Add(bind.value, handler);
        }
    }
}
