using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TrickyLib.Extension;

namespace TrickyLib.Threading
{
    public static class ThreadStateExtension
    {
        public static bool IsRunning(this ThreadState threadState)
        {
            if (threadState == ThreadState.Running)
                return true;

            if (threadState.HasFlag(ThreadState.Suspended))
                return true;

            if (threadState.HasFlag(ThreadState.SuspendRequested))
                return true;

            if (threadState.HasFlag(ThreadState.WaitSleepJoin))
                return true;

            return false;
        }

        public static bool IsSuspending(this ThreadState threadState)
        {
            if (threadState.HasFlag(ThreadState.Suspended))
                return true;

            if (threadState.HasFlag(ThreadState.SuspendRequested))
                return true;

            return false;
        }
    }

    public abstract class BaseThreadPool
    {
        public delegate void FinishedItemCountChangedHandler(object sender, EventArgs e);
        private event FinishedItemCountChangedHandler _finishedItemCountChanged;
        public event FinishedItemCountChangedHandler FinishedItemCountChanged
        {
            add
            {
                if (_finishedItemCountChanged == null || !_finishedItemCountChanged.GetInvocationList().Contains(value))
                    _finishedItemCountChanged += value;
            }
            remove
            {
                _finishedItemCountChanged -= value;
            }
        }

        public delegate void ThreadsFinishedHandler(BaseThreadPool sender);
        private event ThreadsFinishedHandler _threadsFinished;
        public event ThreadsFinishedHandler ThreadsFinished
        {
            add
            {
                if (_threadsFinished == null || !_threadsFinished.GetInvocationList().Contains(value))
                    _threadsFinished += value;
            }
            remove
            {
                _threadsFinished -= value;
            }
        }

        public delegate void FinishedPercentageChangedHandler(object sender, double percentage);
        private event FinishedPercentageChangedHandler _finishedPercentageChanged;
        public event FinishedPercentageChangedHandler FinishedPercentageChanged
        {
            add
            {
                if (_finishedPercentageChanged == null || !_finishedPercentageChanged.GetInvocationList().Contains(value))
                    _finishedPercentageChanged += value;
            }
            remove
            {
                _finishedPercentageChanged -= value;
            }
        }

        public delegate void Finished10MorePercentageHandler(BaseThreadPool sender);
        private event Finished10MorePercentageHandler _finished10MorePercentage;
        public event Finished10MorePercentageHandler Finished10MorePercentage
        {
            add
            {
                if (_finished10MorePercentage == null || !_finished10MorePercentage.GetInvocationList().Contains(value))
                    _finished10MorePercentage += value;
            }
            remove
            {
                _finished10MorePercentage -= value;
            }
        }

        private int lastFinishedPercentage = 0;

        private int _finishedItemCount;
        public int FinishedItemCount
        {
            get
            {
                return this._finishedItemCount;
            }
            set
            {
                this._finishedItemCount = value;

                if (_finishedItemCountChanged != null)
                    _finishedItemCountChanged(this, new EventArgs());

                int currentFinishedPercentage = this.TotalItemCount <= 0 ? 0 : Convert.ToInt32(value * 100 / this.TotalItemCount);
                if (currentFinishedPercentage != lastFinishedPercentage)
                {
                    if (currentFinishedPercentage / 10 - lastFinishedPercentage / 10 > 0)
                        FinishWork(currentFinishedPercentage < 100);

                    if (_finishedPercentageChanged != null)
                        _finishedPercentageChanged(this, currentFinishedPercentage / 100.0);

                    lastFinishedPercentage = currentFinishedPercentage;
                }
            }
        }
        public int TotalItemCount
        {
            get
            {
                if (this.Items != null)
                    return this.Items.Count;
                else
                    return 0;
            }
        }

        public List<BaseThread> Threads { get; set; }
        public List<object> Items { get; set; }
        public int ThreadCount { get; protected set; }
        public List<KeyValuePair<int, object>> Results { get; set; }

