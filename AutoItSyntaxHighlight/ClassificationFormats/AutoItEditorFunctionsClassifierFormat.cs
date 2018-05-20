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
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace AutoItSyntaxHighlight.ClassificationFormats
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "AutoItEditorFunctionsClassifier")]
    [Name("AutoItEditorFunctionsClassifier")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class AutoItEditorFunctionsClassifierFormat : ClassificationFormatDefinition
    {
        public AutoItEditorFunctionsClassifierFormat()
        {
            this.DisplayName = "AutoIt functions";
            var color = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundBrushKey);
            if (color.R == 37 && color.G == 37 && color.B == 38)
            {
                this.ForegroundColor = Color.FromRgb(78, 201, 176); // Dark Theme
            }
            else
            {
                this.ForegroundColor = Color.FromRgb(42, 143, 186); // Other Themes
            }
        }
    }
}
