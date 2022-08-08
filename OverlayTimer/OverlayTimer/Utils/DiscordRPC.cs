using DiscordRPC;

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
            if (client != null && client.IsInitialized)
                client.Dispose();
        }

        public static void UpdateDetails(string details)
        {
            if (client != null && client.IsInitialized)
                client.UpdateDetails(details);
        }

        public static void UpdateState(string state)
        {
            if (client != null && client.IsInitialized)
                client.UpdateState(state);
        }
        public static void ShowTime()
        {
            if (client != null && client.IsInitialized)
                client.UpdateStartTime();
        }

        public static void HideTime()
        {
            if (client != null && client.IsInitialized)
                client.UpdateClearTime();
        }
    }
}
