using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayTimer.Utils
{
    internal class RPC
    {
        public static DiscordRpcClient client;

        public static void Initialize()
        {
            client = new DiscordRpcClient("1005579762028249178");
            Button button = new Button()
            {
                Url = "https://github.com/Psenix/OverlayTimer/",
                Label = "Overlay Timer"
            };
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                Details = "In Menu",
                Buttons = new Button[] { button },
                Assets = new Assets()
                {
                    LargeImageKey = "https://i.imgur.com/neqKcTu.png",
                    LargeImageText = "Psenix's Overlay Timer",
                }
            }); 
        }
        public static void Deinitialize()
        {
            client.Dispose();
        }

        public static void UpdateDetails(string details)
        {
            client.UpdateDetails(details);
        }

        public static void UpdateState(string state)
        {
            client.UpdateState(state);
        }
        public static void ShowTime()
        {
            client.UpdateStartTime();
        }

        public static void HideTime()
        {
            client.UpdateClearTime();
        }
    }
}
