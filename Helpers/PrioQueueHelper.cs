using CurePlease.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class PrioQueueHelper
    {
        private static Queue<CastingAction> queue = new Queue<CastingAction>();
        public static CastingAction popQueueItem()
        {
            return queue.Count() > 0 ? queue.Dequeue(): null;
        }
        public static CastingAction peekQueueItem()
        {
            return queue.Count() > 0 ? queue.Peek(): null;
        }
        public static void pushQueueItem(string spell, string player)
        {
            queue.Enqueue(new CastingAction(SpellType.Prio, spell, player));
        }
    }
}
