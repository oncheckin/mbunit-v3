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
using Gallio.Collections;
using Gallio.Model;
using Gallio.Model.Reflection;
using Gallio.Plugin.XunitAdapter.Properties;

namespace Gallio.Plugin.XunitAdapter.Model
{
    /// <summary>
    /// Builds a test object model based on reflection against Xunit framework attributes.
    /// </summary>
    public class XunitTestFramework : BaseTestFramework
    {
        /// <inheritdoc />
        public override string Name
        {
            get { return Resources.XunitTestFramework_XunitFrameworkName; }
        }

        /// <inheritdoc />
        public override ITestExplorer CreateTestExplorer()
        {
            return new XunitTestExplorer();
        }
    }
}
