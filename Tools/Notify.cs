using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Exchange_App.Tools
{
    public static class Notify
    {
        public static int Warning = 0;
        public static int Success = 1;
        public static int Error = 2;


        public static void ShowNotify(string message, int timespan, int type) {
            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: App.Current.MainWindow,
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(1),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(timespan));

                cfg.Dispatcher = App.Current.Dispatcher;
            });

            if (type == Warning)
            {
                notifier.ShowWarning(message);
            }
            else if (type == Success)
            {
                notifier.ShowSuccess(message);
            } else if (type == Error) { 
                notifier.ShowError(message);
            } else
            {
                notifier.ShowInformation(message);
            }


        }
    }
}
