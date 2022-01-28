using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairyGUI.DataBind.BindCustomDatas
{
    [System.Serializable]
    class RichTextFieldBindCustomData : BindCustomData
    {
        [System.Serializable]
        public new class BindTemplate : BindCustomData.BindTemplate
        {
            public string text;
        }

        public BindTemplate bind;

        internal override void Init(GObject gObject, INotifyPropertyChanged view)
        {
            var richTextField = gObject.asRichTextField;

            BindEnable(bind.enable, gObject, view);
            BindTips(bind.tips, gObject, view);

            BindText(richTextField, view);
        }

        private void BindText(GRichTextField richTextField, INotifyPropertyChanged view)
        {
            if(bind.text == null)
            {
                return;
            }

            var property = view.GetType().GetProperty(bind.text);
            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    richTextField.text = property.GetValue(view).ToString();
                },
                OnViewUpdate = (view) =>
                {
                    richTextField.text = property.GetValue(view).ToString();
                }
            };

            handerManager.Add(bind.text, handler);
        }
    }
}
