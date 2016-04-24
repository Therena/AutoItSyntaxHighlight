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
using AutoItSyntaxHighlight.Helper;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AutoItSyntaxHighlight.Lexer
{
    internal sealed class AutoItLexerStrings : IAutoItLexer
    {
        private readonly IClassificationType m_Type;

        public AutoItLexerStrings(IClassificationTypeRegistryService registry)
        {
            m_Type = registry.GetClassificationType("AutoItEditorStringClassifier");
        }

        public List<ClassificationSpan> Parse(SnapshotSpan span)
        {
            string code = span.GetText();
            code = code.Replace("\\\"", "**");

            Regex reg = new Regex("\"[^\"]*\"", RegexOptions.IgnoreCase);
            var matches = reg.Matches(code);

            if (matches.Count == 0)
            {
                return new List<ClassificationSpan>();
            }

            List<ClassificationSpan> classifications = new List<ClassificationSpan>();
            foreach (Match match in matches)
            {
                Span spanWord = new Span(span.Start.Position + match.Index, match.Length);

                SnapshotSpan snapshot = new SnapshotSpan(span.Snapshot, spanWord);
                classifications.Add(new ClassificationSpan(snapshot, m_Type));
            }
            return classifications;
        }
    }
}
