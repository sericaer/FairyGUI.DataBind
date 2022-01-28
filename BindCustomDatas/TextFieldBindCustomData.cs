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

        internal override void Init(GObject gObject, INotifyPropertyChanged view)
        {
            //BindEnable(bind.enable, gObject, view);
            BindText(gObject.asTextField, view);
        }

        private void BindText(GTextField textField, INotifyPropertyChanged view)
        {
            var property = view.GetType().GetProperty(bind.text);
            if (property == null)
            {
                return;
            }

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

            handerManager.Add(bind.text, handler);
        }
    }
}
