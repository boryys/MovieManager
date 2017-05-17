using Microsoft.Win32;
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
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Globalization;

namespace MovieManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Movie> MoviesList { get; set; }
        public ObservableCollection<Movie> SearchList { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            MoviesList = new ObservableCollection<Movie>();
            SearchList = new ObservableCollection<Movie>();

            this.DataContext = MoviesList;
            SearchViewList.ItemsSource = SearchList;

            TitleBox.Text = "type title";
            DirectorBox.Text = "type author";
        }

        private void MenuItem_Import(object sender, RoutedEventArgs e)
        {
            XmlSerializer SerializerObj = new XmlSerializer(MoviesList.GetType());
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML documents (.xml) |*.xml";

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var path = openFileDialog.FileName;
                StreamReader rd = new StreamReader(path);
                ObservableCollection<Movie> list = new ObservableCollection<Movie>();

                list = (ObservableCollection<Movie>)SerializerObj.Deserialize(rd);
                foreach (Movie item in list)
                {
                    MoviesList.Add(item);
                }
            }
        }

        private void MenuItem_Export(object sender, RoutedEventArgs e)
        {
            XmlSerializer SerializerObj = new XmlSerializer(MoviesList.GetType());
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.Filter = "XML documents (.xml) |*.xml";

            Nullable<bool> result = saveFileDialog.ShowDialog();
            if (result == true)
            {
                var path = saveFileDialog.FileName;
                StreamWriter wr = new StreamWriter(path);
                SerializerObj.Serialize(wr, MoviesList);
            }
        }

        private void MenuItem_Help(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You won't find any help here.", "Help");
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 100000; i++)
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
            SearchList.Clear();
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

        private void TitleCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TitleBox.Text = "type title";
        }

        private void DirectorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            DirectorBox.Text = "type director";
        }

        private void RatingCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RatingBox.Text = Rating.Terrible.ToString();
        }

        private void RatingCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RatingBox.Text = "";
        }

        private void TypeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TypeBox.Text = Type.Thriller.ToString();
        }

        private void TypeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TypeBox.Text = "";
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (!TitleBox.IsEnabled && !DirectorBox.IsEnabled && !RatingBox.IsEnabled && !TypeBox.IsEnabled)
            {
                MessageBox.Show("You must specify some criteria.", "Error");
            }
            else
            {
                SearchList.Clear();

                Movie temp = new Movie();

                //1
                if (TitleBox.IsEnabled && !DirectorBox.IsEnabled && !RatingBox.IsEnabled && !TypeBox.IsEnabled)
                {
                    foreach(var item in MoviesList)
                    {
                        if(item.Title.Equals(TitleBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //2
                if (!TitleBox.IsEnabled && DirectorBox.IsEnabled && !RatingBox.IsEnabled && !TypeBox.IsEnabled)
                {
                    foreach (var item in MoviesList)
                    {
                        if (item.Director.Equals(DirectorBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //3
                if (!TitleBox.IsEnabled && !DirectorBox.IsEnabled && RatingBox.IsEnabled && !TypeBox.IsEnabled)
                {
                    string ratingsearch;

                    foreach (var item in MoviesList)
                    {
                        ratingsearch = item.Rating.ToString();

                        if (ratingsearch.Equals(RatingBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //4
                if (!TitleBox.IsEnabled && !DirectorBox.IsEnabled && !RatingBox.IsEnabled && TypeBox.IsEnabled)
                {
                    string typesearch;

                    foreach (var item in MoviesList)
                    {
                        typesearch = item.Type.ToString();

                        if (typesearch.Equals(TypeBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //5
                if (TitleBox.IsEnabled && DirectorBox.IsEnabled && !RatingBox.IsEnabled && !TypeBox.IsEnabled)
                {
                    foreach (var item in MoviesList)
                    {
                        if (item.Title.Equals(TitleBox.Text) && item.Director.Equals(DirectorBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //6
                if (TitleBox.IsEnabled && !DirectorBox.IsEnabled && RatingBox.IsEnabled && !TypeBox.IsEnabled)
                {
                    string ratingsearch;

                    foreach (var item in MoviesList)
                    {
                        ratingsearch = item.Rating.ToString();

                        if (item.Title.Equals(TitleBox.Text) && ratingsearch.Equals(RatingBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //7
                if (TitleBox.IsEnabled && !DirectorBox.IsEnabled && !RatingBox.IsEnabled && TypeBox.IsEnabled)
                {
                    string typesearch;

                    foreach (var item in MoviesList)
                    {
                        typesearch = item.Type.ToString();

                        if (item.Title.Equals(TitleBox.Text) && typesearch.Equals(TypeBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //8
                if (!TitleBox.IsEnabled && DirectorBox.IsEnabled && RatingBox.IsEnabled && !TypeBox.IsEnabled)
                {
                    string ratingsearch;

                    foreach (var item in MoviesList)
                    {
                        ratingsearch = item.Rating.ToString();

                        if (item.Director.Equals(DirectorBox.Text) && ratingsearch.Equals(RatingBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //9
                if (!TitleBox.IsEnabled && DirectorBox.IsEnabled && !RatingBox.IsEnabled && TypeBox.IsEnabled)
                {
                    string typesearch;

                    foreach (var item in MoviesList)
                    {
                        typesearch = item.Type.ToString();

                        if (item.Director.Equals(DirectorBox.Text) && typesearch.Equals(TypeBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //10
                if (!TitleBox.IsEnabled && !DirectorBox.IsEnabled && RatingBox.IsEnabled && TypeBox.IsEnabled)
                {
                    string ratingsearch;
                    string typesearch;

                    foreach (var item in MoviesList)
                    {
                        ratingsearch = item.Rating.ToString();
                        typesearch = item.Type.ToString();

                        if (ratingsearch.Equals(RatingBox.Text) && typesearch.Equals(TypeBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //11
                if (TitleBox.IsEnabled && DirectorBox.IsEnabled && RatingBox.IsEnabled && !TypeBox.IsEnabled)
                {
                    string ratingsearch;

                    foreach (var item in MoviesList)
                    {
                        ratingsearch = item.Rating.ToString();

                        if (item.Title.Equals(TitleBox.Text) && item.Director.Equals(DirectorBox.Text) && ratingsearch.Equals(RatingBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //12
                if (TitleBox.IsEnabled && DirectorBox.IsEnabled && !RatingBox.IsEnabled && TypeBox.IsEnabled)
                {
                    string typesearch;

                    foreach (var item in MoviesList)
                    {
                        typesearch = item.Type.ToString();

                        if (item.Title.Equals(TitleBox.Text) && item.Director.Equals(DirectorBox.Text) && typesearch.Equals(TypeBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //13
                if (TitleBox.IsEnabled && !DirectorBox.IsEnabled && RatingBox.IsEnabled && TypeBox.IsEnabled)
                {
                    string ratingsearch;
                    string typesearch;

                    foreach (var item in MoviesList)
                    {
                        ratingsearch = item.Rating.ToString();
                        typesearch = item.Type.ToString();

                        if (item.Title.Equals(TitleBox.Text) && ratingsearch.Equals(RatingBox.Text) && typesearch.Equals(TypeBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //14
                if (!TitleBox.IsEnabled && DirectorBox.IsEnabled && RatingBox.IsEnabled && TypeBox.IsEnabled)
                {
                    string ratingsearch;
                    string typesearch;

                    foreach (var item in MoviesList)
                    {
                        ratingsearch = item.Rating.ToString();
                        typesearch = item.Type.ToString();

                        if (item.Director.Equals(DirectorBox.Text) && ratingsearch.Equals(RatingBox.Text) && typesearch.Equals(TypeBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
                //15
                if (TitleBox.IsEnabled && DirectorBox.IsEnabled && RatingBox.IsEnabled && TypeBox.IsEnabled)
                {
                    string ratingsearch;
                    string typesearch;

                    foreach (var item in MoviesList)
                    {
                        ratingsearch = item.Rating.ToString();
                        typesearch = item.Type.ToString();

                        if (item.Title.Equals(TitleBox.Text) && item.Director.Equals(DirectorBox.Text) && ratingsearch.Equals(RatingBox.Text) && typesearch.Equals(TypeBox.Text))
                        {
                            temp = item;
                            SearchList.Add(item);
                        }
                    }
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in SearchList)
            {
                MoviesList.Remove(item);
            }

            SearchList.Clear();
        }
    }

    public class EmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Please enter some text");
            }
            else
            {
                if (value.ToString().Length == 0)
                {
                    return new ValidationResult(false, "Please enter some text");
                }
            }
            return ValidationResult.ValidResult;
        }
    }
}
