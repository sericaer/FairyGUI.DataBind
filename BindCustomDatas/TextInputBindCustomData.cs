using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FairyGUI.DataBind.BindCustomDatas
{
    [System.Serializable]
    class TextInputBindCustomData : BindCustomData
    {
        [System.Serializable]
        public new class BindTemplate : BindCustomData.BindTemplate
        {
            public string text;
        }

        public BindTemplate bind;

        internal override void Init(GObject gObject, INotifyPropertyChanged view)
        {
            var textInput = gObject.asTextInput;

            BindEnable(bind.enable, textInput, view);

            BindText(textInput, view);
        }

        private void BindText(GTextInput textInput, INotifyPropertyChanged view)
        {
            if (bind.text == null)
            {
                return;
            }

            var bindkey = bind.text;
            var property = view.GetType().GetProperty(bindkey);

            Action<object> viewUpdate = (view) =>
            {
                textInput.text = property.GetValue(view).ToString();
            };

            EventCallback1 uiUpdate = (context) =>
            {
                property.SetValue(view, textInput.text);
            };

            var handler = new BindHandler()
            {
                Init = (view) =>
                {
                    viewUpdate(view);
                    textInput.onChanged.Add(uiUpdate);

                },

                Exit = () =>
                {
                    textInput.onChanged.Remove(uiUpdate);
                },

                OnViewUpdate = viewUpdate
            };

            handerManager.Add(bindkey, handler);
        }
    }
}