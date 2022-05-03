using EliteMMO.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class ItemHelper
    {
        public static ushort GetItemId(EliteAPI api, string name)
        {
            EliteAPI.IItem item = api.Resources.GetItem(name, 0);
            return item != null ? (ushort)item.ItemID : (ushort)0;
        }
        public static int GetTempItemCount(EliteAPI api, ushort itemid)
        {
            int count = 0;
            for (int x = 0; x <= 80; x++)
            {
                EliteAPI.InventoryItem item = api.Inventory.GetContainerItem(3, x);
                if (item != null && item.Id == itemid)
                {
                    count += (int)item.Count;
                }
            }

            return count;
        }
        private static int GetInventoryItemCount(EliteAPI api, ushort itemid)
        {
            int count = 0;
            for (int x = 0; x <= 80; x++)
            {
                EliteAPI.InventoryItem item = api.Inventory.GetContainerItem(0, x);
                if (item != null && item.Id == itemid)
                {
                    count += (int)item.Count;
                }
            }

            return count;
        }

        public static bool HasItem(EliteAPI api, string name)
        {
            return GetInventoryItemCount(api, GetItemId(api, name)) > 0 || GetTempItemCount(api, GetItemId(api, name)) > 0;
        }

        public static string GetSilenaItem()
        {
            if (OptionsForm.config.plSilenceItem == 0)
            {
                return "Catholicon";
            }
            else if (OptionsForm.config.plSilenceItem == 1)
            {
                return "Echo Drops";
            }
            else if (OptionsForm.config.plSilenceItem == 2)
            {
                return "Remedy";
            }
            else if (OptionsForm.config.plSilenceItem == 3)
            {
                return "Remedy Ointment";
            }
            else if (OptionsForm.config.plSilenceItem == 4)
            {
                return "Vicar's Drink";
            }
            return "Echo Drops";
        }
        public static string GetCursnaItem()
        {
            if (OptionsForm.config.plDoomitem == 0)
            {
                return "Holy Water";
            }
            else if (OptionsForm.config.plDoomitem == 1)
            {
                return "Hallowed Water";
            }
            return "Holy Water";
        }
    }
}
