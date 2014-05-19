﻿using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBTetminal2
{
    public class Logger : ILoggerFacade
    {
        private Queue<Tuple<string, Category, Priority>> savedLogs = new Queue<Tuple<string, Category, Priority>>();
        private Action<string, Category, Priority> callback;

        /// <summary>
        /// Gets or sets the callback to receive logs.
        /// </summary>
        /// <value>An Action&lt;string, Category, Priority&gt; callback.</value>
        public Action<string, Category, Priority> Callback
        {
            get
            {
                return this.callback;
            }
            set
            {
                this.callback = value;
            }
        }

        /// <summary>
        /// Write a new log entry with the specified category and priority.
        /// </summary>
        /// <param name="message">Message body to log.</param>
        /// <param name="category">Category of the entry.</param>
        /// <param name="priority">The priority of the entry.</param>
        public void Log(string message, Category category, Priority priority)
        {
            if (this.Callback != null)
            {
                this.Callback(message, category, priority);

            }
            else
            {
                savedLogs.Enqueue(new Tuple<string, Category, Priority>(message, category, priority));
            }
        }


        /// <summary>
        /// Replays the saved logs if the Callback has been set.
        /// </summary>
        public void ReplaySavedLogs()
        {
            if (this.Callback != null)
            {
                while (this.savedLogs.Count > 0)
                {
                    var log = this.savedLogs.Dequeue();
                    this.Callback(log.Item1, log.Item2, log.Item3);
                }
            }
        }
    }
}
