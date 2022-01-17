﻿using System;
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

        public override IEnumerable<(string key, BindHandler handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view)
        {
            var rslt = new List<(string key, BindHandler handler)>();

            if (bind != null)
            {
                var button = gObject as GButton;

                if (bind.onClick != null)
                {
                    var bindkey = bind.onClick;
                    var method = view.GetType().GetMethod(bindkey);

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
                        Exit = ()=>
                        {
                            button.onClick.Remove(onClick);
                        }
                    };

                    rslt.Add((bindkey, handler));
                }
            }

            return rslt;
        }
    }
}
