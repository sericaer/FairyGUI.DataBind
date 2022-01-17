using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FairyGUI.DataBind.BindCustomDatas
{
    [System.Serializable]
    class TextInputBindCustomData : BindCustomData
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
                var textInput = gObject as GTextInput;

                if (bind.text != null)
                {
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

                        Exit = ()=>
                        {
                            textInput.onChanged.Remove(uiUpdate);
                        },

                        OnViewUpdate = viewUpdate
                    };

                    rslt.Add((bindkey, handler));
                }
            }

            return rslt;
        }
    }
}