using Microsoft.UI.Dispatching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DuoTesting.Helper
{
    public static class WinUITestHelper
    {
        public static void RunOnUIThread(Action testAction)
        {
            ManualResetEventSlim testCompleted = new ManualResetEventSlim(false);
            Exception? testException = null;

            // WinUI 3 requires DispatcherQueue
            DispatcherQueueController? controller = null;
            DispatcherQueue? dispatcherQueue = null;

            var thread = new Thread(() =>
            {
                try
                {
                    // Initialize DispatcherQueue (WinUI 3's equivalent of Dispatcher)
                    controller = DispatcherQueueController.CreateOnCurrentThread();
                    dispatcherQueue = DispatcherQueue.GetForCurrentThread();

                    //  Run test on UI thread
                    dispatcherQueue.TryEnqueue(() =>
                    {
                        try
                        {
                            testAction();
                        }
                        finally
                        {
                            testCompleted.Set();
                        }
                    });

                    // Process UI messages (like Dispatcher.Run())
                    while (!testCompleted.IsSet)
                    {
                        Thread.Sleep(10);
                        dispatcherQueue.TryEnqueue(() => { }); // Keep queue alive
                    }
                }
                catch (Exception ex)
                {
                    testException = ex;
                }
                finally
                {
                    testCompleted.Set();
                    controller?.ShutdownQueueAsync().AsTask().Wait(); // Cleanup
                }
            });

            thread.SetApartmentState(ApartmentState.STA); // Still needed for COM
            thread.Start();
            thread.Join();

            if (testException != null)
                throw testException;
        }
    }
}
