//
// Copyright 2017 David Roller
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
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace AutoItSyntaxHighlight
{
    internal static class AutoItEditorClassifierClassificationDefinition
    {
        // This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169
#pragma warning disable CS0414
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AutoItEditorClassifier")]
        private static ClassificationTypeDefinition typeDefinition = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AutoItEditorCommentClassifier")]
        private static ClassificationTypeDefinition typeCommentDefinition = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AutoItEditorStringClassifier")]
        private static ClassificationTypeDefinition typeStringDefinition = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AutoItEditorFunctionsClassifier")]
        private static ClassificationTypeDefinition typeFunctionDefinition = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("AutoItEditorKeywordClassifier")]
        private static ClassificationTypeDefinition typeKeywordDefinition = null;

        [Export]
        [Name("au3")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition AutoItContentDefinition = null;

        [Export]
        [FileExtension(".au3")]
        [ContentType("au3")]
        internal static FileExtensionToContentTypeDefinition AutoItFileExtensionDefinition = null;

#pragma warning restore 169
#pragma warning restore CS0414
    }
}
