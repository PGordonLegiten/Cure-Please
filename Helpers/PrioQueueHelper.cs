using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class QueueItem
    {
        public string SpellName {get; set;}
        public string Target { get; set; }
    }
    internal class PrioQueueHelper
    {
        private static Queue<QueueItem> queue = new Queue<QueueItem>();
        public static QueueItem popQueueItem()
        {
            return queue.Count() > 0 ? queue.Dequeue(): null;
        }
        public static QueueItem peekQueueItem()
        {
            return queue.Count() > 0 ? queue.Peek(): null;
        }
        public static void pushQueueItem(string spell, string player)
        {
            queue.Enqueue(new QueueItem() { SpellName = spell, Target = player });
        }
    }
}
