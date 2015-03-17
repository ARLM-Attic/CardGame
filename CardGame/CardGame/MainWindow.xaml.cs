using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CardGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int zIndex = 0;
        Image selectedImage = null;
        List<Image> cards = new List<Image>();
        bool active = false;
        Point LastPoint;
        Rectangle activeCard;
        double borderOffset;
        Random rand = new Random();
        public const int cardWidth = 72, cardHeight = 96;
        public MainWindow()
        {
            InitializeComponent();
            AddCard("DrawPile", new Uri(@".\Images\b1fv.png", UriKind.Relative), 0, 0, deck_MouseLeftButtonDown);

        }

        void AddCard(String cardName, Uri imagePath, double left, double top, MouseButtonEventHandler action = null)
        {
            Image deck = new Image();
            deck.Source = new BitmapImage(imagePath);
            deck.Width = cardWidth;
            deck.Height = cardHeight;
            deck.Name = cardName;
            deck.SetValue(Canvas.LeftProperty, left);
            deck.SetValue(Canvas.TopProperty, top);
            deck.SetValue(Canvas.ZIndexProperty, zIndex++);
            deck.MouseLeftButtonDown += action;
            cards.Add(deck);
            MainCanvas.Children.Add(deck);
        }

        void deck_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.Out.WriteLine("Draw Pile Clicked");
            String name = (rand.Next(51) + 1).ToString();
            AddCard("Card" + name, new Uri(@".\Images\" + name + ".png", UriKind.Relative), 10, 0, card_MouseLeftButtonDown);
        }

        void card_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LastPoint = Mouse.GetPosition(MainCanvas);
            selectedImage = (Image)sender;
            selectedImage.SetValue(Canvas.ZIndexProperty, zIndex++);
            active = true;
            Debug.WriteLine("Card Clicked");

            activeCard = new System.Windows.Shapes.Rectangle();
            activeCard.Stroke = new SolidColorBrush(Colors.DarkRed);
            activeCard.StrokeThickness = 2.0;
            activeCard.Fill = Brushes.Transparent;

            borderOffset = activeCard.StrokeThickness;
            activeCard.Width = selectedImage.ActualWidth + borderOffset * 2;
            activeCard.Height = selectedImage.ActualHeight + borderOffset * 2;
            Canvas.SetLeft(activeCard, Canvas.GetLeft(selectedImage) - borderOffset);
            Canvas.SetTop(activeCard, Canvas.GetTop(selectedImage) - borderOffset);

            activeCard.SetValue(Canvas.ZIndexProperty, zIndex + 1);

            MainCanvas.Children.Add(activeCard);
        }


        private void MainCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (active && selectedImage != null)
            {
                Debug.WriteLine("Mouse Move");
                Debug.WriteLine("Working");
                Point point = Mouse.GetPosition(MainCanvas);
                double deltaX = point.X - LastPoint.X;
                double deltaY = point.Y - LastPoint.Y;
                LastPoint = Mouse.GetPosition(MainCanvas);

                double newX = Canvas.GetLeft(selectedImage);
                newX += deltaX;
                double newY = Canvas.GetTop(selectedImage);
                newY += deltaY;

                Canvas.SetLeft(selectedImage, newX);
                Canvas.SetTop(selectedImage, newY);
                Canvas.SetLeft(activeCard, Canvas.GetLeft(selectedImage) - borderOffset);
                Canvas.SetTop(activeCard, Canvas.GetTop(selectedImage) - borderOffset);
            }

        }


        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MainCanvas.Children.Remove(activeCard);
            selectedImage = null;
            active = false;
            Debug.WriteLine("Mouse Up");
        }




    }
}
