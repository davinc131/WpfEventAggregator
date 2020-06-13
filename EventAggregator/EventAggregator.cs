using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventAggregator
{
    public class EventAggregator : IEventAggregator
    {
        private Dictionary<Type, List<WeakReference>> eventSubscribers = new Dictionary<Type, List<WeakReference>>();
        private readonly object lockSubscriberDictionary = new object();

        #region IEventAggregator Members

        /// <summary>
        /// Publicar um envento
        /// </summary>
        /// <typeparam name="TEventType"></typeparam>
        /// <param name="eventToPublish"></param>
        public void PublishEvent<TEventType>(TEventType eventToPublish)
        {
            var subscriberType = typeof(ISubscriber<>).MakeGenericType(typeof(TEventType));
            var subscribers = GetSubscriberList(subscriberType);

            List<WeakReference> subscribersToBeRemoved = new List<WeakReference>();

            foreach(var weakSubscriber in subscribers)
            {
                if (weakSubscriber.IsAlive)
                {
                    var subscriber = (ISubscriber<TEventType>)weakSubscriber.Target;
                    InvokeSubscriberEvent<TEventType>(eventToPublish, subscriber);
                }
                else
                {
                    subscribersToBeRemoved.Add(weakSubscriber);
                }
            }

            if (subscribersToBeRemoved.Any())
            {
                lock (lockSubscriberDictionary)
                {
                    foreach(var remove in subscribersToBeRemoved)
                    {
                        subscribers.Remove(remove);
                    }
                }
            }
        }

        /// <summary>
        /// Inscreva-se em um evento.
        /// </summary>
        /// <param name="subscriber"></param>
        public void SubscribeEvent(object subscriber)
        {
            lock(lockSubscriberDictionary)
            {
                var subscriberTypes = subscriber.GetType().GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

                WeakReference weakReference = new WeakReference(subscriber);

                foreach(var subscriberType in subscriberTypes)
                {
                    List<WeakReference> subscribers = GetSubscriberList(subscriberType);

                    subscribers.Add(weakReference);
                }
            }
        }

        #endregion

        private void InvokeSubscriberEvent<TEventType>(TEventType eventToPublish, ISubscriber<TEventType> subscriber)
        {
            //Sincronizar a chamada do método

            SynchronizationContext syncContext = SynchronizationContext.Current;

            if (syncContext == null)
                syncContext = new SynchronizationContext();

            syncContext.Post(s => subscriber.OnEventHandler(eventToPublish), null);
        }

        private List<WeakReference> GetSubscriberList(Type subscriberType)
        {
            List<WeakReference> subscribersList = null;

            lock(lockSubscriberDictionary){
                bool found = this.eventSubscribers.TryGetValue(subscriberType, out subscribersList);

                if (!found)
                {
                    subscribersList = new List<WeakReference>();
                    this.eventSubscribers.Add(subscriberType, subscribersList);
                }
            }
            return subscribersList;
        }
    }
}
