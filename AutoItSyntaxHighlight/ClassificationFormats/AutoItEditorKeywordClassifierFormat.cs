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
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace AutoItSyntaxHighlight.ClassificationFormats
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AutoItEditorKeywordClassifier")]
    [Name("AutoItEditorKeywordClassifier")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
    internal sealed class AutoItEditorKeywordClassifierFormat : ClassificationFormatDefinition
    {
        public AutoItEditorKeywordClassifierFormat()
        {
            this.DisplayName = "AutoItEditorKeywordClassifier"; // Human readable version of the name
            var color = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundBrushKey);
            if (color.R == 37 && color.G == 37 && color.B == 38)
            {
                this.ForegroundColor = Color.FromRgb(86, 156, 214); // Dark Theme
            }
            else
            {
                this.ForegroundColor = Color.FromRgb(0, 0, 255); // Other Themes
            }
        }
    }
}
