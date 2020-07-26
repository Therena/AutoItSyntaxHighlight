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
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AutoItSyntaxHighlight.Lexer
{
    internal sealed class AutoItLexerKeywords : IAutoItLexer
    {
        private readonly IClassificationType m_Type;
        private List<string> m_Keywords;

#pragma warning disable CS0067
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore CS0067

        public AutoItLexerKeywords(IClassificationTypeRegistryService registry)
        {
            m_Type = registry.GetClassificationType("AutoItEditorKeywordClassifier");

            // https://www.autoitscript.com/autoit3/docs/keywords.htm
            m_Keywords = new List<string>(new string[] { "True", "False",
                "ContinueCase", "ContinueLoop", "#Region", "#EndRegion",
                "Default", "Dim", "Global", "Local", "Const", "Do", "Until", "Enum",
                "Exit", "ExitLoop", "For", "To", "Step", "Next", "In", "Next",
                "Func", "Return", "EndFunc", "If", "Then", "ElseIf", "Else", "EndIf",
                "#include-once", "#include", "#NoTrayIcon", "Null", "#OnAutoItStartRegister",
                "#pragma", "ReDim", "#RequireAdmin", "Select", "Case", "EndSelect",
                "Static", "Switch", "Case", "EndSwitch", "Volatile", "While", "WEnd",
                "With", "EndWith" });
        }

        public List<PrioritiesClassificationSpan> Parse(SnapshotSpan span)
        {
            List<PrioritiesClassificationSpan> classifications = new List<PrioritiesClassificationSpan>();
            foreach (string word in m_Keywords)
            {
                Regex reg = new Regex(@"(" + word + @")[\s<]+", RegexOptions.IgnoreCase);
                var matches = reg.Matches(span.GetText());

                if (matches.Count == 0)
                {
                    continue;
                }

                foreach (Match match in matches)
                {
                    if (match.Groups.Count == 0)
                    {
                        continue;
                    }

                    Group group = match.Groups[1];
                    if (group.Index > 0)
                    {
                        Regex regWhitspace = new Regex(@"\s+", RegexOptions.IgnoreCase);
                        string code = "" + span.GetText()[group.Index - 1];
                        if (regWhitspace.IsMatch(code) == false)
                        {
                            continue;
                        }
                    }

                    Span spanWord = new Span(span.Start.Position + group.Index, group.Length);
                    SnapshotSpan snapshot = new SnapshotSpan(span.Snapshot, spanWord);

                    var prioSpan = new PrioritiesClassificationSpan();
                    prioSpan.Span = new ClassificationSpan(snapshot, m_Type);
                    prioSpan.Priority = 200;
                    classifications.Add(prioSpan);
                }
            }
            return classifications;
        }
    }
}
