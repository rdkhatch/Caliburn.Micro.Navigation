using System.Windows;

namespace Caliburn.Micro.Navigation
{
    public class NavigationView
    {
        /// <summary>
        /// A dependency property for attaching a model to the UI.
        /// </summary>
        public static DependencyProperty ModelProperty =
            DependencyProperty.RegisterAttached(
                "Model",
                typeof(object),
                typeof(NavigationView),
                new PropertyMetadata(OnModelChanged)
                );

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="d">The element to attach the model to.</param>
        /// <param name="value">The model.</param>
        public static void SetModel(DependencyObject d, object value)
        {
            d.SetValue(ModelProperty, value);
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <param name="d">The element the model is attached to.</param>
        /// <returns>The model.</returns>
        public static object GetModel(DependencyObject d)
        {
            return d.GetValue(ModelProperty);
        }

        private static void OnModelChanged(DependencyObject targetLocation, DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue == args.NewValue)
                return;

            if (args.NewValue == null) return;
            var context = View.GetContext(targetLocation);
            var view = ViewLocator.LocateForModel(args.NewValue, targetLocation, context);
            ViewModelBinder.Bind(args.NewValue, view, context);
        }
    }
}
