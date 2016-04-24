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
using Microsoft.VisualStudio.Text;

namespace AutoItSyntaxHighlight.Helper
{
    internal static class SpanConversions
    {
        private static bool IsBiggerThan(Span bigger, Span lower)
        {
            return bigger.Start < lower.Start && bigger.End > lower.End;
        }

        public static Span GetRelativeComplement(Span span, Span intersection)
        {
            if (span == intersection || IsBiggerThan(intersection, span))
            {
                return new Span();
            }

            if (span.Start < intersection.Start && span.End == intersection.End ||
                span.Start == intersection.Start && span.End > intersection.End)
            {
                return new Span(span.Start, span.Length - intersection.Length);
            }

            return span;
        }

        public static bool IsPointPartOfStringSpan(int point)
        {
            //foreach (Span stringSpan in m_Strings)
            //{
            //    if (stringSpan.Start <= point && stringSpan.End >= point)
            //    {
            //        return true;
            //    }
            //}
            return false;
        }

        public static Span GetIntersectionWithComments(Span spanWord)
        {
            //Span spanWordResult = spanWord;
            //foreach (var commentSpan in m_Comments)
            //{
            //    if (spanWordResult.IntersectsWith(commentSpan) == false)
            //    {
            //        continue;
            //    }

            //    var spanIntersect = spanWordResult.Intersection(commentSpan).Value;
            //    spanWordResult = GetRelativeComplement(spanWordResult, spanIntersect);
            //}
            //return spanWordResult;
            return spanWord;
        }
    }
}
