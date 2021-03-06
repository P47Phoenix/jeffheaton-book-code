// The Heaton Research Spider for .Net 
// Copyright 2007 by Heaton Research, Inc.
// 
// From the book:
// 
// HTTP Recipes for C# Bots, ISBN: 0-9773206-7-7
// http://www.heatonresearch.com/articles/series/20/
// 
// This class is released under the:
// GNU Lesser General Public License (LGPL)
// http://www.gnu.org/copyleft/lesser.html
//
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace HeatonResearch.Spider
{
    /// <summary>
    /// SpiderOptions: This class contains options for the
    /// spider's execution.
    /// </summary>
    public class SpiderOptions
    {
        /// <summary>
        /// Specifies that when the spider starts up it should clear the workload.
        /// </summary>
        public const String STARTUP_CLEAR = "CLEAR";

        /// <summary>
        /// Specifies that the spider should resume processing its workload.
        /// </summary>
        public const String STARTUP_RESUME = "RESUME";

        /// <summary>
        /// How many milliseconds to wait when downloading pages.
        /// </summary>
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }

        /// <summary>
        /// The maximum depth to search pages. -1 specifies no maximum depth.
        /// </summary>
        public int MaxDepth
        {
            get
            {
                return maxDepth;
            }
            set
            {
                maxDepth = value;
            }
        }

        /// <summary>
        /// What user agent should be reported to the web site.  This allows the web site to determine what browser is being used.
        /// </summary>
        public String UserAgent
        {
            get
            {
                return userAgent;
            }
            set
            {
                userAgent = value;
            }
        }


        /// <summary>
        /// The connection string for databases. Used to hold the workload.
        /// </summary>
        public String DbConnectionString
        {
            get
            {
                return dbConnectionString;
            }
            set
            {
                dbConnectionString = value;
            }
        }


        /// <summary>
        /// What class to use as a workload manager.
        /// </summary>
        public String WorkloadManager
        {
            get
            {
                return workloadManager;
            }
            set
            {
                workloadManager = value;
            }
        }


        /// <summary>
        /// How to startup the spider, either clear or resume.
        /// </summary>
        public String Startup
        {
            get
            {
                return startup;
            }
            set
            {
                startup = value;
            }
        }

        /// <summary>
        ///  Specifies a class to be used a filter.
        /// </summary>
        public List<String> Filter
        {
            get
            {
                return filter;
            }
        }



        private int timeout = 60000;
        private int maxDepth = -1;
        private String userAgent = null;
        private String dbConnectionString;
        private String workloadManager;
        private String startup = STARTUP_CLEAR;
        private List<String> filter = new List<String>();

        /// <summary>
        /// Load the spider settings from a configuration file.
        /// </summary>
        /// <param name="inputFile">The name of the configuration file.</param>
        public void Load(String inputFile)
        {
            StreamReader r = File.OpenText(inputFile);

            String line;
            while ((line = r.ReadLine()) != null)
            {
                    ParseLine(line);
               
            }
            r.Close();
        }


        /// <summary>
        /// The line of text read from the configuration file.
        /// </summary>
        /// <param name="line">The line of text read from the configuration file.</param>
        private void ParseLine(String line)
        {
            String name, value;
            int i = line.IndexOf(':');
            if (i == -1)
            {
                return;
            }
            name = line.Substring(0, i).Trim();
            value = line.Substring(i + 1).Trim();

            if (value.Trim().Length == 0)
            {
                value = null;
            }


            Type myType = typeof(SpiderOptions);
            FieldInfo field = myType.GetField(name,
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                throw new SpiderException("Unknown configuration file element: " + name + " .");
            }
            else if (field.FieldType.Equals(typeof(String)))
            {
                field.SetValue(this, value);
            }
            else if (field.FieldType.Equals(typeof(List<String>)))
            {
                List<String> list = (List<String>)field.GetValue(this);
                list.Add(value);
            }
            else
            {
                int x = int.Parse(value);
                field.SetValue(this, x);
            }
        }

        
    }
}
