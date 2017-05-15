using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Movie> MoviesList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MoviesList = new ObservableCollection<Movie>();
            MoviesListView.ItemsSource = MoviesList;

            this.DataContext = MoviesList;
            
        }

        private void MenuItem_Import(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Export(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You won't find any help here.", "Help");
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                MoviesList.Add(new Movie()
                {
                    Title = "Rubber",
                    Director = "Quentin Dupieux",
                    Rating = Rating.Awesome,
                    Type = Type.Horror
                });
            }
        }

        private void MenuItem_DeleteAll(object sender, RoutedEventArgs e)
        {
            MoviesList.Clear();
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MoviesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var i in FindVisualChildren<ListBoxItem>(MoviesListView))
            {
                if (i.IsSelected == true)
                {
                    foreach (var x in FindVisualChildren<TextBlock>(i))
                    {
                        if (x.Name.ToString() == "rating") x.Visibility = Visibility.Visible;
                        if (x.Name.ToString() == "type") x.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    foreach (var x in FindVisualChildren<TextBlock>(i))
                    {
                        if (x.Name.ToString() == "rating") x.Visibility = Visibility.Collapsed;
                        if (x.Name.ToString() == "type") x.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T) yield return (T)child;
                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
