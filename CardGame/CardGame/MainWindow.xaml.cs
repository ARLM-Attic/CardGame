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
        public MainWindow()
        {
            InitializeComponent();

            Image mikeImage = new Image();
            mikeImage.Source = new BitmapImage(new Uri(@".\Capture.PNG", UriKind.Relative));
            mikeImage.Width = 50;
            mikeImage.Height = 50;
            mikeImage.Name = "MikeCard";
            mikeImage.SetValue(Canvas.LeftProperty, (double)50);
            mikeImage.SetValue(Canvas.TopProperty, (double)50);
            mikeImage.SetValue(Canvas.ZIndexProperty, zIndex++);
            cards.Add(mikeImage);

            Image mikeImage2 = new Image();
            mikeImage2.Source = new BitmapImage(new Uri(@".\Capture.PNG", UriKind.Relative));
            mikeImage2.Width = 50;
            mikeImage2.Height = 50;
            mikeImage2.Name = "MikeCard";
            mikeImage2.SetValue(Canvas.LeftProperty, (double)150);
            mikeImage2.SetValue(Canvas.TopProperty, (double)150);
            mikeImage.SetValue(Canvas.ZIndexProperty, zIndex++);
            cards.Add(mikeImage2);


            MainCanvas.Children.Add(mikeImage);
            MainCanvas.Children.Add(mikeImage2);
        }


        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LastPoint = Mouse.GetPosition(MainCanvas);
            foreach (Image i in cards)
            {
                double xStart = (double)i.GetValue(Canvas.LeftProperty);
                double xStop = xStart + i.Width;
                double yStart = Canvas.GetTop(i);
                double yStop = yStart + i.Height;
                Debug.WriteLine("xStart " + xStart);
                Debug.WriteLine("xStop " + xStop);
                Debug.WriteLine("yStart " + yStart);
                Debug.WriteLine("yStop " + yStop);
                if ((LastPoint.X >= xStart && LastPoint.X <= xStop) && (LastPoint.Y >= yStart && LastPoint.Y <= yStop))
                {
                    if (selectedImage != null)
                    {
                        if ((int)i.GetValue(Canvas.ZIndexProperty) > (int)selectedImage.GetValue(Canvas.ZIndexProperty))
                        {
                            selectedImage = i;
                        }
                    }
                    else
                    {
                        selectedImage = i;
                    }
                }

            }
            selectedImage.SetValue(Canvas.ZIndexProperty, zIndex++);
            active = true;
            Debug.WriteLine("Mouse Down");
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
            }

        }


        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectedImage = null;
            active = false;
            Debug.WriteLine("Mouse Up");
        }



    }
}
