﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Caliburn.Micro.Navigation
{
    public static class NavigationConvention
    {
        public static void AddNavigationConventions()
        {
            var conv = ConventionManager.AddElementConvention<Frame>(ContentControl.ContentProperty, "DataContext", "Loaded");
            conv.GetBindableProperty =
                delegate(DependencyObject foundControl)
                {
                    var element = (ContentControl)foundControl;
                    return element.ContentTemplate == null && !(element.Content is DependencyObject)
                        ? NavigationView.ModelProperty
                        : ContentControl.ContentProperty;
                };

            conv.ApplyBinding = (viewModelType, path, property, element, convention) =>
            {
                if (!ConventionManager.SetBindingWithoutBindingOverwrite(viewModelType, path, property, element, convention, convention.GetBindableProperty((DependencyObject)element)))
                    return false;

                var frame = element as Frame;
                if (!typeof(NavigationConductor).IsAssignableFrom(viewModelType) || frame == null)
                    return true;

                var binding = new Binding("FrameSource") { Mode = BindingMode.TwoWay };
                BindingOperations.SetBinding(frame, Frame.SourceProperty, binding);
                var calLoader = frame.ContentLoader as CaliburnContentLoader;
                if (calLoader == null) return true;

                var contentLoaderBinder = new Binding("DataContext") { Source = frame, Mode = BindingMode.OneWay };
                BindingOperations.SetBinding(calLoader, CaliburnContentLoader.NavigationConductorProperty,
                                             contentLoaderBinder);
                return true;
            };
        }
    }
}
