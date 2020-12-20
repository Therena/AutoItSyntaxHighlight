//
// Copyright 2020 David Roller 
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
        private readonly List<IAutoItLexer> m_Lexer;

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
        
        public IClassificationType ClassificationType { get; }

        public AutoItLexer(IClassificationTypeRegistryService registry)
        {
            this.ClassificationType = registry.GetClassificationType("AutoItEditorClassifier");

            m_Lexer = new List<IAutoItLexer>
            {
                new AutoItLexerComments(registry),
                new AutoItLexerFunctions(registry),
                new AutoItLexerKeywords(registry),
                new AutoItLexerStrings(registry)
            };

            foreach (var item in m_Lexer)
            {
                item.ClassificationChanged += LexerClassificationChanged;
            }
        }
        private void LexerClassificationChanged(object sender, ClassificationChangedEventArgs e)
        {
            ClassificationChanged?.Invoke(sender, e);
        }

        public List<ClassificationSpan> FlattenSpanList(List<PrioritiesClassificationSpan> spanList)
        {
            List<ClassificationSpan> classifications = new List<ClassificationSpan>();
            foreach (var one in spanList)
            {
                bool hasNotHighestPriority = true;
                foreach (var two in spanList)
                {
                    if (one == two || one.Span.Span.OverlapsWith(two.Span.Span) == false)
                    {
                        continue;
                    }

                    if (one.Priority < two.Priority)
                    {
                        hasNotHighestPriority = false;
                        break;
                    }
                }

                if(hasNotHighestPriority)
                {
                    classifications.Add(one.Span);
                }
            }
            return classifications;
        }

        public List<ClassificationSpan> Parse(SnapshotSpan span)
        {
            List<PrioritiesClassificationSpan> priorClassification = new List<PrioritiesClassificationSpan>();
            foreach (var lexer in m_Lexer)
            {
                priorClassification.AddRange(lexer.Parse(span));
            }
            return FlattenSpanList(priorClassification);
        }
    }
}
