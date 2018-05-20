//
// Copyright 2018 David Roller 
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace AutoItSyntaxHighlight
{
    internal class AutoItEditorClassifier : IClassifier
    {
        private AutoItLexer m_Lexer;

        internal AutoItEditorClassifier(IClassificationTypeRegistryService registry)
        {
            m_Lexer = new AutoItLexer(registry);
            m_Lexer.ClassificationChanged += LexerClassificationChanged;
        }

        private void LexerClassificationChanged(object sender, ClassificationChangedEventArgs e)
        {
            ClassificationChanged?.Invoke(sender, e);
        }

        #region IClassifier

#pragma warning disable 67
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore 67

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            return m_Lexer.Parse(span);
        }

        #endregion
    }
}