        private int _finishedThreadCount;
        public int FinishedThreadCount
        {
            get
            {
                return this._finishedThreadCount;
            }
            set
            {
                lock (this)
                {
                    this._finishedThreadCount = value;
                    if (value == this.ThreadCount)
                    {
                        FinishWork(false);
                        if (_threadsFinished != null)
                            _threadsFinished(this);
                    }
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                if (this.Threads == null || this.Threads.Count == 0)
                    return false;
                else
                {
                    if (this.FinishedThreadCount == this.ThreadCount)
                        return false;

                    foreach (var thread in this.Threads)
                        if (thread.ThreadState.IsRunning())
                            return true;

                    return false;
                }
            }
        }
        public bool NeedConsolePrint { get; set; }

        public BaseThreadPool(int threadCount, bool needConsolePrint = true)
        {
            this.ThreadCount = threadCount;
            this.Threads = new List<BaseThread>();
            this.Items = new List<object>();
        }

        public void StartWork()
        {
            StartWork(false);
        }

        public void StartWork(bool useRandomItems)
        {
            try
            {
                AssignWork(useRandomItems);
                if (Items == null || Items.Count <= 0)
                    throw new Exception("The items are emtpy. Please check whether you have specify the [Performance File]. If yes, please remove that and try again");

                StartThreads();
                JoinThreads();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void StartWork(int millionSeconds)
        {
            StartWork(false, millionSeconds);
        }

        public void StartWork(bool useRandomItems, int millionSeconds)
        {
            try
            {
                AssignWork(useRandomItems);
                if (Items == null || Items.Count <= 0)
                    throw new Exception("The items are emtpy. Please check whether you have specify the [Performance File]. If yes, please remove that and try again");
                StartThreads();
                JoinThreads(millionSeconds);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void StartThreads()
        {
            this.FinishedItemCount = 0;
            this.FinishedThreadCount = 0;

            foreach (var thread in this.Threads)
                thread.Start();
        }

        private void JoinThreads(int millionSeconds)
        {
            foreach (var thread in this.Threads)
                thread.Join(millionSeconds);
        }

        private void JoinThreads()
        {
            foreach (var thread in this.Threads)
                thread.Join();
        }

        protected void PrepareForRestart()
        {
            for (int i = 0; i < this.ThreadCount; i++)
                this.Threads[i].RefreshThread();

            this.FinishedThreadCount = 0;
        }

        protected void AssignWork()
        {
            AssignWork(false);
        }

        protected virtual void AssignWork(bool useRandomItems)
        {
            if (this.Items == null || this.Items.Count <= 0)
                throw new ArgumentNullException("Items is empty, please initial [Items] before calling [AssignWork()]. You may check whether you have specified the [PerformanceFile] and it contains all the parameters. You can remove it and try again");

            if (useRandomItems)
                this.Items = this.Items.ToRandomList();

            if (this.ThreadCount > this.Items.Count)
                this.ThreadCount = this.Items.Count;

            int per = (this.Items.Count + 1) / this.ThreadCount;
            int currentIndex = 0;

            if (this.Threads == null)
                this.Threads = new List<BaseThread>();
            else
                this.Threads.Clear();

            for (int i = 0; i < this.ThreadCount - 1; i++)
            {
                BaseThread thread = CreateBaseThread(this.Items.GetRange(currentIndex, per).ToArray());
                thread.StartIndex = currentIndex;

                this.Threads.Add(thread);
                currentIndex += per;
            }

            if (currentIndex < this.Items.Count)
            {
                BaseThread thread = CreateBaseThread(this.Items.GetRange(currentIndex, this.Items.Count - currentIndex).ToArray());
                thread.StartIndex = currentIndex;

                this.Threads.Add(thread);
            }

            for (int i = 0; i < this.ThreadCount; i++)
                this.Threads[i].ThreadID = i;
        }

        public virtual void StopWork(bool callFinishWork)
        {
            foreach (var thread in this.Threads)
            {
                if (thread.ThreadState.IsSuspending())
                    thread.Resume();

                thread.Stop();
            }

            if (callFinishWork)
            {
                FinishWork(false);

                if (_threadsFinished != null)
                    _threadsFinished(this);
            }
        }

        public virtual void PauseWork(bool callFinishWork)
        {
            foreach (var thread in this.Threads)
                thread.Pause();

            if (callFinishWork)
                FinishWork(true);
        }

        public void ResumeWork()
        {
            foreach (var thread in this.Threads)
                thread.Resume();
        }

        protected virtual void FinishWork(bool isPause)
        {
            this.Results = new List<KeyValuePair<int, object>>();
            foreach (var thread in this.Threads)
            {
                lock (thread)
                {
                    this.Results.AddRange(thread.Results);
                }
            }

            if (_finished10MorePercentage != null)
                _finished10MorePercentage(this);
        }

        protected abstract BaseThread CreateBaseThread(object[] items);
    }

    public abstract class BaseThread
    {
        protected BaseThreadPool Parent { get; set; }
        protected Thread WorkThread { get; set; }
        public List<KeyValuePair<int, object>> Results { get; set; }

        public object[] Items { get; set; }
        public int StartIndex { get; set; }
        public int ThreadID { get; set; }
        public string ThreadGUID { get; protected set; }
        public ThreadState ThreadState
        {
            get
            {
                if (this.WorkThread == null)
                    throw new ArgumentNullException("Thread is null");
                else
                    return this.WorkThread.ThreadState;
            }
        }

        public int HandledItemCount { get; set; }

        private int _finishedItemCount;
        public int FinishedItemCount
        {
            get
            {
                return this._finishedItemCount;
            }
            set
            {
                lock (this.Parent)
                {
                    this.Parent.FinishedItemCount += value - this._finishedItemCount;

                    if (this.Parent.NeedConsolePrint)
                        Console.WriteLine("[Thread " + this.ThreadID.ToString() + "]" + " I finished " + value * 1.0 / this.Items.Length);
                }

                this._finishedItemCount = value;
            }
        }

        public bool AllHandled
        {
            get
            {
                return this.HandledItemCount >= this.Items.Length;
            }
        }

        public BaseThread(BaseThreadPool parent, object[] items)
        {
            this.ThreadGUID = System.Guid.NewGuid().ToString().Replace("-", "");

            this.Parent = parent;
            this.Items = items;
            this.WorkThread = new Thread(Work);
            this.Results = new List<KeyValuePair<int, object>>();
        }

        public void Start()
        {
            try
            {
                if (this.WorkThread != null)
                    this.WorkThread.Start();
                else
                    throw new ArgumentNullException("Thread");
            }
            catch (Exception e)
            {
                if (this.Parent.NeedConsolePrint)
                    Console.WriteLine(e.Message);

                throw e;
            }
        }

        public void Join(int milliSeconds)
        {
            try
            {
                if (this.WorkThread != null)
                    this.WorkThread.Join(milliSeconds);
                else
                    throw new ArgumentNullException("Thread");
            }
            catch (Exception e)
            {
                if (this.Parent.NeedConsolePrint)
                    Console.WriteLine(e.Message);

                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public void Join()
        {
            try
            {
                if (this.WorkThread != null)
                    this.WorkThread.Join();
                else
                    throw new ArgumentNullException("Thread");
            }
            catch (Exception e)
            {
                if (this.Parent.NeedConsolePrint)
                    Console.WriteLine(e.Message);

                Console.WriteLine(e.Message);
                throw e;
            }
        }

        public void RefreshThread()
        {
            this.HandledItemCount = 0;
            this.WorkThread = new Thread(Work);
        }

        public void PrintFinished(int step)
        {
            if (this.HandledItemCount % step == 0 && this.Parent.NeedConsolePrint)
                Console.WriteLine("Thread {0} finished: {1}", this.ThreadID, this.HandledItemCount);
        }

        public virtual void Stop()
        {
            if (this.WorkThread != null && WorkThread.ThreadState.IsRunning())
            {
                if (WorkThread.ThreadState.IsSuspending())
                    this.WorkThread.Resume();

                this.WorkThread.Abort();
            }
        }

        public void Pause()
        {
            if (this.WorkThread != null && WorkThread.ThreadState.IsRunning())
                this.WorkThread.Suspend();
        }

        public void Resume()
        {
            if (this.WorkThread != null && WorkThread.ThreadState.IsSuspending())
                this.WorkThread.Resume();
        }

        public virtual void Work()
        {
            this.Results = this.Results.OrderBy(kv => kv.Key).ToList();

            lock (this.Parent)
            {
                Parent.FinishedThreadCount++;
            }
        }

        protected void ReceiveResult(KeyValuePair<int, object> ResultKv)
        {
            if (ResultKv.Key < 0)
                return;

            lock (this.Results)
            {
                this.Results.Add(ResultKv);
            }
        }
    }

    public abstract class BaseThread_Range : BaseThread
    {
        public int DoRange { get; set; }

        public BaseThread_Range(BaseThreadPool parent, object[] items, int doRange)
            : base(parent, items)
        {
            if (doRange <= 0)
                throw new ArgumentOutOfRangeException("doRange should be larger than 0");
            else
                this.DoRange = doRange;
        }

        public override void Work()
        {
            while (this.HandledItemCount < this.Items.Length)
            {
                var kv = GetNextItems();
                var result = DoItems(kv.Value);
                ReceiveResult(new KeyValuePair<int, object>(kv.Key, result));

                lock (this.Items)
                {
                    FinishedItemCount += kv.Value.Length;
                }
            }
            int unFinishedThreadID = -1;
            while ((unFinishedThreadID = this.Parent.Threads.FindIndex(t => !t.AllHandled)) >= 0)
            {
                var kv = ((BaseThread_Range)this.Parent.Threads[unFinishedThreadID]).GetNextItems();
                var result = DoItems(kv.Value);
                ((BaseThread_Range)this.Parent.Threads[unFinishedThreadID]).ReceiveResult(new KeyValuePair<int, object>(kv.Key, result));

                lock (this.Parent.Threads[unFinishedThreadID].Items)
                {
                    this.Parent.Threads[unFinishedThreadID].FinishedItemCount += kv.Value.Length;
                }
            }
            base.Work();
        }

        private KeyValuePair<int, object[]> GetNextItems()
        {
            lock (this.Items)
            {
                if (this.HandledItemCount >= this.Items.Length)
                    return new KeyValuePair<int, object[]>(-1, null);
                else
                {
                    object[] items = this.Items.GetRange(this.HandledItemCount, this.DoRange).ToArray();
                    var nextItemsKv = new KeyValuePair<int, object[]>(this.HandledItemCount, items);
                    this.HandledItemCount += items.Length;

                    return nextItemsKv;
                }
            }
        }

        protected abstract object DoItems(object[] item);
    }

    public abstract class BaseThread_One : BaseThread
    {

        public BaseThread_One(BaseThreadPool parent, object[] items)
            : base(parent, items)
        {
        }

        public override void Work()
        {
            while (this.HandledItemCount < this.Items.Length)
            {
                var kv = GetNextItem();
                var result = DoItem(kv.Value);
                ReceiveResult(new KeyValuePair<int, object>(kv.Key, result));

                lock (this.Items)
                {
                    ++FinishedItemCount;
                }
            }
            int unFinishedThreadID = -1;
            while ((unFinishedThreadID = this.Parent.Threads.FindIndex(t => !t.AllHandled)) >= 0)
            {
                var kv = ((BaseThread_One)this.Parent.Threads[unFinishedThreadID]).GetNextItem();
                var result = DoItem(kv.Value);
                ((BaseThread_One)this.Parent.Threads[unFinishedThreadID]).ReceiveResult(new KeyValuePair<int, object>(kv.Key, result));

                lock (this.Parent.Threads[unFinishedThreadID].Items)
                {
                    ++this.Parent.Threads[unFinishedThreadID].FinishedItemCount;
                }
            }
            base.Work();
        }

        private KeyValuePair<int, object> GetNextItem()
        {
            lock (this.Items)
            {
                if (this.HandledItemCount >= this.Items.Length)
                    return new KeyValuePair<int, object>(-1, null);
                else
                {
                    object item = this.Items[this.HandledItemCount];
                    var nextItemKv = new KeyValuePair<int, object>(this.HandledItemCount, item);
                    this.HandledItemCount++;

                    return nextItemKv;
                }
            }
        }

        protected abstract object DoItem(object item);
    }
}
