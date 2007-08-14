// Copyright 2007 MbUnit Project - http://www.mbunit.com/
// Portions Copyright 2000-2004 Jonathan De Halleux, Jamie Cansdale
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using MbUnit.Framework.Kernel.Utilities;

namespace MbUnit.Framework.Kernel.Serialization
{
    /// <summary>
    /// The test model captures the root of the test tree along with an index by id.
    /// </summary>
    /// <remarks>
    /// This class is safe for used by multiple threads.
    /// </remarks>
    [Serializable]
    [XmlRoot("testModel", Namespace=SerializationUtils.XmlNamespace)]
    [XmlType(Namespace=SerializationUtils.XmlNamespace)]
    public sealed class TestModel
    {
        [NonSerialized]
        private Dictionary<string, TestInfo> tests;

        private TestInfo rootTest;

        /// <summary>
        /// Creates an uninitialized instance for Xml deserialization.
        /// </summary>
        private TestModel()
        {
        }

        /// <summary>
        /// Creates a test model.
        /// </summary>
        /// <param name="rootTest">The root test</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rootTest"/> is null</exception>
        public TestModel(TestInfo rootTest)
        {
            if (rootTest == null)
                throw new ArgumentNullException("rootTest");

            this.rootTest = rootTest;
        }

        /// <summary>
        /// Gets or sets the root test in the model.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null</exception>
        [XmlElement("root", IsNullable = false)]
        public TestInfo RootTest
        {
            get { return rootTest; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                lock (this)
                {
                    rootTest = value;
                    tests = null;
                }
            }
        }

        /// <summary>
        /// Gets a dictionary of tests indexed by id.
        /// </summary>
        [XmlIgnore]
        public IDictionary<string, TestInfo> Tests
        {
            get
            {
                lock (this)
                {
                    if (tests == null)
                    {
                        tests = new Dictionary<string, TestInfo>();
                        PopulateTests(rootTest);
                    }

                    return tests;
                }
            }
        }

        private void PopulateTests(TestInfo test)
        {
            tests[test.Id] = test;

            foreach (TestInfo child in test.Children)
                PopulateTests(child);
        }
    }
}
