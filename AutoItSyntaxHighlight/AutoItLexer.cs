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
using AutoItSyntaxHighlight.Helper;
using AutoItSyntaxHighlight.Lexer;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;

namespace AutoItSyntaxHighlight
{
    internal sealed class AutoItLexer
    {
        private List<IAutoItLexer> m_Lexer;
        private readonly IClassificationType classificationType;
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        public AutoItLexer(IClassificationTypeRegistryService registry)
        {
            this.classificationType = registry.GetClassificationType("AutoItEditorClassifier");

            m_Lexer = new List<IAutoItLexer>();
            m_Lexer.Add(new AutoItLexerComments(registry));
            m_Lexer.Add(new AutoItLexerFunctions(registry));
            m_Lexer.Add(new AutoItLexerKeywords(registry));
            m_Lexer.Add(new AutoItLexerStrings(registry));

            foreach(var item in m_Lexer)
            {
                item.ClassificationChanged += LexerClassificationChanged;
            }
        }
        private void LexerClassificationChanged(object sender, ClassificationChangedEventArgs e)
        {
            ClassificationChanged?.Invoke(sender, e);
        }

        public List<ClassificationSpan> flattenSpanList(List<PrioritiesClassificationSpan> spanList)
        {
            List<ClassificationSpan> classifications = new List<ClassificationSpan>();
            foreach (var one in spanList)
            {
                bool hasntHighestPriority = true;
                foreach (var two in spanList)
                {
                    if (one == two || one.Span.Span.OverlapsWith(two.Span.Span) == false)
                    {
                        continue;
                    }

                    if (one.Priority < two.Priority)
                    {
                        hasntHighestPriority = false;
                        break;
                    }
                }

                if(hasntHighestPriority)
                {
                    classifications.Add(one.Span);
                }
            }
            return classifications;
        }

        public List<ClassificationSpan> Parse(SnapshotSpan span)
        {
            List<PrioritiesClassificationSpan> prioClassi = new List<PrioritiesClassificationSpan>();
            foreach (var lexer in m_Lexer)
            {
                prioClassi.AddRange(lexer.Parse(span));
            }
            return flattenSpanList(prioClassi);
        }
    }
}
