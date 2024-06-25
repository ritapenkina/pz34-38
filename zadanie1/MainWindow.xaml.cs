using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

namespace pz1_penkina
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _path = Environment.CurrentDirectory + @"/Assets/";
        DispatcherTimer gameTimer = new DispatcherTimer();

        List<Rectangle> itemRemover = new List<Rectangle>();
        Random rand = new Random();
        ImageBrush playerImage = new ImageBrush();
        ImageBrush starImage = new ImageBrush();
        Rect playerHitBox;

        int speed = 15;
        int playerspeed = 10;
        int carNum;
        int starCounter = 30;
        int powerModeCounter = 200;

        double score; double i;

        bool moveLeft, moveRight, gameOver, powerMode;

        public MainWindow()
        {
            InitializeComponent();
            myCanvas.Focus();
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            StartGame();

        }
        public void StartGame()
        {

            foreach (var a in itemRemover)
            {
                myCanvas.Children.Remove(a);
            }
            itemRemover.Clear();
            foreach (var a in myCanvas.Children.OfType<Rectangle>())
            {
                itemRemover.Add(a);
            }

            score = 0;
            i = 1;
            speed = 15;
            starCounter = 30;
            powerModeCounter = 200;
            gameOver = false;
            powerMode = false;

            playerImage.ImageSource = new BitmapImage(new Uri(_path + "playerImage.png"));
            player.Fill = playerImage;
            Canvas.SetLeft(player, 237);
            Canvas.SetTop(player, 390);


            starImage.ImageSource = new BitmapImage(new Uri(_path + "star.png"));


            for (int i = 0; i < 5; i++)
            {
                Rectangle roadMark = new Rectangle
                {
                    Height = 100,
                    Width = 20,
                    Fill = Brushes.White,
                    Tag = "roadMark",
                };
                Canvas.SetLeft(roadMark, 237);
                Canvas.SetTop(roadMark, i * 209);
                myCanvas.Children.Add(roadMark);
                ChangeCars(roadMark);
            }


            for (int j = 0; j < 3; j++)
            {
                Rectangle newCar = new Rectangle
                {
                    Height = 80,
                    Width = 50,
                    Tag = "Car",
                };
                myCanvas.Children.Add(newCar);
                ChangeCars(newCar);
            }


            gameTimer.Start();
        }
        private void ChangeCars(Rectangle car)
        {
            carNum = rand.Next(1, 6);
            ImageBrush carImage = new ImageBrush();


            switch (carNum)
            {
                case 1:
                    carImage.ImageSource = new BitmapImage(new Uri(_path + "car1.png", UriKind.Relative));
                    break;
                case 2:
                    carImage.ImageSource = new BitmapImage(new Uri(_path + "car2.png", UriKind.Relative));
                    break;
                case 3:
                    carImage.ImageSource = new BitmapImage(new Uri(_path + "car3.png", UriKind.Relative));
                    break;
                case 4:
                    carImage.ImageSource = new BitmapImage(new Uri(_path + "car4.png", UriKind.Relative));
                    break;
                case 5:
                    carImage.ImageSource = new BitmapImage(new Uri(_path + "car5.png", UriKind.Relative));
                    break;
                case 6:
                    carImage.ImageSource = new BitmapImage(new Uri(_path + "car6.png", UriKind.Relative));
                    break;
            }
            car.Fill = carImage;

            Canvas.SetTop(car, (rand.Next(100, 400) * -1));
            Canvas.SetLeft(car, rand.Next(0, 430));
        }

        private void PowerUp()
        {
            i += .5;

            if (i > 4)
            {
                i = 1;
            }

            switch (i)
            {
                case 1:
                    playerImage.ImageSource = new BitmapImage(new Uri(_path + "powermode1.png", UriKind.Relative));
                    break;
                case 2:
                    playerImage.ImageSource = new BitmapImage(new Uri(_path + "powermode2.png", UriKind.Relative));
                    break;
                case 3:
                    playerImage.ImageSource = new BitmapImage(new Uri(_path + "powermode3.png", UriKind.Relative));
                    break;
                case 4:
                    playerImage.ImageSource = new BitmapImage(new Uri(_path + "powermode4.png", UriKind.Relative));
                    break;
            }

            myCanvas.Background = Brushes.LightCoral;
        }


        private void MakeStar()
        {
            Rectangle newStar = new Rectangle
            {
                Height = 50,
                Width = 50,
                Tag = "star",
                Fill = starImage
            };

            Canvas.SetLeft(newStar, rand.Next(0, 430));
            Canvas.SetTop(newStar, (rand.Next(100, 400) * -1));

            myCanvas.Children.Add(newStar);
        }


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }

            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void OnKeyUP(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }

            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
            if (e.Key == Key.Enter && gameOver == true)
            {
                StartGame();
            }
        }
        private void GameLoop(object sender, EventArgs e)
        {
            score += .05;
            starCounter -= 1;
            scoreText.Content = "Survived " + score.ToString("#.#") + " Seconds";
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerspeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerspeed);
            }

            if (starCounter < 1)
            {
                MakeStar();
                starCounter = rand.Next(600, 900);
            }

            foreach (var x in myCanvas.Children.OfType<Rectangle>())

                if ((string)x.Tag == "roadMarks")
                {

                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);

                    if (Canvas.GetTop(x) > 510)
                    {
                        Canvas.SetTop(x, -152);
                    }

                    if ((string)x.Tag == "Car")
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) + speed);
                        if (Canvas.GetTop(x) > 500)
                        {
                            ChangeCars(x);
                        }
                        Rect carHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                        //тут была ошибка
                        if (playerHitBox.IntersectsWith(carHitBox))
                        {
                            gameTimer.Stop();
                            gameOver = true;
                            scoreText.Content += " Press Enter to play again";
                        }
                    }

                    if ((string)x.Tag == "star")
                    {

                        Canvas.SetTop(x, Canvas.GetTop(x) + 5);
                        // и тут 
                        if (Canvas.GetTop(x) > 400)
                        {
                            itemRemover.Add(x);
                        }

                        Rect starHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                        if (playerHitBox.IntersectsWith(starHitBox))
                        {
                            itemRemover.Add(x);
                            powerMode = true;
                            powerModeCounter = 200;
                        }
                    }
                }

            if (powerMode == true)
            {
                powerModeCounter -= 1;
                PowerUp();

                if (powerModeCounter < 1)
                {
                    powerMode = false;
                }
                else
                {
                    myCanvas.Background = Brushes.Gray;
                    playerImage.ImageSource = new BitmapImage(new Uri(_path + "playerImage.png", UriKind.Relative));
                }

                foreach (Rectangle y in itemRemover)
                {
                    myCanvas.Children.Remove(y);
                }

                //if (score >= 10 && score < 20)
                // {
                //     speed = 12;
                //  }
                // if (score >= 20 && score < 30)
                //  {
                //      speed = 14;
                //  }
                //  if (score >= 30 && score < 40)
                //  {
                //       speed = 16;
                // }
                //  if (score >= 40 && score < 50)
                //  {
                //      speed = 18;
                //   }
                //   if (score >= 50 && score < 80)
                //   {
                //       speed = 22;
                //   }
            }
        }
    }
    }

