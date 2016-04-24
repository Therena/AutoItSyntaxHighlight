//
// Copyright 2016 David Roller
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
using AutoItSyntaxHighlight.Lexer;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;

namespace AutoItSyntaxHighlight
{
    internal sealed class AutoItLexer
    {
        private List<IAutoItLexer> m_Lexer;
        private readonly IClassificationType classificationType;

        public AutoItLexer(IClassificationTypeRegistryService registry)
        {
            this.classificationType = registry.GetClassificationType("AutoItEditorClassifier");

            m_Lexer = new List<IAutoItLexer>();
            m_Lexer.Add(new AutoItLexerComments(registry));
            m_Lexer.Add(new AutoItLexerFunctions(registry));
            m_Lexer.Add(new AutoItLexerKeywords(registry));
            m_Lexer.Add(new AutoItLexerStrings(registry));
        }

        public List<ClassificationSpan> Parse(SnapshotSpan span)
        {
            List<ClassificationSpan> classifications = new List<ClassificationSpan>();
            foreach (var lexer in m_Lexer)
            {
                classifications.AddRange(lexer.Parse(span));
            }
            return classifications;
        }
    }
}
