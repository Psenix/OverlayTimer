﻿#pragma checksum "..\..\..\Pages\SelectPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "02590BF48EFFF20326A2D5AA694E8AE28823B870265B9C2A51DF84B477FA39EC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using OverlayTimer;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace OverlayTimer {
    
    
    /// <summary>
    /// SelectPage
    /// </summary>
    public partial class SelectPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 37 "..\..\..\Pages\SelectPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button DoneBtn;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\Pages\SelectPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Options;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Pages\SelectPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBoxItem LoadingItem;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/OverlayTimer;component/pages/selectpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Pages\SelectPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.DoneBtn = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\..\Pages\SelectPage.xaml"
            this.DoneBtn.Click += new System.Windows.RoutedEventHandler(this.DoneBtn_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Options = ((System.Windows.Controls.ComboBox)(target));
            
            #line 39 "..\..\..\Pages\SelectPage.xaml"
            this.Options.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Options_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 39 "..\..\..\Pages\SelectPage.xaml"
            this.Options.GotFocus += new System.Windows.RoutedEventHandler(this.Options_GotFocus);
            
            #line default
            #line hidden
            return;
            case 3:
            this.LoadingItem = ((System.Windows.Controls.ComboBoxItem)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

