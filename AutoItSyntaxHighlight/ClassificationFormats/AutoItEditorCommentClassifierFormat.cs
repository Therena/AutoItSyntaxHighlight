//
// Copyright 2018 David Roller 
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.PlatformUI;

namespace AutoItSyntaxHighlight.ClassificationFormats
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AutoItEditorCommentClassifier")]
    [Name("AutoItEditorCommentClassifier")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.Default)] // Set the priority to be after the default classifiers
    internal sealed class AutoItEditorCommentClassifierFormat : ClassificationFormatDefinition
    {
        public AutoItEditorCommentClassifierFormat()
        {
            this.DisplayName = "AutoIt comments";
            var color = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundBrushKey);
            if(color.R == 37 && color.G == 37 && color.B == 38)
            {
                this.ForegroundColor = Color.FromRgb(87, 166, 74); // Dark Theme
            }
            else
            {
                this.ForegroundColor = Color.FromRgb(0, 128, 0); // Other Themes
            }
        }
    }
}
