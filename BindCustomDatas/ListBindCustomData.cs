using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace FairyGUI.DataBind.BindCustomDatas
{
    [System.Serializable]
    internal class ListBindCustomData : BindCustomData
    {
        [System.Serializable]
        public class ListBindTemplate : BindTemplate
        {
            public string listSource;
        }

        public ListBindTemplate bind;

        public override IEnumerable<(string key, BindHandler handler)> BindUI2View(GObject gObject, INotifyPropertyChanged view)
        {
            var rslt = new List<(string key, BindHandler handler)>();
            if (bind == null)
            {
                return rslt;
            }

            BindListSource(bind.listSource, view, gObject, rslt);

            return rslt;
        }

        private void BindListSource(string listSource, INotifyPropertyChanged view, GObject gObject, List<(string key, BindHandler handler)> rslt)
        {
            if(listSource == null)
            {
                return;
            }

            var viewCollection = view.GetType().GetProperty(listSource).GetValue(view);
            var gList = gObject as GList;

            NotifyCollectionChangedEventHandler onCollectionChanged = (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            foreach (var data in e.NewItems)
                            {
                                var newItem = gList.AddItemFromPool().asCom;
                                newItem.BindDataSource(data as INotifyPropertyChanged);
                                gList.AddChild(newItem);
                            }
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        {
                            foreach (var data in e.OldItems)
                            {
                                var context = BindContext.all.Values.SingleOrDefault(x => x.view == data);
                                if(context == null)
                                {
                                    Debug.Log($"[JLOG] can not find {data.GetHashCode()}");
                                }
                                gList.RemoveChild(context.gComponent);
                            }
                        }
                        break;
                    default:
                        break;
                }
            };

            var handler = new BindHandler()
            {
                Init = (view) =>
                {

                    foreach (var data in (IEnumerable<object>)viewCollection)
                    {
                        var newItem = gList.AddItemFromPool().asCom;
                        newItem.BindDataSource(data as INotifyPropertyChanged);
                        gList.AddChild(newItem);
                    }

                    ((INotifyCollectionChanged)viewCollection).CollectionChanged += onCollectionChanged;
                },

                Exit = () =>
                {
                    ((INotifyCollectionChanged)viewCollection).CollectionChanged -= onCollectionChanged;
                }
            };

            Debug.Log($"[JLOG] GListHander({handler.GetHashCode()})");

            rslt.Add((bind.listSource, handler));
        }

        internal override void Init(GObject leaf, INotifyPropertyChanged view)
        {
            throw new NotImplementedException();
        }
    }
}