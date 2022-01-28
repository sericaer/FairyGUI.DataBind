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

        internal override void Init(GObject gObject, INotifyPropertyChanged view)
        {
            var list = gObject.asList;

            BindListSource(list, view);
        }

        private void BindListSource(GList gList, INotifyPropertyChanged view)
        {
            if (bind.listSource == null)
            {
                return;
            }

            var viewCollection = view.GetType().GetProperty(bind.listSource).GetValue(view);

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
                                if (context == null)
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

            handerManager.Add(bind.listSource, handler);
        }
    }
}