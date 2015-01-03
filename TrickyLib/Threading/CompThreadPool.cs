using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TrickyLib.Threading
{
    public interface ICompTask
    {
        void Process();
    }

    public class CompThreadPool
    {
        int m_totalSlots;

        ManualResetEvent m_quitEvt;
        AutoResetEvent m_workEvt;
        AutoResetEvent m_slotEvt;
        WaitHandle[] m_waits;
        Queue<ICompTask> m_taskQ;
        Thread[] m_threads;

        public CompThreadPool(int nThreads, int nSlots)
        {
            m_totalSlots = nSlots;
            m_quitEvt = new ManualResetEvent(false);
            m_workEvt = new AutoResetEvent(false);
            m_slotEvt = new AutoResetEvent(false);
            m_waits = new WaitHandle[2];
            m_waits[0] = m_workEvt;
            m_waits[1] = m_quitEvt;
            m_taskQ = new Queue<ICompTask>();

            m_threads = new Thread[nThreads];
            for (int i = 0; i < nThreads; ++i)
            {
                m_threads[i] = new Thread(Worker);
                m_threads[i].Start();
            }
        }

        public void Worker()
        {
            int signal = 0;
            while (true)
            {
                ICompTask task = null;
                lock (m_taskQ)
                {
                    if (m_taskQ.Count > 0)
                        task = m_taskQ.Dequeue();
                }

                if (task != null)
                {
                    m_slotEvt.Set();
                    task.Process();
                    continue;
                }

                if (signal > 0) break;
                signal = WaitHandle.WaitAny(m_waits);
            }
        }

        public void SignalQuit()
        {
            m_quitEvt.Set();
        }

        public void Join()
        {
            foreach (Thread t in m_threads)
                t.Join();
        }

        public void AddTask(ICompTask task)
        {
            while (true)
            {
                lock (m_taskQ)
                {
                    if (m_taskQ.Count < m_totalSlots)
                    {
                        m_taskQ.Enqueue(task);
                        m_workEvt.Set();
                        break;
                    }
                }
                m_slotEvt.WaitOne();
            }
        }
    }
}
