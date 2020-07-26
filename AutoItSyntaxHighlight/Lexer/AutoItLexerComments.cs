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
using System.Linq;
using AutoItSyntaxHighlight.Helper;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AutoItSyntaxHighlight.Lexer
{
    internal sealed class AutoItLexerComments : IAutoItLexer
    {
        private Regex m_Regex;
        private readonly IClassificationType m_Type;
        private List<MultiLineComment> m_MultiLineComments;
        private IClassificationTypeRegistryService m_Registry;
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        public AutoItLexerComments(IClassificationTypeRegistryService registry)
        {
            m_Registry = registry;
            m_MultiLineComments = new List<MultiLineComment>();
            m_Type = m_Registry.GetClassificationType("AutoItEditorCommentClassifier");
            m_Regex = new Regex(@"\s*;", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private List<PrioritiesClassificationSpan> ParseMultiLine(SnapshotSpan span)
        {
            List<PrioritiesClassificationSpan> classifications = new List<PrioritiesClassificationSpan>();

            string code = span.GetText();

            int startPosition = IndexOfCommentStart(code);
            if (startPosition < 0)
            {
                return new List<PrioritiesClassificationSpan>();
            }
            
            int segmentIndex = span.Start.Position + startPosition;
            int lineNumber = span.Snapshot.GetLineNumberFromPosition(segmentIndex);

            try
            {
                string codeSegement = span.Snapshot.GetLineFromLineNumber(lineNumber).GetText();
                int endPosition = IndexOfCommentEnd(codeSegement);
                while (endPosition < 0)
                {
                    ++lineNumber;
                    codeSegement = span.Snapshot.GetLineFromLineNumber(lineNumber).GetText();
                    endPosition = IndexOfCommentEnd(codeSegement);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                lineNumber = lineNumber - 1;
            }

            int startIndex = span.Start.Position + startPosition;
            int endIndex = span.Snapshot.GetLineFromLineNumber(lineNumber).End.Position;
            var multiSpan = new SnapshotSpan(span.Snapshot, startIndex, (endIndex - startIndex));

            if (multiSpan.End > span.End)
            {
                ClassificationChanged?.Invoke(this,
                    new ClassificationChangedEventArgs(new SnapshotSpan(span.End + 1, multiSpan.End)));
            }

            var prioSpan = new PrioritiesClassificationSpan();
            prioSpan.Span = new ClassificationSpan(multiSpan, m_Type);
            prioSpan.Priority = 400;
            classifications.Add(prioSpan);

            if (m_MultiLineComments.Any(a => a.Tracking.GetSpan(span.Snapshot).Span == multiSpan.Span) == false)
            {
                m_MultiLineComments.Add(new MultiLineComment()
                {
                    Version = span.Snapshot.Version,
                    Tracking = span.Snapshot.CreateTrackingSpan(multiSpan.Span, SpanTrackingMode.EdgeExclusive)
                });
            }

            return classifications;
        }

        public List<PrioritiesClassificationSpan> Parse(SnapshotSpan span)
        {
            List<PrioritiesClassificationSpan> classifications = new List<PrioritiesClassificationSpan>();

            bool isInsideOfComment = false;
            for (int i = m_MultiLineComments.Count - 1; i >= 0; i--)
            {
                var comment = m_MultiLineComments[i];
                var multiSpan = comment.Tracking.GetSpan(span.Snapshot);
                if(multiSpan.Length == 0)
                {
                    m_MultiLineComments.RemoveAt(i);
                    continue;
                }

                if (span.IntersectsWith(multiSpan) == false)
                {
                    continue;
                }

                isInsideOfComment = true;
                if (span.Snapshot.Version == comment.Version)
                {
                    var prioSpan = new PrioritiesClassificationSpan();
                    prioSpan.Span = new ClassificationSpan(multiSpan, m_Type);
                    prioSpan.Priority = 400;
                    classifications.Add(prioSpan);
                }
                else
                {
                    m_MultiLineComments.RemoveAt(i);
                    ClassificationChanged?.Invoke(this, new ClassificationChangedEventArgs(multiSpan));
                    continue;
                }
            }

            if (isInsideOfComment == false)
            {
                classifications.AddRange(ParseMultiLine(span));
            }

            string code = span.GetText();
            var matches = m_Regex.Matches(code);

            if (matches.Count == 0)
            {
                return classifications;
            }

            foreach (Match match in matches)
            {
                if(IsMatchInString(match, span))
                {
                    continue;
                }

                Span spanWord = new Span(span.Start.Position + match.Index,
                    span.GetText().Length - match.Index);
                SnapshotSpan snapshot = new SnapshotSpan(span.Snapshot, spanWord);

                var prioSpan = new PrioritiesClassificationSpan();
                prioSpan.Span = new ClassificationSpan(snapshot, m_Type);
                prioSpan.Priority = 400;
                classifications.Add(prioSpan);
            }
            return classifications;
        }

        private bool IsMatchInString(Match match, SnapshotSpan span)
        {
            var stringLexer = new AutoItLexerStrings(m_Registry);
            var stringSpans = stringLexer.Parse(span);

            foreach(var strSpan in stringSpans)
            {
                if (strSpan.Span.Span.Contains(span.Start.Position + match.Index))
                {
                    return true;
                }
            }
            return false;
        }

        private int IndexOfCommentStart(string code)
        {
            if (code.Contains("#cs"))
            {
                return code.IndexOf("#cs");
            }

            if (code.Contains("#comment-start"))
            {
                return code.IndexOf("#comment-start");
            }
            return -1;
        }

        private int IndexOfCommentEnd(string code)
        {
            if (code.Contains("#ce"))
            {
                return code.IndexOf("#ce");
            }

            if (code.Contains("#comment-end"))
            {
                return code.IndexOf("#comment-end");
            }
            return -1;
        }
    }
}
